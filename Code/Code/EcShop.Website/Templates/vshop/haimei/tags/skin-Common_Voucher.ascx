<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<div class="alert alert-success alert-dismissable">
    <div class="name font-xl">
        <%# Eval("Name") %>（满<%# Eval("Amount", "{0:F2}")%>可以使用<%# Eval("DiscountValue", "{0:F2}")%>）</div>
    <div class="date">
        有效期：<%# Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd") %>至<%# Convert.ToDateTime(Eval("ClosingTime")).ToString("yyyy-MM-dd") %></div>
</div>
