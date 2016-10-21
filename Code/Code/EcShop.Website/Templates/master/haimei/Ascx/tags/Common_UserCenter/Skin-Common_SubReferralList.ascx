<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.AccountCenter.CodeBehind" Assembly="EcShop.UI.AccountCenter.CodeBehind" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
        <tr id="spqingdan_title">
            <td align="center">
                �ƹ�Ա
            </td>
            <td align="center">
                ����
            </td>
            <td align="center">
                ��ϵ�绰
            </td>
            <td align="center">
                ֱ���ƹ㶩����
            </td>
            <td align="center">
                ����Ӷ��Ԫ��
            </td>
            <td align="center">
                ���ͨ������
            </td>
            <td align="center">
                ���һ���ƹ�����
            </td>
        </tr>
        <asp:Repeater ID="rptSubMembers" runat="server">
        <ItemTemplate>
            <tr>
            <td style="display:none">
                <asp:Literal runat="server" ID="litUserId" Text='<%# Eval("UserId") %>'></asp:Literal>
            </td>
                <td align="center">
                    <%# Eval("UserName")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("RealName") %>&nbsp;
                </td>
                <td align="center">
                   <%#Eval("CellPhone")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("ReferralOrderNumber")%>&nbsp;
                </td>
                <td align="center">
                   <%#((Decimal)Eval("SubReferralSplittin")).ToString("f2")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("ReferralAuditDate")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("LastReferralDate")%>&nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
