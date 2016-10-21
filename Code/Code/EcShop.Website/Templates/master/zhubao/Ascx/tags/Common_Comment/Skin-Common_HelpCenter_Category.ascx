<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<h4><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("Helps",Eval("CategoryId"))%>'><%#Eval("Name")%></a></h4>