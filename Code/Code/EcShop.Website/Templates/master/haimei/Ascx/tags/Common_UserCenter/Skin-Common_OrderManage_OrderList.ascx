<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<script src="/Utility/expressInfo.js" type="text/javascript"></script>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable fixed">
	<thead>	
    <tr id="spqingdan_title" class="ol-title">
        <td align="center" class="c1">商品详细</td>
        <td align="center" nowrap="nowrap" class="c2">单价</td>
        <td align="center" class="c3">数量</td>
        <td align="center" class="c4">商品操作</td>
        <td align="center" class="c5">订单金额</td>
        <td align="center" class="c6">订单状态</td>
        <td align="center" class="c7">交易操作</td>
    </tr>
    </thead>
    
  <%--<table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="7">
                    <span>订单编号：<em class="order-ser">201507225988731</em></span> <span>下单日期：2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">删除</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>【2罐装】美国Gerber嘉宝 2段蜜桃苹果米粉 227g 婴幼儿辅食</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4">未付款</td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>等待买家付款</p>
                    <p><a>订单详情</a></p>
                </td>
                <td class="c7">
                	<a class="blue-btn red-btn">立即付款</a>
                    <p><a>取消订单</a></p>
                    <p><a>再次购买</a></p>
                </td>
             </tr>             
     </table>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>订单编号：<em class="order-ser">201507225988731</em></span> <span>下单日期：2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">删除</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>德国喜宝 有机早安水果谷物米糊 6＋ 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><a>退款/退货</a></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>卖家已发货</p>
                    <p><a>订单详情</a></p>
                    <p><a class="red-link">查看物流</a></p>
                </td>
                <td class="c7">
                	<a class="blue-btn">确认收货</a>
                </td>
             </tr>             
     </table>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>订单编号：<em class="order-ser">201507225988731</em></span> <span>下单日期：2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">删除</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>德国喜宝 有机早安水果谷物米糊 6＋ 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><a>退款/退货</a></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>等待发货</p>
                    <p><a>订单详情</a></p>                    
                </td>
                <td class="c7">
                	<a class="blue-btn gray-btn">提醒发货</a>
                </td>
             </tr>             
     </table>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>订单编号：<em class="order-ser">201507225988731</em></span> <span>下单日期：2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">删除</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>德国喜宝 有机早安水果谷物米糊 6＋ 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><a>退款/退货</a></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>交易完成</p>
                    <p><a>订单详情</a></p> 
                    <p><a>查看物流</a></p>                   
                </td>
                <td class="c7">
                	<a class="blue-btn gray-btn">评价商品</a>
                </td>
             </tr>             
     </table>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>订单编号：<em class="order-ser">201507225988731</em></span> <span>下单日期：2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">删除</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>德国喜宝 有机早安水果谷物米糊 6＋ 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><p class="red-link">已退款</p></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>交易关闭</p>
                    <p><a>订单详情</a></p> 
                    <p><a>查看物流</a></p>                   
                </td>
                <td class="c7">
                	<a class="agi-link">重新下单 </a>
                </td>
             </tr>             
     </table>--%>
<%-- <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>订单编号：<em class="order-ser">201507225988731</em></span> <span>下单日期：2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">删除</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1" colspan="3">
                	<table>
                      <tr>
                        <td><a class="glink">
                    	    <span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                            <span>德国喜宝 有机早安水果谷物米糊 6＋ 250g</span>
                            </a>
                        </td>
                        <td class="c2">112.00</td>
                        <td class="c3">1</td>
                       </tr>
                     </table>
                </td>
                <td class="c4"><a>退款/退货</a></td>
                <td rowspan="3" class="c5">301.00</td>
                <td rowspan="3" class="c6">
                	<p>卖家已发货</p>
                    <p><a>订单详情</a></p>
                    <p><a class="red-link">查看物流</a></p>
                </td>
                <td rowspan="3" class="c7">
                	<a class="blue-btn">确认收货</a>
                </td>
             </tr>
             <tr class="ddgl_1">
             	             	<td class="c1" colspan="3">
                	<table>
                      <tr>
                        <td><a class="glink">
                    	    <span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                            <span>德国喜宝 有机早安水果谷物米糊 6＋ 250g</span>
                            </a>
                        </td>
                        <td class="c2">112.00</td>
                        <td class="c3">1</td>
                       </tr>
                     </table>
                </td>
                <td class="c4"><a>退款/退货</a></td>
             </tr>
             <tr class="ddgl_1">
             	<td class="c1" colspan="3">
                	<table>
                      <tr>
                        <td><a class="glink">
                    	    <span class="img"><img title="商品图片" src="/templates/master/haimei/images/temp/g1.png"></span>
                            <span>德国喜宝 有机早安水果谷物米糊 6＋ 250g</span>
                            </a>
                        </td>
                        <td class="c2">112.00</td>
                        <td class="c3">1</td>
                       </tr>
                     </table>
                </td>
                <td class="c4"><a>退款/退货</a></td>
             </tr>
     </table>--%>
    <!--订单查询增加服务窗订单-->
    <asp:Repeater ID="listOrders" runat="server">
        <itemtemplate> 
        	<tbody>
            	<tr class="ol-blank"><td colspan="7"></td></tr>
            </tbody> 
        	 <tbody>
		        <tr class="ddgl">
			        <td colspan="7">
				        <span>订单编号：<em class="order-ser"><%# Eval("orderId")%></em></span><span>下单时间：<%# Eval("OrderDate")%></span></td>
			        <%--<a class="remove-btn">删除</a>--%>
<%--                     <td><asp:LinkButton runat="server" ID="lbtDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "orderId")%>'
                      CommandName="LogicDelete" Text="订单删除" OnClientClick="return confirm('确定要把订单移入回收站吗？')" CssClass="remove-btn"/>
			        </td>--%>
		        </tr>
		         <tr class="ddgl_1">
			        <td class="pd-td" colspan="3">
                        <asp:Repeater ID="rpProduct" runat="server">
                            <itemtemplate>
                            <table class="pd-table" style="width:100%;">
                                   <tr class="aa-<%# Eval("RowNumber") %>">
                                       <td class="pd1"><Hi:UserProductDetailsLink ID="UserProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                            ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                                                <span class="img"><img title='<%# Eval("ItemDescription") %>' src="<%# string.IsNullOrEmpty(Eval("ThumbnailsUrl").ToString())?Utils.ApplicationPath+HiContext.Current.SiteSettings.DefaultProductThumbnail2:Utils.ApplicationPath+Eval("ThumbnailsUrl").ToString().Replace("thumbs40/40","thumbs60/60") %>" />
                                                </span>
                                                <span><%# Eval("ItemDescription") %></span>
                                       </Hi:UserProductDetailsLink></td>

                                       <td class="pd2"><Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' /></td>
                                       <td class="pd3"><%# Eval("Quantity") %></td>
                                    </tr>
                                </table>
                                </itemtemplate>
                        </asp:Repeater>
			        </td>
                    <td class="c4">
                       <%--<a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server"
                            id="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>退款</a>
                        <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server"
                            id="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>退货</a>--%>
                        <a href="javascript:void(0)" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="lkbtnNotPay" visible="false">未付款</a>
                       <%-- <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server"
                            id="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>换货</a>--%>
                    </td>
			        <td class="c5">
                       <span>
                       <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>' runat="server" /></span>
			        </td>
			        <td class="c6">
				        <p>
                           <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' IsRefund='<%# Eval("IsRefund") %>' PayDate='<%# Eval("PayDate") %>' runat="server"/>
				        </p>
				        <p><asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>' Text="订单详情" /></p>
                        <p>
                           <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(<%# Eval("OrderId") %>)">查看物流</a></asp:Label>
                        </p>

			        </td>
			        <td class="c7">
                        <a href="javascript:void(0)" class="blue-btn red-btn" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="hlinkPay">立即付款</a>
				        <p><Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="取消" CommandArgument='<%# Eval("OrderId") %>'
                            CommandName="CLOSE_TRADE" DeleteMsg="确认取消该订单吗？" />
				        </p>

                        <p><%--<Hi:ImageLinkButton ID="lkbtnRefund" IsShow="true" runat="server" Text="申请退款"/>--%>
                            <asp:LinkButton ID="lkbtnRefund" runat="server"  Visible="false" CommandArgument='<%# Eval("OrderId") %>'   CommandName="CLOSE_Refund"  >申请退款</asp:LinkButton>
                            <asp:Label ID="lb_refundtitle" runat="server" Text="退款中" Visible="false"></asp:Label>
				        </p>
				       <%-- <p><a>再次购买</a></p>--%>
                       <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderReviews",Eval("orderId")) %>'>评论</asp:HyperLink>
                        <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="确认收货"
                            CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认已经收到货并完成该订单吗？"
                            Visible="false" ForeColor="Red" />

			        </td>
		         </tr>             
           </tbody>
        </itemtemplate>
    </asp:Repeater>
    </table>
    <asp:Panel ID="panl_nodata" runat="server" Visible="false">
    	<div class="em-con">    	
            <div class="message">
                        <div class="no-data fix">
                            <p>您还没有订单哦~</p>
                            <div class="g-btns">
                                <a class="g-btn" href="../../../ShoppingCart.aspx">去我的购物车</a><a class="g-btn red-btn" href="../../../Default.aspx">去首页逛逛</a>
                            </div>
                        </div> 
                    </div>
         </div>
    </asp:Panel>

<div id="myTab_Content1" class="none">
    <div id="spExpressData">
        正在加载中....
    在加载中....
    </div>
</div>
<script type="text/javascript">
    function GetLogisticsInformation(orderId) {
        $('#spExpressData').expressInfo(orderId, 'OrderId');
        ShowMessageDialog("物流详情", "Exprass", "myTab_Content1")
    }
    function IsRefund(orderid, SourceId) {
        var data = {};
        var msg = "您确定要申请退款吗？";
        if (SourceId != "")//拆单
        {
            msg = "同一付款订单：";
            $.ajax({
                url: "/api/VshopProcess.ashx?action=Getlistofchildren&SourceOrderId=" + SourceId,
                type: 'post', dataType: 'json', timeout: 10000,
                error: function (e) {
                    alert("网络异常，请确认网络是否连接！");
                },
                success: function (data) {
                    if (data.Success == -1) {
                        alert("退款异常，请稍后再试！");
                        return;
                    }
                    msg += data.data + "会同步取消，是否确定取消所有订单？";
                    rqsubmit(msg, orderid, SourceId);
                }
            });
        } else {
            rqsubmit(msg, orderid, SourceId);
        }

    }
    //执行退款
    function rqsubmit(msg, orderid, SourceId)
    {
        if (confirm(msg))
        {
            $.ajax({
                url: "/api/VshopProcess.ashx?action=RqRefundOrder&SourceOrderId=" + SourceId + "&orderid=" + orderid,
                type: 'post', dataType: 'json', timeout: 10000,
                error: function (e)
                {
                    location.replace(location);
                },
                success: function (data)
                {
                    if (data.success == "0")
                    {
                        alert('申请退款成功！');
                    }
                    else
                    {
                        alert("申请退款失败，请重试！");
                    }
                }

            });
        }
    }

</script>
