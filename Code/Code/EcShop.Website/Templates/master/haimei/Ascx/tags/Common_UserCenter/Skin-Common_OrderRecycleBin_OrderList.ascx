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
    <!--订单查询增加服务窗订单-->
    <asp:Repeater ID="listOrders" runat="server">
        <itemtemplate> 
        	<tbody>
            	<tr class="ol-blank"><td colspan="7"></td></tr>
            </tbody> 
        	 <tbody>
		        <tr class="ddgl">
			        <td colspan="6">
				        <span>订单编号：<em class="order-ser"><%# Eval("orderId")%></em></span> <span>下单日期：<%# Eval("OrderDate")%></span>
			        </td>
                    <td>
                         <asp:LinkButton runat="server" ID="lbtDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "orderId")%>'
                      CommandName="RevertDel" Text="还原" OnClientClick="return confirm('确定要把订单还原吗？')" CssClass="remove-btn"/>

                        <asp:LinkButton runat="server" ID="lbtcompleteDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "orderId")%>'
                      CommandName="completeDel" Text="彻底删除" OnClientClick="return confirm('确定要把订单彻底删除吗？')" CssClass="remove-btn"/>
			        </td>
		        </tr>
		         <tr class="ddgl_1">
			        <td class="pd-td" colspan="3">
                        <asp:Repeater ID="rpProduct" runat="server">
                            <itemtemplate>
                            <table class="pd-table">
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
                       <a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server"
                            id="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>退款</a>
                        <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server"
                            id="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>退货</a>
                        <a href="javascript:void(0)" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="lkbtnNotPay" visible="false">未付款</a>
                        <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server"
                            id="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>换货</a>
                    </td>
			        <td class="c5">
                       <span>
                       <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>' runat="server" /></span>
			        </td>
			        <td class="c6">
				        <p>
                           <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server"/>
				        </p>
				        <p><asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>' Text="订单详情" /></p>
                        <p>
                           <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(<%# Eval("OrderId") %>)">查看物流</a></asp:Label>
                        </p>
			        </td>
			        <td class="c7">
                      
			        </td>
		         </tr>             
           </tbody>
        </itemtemplate>
    </asp:Repeater>
    </table>
    <asp:Panel ID="panl_nodata" runat="server" Visible="false">
    	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
            <tr>
                <td colspan="5" align="center">暂无订单，这就去挑选商品：<a href="/Default.aspx">商城首页</a> <a href="/User/Favorites.aspx">收藏夹</a>
                </td>
            </tr>
        </table>
    </asp:Panel>

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
