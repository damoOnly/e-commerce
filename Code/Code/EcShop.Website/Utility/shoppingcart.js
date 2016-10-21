$(function () {

    function selectItem(obj) {
        $eleproduct = $(".ck_productId:not(:checked)");
        var pids = '';
        $eleproduct.each(function (i, n) {
            var $eleitem = $(n);
            pids += $eleitem.val() + ',';
        });

        $.ajax({
            url: "API/MasterProcess.ashx",
            type: "post",
            dataType: "json",
            timeout: 10000,
            async: false,
            data: { action: "SelectItem", pids: pids, skuId: $(obj).attr("SkuId"), quantity: parseInt($(obj).val()) },
            success: function (resultData) {
                if (resultData.Success == 1) {
                    // <p>总价（不含运费）：<span class="sumery-price">￥<span><span id="ShoppingCart_lblTotalPrice">1141.50</span></span></span></p>
                    // <p><span class="mr15">商品金额：761.00</span><span class="mr15">商品关税：380.50 </span><span><a id="ShoppingCart_hlkReducedPromotion"> 优惠：0.00</a></span></p>

                    var str = '';
                    str += '总价（不含运费）：<span class="sumery-price">￥<span><span id="ShoppingCart_lblTotalPrice">' + resultData.TotalPrice + '</span></span></span></p>';
                    str += '<p><span class="mr15">商品金额：' + resultData.AmoutPrice + '</span>';
                    if (parseInt(resultData.Tax) > 50) {
                        str += '<span class="mr15">商品关税：' + resultData.Tax + ' </span>';
                    } else {
                        
                        str += '<span class="mr15">商品关税：<span style="text-decoration: line-through;">' + resultData.Tax + '</span> </span><span style="margin-right:20px;"><em style="border-top:4px solid #fff;border-right:4px solid #f00;border-bottom:4px solid #fff;display:inline-block;margin-top:4px;"></em><span style="background:#ff0000;color:#fff;">（订单关税≤50免征）</span></span>';
                    }
                    str += '<span class="mr15">活动优惠：' + resultData.ActivityReduct + ' </span>';
                    str += '<span><a title="' + resultData.ReducedPromotionName + '" id="ShoppingCart_hlkReducedPromotion" href="' + resultData.NavigateUrl + '"> 优惠：' + resultData.ReducedPromotion + '</a></span></p>';
                    $('#priceinfo').html(str);
					if(!obj){
						return;
					}
                    var $submitBtn = obj.closest(".num-option").find("input.cart_update");
                    if ($submitBtn != null) {
                       $submitBtn.click();
                    }
                }
                else if (resultData.Status != "OK") {
                    alert("由于库存不足，最多只可购买" + resultData.Status + "件");
                    obj.val(resultData.Status);
                }
            }
        });


    }


	$(":checkbox").attr("checked","checked");	
	$('#chkAll').click(function(){
		$($(".ck_productId")[0]).change();
	});
	//店铺 勾选 变化
	$("[name=store-check]").change(function () {
		var checked = $(this)[0].checked;
		var $tr = $(this).parents(".store-title").next("tr");
		$tr.find("input.ck_productId").each(function(index, element) {
            element.checked = checked;
        });
		$tr.find("input.ck_productId").eq(0).change();
	});
	$(".ck_productId").change(function () {
		var b=true;
		var $tr = $(this).closest("table").closest("tr");
		var $store = $tr.prev(".store-title");
		var $stProInput = $tr.find(".ck_productId");
		var stChecked = true;
		$stProInput.each(function(index, element) {
            if(!element.checked){
				stChecked = false;
			}
        });
		if($store.length){
			$store.find("input[name=store-check]")[0].checked = stChecked;
		}
		
		var $eleproduct = $(".ck_productId");
		var chk = 0;
		$eleproduct.each(function(i,n){
			var checked=n.checked;
			if(!checked){
				$("#chkAll").removeAttr("checked");
				b=false;
				return;
			}
			if ($(n)[0].checked) {
			    chk++;
			}
		});
		if(b){
		    $("#chkAll").attr("checked", "checked");
		}
		var $ShoppingCart_btnCheckout = $("#ShoppingCart_btnCheckout");
		var $cart_buy_cartsub = $(".cart_buy_cartsub");

		if (chk > 0) {
		    $ShoppingCart_btnCheckout.removeAttr("disabled");
		    $cart_buy_cartsub.css("background", "#e54346");
		} else {
            //变灰
		    $ShoppingCart_btnCheckout.attr("disabled", "disabled");
		    $cart_buy_cartsub.css("background","grey");
		}		
		selectItem();//刷新金额
	});

	$(".cart_txtbuynum").unbind('blur').blur(function () {
	    var elem = $(this);
	    var number = parseInt(elem.val());
	    var yzelem = /^\+?[1-9][0-9]*$/;
	    if (!yzelem.test(number)) {
	        alert("购买数量必须为大于0的正整数");
	        elem.val(1);
	        return false;
	    }
	    selectItem(elem);//刷新金额
	});
});

