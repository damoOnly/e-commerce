<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<span><a href='<%# Globals.ApplicationPath + "/Vshop/GroupBuyList.aspx?categoryId=" + Eval("CategoryId") %> '><%# Eval("Name") %></a></span>