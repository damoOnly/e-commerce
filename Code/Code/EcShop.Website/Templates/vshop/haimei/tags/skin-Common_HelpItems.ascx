<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<a href="<%# Globals.ApplicationPath + "/VShop/HelpItem.aspx?HelpId=" + Eval("HelpId") %>"><%#Eval("Title") %></a>