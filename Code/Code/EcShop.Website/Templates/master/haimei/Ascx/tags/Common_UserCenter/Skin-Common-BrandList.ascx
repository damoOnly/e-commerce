<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="sw-con">
    <div class="sw-item selected">
        <div class="brand-list">
            <ul class="fix">
                <asp:Repeater ID="rp_Brandguest" runat="server">
                    <itemtemplate>
                        <li>
                            <%--<Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("BrandName") %>' ProductId='<%# Eval("BrandId") %>' ImageLink="true">
                                        <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="Logo" CustomToolTip="BrandName" />
                                </Hi:ProductDetailsLink>--%>
                            <a href='brand/brand_detail-<%#Eval("BrandId") %>.aspx'><img src='<%# Eval("Logo") %>' class="lazyload"/></a> </li>
                    </itemtemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>  
</div>
