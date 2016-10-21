<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.ReconciliationOrders" CodeBehind="ReconciliationOrders.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">
        var $div = $("div[class='dataarea mainwidth databody']");
        var divstyle = "display:none; position: absolute; z-index: 500; top: 0px; left: 0px; opacity: 0.4; background-color: rgb(51, 51, 51);";
        $("#popDivLock1").attr("style", divstyle + "height:" + $div.height() + ";width:" + $div.width());
        function UploadingClick() {
            //弹出遮罩层
            $("#popDivLock1").show();
            $("#divUpload").show();
            //<div id="popDivLock" style="height: 645px; width: 1745px; display: block; background-color: rgb(51, 51, 51); position: absolute; z-index: 500; top: 0px; left: 0px; opacity: 0.4;"></div>
        }
        function hideDiv() {
            $("#popDivLock1").hide();
            $("#divUpload").hide();
        }
    </script>    
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>对账订单报表</h1>
            <span>统计订单信息</span>
        </div>
        <div id="popDivLock1" style="display:none; height: 645px; width: 1745px; position: absolute; z-index: 500; top: 0px; left: 0px; opacity: 0.4; background-color: rgb(51, 51, 51);"></div>
        <div id="divUpload" style="display: none;height:200px; background: #fff none repeat scroll 0 0; left: 0; margin: 0 auto; opacity: 1; position: absolute; right: 0; text-align: center; vertical-align: middle; width: 450px;padding:25px 5px 5px; z-index: 501;">
            <table style="text-align: left; width: 100%;">
                <tr>
                    <td colspan="2">
                        <span class="pop-close" onclick="hideDiv()"></span></td>
                </tr>
                <tr>
                    <td width="120px">对账单类型：</td>
                    <td>
                        <asp:DropDownList ID="ddlUploadingCheck" runat="server">
                            <asp:ListItem Value="1" Text="支付宝对账单"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Excel路径：</td>
                    <td>
                        <asp:FileUpload ID="fileCheck" runat="server"/></td>
                </tr>
                <tr>
                    <td></td>
                    <td class="submit_btnshangjia">
                        <asp:LinkButton ID="linkUpload" runat="server" Text="上传" /></td>
                </tr>
            </table>
        </div>
        <!--数据列表区域-->
        <div class="datalist">
            <!--搜索-->
            <!--结束-->
            <div class="searcharea clearfix ">
                <ul class="a_none_left">
                    <li><span>交易时间从：</span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" class="forminput" /></li>
                    <li><span>至：</span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate" class="forminput" /></li>
                    <li>
                        <asp:Button ID="btnQueryBalanceDetails" runat="server" Text="查询" class="searchbutton" />
                    </li>
                    <li>
                        <p>
                            <asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" />
                        </p>
                    </li>
                    <li>
                        <p><a id="btnUploadingCheck" href="#" onclick="UploadingClick()">上传对账单</a></p>

                    </li>
                </ul>
            </div>
            <div class="functionHandleArea clearfix">
                <!--分页功能-->
                <div class="pageHandleArea" style="float: left;">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>

                </div>

                <div class="pageNumber" style="float: right;">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                    </div>
                </div>
                <!--结束-->
            </div>
            <div style="width: 100%; overflow: auto;">
                <UI:Grid ID="grdBalanceDetails" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="3000px">
                    <columns>
                        <asp:TemplateField HeaderText="订单号" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("OrderId") %>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="付款日期" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("TradingTime") %>
                            </itemtemplate>
                        </asp:TemplateField>
                        <Hi:MoneyColumnForAdmin HeaderText="实际金额" DataField="ActualAmount" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="收款金额" DataField="TotalAmount" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="退款金额" DataField="RefundAmount" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField HeaderText="买家昵称" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("RealName") %>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="拍单时间" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("OrderDate") %>
                            </itemtemplate>
                        </asp:TemplateField>
                        <Hi:MoneyColumnForAdmin HeaderText="商品总金额" DataField="Amount" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="订单金额" DataField="OrderTotal" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="优惠金额" DataField="DiscountAmount" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="运费" DataField="AdjustedFreight" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="税额" DataField="Tax" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="优惠券" DataField="CouponValue" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="现金券" DataField="VoucherValue" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="货到付款服务费" DataField="PayCharge" HeaderStyle-CssClass="td_right td_left" />

                        <asp:TemplateField HeaderText="订单备注" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("ManagerRemark") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="买家留言" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("Remark") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="收货地址" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("Address") %>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="收货人名称" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("ShipTo") %>
                            </itemtemplate>
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="州/省" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("Province") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="城市" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("City") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="区" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("Area") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="邮编" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("ZipCode") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="联系电话" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("TelPhone") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="手机" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("CellPhone") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="买家选择物流" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("ModeName") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="订单状态" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("OrderStatus") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="发货快递单号" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("ShipOrderNumber") %>
                            </itemtemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="快递公司" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <%# Eval("ExpressCompanyName") %>
                            </itemtemplate>
                        </asp:TemplateField>

                    </columns>
                </UI:Grid>

                <div class="blank12 clearfix"></div>
            </div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>

