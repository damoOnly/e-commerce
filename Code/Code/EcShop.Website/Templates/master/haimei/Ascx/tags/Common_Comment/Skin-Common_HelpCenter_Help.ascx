<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>
 <li class="article_list">
        <em style="float:right; margin-right:10px;"><Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("AddedDate") %>' runat="server" /></em>
        <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails",Eval("HelpId"))%>'><Hi:SubStringLabel ID="SubStringLabel1" Field="Title" runat="server" /></a>        
</li>


