<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
  <tr id="spqingdan_title" class="ddgl">
    <td width="70px" align="center">商品信息</td>
    <td width="60px" align="center">货号</td>
    <td width="242px" align="center">商品名称</td>
    <td width="120px" align="center">订单编号</td>
    <td width="72px" align="center">购买数量</td>
    <td width="72px" align="center">商品单价</td>
    <td  align="center">商品数量</td>
  </tr>
<asp:Repeater ID="dataListOrderItems" runat="server">
         <itemtemplate>
            <tr>
                <td>
                    <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                        <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl"/>
                    </Hi:ProductDetailsLink>                            
                </td>
                <td>
                    <span class="tl"><asp:Literal ID="litSKU" runat="server" Text='<%# Eval("SKU")+"&nbsp;" %>'></asp:Literal></span></td>
                <td>
                	<p class="tl">
                        <Hi:ProductDetailsLink ID="productNavigationDetails"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' runat="server"/>
                        <br />
                        <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                        <br />
                        <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                                Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                        </p>
                </td>
                <td>
                	<p class="tl"><%# Eval("OrderId") %></p>
                	<p class="tl"><Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server"/></p>
                </td>
                <td>
                    <asp:Literal ID="lblProductQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
                <td>
                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel" runat="server"  Money='<%# Eval("ItemListPrice") %>' />                 
                </td>
                <td>
                    <asp:Literal ID="lblShipQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:Literal></td>
            </tr>
     </itemtemplate>
    </asp:Repeater>
</table>