var isadd = false;

$(document).ready(function () {
    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });
    BindFavotesTag(); //绑定收藏标签

    $("#tagDiv i").live("click", function () { removeTag(this); }); //绑定删除操作

    $("#sptyperemark").bind("click", function () { ShowProductTypeRemark(); });//显示商品类型说明
    $("#buyAmount").bind("change", function () { 
		var theme = "disabled";
		var stock = parseInt($("#productDetails_Stock").text());
		var $btnSub = $('div[name="spSub"]');
		var $btnAdd = $('div[name="spAdd"]');
		var val = $(this).val();
		if(isNaN(val) || val == ""){
		   val = $(this).attr("old-v");
		   if(isNaN(val) || val == ""){
			   val = 1;
		   }
	   	}
		
		val = parseInt(val);
		var cardinalityQ = $("#buyCardinality").val();
		if (val < cardinalityQ) {
		    val = cardinalityQ;
		}
		if(val < stock){
			$btnAdd.removeClass(theme);			
		}else{
			val = stock;
			$btnAdd.addClass(theme);			
		}
		var nVal = $(this).val();
		if (nVal > cardinalityQ && stock != 1) {
		   $btnSub.removeClass(theme);
		}else{
		   $btnSub.addClass(theme);
		}
		$(this).val(val).attr("old-v",val);
		ChangeBuyAmount();
	});
    $("#ProductDetails_nowBuyBtn").bind("click", function () {
        var theme = "disabled";
        if ($(this).hasClass(theme)) {
            $(this).attr("disabled", "disabled");
            return;
        }
        AddCurrentProductToCart();
    });//立即购买
    //$(".buy-btn").bind("click", function () { AddCurrentProductToCart(); });//立即购买
    $('#buyButton').bind("click", function () { AddCurrentProductToCart(); });//立即购买
    $("#addcartButton").bind("click", function () {
        var theme = "disabled";
        if ($(this).hasClass(theme)) {
            $(this).attr("disabled", "disabled");
            alert("商品库存不足 1 件，请修改购买数量!");
            return;
        }
        AddProductToCart();
    });  //加入购物车
    $("#imgCloseLogin").bind("click", function () { $("#loginForBuy").hide(); });
    $("#btnLoginAndBuy").bind("click", function () { LoginAndBuy(); });
    $("#btnAmoBuy").bind("click", function () { AnonymousBuy(); });
    $("#textfieldusername").keydown(function (e) {
        if (e.keyCode == 13) {
            LoginAndBuy();
        }
    });

    $("#textfieldpassword").keydown(function (e) {
        if (e.keyCode == 13) {
            LoginAndBuy();
        }
    });
    if ($("#hiddenProductType").val() == null || $("#hiddenProductType").val() == "undefined") {
        $("#sptyperemark").css("display", "none");
    }

});

function SelectSkus(clt) {
    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    $("#skuContent_" + AttributeId).attr("ValueStr", $(clt).attr("value"));
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var skuValues = [];
    var skuShows = "";
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            skuValues.push($(this).attr("value").split(':')[1]);
            skuShows += $(this).attr("ValueStr") + ",";
        });
        //        selectedOptions = selectedOptions.substring(0, selectedOptions.length - 1);
        skuShows = skuShows.substring(0, skuShows.length - 1);

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
            ResetCurrentSku(selectedSku.SkuId, selectedSku.SKU, selectedSku.Weight, selectedSku.Stock, selectedSku.AlertStock, selectedSku.SalePrice, "已选择：" + skuShows);
        else
            ResetCurrentSku("", "", "", "", "0"); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
        //disableShoppingBtn(false);
        //        $.ajax({
        //            url: "ShoppingHandler.aspx",
        //            type: 'post', dataType: 'json', timeout: 10000,
        //            data: { action: "GetSkuByOptions", productId: $("#hiddenProductId").val(), options: selectedOptions },
        //            success: function(resultData) {
        //                if (resultData.Status == "OK") {
        //                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.AlertStock, resultData.SalePrice, "已选择：" + skuShows);
        //                }
        //                else {
        //                    ResetCurrentSku("", "", "", "", "", "0", "请选择："); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
        //                }
        //            }
        //        });
    }
}

// 是否所有规格都已选
function IsallSelected() {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        if ($(this).attr("value").length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

// 重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId) {
    //判断某规格是否已下架
    var pvid = skuSelectId.split("_");
    $.ajax({
        url: "ShoppingHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "UnUpsellingSku", productId: $("#hiddenProductId").val(), AttributeId: pvid[1], ValueId: pvid[2] },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                $.each($("#productSkuSelector dd input,#productSkuSelector dd img"), function (index, item) {
                    var currentPid = $(this).attr("AttributeId");
                    var currentVid = $(this).attr("ValueId");
                    // 不同属性选择绑定事件
                    var isBind = false;
                    $.each($(resultData.SkuItems), function () {
                        if (currentPid == this.AttributeId && currentVid == this.ValueId) {
                            isBind = true;
                        }
                    });
                    if (isBind) {
                        if ($(item).attr("class") == "SKUUNSelectValueClass") {
                            $(item).attr("class", "SKUValueClass");
                        }
                        $(item).attr("disabled", false);
                    }
                    else {
                        $(item).attr("class", "SKUUNSelectValueClass");
                        $(item).attr("disabled", true);
                    }
                });
                if (resultData.Stock) {
                    var stock = Number(resultData.Stock);
                    $("#productDetails_Stock").html(stock);
                    var theme = "disabled";
                    if (stock == 0) {
                        $("#ProductDetails_nowBuyBtn").addClass(theme);
                        $("#addcartButton").addClass(theme);
                        $("#buyAmount").attr(theme,theme);
                        $("#btn-add").addClass(theme);
                    } else {

                        $("#ProductDetails_nowBuyBtn").removeClass(theme);
                        $("#addcartButton").removeClass(theme);
                        $("#buyAmount").removeAttr(theme, theme);
                        $("#btn-add").removeClass(theme);

                        var imgsrc = $("#ProductDetails_productImg").attr("src");
                        if (imgsrc == undefined || imgsrc == "" || imgsrc == null) {
                            $(".gd-code-wrap").css("display", "none");
                        } else {

                            if (Number($("#productDetails_Stock").text()) == 0) {
                            } else {
                                $(".gd-code-wrap").css("display", "");
                            }
                        }
                    }
                }
            } 
        }
    });

    $.each($("#" + skuRowId + " dd input,#" + skuRowId + " dd img"), function () {
        $(this).attr({ "class": "SKUValueClass" });
    });

    $("#" + skuSelectId).attr({ "class": "SKUSelectValueClass" });
}

// 重置SKU
function ResetCurrentSku(skuId, sku, weight, stock, alertstock, salePrice, options) {
    $("#showSelectSKU").html(options);
    $("#productDetails_sku").html(sku);
    $("#productDetails_sku_v").val(skuId);
    if (stock != "")
        $("#productDetails_Stock").html(stock);
    if (alertstock != "")
        $("#productDetails_AlertStock").val(alertstock);
    if (weight != "")
        weight = weight + " g";
    $("#ProductDetails_litWeight").html(weight);
    $("#SubmitOrder_Weight").html(weight);

    $("#ProductDetails_lblBuyPrice").html(salePrice);

    if (ValidateBuyAmount() && document.URL.toLowerCase().indexOf("groupbuyproduct_detail") == -1 && document.URL.toLowerCase().indexOf("countdownproduct_detail") == -1 && document.URL.toLowerCase().indexOf("groupbuyproductdetails.aspx") == -1 && document.URL.toLowerCase().indexOf("countdownproductsdetails.aspx") == -1) {
        var quantity = parseInt($("#buyAmount").val());
        var totalPrice = eval(salePrice) * quantity;
        if (!isNaN(totalPrice)) {
            $("#productDetails_Total").html(totalPrice.toFixed(2));
            $("#productDetails_Total_v").val(totalPrice);
        }
        else {
            $("#productDetails_Total").html("0");
            $("#productDetails_Total_v").val("0");
        }
    }
}

//添加收藏
//选中标签
function BindFavotesTag() {
    $.ajax({
        url: '/Handler/MemberHandler.ashx?action=BindFavorite',
        type: 'POST',
        dataType: 'json',
        timeout: 5000,
        error: function () {
            alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            if (json.success) {
                $("#tag_num").empty();
                $(json.msg).each(function (index, item) {
                    $("#tag_num").append("<li><a href=\"#none\" onclick=\"selected(this)\">" + item.TagName + "</a><i></i></li>");
                });
            }
        }
    });

}

function selected(object_a) {
    if ($(object_a).hasClass("current")) {
        $(object_a).removeClass("current");
    } else {
        $(object_a).addClass("current").siblings("a").removeClass("current");
    }
}

function AppendTags() {
    var tagval = $("#txttags").val().replace("自定义", "").replace(/\s/g, ""); //获取新增的标签值
    if (!checkLength($("#txttags")))
        return false;
    $(".att-tag-new").before(" <li><a class=\"current\" href=\"#none\" onclick=\"selected(this)\">" + tagval + "</a><i></i></li>");
    var licount = $("#tag_num2 li").size(); //获取总共添加的标签数量
    if (licount == 4) {
        $(".att-tag-new").hide();
    }
    $("#txttags").val('自定义');
}

function checkLength(txtobj) {
    var regu = "^[0-9a-zA-Z\u4e00-\u9fa5]{1,10}$";
    var re = new RegExp(regu);
    var tagval = $("#txttags").val().replace("自定义", "").replace(/\s/g, "");
    if (tagval.length <= 0) {
        $("#tishi span:eq(3)").show();
        return false;
    }
    if (!re.test(tagval)) {
        $("#tishi span:eq(4)").show();
        return false;
    }
    return true;
}


function removeTag(tag) {
    var removetext = $(tag).prev().text();
    var ul_id = $(tag).parents("ul").attr("id");
    if (ul_id == "tag_num2") {
        if ($(".att-tag-new").is(":hidden")) {
            $(".att-tag-new").show();
        } else {
            $(tag).parents("li").remove();
        }
    } else {
        var data = {};
        data.tagname = removetext;
        $.ajax({
            url: '/Handler/MemberHandler.ashx?action=DelteFavoriteTags',
            type: 'POST',
            data: data,
            dataType: 'json',
            timeout: 5000,
            error: function () {
                alert('操作错误,请与系统管理员联系!');
            },
            success: function (json) {
                if (json.success) {
                    $("#tag_num").empty();
                    $(tag).parents("li").remove();
                    $(json.msg).each(function (index, item) {
                        $("#tag_num").append("<li><a href=\"#none\" onclick=\"selected(this)\">" + item.TagName + "</a><i></i></li>");
                    });
                } else {
                    if (json.msg == "1") {
                        $("#tishi span:eq(5)").show();
                    } else if (json.msg == "2") {
                        $("#tishi span:eq(6)").show();
                    }
                }
            }
        });
    }

}

function checkTagNum(txtobj) {
    $("#tishi span").hide();
    var isValid = true;
    var num = $(".att-tag-list li a[class='current']").length;
    if (num <= 0) {
        $("#tishi span:eq(1)").show();
        isValid = false;
    }
    if (num >= 4) {
        $("#tishi span:eq(0)").show();
        isValid = false;
    }
    return isValid;
}

function SaveTags() {
    if (!checkTagNum($("#txttags"))) {//判断收藏记录数
        return false;
    }
    var tags = "";
    var data = {};
    data.favoriteid = favoriteid;
    $(".att-tag-list li a[class='current']").each(function () { tags += $(this).text() + "," });
    data.tags = tags.substr(0, tags.length - 1);

    $.ajax({
        url: '/Handler/MemberHandler.ashx?action=UpdateFavorite',
        type: 'POST',
        data: data,
        dataType: 'json',
        timeout: 5000,
        error: function () {
            alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            if (json.success) {
                $("#tishi span:eq(2)").show();
            }
        }
    });
    parent.$("#divFavorite").hide();
    return true;
}

function CloseFavorite() {
    $("#divFavorite").hide();
    $("#tishi span").hide();
}
function AddToFavorite() {
    if ($("#hiddenIsLogin").val() == "nologin") {
        $("#loginForBuy").show();
        return false;
    }
    var xtarget = $("#addFavorite").offset().left;
    var ytarget = $("#addFavorite").offset().top;
    if ($("#addcartButton").length > 0) {
        xtarget = $("#addcartButton").offset().left;
        ytarget = $("#addcartButton").offset().top;
    }


    //var xtarget = $("#addcartButton").offset().left;
    //var ytarget = $("#addcartButton").offset().top;

    $("#divFavorite").css("top", parseInt(ytarget + 40) + "px");

    $("#divFavorite").css("left", parseInt(xtarget) + "px");
    if ($(document).scrollTop() <= 145) {
        $("#divFavorite").css("top", parseInt(ytarget - 125) + "px");
    }
    $(".Favorite_title_r,.btn-cancel").bind("click", function () {
        $("#divFavorite").css('display', 'none')
    });
    $(".btn-ok").bind("click", function () { UpdateTags() });

    var data = {};
    data.ProductId = $("#hiddenpid").val();
    $.post("/Handler/MemberHandler.ashx?action=AddFavorite", data, function (result) {
        if (result.success) {
            favoriteid = result.favoriteid;
            $("#divFavorite").show();
            $("#favoriteCount").text(result.Count);
        }
        else {
            $("#divAlready").css("top", parseInt(ytarget + 40) + "px");

            $("#divAlready").css("left", parseInt(xtarget) + "px");
            if ($(document).scrollTop() <= 145) {
                $("#divAlready").css("top", parseInt(ytarget - 125) + "px");
            }
            $(".Favorite_title_r").bind("click", function () { $("#divAlready").css('display', 'none') });
            $("#divAlready").show();
        }
    });

   _adwq.push([ 
    '_setAction','8ke8d7', 
     $("#hiid_AdUserId").val(), //请填入用户id 
     data.ProductId //请填入商品id，选填
    ]); 


}
// 购买数量变化以后的处理
function ChangeBuyAmount() {
    if (ValidateBuyAmount()) {
        var quantity = parseInt($("#buyAmount").val());
        var oldQuantiy = parseInt($("#oldBuyNumHidden").val());
        var productTotal = eval($("#productDetails_Total").html());
        var totalPrice = productTotal / oldQuantiy * quantity;

        $("#productDetails_Total").html(totalPrice.toFixed(2));
        $("#oldBuyNumHidden").attr("value", quantity);
    }
}

// 购买按钮单击事件
function AddCurrentProductToCart() {
    isadd = false;
    if (!ValidateBuyAmount())
    {
        return false;
    }

    if (!IsallSelected()) {
        alert("请选择规格");
        return false;
    }
    var quantity = parseInt($("#buyAmount").val());    
    var cardinality = getBuyCardinality();
    if (quantity < cardinality) {
        alert($('#buyCardinalityTip').attr('title'));
        return false;
    }
    var maxcount = parseInt($("#maxcount").html());
    var stock = parseInt(document.getElementById("productDetails_Stock").innerHTML);
    if (quantity > stock) {
        alert("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }
    if (maxcount != "" && maxcount != null) {
        if (quantity > maxcount) {
            alert("此为限购商品，每人限购" + maxcount + "件");
            return false;
        }
    }
    if ($('#txtMaxCount').length > 0) {
        maxcount = parseInt($('#txtMaxCount').val());
        var soldCount = parseInt($('#txtSoldCount').val());
        if (quantity > maxcount - soldCount) {
            alert("购买不能超过" + (maxcount - soldCount) + "件");
            return false;
        }
    }
    if ($("#hiddenIsLogin").val() == "nologin") {
        $("#loginForBuy").show();
        return false;
    }

    var productSKuArr = $("#productDetails_sku_v").val();
    
    $.ajax({
        url: '/Handler/MemberHandler.ashx?action=CheckPurchase',
        type: 'post', dataType: 'json', timeout: 10000,
        data: { productSkuId: productSKuArr, quantity: quantity },
        async: false,
       
        success: function (resultData)
        {
            if (resultData.Status == "4")
            {
                alert("您购买的数量已经超过限购数量，请调整数量或选购其他商品！");
            } else {
                BuyProduct();
            }
          
        }
    });
}

function getBuyCardinality() {
    var s = $('#buyCardinalityTip').text();
    if (s == '') {
        return 1;
    }
    var c = s.substring(5);
    return parseInt(c);
}

// 登录后再购买
function LoginAndBuy() {
    var username = $("#textfieldusername").val();
    var password = $("#textfieldpassword").val();
    var thisURL = document.URL;

    if (username.length == 0 || password.length == 0) {
        alert("请输入您的用户名和密码!");
        return;
    }

    $.ajax({
        url: "Login.aspx",
        type: "post",
        dataType: "json",
        timeout: 10000,
        data: { username: username, password: password, action: "Common_UserLogin" },
        async: false,
        success: function (data) {
            if (data.Status == "Succes") {
                if (isadd) {
                    $("#loginForBuy").hide('hide');
                    $("#hiddenIsLogin").val('logined');
                    BuyProductToCart();//调用添加到购物车
                } else {
                    BuyProduct();
                    window.location.reload();
                }

            }
            else {
                alert(data.Msg);
            }
        }
    });
}

// 购买商品
function BuyProduct()
{
    var thisURL = document.URL.toLowerCase();
    if ($("#productDetails_sku_v").val().replace(/\s/g, "") == "") {
        alert("此商品已经不存在(可能库存不足或被删除或被下架)，暂时不能购买");
        return false;
    }
    if (thisURL.indexOf("groupbuyproduct_detail") != -1 || thisURL.indexOf("groupbuyproductdetails.aspx") != -1) {
        location.href = applicationPath + "/SubmmitOrder.aspx?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=groupBuy";
    }
    else if (thisURL.indexOf("countdownproduct_detail") != -1 || thisURL.indexOf("countdownproductsdetails.aspx") != -1) {
        location.href = applicationPath + "/SubmmitOrder.aspx?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=countDown";
    }
    else {

        if ($("#buyAmount").val().replace(/\s/g, "") == "" || parseInt($("#buyAmount").val().replace(/\s/g, "")) <= 0) {
            alert("商品库存不足 " + parseInt($("#buyAmount").val()) + " 件，请修改购买数量!");
            return false;
        }
        location.href = applicationPath + "/SubmmitOrder.aspx?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=signBuy";
    }
}

// 验证数量输入
function ValidateBuyAmount() {
    var buyAmount = $("#buyAmount");
    var ibuyNum = parseInt($("#buyAmount").val());
    if ($(buyAmount).val().length == 0 || isNaN(ibuyNum) || ibuyNum <= 0) {
        alert("请先填写购买数量,购买数量必须大于0!!");
        return false;
    }
    if ($(buyAmount).val() == "0" || $(buyAmount).val().length > 5 || ibuyNum <= 0 || ibuyNum > 99999) {
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
//*************匿名购买**********************************//
function AnonymousBuy() {
    if (isadd) {
        BuyProductToCart();
    }
    else {
        BuyProduct();
    }
    $("#loginForBuy").hide();
}

//*************2011-07-25  添加到购物车按钮单击事件****************//
function AddProductToCart() {
    if (!ValidateBuyAmount()) {
        return false;
    }

    if (!IsallSelected()) {
        alert("请选择规格");
        return false;
    }

    var quantity = parseInt($("#buyAmount").val());
    var stock = parseInt(document.getElementById("productDetails_Stock").innerHTML);
    if (quantity > stock) {
        alert("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }

    var count_sku = GetSpCount($("#productDetails_sku_v").val());
    // var AllStock = parseInt(document.getElementById("txtAllstock").value);
    // if (isNaN(AllStock) || AllStock <= 0) AllStock = stock;
    if (quantity + count_sku > stock) {
        alert("商品库存不足，您购物车中已存在该规格的商品数量为" + count_sku + "!");
        return false;
    }
    BuyProductToCart();//添加到购物车
}
//更新当前购物车指定规格已购买的数量
function UpdateSpCount(skuid, quantity) {
    quantity = parseInt(quantity);
    if (isNaN(quantity)) quantity = 0;
    spCountVal = $("#txCartQuantity").val();
    var newspCountVal = "";
    if (spCountVal == "") { newspCountVal = skuid + "|" + quantity; }
    else
    {
        var findSkuId = false;
        var spCountArr = spCountVal.split(",");
        for (var i = 0; i < spCountArr.length; i++) {
            if (spCountArr == "") continue;
            var itemArr = spCountArr[i].split('|');
            if (itemArr.length >= 2) {

                if (itemArr[0] == skuid) {
                    var temp_quantity = parseInt(itemArr[1]);
                    if (isNaN(temp_quantity)) temp_quantity = 0;
                    //spCountArr[i] = skuid + "|" + (temp_quantity + quantity);
                    spCountArr[i] = skuid + "|" + (quantity);
                    findSkuId = true;
                }
            }
            newspCountVal += (spCountArr[i]);
        }
        if (!findSkuId) newspCountVal += (newspCountVal == "" ? "" : ",") + (skuid + "|" + quantity);
    }
    $("#txCartQuantity").val(newspCountVal);
}
///获取当前购物车指定规格已购买的数量
function GetSpCount(skuid) {
    spCountVal = $("#txCartQuantity").val();
    if (spCountVal == "") { return 0; }
    else
    {
        var spCountArr = spCountVal.split(",");
        for (var i = 0; i < spCountArr.length; i++) {
            if (spCountArr == "") continue;
            var itemArr = spCountArr[i].split('|');
            if (itemArr.length >= 2) {
                if (itemArr[0] == skuid) {
                    var temp_quantity = parseInt(itemArr[1]);
                    return temp_quantity;
                }
            }
        }
    }
    return 0;
}

function BuyProductToCart() {

    var xtarget = $("#addcartButton").offset().left;
    var ytarget = $("#addcartButton").offset().top;
    $("#divshow,#divbefore").css("top", parseInt(ytarget + 40) + "px");

    $("#divshow,#divbefore").css("left", parseInt(xtarget) + "px");
    if ($(document).scrollTop() <= 145) {
        $("#divshow,#divbefore").css("top", parseInt(ytarget - 125) + "px");
    }
    $(".dialog_title_r,.btn-continue").bind("click", function () { $("#divshow").css('display', 'none') });
    $(".btn-viewcart").attr("href", applicationPath + "/ShoppingCart.aspx");
    $.ajax({
        url: "ShoppingHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyAmount").val()), productSkuId: $("#productDetails_sku_v").val() },
        async: false,
        beforeSend: function () {
            $("#divbefore").css('display', 'block');
        },
        complete: function () {
            // setTimeout("if($('#divshow').css('display')=='block'){$('#divshow').css('display','none')}",6000);
            //$("#divshow").blur(function(){alert('aaaa')});
        },
        success: function (resultData)
        {
       
            if (resultData.Status == "OK")
            {
                $("#divbefore").css('display', 'none');
                //$("#divshow").css('display', 'block');//显示添加购物成功
                function showTip()
                {
					if(window.successTimeout){
						window.clearTimeout(window.successTimeout);
					}
					$("#success-tip").html("+"+$("#buyAmount").val()).show().stop().animate({top:-48},200,function(){
						window.successTimeout = window.setTimeout(function(){
							$("#success-tip").stop().fadeOut(500,function(){
								$("#success-tip").css({top:0});
							});
						},500)
					});
				}
				showTip();
                $("#spcounttype").text(resultData.Quantity);
                UpdateSpCount($("#productDetails_sku_v").val(), resultData.SkuQuantity);
                $("#sptotal").text(resultData.TotalMoney);

                $("#spcartNum").text(resultData.Quantity);
                $("#ProductDetails_ctl03___cartMoney").text(resultData.TotalMoney);
				
				if(resultData.Quantity){
					$("#cart-num").show().html(resultData.Quantity);
				}
				if(resultData.data!=null)
				{
				    _adwq.push(['_setDataType', 'cart']);
				    _adwq.push(['_setCustomer', $("#hiid_AdUserId").val()   //1234567是一个例子，请换成当前登陆用户ID或用户名，未登录情况下传空字符串
				    ]);
				    _adwq.push(['_setItem',
                        resultData.data[0].skuId,            // 请填入商品编号 - 必填项 
                        resultData.data[0].ProductName,            // 请填入商品名称 - 必填项 
                        resultData.data[0].SalePrice,             // 请填入商品金额 - 必填项 
                        1,             // 请填入商品数量 - 必填项 
                        resultData.data[0].CategoryId,          // 请填入商品分类编号 - 必填项 
                        ''           // 请填入商品分类名称 - 必填项 
				    ]);
				    _adwq.push(['_trackTrans']);  // 触发加入购物车数据提交 - 固定值 - 必填项
				}
            } else if (resultData.Status == "0") {
                // 商品已经下架
                $("#divbefore").css('display', 'none');
                alert("此商品已经不存在(可能被删除或被下架)，暂时不能购买");
            }
            else if (resultData.Status == "1") {
                // 商品库存不足
                $("#divbefore").css('display', 'none');
                alert("商品库存不足 " + parseInt($("#buyAmount").val()) + " 件，请修改购买数量!");
            }
            else if (resultData.Status == "2") {
                // 规格不存在
                $("#divbefore").css('display', 'none');
                alert("商品规格获取失败，可能已被管理员删除！");
            } else if (resultData.Status == "4")
            {
                // 商品已经下架
                $("#divbefore").css('display', 'none');
                alert("当前商品已超过限购数量，请调整数量或选购其他商品！");
            }
            else {
                // 抛出异常消息
                $("#divbefore").css('display', 'none');
                alert(resultData.Status);
            }
        }
    });
}



//点击弹出商品类型说明层
function ShowProductTypeRemark() {

    if ($("#hiddenProductType").val() != null && $("#hiddenProductType").val() != "undefined") {//是否存在商品类型
        var typeId = $("#hiddenProductType").val();
        var x = $("#sptyperemark").offset().left + 70;
        var y = $(document).scrollTop();
        var divobj = $("<div id=\"dv_typeremark\" class=\"blk\"></div>");
        var div_m = $("<div class=\"main\"></div>");
        var div_mclose = $("<div class=\"c_head\">查看适合我的尺寸<a class=\"closeBtn\" id=\"a_close\">关闭</a></div>");
        var div_mcontent = $("<div class=\"con\">暂无说明</div>");
        if (typeId != "" && typeId != "undefined") {
            div_mcontent.html(typeId);
        }
        if ($("#dv_typeremark").html() == null) {
            div_m.append(div_mclose);
            div_m.append(div_mcontent);
            divobj.append(div_m);
            $(divobj).appendTo("body");
            $("#dv_typeremark").css("margin-top", y - 50);
            $(divobj).show('slow');


        } else {
            if ($('#dv_typeremark').css("display") == "" && $('#dv_typeremark').css("display") != "undefined") {
                $("#dv_typeremark").show('slow');
            }
        }
        $("#a_close").bind("click", function () { $('#dv_typeremark').hide('hide'); $("#dv_typeremark").remove(); })
    }

}


