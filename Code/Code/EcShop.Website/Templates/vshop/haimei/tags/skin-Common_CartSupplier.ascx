<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.UI.SaleSystem.CodeBehind.Common" %>
<%@ Import Namespace="EcShop.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>

<li>
    <div class="ct-wrap">
        <div class="ct-shop fix">
            <label class="ck-label">
                <input type="checkbox" type="checkbox" class="checkbox" value='<%# Eval("SupplierId")%>' />
                <input type="hidden" id="hidsupplierId" runat="server" value='<%# Eval("SupplierId")%>' />
            </label>
            <a class="shop-name" href='/vshop/SupplierDefault.aspx?SupplierId=<%# Eval("SupplierId")%>'>
                <img src='<%#Eval("SupplierImageUrl") %>' /><%# Eval("SupplierName")%></a>

        </div>
    </div>
    <ul>
                    <hi:vshoptemplatedrepeater id="rptCartProducts" templatefile="/Tags/skin-Common_CartProducts.ascx"
                                               runat="server" />
                </ul>
</li>
