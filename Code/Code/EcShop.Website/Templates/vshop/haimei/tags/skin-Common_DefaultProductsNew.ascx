<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
    <div class="pd-box">
        <div class="pd-img"><a href="<%# Globals.ApplicationPath + "/vShop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>"><img src="/templates/wapshop/haidao/resource/default/image/lazy.png" data-original="<%#Eval("ThumbnailUrl220")%>" class="lazyload"/>
       </a></div>
        <div class="pd-info">
            <a href="<%# Globals.ApplicationPath + "/vShop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                <div class="pd-name"><%# Eval("ProductName") %></div>
                <div class="pd-price">￥<%# Eval("SalePrice", "{0:F2}") %></div>
                <div class="pd-rate"><label>税率：</label><span><%# Eval("TaxRate", "{0:0%}")%></span></div>
            </a>
            <a class="cart-btn" title="加入购物车" stock="<%# Eval("Stock") %>" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>" productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>"></a>
        </div>
    </div>
</li> 