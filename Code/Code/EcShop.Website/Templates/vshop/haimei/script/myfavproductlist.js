var pageNumber = 1;
var totalPage = 0;
var size = 8;
var isFirstLoad = true;
$(function () {
    //加载更多
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
});
function loadMore() {
    var postData = GetPostDate();

    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: postData,
        async: false,
        success: function (resultData) {

            var products = resultData.favproducts;
            if (products.length > 0) {
                var str = '';
                for (var i = 0; i < products.length; i++) {
                    str += '<li>';
                    str += '<div class="ct-box">';
                    str += '<div class="ct-img">';
                    str += '<a href="/Vshop/ProductDetails.aspx?ProductId=' + products[i].ProductId + '">';
                    str += '<img  src="' + products[i].ThumbnailUrl60 + '" >';
                    str += '</a>';
                    str += '</div>';
                    str += '<div class="ct-info">';
                    str += '<a href="/Vshop/ProductDetails.aspx?ProductId=' + products[i].ProductId + '">';
                    str += '<div class="ct-name">' + products[i].ProductName + '</div>';

                    str += '<div class="fix">';
                    if (products[i].SalePrice != null && products[i].SalePrice != undefined && products[i].SalePrice != "") {
                        str += '<span class="ct-price">¥<span>' + products[i].SalePrice.toFixed(2) + '</span></span>';
                    }
                    str += '</div>';
                    str += '</a>';
                    str += '<div class="goods-num fix">';
                    str += '<div class="info">';
                    str += '<a class="del-btn" href="javascript:void(0)" onclick="Submit(' + products[i].FavoriteId + ')"></a>';
                    str += '</div>';
                    str += '</div>';
                    str += '</div>';
                    str += '</div>';
                    str += '</li>';
                }
                $(".goods-list-grid").append(str);
                //图片懒加载
                $("img.lazyload").lazyload({ failure_limit: 1 });
                pageNumber++;
                totalPage = resultData.totalPage;
            } else {
                $('.groom-wrap').show();
                $('#more-tip').removeClass("show").html('没有更多商品了！').hide();
                $(window).unbind("scroll.loadmore");
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
    var postData = {
        action: "LoadMoreFavProducts",
        pageNumber: pageNumber,
        size: size
    };
    return postData;
}