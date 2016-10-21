<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li>
    <div class="gd-box fix" style="position: relative;">
    	<span style="position: absolute;left: -1px;top:-1px; display: block;z-index:10;">
         <img src="/templates/vshop/haimei/images/hot_vshop.png" width="30">   
        </span>
    	<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>" target="_self">
            <div class="gd-img"><Hi:ListImage runat="server" DataField="ThumbnailUrl220" /><%# int.Parse(Eval("ActivityId").ToString()) > 0 ? "<div class='gd-promote'>促</div>":"" %></div>
            <div class="gd-info">
                
                <p class="gd-price">￥<span><%# Eval("SalePrice", "{0:F2}") %></span><s><%# !string.IsNullOrEmpty(Eval("MarketPrice").ToString()) ? (Decimal.Parse(Eval("MarketPrice").ToString()) > 0?Decimal.Parse(Eval("MarketPrice").ToString()).ToString("C"):""):"" %></s></p>
                <p class="gd-name"><%# Eval("ProductName") %></p>
                <!--<p class="gd-rate">税率：<%# Eval("TaxRate", "{0:0%}")%></p>-->
                <div class="dis-none">
                    <p class="gd-sales">已售<%# Eval("SaleCounts")%>件</p>
                    <p class="gd-sales"><%# Eval("ShortDescription")%></p>
                </div>
            </div>
        </a><!--<a class="fav-btn"></a>--><a class="cart-btn fast-add show-skus" title="加入购物车" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>" productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>"></a></div>
</li>



