<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ReturnsApply.aspx.cs" Inherits="EcShop.UI.Web.Admin.sales.ReturnsApply" %>

<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js?v=20150918" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <!--搜索-->
        <div class="title">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1>退货申请单</h1>
            <span>对退货申请单进行管理</span>
        </div>
        <div class="datalist">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>订单编号：</span><span>
                        <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                            runat="server" Style="display: none;"></asp:Label>
                    </span></li>
                    <li><span>处理状态：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList runat="server" ID="ddlHandleStatus">
                                <asp:ListItem Value="-1">所有状态</asp:ListItem>
                                <asp:ListItem Value="0">待处理</asp:ListItem>
                                <asp:ListItem Value="3">已受理</asp:ListItem>
                                <asp:ListItem Value="1">已完成</asp:ListItem>
                                <asp:ListItem Value="2">已拒绝</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span></li>
                    <li><span>供货商：</span><span>
                    <abbr class="formselect">
                         <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" NullToDisplay="请选择" Width="107"></Hi:SupplierDropDownList>
                    </abbr>
                </span></li>
                    <li><span>申请时间：</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                    <li><span>处理人：</span><span>
                        <asp:TextBox ID="txtHandler" runat="server" CssClass="forminput" />
                    </span></li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
                    </li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix m_none">
                <!--分页功能-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                    </div>
                </div>
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                                <a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span> <span class="delete">
                                    <Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" />
                                </span>
                            <span class="printorder excelOrder">
                                <a onclick="OrderReturns(this)">导&nbsp;出</a></span>
                            <span class="printorder excelRefundDetails">
                                <a onclick="OrderReturnsDetails(this)">导出退货详情</a></span>
                        </li>
                    </ul>
                </div>
            </div>
            <input type="hidden" id="hidOrderId" runat="server" />
            <!--数据列表区域-->
            <div class="datalist">
                <asp:DataList ID="dlstReturns" runat="server" DataKeyField="ReturnsId" Width="100%">
                    <HeaderTemplate>
                        <table width="0" border="0" cellspacing="0" style="table-layout: fixed">
                            <tr class="table_title">
                                <td width="4%" class="td_right td_left">选择
                                </td>
                                <td width="10%" class="td_right td_left">订单编号
                                </td>
                                <td width="7%" class="td_right td_left">订单金额(元)
                                </td>
                                <td width="6%" class="td_right td_left">退款(元)
                                </td>
                                <td width="8%" class="td_right td_left">申请时间
                                </td>
                                <td class="td_right td_left" width="12%">退款备注
                                </td>
                                <td class="td_right td_left">处理状态
                                </td>
                                <td class="td_right td_left">处理时间
                                </td>
                                <td class="td_right td_left" width="10%">管理员备注
                                </td>
                                <td class="td_right td_left" width="10%">订单备注
                                </td>
                                <td class="td_right td_left">处理人
                                </td>
                                <td width="12%" class="td_left td_right">操作
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ReturnsId") %>' />
                            </td>
                            <td>
                                <%# Eval("OrderId") %>&nbsp;

                               <label runat="server">
                                   <%# string.IsNullOrEmpty(Eval("SourceOrderId").ToString()) ? "":(Eval("SourceOrderId").ToString().IndexOf(",") > 0 ? 
                                   "<a onclick='ShowSourceOrder(this,\""+(Eval("SourceOrderId").ToString())+"\")'>合单</a>": 
                                   "<a onclick='ShowSourceOrder(this,\""+(Eval("SourceOrderId").ToString())+"\")'>分单</a>") %>
                               </label>

                            </td>
                            <td>&nbsp;<Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
                            </td>
                            <td>&nbsp;<Hi:FormatedMoneyLabel ID="lblRefundMoney" Money='<%#Eval("RefundMoney") %>' runat="server" />
                            </td>
                            <td>&nbsp;<%# Eval("ApplyForTime")%></td>
                            <td style="word-wrap: break-word">&nbsp;<%# Eval("Comments")%></td>
                            <td>&nbsp;<asp:Label ID="lblHandleStatus" runat="server" Text='<%# Eval("HandleStatus")%>'></asp:Label>
                            </td>
                            <td>&nbsp;<%# Eval("HandleTime", "{0:d}")%></td>
                            <td style="word-break: break-all;">&nbsp;<%# Eval("AdminRemark")%></td>
                            <td style="word-break: break-all;">&nbsp;<%# Eval("Remark")%></td>
                            <td>&nbsp;<%# Eval("Operator") %></td>
                            <td>
                                <%--                              <a href='<%# "OrderDetails.aspx?OrderId="+Eval("OrderId") %>'>详情</a>--%>
                                <a href='<%# "ReturnsDetails.aspx?ReturnsId="+Eval("ReturnsId") %>'>详情</a>
                                <a href="javascript:void(0)" onclick="return CheckReturn(this.title,this)" runat="server" id="lkbtnCheckReturns" visible="false" aid='<%# Eval("ReturnsId") %>' title='<%# Eval("OrderId") %>'>确认退货</a>
                                <a href="javascript:void(0)" onclick="DoReceive(this.title,this)" runat="server" id="lkbtnReceive" visible="false" title='<%# Eval("ReturnsId") %>'>领取</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:DataList>
                <div class="blank5 clearfix">
                </div>
            </div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--确认退货--->
    <div id="CheckReturn" style="display: none;">
        <div class="frame-content" style="margin-top: -20px;">
            <div style="max-height:450px;overflow:auto;">
            <p>
                <em>执行本操作前确保：<br />
                    1.已收到买家寄换回来的货品，并确认无误；
                    2.确认买家的申请退款方式；<br />
                    3.最大允许退款金额=退货商品金额+订单税费+订单运费</em>
            </p>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                <tr>
                    <td align="right" width="30%">订单号:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblOrderId" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">订单金额:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblOrderTotal" runat="server" /></td>
                </tr>
                <tr>
                    <td align="right">允许退款金额:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblCanRefundMoney" runat="server" /></td>
                </tr>
                <tr>
                    <td align="right">买家退款方式:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblRefundType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">退货原因:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系人:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblContacts" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">电子邮件:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblEmail" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系电话:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblTelephone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系地址:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblAddress" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">退款金额:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" CssClass="forminput" /></td>
                </tr>
                <tr>
                    <td align="right">收取清关费:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:TextBox ID="return_txtCustomsClearanceFee" runat="server" CssClass="forminput" /></td>
                </tr>
                <tr>
                    <td align="right">收取快递费:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:TextBox ID="return_txtExpressFee" runat="server" CssClass="forminput" /></td>
                </tr>
                <tr>
                    <td align="right">费用归属:</td>
                    <td align="left" class="bd_td">&nbsp;
                    <asp:DropDownList ID="return_dropFeeAffiliation" runat="server" CssClass="forminput">
                        <asp:ListItem Value="1" Text="客户承担"></asp:ListItem>
                        <asp:ListItem Value="2" Text="公司承担"></asp:ListItem>
                        <asp:ListItem Value="3" Text="供应商承担"></asp:ListItem>
                    </asp:DropDownList></td>
                </tr>
            </table>
            <p>
                <span class="frame-span frame-input100" style="margin-left: 10px;">管理员备注:</span> <span>
                    <asp:TextBox ID="return_txtAdminRemark" runat="server" CssClass="forminput" Style="margin-left: 10px;" Width="243" /></span>
            </p>
            </div>
            <div style="text-align: center; padding-top: 10px;">
                <input type="button" id="Button4" onclick="acceptReturn();" class="submit_DAqueding"
                    value="确认退货" />
                &nbsp;
                <input type="button" id="Button5" onclick="javascript: refuseReturn();" class="submit_DAqueding"
                    value="拒绝退货" />
            </div>
        </div>
    </div>

    <!--导出退货申请单-->
    <div id="OrderReturns" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnSelExport" onclick="javascript: ExReturnsOrders();" class="submit_DAqueding"
                value="选中单号导出" />
            &nbsp;
                <input type="button" id="btnTimeExport" onclick="javascript: ExReturnsOrdersTime();" class="submit_DAqueding"
                    value="按时间段导出" />
            &nbsp;
        </div>
    </div>
    <div id="OrderReturnsDetails" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnSelExport1" onclick="javascript: ExReturnsOrdersDetalis();" class="submit_DAqueding"
                value="选中单号导出" />
            &nbsp;
                <input type="button" id="btnTimeExport1" onclick="javascript: ExReturnsOrdersDetailsTime();" class="submit_DAqueding"
                    value="按时间段导出" />
            &nbsp;
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <input type="hidden" id="hidCustomsClearanceFee" runat="server" />
        <input type="hidden" id="hidExpressFee" runat="server" />
        <input type="hidden" id="hidFeeAffiliation" runat="server" />
        <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="确认退货" />
        <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="拒绝退货" />

        <asp:Button ID="btnExcelOrderReturns" runat="server" CssClass="submit_DAqueding" Text="导出退货申请单(按选中单号)" />&nbsp;
        <asp:Button ID="btnExcelOrderReturnsTime" runat="server" CssClass="submit_DAqueding" Text="导出退货申请单(按时间段)" />&nbsp;

        <asp:Button ID="btnExcelOrderReturnsDetails" runat="server" CssClass="submit_DAqueding" Text="导出退货详情(按选中单号)" />&nbsp;
        <asp:Button ID="btnExcelOrderReturnsDetailsTime" runat="server" CssClass="submit_DAqueding" Text="导出退货详情(按时间段)" />&nbsp;
    </div>
    <div id="displaySourceOrder_div" style="display: none">
        <div class="frame-content"></div>
    </div>
    <script type="text/javascript">

        $(function () {
            //验证退款金额...
            $("#ctl00_contentHolder_btnAcceptReturn").click(function () {
                var returnMoney = $("#ctl00_contentHolder_return_txtRefundMoney").val();
                if (isNaN(returnMoney) || returnMoney == "") {
                    alert("请输入数字金额");
                    return false;
                }
                returnMoney = parseFloat(returnMoney);
                var canReturnMoney = $("#ctl00_contentHolder_return_lblCanRefundMoney").html();
                if (!isNaN(canReturnMoney)) {
                    canReturnMoney = parseFloat(canReturnMoney);
                    if (canReturnMoney > 0 && returnMoney > canReturnMoney) {
                        alert("退款金额不能大于允许退款的最大金额");
                        return false;
                    }
                } else {
                    alert("允许的最大金额异常");
                    return false;
                }
                //清关费
                var customsClearanceFee = $("#ctl00_contentHolder_return_txtCustomsClearanceFee").val();
                if (isNaN(customsClearanceFee) || customsClearanceFee == "") {
                    alert("清关费用输入错误！");
                    return false;
                }
                var expressFee = $("#ctl00_contentHolder_return_txtExpressFee").val();
                if (isNaN(expressFee) || expressFee == "") {
                    alert("快递费用输入错误");
                    return false;
                }
            })
        })
        //显示来源单详情(合单或分单)
        function ShowSourceOrder(elem, sourceOrderId) {
            if (sourceOrderId == null || sourceOrderId == "" || sourceOrderId == undefined) {
                alert("来源订单不能为空!");
                return false;
            }
            else {
                var $elem = $(elem);
                var lefttop = $elem.offset();

                var str = "<table>";

                //合单
                if (sourceOrderId.indexOf(',') > 0) {
                    var sourceIdArr = sourceOrderId.split(',');
                    if (sourceIdArr != null && sourceIdArr.length > 0) {

                        for (var i = 0; i < sourceIdArr.length; i++) {
                            if (sourceIdArr[i] != null && sourceIdArr[i] != "" && sourceIdArr[i] != undefined) {
                                str += "<tr><td>来源单号" + (i + 1) + "：</td><td><a href='../../admin/default.html?sales/OrderDetails.aspx?OrderId=" + sourceIdArr[i] + "' target='_blank'>" + sourceIdArr[i] + "</a></td></tr>";
                            }
                        }
                    }
                }
                    //分单
                else {
                    str += "<tr><td>来源单号：</td><td><a href='../../admin/default.html?sales/OrderDetails.aspx?OrderId=" + sourceOrderId + "' target='_blank'>" + sourceOrderId + "</a></td></tr>";
                }
                str += "</table>";

                $(".frame-content").html(str);
                /*
                var scrolltop = $(window).scrollTop();               
                if (scrolltop > 0) {
                    lefttop.top = lefttop.top - scrolltop;
                }*/
                var dialog = DialogShowSourceOrder("显示来源单详情", "displaytag", "displaySourceOrder_div", "ctl00_contentHolder_displaytag");
                lefttop.left = lefttop.left + $elem.outerWidth() + 2;
                dialog.DOM.wrap.addClass("posa").css(lefttop);
            }
        }

        function DoReceive(ReturnsId, ele) {
            var curr = $(ele);
            $.ajax({
                url: "/admin/sales/OrderProcess.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "OrderReturnsReceive", ReturnsId: ReturnsId },
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        alert("领取成功");
                        (curr.parent().parent()).css("display", "none");
                    }
                    else if (resultData.Status == "-1") {
                        alert("领取失败, 请重试");
                    }
                    else {
                        alert("领取失败, 请重试");
                    }
                }
            });
        }

        //导出退货申请单
        function OrderReturns(elem) {
            var $elem = $(elem);
            var lefttop = $elem.offset();
            var dialog = ShowMessageDialogOrder("导出退货申请单", "orderReturns", "OrderReturns");
            lefttop.left = lefttop.left + $elem.outerWidth() + 2;
            dialog.DOM.wrap.css(lefttop);
        }

        function OrderReturnsDetails(elem) {
            var $elem = $(elem);
            var lefttop = $elem.offset();
            var dialog = ShowMessageDialogOrder("导出退货详情", "OrderReturnsDetails", "OrderReturnsDetails");
            lefttop.left = lefttop.left + $elem.outerWidth() + 2;
            dialog.DOM.wrap.css(lefttop);
        }

        /**导出退货申请单开始**/
        function ExReturnsOrders() {
            var ReturnsIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function (index, domEle) {
                var tr = $(domEle).parent().parent().css("display");
                if (tr != "none") {
                    ReturnsIds += $(this).val() + ",";
                }
            });
            if (ReturnsIds == "") {
                alert("请选择要导出的退货申请单");
            } else {
                $("#ctl00_contentHolder_btnExcelOrderReturns").trigger("click");
            }
        }
        function ExReturnsOrdersTime() {
            var startTime = $('#ctl00_contentHolder_calendarStartDate').val();
            var endTime = $('#ctl00_contentHolder_calendarEndDate').val();
            if (startTime == '' || endTime == '') {
                alert("请选择时间区域");
            } else {
                $("#ctl00_contentHolder_btnExcelOrderReturnsTime").trigger("click");
            }
        }
        /**导出退货申请单结束**/

        function ExReturnsOrdersDetalis() {
            var ReturnsIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function (index, domEle) {
                var tr = $(domEle).parent().parent().css("display");
                if (tr != "none") {
                    ReturnsIds += $(this).val() + ",";
                }
            });
            if (ReturnsIds == "") {
                alert("请选择要导出的退货申请单");
            } else {
                $("#ctl00_contentHolder_btnExcelOrderReturnsDetails").trigger("click");
            }
        }
        function ExReturnsOrdersDetailsTime() {
            var startTime = $('#ctl00_contentHolder_calendarStartDate').val();
            var endTime = $('#ctl00_contentHolder_calendarEndDate').val();
            if (startTime == '' || endTime == '') {
                alert("请选择时间区域");
            } else {
                $("#ctl00_contentHolder_btnExcelOrderReturnsDetailsTime").trigger("click");
            }
        }
    </script>
</asp:Content>
