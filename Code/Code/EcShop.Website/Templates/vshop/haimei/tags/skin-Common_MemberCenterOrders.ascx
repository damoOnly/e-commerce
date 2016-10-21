<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<li>
    <div class="odbox">
        <a class="odbox-head fix" href="<%# Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + Eval("OrderId") %>">
            <label class="odbox-date">下单时间：<span><%# Convert.ToDateTime(Eval("OrderDate")).ToString("yyyy-MM-dd HH:mm:ss")%></span><!--</label><label class="odbox-ser">订单编号：<span><%# Eval("OrderId")%></span>--></label>
            <label class="fr odbox-order">订单详情</label>
        </a>
        <div class="odbox-con">
            <div class="odbox-gds fix">
                <asp:Repeater ID="Repeater2" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                    <itemtemplate>
                    <div class="odbox-list">
                        <a style="position: absolute;" href="<%# Globals.ApplicationPath + "/vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                        	<Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                        </a>
                        <div class="odbox-detail">
                            <p>商品名称： <%# Eval("ItemDescription") %></p>
                            <p class="color666">价格： <%# Eval("ItemAdjustedPrice","{0:F2}")%></p>
                            <p class="color888">数量：<%# Eval("Quantity") %></p>
                        </div>
                    </div>
                    </itemtemplate>
            	</asp:Repeater>
            </div>
            <div class="odbox-info fix" style="overflow:hidden;">                
                <div class="odbox-summer fl">
                    <div class="odbox-tpc">
                    	<p>
                            <label>总价：</label>
                            <span class="odbox-price">￥<%# Eval("OrderTotal","{0:F2}")%></span>
                            （<label>运费:</label>
                            <span><%# Eval("AdjustedFreight","{0:F2}")%>元</span>）
                        </p>
                    </div>
                    <div class="odbox-status">
                     <Hi:OrderStatusLabel ID="OrderStatusLabel1" OrderStatusCode='<%# Eval("OrderStatus") %>' IsRefund='<%# Eval("IsRefund") %>' OrderGateway='<%# Eval("Gateway") %>'
                    runat="server" />
                    </div>
                </div>
                <div class="odbox-op fix fr">
                	<a href='javascript:void(0)' onclick='FinishOrder("<%#Eval("OrderId") %>")' class='ms-btn finish <%# (int)Eval("OrderStatus") == 3 ? "" : "hide"%>'>
                    确认收货</a> <a href='<%# Globals.ApplicationPath + "/Vshop/MyLogistics.aspx?OrderId=" + Eval("OrderId") %> '
                        class='gr-btn <%# ((int)Eval("OrderStatus") == 3 || (int)Eval("OrderStatus") == 5) ? "" : "hide"%>'>
                        查看物流</a>
                    <%--<a href='<%# Globals.ApplicationPath + "/Vshop/CustomerService.aspx?Action=Refund&OrderId=" + Eval("OrderId") %> '
                        class='<%# ((int)Eval("OrderStatus") == 2 || (int)Eval("OrderStatus") == 3) ? "" : "hide"%>'>
                        退款</a>
                        <a href='<%# Globals.ApplicationPath + "/Vshop/CustomerService.aspx?Action=Return&OrderId=" + Eval("OrderId") %> '
                        class='<%# (int)Eval("OrderStatus") == 5 ? "" : "hide"%>'>
                        退货</a>
                        <a href='<%# Globals.ApplicationPath + "/Vshop/CustomerService.aspx?Action=Replace&OrderId=" + Eval("OrderId") %> '
                        class='<%# (int)Eval("OrderStatus") == 5 ? "" : "hide"%>'>
                        换货</a>--%>
                        <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderId") %> '
                         class='ms-btn <%# (int)Eval("OrderStatus") == 1&&(int)Eval("PaymentTypeId")!=0&&Eval("GateWay")!="hishop.plugins.payment.bankrequest"&&Eval("GateWay")!="hishop.plugins.payment.podrequest"? "" : "hide"%>'
                        >立即付款</a>
						<a href='javascript:void(0)' onclick='CancelOrder(<%#Eval("OrderId") %>)' 
                         class='<%# (int)Eval("OrderStatus") == 1&&(int)Eval("PaymentTypeId")!=0&&Eval("GateWay")!="hishop.plugins.payment.bankrequest"&&Eval("GateWay")!="hishop.plugins.payment.podrequest"? "" : "hide"%>'
                        >取消订单</a>
                      <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderId")+"&onlyHelp=true" %> '
                         class='<%# Eval("GateWay")!=DBNull.Value&&Eval("GateWay")=="hishop.plugins.payment.bankrequest"&&(int)Eval("OrderStatus")==1 ? "" : "hide"%>'
                        >线下支付帮助</a>
                        <a  href='javascript:void(0)' onclick="RefundOrder('<%#Eval("OrderId") %>','<%#Eval("SourceOrderId") %>')" class='<%#Eval("IsCancelOrder").ToString()=="1"&&Eval("IsRefund").ToString()=="0"? "" : "hide"%>'>
                            申请退款
                        </a>
                        <a  href='javascript:void(0)'  class='<%#Eval("IsRefund").ToString()=="2"? "" : "hide"%>'>
                            退款中
                        </a>
                        <a  href='javascript:void(0)'  class='<%#Eval("IsRefund").ToString()=="1"? "" : "hide"%>'>
                            退款完成
                        </a>
                </div>
            </div>
        </div>
    </div>
</li>

