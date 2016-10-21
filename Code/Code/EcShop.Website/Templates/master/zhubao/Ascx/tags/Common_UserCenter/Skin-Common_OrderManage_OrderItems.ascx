<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
  <tr id="spqingdan_title">
    <td width="9%" align="center">商品信息</td>
    <td width="8%" align="center">货号</td>
    <td width="47%" align="center">商品名称</td>
    <td width="12%" align="center">购买数量</td>
    <td width="12%" align="center">商品单价</td>
    <td width="12%" align="center">发货数量</td>
  </tr>
<asp:Repeater ID="dataListOrderItems" runat="server">
         <ItemTemplate>
            <tr>
                <td align="center">
                    <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                        <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl"/>
                    </Hi:ProductDetailsLink>                            
                </td>
                <td align="center">
                    <asp:Literal ID="litSKU" runat="server" Text='<%# Eval("SKU")+"&nbsp;" %>'></asp:Literal></td>
                <td align="center">
                <Hi:ProductDetailsLink ID="productNavigationDetails"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' runat="server"/>
                <br />
                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                <br />
                <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                        Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                </td>
                <td align="center">
                    <asp:Literal ID="lblProductQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel" runat="server"  Money='<%# Eval("ItemListPrice") %>' />                 
                </td>
                <td align="center">
                    <asp:Literal ID="lblShipQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:Literal></td>
            </tr>
     </ItemTemplate>
    </asp:Repeater>
</table>