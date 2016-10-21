<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<dl>
<dt><img src="<%#Eval("IconUrl") %>" /><span><%#Eval("Name") %></span></dt>
<dd>
<ul class="help-submenu">
<asp:Repeater ID="rptHelp" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Category") %>'>
       <ItemTemplate>
	   <li>  <a href='<%#  ResolveUrl("/helpItem.aspx?helpid="+Eval("HelpId")) %>' id=<%#"help"+Eval("HelpId") %>><%#Eval("Title") %></a></li>
      </ItemTemplate>
    </asp:Repeater>
		</ul>
     </dd>
</dl>