<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server">
<div>¹ºÎï´ü<b><asp:Literal ID="cartNum" runat="server" Text="0" /></b> ¼þ <b style=" display:none;">
<Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" /></b> </div></Hi:SiteUrl>
 