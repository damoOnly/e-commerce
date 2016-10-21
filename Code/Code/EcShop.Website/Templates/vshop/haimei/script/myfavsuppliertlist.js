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

            var suppliers = resultData.favsuppliers;
            if (suppliers.length > 0) {
                var str = '';
                for (var i = 0; i < suppliers.length; i++) {
                    str += '<li>';
                    str += '<a href="/Vshop/SupplierDefault.aspx?SupplierId=' + suppliers[i].SupplierId + '">';
                    str += '<div class="supp-wrap fix">';
                    str += '<div class="fr">';
                    var suppfav = "supp-fav";
                    if (suppliers[i].IsCollect == 1) {
                        suppfav = "supp-fav-like";
                    }
                    str += '<span class=' + suppfav + '>' + suppliers[i].CollectCount + '</span>';
                    str += '</div>';
                    str += '<div class="supp-img">';
                    str += '<img class="img-responsive" src="' + suppliers[i].Logo + '" />';
                    str += '</div>';
                    str += '<div class="supp-con">';
                    str += '<div class="supp-name">' + suppliers[i].SupplierName + '</div>';
                    str += '<p>店主：' + suppliers[i].ShopOwner + '<span class="ml20">' + suppliers[i].CountyName + '</span></p>';
                    str += '<p>开店：' + suppliers[i].CreateDate + '</p>';
                    str += '</div>';
                    str += '</div>';
                    str += '</a>';
                    str += '<a class="del-btn" href="javascript:void(0)" onclick="Submit(' + suppliers[i].SupplierId + ')"></a>';
                    str += '</li>';
                }
                $(".goods-list-grid").append(str);
                //图片懒加载
                $("img.lazyload").lazyload({ failure_limit: 1 });
                pageNumber++;
                totalPage = resultData.totalPage;
            } else {
                $('.groom-wrap').show();
                $('#more-tip').removeClass("show").html('没有更多店铺了！').hide();
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
        action: "LoadMoreFavSuppliers",
        pageNumber: pageNumber,
        size: size
    };
    return postData;
}