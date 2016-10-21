function CheckIsSelect() {
    //从子窗口传过来的值
    var skuIds = window.top.frammain.currentSkuList;

    if (skuIds == "" || skuIds == null || skuIds.length <= 0) {
        return;
    }
    var skuIdStr = skuIds.split(',');
    if (skuIdStr == null || skuIdStr == "" || skuIdStr.length <= 0) {
        return;
    }
    for (var a = 0; a < skuIdStr.length; a++) {
        $("input[type='checkbox'][name='CheckBoxGroup']").each(function (i, elem) {
            var velem = $(elem);
            if (velem.val() == skuIdStr[a]) {
                velem[0].checked = true;
            }
        });
    }
}

//合并两个数组，并去掉重复的项
function ConcatArray(arry, orderItemArray) {
    var newArray = [];
    if (arry == null || arry.length <= 0) {
        newArray = orderItemArray;
    } else {
        if (orderItemArray.length >= arry.length) {
            for (var i = 0; i < orderItemArray.length; i++) {
                for (var j = 0; j < arry.length; j++) {
                    if (orderItemArray[i].SkuId == arry[j].SkuId) {
                        orderItemArray.splice(i, 1);
                        newArray = arry.concat(orderItemArray);
                    }
                }
            }
        } else {
            for (var i = 0; i < arry.length; i++) {
                for (var j = 0; j < orderItemArray.length; j++) {
                    if (arry[i].SkuId == orderItemArray[j].SkuId) {
                        arry.splice(i, 1);
                        newArray = orderItemArray.concat(orderItemArray);
                    }
                }
            }
        }

        newArray = arry.concat(orderItemArray);
    }
    return newArray;
}

$(function () {
    $("#btnAdd").unbind("click").bind("click", function () {
        var skuIdStr = "";
        var arry = window.top.frammain.skusListvalue;

        var orderItemArray = [];
        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
            var item = $(rowItem);

            var elemTr = item.closest("tr");
            skuIdStr += item.attr("value") + ",";
            var obj = new Object();

            obj.SkuId = item.attr("value");
            obj.ProductId = Number(item.next().val());
            obj.ThumbnailUrl40 = elemTr.find("td:eq(2) div[class=productInfo] img").attr("src");
            obj.strAttName = elemTr.find("td:eq(2) div[class=strAttName] span").text().trim();
            obj.ProductName = elemTr.find("td:eq(2) span[class=Name]").text().trim();
            obj.SKU = elemTr.find("td:eq(5)").text().trim();
            obj.TaxRate = parseFloat(elemTr.find("td:eq(7)").text().trim());
            obj.Stock = Number(elemTr.find("td:eq(6)").text().trim());
            obj.SalePrice = parseFloat(elemTr.find("td:eq(8)").text().trim());

            orderItemArray.push(obj);
        });

        var newArray = ConcatArray(arry, orderItemArray)

        if (skuIdStr.length == 0) {
            alert("请选择商品规格");
            return false;
        }

        //商品规格Id
        skuIdStr = skuIdStr.substring(0, skuIdStr.length - 1);

        //把商品规格信息传给子窗口页面
        window.top.frammain.skusListvalue = newArray;
        if (skuIdStr != undefined && skuIdStr != null && skuIdStr != "") {
            window.parent.$(".aui_close").click();
        }
    });

    CheckIsSelect();
});