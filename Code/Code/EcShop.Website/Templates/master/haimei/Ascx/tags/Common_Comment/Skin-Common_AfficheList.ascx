<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>
	<li><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("AffichesDetails",Eval("AfficheId"))%>' target="_blank" Title='<%#Eval("Title") %>'>
<Hi:SubStringLabel ID="SubStringLabel" Field="Title" StrLength="14" runat="server"  /></a></li>
