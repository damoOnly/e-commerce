function ShowDropDown() {
    $("#ddlSubType").show();
    $("#Tburl").hide();
    $("#ddlThridType").hide();
    $("#navigateDesc").hide();
}
function ShowThirdDropDown() {
    $("#ddlSubType").trigger("change");
}
function HiddenAll() {
    $("#Tburl").hide();
    $("#ddlSubType").hide();
    $("#ddlThridType").hide();
    $("#navigateDesc").hide();
    $("#dropCategories").hide();
    $("#dropImportSourceType").hide();
    $("#linkSelectProduct").hide();
    $("#productName").hide();
    $("#dropBrandTypes").hide();
}
function ShowTextBox() {
    $("#ddlSubType").hide();
    $("#Tburl").show();
    $("#navigateDesc").hide();
    $("#ddlThridType").hide();
    $("#linkSelectProduct").hide();
    $("#productName").hide();
    $("#dropImportSourceType").hide();
    $("#dropBrandTypes").hide();
}
function ShowNavigate() {
    $("#ddlSubType").hide();
    $("#ddlThridType").hide();
    $("#linkSelectProduct").hide();
    $("#productName").hide();
    $("#dropImportSourceType").hide();
    $("#Tburl").show();
    $("#navigateDesc").show();
    $("#dropBrandTypes").hide();
}
function GetTopics() {
    $.ajax({
        url: "../VsiteHandler.ashx",
        type: "POST",
        dataType: "json",
        data: { "actionName": "Topic" },
        success: function (result) {
            $("#ddlSubType").empty();
            if (result != null) {
                $(result).each(
                        function (index, item) {
                            var option = $("<option value=" + item.TopicId + ">" + item.Title + "</option>");
                            $("#ddlSubType").append(option);
                        }
                        );
            }
            else {
                // alert("加载专题错误！");
            }
        },
        error: function (xmlHttpRequest, error) {
            alert(error);
        }
    });
}

function GetCategory() {
    $.ajax({
        url: "../VsiteHandler.ashx",
        type: "POST",
        dataType: "json",
        data: { "actionName": "Category" },
        success: function (result) {
            $("#ddlSubType").empty();
            if (result != null) {
                $(result).each(
                        function (index, item) {
                            var option = $("<option value=" + item.CateId + ">" + item.CateName + "</option>");
                            $("#ddlSubType").append(option);
                        }
                        );
            }
            else {
                // alert("加载专题错误！");
            }
        },
        error: function (xmlHttpRequest, error) {
            alert(error);
        }
    });
}


function GetVotes() {
    $.ajax({
        url: "../VsiteHandler.ashx",
        type: "POST",
        dataType: "json",
        data: { "actionName": "Vote" },
        success: function (result) {
            $("#ddlSubType").empty();
            if (result != null) {
                $(result).each(
                        function (index, item) {
                            var option = $("<option value=" + item.VoteId + ">" + item.VoteName + "</option>");
                            $("#ddlSubType").append(option);
                        }
                        );
            }
            else {
                // alert("加载投票错误！");
            }
        },
        error: function (xmlHttpRequest, error) {
            alert(error);
        }
    });
}

function GetActivity() {
    $.ajax({
        url: "../VsiteHandler.ashx",
        type: "POST",
        async: false,
        dataType: "json",
        data: { "actionName": "Activity" },
        success: function (result) {
            $("#ddlSubType").empty();
            if (result != null) {
                $("#ddlSubType").append('<option>请选择活动</option>');
                $(result).each(
                        function (index, item) {
                            var option = $("<option value=" + item.Value + ">" + item.Name + "</option>");
                            $("#ddlSubType").append(option);
                        }
                        );
            }
            else {
                // alert("加载活动错误！");
            }
        },
        error: function (xmlHttpRequest, error) {
            alert(error);
        }
    });
}
//加载文章分类
function GetArticleCategory() {
    $.ajax({
        url: "../VsiteHandler.ashx",
        type: "POST",
        async: false,
        dataType: "json",
        data: { "actionName": "ArticleCategory" },
        success: function (result) {
            $("#ddlSubType").empty();
            if (result != null) {
                $("#ddlSubType").append('<option>请选择文章分类</option>');
                $(result).each(
                        function (index, item) {
                            if (index == 0)
                                var option = $("<option value=" + item.Value + " selected=\"selected\">" + item.Name + "</option>");
                            else
                                var option = $("<option value=" + item.Value + ">" + item.Name + "</option>");
                            $("#ddlSubType").append(option);
                        }
                        );
            }
            else {
                // alert("加载活动错误！");
            }
        },
        error: function (xmlHttpRequest, error) {
            alert(error);
        }
    });
}
///加载文章列表
function GetArticleList(type) {
    $.ajax({
        url: "../VsiteHandler.ashx",
        type: "POST",
        dataType: "json",
        data: { "actionName": "ArticleList", "categoryId": type },
        success: function (result) {
            $("#ddlThridType").empty();
            if (result != null && result.length > 0) {
                $(result).each(
                        function (index, item) {
                            var option = $("<option value=" + item.Value + ">" + item.Name + "</option>");
                            $("#ddlThridType").append(option);
                        }
                        );
            }
            else {
                alert("加载文章列表错误,或者你没有添加该栏目下的文章请先添加！");
            }
        },
        error: function (xmlHttpRequest, error) {
            alert(xmlHttpRequest.toString());
        }
    });
}
function GetLoctionUrl() {
    var typeval = $("#ddlType").val();
    var result;
    switch (typeval) {
        case "Topic":
            result = $("#ddlSubType").val();
            break;
        case "Vote":
            result = $("#ddlSubType").val();
            break;
        case "Activity":
            var thirdtype = $("#ddlThridType").val();
            result = $("#ddlSubType").val() + "," + thirdtype;
            if (thirdtype == "" || thirdtype == null) {
                alert("请选择一个活动");
                return false;
            }
            break;
        case "Home":
            result = "Default.aspx";
            break;
        case "Category":
            // result = $("#ddlSubType").val();
            //result = "ProductSearch.aspx";
            var categoryId = $("#dropCategories").val();
            if (categoryId == null || categoryId == "") { alert("请选择一个分类"); return false; }
            result = categoryId;
            break;
        case "ShoppingCart":
            result = "ShoppingCart.aspx";
            break;
        case "OrderCenter":
            result = "MemberCenter.aspx"
        case "VipCard":
            result = "MemberCard.aspx";
            break;
        case "Link":
            result = $("#Tburl").val();
            break;
        case "Phone":
            result = $("#Tburl").val();
            break;
        case "Address":
            result = $("#Tburl").val();
            break;
        case "GroupBuy":
            result = "GroupBuyList.aspx";
            break;
        case "Brand":
            var BrandId = $("#dropBrandTypes").val();
            if (BrandId == null || BrandId == "") { alert("请选择品牌"); return false; }
            result = BrandId;
            break;
        case "ImportSourceType":
            var imtypeId = $("#dropImportSourceType").val();
            if (imtypeId == null || imtypeId == "") { alert("请选择原产地"); return false; }
            result = imtypeId;
            break;
        case "Article":
            var thirdtype = $("#ddlThridType").val();
            if (thirdtype == null || thirdtype == "") { alert("请选择一篇文章"); return false; }
            result = "ArticleDetails.aspx?articleId=" + thirdtype;
            break;

        case "Product":
            var productId = $("#productid").val();
            if (productId == null || productId == "") { alert("请选择一件商品"); return false; }
            result = productId;
            break;

    }
    $("#locationUrl").val(result);
    return true;
}

function showThird(type) {

    $.ajax({
        url: "../VsiteHandler.ashx",
        type: "POST",
        dataType: "json",
        data: { "actionName": "ActivityList", "acttype": type },
        success: function (result) {
            $("#ddlThridType").empty();
            if (result != null && result.length > 0) {
                $(result).each(
                        function (index, item) {
                            var option = $("<option value=" + item.ActivityId + ">" + item.ActivityName + "</option>");
                            $("#ddlThridType").append(option);
                        }
                        );
            }
            else {
                alert("加载活动列表错误,或者你没有添加该栏目下的活动请先添加！");
            }
        },
        error: function (xmlHttpRequest, error) {
            // alert(xmlHttpRequest.toString());
        }
    });
}

function BindSubType() {
    $("#ddlSubType").bind("change", function () {

        var typeval = $(this).val();
        if ($("#ddlType").val() == "Activity") {
            showThird(typeval);
        }
        if ($("#ddlType").val() == "Article") {
            GetArticleList(typeval);
        }
    });

}

function BindType() {
    BindSubType();
    $("#ddlType").bind("change", function () {
        var typeval = $(this).val();
        switch (typeval) {
            case "Topic":
                ShowDropDown();
                GetTopics();
                $("#dropCategories").hide();
                $("#dropImportSourceType").hide();
                $("#linkSelectProduct").hide();
                $("#productName").hide();
                $("#dropImportSourceType").hide();
                $("#dropBrandTypes").hide();

                break;
            case "Vote":
                ShowDropDown();
                GetVotes();
                $("#dropCategories").hide();
                $("#dropImportSourceType").hide();
                $("#linkSelectProduct").hide();
                $("#productName").hide();
                $("#dropImportSourceType").hide();
                $("#dropBrandTypes").hide();
                break;
            case "Activity":
                ShowDropDown();
                GetActivity();
                showThird($("#ddlSubType").val());
                //$("#ddlThridType").show();
                $("#dropCategories").hide();
                $("#dropImportSourceType").hide();
                $("#linkSelectProduct").hide();
                $("#productName").hide();
                $("#dropImportSourceType").hide();
                $("#dropBrandTypes").hide();
                break;
            case "Home":
                HiddenAll();
                break;
            case "OrderCenter":
                HiddenAll();
                break;
            case "Category":
                //  ShowDropDown();
                //GetCategory();
                //HiddenAll();
                $("#Tburl").hide();
                $("#ddlSubType").hide();
                $("#ddlThridType").hide();
                $("#navigateDesc").hide();
                $("#dropImportSourceType").hide();
                $("#linkSelectProduct").hide();
                $("#productName").hide();
                $("#dropBrandTypes").hide();
                $("#dropCategories").show();
                break;
            case "ShoppingCart":
                HiddenAll();
                break;
            case "VipCard":
                HiddenAll();
                break;
            case "Link":
                ShowTextBox();
                break;
            case "Phone":
                ShowTextBox();
                $("#dropCategories").hide();
                $("#dropImportSourceType").hide();
                $("#linkSelectProduct").hide();
                $("#productName").hide();
                $("#dropImportSourceType").hide();
                $("#dropBrandTypes").hide();
                break;
            case "Address":
                ShowNavigate();
                break;
            case "GroupBuy":
                HiddenAll();
                break;
            case "Brand":
                $("#Tburl").hide();
                $("#ddlSubType").hide();
                $("#ddlThridType").hide();
                $("#navigateDesc").hide();
                $("#dropCategories").hide();
                $("#productName").hide();
                $("#linkSelectProduct").hide();
                $("#dropImportSourceType").hide();
                $("#dropBrandTypes").show();
                break;
            case "Article":
                ShowDropDown();
                GetArticleCategory();
                GetArticleList($("#ddlSubType").val());
                $("#ddlThridType").show();

                $("#dropCategories").hide();
                $("#dropImportSourceType").hide();
                $("#productName").hide();
                $("#linkSelectProduct").hide();
                $("#dropImportSourceType").hide();
                $("#dropBrandTypes").hide();
                break;
            case "ImportSourceType":
                $("#Tburl").hide();
                $("#ddlSubType").hide();
                $("#ddlThridType").hide();
                $("#navigateDesc").hide();
                $("#dropCategories").hide();
                $("#productName").hide();
                $("#linkSelectProduct").hide();
                $("#dropImportSourceType").show();
                $("#dropBrandTypes").hide();
                break;

            case "Product":
                $("#Tburl").hide();
                $("#ddlSubType").hide();
                $("#ddlThridType").hide();
                $("#navigateDesc").hide();
                $("#dropCategories").hide();
                $("#dropImportSourceType").hide();
                $("#productName").show();
                $("#linkSelectProduct").show();
                $("#dropBrandTypes").hide();
        }
    }
);

}

function ShowType() {
    $("#ddlType option").each(function (i) {
        var optionValue = this.value;
        switch (optionValue) {
            case "Topic":
                this.text = "主题";
                break;
            case "Activity":
                this.text = "活动";
                break;
            case "Home":
                this.text = "首页";
                break;
            case "Category":
                this.text = "商品分类";
                break;
            case "ShoppingCart":
                this.text = "购物车";
                break;
            case "OrderCenter":
                this.text = "订单中心";
                break;
            case "VipCard":
                this.text = "会员卡";
                break;
            case "Link":
                this.text = "链接";
                break;
            case "Phone":
                this.text = "电话";
                break;
            case "Address":
                this.text = "地址";
                break;
            case "GroupBuy":
                this.text = "团购";
                break;
            case "Brand":
                this.text = "品牌";
                break;
            case "Article":
                this.text = "文章";
                break;
            case "ImportSourceType":
                this.text = "原产地";
                break;
            case "Product":
                this.text = "商品";
                break;
            case "Register":
                this.text = "注册";
                break;
        }
    });
}