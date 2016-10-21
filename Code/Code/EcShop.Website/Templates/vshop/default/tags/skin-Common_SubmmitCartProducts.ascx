<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="box">
    <div class="left">
        <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>"
            class="detailLink">
            <Hi:ListImage runat="server" DataField="ThumbnailUrl60" /></a>
    </div>
    <div class="right">
        <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>"
            class="detailLink">
            <div class="bcolor name">
                <%# Eval("Name")%>
            </div>
        </a>
        <div class="specification">
            <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
            <input name="promotionName" type="hidden" value="<%#Convert.ToString(Eval("PromotionName"))%>" />
            <input name="promotionShortName" type="hidden" value="<%# PromotionHelper.GetShortName((PromoteType)Eval("PromoteType")) %>" />
        </div>
        <div class="price text-danger">
            ¥<%# Eval("AdjustedPrice", "{0:F2}")%><span class="bcolor"> x
                <%# Eval("Quantity")%></span></div>
			<div class="item_taxrate" style="display:none"><%# Eval("TaxRate")%></div>
		<div class="item_quantity" style="display:none"><%# Eval("Quantity")%></div>
    </div>
</div>
<hr class="hr" />
