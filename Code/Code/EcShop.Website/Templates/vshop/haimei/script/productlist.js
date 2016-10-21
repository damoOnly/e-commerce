var pageNumber = 1;
var totalPage = 0;
var size = 8;
var Depth = getParam("Depth");
var categoryId = getParam("categoryId");
var TopicId = getParam("TopicId"), SupplierId = getParam("SupplierId"), keyWord = getParam("keyWord"), importsourceid = getParam('importsourceid'), sort = getParam("sort"), brandid = getParam("brandid"), order = getParam("order");
var isFirstLoad = true;
var postData = '';
$(function () {
    //加载更多
    if (Depth == 1) {
        ShowTwoCategory();
        $(".pl-page").css("padding-top", '135px');
        $(".newcategory").show();
    }
    $(window).bind("scroll.loadmore", function () {
        var scrolltop = $(document).scrollTop();
        var winH = window.innerHeight;
        var docH = $(document).height();
        if (scrolltop >= (docH - winH)) {
            $('#more-tip').addClass("show");
            loadMore();
            //alert("已到达底部");
        }
    });
    var scrollTop0 = $(document).scrollTop();
    $(window).scroll(function () {
        var scrollTop1 = $(document).scrollTop();
        if ((scrollTop1 - scrollTop0) > 50 && $(".order-list").css("display") == 'none') {
            $(".order-list").slideDown(100, function () {
                var h = parseInt($(".neworderlist").css("top")) + $(".oneworderlist").height() + 5;
                $(".page").css({ "paddingTop": h });
            });   
        }
        else if ((scrollTop1 - scrollTop0) < -50 && $(".order-list").css("display") == 'block' && scrollTop1 != 0) {
            $(".order-list").slideUp(100, function () {
                var h = parseInt($(".neworderlist").css("top")) + $(".oneworderlist").height() + 5;
                $(".page").css({ "paddingTop": h });
            });
        }
        else if (scrollTop1 == 0) {
            $(".order-list").slideDown(100, function () {
                var h = parseInt($(".neworderlist").css("top")) + $(".neworderlist").height() + 5;
                $(".page").css({ "paddingTop": h });
            });
        }
        scrollTop0 = scrollTop1;
    })
    setStatus();
    $(".order-list").delegate("a", "click", function () {
        sort = $(this).attr("name");
        var theme = "selected ";
        var theme2 = "selected down up";
        if (sort != 'default') {
            if (order == 'desc') {
                order = 'asc';
                theme = 'selected up';
            } else {
                order = 'desc';
                theme = 'selected down';
            }  
        } else { sort = '';order = ''; theme = "selected "; }
        $(this).parent().attr("class", theme).siblings().removeClass(theme2);
        reloadPro(categoryId);
    })
});
function loadMore() {
    postData = GetPostDate();
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: postData,
        async: false,
        success: function (resultData) {
            var products = resultData.products;
            if (products.length > 0) {
                var str = '';
                for (var i = 0; i < products.length; i++) {
                    str += '<li>';
                    str += '<div class="gd-box fix">';
                    str += '<a href="/Vshop/ProductDetails.aspx?ProductId=' + products[i].ProductId + '">';
                    var tempdiscount;
                    if (products[i].SalePrice != null && products[i].SalePrice != undefined && products[i].SalePrice != "" && products[i].MarketPrice != null && products[i].MarketPrice != undefined && products[i].MarketPrice != "") {
                        tempdiscount = (products[i].SalePrice / products[i].MarketPrice).toFixed(2) * 10;
                    }
                    //<%# bool.Parse(Eval("IsDisplayDiscount").ToString())  ? "<div class='prolist-discount'><em>"+(decimal.Parse(Eval("SalePrice").ToString())/decimal.Parse(Eval("MarketPrice").ToString()))*10+"折</em></div>":"" %>
                    str += (products[i].IsDisplayDiscount != null && products[i].IsDisplayDiscount != "") ? products[i].IsDisplayDiscount : false ? "<div class='prolist-discount'><em>" + tempdiscount + "折</em></div>" : "";
                    str += '<div class="gd-img">';
                    str += '<img class="lazyload" src="/resource/default/image/lazy.png" data-original="' + products[i].ThumbnailUrl220 + '" style="border-width:0px;">';
                    str += (products[i].ActivityId && products[i].IsPromotion) ? "<div class='gd-promote'>促</div>" : "";
                    str += '</div>';
                    str += '<div class="gd-info">';
                    str += '<p class="gd-name">' + products[i].ProductName + '</p>';
                    str += '<div class="gd-price-wrap"><p class="gd-price">';
                    if (products[i].SalePrice != null && products[i].SalePrice != undefined && products[i].SalePrice != "") {
                        str += '¥<span>' + products[i].SalePrice.toFixed(2) + '</span>';
                    }
                    str += '</p>';
                    str +='<p class="gd-rate as">';
                    if(products[i].MarketPrice != null && products[i].MarketPrice != undefined && products[i].MarketPrice != "") {
                        str += '&yen<s>' + products[i].MarketPrice.toFixed(2) + '</s>';
                    }
                    str += '</p></div>';
                    //str += '<p class="gd-rate">税率：' + products[i].TaxRate * 100 + '%</p>';
                    str += '<p class="gd-sales dis-none">已售' + products[i].SaleCounts + '</p>';
                    str += '<div class="dis-none">';
                    str += '<p class="gd-sales">' + products[i].ShortDescription + '</p>';
                    str += '</div>';
                    str += '<div class="shop-place"></div></div>';
                    str += '</a><a class="gd-shop" href="/vshop/SupplierDefault.aspx?SupplierId=' + products[i].SupplierId + '" target="_blank" > <img src=' + products[i].Icon + ' style="height:18px;"/>' + products[i].ShopName + '</a>';

                    var tempclass = "fast-add show-skus  cart-btn-disable";
                    if (products[i].Stock > 0) {
                        tempclass = "fast-add show-skus cart-btn";
                    }
                    str += '<a class="' + tempclass + '" title="加入购物车" fastbuy_skuid="' + products[i].fastbuy_skuid + '" smallimg="' + products[i].ThumbnailUrl60 + '" productid="' + products[i].ProductId + '" price="' + products[i].SalePrice.toFixed(2) + '">';
                    str += '</a>';
                    str += '</div>';
                    str += '</li>';
                }
                $(".goods-list-grid").append(str);
                //图片懒加载
                $("img.lazyload").lazyload({ failure_limit: 1 });
                pageNumber++;
                totalPage = resultData.totalPage;
            } else {
             //   $('.groom-wrap').show();
                $('#more-tip').removeClass("show").html('没有更多商品了！').hide();
               // $(window).unbind("scroll.loadmore");
            }
        }
    });
}
function GetPostDate() {
    if (isFirstLoad === true) {
        pageNumber = 2;
        isFirstLoad = false;
    } else {
        pageNumber++;
    }
     postData = {
        action: "LoadMoreProducts",
        pageNumber: pageNumber,
        TopicId: TopicId,
        SupplierId: SupplierId,
        keyWord: keyWord,
        categoryId:categoryId,
        sort: sort,
        brandid: brandid,
        order: order,
        size: size
    };
    return postData;
}
function reloadPro(cate) {
    $(".goods-list-grid").html("");
    categoryId = cate;
    isFirstLoad = false;
    pageNumber = 0;
    loadMore();
}
$(".newcategory").delegate("a", "click", function () {
    categoryId = $(this).attr('cate');
    reloadPro(categoryId);
    ShowThirdCategoryOrBrand(categoryId);
    if ($(this).html() != "全部") {
        
    } else {
        $(".thirdcatagory").html('').hide();
        var h = parseInt($(".neworderlist").css("top")) + $(".neworderlist").height() + 5;
        $(".page").css({ "paddingTop": h });
    }
    $(".newcategorylist a").removeClass('be_cur');
    $(this).addClass("be_cur");
    var obj = $(this).parent();
    if (obj.offset().left < 0 || obj.offset().left > $(window).width()) {
        var long = 0;
        $(".newcategorylist").animate({ left: -(obj.position().left + obj.outerWidth() - $(window).width()) }, 200)
    }
});
$(".thirdcatagory").delegate("a", "click", function () {
    categoryId = $(this).attr('cate');
    reloadPro(categoryId);
    $(".thirdcatagorylist ul a").removeClass("be_cur");
    $(this).addClass("be_cur");
    var obj = $(this).parent();
    if (obj.offset().left < 0 || obj.offset().left > $(window).width()) {
        var long = 0;
        $(".thirdcatagorylist").animate({ left: -(obj.position().left + obj.outerWidth() - $(window).width()) }, 200)
    }
});
//显示二级分类
function ShowTwoCategory() {
 //   var categoryId = getParam('CategoryId');
    if (categoryId == null || categoryId == undefined) {
        categoryId = "";
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ShowTwoCategory", CategoryId: categoryId },
        success: function (resultData) {
            if (resultData != null) {
                if (resultData.Status == "-1") {
                    alert_h("没有该分类, 请重试");
                }
                else {
                    var strs = "";
                    if (resultData[0].NextCategoryPartInfo.length > 0) {
                        strs = '<div class="newcategorylist"><ul><li><a href="javascript:void(0)" class="be_cur" cate="' + resultData[0].CategoryId + '">全部</a></li>'
                        for (var i = 0; i < resultData[0].NextCategoryPartInfo.length; i++)
                        { var categoryInfo = resultData[0].NextCategoryPartInfo[i]; strs += '<li><a href="javascript:void(0)" cate="' + categoryInfo.CategoryId + '">' + categoryInfo.Name + '</a></li>'; }
                        strs += '</ul></div>';
                    }
                    $(".newcategory").html(strs);
                    loaded();
                }
            }
        },
        error: function () { alert('请求有误') }
    });
}

//显示三级
function ShowThirdCategoryOrBrand(CategoryId) {
    if (categoryId == null || categoryId == undefined) {
        categoryId = "";
    }
    var brandid = getParam('brandid');
    if (brandid == null || brandid == undefined) {
        brandid = "";
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ShowThirdCategoryOrBrand", CategoryId: categoryId, BrandId: brandid },
        success: function (data) {
            var strs = '';
            if (data != null) {
                if (data.Status == "-1") {
                    strs = "没有更多分类, 请重试"
                }
                else {
                    if (data[0].NextCategoryPartInfo.length > 0) {
                        strs = '<div class="thirdcatagorylist"><ul><li><a href="javascript:void(0)" class="be_cur" cate="' + data[0].CategoryId + '">全部</a></li>'
                        for (var i = 0; i < data[0].NextCategoryPartInfo.length; i++)
                        { var categoryInfo = data[0].NextCategoryPartInfo[i]; strs += '<li><a href="javascript:void(0)" cate="' + categoryInfo.CategoryId + '">' + categoryInfo.Name + '</a></li>'; }
                        strs += '</ul></div>';
                    }    
                }
                $(".thirdcatagory").html(strs).show();
                var h = $(".neworderlist").offset().top + $(".neworderlist").height();
                $(".page").stop(true, true).animate({ "paddingTop": h }, 100);
                scroll2();
            }
        }
    });
}
function loaded() {
    var width = 0;
    $(".newcategorylist li").each(function () {
        width += $(this).outerWidth();
    });
    $(".newcategorylist").width(width);
    var myScroll1 = new IScroll('.newcategory', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false });
};
function scroll2() {
    var width2 = 0;
    $(".thirdcatagorylist li").each(function () {
        width2 += $(this).outerWidth() + 2;
    });
    $(".thirdcatagorylist ul").outerWidth(width2);
    var myScroll2 = new IScroll('.thirdcatagory', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false });
};
//加载类目
function setStatus() {
    var sort = getParam('sort');
    var order = getParam('order');
    if (sort && order) {
        var $link = $('.order-list a[name="' + sort + '"]');
        var $curLi = $link.parent();
        var theme = "selected ";
        var theme2 = "selected down up";
        if (order == 'desc') {
            theme += 'down';
        } else {
            theme += 'up';
        }
        $curLi.addClass(theme).siblings().removeClass(theme + theme2);
    } else {
        var $link = $('.order-list a[name=default]');
        var $curLi = $link.parent();
        var theme = "selected ";
        $curLi.addClass(theme).siblings().removeClass(theme + theme2);
    }
    //var categoryId = getParam('categoryId');
    //var keyWords = getParam('keyWord');
    //var brandid = getParam('brandid');
    //var importsourceid = getParam('importsourceid');
    //var TopicId = getParam('TopicId');
    if (categoryId == null || categoryId == undefined) {
        categoryId = "";
    }

    if (keyWord == null || keyWord == undefined) {
        keyWord = "";
    }

    if (brandid == null || brandid == undefined) {
        brandid = "";
    }

    if (importsourceid == null || importsourceid == undefined) {
        importsourceid = "";
    }


    if (TopicId == null || TopicId == undefined) {
        TopicId = "";
    }

   // var SupplierId = getParam('SupplierId');

    if (SupplierId == null || SupplierId == undefined) {
        SupplierId = "";
    }

    $(".order-list > ul > li a:not(.search-arrow)").each(function (i, ele) {
        $link = $(ele);
        var $li = $link.parent();
        url = '/vshop/SupproductList.aspx?categoryId=' + categoryId + '&keyWord=' + keyWord + '&brandid=' + brandid + "&TopicId=" + TopicId + "&importsourceid=" + importsourceid + "&SupplierId=" + SupplierId;// escape(keyWords);
        if (i > 0) {//第一个为默认面，不设置排序
            url += '&sort=' + $link.attr('name');
            if ($li.hasClass('up')) {
                url += '&order=desc';
            } else {
                url += '&order=asc';
            }
        }
    //    $link.attr('href', url);
    });
}
//类目筛选
function ShowCategory() {
    var isshow = true;//false;
    var categoryId = getParam('categoryId');
    if (categoryId == null || categoryId == undefined) {
        categoryId = "";
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ShowCategoryBrands", categoryId: categoryId },
        success: function (resultData) {
            if (resultData != null) {
                if (resultData.Status == "-1") {
                    alert_h("没有分类, 请重试");
                }
                else {
                    var strs = "";
                    var tempdepth = 1;
                    if (resultData.importSources.length > 0 || resultData.brands.length > 0 || resultData.Categorys.length) {
                        strs += "<div class=\"brush-cover\"></div>";
                        strs += "<div class=\"brush-con fix\">";
                        strs += "<div class=\"brush-header fix\"><a class=\"cancel-btn\">取消</a>筛选</div>";
                        strs += "<div class=\"brush-list fix\">";

                        //原产地
                        if (resultData.importSources.length > 0) {
                            strs += "<ol class=\"fix\">";
                            strs += "<h4>原产地</h4>";
                            strs += "<div class=\"fix brand-list\">";
                            for (var i = 0; i < resultData.importSources.length; i++) {
                                var importinfo = resultData.importSources[i];

                                strs += "<a ImportSourceId=" + importinfo.ImportSourceId + " name=\"ImportSourceId\"><img src=" + importinfo.Icon + " />" + importinfo.CnArea + "</a>";
                            }
                            strs += "</div>";
                            strs += "</ol>";
                        }
                        //品牌
                        if (resultData.brands.length > 0) {
                            strs += "<ol class=\"fix\">";
                            strs += "<h4>品牌</h4>";
                            strs += "<div class=\"fix brand-list\">";
                            for (var i = 0; i < resultData.brands.length; i++) {
                                var brandInfo = resultData.brands[i];

                                strs += "<a BrandId=" + brandInfo.BrandId + " name=\"BrandId\"><img src=" + brandInfo.Logo + " name=\"BrandId\"/>" + brandInfo.BrandName + "</a>";
                            }
                            strs += "</div>";
                            strs += "</ol>";
                        }
                        //分类				
                        for (var i = 0; i < resultData.Categorys.length; i++) {
                            if (resultData.Categorys[i]["CategoryId"] == categoryId) {
                                tempdepth = resultData.Categorys[i]["Depth"];
                                continue;
                            }
                        }
                        if (tempdepth == 1) {

                            for (var i = 0; i < resultData.Categorys.length; i++) {
                                var categoryInfo = resultData.Categorys[i];
                                if (categoryInfo.Depth == 2 && categoryInfo.ParentCategoryId == categoryId) {
                                    strs += "<ol class=\"fix\">";

                                    var tmpCid = categoryInfo.CategoryId;
                                    strs += " <h4>" + categoryInfo.Name + "</h4>";
                                    if (!categoryInfo.HasChildren) {
                                        continue;
                                    }
                                    strs += "<div class=\"fix\">";
                                    for (var j = 0; j < resultData.Categorys.length; j++) {
                                        var tempcategoryInfo = resultData.Categorys[j];
                                        if (tempcategoryInfo.Depth == 3 && tempcategoryInfo.ParentCategoryId == tmpCid) {
                                            strs += "<a CategoryId=" + tempcategoryInfo.CategoryId + " name=\"CategoryId\">" + tempcategoryInfo.Name + "</a>";
                                        }
                                    }
                                    strs += "</div></ol>";
                                    isshow = true;
                                }
                            }
                        } else if (tempdepth == 2) {
                            for (var i = 0; i < resultData.Categorys.length; i++) {
                                var categoryInfo = resultData.Categorys[i];
                                if (categoryInfo.Depth == 2 && categoryInfo.CategoryId == categoryId) {
                                    strs += "<ol class=\"fix\">";

                                    var tmpCid = categoryInfo.CategoryId;
                                    strs += " <h4>" + categoryInfo.Name + "</h4>";
                                    if (!categoryInfo.HasChildren) {
                                        continue;
                                    }
                                    strs += "<div class=\"fix\">";
                                    for (var j = 0; j < resultData.Categorys.length; j++) {
                                        var tempcategoryInfo = resultData.Categorys[j];
                                        if (tempcategoryInfo.Depth == 3 && tempcategoryInfo.ParentCategoryId == tmpCid) {
                                            strs += "<a CategoryId=" + tempcategoryInfo.CategoryId + " name=\"CategoryId\">" + tempcategoryInfo.Name + "</a>";
                                        }
                                    }
                                    strs += "</div></ol>";
                                    isshow = true;
                                }
                            }
                        } else if (tempdepth == 3) {
                            strs += "<ol class=\"fix\">";
                            strs += "<h4>类别</h4>";
                            strs += "<div class=\"fix\">";
                            for (var i = 0; i < resultData.Categorys.length; i++) {
                                var categoryInfo = resultData.Categorys[i];
                                if (categoryInfo.Depth == 3 && categoryInfo.ParentCategoryId == categoryId) {
                                    strs += "<a CategoryId=" + categoryInfo.CategoryId + " name=\"CategoryId\">" + categoryInfo.Name + "</a>";
                                } else if (categoryInfo.Depth == 3 && categoryInfo.CategoryId == categoryId) {
                                    strs += "<a CategoryId=" + categoryInfo.CategoryId + " name=\"CategoryId\">" + categoryInfo.Name + "</a>";
                                }
                                isshow = true;
                            }
                            strs += "</div></ol>";
                        }
                        //strs += "</div><div class=\"brush-btns\"> <a class=\"gray-btn ok-btn\">确定</a><a class=\"cancel-btn\">取消</a> </div>";
                        strs += "</div>";
                        strs += "<div class=\"brush-footer fix\"><div class=\"brush-btns\"><a class=\"reset-btn\">重置</a><a class=\"cancel-btn ok-btn\">确定</a></div></div>";
                        strs += "</div>";
                    }
                    if (!isshow) {
                        strs = "";
                    } else {
                        $("#liabrush").show();
                    }
                    $("#brush-wrap").html(strs);
                }
            }
        }
    });
}