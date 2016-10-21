<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle ID="PageTitle1" runat="server" />
    <Hi:MetaTags ID="MetaTags1" runat="server" />
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/user.css"></Hi:TemplateStyle>
    <link href="/Utility/validate/pagevalidator.css" rel="stylesheet" type="text/css" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <script type="text/javascript">
        function searchs() {
            var item = $("#drop_Search_Class").val();
            var key = $("#txt_Search_Keywords").val();
            if (key == undefined)
                key = "";

            key = key.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace('/', '');

            var url = applicationPath + "/SubCategory.aspx?keywords=" + encodeURIComponent(key);
            if (item != undefined)
                url += "&categoryId=" + item;
            window.location.href = url;
        }

        $(document).ready(function () {
            $('#txt_Search_Keywords').keydown(function (e) {
                if (e.keyCode == 13) {
                    searchs();
                    return false;
                }
            })
        });
	  
  </script>
</head>
<body>

    <div class="hyzxheader">
        <div class="hyzxheadertop">
            <div class="hyzxheadertopc">
                <div class="hyzxheadertopcl">
                    <Hi:Common_CurrentUser runat="server" ID="lblCurrentUser" />&nbsp;&nbsp;
                    [<Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" Text="会员中心" />&nbsp;&nbsp;&nbsp;
                     <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" />]</div>
                <div class="hyzxheadertopcr">
                    <Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" runat="server">我的购物车</Hi:SiteUrl>|
                    <Hi:SiteUrl ID="SiteUrl2" UrlName="LeaveComments" runat="server">商城交流区</Hi:SiteUrl>|
                    <Hi:SiteUrl ID="SiteUrl4" UrlName="Category" runat="server">全部分类</Hi:SiteUrl>|
                    <Hi:SiteUrl ID="SiteUrl5" UrlName="AllArticles" runat="server">商城资讯</Hi:SiteUrl>
                </div>
            </div>
        </div>
        <div class="hyzxheadersea">
            <div class="hyzxheaderseal">
                <Hi:Common_Logo ID="Common_Logo1" runat="server"/>
            </div>
            <div class="hyzxheadersear">
                <div class="hyzxheadersear1">
                    <input class="hyzxinput" id="txt_Search_Keywords" type="text" /><input class="hyzxbutton" onclick="searchs()" type="button" /></div>
                <div class="hyzxheadersear2">
                    <em>热门关键字</em><Hi:Common_SubjectKeyword ID="Common_SubjectKeyword1" runat="server" CommentId="4" /></div>
            </div>
        </div>
        <div class="hyzxheadermeun">
            <div class="hyzxheadermeunc">
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl6" UrlName="user_UserDefault" runat="server" CssClass="zhanghu">我的账户</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl7" UrlName="home" runat="server">商城首页</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl8" UrlName="user_UserOrders" runat="server">订单查询</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl9" UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl></li>
                </ul>
                <div class="hyzxshopping">
                    <Hi:Common_ShoppingCart_Info ID="Common_ShoppingCart_Info1" runat="server"/></div>
            </div>
        </div>
    </div>
