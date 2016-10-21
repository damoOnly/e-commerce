<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li>
    <div class="gd-box fix">
        <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
            <%--<div class="prolist-discount"><em>5.5折</em></div>--%>
            <%# (bool.Parse(Eval("IsDisplayDiscount").ToString())&& Eval("MarketPrice").ToString()!="" && decimal.Parse(Eval("MarketPrice").ToString())>0)  ? "<div class='prolist-discount'><em>"+((decimal.Parse(Eval("SalePrice").ToString())/decimal.Parse(Eval("MarketPrice").ToString()))*10).ToString("F2")+"折</em></div>":"" %>
            <div class="gd-img">
                <img src="resource/default/image/lazy.png" data-original="<%# Eval("ThumbnailUrl220") %>" class="lazyload" />
                <%--<%# (int.Parse(Eval("ActivityId").ToString()) > 0 && bool.Parse(Eval("IsPromotion").ToString())) ? "<div class='gd-promote'>促</div>":"" %>--%>
            </div>
            <div class="gd-info">
                <p class="gd-name"><%# Eval("ProductName") %></p>
                <div class="gd-price-wrap">
                    <p class="gd-price">&yen;<span><%# Eval("SalePrice", "{0:F2}") %></span></p>
                    <p class="gd-rate as">&yen;<s><%# Eval("MarketPrice", "{0:F2}")%></s></p>
                </div>
                <%--<p class="gd-rate">税率：<%# Eval("TaxRate", "{0:0%}")%></p>--%>
                <p class="gd-sales dis-none">已售<%# Eval("SaleCounts")%>件</p>
                <div class="dis-none">
                    <p class="gd-sales"><%# Eval("ShortDescription")%></p>
                </div>
                <div class="shop-place"></div>
            </div>
        </a>

        <a class="gd-shop" href="/vshop/SupProductList.aspx?SupplierId=<%# Eval("SupplierId") %>" target="_blank">
            <img src='<%# Eval("Icon")%>' style="height:18px;"/><%# string.IsNullOrWhiteSpace(Eval("ShopName").ToString())?Eval("SupplierName"):Eval("ShopName") %></a>
        <a class='<%# int.Parse(Eval("Stock").ToString()) > 0 ? "fast-add show-skus cart-btn":"fast-add show-skus  cart-btn-disable"%>' title="加入购物车" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>" productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>"></a>
    </div>
</li>
