<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td width="11%" align="center">������</td>
        <td width="19%" align="center">Ӷ���������</td>
        <td width="10%" align="center">Ӷ��</td>
        <td width="12%" align="center">Ӷ������</td>
        <td width="11%" align="center">״̬</td>
        <td width="27%" align="center">��ע</td>
    </tr>
    <asp:Repeater ID="repeaterAccountDetails" runat="server">
        <ItemTemplate>
            <tr>
                <td width="11%" align="center">
                    <asp:Literal ID="litOrderId" runat="server" Text='<%#Eval("OrderId") %>'></asp:Literal></td>
                <td width="19%" align="center">
                    <Hi:FormatedTimeLabel ID="lblTradeDate" runat="server" Time='<%# Eval("TradeDate") %>'></Hi:FormatedTimeLabel></td>
                <td width="10%" align="center">
                    <%# (int)Eval("TradeType") == 4 ? "-" + Eval("Expenses", "{0:F2}") : Eval("Income", "{0:F2}")%></td>
                <td width="12%" align="center">
                    <Hi:SplittingTypeNameLabel ID="SplittingTypeNameLabel1" runat="server" SplittingType='<%# Eval("TradeType")%>' />
                </td>
                <td width="11%" align="center">
                    <%# (bool)Eval("IsUse") ? ((int)Eval("TradeType") == 4?"������":"������")  : "��������"%>
                </td>
                <td width="27%" align="center">
                    <asp:Literal ID="litRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Literal></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
