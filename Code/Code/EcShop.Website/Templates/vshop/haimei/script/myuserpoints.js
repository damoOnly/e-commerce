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
            var userpoints = resultData.myuserpoints;
            if (userpoints.length > 0) {
                var str = '';
                for (var i = 0; i < userpoints.length; i++) {
                    //                    <tr>
                    //    <td>
                    //        <span class="green-lb"><%#int.Parse(Eval("Increased").ToString())>0?"+"+Eval("Increased").ToString():"" %><%#int.Parse(Eval("Reduced").ToString())>0?"-"+Eval("Reduced").ToString():"" %></span>
                    //    </td>
                    //    <td><%#Eval("TradeDate")%>
                    //    </td>
                    //    <td><%#Eval("TradeType").ToString()=="0"? "兑换优惠券":Eval("TradeType").ToString()=="1"?"兑换礼品":Eval("TradeType").ToString()=="2"?"购物奖励":Eval("TradeType").ToString()=="3"?"退款扣积分":""%>
                    //    </td>
                    //    <td><%#Eval("Remark") %>
                    //    </td>
                    //</tr>
                    var tempIncreased = "";
                    if (userpoints[i].Increased > 0) {
                        tempIncreased = "+" + userpoints[i].Increased;
                    } else if (userpoints[i].Reduced > 0) {
                        tempIncreased = "-" + userpoints[i].Reduced;
                    }

                    var tempTrade = "";
                    if (userpoints[i].TradeType == 0) {
                        tempTrade = "兑换优惠券";
                    } else if (userpoints[i].TradeType == 1) {
                        tempTrade = "兑换礼品";
                    } else if (userpoints[i].TradeType == 2) {
                        tempTrade = "购物奖励";
                    } else if (userpoints[i].TradeType == 3) {
                        tempTrade = "退款扣积分";
                    }

                    str += '<tr>';
                    str += '<td>';
                    str += '<span class="green-lb">' + tempIncreased + '</span>';
                    str += '</td>';
                    str += '<td>' + userpoints[i].TradeDate + '</td>';
                    str += '<td>' + tempTrade + '</td>';
                    str += '<td>' + userpoints[i].Remark + '</td>';
                    str += '</tr>';
                    //str += '<li>';
                    //str += '<div class="gd-box fix">';
                    //str += '<a href="/Vshop/ProductDetails.aspx?ProductId=' + products[i].ProductId + '">';
                    //str += '<div class="gd-img">';
                    //str += '<img class="lazyload" src="/resource/default/image/lazy.png" data-original="' + products[i].ThumbnailUrl220 + '" style="border-width:0px;">';
                    //str += products[i].ActivityId ? "<div class='promoteImg'>促</div>" : "";
                    //str += '</div>';
                    //str += '<div class="gd-info">';
                    //str += '<p class="gd-name">' + products[i].ProductName + '</p>';
                    //str += '<p class="gd-price">';
                    //if (products[i].SalePrice != null && products[i].SalePrice != undefined && products[i].SalePrice != "") {
                    //    str += '¥<span>' + products[i].SalePrice.toFixed(2) + '</span>';
                    //}
                    //str += '</p>';
                    //str += '<p class="gd-rate">税率：' + products[i].TaxRate * 100 + '%</p>';
                    //str += '<p class="gd-sales">已售' + products[i].SaleCounts + '</p>';
                    //str += '<div class="dis-none">';
                    //str += '<p class="gd-sales">' + products[i].ShortDescription + '</p>';
                    //str += '</div>';
                    //str += '</div>';
                    //str += '</a>';
                    //str += '<a class="fast-add show-skus" title="加入购物车" fastbuy_skuid="' + products[i].fastbuy_skuid + '" smallimg="' + products[i].ThumbnailUrl60 + '" productid="' + products[i].ProductId + '" price="' + products[i].SalePrice.toFixed(2) + '">';
                    //str += '</a>';
                    //str += '</div>';
                    //str += '</li>';
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
        action: "LoadMoreUserPoints",
        pageNumber: pageNumber,
        size: size
    };
    return postData;
}