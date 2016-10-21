<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="well goods-box">
    <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
        <div class="info">
            <div class="name font-xl bcolor">
                <%# Eval("ProductName")%></div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription")%></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}")%>
                <%--<span class="sales font-s text-muted">收藏时：¥5542.00</span>--%>
            </div>
        </div>
    </a><a href="javascript:void(0)" onclick="Submit('<%# Eval("FavoriteId")%>')"><span
        class="glyphicon glyphicon-remove-circle move"></span></a>
</div>
