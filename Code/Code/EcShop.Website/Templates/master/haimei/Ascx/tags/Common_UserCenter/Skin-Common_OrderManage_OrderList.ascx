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
    
  <%--<table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="7">
                    <span>������ţ�<em class="order-ser">201507225988731</em></span> <span>�µ����ڣ�2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">ɾ��</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>��2��װ������Gerber�α� 2������ƻ���׷� 227g Ӥ�׶���ʳ</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4">δ����</td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>�ȴ���Ҹ���</p>
                    <p><a>��������</a></p>
                </td>
                <td class="c7">
                	<a class="blue-btn red-btn">��������</a>
                    <p><a>ȡ������</a></p>
                    <p><a>�ٴι���</a></p>
                </td>
             </tr>             
     </table>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>������ţ�<em class="order-ser">201507225988731</em></span> <span>�µ����ڣ�2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">ɾ��</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>�¹�ϲ�� �л��簲ˮ�������׺� 6�� 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><a>�˿�/�˻�</a></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>�����ѷ���</p>
                    <p><a>��������</a></p>
                    <p><a class="red-link">�鿴����</a></p>
                </td>
                <td class="c7">
                	<a class="blue-btn">ȷ���ջ�</a>
                </td>
             </tr>             
     </table>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>������ţ�<em class="order-ser">201507225988731</em></span> <span>�µ����ڣ�2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">ɾ��</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>�¹�ϲ�� �л��簲ˮ�������׺� 6�� 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><a>�˿�/�˻�</a></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>�ȴ�����</p>
                    <p><a>��������</a></p>                    
                </td>
                <td class="c7">
                	<a class="blue-btn gray-btn">���ѷ���</a>
                </td>
             </tr>             
     </table>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>������ţ�<em class="order-ser">201507225988731</em></span> <span>�µ����ڣ�2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">ɾ��</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>�¹�ϲ�� �л��簲ˮ�������׺� 6�� 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><a>�˿�/�˻�</a></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>�������</p>
                    <p><a>��������</a></p> 
                    <p><a>�鿴����</a></p>                   
                </td>
                <td class="c7">
                	<a class="blue-btn gray-btn">������Ʒ</a>
                </td>
             </tr>             
     </table>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>������ţ�<em class="order-ser">201507225988731</em></span> <span>�µ����ڣ�2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">ɾ��</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1">
                	<a class="glink">
                    	<span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                        <span>�¹�ϲ�� �л��簲ˮ�������׺� 6�� 250g</span>
                    </a>
                </td>
                <td class="c2">112.00</td>
                <td class="c3">1</td>
                <td class="c4"><p class="red-link">���˿�</p></td>
                <td class="c5">301.00</td>
                <td class="c6">
                	<p>���׹ر�</p>
                    <p><a>��������</a></p> 
                    <p><a>�鿴����</a></p>                   
                </td>
                <td class="c7">
                	<a class="agi-link">�����µ� </a>
                </td>
             </tr>             
     </table>--%>
<%-- <table width="100%" border="0" cellspacing="0" cellpadding="0" class="oltable">
            <tr class="ddgl">
                <td colspan="6">
                    <span>������ţ�<em class="order-ser">201507225988731</em></span> <span>�µ����ڣ�2015/7/22 10:23:48</span>
                </td>
                <td><a class="remove-btn">ɾ��</a></td>
            </tr>
             <tr class="ddgl_1">
             	<td class="c1" colspan="3">
                	<table>
                      <tr>
                        <td><a class="glink">
                    	    <span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                            <span>�¹�ϲ�� �л��簲ˮ�������׺� 6�� 250g</span>
                            </a>
                        </td>
                        <td class="c2">112.00</td>
                        <td class="c3">1</td>
                       </tr>
                     </table>
                </td>
                <td class="c4"><a>�˿�/�˻�</a></td>
                <td rowspan="3" class="c5">301.00</td>
                <td rowspan="3" class="c6">
                	<p>�����ѷ���</p>
                    <p><a>��������</a></p>
                    <p><a class="red-link">�鿴����</a></p>
                </td>
                <td rowspan="3" class="c7">
                	<a class="blue-btn">ȷ���ջ�</a>
                </td>
             </tr>
             <tr class="ddgl_1">
             	             	<td class="c1" colspan="3">
                	<table>
                      <tr>
                        <td><a class="glink">
                    	    <span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                            <span>�¹�ϲ�� �л��簲ˮ�������׺� 6�� 250g</span>
                            </a>
                        </td>
                        <td class="c2">112.00</td>
                        <td class="c3">1</td>
                       </tr>
                     </table>
                </td>
                <td class="c4"><a>�˿�/�˻�</a></td>
             </tr>
             <tr class="ddgl_1">
             	<td class="c1" colspan="3">
                	<table>
                      <tr>
                        <td><a class="glink">
                    	    <span class="img"><img title="��ƷͼƬ" src="/templates/master/haimei/images/temp/g1.png"></span>
                            <span>�¹�ϲ�� �л��簲ˮ�������׺� 6�� 250g</span>
                            </a>
                        </td>
                        <td class="c2">112.00</td>
                        <td class="c3">1</td>
                       </tr>
                     </table>
                </td>
                <td class="c4"><a>�˿�/�˻�</a></td>
             </tr>
     </table>--%>
    <!--������ѯ���ӷ��񴰶���-->
    <asp:Repeater ID="listOrders" runat="server">
        <itemtemplate> 
        	<tbody>
            	<tr class="ol-blank"><td colspan="7"></td></tr>
            </tbody> 
        	 <tbody>
		        <tr class="ddgl">
			        <td colspan="7">
				        <span>������ţ�<em class="order-ser"><%# Eval("orderId")%></em></span><span>�µ�ʱ�䣺<%# Eval("OrderDate")%></span></td>
			        <%--<a class="remove-btn">ɾ��</a>--%>
<%--                     <td><asp:LinkButton runat="server" ID="lbtDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "orderId")%>'
                      CommandName="LogicDelete" Text="����ɾ��" OnClientClick="return confirm('ȷ��Ҫ�Ѷ����������վ��')" CssClass="remove-btn"/>
			        </td>--%>
		        </tr>
		         <tr class="ddgl_1">
			        <td class="pd-td" colspan="3">
                        <asp:Repeater ID="rpProduct" runat="server">
                            <itemtemplate>
                            <table class="pd-table" style="width:100%;">
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
                       <%--<a href="javascript:void(0)" onclick="return ApplyForRefund(this.title)" runat="server"
                            id="lkbtnApplyForRefund" visible="false" title='<%# Eval("OrderId") %>'>�˿�</a>
                        <a href="javascript:void(0)" onclick="return ApplyForReturn(this.title)" runat="server"
                            id="lkbtnApplyForReturn" visible="false" title='<%# Eval("OrderId") %>'>�˻�</a>--%>
                        <a href="javascript:void(0)" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="lkbtnNotPay" visible="false">δ����</a>
                       <%-- <a href="javascript:void(0)" onclick="return ApplyForReplace(this.title)" runat="server"
                            id="lkbtnApplyForReplace" visible="false" title='<%# Eval("OrderId") %>'>����</a>--%>
                    </td>
			        <td class="c5">
                       <span>
                       <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>' runat="server" /></span>
			        </td>
			        <td class="c6">
				        <p>
                           <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' IsRefund='<%# Eval("IsRefund") %>' PayDate='<%# Eval("PayDate") %>' runat="server"/>
				        </p>
				        <p><asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>' Text="��������" /></p>
                        <p>
                           <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(<%# Eval("OrderId") %>)">�鿴����</a></asp:Label>
                        </p>

			        </td>
			        <td class="c7">
                        <a href="javascript:void(0)" class="blue-btn red-btn" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' id="hlinkPay">��������</a>
				        <p><Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="ȡ��" CommandArgument='<%# Eval("OrderId") %>'
                            CommandName="CLOSE_TRADE" DeleteMsg="ȷ��ȡ���ö�����" />
				        </p>

                        <p><%--<Hi:ImageLinkButton ID="lkbtnRefund" IsShow="true" runat="server" Text="�����˿�"/>--%>
                            <asp:LinkButton ID="lkbtnRefund" runat="server"  Visible="false" CommandArgument='<%# Eval("OrderId") %>'   CommandName="CLOSE_Refund"  >�����˿�</asp:LinkButton>
                            <asp:Label ID="lb_refundtitle" runat="server" Text="�˿���" Visible="false"></asp:Label>
				        </p>
				       <%-- <p><a>�ٴι���</a></p>--%>
                       <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderReviews",Eval("orderId")) %>'>����</asp:HyperLink>
                        <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="ȷ���ջ�"
                            CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="ȷ���Ѿ��յ�������ɸö�����"
                            Visible="false" ForeColor="Red" />

			        </td>
		         </tr>             
           </tbody>
        </itemtemplate>
    </asp:Repeater>
    </table>
    <asp:Panel ID="panl_nodata" runat="server" Visible="false">
    	<div class="em-con">    	
            <div class="message">
                        <div class="no-data fix">
                            <p>����û�ж���Ŷ~</p>
                            <div class="g-btns">
                                <a class="g-btn" href="../../../ShoppingCart.aspx">ȥ�ҵĹ��ﳵ</a><a class="g-btn red-btn" href="../../../Default.aspx">ȥ��ҳ���</a>
                            </div>
                        </div> 
                    </div>
         </div>
    </asp:Panel>

<div id="myTab_Content1" class="none">
    <div id="spExpressData">
        ���ڼ�����....
    �ڼ�����....
    </div>
</div>
<script type="text/javascript">
    function GetLogisticsInformation(orderId) {
        $('#spExpressData').expressInfo(orderId, 'OrderId');
        ShowMessageDialog("��������", "Exprass", "myTab_Content1")
    }
    function IsRefund(orderid, SourceId) {
        var data = {};
        var msg = "��ȷ��Ҫ�����˿���";
        if (SourceId != "")//��
        {
            msg = "ͬһ�������";
            $.ajax({
                url: "/api/VshopProcess.ashx?action=Getlistofchildren&SourceOrderId=" + SourceId,
                type: 'post', dataType: 'json', timeout: 10000,
                error: function (e) {
                    alert("�����쳣����ȷ�������Ƿ����ӣ�");
                },
                success: function (data) {
                    if (data.Success == -1) {
                        alert("�˿��쳣�����Ժ����ԣ�");
                        return;
                    }
                    msg += data.data + "��ͬ��ȡ�����Ƿ�ȷ��ȡ�����ж�����";
                    rqsubmit(msg, orderid, SourceId);
                }
            });
        } else {
            rqsubmit(msg, orderid, SourceId);
        }

    }
    //ִ���˿�
    function rqsubmit(msg, orderid, SourceId)
    {
        if (confirm(msg))
        {
            $.ajax({
                url: "/api/VshopProcess.ashx?action=RqRefundOrder&SourceOrderId=" + SourceId + "&orderid=" + orderid,
                type: 'post', dataType: 'json', timeout: 10000,
                error: function (e)
                {
                    location.replace(location);
                },
                success: function (data)
                {
                    if (data.success == "0")
                    {
                        alert('�����˿�ɹ���');
                    }
                    else
                    {
                        alert("�����˿�ʧ�ܣ������ԣ�");
                    }
                }

            });
        }
    }

</script>
