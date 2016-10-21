$(function () {
    //获取SkuId和数量
    function GetskuArray() {
        var skuArray = new Array();
        var param;
        var skuElem = $(".ck_SkuId");
        if (skuElem != null && skuElem.length > 0) {
            skuElem.each(function (index, rowElem) {
                var elem = $(rowElem);
                var elemTr = elem.closest("tr");
                param = new Object();

                param.SkuId = elem.attr("id");
                param.Qty = Number(elemTr.find("td:eq(3)").find("input[name=txtNum]").val());

                skuArray.push(param);
            });
        }
        return skuArray;
    }
    var getArry = GetskuArray();
    var skuStr = window.JSON.stringify(getArry);

    $("#ctl00_contentHolder_hiddenSkus").val(skuStr);
})

    function ShowAddDiv() {
        DialogFrameClose("product/SearchCombinationProduct.aspx", "添加组合商品", null, null);
    }

    function Remove(jq) {
        $(jq).parent().parent().remove();
        checkAllprice();
        CalWeight();
    }

    function CollectInfos() {
        var inputstr = '';
        var flag = true;
        if (selectCombinationValue == "2") {
            var trs = $("tr[name='appendlist']");
            if (trs.length == 0) {
                alert("请选择组合商品");
                flag = false;
            }
            else {
                trs.each(function (i, item) {
                    var num = $(item).children().eq(3).find("input").val();
                    if (num == "" || num == 'undefined') {
                        alert("组合商品数量只能为正整数!");
                        flag = false;
                        $(item).children().eq(3).find("input").focus();
                        return false;
                    }
                    if (isNaN(num) || parseInt(num) <= 0) {
                        alert("组合商品数量只能为正整数!");
                        flag = false;
                        $(item).children().eq(3).find("input").focus();
                        return false;
                    }
                    var reg = /^\d+(\.\d{1,2})?$/;
                    var curValue = $(item).children().eq(2).find("input").val();
                    if (curValue == "" || curValue == 'undefined') {
                        alert("商品价格输入不合法!");
                        flag = false;
                        $(item).children().eq(2).find("input").focus();
                        return false;
                    }
                    if (!reg.test(curValue)) {
                        alert("商品价格输入不合法!");
                        flag = false;
                        $(item).children().eq(2).find("input").focus();
                        return false;
                    }
                    $(item).children().eq(3).find("input").val(parseInt(num));
                
                    inputstr += $(item).children().eq(4).html() + "|" + parseInt(num) + "|" + curValue + ",";
                });
                inputstr = inputstr.substr(0, inputstr.length - 1);
                $("#selectProductsinfo").val(inputstr);
            }
        }
        return flag;
    }
    function GetTotalPrice() {
        var total = 0;
        var inputNum = $("#addlist input");
        if (inputNum.length > 0) {
            inputNum.each(function(i, item) {
                var tdprice = parseFloat($(item).parent().prev().html()) * parseInt($(item).val());
                total += tdprice;
            });
            $("#addlist input").bind("blur", function() { GetTotalPrice() });
        }
        $("#totalprice").html(Math.round(total * 100) / 100);
    }
    function checkAllprice() {
        var allPrice =0;
        $("tr[name='appendlist']").each(function (i, item) {
            allPrice +=parseInt($(item).children().eq(3).find("input").val()) * (parseFloat($(item).children().eq(2).find("input").val()).toFixed(2));
        });
        $("#ctl00_contentHolder_txtSalePrice").val(allPrice.toFixed(2));
    }
    function CloseFrameWindow() {
        GetTotalPrice();
    }
    function checkFactNum(current)
    {
        var curr = $(current);
        var txtNum = Number(curr.val());
        if (txtNum == 0) {
            txtNum = 1;
            txtNum.val(txtNum);
            alert("数量不能为0!");
            return false;
        }
        var hidNum = curr.next();
        hidNum.val(txtNum);
        checkAllprice();
        CalWeight();
    }

    function CalWeight()
    {
        var weight = 0;//净重
        var grossweight = 0;//毛重
        $("tr[name='appendlist']").each(function (i, item) {
            var $item = $(item).children();
            var num = parseInt($item.eq(3).find("input").val());
            var sweight = parseFloat(parseFloat($($item.eq(3).find("input")[1]).attr("weight")).toFixed(2));
            var sgrossweight = parseFloat(parseFloat($($item.eq(3).find("input")[1]).attr("grossweight")).toFixed(2));
            weight = weight + sweight * num;
            grossweight = grossweight + (sgrossweight == 0 ? sweight * num : sgrossweight * num);
        });
        var vWeight = Number($("#ctl00_contentHolder_txtWeight").val());
        var vGrossWeight = Number($("#ctl00_contentHolder_txtWeight").val());
        $("#ctl00_contentHolder_txtWeight").val(weight);
        $("#ctl00_contentHolder_txtGrossWeight").val(grossweight);
    }


    /*
    
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
    
        //DialogFrame("product/SearchCombinationProduct.aspx", "添加组合商品", 900, null, GetSkusList);
    
    //定义全局变量
    window.skusListvalue = [];
    window.shippingIdInfo = [];
    window.currentSkuList = "";
    GetskuArray();
    GetcurrentSkus();
    
    
    //回调函数
    function GetSkusList() {
    
        var skusHtml = "";
        if (skusListvalue != null && skusListvalue.length > 0) {
    
            for (var i = 0; i < skusListvalue.length; i++) {
                if (skusListvalue[i].ThumbnailUrl40 == null || skusListvalue[i].ThumbnailUrl40 == "") {
                    skusListvalue[i].ThumbnailUrl40 = "/utility/pics/none.gif";
                }
    
                skusHtml += "<tr name='appendlist'>"
                             + "<td>" + skusListvalue[i].ProductName + "</td>"
                             + "<td>" + skusListvalue[i].SkuContent + "</td>"
                             + "<td><input type='text' value='" + skusListvalue[i].SalePrice + "' name='curValue' onblur='checkAllprice()'/></td>"
                             +"<td><input type='text' value='1' name='txtNum' onblur='checkAllprice()' onchange='checkFactNum(this);'/>"
                             + "<input type='hidden' value='1' name='hidNumWeight' weight='" + skusListvalue[i].Weight + "' grossweight='" + skusListvalue[i].GrossWeight + "'/>"
                             + "</td><td style='display:none'>" + skusListvalue[i].SkuId + "|" + skusListvalue[i].SkuContent + "| " + skusListvalue[i].SalePrice + "|" + skusListvalue[i].ProductId + "|" + skusListvalue[i].ProductName + "|" + skusListvalue[i].ThumbnailUrl40 + "|" + skusListvalue[i].Weight + "|" + skusListvalue[i].GrossWeight + "</td>"
                             + "<td><span  id='" + skusListvalue[i].SkuId + "' style='cursor:pointer;color:blue' class='ck_SkuId' onclick='Remove(this)'>删除</span></td>"
                             +"</tr>";
    
                //税费+=商品数量*商品税率*商品金额
                //taxRate = taxRate + 1 * skusListvalue[i].TaxRate * skusListvalue[i].SalePrice;
                ////总金额
                //totalPrice = totalPrice + skusListvalue[i].SalePrice;
            }
            //CacluPrice(skusHtml, taxRate, totalPrice);
    
        } else {
    
            //CacluPrice(skusHtml, taxRate, totalPrice);
        }
    
        GetcurrentSkus();
        //CalculateTotalPrice();
    }*/
