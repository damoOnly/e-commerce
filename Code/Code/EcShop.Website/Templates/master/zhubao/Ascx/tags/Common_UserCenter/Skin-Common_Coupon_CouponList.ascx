<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr>
        <td align="center">
            <asp:Literal ID="litCouponName" runat="server" Text="�Ż�ȯ����"></asp:Literal></td>
        <td align="center">
            <asp:Literal ID="litCouponCode" runat="server" Text="����"></asp:Literal></td>
        <td align="center">
            <asp:Literal ID="litTradeDateHead" runat="server" Text="������"></asp:Literal></td>
        <td align="center">
            <asp:Literal ID="litTradeTypeHead" runat="server" Text="��ֵ"></asp:Literal></td>
        <td align="center">
            <asp:Literal ID="lblStartTime" runat="server" Text="��ʼ����"></asp:Literal></td>
        <td align="center">
            <asp:Literal ID="litIncomeHead" runat="server" Text="��Ч��(ֹ)"></asp:Literal></td>
        <td align="center">
            <asp:Literal ID="Literal1" runat="server" Text="ʹ��״̬"></asp:Literal></td>
    </tr>
    <asp:Repeater ID="repeaterCoupon" runat="server" >
        <ItemTemplate>
            <tr class="ddgl_1">
                <td align="center">
                    <%# Eval("Name") %>
                <td align="center">
                    <%# Eval("ClaimCode")%>
                </td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="lblAmount" Money='<%#Eval("Amount") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="lblValue" Money='<%#Eval("DiscountValue") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                <td align="center">
                    <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%#Eval("StartTime") %>' runat="server"></Hi:FormatedTimeLabel></td>
                    
                <td align="center">
                    <Hi:FormatedTimeLabel ID="lblClosingTime" Time='<%#Eval("ClosingTime") %>' runat="server"></Hi:FormatedTimeLabel></td>
                <td align="center"><%# Eval("CouponStatus").ToString() == "1" ? "��ʹ��" : "δʹ��"%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>