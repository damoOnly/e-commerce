﻿<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="EcShop.UI.Web.Admin.OrderDetails" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ChargesList" Src="~/Admin/Ascx/Order_ChargesList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title title_height m_none td_bottom">
            <em>
                <img src="../images/02.gif" width="32" height="32" /></em>
            <h1 class="title_line">订单信息详情</h1>
        </div>
        <div class="Purchase">
            <div class="Settlement">
                <table>
                    <tr>
                        <td><strong class="fonts colorE">当前订单（<asp:Literal runat="server" ID="litOrderId" />）状态：<Hi:OrderStatusLabel ID="lblOrderStatus" runat="server" /></strong>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbCloseReason" Text="关闭原因：">
                                <asp:Label runat="server" ID="lbReason"></asp:Label></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li></li>
                                <li>会员名：<asp:Literal runat="server" ID="litUserName" />
                                    真实姓名：<asp:Literal runat="server" ID="litRealName" />
                                    联系电话：<asp:Literal runat="server" ID="litUserTel" />
                                    电子邮件：<asp:Literal runat="server" ID="litUserEmail" />
                                </li>
                                <li>
                                    <asp:Literal ID="litPayTime" runat="server" />
                                    <asp:Literal ID="litSendGoodTime" runat="server" />
                                    <asp:Literal ID="litFinishTime" runat="server" />
                                </li>
                                <li>
                                    <asp:Literal runat="server" ID="litSourceOrder" />
                                </li>
                                <li>
                                    <asp:Literal ID="litManagerRemark" runat="server" />
                                </li>
                                <li class="Pg_8"><span class="submit_btnxiugai">
                                    <asp:HyperLink runat="server" ID="lkbtnEditPrice" Height="25px" Text="修改价格" /></span>
                                    <span class="submit_btnbianji"><a href="javascript:ShowRemarkOrder();">备注</a></span>
                                    <span class="submit_btnguanbi"><a id="lbtnClocsOrder" runat="server" href="javascript:ShowCloseOrder();">关闭订单</a></span>
                                    <span class="submit_faihuo">
                                        <asp:HyperLink runat="server" ID="lkbtnSendGoods" Text="发货" NavigateUrl="javascript:ShowSend()"></asp:HyperLink></span>
                                </li>
                            </ul>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="blank12 clearfix"></div>
        <div class="list">
            <cc1:Order_ItemsList runat="server" ID="itemsList" />
            <asp:HyperLink runat="server" ID="hlkOrderGifts" Text="添加订单礼品" ForeColor="blue" />
            <cc1:Order_ChargesList ID="chargesList" runat="server" />
            <cc1:Order_ShippingAddress runat="server" ID="shippingAddress" />
            <h1>备注信息</h1>
            <div class="Settlement">
            <UI:Grid ID="gridRemarks" runat="server" ShowHeader="true" EmptyDataText="暂无备注信息" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                <Columns>
                    <asp:ImageField DataImageUrlField="TagImg" HeaderText="标志" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%" HeaderStyle-Font-Bold="false"></asp:ImageField>
                    <asp:BoundField  HeaderText="备忘录" DataField="Remark" NullDisplayText="-" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="50%" HeaderStyle-Font-Bold="false" />
                    <asp:BoundField  HeaderText="备注人" DataField="Operator" NullDisplayText="-" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%" HeaderStyle-Font-Bold="false" />
                    <asp:BoundField  HeaderText="备注时间" DataField="RecordTime" NullDisplayText="-" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%" HeaderStyle-Font-Bold="false" />
                </Columns>   
            </UI:Grid>
                </div>
        </div>
    </div>

    <!--编辑备注信息-->
    <div id="RemarkOrder" style="display: none;">
        <div class="frame-content">
            <p><span class="frame-span frame-input110">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></p>

            <p><span class="frame-span frame-input110">成交时间：</span><Hi:FormatedTimeLabel runat="server" ID="lblorderDateForRemark" /></p>
            <p>
                <span class="frame-span frame-input110">订单实收款(元)：</span><em><Hi:FormatedMoneyLabel
                    ID="lblorderTotalForRemark" runat="server" /></em>
            </p>
            <span class="frame-span frame-input110">标志：</span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" />

            <p><span class="frame-span frame-input110">备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></p>
        </div>
    </div>

    <!--关闭订单-->
    <div id="closeOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em>
            </p>
            <p>
                <span>关闭该订单的理由：<em>*</em></span><Hi:CloseTranReasonDropDownList runat="server"
                    ID="ddlCloseReason" />
            </p>
        </div>
    </div>

    <!--修改配送方式-->
    <div id="setShippingMode" style="display: none;">
        <div class="frame-content">
            <p><span class="frame-span frame-input130">请选择新的配送方式:<em>*</em></span><Hi:ShippingModeDropDownList runat="server" ID="ddlshippingMode" /></p>
        </div>
    </div>

    <!--修改支付方式-->
    <div id="setPaymentMode" style="display: none;">
        <div class="frame-content">
            <p><span class="frame-span frame-input130">请选择新的支付方式:<em>*</em></span><Hi:PaymentDropDownList runat="server" ID="ddlpayment" /></p>
        </div>
    </div>

     <!--修改货号-->
    <div id="setSkuPanel" style="display: none;">
        <div class="frame-content">
            <p><span style="float:left;clear:both;">请输入新的货号：<em>*</em>&nbsp;</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSku" /></p>
        </div>
    </div>
    <!--修改供货商-->
    <div id="setSupplierPanel" style="display: none;">
        <div class="frame-content">
            <p><span style="float:left;clear:both;">请选择新的供货商：<em>*</em>&nbsp;</span><Hi:SupplierDropDownList runat="server"  ID="ddlSupplier" /></p>
        </div>
    </div>

    <div style="display: none">
        <asp:HiddenField runat="server" ID="hiddenOrderValue" />
        <asp:HiddenField runat="server" ID="hiddenOrderStatus" />
        <asp:Button runat="server" ID="btnRemark" Text="编辑备注" CssClass="submit_DAqueding" />
        <asp:Button ID="btnCloseOrder" runat="server" CssClass="submit_DAqueding" Text="关闭订单" />
        <asp:Button ID="btnMondifyShip" runat="server" CssClass="submit_DAqueding" Text="修改配送方式" />
        <asp:Button ID="btnMondifyPay" runat="server" CssClass="submit_DAqueding" Text="修改支付方式" />
        <asp:Button ID="btnUpdateSku" runat="server" CssClass="submit_DAqueding" Text="修改货号" />
        <asp:Button ID="btnUpdateSupplier" runat="server" CssClass="submit_DAqueding" Text="修改供货商" />
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function(){
            if(<%=orderStatus%>!=2){
                $('a.orderitem').remove();
            }
        });
        var formtype = "";
        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                alert("请选择关闭的理由");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }
         
        function ValidationPayment() {
            var payment = document.getElementById("ctl00_contentHolder_ddlpayment").value;
            if (payment == "") {
                alert("请选择支付方式");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlpayment", payment);
            return true;
        }
         
        function ValidationShippingMode() {
            var shipmode = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
            if (shipmode == "") {
                alert("请选择配送方式");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlshippingMode", shipmode);
            return true;
        }

        //备注弹出框
        function ShowRemarkOrder() {
            arrytext = null;
            formtype = "remark";

            DialogShow("订单备注", 'orderrmark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
        }

        //关闭订单
        function ShowCloseOrder() {
            arrytext = null;
            formtype = "closeorder";
            DialogShow("关闭订单", 'closeorder', 'closeOrder', 'ctl00_contentHolder_btnCloseOrder');
        }

        //发货
        function ShowSend() {
            var orderId = <%=Page.Request.QueryString["OrderId"] %>;
             DialogFrame("sales/SendOrderGoods.aspx?OrderId=" + orderId, '发货', null, null)
         }


         //修改支付方式
         function UpdatePaymentMode() {
             arrytext = null;
             formtype = "paytype";
             DialogShow("修改支付方式", 'paytype', 'setPaymentMode', 'ctl00_contentHolder_btnMondifyPay');
         }


         //修改配送方式
         function UpdateShippingMode() {
             arrytext = null;
             formtype = "shipptype";
             DialogShow("修改配送方式", 'updateship', 'setShippingMode', 'ctl00_contentHolder_btnMondifyShip');
         }

        //修改货号
         function updateSku(orderId,skuId,productId,oldSku){
             arrytext = null;
             formtype = "sku";
             DialogShow("修改货号", 'sku', 'setSkuPanel', '<%:btnUpdateSku.ClientID%>');
             $('#<%:txtSku.ClientID%>').val(oldSku).select().focus();
             $('#<%:hiddenOrderValue.ClientID%>').val(orderId+'|'+skuId+'|'+productId);
         }

         //修改供货商
         function updateSupplier(orderId,skuId,productId,oldSupplierId){
             arrytext = null;
             formtype = "supplier";
             DialogShow("修改供货商", 'supplier', 'setSupplierPanel', '<%:btnUpdateSupplier.ClientID%>');
             $.each($('#<%:ddlSupplier.ClientID%> option'),function(){
                 if($(this).val().indexOf(oldSupplierId+'|')==0){
                     $(this).attr('selected','selected');
                     return false;
                 }
             });
             $('#<%:hiddenOrderValue.ClientID%>').val(orderId+'|'+skuId+'|'+productId);
         }

         function validatorForm() {
             switch (formtype) {
                 case "remark":
                     $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                     if ($radioId == null || $radioId == "undefined") {
                         alert('请先标记备注');
                         return false;
                     }
                     setArryText($radioId.id, "true");
                     setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                     break;
                 case "shipptype":
                     return ValidationShippingMode();
                     break;
                 case "closeorder":
                     return ValidationCloseReason();
                     break;
                 case "paytype":
                     return ValidationPayment();
                     break;
                 case "changeorder":
                     if ($("#txtpost").val().replace(/\s/g, "") == "") {
                         alert("发货单号不允许为空！");
                         return false;
                     }
                     setArryText("txtpost", $("#txtpost").val());
                     break;
                 case "sku":
                     var newSku=$.trim($('#<%:txtSku.ClientID%>').val());
                     if(newSku==''){
                         alert("货号不允许为空！");
                         return false;
                     }
                     setArryText('<%:txtSku.ClientID%>',newSku);
                     break;
                 case "supplier":
                     var supplierId =$('#<%:ddlSupplier.ClientID%>').val();
                     if (supplierId == "") {
                         alert("请选择供货商");
                         return false;
                     }
                     setArryText('<%:ddlSupplier.ClientID%>', supplierId);
                     break;
             };
             return true;
         }
         function CloseFrameWindow() {
             var win = art.dialog.open.origin;
             win.location.reload();
         }    
    </script>
</asp:Content>
