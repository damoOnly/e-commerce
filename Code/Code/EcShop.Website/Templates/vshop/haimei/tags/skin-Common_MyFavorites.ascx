<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<!--<div class="well goods-box">
    <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
        <div class="info">
            <div class="name font-xl bcolor">
                <%# Eval("ProductName")%></div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription")%></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}")%>                
            </div>
        </div>
    </a><a href="javascript:void(0)" onclick="Submit('<%# Eval("FavoriteId")%>')"><span
        class="glyphicon glyphicon-remove-circle move"></span></a>
</div>-->
<li>    
    <div class="ct-box">
        <div class="ct-img">
            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
            </a>
        </div>
        <div class="ct-info">
            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                <div class="ct-name"><%# Eval("ProductName")%></div>                
                <div class="fix"><span class="ct-price">¥<span><%# Eval("SalePrice", "{0:F2}")%></span></span></div>
                
               
            </a>
            <div class="goods-num fix">
                <div class="info"><a class="del-btn" href="javascript:void(0)" onclick="Submit('<%# Eval("FavoriteId")%>')"></a></div>
                
            </div>
        </div>
    </div>
</li>