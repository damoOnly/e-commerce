$(function () {
    $("#btnAdd").bind("click", function () {
        var shippingIdStr = "";
        var shippingIdArr = "[";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
            shippingIdStr += $(rowItem).attr("value") + ",";
            shippingIdArr += $(rowItem).next("#shippingIdStr").html().trim() + ",";
        });

        shippingIdStr = shippingIdStr.substring(0, shippingIdStr.length - 1);

        var shippingArray = null;

        if (shippingIdStr) {
            shippingArray = shippingIdStr.split(',');
            if (shippingArray != null && shippingArray.length > 1) {
                alert("只能选择一个会员收货地址");
                return false;
            }
        }

        //收获地址信息
        shippingIdArr = shippingIdArr.substring(0, shippingIdArr.length - 1) + "]";

        //把收获地址信息传给子窗口页面
        window.top.frammain.shippingIdInfo = shippingIdArr;

        if (shippingIdStr != undefined && shippingIdStr != null && shippingIdStr != "") {
            window.parent.$(".aui_close").click();
        } else {
            alert("请选择一个会员收货地址");
            return false;
        }
    });

    $("input[name='CheckBoxGroup']").bind("change", function () {
        var flag = $(this)[0].checked;
        if (flag) {
            $(this).attr("hasCheck", "true");
            $(".cb_ShippingId").each(function (i, elem) {
                var elem = $(elem);
                if (elem.attr("hasCheck") != "true") {
                    elem[0].checked = false;
                } else {
                    elem.attr("hasCheck", "false");
                }
            });
        }
    });
});