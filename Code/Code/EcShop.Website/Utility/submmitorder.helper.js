$(document).ready(function () {
	
    // 如果默认选中了一个收货地址
    if ($("#addresslist .ad-ele").length > 0) {
        /*$("#addresslist .list").click(function () {
            $(this).addClass("select").siblings().removeClass("select");
            if ($(this).find("input[type='hidden']") != null && $(this).find("input[type='hidden']") != "" && $(this).find("input[type='hidden']") != "undefined") {
                ResetAddress($(this).find("input[type='hidden']").eq(0).val());
            }
        });*/
		$("input[name=hidShippingId]").change(function(){
			var val = $("input[name=hidShippingId]:checked").val();
			var $ele = $(this).closest(".ad-ele");
			var theme = "selected";
			$ele.addClass(theme).siblings().removeClass(theme);
			ResetAddress(val);
		});
		if($("input[name=hidShippingId]").eq(0) && $("input[name=hidShippingId]").eq(0).length){
			$("input[name=hidShippingId]").eq(0)[0].checked = true;
			$("input[name=hidShippingId]").eq(0).change();
		    $(".regions").replaceWith("");
		    $("#region_" + $("input[name=hidShippingId]").eq(0)[0].value).prepend("<span class ='regions'>【默认地址】</span>");
		}
        $("#user_shippingaddress").css("display", "none");
        /*var firstshippId = $("#addresslist .list:first").addClass("select").find("input[type='hidden']").eq(0).val();
        if (firstshippId != "" && firstshippId != "undefined" && parseInt(firstshippId) > 0) {
            ResetAddress(firstshippId);
        }*/
    } else {
        // $("#btnaddr").hide();
		$('.address_list').hide();
    }
    //计算 下面 订单 是否 为固定在底部 
	var $cartSumery = $(".order-sumery");	
	var theme = "fixed";
	var offset = $cartSumery.offset();
	function sumeryFixed(){	
		if(!offset){
			return;
		}	
		var st = $(window).scrollTop();
		var height = $(window).height();
		if(st >= offset.top /*|| offset.top > height*/){
			$cartSumery.addClass(theme);
		}else{
			$cartSumery.removeClass(theme);
		}
	}		
	$(window).bind("scroll.sumery",function(){
		sumeryFixed();
	}).bind("resize",function(){
		sumeryFixed();
	})
	sumeryFixed();
    // 收获地址列表选择触发事件
    $("input[type='radio'][name='SubmmitOrder$Common_ShippingAddressesRadioButtonList']").bind('click', function () {
        var shippingId = $(this).attr("value");
        ResetAddress(shippingId);
    })
    //地址栏收起 展开
	$("#ex-opadd").click(function(){
		var theme = "col";
		var $adlist = $("#addresslist");
		if($(this).hasClass(theme)){
			$(this).removeClass(theme).html("收起地址");
			$adlist.show();
		}else{
			$(this).addClass(theme).html("展开地址");
			$adlist.hide();
		}
	})
	//优惠券 现金券 收起 展开
	$(".coupon-info li .arrow-icon").bind("click",function(){
		var theme = "arrow";
		var $li = $(this).closest("li");
		var $others = $li.siblings(":not(:last-child)");
		var index = $li.index();
		if(index != 3){
			$others.removeClass(theme);	
		}
		$li.toggleClass(theme);
	})
	
	$("#SubmmitOrder_CmbCoupCode,#SubmmitOrder_CmbVoucherCode").bind("change",function(){
		var val = $(this).val();
		var theme = "cou-disablebtn";
		var attr = "disabled";
		var $btn = $(this).closest(".cou-con").find(".cou-btn");
		if(val != "" && val != "0"){
			$btn.removeClass(theme).removeAttr(attr);

		}else{
		    $btn.addClass(theme).attr(attr, attr);
		    $("#SubmitOrder_CouponName").html("");
		    $("#SubmmitOrder_litCouponAmout").html("0.00");
		    $("#SubmmitOrder_htmlCouponCode").val("");
		    CalculateTotalPrice();
		    $("#btnVoucherByCode").attr({ "disabled": "disabled" });
		    $("#btnVoucher").attr({ "disabled": "disabled" });
		    $("#btnVoucherByCode").attr("class", "cou-disablebtn");
		    $("#btnVoucher").attr("class", "cou-disablebtn");
		}
	});	
	$("#txtvouchercode,#txtvoucherpassword").bind("keyup",function(){
		var val = $(this).val();
		var val2 = $(this).siblings("input").val();
		var theme = "cou-disablebtn";
		var attr = "disabled";
		var $btn = $(this).closest(".cou-con").find(".cou-btn");
		if(val != "" && val2 != ""){
			$btn.removeClass(theme).removeAttr(attr);
		}else{
			$btn.addClass(theme).attr(attr,attr);
		}
	});	
    //3级收货地址选择触发事件,不触发
    // $("#ddlRegions1").bind("change", function () {
        // CalculateFreight($("#ddlRegions1").val());
    // })

    // 配送方式选择触发事件,已经使用新控件
    // $("input[name='shippButton'][type='radio']").bind('click', function () {
        // var regionId = $("#regionSelectorValue").val();
        // var shippmodeId = $(this).attr("value");
        // $("#SubmmitOrder_inputShippingModeId").val(shippmodeId);
        // //CalculateFreight(regionId);     海美生活运费与配送方式没有关系
    // });

    //$("#SubmmitOrder_inputShippingModeId").val($("#drpShipToDate").val());

	//$("#drpShipToDate").bind('change',function(){
	//	var $selShip=$(this).find("option:selected");
	//	 $("#SubmmitOrder_inputShippingModeId").val($selShip.val());
	//});
	
    // 支付方式选择触发事件
    $("input[name='paymentMode'][type='radio']").bind('click', function () {
		var $ele = $(this).closest("li");
		var theme = "selected";
	    $ele.addClass(theme).siblings().removeClass(theme);
        $('#SubmmitOrder_inputPaymentModeId').val($(this).val());
        CalculateTotalPrice();
    }).eq(0).click();
	
	//判断是否显示提示有大于1000金额的提示消息
	$(function(){
		var taxrate=$(".item_taxrate");
		var $item_quantity=$(".item_quantity");
		var item_quantity=0;
		$.each(taxrate,function(i,n){
			item_quantity += parseInt($.trim($($item_quantity[i]).html()));
			
		});
		var num = parseInt($('.cart_Order_total>div').eq(1).text().split('：')[1]*100)/100;
		if(item_quantity>1&&num>1000){
			$('#split-order').show();
		}
		else{
			$('#split-order').hide();
		}
	});

	$('#SubmmitOrder_btnCreateOrder').click(function () {
	    
		if(!CheckOrderInfo()){
			return false;//验证表单
		}
		var isCustomsClearance=$("#SubmmitOrder_isCustomsClearance").val();//限购
		var isOneTemplateId=$("#SubmmitOrder_isOneTemplateId").val();
		var shoppingCartProductQuantity=$("#SubmmitOrder_shoppingCartProductQuantity").val();
		var productTotal=parseFloat($.trim($("#SubmmitOrder_lblTotalPrice").html()));

		if(productTotal!=NaN){
		   if (productTotal > 1000 && shoppingCartProductQuantity > 1) {

				if (confirm("亲，本单超过1000元，根据海关规定必须拆单才能报关哟！")) {
				    $('#SubmmitOrder_isUnpackOrder').val('1');
				} else {
				   
				    return false;
				}
		   }


		   else if (is_conformunpack(shoppingCartProductQuantity))//拆单检测
		   {
		       if (confirm("亲，税费超过50不能给你免税哟，拆单即可享受免税优惠！")) {
		           $('#SubmmitOrder_isUnpackOrder').val('1');
		       } else {
		           $('#SubmmitOrder_isUnpackOrder').val('0');

		       }
		   }
		}

		
		
	});
	//
    $("#SubmmitOrder_chkTax").click(function () {
        if ($("#SubmmitOrder_chkTax").prop("checked") == true) {
            var tax = eval($("#SubmmitOrder_lblTotalPrice").html()) * eval($("#SubmmitOrder_litTaxRate").html()) / 100;
            $("#SubmmitOrder_lblTax").html(tax.toFixed(2));
        }
        else {
            $("#SubmmitOrder_lblTax").html("0.00");
        }

        CalculateTotalPrice();
    });
    // 使用优惠券
    $("#btnCoupon").bind('click', (function () {
        if (location.href.indexOf("groupBuy") > 0 || location.href.indexOf("countDown") > 0) {
            alert("团购或限时抢购不能使用优惠券");
            return false;
        }
        var couponCode = $("#SubmmitOrder_CmbCoupCode option:selected").val();
        var cartTotal = $("#SubmmitOrder_lblTotalPrice").html();
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: "ProcessorUseCoupon", CartTotal: cartTotal, CouponCode: couponCode },
            cache: false,
            success: function (resultData) {
                if (resultData.Status == "OK")
                {
                    $("#SubmitOrder_CouponName").html(resultData.CouponName);
                    $("#SubmmitOrder_litCouponAmout").html("-" + resultData.DiscountValue);
                    $("#SubmmitOrder_htmlCouponCode").val(couponCode);
                    CalculateTotalPrice();
                    $("#btnVoucherByCode").attr({ "disabled": "disabled" });
                    $("#btnVoucher").attr({ "disabled": "disabled" });
                    $("#btnVoucherByCode").attr("class","cou-disablebtn");
                    $("#btnVoucher").attr("class", "cou-disablebtn");
                }
                else {
                    alert("您的优惠券编号无效(可能不在有效期范围内), 或者您的商品金额不够");
                }
            }
        });
    }));

    $("#btnVoucher").bind('click', (function () {
        if (location.href.indexOf("groupBuy") > 0 || location.href.indexOf("countDown") > 0) {
            alert("团购或限时抢购不能使用现金券");
            return false;
        }
        var voucherCode = $("#SubmmitOrder_CmbVoucherCode option:selected").val();
        var cartTotal = $("#SubmmitOrder_lblTotalPrice").html();
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: "ProcessorUseVoucherBySelect", CartTotal: cartTotal, VoucherCode: voucherCode },
            cache: false,
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    // $("#SubmitOrder_VoucherName").html(resultData.VoucherName);
                    $("#SubmmitOrder_litVoucherAmout").html("-" + resultData.DiscountValue);
                    $("#SubmmitOrder_htmlVoucherCode").val(voucherCode);
                    CalculateTotalPrice();
                    $("#btnVoucherByCode").attr({ "disabled": "disabled" });
                    $("#btnCoupon").attr({ "disabled": "disabled" });
                    $("#btnVoucherByCode").attr("class", "cou-disablebtn");
                    $("#btnCoupon").attr("class", "cou-disablebtn");
                }
                else {
                    alert("您的现金券(可能不在有效期范围内), 或者您的商品金额不够");
                }
            }
        });
    }));


    $("#btnVoucherByCode").bind('click', (function () {
        if (location.href.indexOf("groupBuy") > 0 || location.href.indexOf("countDown") > 0) {
            alert("团购或限时抢购不能使用现金券");
            return false;
        }
        var voucherCode = $("#txtvouchercode").val();
        var voucherPassword = $("#txtvoucherpassword").val();
        var cartTotal = $("#SubmmitOrder_lblTotalPrice").html();
        if (voucherCode == "" || voucherPassword == "") {
            alert("请输入现金券号码和密码");
            return false;
        }
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: "ProcessorUseVoucherByCode", CartTotal: cartTotal, VoucherCode: voucherCode, VoucherPassword: voucherPassword },
            cache: false,
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    // $("#SubmitOrder_VoucherName").html(resultData.VoucherName);
                    $("#SubmmitOrder_litVoucherAmout").html("-" + resultData.DiscountValue);
                    $("#SubmmitOrder_htmlVoucherCode").val(voucherCode);
                    CalculateTotalPrice();
                    $("#btnVoucher").attr({ "disabled": "disabled" });
                    $("#btnCoupon").attr({ "disabled": "disabled" });
                    $("#btnVoucher").attr("class", "cou-disablebtn");
                    $("#btnCoupon").attr("class", "cou-disablebtn");
                }
                else {
                    alert("您输入的现金券无效");
                }
            }
        });
    }));

    $("#btnaddaddress").click(function () {
        ClearAddress();
		$("#ad-tit").hide();
		$("#tab_address").show();
		$("#SubmmitOrder_hidShippingId").val(0);
		$("#user_shippingaddress").attr("class", "cart_Order_address2").show();
		

    });
    $("#imgCloseLogin").click(function () {
        $("#user_shippingaddress").hide();
        $("#tab_address").hide();
		$("#ad-tit").show();
		$("#user_shippingaddress").attr("class", "cart_Order_address");
    });



    $("#a_salemode").toggle(function () {
        $("#tab_pasteaddress").css("display", "block");
        $("#user_shippingaddress").show();

        $(this).text("切换到普通模式");


        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#addresslist").hide();
            $("#addresslist .list").removeClass("select");
        }
        $("#btnaddr").hide();

        ClearAddress();

    }, function () {
        $("#user_shippingaddress").attr("class", "cart_Order_address");
        $("#tab_pasteaddress").css("display", "none");
        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#user_shippingaddress").hide();
        }


        $(this).text("切换到代销模式");

        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#addresslist").show();
            $("#addresslist .list").removeClass("select");

        }
    });


    //根据是否有优惠券可用显示提示信息
    if ($("#SubmmitOrder_CmbCoupCode option").length > 1)
    {
        $("#coupcodeTip").css("display", "none");
    }
    else
    {
        $("#coupcodeTip").css("display", "");
    }

});

function ClearAddress() {
    var $submmitOrder_txtShipTo = $("#SubmmitOrder_txtShipTo");
    $submmitOrder_txtShipTo.val('');
    $submmitOrder_txtShipTo.removeClass("txtborder");
    $("#SubmmitOrder_txtShipToTip").removeClass("msgError").html('');

    var $submmitOrder_txtAddress = $("#SubmmitOrder_txtAddress");
    
    $submmitOrder_txtAddress.val('');
    $submmitOrder_txtAddress.removeClass("txtborder");
    $("#SubmmitOrder_txtAddressTip").removeClass("msgError").html('');

    $("#SubmmitOrder_txtZipcode").val('');
    $("#SubmmitOrder_txtCellPhone").val('');

    var $submmitOrder_txtTelPhone = $("#SubmmitOrder_txtTelPhone");
    $submmitOrder_txtTelPhone.val('');
    $submmitOrder_txtTelPhone.removeClass("txtborder");
    $("#SubmmitOrder_txtTelPhoneTip").removeClass("msgError").html('');

    var $submmitOrder_txtCellPhone = $("#SubmmitOrder_txtCellPhone");
    $submmitOrder_txtCellPhone.val('');
    $submmitOrder_txtCellPhone.removeClass("txtborder");
    $("#SubmmitOrder_txtCellPhoneTip").removeClass("msgError").html('');

    var $submmitOrder_txtIdentityCard = $("#SubmmitOrder_txtIdentityCard")
    $submmitOrder_txtIdentityCard.val('');
    $submmitOrder_txtIdentityCard.removeClass("txtborder");
    $("#SubmmitOrder_txtIdentityCardTip").removeClass("msgError").html('');

    $("#ddlRegions1").val(null);
    $("#ddlRegions2").val(null);
    $("#ddlRegions3").val(null);
    $("#regionSelectorValue").val(null);

    
}


function AddShippAddress()
{
    var shipToVal = $("#SubmmitOrder_txtShipTo").val();
    if (shipToVal != "" && shipToVal != null && shipToVal != undefined) {
        if (shipToVal.indexOf('先生') >= 0 || shipToVal.indexOf('小姐') >= 0) {
            alert("收货人名字不能包含‘先生’或者‘小姐’,请填写真实姓名");
            return false;
        }
    }
   // var identityCard = $("#SubmmitOrder_txtIdentityCard").val();
   // if (identityCard != undefined && identityCard != "" && identityCard != null && identityCard == "111111111111111111") {
   //     alert("请输入正确身份证号码，长度为18个字符");
   //     return false;
   // }
    if (!PageIsValid()) {
        //alert("部分信息没有通过检验，请查看页面提示");
        return false;
    }
   
    if ($("#ddlRegions3 option").length > 1) {
        if ($("#ddlRegions3").val() == "" || $("#ddlRegions3").val() == undefined) {
            alert("请选择收货地址区县");
            return false;
        }
    }
    else {
        if ($("#ddlRegions2").val() == "" || $("#ddlRegions2").val() == undefined) {
            alert("请选择收货地址市");
            return false;
        }
    }
    var shippingId = $("#SubmmitOrder_hidShippingId").val();
    if (shippingId == 0) {
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                Action: "AddShippingAddress", ShippingTo: $("#SubmmitOrder_txtShipTo").val().replace(/\s/g, ""),
                ProvinceId: $("#ddlRegions1").val(),
                RegionId: $("#regionSelectorValue").val(),
                CityId: $("#ddlRegions2").val(),
                AddressDetails: $("#SubmmitOrder_txtAddress").val().replace(/\s/g, ""),
                ZipCode: $("#SubmmitOrder_txtZipcode").val().replace(/\s/g, ""),
                TelPhone: $("#SubmmitOrder_txtTelPhone").val().replace(/\s/g, ""),
                CellHphone: $("#SubmmitOrder_txtCellPhone").val().replace(/\s/g, ""),
                IdentityCard:''
            },
            success: function (resultData)
            {
                if (resultData.Status == "OK") {
                    /*var divlist = "<div class=\"list select\">";
                    divlist += "<div class=\"inner\">";
                    divlist += "<div class=\"addr-hd\">";
                    divlist += resultData.Result.RegionId + "（<span class=\"name\">" + resultData.Result.ShipTo + "</span><span>收）</span>";
                    divlist += "</div>";
                    divlist += "<div class=\"addr-bd\" title=\"" + resultData.Result.ShippingAddress + "\">";
                    divlist += "<span class=\"street\">" + resultData.Result.ShippingAddress + "</span><span class=\"phone\">" + resultData.Result.CellPhone + "</span>";
                    divlist += "<span class=\"last\">&nbsp;</span>";
                    divlist += "</div>";
                    divlist += "</div>";
                    divlist += "<em class=\"curmarker\"></em><input type=\"hidden\" class=\"hidShippingId\" value=\"" + resultData.Result.ShippingId + "\"/>";
                    divlist += "</div>";
                    $(".list").removeClass("select");*/
                    var divlist = '';
                    var returnId=resultData.Result.ShippingId;
                    divlist += '<div class="ad-ele fix" id="' + returnId + '">';
                    divlist += '<div class="toolbar"><a onclick="SetDefaultAddress(' + returnId + ')">设为默认地址</a><a onclick="EditAddress(' + returnId + ')">编辑</a><a onclick="DeleteAddress(' + returnId + ')">删除</a></div>';
                    divlist += '<label id="title_'+returnId+'" title="' + resultData.Result.RegionId + resultData.Result.ShippingAddress + ' ">';
                    divlist += '<input type="radio" checked name="hidShippingId" value="' + returnId + '" /><span><span id="region_' + returnId + '">';
                    divlist += resultData.Result.RegionId + '</span><span id="address_'+returnId+'" class="mr5">' + resultData.Result.ShippingAddress + '</span>';
                    divlist += '<span id="shipto_' + returnId + '" class="mr5">' + resultData.Result.ShipTo + '</span><span id="cellphone_'+returnId+'" class="mr5">' + resultData.Result.CellPhone + '</span></span></label>';
                    divlist += '<input type="hidden" class="hidShippingId" value="' + resultData.Result.ShippingId + '" /></div>';


                    $("#user_shippingaddress").hide();
                    //var ConsigneeShippingId=$("#SubmmitOrder_hidShippingId").
                    //ConsigneeShippingId.val(resultData.Result.ShippingId);
                    //ConsigneeShippingId.focus();
                    $('.address_list').show();

                    if ($.trim($('.address_tab').text()) != '') {
                        $(".ad-ele:last").after(divlist);
                    } else {
                        $('.address_tab').append(divlist);
                    }
                    $("#addresslist .list").unbind("click");
                    $("#addresslist .list").bind("click", function () {
                        $(this).addClass("select").siblings().removeClass("select");
                        if ($(this).find("input[type='hidden']") != null && $(this).find("input[type='hidden']") != "" && $(this).find("input[type='hidden']") != "undefined") {
                            ResetAddress($(this).find("input[type='hidden']").eq(0).val());
                        }
                    });
                    ResetAddress(resultData.Result.ShippingId);
                    //CalculateFreight($("#regionSelectorValue").val());//不再执行重置收货地址
                }
                else {
                    alert(resultData.Result);
                }
            }
        });
    }

    else
    {
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                Action: "UpdateShippingAddress", ShippingTo: $("#SubmmitOrder_txtShipTo").val().replace(/\s/g, ""),
                ProvinceId: $("#ddlRegions1").val(),
                RegionId: $("#regionSelectorValue").val(),
                CityId: $("#ddlRegions2").val(),
                AddressDetails: $("#SubmmitOrder_txtAddress").val().replace(/\s/g, ""),
                ZipCode: $("#SubmmitOrder_txtZipcode").val().replace(/\s/g, ""),
                TelPhone: $("#SubmmitOrder_txtTelPhone").val().replace(/\s/g, ""),
                CellHphone: $("#SubmmitOrder_txtCellPhone").val().replace(/\s/g, ""),
                IdentityCard: "",
                ShippingId: shippingId
            },
            success: function (resultData) {
                if (resultData.Status == "OK") {

                    $("#title_" + shippingId).attr("title", resultData.Result.RegionId + resultData.Result.ShippingAddress);
                    $("#region_" + shippingId).text(resultData.Result.RegionId);
                    $("#address_" + shippingId).text(resultData.Result.ShippingAddress);
                    $("#shipto_" + shippingId).text(resultData.Result.ShipTo);
                    $("#cellphone_" + shippingId).text(resultData.Result.CellPhone);

                    $("#user_shippingaddress").hide();
                    $('.address_list').show();
                }

                else {
                    alert(resultData.Result);
                }
            }
        });
    }
}

// 重置收货地址
function ResetAddress(shippingId) {
    var ConsigneeName = $("#SubmmitOrder_txtShipTo");
    var ConsigneeAddress = $("#SubmmitOrder_txtAddress");
    var ConsigneePostCode = $("#SubmmitOrder_txtZipcode");
    var ConsigneeTel = $("#SubmmitOrder_txtTelPhone");
    var ConsigneeHandSet = $("#SubmmitOrder_txtCellPhone");
	var ConsigneeShippingId=$("#SubmmitOrder_hidShippingId");
	var ConsigneeIdentityCard = $("#SubmmitOrder_txtIdentityCard");

    //手动可修改姓名和身份证号码
	var ConsigneeName1 =$("#SubmmitOrder_txtShipTo1");
	var ConsigneeIdentityCard1 =  $("#SubmmitOrder_txtIdentityCard1");

    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: "GetUserShippingAddress", ShippingId: shippingId },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                $(ConsigneeName).val(resultData.ShipTo);
                ConsigneeName.focus();

                $(ConsigneeAddress).val(resultData.Address);
                ConsigneeAddress.focus();

                $(ConsigneePostCode).val(resultData.Zipcode);
                ConsigneePostCode.focus();

                $(ConsigneeTel).val(resultData.TelPhone);
                ConsigneeTel.focus();

                $(ConsigneeHandSet).val(resultData.CellPhone);
                ConsigneeHandSet.focus();
				
				$(ConsigneeShippingId).val(resultData.ShippingId);
				ConsigneeShippingId.focus();
				
				$(ConsigneeIdentityCard).val(resultData.IdentityCard);
				ConsigneeIdentityCard.focus();

                //手动可修改姓名和身份证号码
				//$(ConsigneeName1).val(resultData.ShipTo);
				//$(ConsigneeIdentityCard1).val(resultData.IdentityCard);


                ResetSelectedRegion(resultData.RegionId);
                CalculateFreight(resultData.RegionId);
            }
            else {
                alert("收货地址选择出错，请重试!");
            }
        }
    });
}
// 总金额
function CalculateTotalPrice() {

    var cartTotalPrice = $("#SubmmitOrder_lblTotalPrice").html();
    var shippmodePrice = $("#SubmmitOrder_lblShippModePrice").html();
    var couponPrice = $("#SubmmitOrder_litCouponAmout").html();
    var voucherPrice = $("#SubmmitOrder_litVoucherAmout").html();

    var tax = $("#SubmmitOrder_lblTax").html();
    
    //关税
    var taxRate = parseFloat($("#SubmmitOrder_lblTotalTax").html().replace("¥", ""));
    if (tax == NaN) {
        return false;
    }

    
    var total = eval(cartTotalPrice) + eval(couponPrice) + eval(tax) + eval(voucherPrice);
    if (taxRate > 50) {
        total += taxRate;
    }
    
    if ($("#SubmmitOrder_hlkFeeFreight").html() == "")
        total = total + eval(shippmodePrice);
    // 计算支付手续费
    var paymentModeId = $('#SubmmitOrder_inputPaymentModeId').val();
    if (paymentModeId == "")
        $("#SubmmitOrder_lblOrderTotal").html(total.toFixed(2));
    else {
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: 'ProcessorPaymentMode', ModeId: paymentModeId, CartTotalPrice: total, TotalPrice: cartTotalPrice, Taxes: tax },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    $("#SubmmitOrder_lblPaymentPrice").html(resultData.Charge);
                    var paymentPrice = eval(resultData.Charge)
                    if (paymentPrice > 0) {
                        $("#divPaymentPrice").show();
                        total = total + paymentPrice;
                    }
                    else {
                        $("#divPaymentPrice").hide();
                    }
                    $("#SubmmitOrder_lblOrderTotal").html(total.toFixed(2));
                }
            }
        });
    }
}

// 重新计算运费
function CalculateFreight(regionId) {
    var weight = $("#SubmmitOrder_litAllWeight").html();
    //var shippingModeId = $("#SubmmitOrder_inputShippingModeId").val();
    var productSku = getParam('productSku');
    var buyAmount = getParam('buyAmount');
    var Bundlingid = getParam('Bundlingid');
	var shippingId=$('.list.select .hidShippingId').val();
    //alert(shippingModeId+"____"+weight+"======="+regionId);
    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: 'CalculateFreight',Weight: weight, RegionId: regionId, productSku: productSku, buyAmount: buyAmount, Bundlingid: Bundlingid,shippingId:shippingId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                if ($("#SubmmitOrder_hlkFeeFreight").html() == "") {
                    $("#SubmmitOrder_lblShippModePrice").html(resultData.Price);
                }
                else {
                    $("#SubmmitOrder_lblShippModePrice").html("0.00");
                }
                CalculateTotalPrice();
            }
        }
    });
}    
// 验证订单
function CheckOrderInfo() {

	var str = $("#SubmmitOrder_txtShipTo1").val();
	var reg = new RegExp("[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*");
	if (str == "" || !reg.test(str)) {
		alert("身份验证未验证，完善个人资料后提交！");
		return false;
	}

	if ($("#SubmmitOrder_txtAddress").val() == "") {
		alert("请输入收货人详细地址");
		return false;
	}
	if ($("#SubmmitOrder_txtTelPhone").val() == "" && $("#SubmmitOrder_txtCellPhone").val() == "") {
		alert("请输入电话号码或手机号码");;
		return false;
	}
	// //身份证验证
    //var memberIdentityCard=$.trim($('#SubmmitOrder_txtIdentityCard').val());
    //if ($('#SubmmitOrder_isCustomsClearance').val() == '1') {
    //    if (memberIdentityCard == "") {
    //        alert("有需要清关商品,请填写身份证号码");
    //        return false;
    //    }
    //    if (memberIdentityCard != '') {
    //        var check = /^[1-9]\d{7}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}$/.test(memberIdentityCard);
    //        if (!check) {
    //            check = /^[1-9]\d{5}[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}(\d|x|X)$/.test(memberIdentityCard);
    //        }
    //        if (!check) {
    //            alert("身份证格式填写错误");
    //            return false;
    //        }
    //    }
    //}
	// 验证配送地区选择了没有
	var selectedRegionId = GetSelectedRegionId();
	if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
		alert("请选择您的收货人地址");
		return false;
	}

	if (!PageIsValid()) {
		alert("部分信息没有通过检验，请查看页面提示");
		return false;
	}
	//if ($("#SubmmitOrder_inputShippingModeId").val() == "") {
	//	alert("请选择配送方式");
	//	return false;
	//}
	if ($("#SubmmitOrder_inputPaymentModeId").val() == "") {
		alert("请选择支付方式");
		return false;
	}
	return true;
};
function is_conformunpack(shoppingCartProductQuantity)//判断是否符合拆单条件
{
   


	var taxrate=$(".item_taxrate");
	var $item_quantity=$(".item_quantity");
	var totalTax=parseFloat($("#SubmmitOrder_lblTotalTax").html().replace("¥",""));
	if(totalTax==NaN){
		return false;
	}
	var b = shoppingCartProductQuantity > 1 && totalTax > 50;

	return b;
	//if(!b){
	//	return false;
	//}
	//var item_taxrate=0;
	//var item_quantity=0;
	//var hastax_count=0;
	//$.each(taxrate,function(i,n){
	//	var $ele=$(n);
	//	item_taxrate=parseFloat($ele.val());
	//	item_quantity=parseInt($.trim($($item_quantity[i]).html()));
	//	if(item_quantity==0){
	//		return false;
	//	}		
	//	if(typeof(b)!="undefined"&&b){
	//		for(i=0;i<item_quantity;i++){
	//			if(item_taxrate>0){
	//				hastax_count++;
	//			}				
	//		}
	//	}		
	//});
	//if(hastax_count>1){
	//return true;
	//}
	//return false;
}

function SetDefaultAddress(shippingId)
{
    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: 'SetDefaultShippingAddress', ShippingId: shippingId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                alert("设置成功");
                // 光标高亮到当前地址
                var $ele = $("#" + shippingId).closest(".ad-ele");
                var theme = "selected";
                $ele.addClass(theme).siblings().removeClass(theme);
                $("#" + shippingId).find("input[name=hidShippingId]")[0].checked = true;
                $(".regions").replaceWith("");
                $("#region_" + shippingId).prepend("<span class ='regions'>【默认地址】</span>");
                ResetAddress(shippingId);
            }
            else
            {
                alert(resultData.Result);
            }
        }
    });
}

function DeleteAddress(shippingId)
{
    if ($("#" + shippingId).find("input[type=radio]").eq(0)[0].checked) {
        alert("该地址被选中，无法删除");
        return;
    }
    else {
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: 'DeleteShippingAddress', ShippingId: shippingId },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    $("#" + shippingId).remove();
                }
                else {
                    alert(resultData.Result);
                }
            }
        });
    }
}

function EditAddress(shippingId)
{
    ClearAddress();
    $("#SubmmitOrder_hidShippingId").val(shippingId);
    $("#ad-tit").hide();
    $("#tab_address").show();
    $("#user_shippingaddress").attr("class", "cart_Order_address2").show();
    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: 'GetUserShippingAddress', ShippingId: shippingId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                $("#SubmmitOrder_txtShipTo").val(resultData.ShipTo);
                $("#regionSelectorValue").val(resultData.RegionId);
                $("#SubmmitOrder_txtAddress").val(resultData.Address);
                $("#SubmmitOrder_txtZipcode").val(resultData.Zipcode);
                $("#SubmmitOrder_txtTelPhone").val(resultData.TelPhone);
                $("#SubmmitOrder_txtCellPhone").val(resultData.CellPhone);
                $("#SubmmitOrder_txtIdentityCard").val(resultData.IdentityCard);
                ResetSelectedRegion(resultData.RegionId);


            }
            else {
                alert("该地址不存在");
            }
        }
    });

  
}

 