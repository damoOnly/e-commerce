<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %> ">
    <div class="well goods-box">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
        <div class="info">
            <div class="name font-xl">
                <%# Eval("ProductName") %></div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription") %></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}") %><span class="sales font-s text-muted">已售<%#Eval("ShowSaleCounts")%>件</span></div>
        </div>
    </div>
</a>