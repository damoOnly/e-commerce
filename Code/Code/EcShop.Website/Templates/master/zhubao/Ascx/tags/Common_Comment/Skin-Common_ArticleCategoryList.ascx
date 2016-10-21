<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<h4><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("Articles",Eval("CategoryId"))%>'><Hi:SubStringLabel ID="lblCategoryName" StrLength="10" Field="Name" runat="server" /></a></h4>