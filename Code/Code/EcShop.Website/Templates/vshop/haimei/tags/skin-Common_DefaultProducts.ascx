<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

    
    <li>
    <div class="gd-box fix"><a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>" target="_self">
        <div class="gd-img"><img src="resource/default/image/lazy.png" data-original="<%# Eval("ThumbnailUrl220") %>" class="lazyload" /> <%# int.Parse(Eval("ActivityId").ToString()) > 0 ? "<div class='gd-promote'>促</div>":"" %></div>
        <div class="gd-info">
            <p class="gd-name"><%# Eval("ProductName") %></p>
            <p class="gd-price">￥<span><%# Eval("SalePrice", "{0:F2}") %></span></p>
            <p class="gd-rate">税率：<%# Eval("TaxRate", "{0:0%}")%></p>
            <div class="dis-none">
                <p class="gd-sales">已售<%# Eval("ShowSaleCounts")%>件</p>
                <p class="gd-sales"><%# Eval("ShortDescription")%></p>
            </div>
        </div>
        </a><!--<a class="fav-btn"></a>--><a class="cart-btn fast-add show-skus" title="加入购物车" smallImg="<%# Eval("ThumbnailUrl40") %>" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>" productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>"></a></div>
</li>
