<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>
<table cellspacing="0" border="0" >
    <tr id="spqingdan_title">
        <th class="content_table_title" width="60px">商品图片</th>
         <th class="content_table_title" width="130px">货号</th>
        <th class="content_table_title" width="350px">商品名称</th>
        <th class="content_table_title" width="290px">评论</th>
         <asp:Repeater ID="rp_orderItem" runat="server">
         <ItemTemplate>
         <tr class="ddgl_1">
                <td align="center">
                <input type="hidden" runat="server" id="hdproductId" value=<%# Eval("ProductId")+"&"+Eval("SKU")+"&"+Eval("SKUid")%>/>
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
                </td>
                <td align="center">
                    <textarea id="txtcontent" rows="3" cols="33" runat="server"></textarea>
                </td>
            </tr>
         </ItemTemplate>
         </asp:Repeater>
    </tr>
</table>