<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>



<li>
    <div class="ct-box">
        <div class="ct-img">
            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
            </a>
        </div>
        <div class="ct-info">
            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                <div class="ct-name"><%# Eval("Name")%></div>
                <div class="fix"><span class="ct-price">¥<span><%# Eval("AdjustedPrice", "{0:F2}")%></span></span><span class="ct-qua">X <%# Eval("Quantity")%></span></div>
                <div class="fix"><span class="ct-rate">税率：<%# Eval("ComTaxRate", "{0:0}")%></span></div>
                <div class="ct-rate"><%# Eval("SkuContent")%></div>
            </a>
        </div>
    </div>
    <input type="hidden" id="promotionProductId" runat="server" value='<%# Eval("ProductId")%>' />
    <ul class="fix">
        <asp:Repeater ID="dtPresendPro" runat="server">
            <ItemTemplate>
                <li>
                    <div class="ct-box">
                        <div class="ct-img">
                            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                                <Hi:ListImage runat="server" DataField="ThumbnailUrl40" />
                            </a>
                        </div>
                        <div class="ct-info">
                            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                                <div class="ct-name"><%# Eval("ProductName")%></div>
                                <div class="fix"><span class="ct-price">¥<span><%# Eval("ItemAdjustedPrice", "{0:F2}")%></span></span><span class="ct-qua">X <%# Eval("DiscountValue")%></span></div>
                                <%--<div class="fix"><span class="ct-rate">税率：<%# Eval("TaxRate", "{0:0%}")%></span></div>--%>
                                <div class="ct-rate"><%# Eval("SkuContent")%></div>
                            </a>
                        </div>
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</li>

