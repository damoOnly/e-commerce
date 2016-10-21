$(document).ready(function () {
    $("input[name='inputQuantity']").bind("blur", function () { chageQuantity(this); }); //立即购买
    if ($("#isCustomsClearance").val() == "1") {
        $(".IdentityCard").show();
    }
    //身份证号码显示
    $("[name='iDelete']").bind("click", function () {
        var obj = this;
        /*myConfirm('询问', '确定要从购物车里删除该商品吗？', '确认删除', function () {
            deleteCartProduct(obj);
        });*/
        deleteCartProduct(obj);
    }); //立即购买
    $("#selectShippingType").bind("change", function () { chageShippingType() });
    $("#selectCoupon").bind("change", function () { chageCoupon() });
    $("#aSubmmitorder").bind("click", function () { conformunpack(); });
});

function deleteCartProduct(obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "DeleteCartProduct", skuId: $(obj).attr("skuId") },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                setCookie("cn", resultData.Quantity);
                $('#cart-num').html(getCookie("cn"));
                location.href = "ShoppingCart.aspx";
            }
        }
    });
}

function chageQuantity(obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ChageQuantity", skuId: $(obj).attr("skuId"), quantity: parseInt($(obj).val()) },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.href = "ShoppingCart.aspx";
                setCookie("cn", resultData.Quantity);
                $('.cf-count').html(getCookie("cn"));
            }
            else {
                alert_h("最多只可购买" + resultData.Status + "件", function () {
                    location.href = "ShoppingCart.aspx";
                });

            }
        }
    });
}
function chageShippingType() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != undefined && $("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#vSubmmitOrder_hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
}

function chageCoupon() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#vSubmmitOrder_hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
}
function conformunpack()
{
    if (!$("#selectPaymentType").val()) {
        alert_h("请选择支付方式");
        return false;
    }
    var memberIdentityCard = $.trim($("#txtmemberIdentityCard").val());
    if ($("#isCustomsClearance").val() == "1")
    {
        if ($('#txtRealName').val() == '' || memberIdentityCard == "")
        {
            $(function () {
                $.ajax({
                    type: "Get",
                    url: "/Handler/MemberHandler.ashx?action=GetUserInfo",
                    datatype: "json",
                    async: false,
                    success: function (data)
                    {
                        $("#txtmemberIdentityCard").val(data.data[0].IdentityCard);
                        $("#txtRealName").val(data.data[0].RealName);
                    }
                });
            });
        }
        //if ($('#txtRealName').val() == '') {
        //    alert_h("身份证信息未验证成功，请到个人资料里面验证！");
        //    return false;
        //}
        //if (memberIdentityCard == "") {
        //    alert_h("请填写个人资料验证身份证信息！");
          
        //    return false;
        //}
        //if (memberIdentityCard != '') {
        //    var check = /^[1-9]\d{5}((1[89]|20)\d{2})(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}[\dx]$/i.test(memberIdentityCard);
        //    if (!check) {
        //        check = /^[1-9]\d{5}[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}(\d|x|X)$/.test(memberIdentityCard);
        //    }
        //    if (!check) {
        //        alert_h("身份证格式填写错误");
        //        return false;
        //    }
        //}
    }
    // var $item_quantity=$(".item_quantity");
    // var item_quantity=0;
    // $item_quantity.each(function(i,n){
    // item_quantity+=parseInt($(this).html());	
    // });
    // if(item_quantity>40){
    // alert_h("商品数量超过40个,请分多次购买");
    // return false;
    // }
    //var curOrderTotal=parseFloat($.trim($(".product-amount").html().replace("¥","").replace("￥", "")));
    //if (curOrderTotal && typeof (curOrderTotal) != "undefined" && parseInt($(".totalQuantity").html()))
    // {
    // if(curOrderTotal>1000)
    // {
    // myConfirm("抱歉，您已超过海关限额￥1000，请分次购买。 </br>海关规定：</br>① 消费者购买进口商品，以“个人自用，合理数量”为原则，每单最大购买限额为1000元人民币。</br>② 如果订单只含单件不可分割商品，则可以超过1000元限值。</br>"
    // );
    // return false;
    // }
    // }
    if (!$('#selectShipToDate').val()) {
        alert_h("请选择送货上门时间");
        return false;
    }
    var IsCanMergeOrder = $("#htmlIsCanMergeOrder").val() == "1";
    //var Isconformunpack = is_conformunpack();
  
    //判断是否多件商品且税费超过50
    var Isconformunpack = parseInt($(".totalQuantity").html()) > 1 && parseFloat($("#showtax").html().replace("¥", "").replace("￥", "")) > 50;
  /*  $(".ct-price").each(function () {
        var price = $(this).find('span').html();
        var num = $(this).next().html().replace("X", "").trim();
        if (parseFloat(price) > 1000 && num > 1) {
            myConfirm(
               '拆单温馨提示', "亲，本单超过1000元，根据海关规定必须拆单才能报关哦！",
               "立即拆单", function () {
                   merge = 1;
                   submmitorder();
               })
        }
    });*/
    var curOrderTotal = parseFloat($.trim($("#showTotalPrice").html().replace("¥", "").replace("￥", "")));
        if (curOrderTotal > 1000 && parseInt($(".totalQuantity").html()) > 1) {
            myConfirm(
                   '拆单温馨提示', "亲，本单超过1000元，根据海关规定必须拆单才能报关哦！",
                   "立即拆单", function () {
                       unpack = 1;
                       submmitorder();
                   })
        }
    else if (Isconformunpack) {
        if (1 == 2) {
            myConfirm('提示', '系统检测到您最近下的订单可以合并到这个订单，建议合并订单，这样可以一起发货，并且一样免税！', '确认合单', function () {
                merge = 1;
                submmitorder();
            }, "不想合并，继续", function () { submmitorder(); });
        }    
        if (Isconformunpack) {
            myConfirm('拆单温馨提示', '亲，税费超过50不能给您免税哟，拆单即可享受免税优惠！', '立即拆单', function () {
                unpack = 1;
                submmitorder();
            }, "不拆单，继续", function () {
                submmitorder();
            });
        }
    } else {
        submmitorder();
    }
}
function submmitorder() {
    // if (!$("#selectShippingType").val()) {
    // alert_h("请选择配送方式");
    // return false;
    // }
    var params = {
        action: "Submmitorder", shippingType: -1/*不用配送方式*/, paymentType: $("#selectPaymentType").val(), couponCode: $("#selectCoupon").val(), shippingId: $('#selectShipTo').val(),
        productSku: getParam("productSku"), buyAmount: getParam("buyAmount"), from: getParam("from"), shiptoDate: $("#selectShipToDate").val(), remark: $('#remark').val(), identityCard: $.trim($("#txtmemberIdentityCard").val()),/*新加入*/isCustomsClearance: $("#isCustomsClearance").val(), unpack: unpack, merge: merge, txtRealName: $('#txtRealName').val(), storeId: getParam("storeId"), siteId: $('#Hidd_SiteId').val(),
        orderSource: GetOrderSouce(), SelVoucherCode: $("#selectVoucher").val(), VoucherCode: $("#txtVoucherCode").val(), VoucherPwd: $('#txtVoucherPwd').val()
    };
    if (params.from == 'countDown')
        params.countDownId = $('#groupbuyHiddenBox').val();
    else
        params.groupbuyId = $('#groupbuyHiddenBox').val();


    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: params,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                if (resultData.paymentType == "OK") {
                    setCookie("wait", resultData.Quantity);
                    $('.wait-pay-num').html(getCookie("wait"));
                    setCookie("cn", "");
                    location.replace("FinishOrder.aspx?orderId=" + resultData.OrderId);
                }
                else
                    location.replace("TransactionPwd.aspx?orderId=" + resultData.OrderId + "&totalAmount=" + $("#total").text());
            }
            else if (resultData.ErrorMsg) {
                alert_h(resultData.ErrorMsg);
            }
        },error: function (ex)
        {
            alert_h(ex);
           alert_h("网络异常,请稍后再试");
        }
    });
}

function is_conformunpack()//判断是否符合拆单条件
{
    var b = parseInt($(".totalQuantity").html()) > 1 && parseFloat($(".tax").html().replace("¥", "").replace("￥", "")) > 50;
    if (!b) {
        return false;
    }
    var $taxrate = $(".item_taxrate");
    var $item_quantity = $(".item_quantity");
    if (b) {
        var item_taxrate = 0;
        var item_quantity = 0;
        var hastax_count = 0;
        $taxrate.each(function (i, n) {
            item_taxrate = parseFloat($(this).html());
            item_quantity = parseInt($.trim($($item_quantity[i]).html()));
            if (item_quantity == 0) {
                return false;
            }
            if (typeof (b) != "undefined" && b) {
                for (i = 0; i < item_quantity; i++) {
                    if (item_taxrate > 0) {
                        hastax_count++;
                    }
                }
            }
        });
        if (hastax_count > 1) {
            return true;
        }
    }
    return false;
}
function calc_itemquantity() {
    var item_count = 0;
    var $item_quantity = $(".item_quantity");
    $item_quantity.each(function (i, n) {
        item_count += parseInt($(this).html());
    });
    return item_count;
}
function is_weixn() {
    var ua = navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == "micromessenger") {
        return true;
    } else {
        return false;
    }
}

function GetOrderSouce() {
    var iSourceOrder = 1;
    url = document.location.href.toLowerCase();
    if (url.indexOf("/vshop/") > -1) iSourceOrder = 3;
    if (url.indexOf("/wapshop/") > -1) iSourceOrder = 4;
    if (url.indexOf("/appshop/") > -1) iSourceOrder = 6;
    if (url.indexOf("/alioh/") > -1) iSourceOrder = 5;
    return iSourceOrder;
}
