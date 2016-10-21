<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
订单号：<span><%# Eval("OrderId")%></span> 
 
    发货单号：<span><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("ShipOrderNumber") %>'>'></asp:Literal></span>
 
    配送方式：<span><asp:Literal ID="Literal2" runat="server" Text='<%# Eval("RealModeName") %>'></asp:Literal></span>&nbsp;&nbsp;&nbsp;&nbsp;