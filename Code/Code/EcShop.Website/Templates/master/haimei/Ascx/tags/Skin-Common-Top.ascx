<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %> 
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
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /><Hi:HeadContainer ID="HeadContainer1" runat="server" /> <Hi:PageTitle runat="server" /><Hi:MetaTags runat="server" /><Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" /><Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css?v=201601071610"></Hi:TemplateStyle>
<Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css?v=20150819"></Hi:TemplateStyle>
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
<div class="top_nav_bg">
    <div class="top_nav">
    <div class="top_link">
    <div class="login_re"><Hi:Common_CurrentUser runat="server" ID="lblCurrentUser"/> <Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" /> <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /></div>
    <div class="link_list">
    <!--<span><Hi:SiteUrl ID="SiteUrl1" UrlName="productunslaes" runat="server">下架区</Hi:SiteUrl></span>
    <span>|</span>-->
 
    <span><Hi:SiteUrl ID="SiteUrl3" UrlName="LeaveComments" runat="server">商城交流区</Hi:SiteUrl></span>
    <span>|</span>
    <span><Hi:SiteUrl ID="SiteUrl4" UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl></span>
    </div>
    </div>
    </div>
     <input id="hiid_AdUserId" clientidmode="Static" runat="server" type="hidden" />
</div>
