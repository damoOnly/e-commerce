<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li>
              <div class="category_pro_pic"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
            <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl220" CustomToolTip="ProductName" />
        </Hi:ProductDetailsLink></div>
              <div class="category_pro_name"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
              <div class="category_pro_price"><p><Hi:RankPriceName ID="RankPriceName1" runat="server" PriceName="" />гд:<strong><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"  Money='<%# Eval("RankPrice") %>' runat="server" /></strong></p><span>гд:<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></span></div>
</li>
