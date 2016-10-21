<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=5.0,minimum-scale=0.5, user-scalable=yes">
<meta name="apple-mobile-web-app-capable" content="yes">
<meta name="apple-mobile-web-app-status-bar-style" content="black">
<meta name="format-detection" content="telephone=no">
<meta http-equiv="Expires" content="-1">
<meta http-equiv="Cache-Control" content="no-cache">
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /><meta name="renderer" content="webkit"/><Hi:HeadContainer ID="HeadContainer1" runat="server" /> <Hi:PageTitle runat="server" /><Hi:MetaTags runat="server" />
<Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    
    
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css?v=20150803"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
      <script type="text/javascript">
       //如果是网站首页直接跳转到WAP端首页，如果是其它页面，且该页面对应WAP端有相应的页面对直接中转到对应的WAP页面，如果没有则不跳转
        var pageurl = document.location.href.toLowerCase();
        var sUserAgent = navigator.userAgent.toLowerCase();
        var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
        var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
        var bIsMidp = sUserAgent.match(/midp/i) == "midp";
        var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
        var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
        var bIsAndroid = sUserAgent.match(/android/i) == "android";
        var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
        var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
        //bIsWM = true;
        if ((/*bIsIpad || */bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) && HasWapRight) {
            //解决分享到微信时，浏览器跳转到网站首页的问题
            DirectUrl = GetWapUrl();
            if (DirectUrl != ""){
                //location.href = "/Wapshop/" + DirectUrl;
			}
        }
        ///判断是否需要跳转，如果当前页面为首页或者帮助页，文单页则跳转，其它的不需要跳转
        function IsDirect() {

            if (pageurl.indexOf("/default.aspx") > -1) return true;
            if (pageurl.indexOf("/article/") >= -1 && pageurl.indexOf("/help/") >= -1) return false;
            return true;
        }
        ///获取PC端对应Wap端的页面地址，如果没有对应地址则返回空
        function GetWapUrl() {
            var PageKey = "";
            var port = document.location.port;
            var domain = document.domain;
            var param = document.location.search;
            if (port != "80" && port != "") domain = domain + ":" + port;
            domain = "http://" + domain;
            if ((pageurl.length == domain.length || (pageurl.length - 1) == domain.length) && pageurl.indexOf(domain) == 0) { return "default.aspx"; }
            if (pageurl.indexOf("default.aspx") > -1) { return "default.aspx" + param; }
            if (pageurl.indexOf("product_detail-") > -1) { PageKey = GetPageKey("product_detail-", false); return "ProductDetails.aspx?ProductId=" + PageKey; }
            if (pageurl.indexOf("productdetails.aspx") > -1) { return "ProductDetails.aspx" + param; }
            if (pageurl.indexOf("brand_detail-") > -1) { PageKey = GetPageKey("brand_detail-", false); return "BrandDetail.aspx?BrandId=" + PageKey; }
            if (pageurl.indexOf("brand.aspx") > -1) { return "brandlist.aspx" + param;; }
            if (pageurl.indexOf("category.aspx") > -1) { return "productsearch.aspx" + param; }
            if (pageurl.indexOf("browse/category-") > -1) { PageKey = GetPageKey("browse/category-", false); return "ProductList.aspx?categoryId=" + PageKey; }
            if (pageurl.indexOf("subcategory.aspx?keywords=") > -1) { PageKey = GetPageKey("keywords="); return "ProductList.aspx?keyWord=" + PageKey; }
            if (pageurl.indexOf("groupbuyproducts.aspx") > -1) { return "GroupBuyList.aspx" + param;; }
            if (pageurl.indexOf("groupbuyproduct_detail-") > -1) { PageKey = GetPageKey("groupbuyproduct_detail-"); return "GroupBuyProductDetails.aspx?groupbuyId=" + PageKey; }
            if (pageurl.indexOf("login.aspx") > -1) { return "login.aspx" + param; }
            if (pageurl.indexOf("register.aspx") > -1) { return "login.aspx" + param + (param == "" ? "?" : "&") + "&action=register"; }
            if (pageurl.indexOf("userdefault.aspx") > -1) { return "MemberCenter.aspx" + param; }
            if (pageurl.indexOf("userorders.aspx") > -1) { return "MemberOrders.aspx" + param; }
            if (pageurl.indexOf("usershippingaddresses.aspx") > -1) { return "ShippingAddresses.aspx" + param; }
            if (pageurl.indexOf("userprofile.aspx") > -1) { return "UserInfo.aspx" + param; }
            if (pageurl.indexOf("/user/") > -1) { return "MemberCenter.aspx"; }
            if (pageurl.indexOf("articles.aspx") > -1) { return "Articles.aspx" + param; }
            if (pageurl.indexOf("article/show-") > -1) { PageKey = GetPageKey("article/show-"); return "ArticleDetails.aspx?ArticleId=" + PageKey; }
            return "";
        }


        function GetPageKey(pagepre, IsEnd) {
            if (pageurl.indexOf(pagepre) > -1) {
                var endIndex = 0;
                if (!IsEnd) {
                    endIndex = pageurl.indexOf(".aspx");
                    if (endIndex <= -1) endIndex = pageurl.indexOf(".htm")
                }
                else {
                    endIndex = pageurl.length;
                }
                var startIndex = pageurl.indexOf(pagepre) + pagepre.length;
                if (startIndex >= 0 && endIndex > startIndex)
                    return pageurl.substring(startIndex, endIndex);
            }
            return "0";
        }

    </script>
     <script type="text/javascript">
         var _adwq = _adwq || [];
         _adwq.push(['_setAccount', 'ybq8i']);
         _adwq.push(['_setDomainName', '.haimylife.com']);
         _adwq.push(['_trackPageview']);
         (function () {
             var adw = document.createElement('script');
             adw.type = 'text/javascript';
             adw.async = true;
             adw.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://s') + '.emarbox.com/js/adw.js';
             var s = document.getElementsByTagName('script')[0];
             s.parentNode.insertBefore(adw, s);
         })();
     </script> 
</head>
<body>
<div class="login-header">
    <input id="hiid_AdUserId" clientidmode="Static" runat="server" type="hidden" />
    <div class="header-shop">
        <p><a href="/">返回首页</a><span class="splite">|</span><a href="/Helps.aspx">帮助中心</a></p>
        <!--<p><label>咨询热线：</label><span class="hot-tel">400-663-9898</span></p>-->
    </div>
    <a class="logo" href="/"><img src="/templates/master/haimei/images/logo.png" title="海美生活" alt="海美生活"/></a>
</div>