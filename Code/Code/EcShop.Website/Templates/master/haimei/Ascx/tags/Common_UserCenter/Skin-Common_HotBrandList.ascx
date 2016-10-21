<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<asp:Repeater ID="rpHotBrandlist" runat="server">
    <ItemTemplate>
        <li>
            <a href='brand/brand_detail-<%#Eval("BrandId") %>.aspx' target="_blank">
                <img src='<%# Eval("BigLogo") %>'  /></a> 
        </li>
    </ItemTemplate>
</asp:Repeater>
