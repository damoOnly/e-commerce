<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li>
              <div class="category_pro_pic"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
            <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl220" CustomToolTip="ProductName" />
        </Hi:ProductDetailsLink></div>
              <div class="category_pro_name"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
              <div class="bars">
                  <div class="category_pro_price"><p><Hi:RankPriceName ID="RankPriceName1" runat="server" PriceName="" />￥<strong><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"  Money='<%# Eval("RankPrice") %>' runat="server" /></strong></p><span <%#"style=\"display:"+(string.IsNullOrEmpty(Eval("MarketPrice").ToString())?"none":"block")+";\""%>>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></span></div>
                  <a class="addcartButton cart-btn" productid="3492" price="0.10" fastbuy_skuid="3492_0">加入购物车</a>
              </div>
</li>
