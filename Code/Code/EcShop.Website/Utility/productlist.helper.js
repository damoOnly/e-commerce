$(document).ready(function () {
    $(".addcartButton").bind("click", function () {
        var theme = "disabled";
        if ($(this).hasClass(theme)) {
            return;
        }
       AddProductToCart(this);
    });  //加入购物车
	
	//规格面板 下去
	$(".pd-size .pd-size-tit").bind("click",function(){
		var $pdSize = $(this).closest(".pd-size").hide();
	})
	$(".pd-size .Product_WareFun_bg .SKURowClass input").bind("click", function () {
	    if (!($(this).hasClass('SKUSelectValueClass'))) {
	        $(this).siblings('input').removeClass('SKUSelectValueClass');
	        $(this).addClass('SKUSelectValueClass');
	        $(this).parents('dl:first').siblings('input:hidden').val($(this).attr('valueid'));
	    }
	    //$(this).toggleClass("SKUSelectValueClass");
	})
})
//加入购物车
function AddProductToCart(ele)
{
	var $ele=$(ele);
	if (!ValidateBuyAmount($ele)) {
	   return false;
	}
	var fastskuid=$ele.attr('fastbuy_skuid');//skuid
	var productid=$ele.attr("productid");
	var quantity=$ele.siblings('.num-wrap').find(':text').val();
	quantity= parseInt(quantity);
	if(fastskuid!=''){
		BuyProductToCart(fastskuid,quantity,$ele); 
	}else{//规格参数显示
	    //window.location.href="/product_detail-"+productid+".aspx";//暂时不支持伪静态
	    if ($ele.siblings(".pd-size").is(':hidden')) {
	        $ele.siblings(".pd-size").show();
	        return false;
	    }
	    AddProductToCart2($ele);
	}
}
// 验证数量输入
function ValidateBuyAmount($ele) {
    var buyAmount = $ele.siblings('.num-wrap').find(':text').val();
    var ibuyNum = parseInt(buyAmount);
    if (buyAmount.length == 0 || isNaN(ibuyNum) || ibuyNum <= 0) {
        alert("请先填写购买数量,购买数量必须大于0");
        return false;
    }
    if (buyAmount == "0" || buyAmount.length > 5 || ibuyNum <= 0 || ibuyNum > 99999) {
        alert("填写的购买数量必须大于0小于99999");
        var str = $(buyAmount).val();
        $(buyAmount).val(str.substring(0, 5));
        return false;
    }
    var amountReg = /^[1-9]d*|0$/;
    if (!amountReg.test(buyAmount)) {
        alert("请填写正确的购买数量");
        return false;
    }
    return true;
}
//加入购物车 提示成功 效果
function showTip($ele,quantity){	
	var $successTip = $("#success-tip");
	var num = $ele.siblings(".num-wrap").find("input").val();
	var $cartBtn = $(".jf_ad4 .cart-btn");
	var $cartNum = $("#cart-num");
	var cw = 26;
	var ch = 26;
	var cartNumP = {top: 5, left: 106};
	var postion = $cartBtn.offset();
	postion = {
		left:postion.left+cartNumP.left,
		top:postion.top+cartNumP.top
	}
	var sw = $successTip.width();
	var sh = $successTip.height();
	var width = $ele.width();
	var height = $ele.height();
	var offset = $ele.offset();
	offset = {
		left:(offset.left+width/2)-sw/2,
		top:(offset.top+height/2)-sh/2
	}
	postion = {
		left:(postion.left+cw/2)-sw/2,
		top:(postion.top+ch/2)-sh/2
	}
	if(window.successTimeout){
		window.clearTimeout(window.successTimeout);
	}
	$successTip.html(num).css(offset).show().stop().animate(postion,500,function(){
		window.successTimeout = window.setTimeout(function(){
			$successTip.stop().fadeOut(200,function(){				
				$cartNum.show().html(quantity);
			});
		},200)
	});
}
				
//异步请求加入购物车
function BuyProductToCart(fastskuid,quantity,$ele) {
    $.ajax({
        url: "ShoppingHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: quantity, productSkuId: fastskuid },
        async: false,
        beforeSend: function () {
            $("#divbefore").css('display', 'block');
        },
        complete: function () {
        },
        success: function (resultData) {
            if (resultData.Status == "OK") {
				if(resultData.Quantity){
					//$("#cart-num").show().html(resultData.Quantity);
				}
				setStock($ele, quantity);
				setQuantityInShoppingCart($ele, quantity);
				showTip($ele, resultData.Quantity);

				if (resultData.data != null)
				{
				    _adwq.push(['_setDataType', 'cart']);
				    _adwq.push(['_setCustomer', $("#hiid_AdUserId").val()  //1234567是一个例子，请换成当前登陆用户ID或用户名，未登录情况下传空字符串
				    ]);
				    _adwq.push(['_setItem',
                        resultData.data[0].skuId,            // 请填入商品编号 - 必填项 
                        resultData.data[0].ProductName,            // 请填入商品名称 - 必填项 
                        resultData.data[0].SalePrice,             // 请填入商品金额 - 必填项 
                        1,             // 请填入商品数量 - 必填项 
                        resultData.data[0].CategoryId,          // 请填入商品分类编号 - 必填项 
                        ''           // 请填入商品分类名称 - 必填项 
				    ]);
				    _adwq.push(['_trackTrans']);  // 触发加入购物车数据提交 - 固定值 - 必填项
				}


            } else if (resultData.Status == "0") {
                // 商品已经下架
                //$("#divbefore").css('display', 'none');
                alert("此商品已经不存在(可能被删除或被下架)，暂时不能购买");
            }
            else if (resultData.Status == "1") {
                // 商品库存不足
                //$("#divbefore").css('display', 'none');
                alert("商品库存只有" + resultData.oldQuan + "件，不足 " + quantity + " 件，请修改购买数量");
            }
            else if (resultData.Status == "2") {
                // 规格不存在
                //$("#divbefore").css('display', 'none');
                alert("商品规格获取失败，可能已被管理员删除");
            } else if (resultData.Status == "4")
            {
                alert("您购买的数量已经超过限购数量，请购买其他商品！");
            }
            else {
                // 抛出异常消息
                //$("#divbefore").css('display', 'none');
                alert(resultData.Status);
            }
        }
    });
}

//添加有规格选择商品到购物车
function AddProductToCart2(ele) {
    if (!ValidateBuyAmount(ele)) {
        return false;
    }

    if (!IsallSelected(ele)) {
        alert("请选择商品规格");
        return false;
    }

    var quantity = ele.siblings('.num-wrap').find(':text').val();
    quantity = parseInt(quantity);
    var stock = getStock(ele);
    var skuId = getSelectedSkuId(ele);
    var count_sku = getQuantityInShoppingCart(ele);   
    if (count_sku > 0) {
        if (quantity + count_sku > stock) {
            alert("商品库存不足，您购物车中已存在 " + count_sku + " 件该规格的商品");
            return false;
        }
    } else if (quantity > stock) {
        alert("商品库存不足 " + quantity + " 件，请修改购买数量");
        return false;
    }
    BuyProductToCart(skuId, quantity, ele);//添加到购物车
}

function getQuantityInShoppingCart(ele) {
    return getProductCount(ele, 'productQuantityInShoppingCart_');   
}

function setQuantityInShoppingCart(ele,count) {
    setProductCount(ele, count, 'productQuantityInShoppingCart_', false);
}

function getStock(ele) {
    return getProductCount(ele, 'productSkus_');
}

function setStock(ele, count) {
    setProductCount(ele, count, 'productSkus_', true);
}

function getProductCount(ele, variablePrefix) {
    var items = eval(variablePrefix + ele.attr('productid'));
    var selectedSkuId = getSelectedSkuId(ele);
    if (items == undefined || items.length == 0) {
        return 0;
    }
    var count = 0;
    $.each(items, function (i) {
        var currentItem = items[i];
        if (currentItem.SkuId == selectedSkuId) {
            count = currentItem.Count;
            return false;
        }
    });
    return count;
}

function setProductCount(ele, count, variablePrefix, isStock) {
    var items = eval(variablePrefix + ele.attr('productid'));
    var selectedSkuId = getSelectedSkuId(ele);
    if (items == undefined || items.length == 0) {
        return;
    }
    $.each(items, function (i) {
        var currentItem = items[i];
        if (currentItem.SkuId == selectedSkuId) {
            if (isStock) {
                currentItem.Count -= count;
            } else {
                currentItem.Count += count;
            }
            return false;
        }
    });
}

function getSelectedSkuId(ele) {
    var array = [];
    var productId = ele.attr('productid');
    array.push(productId);
    var skuElements = ele.siblings('.pd-size').find("input[name='skuCountname']");
    $.each(skuElements, function () {
        array.push($(this).val());
    });
    return array.join('_');
}

    function IsallSelected(ele) {
        var allSelected = true;
        var skuCountnames = ele.siblings('.pd-size').find("input[type='hidden'][name='skuCountname']");
        $.each(skuCountnames, function () {
            if ($(this).attr("value").length == 0) {
                allSelected = false;
            }
        });
        return allSelected;
    }
