<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
    <div class="ct-box">
        <div class="ct-img"> <a href='<%# Globals.ApplicationPath + "/VShop/ProductReview.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("SkuId") %> '>
            <Hi:ListImage runat="server" DataField="ThumbnailsUrl" /></a> </div>
        <div class="specification">
            <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
            <input name="promotionName" type="hidden" value="<%#Convert.ToString(Eval("PromotionName"))%>" />
              <input name="ss" type="hidden" value="<%# Eval("PromoteType") %>" />
            <input name="promotionShortName" type="hidden" value="<%# PromotionHelper.GetShortName((PromoteType)Eval("PromoteType")) %>" />
        </div>
        <div class="ct-info"> <a href="<%# Globals.ApplicationPath + "/VShop/ProductReview.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("SkuId") %> ">
            <div class="ct-name"><%# Eval("ItemDescription")%></div>
            <div class="fix"><span class="ct-price">¥<span><%# Eval("ItemAdjustedPrice", "{0:F2}")%></span></span><span class="ct-qua">X <%# Eval("Quantity")%></span></div>
            <div class="fix"><span class="ct-rate">税率：<%# Eval("TaxRate")%></span></div>            
            </a> </div>
    </div>
</li>
