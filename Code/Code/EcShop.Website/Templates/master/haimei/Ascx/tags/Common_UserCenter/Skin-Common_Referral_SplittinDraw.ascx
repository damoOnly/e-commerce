<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
<tr id="spqingdan_title">
    <td width="15%" align="center">���ֽ��</td>       
    <td width="30%" align="center">�����˺�</td>
    <td width="20%" align="center">��������</td> 
      <td width="15%" align="center">���״̬</td> 
    <td width="20%" align="center">�������</td>
  </tr>
<asp:Repeater ID="rptSplittinDraws" runat="server" >
    <ItemTemplate>
    <tr>
        <td width="11%" align="center">
            <%#Eval("Amount", "{0:F2}") %></td>
        <td width="11%" align="center">
            <%#Eval("Account")%></td>
        <td width="19%" align="center">
            <Hi:FormatedTimeLabel ID="lblTradeDate" runat="server" Time='<%# Eval("RequestDate") %>'></Hi:FormatedTimeLabel></td>      
         <td width="11%" align="center">
            <%#(int)Eval("AuditStatus") == 1?"������":"���ֳɹ�"%></td>
        <td width="19%" align="center">
            <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" runat="server" Time='<%# Eval("AccountDate") %>'></Hi:FormatedTimeLabel></td>    
    </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
