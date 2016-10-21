<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<asp:Repeater ID="rptsuppliercouponlist" runat="server">
    <ItemTemplate>       
      
        <li>
            <a href='/ActiveProductList.aspx?topic=<%#Eval("TopicId") %>'>
                <img src=<%#Eval("PCListImageUrl") %>>
            </a>
        </li>
    </ItemTemplate>
</asp:Repeater>

