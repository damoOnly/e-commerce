<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server">
<img src="/Templates/master/default/images/users/hyzx_13.jpg" width="54" height="23" />
<div>购物袋中有<b><asp:Literal ID="cartNum" runat="server" Text="0" /></b> 件商品 合计<b>
<Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" /></b>元 </div></Hi:SiteUrl>
 