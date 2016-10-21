<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

    <div class="pos-rel">
		<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
        <div>
        <div class="p-img">
            <Hi:ListImage runat="server" DataField="ThumbnailUrl220" />
            <%# int.Parse(Eval("ActivityId").ToString()) > 0 ? "<div class='promoteImg'>促</div>":"" %>
        </div>
            <div class="info">
                <div class="name bcolor"><%# Eval("ProductName") %></div>
                <div class="price font-s text-danger">
                   ¥<%# Eval("SalePrice", "{0:F2}") %>
                </div>
                <div class="rate text-muted font-xs">
                    税率：<%# Eval("TaxRate", "{0:0%}")%>

                </div>
                <%--<div class="sales text-muted font-xs">已售<%# Eval("SaleCounts")%>件</div>--%>
               <%-- <div class="sales text-muted font-xs"><%# Eval("ShortDescription")%></div>--%>
            </div>
        </div>
		</a>
		<a class="fast-add show-skus" title="加入购物车" fastbuy_skuid="<%# Eval("fastbuy_skuid") %>" productid="<%#Eval("ProductId") %>" price="<%# Eval("SalePrice", "{0:F2}") %>"><span class="glyphicon shopping-cart-add"></span></a>
    </div>



