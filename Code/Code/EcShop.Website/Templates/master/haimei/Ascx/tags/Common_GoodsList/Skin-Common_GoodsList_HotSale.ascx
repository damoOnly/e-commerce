<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li class="fix">
    <div class="pro-img">
        <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>'
            ProductId='<%# Eval("ProductId") %>' ImageLink="true">
       <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl220" CustomToolTip="ProductName" />
        </Hi:ProductDetailsLink>
    </div>
    <div class="pro-info">
        <div class="pro-name">
            <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductName='<%# Eval("ProductName") %>'
                ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink>
        </div>
        <div class="pro-price"><strong><%# Decimal.Parse(!string.IsNullOrEmpty(Eval("SalePrice").ToString()) ? Eval("SalePrice").ToString():"0.00").ToString("C") %></strong><span><%# !string.IsNullOrEmpty(Eval("MarketPrice").ToString()) ? (Decimal.Parse(Eval("MarketPrice").ToString()) > 0?Decimal.Parse(Eval("MarketPrice").ToString()).ToString("C"):""):"" %></span></div>
    </div>
</li>
