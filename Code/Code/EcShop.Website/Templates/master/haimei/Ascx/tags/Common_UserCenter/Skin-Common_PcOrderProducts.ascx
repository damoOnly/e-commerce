<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls"%>
<%--<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">--%>
<div class="box">
	<input type="checkbox" class="checkbox" name="goodid" skuid="<%# Eval("SkuId")%>"/>
    <div class="left">
        <a href='<%# Globals.ApplicationPath + "/user/OrderDetails.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("sku") %> '>
            <Hi:ListImage runat="server" DataField="ThumbnailsUrl" /></a>
    </div>
    <div class="right">
        <div class="name bcolor">
            <a href='<%# Globals.ApplicationPath + "/user/OrderDetails.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("sku") %> '>
                <%# Eval("ItemDescription")%></a></div>
        <div class="specification">
            <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />


        </div>
        <div class="price text-danger">
            ¥<%# Eval("ItemAdjustedPrice", "{0:F2}")%><span class="bcolor"> x
                <%# Eval("Quantity")%></span></div>
        <div class="goods-choose">
    	<div class="goods-num">
                <div class="n-options clearfix" > <a class="op-btn reduce" name="spSub"><span></span></a> <a class="op-btn plus" name="spAdd"><span></span></a>
                    <div class="input-con">
                        <input type="tel" class="ui_textinput" readonly="true" name="buyNum" num='<%# Eval("Quantity")%>' value='<%# Eval("Quantity")%>' skuid="270_0">
                    </div>
                </div>
            </div>
    </div>
    </div>
    
</div>