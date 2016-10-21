<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<asp:Repeater ID="rptsuppliercouponlist" runat="server">
    <ItemTemplate>
        <div class="ad-img">
             <a href='/ActiveProductList.aspx?topic=<%#Eval("TopicId") %>'>
                <img src=<%#Eval("PCListImageUrl") %>>
            </a>
		</div>
    </ItemTemplate>
</asp:Repeater>

