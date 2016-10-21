<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls"%>



<li>
    <label class="ck-label">
        <input type="checkbox" type="checkbox" class="checkbox" name="goodid" skuid="<%# Eval("SkuId")%>" />
    </label>
    <div class="ct-box">
        <div class="ct-img">
            <a href="<%# Globals.ApplicationPath + "/VShop/ProductReview.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("sku") %> '">
                <Hi:ListImage runat="server" DataField="ThumbnailsUrl" />
            </a>
        </div>
        <div class="ct-info">
            <a href="<%# Globals.ApplicationPath + "/VShop/ProductReview.aspx?ProductId=" + Eval("ProductId") %>&OrderId=<%=Request.QueryString["OrderId"] %>&SkuId=<%# Eval("sku") %> '">
                <div class="ct-name"><%# Eval("ItemDescription")%></div>
                <div>
                    <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
                </div>
                <div class="fix"><span class="ct-price">¥<span><%# Eval("ItemAdjustedPrice", "{0:F2}")%></span></span><span class="ct-rate"><span class="ml5 mr5">X</span><%# Eval("Quantity")%></span> </div>
                
                <%--                <div class="ct-rate">
                    <label>适用税率：</label>
                    <span><%#(Eval("TaxRate")==null?0:(int)(decimal.Parse(Eval("TaxRate").ToString())*100))%>%</span>
                </div>--%>
            </a>
            <div class="goods-num fix">               
                <div class="n-options fix">
                    <a class="op-btn reduce" name="spSub"><span></span></a><a class="op-btn plus" name="spAdd"><span></span></a>
                    <div class="input-con">
                        <input type="tel" class="buy-num" readonly="true" name="buyNum" num='<%# Eval("Quantity")%>' value='<%# Eval("Quantity")%>' skuid='<%# Eval("SkuId")%>' />
                    </div>
                </div>
            </div>
        </div>
    </div>
</li>

