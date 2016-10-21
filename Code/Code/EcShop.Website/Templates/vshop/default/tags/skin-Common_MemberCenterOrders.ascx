<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%--<a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderId") %> '
    class='<%# (int)Eval("OrderStatus") == 1 ? "btn btn-danger btn-xs" : "hide"%>'>去付款</a>
<a href='javascript:void(0)' onclick='FinishOrder(<%#Eval("OrderId") %>)' class='<%# (int)Eval("OrderStatus") == 3 ? "btn btn-danger btn-xs" : "hide"%>'>
    确认收货</a> <a href='<%# Globals.ApplicationPath + "/wapshop/MyLogistics.aspx?OrderId=" + Eval("OrderId") %> '
        class='<%# ((int)Eval("OrderStatus") == 3 || (int)Eval("OrderStatus") == 5) ? "btn btn-danger btn-xs" : "hide"%>'>
        查看物流</a>--%>
<div class="panel panel-default order-list">
    <div class="panel-heading">
        <h3 class="panel-title">
            <%# Eval("OrderId")%></h3>
        <span class="badge-h badge-danger order-amount">¥<%# Eval("OrderTotal","{0:F2}")%></span> <span class="date">
            <%# Convert.ToDateTime(Eval("OrderDate")).ToString("yyyy-MM-dd")%></span>
    </div>
    <div class="panel-body">
        <a href="<%# Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + Eval("OrderId") %>">
            <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                <ItemTemplate>
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                </ItemTemplate>
            </asp:Repeater>
        </a>
        <div class="info">
            <div class="status">
                <Hi:OrderStatusLabel ID="OrderStatusLabel1" OrderStatusCode='<%# Eval("OrderStatus") %>'
                    runat="server" /></div>
            <div class="action">
                <a href='javascript:void(0)' onclick='FinishOrder(<%#Eval("OrderId") %>)' class='btn btn-warning btn-xs <%# (int)Eval("OrderStatus") == 3 ? "btn btn-danger btn-xs" : "hide"%>'>
                    确认收货</a> <a href='<%# Globals.ApplicationPath + "/Vshop/MyLogistics.aspx?OrderId=" + Eval("OrderId") %> '
                        class='btn btn-warning btn-xs <%# ((int)Eval("OrderStatus") == 3 || (int)Eval("OrderStatus") == 5) ? "btn btn-warning btn-xs" : "hide"%>'>
                        查看物流</a>
                    <a href='<%# Globals.ApplicationPath + "/Vshop/CustomerService.aspx?Action=Refund&OrderId=" + Eval("OrderId") %> '
                        class='btn btn-warning btn-xs <%# ((int)Eval("OrderStatus") == 2 || (int)Eval("OrderStatus") == 3) ? "btn btn-warning btn-xs" : "hide"%>'>
                        退款</a>
                        <a href='<%# Globals.ApplicationPath + "/Vshop/CustomerService.aspx?Action=Return&OrderId=" + Eval("OrderId") %> '
                        class='btn btn-warning btn-xs <%# (int)Eval("OrderStatus") == 5 ? "btn btn-warning btn-xs" : "hide"%>'>
                        退货</a>
                        <a href='<%# Globals.ApplicationPath + "/Vshop/CustomerService.aspx?Action=Replace&OrderId=" + Eval("OrderId") %> '
                        class='btn btn-warning btn-xs <%# (int)Eval("OrderStatus") == 5 ? "btn btn-warning btn-xs" : "hide"%>'>
                        换货</a>
                        <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderId") %> '
                         class='btn btn-warning btn-xs <%# (int)Eval("OrderStatus") == 1&&(int)Eval("PaymentTypeId")!=0&&Eval("GateWay")!="hishop.plugins.payment.bankrequest"&&Eval("GateWay")!="hishop.plugins.payment.podrequest"? "btn btn-danger btn-xs" : "hide"%>'
                        >去付款</a>
						<a href='javascript:void(0)' onclick='CancelOrder(<%#Eval("OrderId") %>)' 
                         class='btn btn-warning btn-xs <%# (int)Eval("OrderStatus") == 1&&(int)Eval("PaymentTypeId")!=0&&Eval("GateWay")!="hishop.plugins.payment.bankrequest"&&Eval("GateWay")!="hishop.plugins.payment.podrequest"? "btn btn-warning btn-xs" : "hide"%>'
                        >取消</a>
                      <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderId")+"&onlyHelp=true" %> '
                         class='btn btn-warning btn-xs <%# Eval("GateWay")!=DBNull.Value&&Eval("GateWay")=="hishop.plugins.payment.bankrequest"&&(int)Eval("OrderStatus")==1 ? "btn btn-danger btn-xs" : "hide"%>'
                        >线下支付帮助</a>
            </div>
        </div>
    </div>
</div>
