<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReturnsDetails.aspx.cs" Inherits="EcShop.UI.Web.Admin.ReturnsDetails" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title title_height m_none td_bottom">
            <em>
                <img src="../images/02.gif" width="32" height="32" /></em>
            <h1 class="title_line">退货详情</h1>
        </div>
        <div class="Purchase">
            <div class="Settlement">
                <table>
                    <tr>
                        <td><strong class="fonts colorE">当前退货单（<asp:Literal runat="server" ID="litReturnsId" />）状态：<asp:Literal runat="server" ID="litReturnsHandleStatus" /></strong>
                        </td>
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
                                    <asp:Literal ID="litApplyTime" runat="server" />
                                    <!--申请时间--->
                                </li>
                                <li>    关联的订单号：
                                    <a href="../../admin/default.html?sales/OrderDetails.aspx?OrderId=<%=orderId %>" target="_blank">
                                <asp:Literal runat="server" ID="litSourceOrder" /></a>
                                    <!--关联的订单号--->
                                </li>
                            </ul>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="blank12 clearfix"></div>
        <div class="list">

            <asp:Repeater ID="rptItmesList" runat="server">
                <HeaderTemplate>
                    <table width="200" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td colspan="2" class="td_right td_left">退货商品名称</td>
                            <td width="13%" class="td_right td_left">商品单价(元) </td>
                            <td width="12%" class="td_right td_left">数量 </td>
                            <td width="9%" class="td_right td_left">税率 </td>
                            <td width="10%" class="td_right td_left">供货商 </td>
                            <td width="13%" class="td_left td_right_fff">小计(元) </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="7%"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                            <Hi:ListImage ID="HiImage2" runat="server" DataField="ThumbnailsUrl" Width="40px" /></a> </td>
                        <td width="32%"><span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                            <%# Eval("ItemDescription")%></a></span> <span class="colorC">
                                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
                        </td>
                        <td>
                            <Hi:FormatedMoneyLabel ID="lblItemAdjustedPrice" runat="server" Money='<%# Eval("ItemAdjustedPrice") %>' /></td>
                        <td>×<asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
                        <td>
                            <asp:Literal runat="server" ID="litTaxRate" Text='<%#Eval("TaxRate") %>' /></td>
                        <td>
                            <asp:Literal runat="server" ID="litSupplierName" Text='<%#Eval("SupplierName").ToString()==""?"  - ":Eval("SupplierName") %>' /></td>
                        <td>
                            <strong class="colorG">
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <div class="Price" style="width: 820px; margin: 0 auto">
                <span style="margin-left: 400px;"><b style="font-size: 16px; color: red">
                    <asp:Literal runat="server" ID="lblAmoutPrice" /></b></span>
            </div>

            <div class="Settlement">
                <!----退货详情---->
                <table width="200" border="0" cellspacing="0">
                    <tr>
                        <td style="width: 100px;" align="right">退货的金额(元)： </td>
                        <td>
                            <asp:Literal ID="litReturnsMoney" runat="server" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">收取清关费：</td>
                        <td>
                            <asp:Literal ID="litCustomsClearanceFee" runat="server" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">收取快递费：</td>
                        <td>
                            <asp:Literal ID="litExpressFee" runat="server" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">快递费用归属：</td>
                        <td colspan="1">
                            <asp:Literal ID="litFeeAffiliation" runat="server" /></td>
                    </tr>
                    <tr>
                        <td align="right">退货状态：</td>
                        <td>
                            <asp:Literal ID="litReturnsHandleStatus1" runat="server" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">处理时间：</td>
                        <td>
                            <asp:Literal ID="litReceiveTime" runat="server" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">物流公司：</td>
                        <td colspan="1">
                            <asp:Literal ID="litLogisticsCompany" runat="server" /></td>
                    </tr>
                    <tr>
                        <td align="right">物流单号：</td>
                        <td colspan="1">
                            <asp:Literal ID="litLogisticsId" runat="server" /></td>
                    </tr>
                    <tr>
                        <td align="right">退货原因：</td>
                        <td colspan="1">
                            <asp:Literal ID="litComments" runat="server" /></td>
                    </tr>
                    <tr>
                        <td align="right">退款途径：</td>
                        <td><span class="colorB">
                            <asp:Literal ID="litRefundType" runat="server" /></span></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div style="display: none">
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function CloseFrameWindow() {
            var win = art.dialog.open.origin;
            win.location.reload();
        }
    </script>
</asp:Content>
