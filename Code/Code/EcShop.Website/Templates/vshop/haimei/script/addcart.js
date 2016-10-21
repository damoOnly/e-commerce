// JavaScript Document
var pageNumber=1;
var totalPage = 0;
var size=20;
$(function(){
	//点击 首页、列表的购物车	
	$(document).undelegate('.fast-add.show-skus',"click").delegate('.fast-add.show-skus','click',function(){
		
		var skuId=$(this).attr('fastbuy_skuid');
		if(skuId!=''){
		    FastBuyProductToCart(skuId);
		    
			return;
		}
		
		var productId=$(this).attr("productid");
		$('#control_item_info_img').attr('src',$(this).attr('smallImg'));
		$("#spSalaPrice").text($(this).attr('price'));
		$.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post',  timeout: 10000,
        data: { action: "SkuSelector", productId: productId },
        success: function (resultData) {
			$('.skus-info').html(resultData);
			var price = $();			
			bindEvent();
        }
		});
		$.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post',  timeout: 10000,
        data: { action: "GetAllSkusInfo",dataType: 'json', productId: productId },
        success: function (resultData) {
			skus_json=resultData;
			setSpeData(skus_json);
        }
		});
		var $wrap = $("#sku-wrap");
		$wrap.show().css({opacity:1});			
	})
	//重新 绑定事件
	function bindEvent(){
		$.each($(".SKUValueClass"), function () {
			$(this).unbind("click").bind("click", function () { SelectSkus(this); });
		});
	
		$("#buyButton").unbind("click").bind("click", function () { BuyProduct(); }); //立即购买
		$("#spAdd").unbind("click").bind("click", function () { $("#buyNum").val(parseInt($("#buyNum").val()) + 1) });
		$("#spSub").unbind("click").bind("click", function () { var num = parseInt($("#buyNum").val()) - 1; if (num > 0) $("#buyNum").val(parseInt($("#buyNum").val()) - 1) });
		$("#spcloces").unbind("click").bind("click", function () { $("#divshow").hide() });
	}
	
	//设置规格面板数据
	function setSpeData(data){
		var stock = 0;
		for(var i=0;i<data.length;i++){
			stock += data[i].Stock;
		}
		$("#hidden_skus").val(JSON.stringify(data));
		$("#buyNum").val(1);
		$("#spStock").text(stock);
	}	
	//关闭 规格面板
	function closeSkuWrap(){
		var $wrap = $("#sku-wrap");
		$wrap.css({opacity:0});
		window.setTimeout(function(){
			$wrap.hide();
		},200)
	}	
	//关闭 规格选项面板
	$(document).undelegate('.sku-close',"click").delegate('.sku-close','click',function(){	
		closeSkuWrap();
	})
	//点击  规格选项面板 确定按钮
	$(document).undelegate('#control-ok',"click").delegate('#control-ok','click',function(){
		if (!ValidateBuyAmount()) {
			return false;
		}
		if (!IsallSelected()) {       
			alert_h("请选择规格");
			return false;
		}
		closeSkuWrap();
		AddProductToCart();		
	})

})
function FastBuyProductToCart(skuId) {

    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: 1, productSkuId: skuId,storeId:getParam("storeId") },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
				setCookie("cn",resultData.Quantity);			
				$('#cart-num').html(getCookie("cn"));
                /*var xtarget = $("#addcartButton").offset().left;
                var ytarget = $("#addcartButton").offset().top;
				
                $("#divshow").css("top", "200px");
                $("#divshow").css("left", parseInt(xtarget) + "px");*/
                /*myConfirm('添加成功', '商品已经添加至购物车', '立即结算', function () {
                    location.replace('ShoppingCart.aspx');
                },"继续选购");*/
				alert_h("商品已经添加至购物车");//
				var me = $('a[fastbuy_skuid="' + skuId + '"]');
				var left_0 = me.offset().left;
				var top_0 = me.offset().top;
				var left_1 = $("#cart-num").offset().left;
				var top_1 = $("#cart-num").offset().top;
				var src = me.parents('.gd-box').find(".gd-img img").attr('data-original');
				var css_0 = { position: 'absolute', left: left_0, top: top_0, width: '20px', height: "20px" };
				$("<img src='http://www.haimylife.com" + src + "'>").css(css_0).appendTo('body').animate({ left: left_1, top: top_1 }, 1000, function () { this.remove(); });
				//addSystemTip('商品已经加入购物车');
                //显示添加购物成功
            }
			else if(resultData.Status=="Error"){
				alert_h(resultData.ErrorMsg);
				//addSystemTip(resultData.ErrorMsg);
			} else if (resultData.Status == "purchase") {
			    alert_h("此商品您已经超过限购数量，请调整数量或选购其他商品！");
			}
            else {
                // 商品已经下架
                alert_h("此商品已经不存在(可能被删除或被下架)，暂时不能购买" + resultData.Status);
				//addSystemTip("此商品已经不存在(可能被删除或被下架)，暂时不能购买" + resultData.Status);
            }
        }
    });
}