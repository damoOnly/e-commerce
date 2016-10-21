$(document).ready(function () {
    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });

    $("#buyButton").bind("click", function () { BuyProduct(); }); //立即购买
    $("#spAdd").bind("click", function () { $("#buyNum").val(parseInt($("#buyNum").val()) + 1) });
    $("#spSub").bind("click", function () { var num = parseInt($("#buyNum").val()) - 1; if (num > 0) $("#buyNum").val(parseInt($("#buyNum").val()) - 1) });
    $("#spcloces").bind("click", function () { $("#divshow").hide() });
});

function disableShoppingBtn(disabled) {//禁用(启用)购买和加入购物车按钮
    var btns = $('button[type=shoppingBtn]');
    if (disabled)
        btns.addClass('disabled');
    else
        btns.removeClass('disabled');
}

function SelectSkus(clt) {
    disableShoppingBtn(true); //禁用购买和加入购物车按钮

    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var skuValues = [];
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            skuValues.push($(this).attr("value").split(':')[1]);
        });
        var skuItems = eval($('#hidden_skus').val());
        var selectedSku;
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
    $.each($("input[type='hidden'][name='skuCountname']"), function() {
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
    var type = GetAgentType();
    var username = getCookie("Vshop-Member");
    if (username == undefined || username == "") {// 如果web端没登录                
        if (type == 2) {// 安卓
            var sessionid = window.HiCmd.webGetSession();
            if (sessionid != "")
                loadIframeURL("/AppShop/AppLogin.aspx?sessionid=" + window.HiCmd.webGetSession());
            else
                window.HiCmd.webLogin("openLogin");
        }
        else if (type == 1)// ios
            loadIframeURL("hishop://webGetSession/loadSessionid/null");
        return false;
    }


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
        alert_h("该规格的商品没有库存，请选择其它的规格！");
        return false;
    }
    if (quantity > stock) {
        alert_h("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }
    location.href = "/AppShop/SubmmitOrder.aspx?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hiddenSkuId").val() + "&from=signBuy";
}

// 验证数量输入
function ValidateBuyAmount() {
    var buyNum = $("#buyNum");
    if ($(buyNum).val().length == 0) {
        alert_h("请先填写购买数量!");
        return false;
    }
    if ($(buyNum).val() == "0" || $(buyNum).val().length > 5) {
        alert_h("填写的购买数量必须大于0小于99999!", function () {
            var str = $(buyNum).val();
            $(buyNum).val(str.substring(0, 5));
            return false;
        });
     
        
    }
    var amountReg = /^[1-9]d*|0$/;
    if (!amountReg.test($(buyNum).val())) {
        alert_h("请填写正确的购买数量!");
        return false;
    }

    return true;
}

function loadSessionid(sessionid) {
    if (sessionid != "")
        loadIframeURL("/AppShop/AppLogin.aspx?sessionid=" + sessionid);
    else
        loadIframeURL("hishop://webLogin/openLogin/null");
}

function openLogin(ret, sessionId) {
    if (ret == 0)
        loadIframeURL("/AppShop/AppLogin.aspx?sessionid=" + sessionid);
}

function AddProductToCart() {
    var type = GetAgentType();
    var username = getCookie("Vshop-Member");
    if (username == undefined || username == "") {// 如果web端没登录                
        if (type == 2) {// 安卓
            var sessionid = window.HiCmd.webGetSession();
            if (sessionid != "")
                loadIframeURL("/AppShop/AppLogin.aspx?sessionid=" + window.HiCmd.webGetSession());
            else
                window.HiCmd.webLogin("openLogin");
        }
        else if (type == 1)// ios
            loadIframeURL("hishop://webGetSession/loadSessionid/null");
        return false;
    }

    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        alert_h("请选择规格");
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

function BuyProductToCart(){
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyNum").val()), productSkuId: $("#hiddenSkuId").val() },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                var xtarget = $("#addcartButton").offset().left;
                var ytarget = $("#addcartButton").offset().top;

                $("#divshow").css("top", "200px");
                $("#divshow").css("left", parseInt(xtarget) + "px");
                myConfirm('添加成功', '商品已经添加至购物车', '现在去购物车', function () {
                    var type = GetAgentType();
                    // 页面跳转处理
                    if (type == 2)
                        window.HiCmd.webGoHome(2);
                    else if (type == 1)// ios
                        loadIframeURL("hishop://webGoHome/null/2");
                    else
                        location.replace('/AppShop/ShoppingCart.aspx');
                });
                //显示添加购物成功
            } 
            else{
                // 商品已经下架
                alert_h("此商品已经不存在(可能被删除或被下架)，暂时不能购买");
            }
        }
    });
}
