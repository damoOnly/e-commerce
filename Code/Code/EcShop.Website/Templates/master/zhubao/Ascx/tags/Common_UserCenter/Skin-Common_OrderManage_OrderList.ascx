<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<script src="/Utility/expressInfo.js" type="text/javascript"></script>


<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td align="center">��Ʒ��Ϣ
        </td>
        <td align="center" nowrap="nowrap">�ջ���
        </td>
        <td align="center">�������
        </td>
        <td align="center">����״̬
        </td>
        <td align="center">����
        </td>
    </tr>
    <!--������ѯ���ӷ��񴰶���-->
    <asp:Repeater ID="listOrders" runat="server">
        <ItemTemplate>
            <tr class="ddgl">
                <td colspan="5">
                    <span>������ţ�<em style="color: #2060af;"><%# Eval("OrderId") %></em></span> <span>�µ����ڣ�<%#Eval("OrderDate") %></span>
                </td>
            </tr>
            <tr class="ddgl_1">
                <td align="left" class="padd">
                    <asp:Repeater ID="rpProduct" runat="server">
                        <ItemTemplate>
                            <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                              
                                 <img title='<%# Eval("ItemDescription") %>' src="<%# string.IsNullOrEmpty(Eval("ThumbnailsUrl").ToString())?Utils.ApplicationPath+HiContext.Current.SiteSettings.DefaultProductThumbnail2:Utils.ApplicationPath+Eval("ThumbnailsUrl").ToString().Replace("thumbs40/40","thumbs60/60") %>" />
                            </Hi:ProductDetailsLink>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td align="center" nowrap="nowrap">
                    <%# Eval("ShipTo") %>
                </td>
                <td align="center" nowrap="nowrap">
                    <span>
                        <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>'
                            runat="server" /></span> <span>
                                <%# Eval("PaymentType") %></span>
                    <%# Eval("GateWay").ToString()=="ecdev.plugins.payment.bankrequest"&&Eval("OrderStatus").ToString()=="1"?"<span><a href=\"bank.aspx?OrderId="+Eval("OrderId")+"\" target=\"_blank\">���¸���</a></span>":"" %>
                </td>
                <td align="center" nowrap="nowrap">
                    <span class="fkzhuangtai">
                        <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>'
                            runat="server" /></span>
                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(<%# Eval("OrderId") %>)">��������</a></asp:Label>
                </td>
                <td align="center" nowrap="nowrap">
                    <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderReviews",Eval("orderId")) %>'>����</asp:HyperLink>
                    <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>'
                        Text="�鿴" />
                    <a href="javascript:void(0)" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="hlinkPay">����</a>
                    <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="ȷ�϶���"
                        CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="ȷ���Ѿ��յ�������ɸö�����"
                        Visible="false" ForeColor="Red" />
                    <Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="�ر�" CommandArgument='<%# Eval("OrderId") %>'
                        CommandName="CLOSE_TRADE" DeleteMsg="ȷ�Ϲرոö�����" Visible="false" />
                    <a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server"
                        id="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>�����˿�</a><br />
                    <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server"
                        id="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>�����˻�</a>
                    <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server"
                        id="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>���뻻��</a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel ID="panl_nodata" runat="server" Visible="false">
        <tr>
            <td colspan="5" align="center">���޶��������ȥ��ѡ��Ʒ��<a href="/Default.aspx">�̳���ҳ</a> <a href="/User/Favorites.aspx">�ղؼ�</a>
            </td>
        </tr>
    </asp:Panel>
</table>
<div id="myTab_Content1" class="none">
    <div id="spExpressData">
        ���ڼ�����....
    </div>
</div>
<script type="text/javascript">
    function GetLogisticsInformation(orderId) {
        $('#spExpressData').expressInfo(orderId, 'OrderId');
        ShowMessageDialog("��������", "Exprass", "myTab_Content1")
    }
</script>
