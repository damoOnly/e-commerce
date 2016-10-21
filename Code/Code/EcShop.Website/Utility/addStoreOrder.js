//保存 cookies
function setMyCookie(key, value) {
    if (arguments.length == 1) {
        if (window.localStorage) {
            return localStorage[key];
        } else {
            var objValue = null;
            var arrStr = document.cookie.split("; ");
            for (var i = 0; i < arrStr.length; i++) {
                var temp = arrStr[i].split("=");
                if (temp[0] == key) {
                    objValue = unescape(temp[1]);
                    break;
                }
            }
            return objValue;
        }
    } else {
        if (window.localStorage) {
            if (typeof value != "string") {
                value = JSON.stringify(value);
            }
            localStorage[key] = value;
        } else {
            var str = key + "=" + escape(value);
            var expires = 0;
            if (expires > 0) {//为0时不设定过期时间，浏览器关闭时cookie自动消失
                var date = new Date();
                var ms = expires * 3600 * 1000;
                date.setTime(date.getTime() + ms);
                str += "; expires=" + date.toGMTString();
            }
            document.cookie = str;
        }
    }
}

//定义全局变量
window.skusListvalue = [];
window.shippingIdInfo = [];
window.currentSkuList = "";

//表单判断
function InitValidators() {

    initValid(new InputValidator('ctl00_contentHolder_txtDeductible', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
    initValid(new InputValidator('ctl00_contentHolder_txtShipTo', 2, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '收货人姓名不能为空，只能是汉字或字母开头，长度在2-20个字符之间'))
    initValid(new InputValidator('ctl00_contentHolder_txtDetailsAddress', 3, 60, false, null, '详细地址不能为空,长度限制在3-60个字符之间'))
    //initValid(new InputValidator('ctl00_contentHolder_txtTelPhone', 3, 20, false, null, '电话号码(区号-电话号码-分机),长度限制在3-20个字符之间'))
    initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 0, 11, true, '^(13|15|18|14|17|18)[0-9]{9}$', '手机号码格式不正确'));

    var identityCard = $.trim($("#ctl00_contentHolder_txtIdentityCard").val());
    if (identityCard && identityCard != '') {
        initValid(new InputValidator('ctl00_contentHolder_txtIdentityCard', 1, 18, true, '^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\d|x|X)', '请输入正确身份证号码，长度在1-18个字符以内'));
    }
}

//获取SkuId和购买数量
function GetskuArray() {
    var skuArray = new Array();
    var param;
    var skuElem = $(".ck_SkuId");
    if (skuElem != null && skuElem.length > 0) {
        skuElem.each(function (index, rowElem) {
            var elem = $(rowElem);
            var elemTr = elem.closest("tr");
            param = new Object();

            param.SkuId = elem.val();
            param.BuyQty = Number(elemTr.find("td:eq(4)").find("input[id=buyAmount]").val());

            skuArray.push(param);
        });
    }
    return skuArray;
}

//提交到后台
function doSubmit() {

    $("#ctl00_contentHolder_hiddenSkus").val('');
    // 商品规格数量需大于等于2
    if ($(".ck_SkuId").length < 1) {
        alert("必须选择一个商品规格！");
        return false;
    }

    var getArry = GetskuArray();
    var skuStr = window.JSON.stringify(getArry);

    $("#ctl00_contentHolder_hiddenSkus").val(skuStr);

    // 1.先执行jquery客户端验证检查其他表单项
    if (!PageIsValid()) {
        return false;
    }
    return true;
}

//统计总金额
function TotalPrice() {
    var totalPrice = 0.00;
    $('input[name="ProductPriceTotal"]').each(function (i, elem) {
        totalPrice = totalPrice + parseFloat($(elem).val());
    });

    var taxRate = parseFloat($("#ctl00_contentHolder_lblTotalTax").html() == "" ? 0.00 : $("#ctl00_contentHolder_lblTotalTax").html());
    var debuctble = parseFloat($("#ctl00_contentHolder_txtDeductible").val() == "" ? 0.00 : $("#ctl00_contentHolder_txtDeductible").val());
    var freight = parseFloat($("#ctl00_contentHolder_lblToalFreight").val() == "" ? 0.00 : $("#ctl00_contentHolder_lblToalFreight").val());

    totalPrice = totalPrice - debuctble + freight;

    //如果总税额<= 50，那么免税,大于50要收费
    if (taxRate > 50) {
        totalPrice = totalPrice + taxRate;
    }

    var decut = parseFloat($("#ctl00_contentHolder_txtDeductible").val() == "" ? 0.00 : $("#ctl00_contentHolder_txtDeductible").val());

    totalPrice = totalPrice - decut;

    $("#ctl00_contentHolder_lblProductTotalPrice").html(totalPrice.toFixed(2));
}


//统计总税额
function TotalTaxRate() {
    //税费 += 商品数量 * 商品税率 * 商品金额
    var tax = 0.00;
    $('.ck_SkuId').each(function (i, elem) {
        var elem = $(elem);
        var elemTr = elem.closest("tr");
        var number = Number(elemTr.find("td:eq(4)").find("input[id=buyAmount]").val());
        var rate = parseFloat(elemTr.find("td:eq(2)").find("div[class=taxRate] span").text()) / 100;
        var price = parseFloat(elemTr.find("td:eq(3) span").text());
        tax = tax + (number * rate * price);
    });
    $("#ctl00_contentHolder_lblTotalTax").html(tax.toFixed(2));
}

//计算运费金额
function CalculateTotalPrice() {

    $("#ctl00_contentHolder_lblToalFreight").html(0.00);

    var getArry = GetskuArray();

    var regionId = Number($("#ctl00_contentHolder_txtRegionId").val()) > 0 ? Number($("#ctl00_contentHolder_txtRegionId").val()) : Number($("#ddlRegions3").val());

    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: 'CalcBackAddOrderFreight', SkuList: window.JSON.stringify(getArry), RegionId: regionId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                var price = parseFloat(resultData.Freight);
                $("#ctl00_contentHolder_lblToalFreight").html(price.toFixed(2));
            }
        }
    });

}

function GetcurrentSkus() {
    currentSkuList = "";
    var ckSkuId = $(".ck_SkuId");
    if (ckSkuId != null && ckSkuId.length > 0) {
        $(".ck_SkuId").each(function (i, elem) {
            currentSkuList += $(elem).val() + ",";
        });
    }
    if (currentSkuList != "" && currentSkuList.length > 0) {
        currentSkuList = currentSkuList.substring(0, currentSkuList.length - 1);
    }
}

function CacluPrice(skusHtml, taxRate, totalPrice) {
    $('#grdProductSkus').html(skusHtml);

    setMyCookie("DataCache_SkusList", skusListvalue);

    var tax = parseFloat($("#ctl00_contentHolder_lblTotalTax").html() == "" ? 0.00 : $("#ctl00_contentHolder_lblTotalTax").html());
    var price = parseFloat($("#ctl00_contentHolder_lblProductTotalPrice").html() == "" ? 0.00 : $("#ctl00_contentHolder_lblProductTotalPrice").html());

    //总税费
    var taxRateAll = tax + taxRate;

    //如果总税额<= 50，那么免税,大于50要收税费
    if (taxRateAll > 50) {
        totalPrice = totalPrice + taxRateAll;
    }
    $("#ctl00_contentHolder_lblTotalTax").html((taxRateAll).toFixed(2));
    $("#ctl00_contentHolder_lblProductTotalPrice").html((price + totalPrice).toFixed(2));

}

//回调函数
function GetSkusList() {

    var taxRate = 0.00;
    var totalPrice = 0.00;
    var skusHtml = "";
    if (skusListvalue != null && skusListvalue.length > 0) {

        for (var i = 0; i < skusListvalue.length; i++) {
            if (skusListvalue[i].ThumbnailUrl40 == null || skusListvalue[i].ThumbnailUrl40 == "") {
                skusListvalue[i].ThumbnailUrl40 = "/utility/pics/none.gif";
            }
            skusHtml += "<tr>"
                        + "<td width='30' align='left'><input type='checkbox' value='" + skusListvalue[i].SkuId + "' class='ck_SkuId' checked='checked'/><input type='hidden' value='" + skusListvalue[i].ProductId + "' class='ck_productId'/></td>"
                        + "<td width='80' align='center'><a href='../../ProductDetails.aspx?productId=" + skusListvalue[i].ProductId + "' target='_blank'><img src='" + skusListvalue[i].ThumbnailUrl40 + "' style='border-width:0px;'></a></span></td>"
                        + "<td width='350' align='left'>"
                        + "<div class='productName'><a href='../../ProductDetails.aspx?productId=" + skusListvalue[i].ProductId + "' target='_blank'><span style='display:inline; float:none;'>" + skusListvalue[i].ProductName + "</span></a></div>"
                        + "<div class='skucontent'>" + skusListvalue[i].strAttName + "</div>"
                        + "<div class='sku'>商品编码：<span style='display:inline; float:none;'>" + skusListvalue[i].SKU + "</span></div>"
                        + "<div class='taxRate'>税率：<span style='display:inline; float:none;'>" + skusListvalue[i].TaxRate * 100 + "</span>%</div>"
                        + "<span class='lblStock' style='display:none;'>" + skusListvalue[i].Stock + "</span></td>"
                        + "<td width='150' align='left'>￥ <span style='display:inline; float:none;'>" + skusListvalue[i].SalePrice + "</span></td>"
                        + "<td width='120' align='center'>"
                        + "<div class='num-wrap fix'>"
                        + "<div class='num-con'>"
                        + "<span id='txtBuyAmount' class='Product_input'>"
                        + "<input class='Product_input' id='buyAmount' type='text' value='1' style='width:30px;'/>"
                        + "<input id='oldBuyNumHidden' type='hidden' value='1'/></span></div>"
                        + "<div class='num-bar'><div class='btn-add' name='spAdd'></div><div class='btn-reduce' name='spSub'></div></div></div></td>"
                        + "<td width='150' align='left'><input type='text' readonly='readonly' value='" + skusListvalue[i].SalePrice + "' name='ProductPriceTotal'/></td>"
                        + "<td width='150' align='left'><a  href='javascript:void(0)' class='backAddOrder_del' SkuId='" + skusListvalue[i].SkuId + "'>删除</a></td>"
                        + "<tr/>"

            //税费+=商品数量*商品税率*商品金额
            taxRate = taxRate + 1 * skusListvalue[i].TaxRate * skusListvalue[i].SalePrice;
            //总金额
            totalPrice = totalPrice + skusListvalue[i].SalePrice;
        }
        CacluPrice(skusHtml, taxRate, totalPrice);

    } else {

        CacluPrice(skusHtml, taxRate, totalPrice);
    }

    GetcurrentSkus();
    CalculateTotalPrice();
}


//清除Cookie
function ClearsCookie() {
    setMyCookie("DataCache_SkusList", "");
    skusListvalue = [];
    GetSkusList();
}

$(document).ready(function () {
    $("#ctl00_contentHolder_txtDeductible").bind("change", function () {
        var deductVal = $(this).val();
        var deductible = /^(0|(0+(\.[0-9]{1,2}))|[1-9]\d*(\.\d{1,2})?)$/;
        if (deductible.test(deductVal)) {
            TotalPrice();
        } else {
            alert("数据类型错误，只能输入实数型数值");
            return false;
        }
    });

    $("#ctl00_contentHolder_ddlpayment").attr("disabled", "disabled");

    InitValidators();

    if (skusListvalue == "" || skusListvalue.length == 0) {
        //取值
        var skusStr = "";
        skusStr = setMyCookie("DataCache_SkusList");
        if (skusStr != undefined && skusStr != "" && skusStr.length > 0) {

            skusListvalue = window.JSON.parse(skusStr);
            GetSkusList();
        }
        
        CalculateTotalPrice();
        TotalTaxRate();
        TotalPrice();
        GetcurrentSkus();
    }

    $("#productsSeldect").click(function () {
        DialogFrame("sales/ProductSelect.aspx", "选择商品规格", 900, null, GetSkusList);
    });

    var citiTimeout, areaTimeout;
    function SetRegion(regionId)
    {
        //根据区域regionId获取省市区县地址
        $.ajax({
            url: "/admin/sales/RegionHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetFullRegion", RegionId: regionId },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                   
                    var provinceId = resultData.provinceId;
                    var cityId = resultData.cityId;

                    $("#ddlRegions1").val(provinceId).change();
                    if (citiTimeout) {
                        window.clearTimeout(citiTimeout);
                        window.clearTimeout(areaTimeout);
                    }
                    citiTimeout = setTimeout(function () {
                        $("#ddlRegions2").val(cityId).change();
                        areaTimeout = window.setTimeout(function () {
                            $("#ddlRegions3").val(regionId);
                       },500)
                   }, 500);
                }
                else {

                }
            }
        });
    }


    //设置会员收获地址
    function GetShippingId() {
        if (shippingIdInfo != null && shippingIdInfo.length > 0) {
            var shippingIdObj = JSON.parse(shippingIdInfo);
            if (shippingIdObj != null && shippingIdObj.length > 0) {

                setMyCookie("DataCache_shippingIdInfo", shippingIdObj);

                var regionId = shippingIdObj[0].RegionId;

                SetRegion(regionId);

                $("#ctl00_contentHolder_txtShippingId").val(shippingIdObj[0].ShippingId);
                $("#ctl00_contentHolder_txtUserId").val(shippingIdObj[0].UserId);
                $("#ctl00_contentHolder_txtUserName").val(shippingIdObj[0].UserName);
                $("#ctl00_contentHolder_txtRegionId").val(regionId);
                $("#ctl00_contentHolder_txtShipTo").val(shippingIdObj[0].ShipTo);
                $("#ctl00_contentHolder_txtDetailsAddress").val(shippingIdObj[0].Address);
                $("#ctl00_contentHolder_txtZipcode").val(shippingIdObj[0].Zipcode);
                $("#ctl00_contentHolder_txtCellPhone").val(shippingIdObj[0].CellPhone);
                $("#ctl00_contentHolder_txtIdentityCard").val(shippingIdObj[0].IdentityCard);
            }
        }
        InitValidators();
    }


    $("#memberSelect").click(function () {
        DialogFrame("member/MembersSelect.aspx", "选择收货地址", 900, null, GetShippingId);
    });

    // 验证数量输入
    function ValidateBuyAmount(elemTr) {
        var buyAmount = elemTr.find("td:eq(4)").find("input[id=buyAmount]");
        var ibuyNum = Number(buyAmount.val());
        if ($(buyAmount).val().length == 0 || isNaN(ibuyNum) || ibuyNum < 0) {
            alert("请先填写购买数量,购买数量必须大于0!");
            return false;
        }
        if ($(buyAmount).val() == "0" || $(buyAmount).val().length > 5 || ibuyNum < 0 || ibuyNum > 99999) {
            alert("填写的购买数量必须大于0小于99999!");
            var str = $(buyAmount).val();
            $(buyAmount).val(str.substring(0, 5));
            return false;
        }
        var amountReg = /^[1-9]d*|0$/;
        if (!amountReg.test($(buyAmount).val())) {
            alert("请填写正确的购买数量!");
            return false;
        }
        return true;
    }

    // 购买数量变化以后的处理
    function ChangeBuyAmount(elemTr, productPriceTotal) {
        if (ValidateBuyAmount(elemTr)) {
            var quantity = Number(elemTr.find("td:eq(4)").find("input[id=buyAmount]").val());
            var oldQuantiy = Number(elemTr.find("td:eq(4)").find("input[id=oldBuyNumHidden]").val());
            var totalPrice = productPriceTotal / oldQuantiy * quantity;
            elemTr.find("td:eq(5) input").val(totalPrice.toFixed(2));
            elemTr.find("td:eq(4)").find("input[id=oldBuyNumHidden]").attr("value", quantity);
            CalculateTotalPrice();
            TotalTaxRate();
            TotalPrice();
            
        }
    }

    //委派，必须写父节点
    $("#grdProductSkus").delegate('div[name="spAdd"]', "click", function () {
        var elem = $(this);
        var elemTr = elem.closest("tr");
        var stock = Number(elemTr.find("td:eq(2) span:eq(3)").text());
        var productPriceTotal = parseFloat(elemTr.find("td:eq(5) input").val());
        var number = elemTr.find("td:eq(4)").find("input[id=buyAmount]");
        var num = Number(number.val()) + 1;
        if (num <= stock) {
            number.val(num);
            ChangeBuyAmount(elemTr, productPriceTotal);
        }
    });

    $("#grdProductSkus").delegate('div[name="spSub"]', "click", function () {
        var elem = $(this);
        var elemTr = elem.closest("tr");
        var stock = Number(elemTr.find("td:eq(2) span:eq(3)").text());
        var productPriceTotal = parseFloat(elemTr.find("td:eq(5) input").val());
        var number = elemTr.find("td:eq(4)").find("input[id=buyAmount]");
        var num = Number(number.val()) - 1;
        if (num > 0) {
            number.val(num);
            ChangeBuyAmount(elemTr, productPriceTotal);
        }
    });

    $("#grdProductSkus").delegate('input[id="buyAmount"]', "change", function () {
        var elem = $(this);
        var elemTr = elem.closest("tr");
        var stock = Number(elemTr.find("td:eq(2) span:eq(3)").text());
        var productPriceTotal = parseFloat(elemTr.find("td:eq(5) input").val());
        var number = elemTr.find("td:eq(4)").find("input[id=buyAmount]");
        var num = Number(number.val());
        if (num > 0) {
            number.val(num);
            ChangeBuyAmount(elemTr, productPriceTotal);
        } else {
            alert("必须要大于0");
            number.val(1);
        }
    });
    //刷新金额
    $(document).delegate(".ck_SkuId", "change", function () {
        //至少一个checkbox没选，就去掉全选
        if (!$(this)[0].checked) {
            $("#chkAll").removeAttr("checked");
        }
        //如果下面所有的都选择了，那就全选
        var chk = 0;
        var $eleSkuId = $(".ck_SkuId");
        $eleSkuId.each(function (i, elem) {
            if ($(elem)[0].checked) {
                chk++;
            }
        });
        if ($eleSkuId.length > 0 && $eleSkuId.length == chk) {
            $(":checkbox").attr("checked", "checked");
        }
    });

    $(document).delegate('#chkAll', "change", function () {
        var flag = $(this)[0].checked;
        var $eleSkuId = $(".ck_SkuId");
        $eleSkuId.each(function (i, n) {
            $(n)[0].checked = flag;
        });
    });

    //移除当前tr
    $("#grdProductSkus").delegate(".backAddOrder_del", "click", function () {
        var elem = $(this);
        var skuId = elem.attr("SkuId");
        var yes = confirm('您确认要删除选中的商品吗？');
        if (yes) {

            var skusInfo = "";
            skusInfo = setMyCookie("DataCache_SkusList");
            if (skusInfo != undefined && skusInfo != "" && skusInfo.length > 0) {

                skusListvalue = window.JSON.parse(skusInfo);

                if (skusListvalue.length > 0) {
                    for (var i = 0; i < skusListvalue.length; i++) {
                        if (skusListvalue[i].SkuId == skuId) {
                            skusListvalue.splice(i,1);
                        }
                    }
                    GetSkusList();
                }
            }

            TotalTaxRate();
            CalculateTotalPrice();
            TotalPrice();
            GetcurrentSkus();

        }
    });

    $("#DellAll").click(function () {
        if ($(".ck_SkuId").length < 1) {
            alert("没有要删除的商品规格");
            return false;
        }
        var yes = confirm('您确认要删除选中的商品吗？');
        if (yes) {
            $("#grdProductSkus").empty();

            setMyCookie("DataCache_SkusList", "");
            skusListvalue = [];

            TotalTaxRate();
            CalculateTotalPrice();
            TotalPrice();
            GetcurrentSkus();
        }
    });
});



