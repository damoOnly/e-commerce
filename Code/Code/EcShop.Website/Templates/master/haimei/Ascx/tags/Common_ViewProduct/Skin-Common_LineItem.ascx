<%@ Control Language="C#"%>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>

<tr>
<td><%#Eval("Username") %></td>
<td><%#Eval("Quantity") %></td>
<td><%#Eval("PayDate")%></td>
<td><%# Eval("SKUContent") %>&nbsp;</td>
</tr>
