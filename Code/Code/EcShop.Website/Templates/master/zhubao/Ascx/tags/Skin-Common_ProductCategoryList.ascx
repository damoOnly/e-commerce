<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<h2><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%> </a></h2>
<div>
<asp:Repeater ID="rptSubCategries" runat="server" >
    <ItemTemplate>
    <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><asp:Literal ID="litName" runat="server" Text='<%# Eval("Name")%>'></asp:Literal></a>
    </ItemTemplate>
</asp:Repeater>
</div>
