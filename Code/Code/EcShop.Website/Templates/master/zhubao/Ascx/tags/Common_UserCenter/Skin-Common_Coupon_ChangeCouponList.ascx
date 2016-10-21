<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
  <tr id="spqingdan_title">
    <td width="25%" align="center">�Ż�ȯ����</td>
    <td width="15%" align="center">������</td>
    <td width="15%" align="center">��ֵ</td>
    <td width="15%" align="center">�һ��������</td>
    <td width="15%" align="center">��Ч�ڣ�ֹ��</td>
    <td width="15%" align="center">����</td>
  </tr>
<asp:Repeater ID="repeaterCoupon" runat="server" >
        <ItemTemplate>
            <tr>
                <td align="center">
                    <Hi:SubStringLabel ID="lblName" Field="Name" StrLength="12" StrReplace="..  " runat="server" /></td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="lblAmount" Money='<%#Eval("Amount") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="lblValue" Money='<%#Eval("DiscountValue") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                <td align="center">
                    <asp:Literal ID="litNeedPoint" Text='<%#Eval("NeedPoint") %>' runat="server" /></td>
                <td align="center">
                    <Hi:FormatedTimeLabel ID="lblClosingTime" Time='<%#Eval("ClosingTime") %>' runat="server"></Hi:FormatedTimeLabel></td>
                <td align="center"><Hi:ImageLinkButton runat="server" ID="lbtnChage" IsShow="true" DeleteMsg="ȷ��Ҫ�һ��𣬶һ���۳����Ļ���" Text="�һ�"  CommandName="Change" CommandArgument='<%#Eval("CouponId") %>' /></td>
                
            </tr>
        </ItemTemplate>
</asp:Repeater>
</table>