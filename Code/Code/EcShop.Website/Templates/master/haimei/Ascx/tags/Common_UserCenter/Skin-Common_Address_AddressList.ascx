<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
  <tr id="spqingdan_title">
  <td width="10%" align="center">�ջ���</td>
    <%--<td width="10%" align="center">��������</td>--%>
    <td width="31%" align="center">�ֵ���ַ</td>
    <td width="15%" align="center">�绰����</td>
    <td width="15%" align="center">�ֻ�����</td>
    <td width="25%" align="center">����</td>
  </tr>
<asp:Repeater ID="repeaterRegionsSelect" runat="server">
     <ItemTemplate>
     
     <tr>
     <td align="center"><asp:Label ID="lblShipTo" runat="server" Text='<%#Bind("ShipTo") %>'></asp:Label></td>
     <%--<td align="center"><asp:Label ID="lblZipcode" runat="server" Text='<%#Bind("ZipCode") %>'></asp:Label></td>--%>
     <td align="center"><Hi:RegionAllName ID="RegionAllName1" RegionId='<%# Eval("RegionId") %>'  runat="server"></Hi:RegionAllName><asp:Label ID="lblAddress" runat="server" Text='<%#Bind("Address") %>'></asp:Label></td>
     <td align="center"><asp:Label ID="lblTellPhone" runat="server" Text='<%#Bind("TelPhone")%>'></asp:Label></td>
     <td align="center"><asp:Label ID="lblPhone" runat="server" Text='<%#Bind("CellPhone") %>'></asp:Label></td>
     <td align="center">
         <%--style="color:red"--%> 
         <asp:LinkButton ID="btnDefault" runat="server" CommandArgument='<%# Eval("ShippingId") %>'  Enabled='<%#Eval("IsDefault").ToString() == "True"?false:true %>' CommandName="SetDefault" Text='<%#Eval("IsDefault").ToString() == "True"?"<span >Ĭ�ϵ�ַ</span>":"��ΪĬ��" %>' />&nbsp;&nbsp;
         <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("ShippingId") %>' CommandName="Edit" Text="�༭" />&nbsp;&nbsp;
         <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("ShippingId") %>'  CommandName="Delete" Text="ɾ��" OnClientClick="return confirm('ȷ��Ҫɾ��������ַ��?')" />
     </td>
     </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
