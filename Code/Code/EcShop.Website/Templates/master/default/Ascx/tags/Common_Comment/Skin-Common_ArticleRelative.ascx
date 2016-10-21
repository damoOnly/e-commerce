<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<li>
    <div class="article_relativeimg">
    <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
           <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl160" CustomToolTip="ProductName" />
    </Hi:ProductDetailsLink>
      </div>
    <div class="article_relative_txt"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
</li>

      

 
