<%@ Control Language="C#"%>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<table class="tab_sales">
<tr class="tr_head">
<td>买家</td><td>数量</td><td>付款时间</td><td>款式和型号</td>
</tr>
<asp:Repeater ID="rp_productsales" runat="server">
<ItemTemplate>
<tr>
<td><%#Eval("Username") %></td>
<td><%#Eval("Quantity") %></td>
<td><%#Eval("PayDate")%></td>
<td><%# Eval("SKUContent") %>&nbsp;</td>
</tr>
</ItemTemplate>
</asp:Repeater>
</table>