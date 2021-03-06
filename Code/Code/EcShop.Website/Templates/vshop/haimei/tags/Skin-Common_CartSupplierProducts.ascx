﻿<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>

<asp:Repeater ID="repeaterSupplier" runat="server">
    <ItemTemplate>
        <li>
            <div class="ct-wrap">
                <div class="ct-shop fix">
                    <label class="ck-label">
                        <input type="checkbox" type="checkbox" class="checkbox" value='<%# Eval("SupplierId")%>' />
                        <input type="hidden" id="hidsupplierId" runat="server" value='<%# Eval("SupplierId")%>' />
                    </label>
                    <a class="shop-name" href='/vshop/SupplierDefault.aspx?SupplierId=<%# Eval("SupplierId")%>'>
                        <img src='<%#Eval("SupplierLogo") %>' /><%# Eval("SupplierName")%></a>

                </div>
                <div class="fix">
                    <asp:Repeater ID="rpProduct" runat="server">
                        <ItemTemplate>
                            <ol>
                                <label class="ck-label">
                                    <input type="checkbox" type="checkbox" class="checkbox" name="goodid" productid='<%# Eval("ProductId")%>' skuid='<%# Eval("SkuId")%>' />
                                </label>
                                <div class="ct-box">
                                    <div class="ct-img">
                                        <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                                            <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
                                        </a>
                                    </div>
                                    <div class="ct-info">
                                        <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                                            <div class="ct-name"><%# Eval("Name")%></div>
                                            <div>
                                                <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
                                                <input name="promotionName" type="hidden" value="<%#Convert.ToString(Eval("PromotionName"))%>" />
                                                <input name="promotionShortName" type="hidden" value="<%# PromotionHelper.GetShortName((PromoteType)Eval("PromoteType")) %>" />
                                            </div>
                                            <div class="fix">
                                                <span class="ct-price">¥<span><%# Eval("AdjustedPrice", "{0:F2}")%></span></span>
                                                <!--<span class="ct-uprice ml10">原价：¥<%# Eval("MemberPrice", "{0:F2}")%></span>-->
                                            </div>
                                            <div class="ct-rate">
                                                <label>税率：</label>
                                                <span><%#Eval("ComTaxRate")%></span>
                                            </div>
                                        </a>
                                        <div class="goods-num fix">
                                            <div class="info"><a class="del-btn" skuid='<%# Eval("SkuId")%>' name="iDelete"></a></div>
                                            <div class="n-options fix">
                                                <a class="op-btn reduce" name="spSub"><span></span></a><a class="op-btn plus" name="spAdd"><span></span></a>
                                                <div class="input-con">
                                                    <input type="tel" class="buy-num" name="buyNum" value='<%# Eval("Quantity")%>' skuid='<%# Eval("SkuId")%>' />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ol>
                            <input type="hidden" id="promotionProductId" runat="server" value='<%# Eval("ProductId")%>' />
                            <asp:Repeater ID="dtPresendPro" runat="server">
                                <ItemTemplate>
                                    <ol>
                                        <label >
                                            
                                        </label>
                                        <div class="ct-box">
                                            <div class="ct-img">
                                                <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                                                    <Hi:ListImage runat="server" DataField="ThumbnailUrl40" />
                                                </a>
                                            </div>
                                            <div class="ct-info">
                                                <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                                                    <div class="ct-name"><%# Eval("ProductName")%></div>
                                                    <div class="fix">
                                                        <span class="ct-price">¥<span><%# Eval("ItemAdjustedPrice", "{0:F2}")%></span> x <%# Eval("DiscountValue")%></span>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>
                                    </ol>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
        </li>
    </ItemTemplate>
</asp:Repeater>
