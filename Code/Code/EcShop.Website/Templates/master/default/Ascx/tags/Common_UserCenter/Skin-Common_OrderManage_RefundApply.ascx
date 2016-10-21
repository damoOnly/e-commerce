<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
		    <tr id="spqingdan_title">
                <td width="25%" align="center">订单编号</td>
                <td width="15%" align="center">申请时间</td>
                <td width="15%" align="center">退款方式</td>
                <td width="15%" align="center">退款金额</td>
                <td width="15%" align="center">状态</td>
                <td width="15%" align="center">操作</td>
            </tr>
            <asp:repeater ID="listRefundOrder" runat="server">
            <ItemTemplate>
			<tr class="ddgl_1">
			    <td align="center"><%# Eval("OrderId") %></td>
			    <td align="center"><%# Eval("ApplyForTime")%></td>
			    <td align="center"><%# Eval("RefundType")%></td>
			    <td align="center"><%# Eval("OrderTotal","{0:F2}")%></td>
                <td align="center"><asp:Label ID="lblHandleStatus" Text='<%# Eval("HandleStatus") %>' runat="server" /></td>
                <td align="center"><asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_UserRefundApplyDetails",Eval("RefundId"))%>' Text="查看" /></td>
			</tr>
            </ItemTemplate>
            </asp:repeater>
		</table>