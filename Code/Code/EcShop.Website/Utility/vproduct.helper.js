$(document).ready(function () {
    var theme = "disabled";
    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });

    $("#buyButton").bind("click", function () { BuyProduct(); }); //立即购买
    $("#buyNum").bind("change", function () {
        var theme = "disabled";
        var stock = parseInt($("#spStock").text());
        var $btnSub = $('div[name="spSub"]');
        var $btnAdd = $('div[name="spAdd"]');
        var val = $(this).val();
        if (isNaN(val) || val == "") {
            val = 1;
        }

        val = parseInt(val);
        var cardinalityQ = $("#hidden_BuyCardinality").val();
        if (val < cardinalityQ) {
            val = cardinalityQ;
        }
        if (val < stock) {
            $btnAdd.removeClass(theme);
        } else {
            val = stock;
            $btnAdd.addClass(theme);
        }
        
        $("#buyNum").val(val);
    });
    $("#spAdd").bind("click", function () {
        if ($(this).hasClass(theme)) {
            return;
        }
        var val = $.trim($("#buyNum").val());
        if (isNaN(val) || val == "") {
            val = 1;
        }
        var num = parseInt(val) + 1;
        var stock = $("#spStock").text();
        //$("#buyNum").val(parseInt(val) + 1);

        if (num <= stock) {
            $("#buyNum").val(num);
            if (num > 1) {
                $(this).removeClass(theme);
            }
            if (num >= stock) {
                $(this).addClass(theme);
            }
        }
    });
    $("#spSub").bind("click", function () {
        if ($(this).hasClass(theme)) {
            return;
        }
        var val = $.trim($("#buyNum").val());
        if (isNaN(val) || val == "") {
            val = 1;
        }
        var num = parseInt(val) - 1;
        var cardinalityQ = $("#hidden_BuyCardinality").val();
        var stock = $("#spStock").text();
        if (num >= cardinalityQ)
        {
            $("#buyNum").val(num);
            if (num == cardinalityQ)
            {
                $(this).addClass(theme);
            }
            if (num < stock) {
                $(this).removeClass(theme);
            }
        }
        //if (num > 0) {
        //    $("#buyNum").val(parseInt(val) - 1);
        //} else {
        //    $("#buyNum").val(1);
        //}
    });
    $("#spcloces").bind("click", function () { $("#divshow").hide() });
});

function disableShoppingBtn(disabled) {//禁用(启用)购买和加入购物车按钮
    var btns = $('button[type=shoppingBtn]');
    if (disabled) {
        btns.addClass('disabled');
        $("#buy-tip").show();
    } else {
        btns.removeClass('disabled');
        $("#buy-tip").hide();
    }
}

function SelectSkus(clt) {
    //禁用购买和加入购物车按钮
    disableShoppingBtn(true);

    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var skuValues = [];
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            skuValues.push($(this).attr("value").split(':')[1]);
        });
        var skuItems = eval($('#hidden_skus').val());
        var selectedSku;
        if (skuItems != undefined && skuItems != null) {
            for (var j = 0; j < skuItems.length; j++) {
                var item = skuItems[j];
                var found = true;
                for (var i = 0; i < skuValues.length; i++) {
                    if (item.SkuId.indexOf('_' + skuValues[i]) == -1)
                        found = false;
                }
                if (found) {
                    selectedSku = item;
                    break;
                }
            }
        }
        if (selectedSku)
            ResetCurrentSku(selectedSku.SkuId, selectedSku.SKU, selectedSku.Weight, selectedSku.Stock, selectedSku.SalePrice);
        else
            ResetCurrentSku("", "", "", "", "0"); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
        disableShoppingBtn(false);
    }
}

// 是否所有规格都已选
function IsallSelected() {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        if ($(this).val().length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

// 重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId) {
    var pvid = skuSelectId.split("_");

    $.each($("#" + skuRowId + " div"), function () {
        $(this).removeClass('active');
    });

    $("#" + skuSelectId).addClass('active');
}

// 重置SKU
function ResetCurrentSku(skuId, sku, weight, stock, salePrice) {
    $("#hiddenSkuId").val(skuId);
    $("#spSalaPrice").html(salePrice);
    if (!isNaN(parseInt(stock))) {
        $("#spStock").html(stock);
    }
    else {
        $("#spStock").html("0");
        alert_h("该规格的商品没有库存，请选择其它的规格！");
    }
}

// 购买按钮单击事件
function BuyProduct() {
    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        alert_h("请选择规格");
        return false;
    }
    var quantity = parseInt($("#buyNum").val());
    var stock = parseInt($("#spStock").html());
    if (isNaN(stock) || stock == 0) {
        alert_h("该规格的商品没有库存！");
        return false;
    }
    if (quantity > stock) {
        alert_h("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }

    //检查商品限购数量
    var productSKuArr = $("#hiddenSkuId").val();
    $.ajax({
        url: '/Handler/MemberHandler.ashx?action=CheckPurchase',
        type: 'post', dataType: 'json', timeout: 10000,
        data: { productSkuId: productSKuArr, quantity: quantity },
        async: false,

        success: function (resultData) {
            if (resultData.Status == "4") {
                alert_h("您购买的数量已经超过限购数量，请调整数量或选购其他商品！");
            } else
            {
                location.href = "SubmmitOrder.aspx?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hiddenSkuId").val() + "&from=signBuy&storeId=" + getParam("storeId");
            }

        }
    });

   
}

// 验证数量输入
function ValidateBuyAmount()
{
    var buyNum = $("#buyNum");
    var ibuyNum = parseInt($("#buyNum").val());
    if ($(buyNum).val().length == 0 || isNaN(ibuyNum) || ibuyNum <= 0) {
        alert_h("购买数量必须大于0!");
        return false;
    }
    if ($(buyNum).val() == "0" || $(buyNum).val().length > 5 || ibuyNum <= 0 || ibuyNum > 99999) {
        alert_h("购买数量为1-99998!");
        var str = $(buyNum).val();
        $(buyNum).val(str.substring(0, 5));
        return false;
    }
    var amountReg = /^[1-9]d*|0$/;
    if (!amountReg.test($(buyNum).val())) {
        alert_h("请填写正确的购买数量!");
        return false;
    }
    var buyCardinality = parseInt($('#hidden_BuyCardinality').val());
    if (buyCardinality > 1 && ibuyNum < buyCardinality) {
        alert_h("购买数量不能小于购买基数" + buyCardinality);
        return false;
    }
    return true;
}

function AddProductToCart()
{
    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        alert_h("请选择规格");
        return false;
    }
    else {
        var quantity = parseInt($("#buyNum").val());
        var stock = parseInt($("#spStock").html());
        if (quantity > stock) {
            alert_h("商品库存不足 " + quantity + " 件，请修改购买数量!");
            return false;
        }

        BuyProductToCart(); //添加到购物车
    }
}

function BuyProductToCart() {

    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyNum").val()), productSkuId: $("#hiddenSkuId").val(), storeId: getParam("storeId") },
        async: false,
        success: function (resultData)
        {
            if (resultData.Status == "OK")
            {
                setCookie("cn", resultData.Quantity);
                $('#cart-num').html(getCookie("cn"));
                /*var xtarget = $("#addcartButton").offset().left;
                var ytarget = $("#addcartButton").offset().top;
				
                $("#divshow").css("top", "200px");
                $("#divshow").css("left", parseInt(xtarget) + "px");*/
                /*myConfirm('添加成功', '商品已经添加至购物车', '立即结算', function () {
                    location.replace('ShoppingCart.aspx');
                },"继续选购");*/
                //addSystemTip('商品已经加入购物车');
               
                var me = $('#addcartButton');
                var left_0 = me.offset().left+me.width()/2;
                var top_0 = me.offset().top, mt = 80;
                var left_1 = $("#cart-num").offset().left;
                var top_1 = $("#cart-num").offset().top;
                var src = $(".swiper-slide").eq(0).find("img").attr('src');
                var css_0 = {position: 'absolute', left: left_0, top: top_0, width: '20px', height: "20px"};
                var obj = $("<img src='" + src + "'>");
                obj.css(css_0).appendTo('body');
                var x = left_0, y = 0, z = (left_1 - left_0) / 2,t=160;
                var setT = setInterval(function () {
                    y =( mt/ (z * z)).toFixed(2) * (x - left_0 - z) * (x - left_0 - z) + (top_0 - mt);
                    obj.css({ left: x, top: y }); x += (left_1 - left_0) / t;
                    if (x > left_1) {
                        clearInterval(setT);
                        alert_h('商品已经加入购物车');
                        obj.remove();}},1);
                       //显示添加购物成功
            }
            else if (resultData.Status == "Error")
            {
                alert_h(resultData.ErrorMsg);
                //addSystemTip(resultData.ErrorMsg);
            } else if (resultData.Status == "purchase")
            {
                alert_h("此商品您已经超过限购数量，请调整数量或选购其他商品！");
            }
            else
            {
                // 商品已经下架
                alert_h("此商品已经不存在(可能被删除或被下架)，暂时不能购买" + resultData.Status);
                //addSystemTip("此商品已经不存在(可能被删除或被下架)，暂时不能购买" + resultData.Status);
            }
        }
    });
}

//店铺收藏
function SupplierFav(supplierId,$ele) {
	supplierId = supplierId || getParam("SupplierId");
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddSupplierFav", SupplierId: supplierId },
        async: false,
        success: function (resultData) {
            if (resultData.success == true) {                
				addSystemTip("收藏成功");
				$ele.addClass("supp-fav-like");
				var count = parseInt($ele.text());
				$ele.text(count+1);
				$ele.off("click").on("click",function(e){
					var sid = supplierId;
					cancelFav(sid,$(this));
					e.preventDefault();
					e.stopPropagation();
				})
            }
            else if (resultData.success == false) {
                addSystemTip(resultData.msg);
            }
        }
    });
}
function cancelFav(id,$ele) {
	var data = {};
	data.SupplierId = id || getParam("SupplierId");
	$.post("/api/VshopProcess.ashx?action=DelCollectSupplier", data, function (json) {
		if (json.success === true) {
			alert_h("取消成功");
			$ele.removeClass("supp-fav-like");
			var count = parseInt($ele.text());
			$ele.text(count-1);
			$ele.off("click").on("click",function(e){
				var sid = $(this).attr("data-id");
				if(!sid) {
					sid = getParam("SupplierId");
				}
				SupplierFav(sid,$(this));
				e.preventDefault();
				e.stopPropagation();
			})
			//location.reload();
		}
		else {
			alert_h(json.msg);
		}
	});
}
