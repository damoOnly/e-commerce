<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
 <li><a href="/vshop/ProductList.aspx?keyWord=<%#Eval("Keywords") %>"><%#Eval("Keywords") %></a></li>

