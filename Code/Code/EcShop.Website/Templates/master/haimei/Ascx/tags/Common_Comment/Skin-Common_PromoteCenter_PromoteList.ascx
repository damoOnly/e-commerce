<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>    

 <li class="article_list">
     <em style="float:right; margin-right:10px;"><%#Eval("PromoteTypeName")%></em>
     <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails",Eval("ActivityId"))%>' ><%#Eval("Name") %></a> 
</li>
 