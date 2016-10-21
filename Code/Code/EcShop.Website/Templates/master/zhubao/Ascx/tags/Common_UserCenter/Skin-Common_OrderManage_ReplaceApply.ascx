<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td width="20%" align="center">订单编号</td>
        <td width="30%" align="center">申请时间</td>
        <td width="30%" align="center">状态</td>
        <td width="20%" align="center">操作</td>
    </tr>
    <asp:repeater ID="listReplace" runat="server">
    <ItemTemplate>
	<tr class="ddgl_1">
		<td align="center"><%# Eval("OrderId") %></td>
		<td align="center"><%# Eval("ApplyForTime")%></td>
        <td align="center"><asp:Label ID="lblHandleStatus" Text='<%# Eval("HandleStatus") %>' runat="server" /></td>
        <td align="center"><asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_UserReplaceApplyDetails",Eval("ReplaceId"))%>'
            Text="查看" /></td>
	</tr>
    </ItemTemplate>
    </asp:repeater>
</table>
