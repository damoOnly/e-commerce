<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
 <li class="article_list">
     <em style="float:right; margin-right:10px;"><Hi:FormatedTimeLabel runat="server" Time='<% #Eval("AddedDate") %>'  ID="time"/></em>
    <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails",Eval("AfficheId"))%>' target="_blank"  Title='<%#Eval("Title") %>'>
        <Hi:SubStringLabel ID="SubStringLabel" Field="Title" StrLength="14" runat="server"  />
    </a>   
</li>
