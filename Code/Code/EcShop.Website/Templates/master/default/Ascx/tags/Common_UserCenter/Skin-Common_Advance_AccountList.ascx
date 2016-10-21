<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
<tr id="spqingdan_title">
    <td width="11%" align="center">��ˮ��</td>
    <td width="19%" align="center">ʱ��</td>
    <td width="12%" align="center">����</td>
    <td width="10%" align="center">����</td>
    <td width="11%" align="center">֧��</td>
    <td width="10%" align="center">�˻����</td>
     <td width="27%" align="center">��ע</td>
  </tr>
<asp:Repeater ID="repeaterAccountDetails" runat="server" >
    <ItemTemplate>
    <tr>
        <td width="11%" align="center">
            <asp:Literal ID="litJournalNumber" runat="server" Text='<%#Eval("JournalNumber") %>'></asp:Literal></td>
        <td width="19%" align="center">
            <Hi:FormatedTimeLabel ID="lblTradeDate" runat="server" Time='<%# Eval("TradeDate") %>'></Hi:FormatedTimeLabel></td>
        <td width="12%" align="center">
            <Hi:TradeTypeNameLabel ID="lblTradeType" runat="server" TradeType="TradeType"></Hi:TradeTypeNameLabel></td>
        <td width="10%" align="center">
            <Hi:FormatedMoneyLabel ID="lblIncome" runat="server" Money='<%# Eval("Income") %>'></Hi:FormatedMoneyLabel></td>
        <td width="11%" align="center">
            <Hi:FormatedMoneyLabel ID="lblExpenses" runat="server" Money='<%# Eval("Expenses") %>'></Hi:FormatedMoneyLabel></td>
        <td width="10%" align="center">
            <Hi:FormatedMoneyLabel ID="lblBalance" runat="server" Money='<%# Eval("Balance") %>'></Hi:FormatedMoneyLabel></td>
        <td width="27%" align="center"><asp:Literal ID="litRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Literal></td>
    </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
