<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<a href="<%# Globals.ApplicationPath + "/VShop/CountDownProductsDetails.aspx?countDownId=" + Eval("CountDownId") %>">
    <div class="well goods-box">
    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl220" />
        <div class="info">
            <div class="name font-xl">
                <%# Eval("ProductName") %></div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription")%></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}") %><del class="font-s text-muted">¥<%# Eval("SalePrice", "{0:F2}") %></del>
                <span class="sales font-s text-muted">限购<%# Eval("MaxCount") == DBNull.Value ? 0 : Eval("MaxCount")%>件</span>
            </div>
        </div>
    </div>
</a>
