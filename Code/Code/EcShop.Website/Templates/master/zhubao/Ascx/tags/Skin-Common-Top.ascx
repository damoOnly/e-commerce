﻿<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %> <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> <html xmlns="http://www.w3.org/1999/xhtml"> <head><meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /><meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" /><meta name="renderer" content="webkit"/><Hi:HeadContainer ID="HeadContainer1" runat="server" /> <Hi:PageTitle runat="server" /><Hi:MetaTags runat="server" /><Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" /><Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css"></Hi:TemplateStyle>
<Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
</head>
<body>
<div class="top_nav_bg">
    <div class="top_nav">
    <div class="top_link">
    <div class="login_re"><Hi:Common_CurrentUser runat="server" ID="lblCurrentUser"/> <Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" /> <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /></div>
    <div class="link_list">
    <span><Hi:SiteUrl ID="SiteUrl1" UrlName="productunslaes" runat="server">下架区</Hi:SiteUrl></span>
    <span>|</span>
 
    <span><Hi:SiteUrl ID="SiteUrl3" UrlName="LeaveComments" runat="server">商城交流区</Hi:SiteUrl></span>
    <span>|</span>
    <span><Hi:SiteUrl ID="SiteUrl4" UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl></span>
    </div>
    </div>

    </div>
</div>
