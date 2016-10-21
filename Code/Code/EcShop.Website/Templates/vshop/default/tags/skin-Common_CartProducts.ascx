<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="well goods-box goods-box-shopcart">
    <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl60" /></a>
    <div class="info">
        <div class="name font-xl bcolor">
            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                <%# Eval("Name")%></a></div>
        <div class="specification">
            <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
            <input name="promotionName" type="hidden" value="<%#Convert.ToString(Eval("PromotionName"))%>" />
            <input name="promotionShortName" type="hidden" value="<%# PromotionHelper.GetShortName((PromoteType)Eval("PromoteType")) %>" />
        </div>
		<div class="taxRate">
            适用税率：<%#Eval("TaxRate")%>%
        </div>
        <div class="price text-danger">
            ¥<%# Eval("AdjustedPrice", "{0:F2}")%>
        </div>
    </div>
    <a href="javascript:void(0)" name="iDelete" skuid='<%# Eval("SkuId")%>'><span class="glyphicon glyphicon-remove-circle move">
    </span></a>
    <div class="goods-num">
        <div name="spSub" class="shopcart-add">
            <span class="glyphicon glyphicon-minus-sign"></span>
        </div>
        <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
            skuid='<%# Eval("SkuId")%>' />
        <div name="spAdd" class="shopcart-minus">
            <span class="glyphicon glyphicon-plus-sign"></span>
        </div>
    </div>
</div>
