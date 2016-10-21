<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Membership.Context" %>
<script src="/Utility/expressInfo.js" type="text/javascript"></script>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable fixed">
	<thead>	
    <tr id="spqingdan_title" class="ol-title">
        <td align="center" class="c1">��Ʒ��ϸ</td>
        <td align="center" nowrap="nowrap" class="c2">����</td>
        <td align="center" class="c3">����</td>
        <td align="center" class="c4">��Ʒ����</td>
        <td align="center" class="c5">�������</td>
        <td align="center" class="c6">����״̬</td>
        <td align="center" class="c7">���ײ���</td>
    </tr>
    </thead>
    <!--������ѯ���ӷ��񴰶���-->
    <asp:Repeater ID="listOrders" runat="server">
        <itemtemplate> 
        	<tbody>
            	<tr class="ol-blank"><td colspan="7"></td></tr>
            </tbody> 
        	 <tbody>
		        <tr class="ddgl">
			        <td colspan="6">
				        <span>������ţ�<em class="order-ser"><%# Eval("orderId")%></em></span> <span>�µ����ڣ�<%# Eval("OrderDate")%></span>
			        </td>
                    <td>
                         <asp:LinkButton runat="server" ID="lbtDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "orderId")%>'
                      CommandName="RevertDel" Text="��ԭ" OnClientClick="return confirm('ȷ��Ҫ�Ѷ�����ԭ��')" CssClass="remove-btn"/>

                        <asp:LinkButton runat="server" ID="lbtcompleteDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "orderId")%>'
                      CommandName="completeDel" Text="����ɾ��" OnClientClick="return confirm('ȷ��Ҫ�Ѷ�������ɾ����')" CssClass="remove-btn"/>
			        </td>
		        </tr>
		         <tr class="ddgl_1">
			        <td class="pd-td" colspan="3">
                        <asp:Repeater ID="rpProduct" runat="server">
                            <itemtemplate>
                            <table class="pd-table">
                                   <tr class="aa-<%# Eval("RowNumber") %>">
                                       <td class="pd1"><Hi:UserProductDetailsLink ID="UserProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                            ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                                                <span class="img"><img title='<%# Eval("ItemDescription") %>' src="<%# string.IsNullOrEmpty(Eval("ThumbnailsUrl").ToString())?Utils.ApplicationPath+HiContext.Current.SiteSettings.DefaultProductThumbnail2:Utils.ApplicationPath+Eval("ThumbnailsUrl").ToString().Replace("thumbs40/40","thumbs60/60") %>" />
                                                </span>
                                                <span><%# Eval("ItemDescription") %></span>
                                       </Hi:UserProductDetailsLink></td>

                                       <td class="pd2"><Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' /></td>
                                       <td class="pd3"><%# Eval("Quantity") %></td>
                                    </tr>
                                </table>
                                </itemtemplate>
                        </asp:Repeater>
			        </td>
                    <td class="c4">
                       <a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server"
                            id="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>�˿�</a>
                        <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server"
                            id="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>�˻�</a>
                        <a href="javascript:void(0)" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="lkbtnNotPay" visible="false">δ����</a>
                        <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server"
                            id="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>����</a>
                    </td>
			        <td class="c5">
                       <span>
                       <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>' runat="server" /></span>
			        </td>
			        <td class="c6">
				        <p>
                           <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server"/>
				        </p>
				        <p><asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>' Text="��������" /></p>
                        <p>
                           <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(<%# Eval("OrderId") %>)">�鿴����</a></asp:Label>
                        </p>
			        </td>
			        <td class="c7">
                      
			        </td>
		         </tr>             
           </tbody>
        </itemtemplate>
    </asp:Repeater>
    </table>
    <asp:Panel ID="panl_nodata" runat="server" Visible="false">
    	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
            <tr>
                <td colspan="5" align="center">���޶��������ȥ��ѡ��Ʒ��<a href="/Default.aspx">�̳���ҳ</a> <a href="/User/Favorites.aspx">�ղؼ�</a>
                </td>
            </tr>
        </table>
    </asp:Panel>

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
