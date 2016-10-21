<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Panel ID="pnlShopProductCart" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <th width="120" align="center">商品图片
            </th>
            <th width="350" align="center">商品名称
            </th>
            <th width="120" align="center">商品单价
            </th>
            <th width="160" align="center">购买数量
            </th>
            <th width="160" align="center">发货数量
            </th>
            <th width="120" align="center" class="brn">小计
            </th>
        </tr>
        <asp:Repeater ID="dataListShoppingCrat" runat="server">
            <ItemTemplate>
                <tr>
                    <td height="40" align="center">
                        <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>'
                            ProductName='<%# Eval("Name")%>' runat="server" ImageLink="true">
                   <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl60" runat="server" />
                        </Hi:ProductDetailsLink>
                    </td>
                    <td align="left" style="padding: 10px">
                        <div class="name">
                            <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" ImageLink="false" />
                        </div>
                        <div style="color: #999;">
                            <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                        </div>
                    </td>
                    <td align="center" class="cart_Order_price">
                        <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" runat="server" Money='<%# Eval("MemberPrice")%>' /></span>
                    </td>
                    <td align="center" class="item_quantity">
                        <asp:Literal runat="server" ID="txtStock" Text='<%# Eval("Quantity")%>' />
                        <div>
                            <asp:Literal ID="litGiveQuantity" Text='<%# (int)Eval("Quantity")==(int)Eval("ShippQuantity")?"":"赠送："+((int)Eval("ShippQuantity")-(int)Eval("Quantity")) %>'
                                runat="server" />
                        </div>
                        <!--Eval("Quantity")-->
                        <!--<%# Eval("ShippQuantity") %>-->
                        <!--<%# Eval("Quantity") %>-->
                    </td>
                    <td align="center">
                        <%# Eval("ShippQuantity")%>
                    </td>
                    <td align="center" class="cart_Order_price brn">
                        <input type="hidden" class="item_taxrate" value='<%#Eval("TaxRate")%>'>
                        <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" runat="server" Money='<%# Eval("SubNewTotal")%>' /></span>
                        <Hi:ProductPromote runat="server" ProductId='<%# Eval("ProductId") %>' IsShoppingCart="true" IsAnonymous="true" />
                    </td>
                </tr>
                <%--<tr>
                    <td align="center">
                        <img src="Templates/master/haimei/images/gift_pro.png" style="border-width: 0px;" />
                        <span style="width: 30px; height: 30px; margin-left: 12px;">
                            <a href="#" target="_blank">
                                <img src="http://www.haimylife.com/Storage/master/product/thumbs60/60_e43e2466712e41129def716316c51468.jpg" style="border-width: 0px;"></a>
                        </span>
                    </td>
                    <td align="left" style="padding: 10px">
                        <div class="cart_commodit_name">
                            <a href="#" target="_blank">泰国MAMA冬阴功味鲜辣方便面60G*12杯装（1箱）</a>
                        </div>
                    </td>
                    <td align="center">
                        <span>￥<span>0.00</span></span>
                    </td>
                    <td align="center">1</td>
                    <td align="center">1</td>
                    <td align="center" class="cart_Order_price brn"><span>￥<span>0.00</span></span></td>
                </tr>--%>
                <input type="hidden" id="promotionProductId" runat="server" value='<%# Eval("ProductId")%>' />
                <asp:Repeater ID="dtPresendPro" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <img src="Templates/master/haimei/images/gift_pro.png" style="border-width: 0px;" />
                                <span style="width: 30px; height: 30px; margin-left: 12px;">
                                <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>'
                                    ProductName='<%# Eval("ProductName")%>' runat="server" ImageLink="true">
                   <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl40" runat="server" />
                                </Hi:ProductDetailsLink>
                                    </span>
                            </td>
                            <td align="left" style="padding: 10px">
                                <div class="cart_commodit_name">
                                    <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("ProductName")%>' runat="server" ImageLink="false" />
                                </div>
                            </td>
                            <td align="center">
                                <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" runat="server" Money='<%# Eval("ItemAdjustedPrice")%>' /></span>
                            </td>
                            <td align="center">
                                <%# Eval("DiscountValue")%>
                            </td>
                            <td align="center">
                                <%# Eval("DiscountValue")%>
                            </td>
                            <td align="center" class="cart_Order_price brn">
                                <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" runat="server" Money='<%# Eval("ItemAdjustedPrice")%>' /></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
