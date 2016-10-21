<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%--<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">--%>
<div class="box">
    <div class="left">
        <a href='<%# Globals.ApplicationPath + "/VShop/ProductReview.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("SkuId") %> '>
            <Hi:ListImage runat="server" DataField="ThumbnailsUrl" /></a>
    </div>
    <div class="right">
        <div class="name bcolor">
            <a href='<%# Globals.ApplicationPath + "/VShop/ProductReview.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("SkuId") %> '>
                <%# Eval("ItemDescription")%></a></div>
        <div class="specification">
            <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
            <input name="promotionName" type="hidden" value="<%#Convert.ToString(Eval("PromotionName"))%>" />
              <input name="ss" type="hidden" value="<%# Eval("PromoteType") %>" />
            <input name="promotionShortName" type="hidden" value="<%# PromotionHelper.GetShortName((PromoteType)Eval("PromoteType")) %>" />
        </div>
        <div class="price text-danger">
            ¥<%# Eval("ItemAdjustedPrice", "{0:F2}")%><span class="bcolor"> x
                <%# Eval("Quantity")%></span></div>
    </div>
</div>
<hr class="hr" />
