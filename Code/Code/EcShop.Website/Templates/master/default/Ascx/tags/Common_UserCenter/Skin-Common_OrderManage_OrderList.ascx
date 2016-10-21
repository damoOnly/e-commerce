<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<script src="/Utility/expressInfo.js" type="text/javascript"></script>


<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td align="center">商品信息
        </td>
        <td align="center" nowrap="nowrap">收货人
        </td>
        <td align="center">订单金额
        </td>
        <td align="center">订单状态
        </td>
        <td align="center">操作
        </td>
    </tr>
    <!--订单查询增加服务窗订单-->
    <asp:Repeater ID="listOrders" runat="server">
        <ItemTemplate>
            <tr class="ddgl">
                <td colspan="5">
                    <span>订单编号：<em style="color: #2060af;"><%# Eval("OrderId") %></em></span> <span>下单日期：<%#Eval("OrderDate") %></span>
                </td>
            </tr>
            <tr class="ddgl_1">
                <td align="left" class="padd">
                    <asp:Repeater ID="rpProduct" runat="server">
                        <ItemTemplate>
                            <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                              
                                 <img title='<%# Eval("ItemDescription") %>' src="<%# string.IsNullOrEmpty(Eval("ThumbnailsUrl").ToString())?Utils.ApplicationPath+HiContext.Current.SiteSettings.DefaultProductThumbnail2:Utils.ApplicationPath+Eval("ThumbnailsUrl").ToString().Replace("thumbs40/40","thumbs60/60") %>" />
                            </Hi:ProductDetailsLink>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td align="center" nowrap="nowrap">
                    <%# Eval("ShipTo") %>
                </td>
                <td align="center" nowrap="nowrap">
                    <span>
                        <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>'
                            runat="server" /></span> <span>
                                <%# Eval("PaymentType") %></span>
                    <%# Eval("GateWay").ToString()=="ecdev.plugins.payment.bankrequest"&&Eval("OrderStatus").ToString()=="1"?"<span><a href=\"bank.aspx?OrderId="+Eval("OrderId")+"\" target=\"_blank\">线下付款</a></span>":"" %>
                </td>
                <td align="center" nowrap="nowrap">
                    <span class="fkzhuangtai">
                        <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>'
                            runat="server" /></span>
                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(<%# Eval("OrderId") %>)">物流跟踪</a></asp:Label>
                </td>
                <td align="center" nowrap="nowrap">
                    <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderReviews",Eval("orderId")) %>'>评论</asp:HyperLink>
                    <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>'
                        Text="查看" />
                    <a href="javascript:void(0)" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="hlinkPay">付款</a>
                    <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="确认订单"
                        CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认已经收到货并完成该订单吗？"
                        Visible="false" ForeColor="Red" />
                    <Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="关闭" CommandArgument='<%# Eval("OrderId") %>'
                        CommandName="CLOSE_TRADE" DeleteMsg="确认关闭该订单吗？" Visible="false" />
                    <a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server"
                        id="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>申请退款</a><br />
                    <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server"
                        id="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>申请退货</a>
                    <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server"
                        id="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>申请换货</a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel ID="panl_nodata" runat="server" Visible="false">
        <tr>
            <td colspan="5" align="center">暂无订单，这就去挑选商品：<a href="/Default.aspx">商城首页</a> <a href="/User/Favorites.aspx">收藏夹</a>
            </td>
        </tr>
    </asp:Panel>
</table>
<div id="myTab_Content1" class="none">
    <div id="spExpressData">
        正在加载中....
    </div>
</div>
<script type="text/javascript">
    function GetLogisticsInformation(orderId) {
        $('#spExpressData').expressInfo(orderId, 'OrderId');
        ShowMessageDialog("物流详情", "Exprass", "myTab_Content1")
    }
</script>
