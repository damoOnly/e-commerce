<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="btn-group fix"><a class="welfare" href="/user/UserPoints.aspx">����</a><a class="order" href="/user/UserOrders.aspx
">������ѯ</a></div><Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server">
<div class="cart-btn">���ﳵ<b id="cart-num"><asp:Literal ID="cartNum" runat="server" Text="0"/></b>��<b style=" display:none;">
<Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" /></b></div></Hi:SiteUrl>
