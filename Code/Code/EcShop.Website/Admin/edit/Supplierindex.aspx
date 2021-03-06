﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Supplierindex.aspx.cs" Inherits="EcShop.UI.Web.Admin.edit.Supplierindex" %>

<script type="text/template" id="pop_template">
    <div class="popover left">
        <div class="arrow"></div>
        <div class="popover-inner popover-delete">
            <div class="popover-content text-center">
                <div class="form-inline">
                    <span class="help-inline item-delete">确定删除?</span>
                    <button type="button" class="btn btn-primary js-btn-confirm" data-loading-text="确定">确定</button>
                    <button type="reset" class="btn js-btn-cancel">取消</button>
                </div>
            </div>
        </div>
    </div>
</script>
<script type="text/template" id="add_field_template">
    <div class="app-add-field">
        <h4>添加内容</h4>
        <ul>
            <li><a class="js-new-field" data-field-type="nav_link">导航<br />
                按钮</a></li>
            <li><a class="js-new-field" data-field-type="goods">商品</a></li>
            <li><a class="js-new-field" data-field-type="image_ad">图片<br>
                广告</a> </li>
            <li><a class="js-new-field" data-field-type="title">标题</a></li>

        </ul>
    </div>
</script>
<div class="page-layout fix">
    <div class="app-design fix">
        <div class="app-preview">
            <div class="app-header"></div>
            <div class="app-entry">
                <div class="app-config js-config-region">
                    <div class="app-field clearfix editing">
                        <h1><span>[页面标题]</span></h1>
                    </div>
                </div>
                <div class="app-fields js-fields-region">
                    <div id="app-fields" class="app-fields ui-sortable">
                        <link rel="stylesheet" type="text/css" href="js/dist/css/bootstrap.min.css" />
                        <link rel="stylesheet" type="text/css" href="resource/default/css/core/basic.css" />
                        <link rel="stylesheet" type="text/css" href="resource/default/css/core/base.css" />
                        <link rel="stylesheet" type="text/css" href="resource/default/css/lib/swiper.min.css">
                        <link rel="stylesheet" type="text/css" href="resource/default/css/lib/page.css">
                        <link rel="stylesheet" type="text/css" href="resource/default/css/ui/app.css">
                        <script src="js/lib/jquery-1.10.2.js"></script>
                        <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
                        <script src="js/lib/head.js"></script>
                        <script src="js/lib/swipe.js"></script>
                        <script src="js/lib/page.js"></script>
                        <script src="/Utility/common.js" type="text/javascript"> </script>
                        <script src="/Templates/vshop/haimei/script/jquery.lazyload.min.js"></script>
                        <script src="/Templates/vshop/haimei/script/main.js"></script>
                        <script src="/Utility/vproduct.helper.js" type="text/javascript"> </script>
                        <script src="/Templates/vshop/haimei/script/addcart.js" type="text/javascript"></script>
                        <header>
                            <a class="list-btn" href="/Vshop/ProductSearch.aspx"></a>
                            <div class="site-btn">
                                <span id="site">深圳</span>
                                <div class="drops" id="drops">
                                </div>
                                <input id="Hidd_SiteId" type="hidden" value="-1" />
                            </div>
                            <form action="/Vshop/ProductList.aspx">
                                <div class="search-box">
                                    <div class="btn-bar">
                                        <a class="msg-btn">消息</a>
                                    </div>
                                    <div class="search-input">
                                        <a class="qcs" id="qrcode-btn"></a>
                                        <a class="search-btn" id="search-btn"></a>
                                        <div class="input-con">
                                            <input id="txtKeywords" type="search" name="keyWord" placeholder="搜索您想要的商品" />
                                            <input type="hidden" name="categoryId" />
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </header>
                        <footer class="sku-wrap" id="sku-wrap">
                            <section class="sku-con" id="sku-con">
                                <div class="sku-inner">

                                    <div class="sku-info clearfix">
                                        <img id="control_item_info_img" width="50" height="50" src="resource/default/image/lazy.png">
                                        <span class="price">¥<span id="spSalaPrice">0.00</span></span>
                                        <div class="sku-close">&nbsp;</div>
                                    </div>
                                    <div class="control-wrap">
                                        <input type="hidden" runat="server" id="hidden_skus" clientidmode="Static" />
                                        <div class="specification-box">
                                            <div class="skus-info"></div>
                                            <div class="buy-num">
                                                <div class="text-muted">购买数量</div>
                                                <div class="list clearfix">
                                                    <div class="goods-num clearfix">
                                                        <div class="n-options clearfix">
                                                            <a class="op-btn reduce" id="spSub"><span></span></a><a class="op-btn plus" id="spAdd"><span></span></a>
                                                            <div class="input-con">
                                                                <input type="tel" id="buyNum" class="form-control" value="1">
                                                            </div>
                                                        </div>
                                                        <div class="info font-s text-muted">（剩余可购买数量：<span id="spStock">3000</span>件）</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="control-btns">
                                    <a class="btn btn-danger" id="control-ok">确定</a>
                                </div>
                            </section>
                        </footer>
                        <footer id="footer" class="cfooter">
                            <a href="/Vshop/" class="selected"><span><i class="glyphicon glyphicon-home"></i>
                                <label>首页</label>
                            </span></a><a href="/Vshop/ProductSearch.aspx"><span><i class="glyphicon glyphicon-th-list"></i>
                                <label>分类</label>
                            </span></a><a href="/Vshop/ShoppingCart.aspx"><span><i class="glyphicon glyphicon-shopping-cart"><em class="cf-count" id="cart-num"></em></i>
                                <label>购物车</label>
                            </span></a><a href="/Vshop/MemberCenter.aspx"><span><i class="glyphicon glyphicon-user"><em class="cf-count" id="wait-pay-num"></em></i>
                                <label>我的海美</label>
                            </span></a>
                            <div class="scrolltop glyphicon glyphicon-chevron-up" id="scrolltop"></div>
                        </footer>
                        <script type="text/javascript">
                            function createSwiper(cid) {
                                var mySwiper = new Swiper('#swiper-' + cid, {
                                    autoplay: 5000,
                                    speed: 200,
                                    width: window.innerWidth,
                                    autoplayDisableOnInteraction: false,
                                    visibilityFullFit: true,
                                    loop: true,
                                    onClick: function (swiper) {
                                        var url = $(swiper.clickedSlide).attr("data-src");
                                        if (url) {
                                            window.location = url;
                                        }
                                    },
                                    pagination: $('#swiper-' + cid).next('.slider-bar'),
                                    paginationClickable: true
                                });
                            }
                        </script>
                    </div>
                </div>
            </div>
            <div class="js-add-region">
                <div></div>
            </div>
        </div>
        <div class="app-sidebar">
            <div class="arrow"></div>
            <div class="app-sidebar-inner js-sidebar-region">
                <div></div>
            </div>
        </div>
        <div class="app-actions" style="display: block; bottom: 0px;">
            <div class="form-actions text-center">
                <input class="btn btn-primary btn-save" type="button" value="保存" data-loading-text="保存..." id="save-btn">
                <input class="btn btn-success" type="button" id="review-btn" value="预览" />
                <input class="btn btn-activate" type="button" id="activate-btn" value="启用" />
            </div>
        </div>
    </div>
    <div class="review-tip">
        <h3>预览</h3>
    </div>
    <div class="review">
        <iframe name="review" src="review.html" width="100%" height="100%"></iframe>
    </div>
</div>
<div id="pop-con"></div>

<!-- Button trigger modal -->
<!--<button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#goodModal"> Launch demo modal </button>-->
<!-- Modal -->
<div class="modal" id="testModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">素材库</h4>
            </div>
            <div class="modal-body">
                <div class="content">
                    <div id="main">
                        <!--<div class="search-wrap clearfix mb10"><div class="search-con fr"><form><button class="btn fr">搜索</button><input type="text" class="search"/></form></div><h3 class="lh30">请选择图片</h3></div>
                <div class="ui-box">
                	<ul class="js-list-body-region widget-image-list clearfix">
                    	<li class="widget-image-item"><div class="js-choose" title="细节图.jpg">                            
                            <div class="widget-image-item-content" style="background-image: url(http://imgqn.koudaitong.com/upload_files/2015/04/17/FrP-CDvOR1RHnq0vh_4mkqdv5vxt.jpg!100x100.jpg)"></div>                            
                            <div class="widget-image-meta">
                                640x8340
                            </div>                            
                            <div class="selected-style"><i class="icon-ok icon-white"></i></div>
                        </div>
                        </li>
                        <li class="widget-image-item"><div class="js-choose" title="细节图.jpg">                            
                            <div class="widget-image-item-content" style="background-image: url(http://imgqn.koudaitong.com/upload_files/2015/04/17/FrP-CDvOR1RHnq0vh_4mkqdv5vxt.jpg!100x100.jpg)"></div>                            
                            <div class="widget-image-meta">
                                640x8340
                            </div>                            
                            <div class="selected-style"><i class="icon-ok icon-white"></i></div>
                        </div>
                        </li>
                        <li class="widget-image-item"><div class="js-choose" title="细节图.jpg">                            
                            <div class="widget-image-item-content" style="background-image: url(http://imgqn.koudaitong.com/upload_files/2015/04/17/FrP-CDvOR1RHnq0vh_4mkqdv5vxt.jpg!100x100.jpg)"></div>                            
                            <div class="widget-image-meta">
                                640x8340
                            </div>                            
                            <div class="selected-style"><i class="icon-ok icon-white"></i></div>
                        </div>
                        </li>
                    </ul>
                </div>
                <div class="page-wrap"></div>-->
                        <div class="search-wrap clearfix mb10">
                            <!--<div class="search-con fr"><button class="btn fr" id="test-search">搜索</button><input type="text" class="search" id="test-search-input"/></div>-->
                            <h3 class="lh30">请选择</h3>
                        </div>
                        <div class="ui-box">
                            <table class="table" id="btable">
                                <thead>
                                    <tr>
                                        <th style="width: 15%;">
                                            <div class="td-cont"><span>图片</span> </div>
                                        </th>
                                        <th style="width: 30%;">
                                            <div class="td-cont"><span>描述</span> </div>
                                        </th>
                                        <th style="width: 35%;">
                                            <div class="td-cont"><span>链接</span> </div>
                                        </th>
                                        <th class="opts">操作按钮 </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="title">
                                            <div class="td-cont"><a target="_blank" class="new_window clearfix" href="http://wap.koudaitong.com/v2/showcase/goods?alias=19hzmplz2"><span class="img">
                                                <img src="http://imgqn.koudaitong.com/upload_files/2015/04/17/FuwJYvmA3kWBM421vZw-gMIqV1dT.jpg" /></span></a> </div>
                                        </td>
                                        <td><a target="_blank" class="new_window clearfix" href="http://wap.koudaitong.com/v2/showcase/goods?alias=19hzmplz2"><span>美国童年时光钙镁锌</span></a></td>
                                        <td class="time">
                                            <div class="td-cont"><span>http://www.baidu.com</span> </div>
                                        </td>
                                        <td class="opts">
                                            <div class="td-cont">
                                                <button class="btn js-choose" href="javascript:void(0);">选取</button>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="page-wrap"></div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                <button type="button" class="btn btn-primary" id="testModal-ok" data-dismiss="modal">确定</button>
            </div>
        </div>
    </div>
</div>
<div class="modal" id="proModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">商品库</h4>
            </div>
            <div class="modal-body">
                <div class="content">
                    <div id="goodmain">
                        <div class="search-wrap clearfix mb10">
                            <div class="search-con fr">
                                <button class="btn fr" id="proModal-search">搜索</button>
                                <input type="text" class="search" id="proModal-search-input" placeholder="搜索您想要的商品" />
                            </div>
                            <h3 class="lh30">请选择商品</h3>
                        </div>
                        <div class="ui-box">
                            <table class="table" id="table">
                                <thead>
                                    <tr>
                                        <th class="title">
                                            <div class="td-cont"><span>商品</span> </div>
                                        </th>
                                        <th class="time">
                                            <div class="td-cont"><span>创建时间</span> </div>
                                        </th>
                                        <th class="opts">操作按钮 </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="title">
                                            <div class="td-cont"><a target="_blank" class="new_window clearfix" href="http://wap.koudaitong.com/v2/showcase/goods?alias=19hzmplz2"><span class="img">
                                                <img src="http://imgqn.koudaitong.com/upload_files/2015/04/17/FuwJYvmA3kWBM421vZw-gMIqV1dT.jpg" /></span><span class="good-name">美国童年时光钙镁锌</span></a> </div>
                                        </td>
                                        <td class="time">
                                            <div class="td-cont">
                                                <span>2015-04-17<br>
                                                    16:52:03</span>
                                            </div>
                                        </td>
                                        <td class="opts">
                                            <div class="td-cont">
                                                <button class="btn js-choose" href="javascript:void(0);">选取</button>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="page-wrap"></div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                <button type="button" class="btn btn-primary" id="proModal-ok" data-dismiss="modal">确定</button>
            </div>
        </div>
    </div>
</div>
<script src="js/dist/js/bootstrap.min.js"></script>
<script src="js/lib/underscore.js"></script>
<script src="js/lib/backbone.js"></script>
<script src="js/bussiness/index.js"></script>
<script type="text/javascript">
    var flag = 0;
    $(function () {
        $("body").addClass("w320");
        //点击保存按钮
        $("#save-btn").on("click", function () {
            var $appFields = $("#app-fields");
            $appFields.find(".editing").removeClass("editing");
            $appFields.find(".actions,.sort").remove();


            formatView(viewList);
            //setMyCookie("viewList",viewList);
            SaveDesignTemplate(viewList);

            $("body").removeClass("w320").addClass("w640").html($appFields.html());
        })
        //点击预览按钮
        $("#review-btn").on("click", function () {
            var $appFields = $("#app-fields");
            var html = $appFields.html();
            var $review = $('.review').html("");
            var iframe = $('<iframe src="review.html" width="100%"  height="100%"></iframe>').appendTo($review);
            iframe.height($appFields.outerHeight(true) + 55);
            iframe.on("load", function () {
                $(this)[0].contentWindow.postMessage({ html: html }, '*');
            });
        })
        $("#activate-btn").on("click", function () {
            var $appFields = $("#app-fields");
            $appFields.find(".editing").removeClass("editing");
            $appFields.find(".actions,.sort").remove();

            ActivateDesignTemplate();
        });
    })
    function ActivateDesignTemplate() {
        $.ajax({
            url: "/API/VshopDesignHandler.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "ActivateDesignTemplate", flag: flag, strHtml: $('#app-fields').html() },
            async: false,
            success: function (resultData) {
                if (resultData.success == 1) {
                    alert("操作成功");
                    if (flag == 0) {
                        flag = 1;
                        $('#activate-btn').val('取消启用');
                    } else {
                        $('#activate-btn').val('启用');
                        flag = 0;
                    }
                }
            }
        });
    }
    function SaveDesignTemplate(viewList) {
        $.ajax({
            url: "/API/VshopDesignHandler.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "SaveDesignTemplate", viewList: JSON.stringify(viewList), strHtml: $('#app-fields').html()},
            async: false,
            success: function (resultData) {
                if (resultData.success == 1) {
                    alert("保存成功");
                }
            }
        });
    }

</script>

