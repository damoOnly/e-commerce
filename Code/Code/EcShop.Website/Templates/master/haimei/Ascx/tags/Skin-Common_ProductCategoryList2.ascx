<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
	 <b><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a></b>
    <span><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'>È«²¿</a></span> 
   <em>  <asp:Repeater ID="rptSubCategries" runat="server">
        <ItemTemplate>
		    <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>|
        </ItemTemplate>
    </asp:Repeater></em>
    
</li>
 