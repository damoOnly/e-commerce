<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReplaceApply.aspx.cs" Inherits="EcShop.UI.Web.Admin.sales.ReplaceApply" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <!--搜索-->
        <div class="title">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1>
                换货申请单</h1>
            <span>对换货申请单进行管理</span>
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
                                    <a onclick="OrderReplace(this)">导&nbsp;出</a></span>
                                <span class="printorder excelRefundDetails">
                            <a onclick="OrderReplaceDetails(this)">导出退换详情</a></span>
                        </li>
                    </ul>
                </div>
            </div>
            <input type="hidden" id="hidOrderId" runat="server" />
            <!--数据列表区域-->
            <div class="datalist">
                <asp:DataList ID="dlstReplace" runat="server" DataKeyField="ReplaceId" Width="100%">
                    <HeaderTemplate>
                        <table width="0" border="0" cellspacing="0" style="TABLE-LAYOUT: fixed">
                            <tr class="table_title">
                                <td width="4%" class="td_right td_left">
                                    选择
                                </td>
                                <td width="10%" class="td_right td_left">
                                    订单编号
                                </td>
                                <td width="7%" class="td_right td_left">
                                    订单金额(元)
                                </td>
                                <td width="6%" class="td_right td_left">
                                     退款(元)
                                </td>
                                <td width="8%" class="td_right td_left">
                                    申请时间
                                </td>
                                <td class="td_right td_left" width="12%">
                                    换货备注
                                </td>
                                <td  class="td_right td_left">
                                    处理状态
                                </td>
                                <td  class="td_right td_left">
                                    处理时间
                                </td>
                                <td class="td_right td_left" width="10%">
                                    管理员备注
                                </td>
                                <td class="td_right td_left" width="10%">
                                    订单备注
                                </td>
                                <td  class="td_right td_left">
                                    处理人
                                </td>
                                <td width="12%" class="td_left td_right">
                                    操作
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ReplaceId") %>' />
                            </td>
                            <td>
                                <%# Eval("OrderId") %> &nbsp;

                               <label runat="server">
                                   <%# string.IsNullOrEmpty(Eval("SourceOrderId").ToString()) ? "":(Eval("SourceOrderId").ToString().IndexOf(",") > 0 ? 
                                   "<a onclick='ShowSourceOrder(this,\""+(Eval("SourceOrderId").ToString())+"\")'>合单</a>": 
                                   "<a onclick='ShowSourceOrder(this,\""+(Eval("SourceOrderId").ToString())+"\")'>分单</a>") %>
                               </label>
                            </td>
                            <td>
                                   &nbsp;<Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
                            </td>
                            <td>
                                &nbsp;<Hi:FormatedMoneyLabel ID="lblRefundMoney" Money='<%#Eval("RefundMoney") %>' runat="server" />
                            </td>
                            <td>
                                <%# Eval("ApplyForTime")%>
                            </td>
                            <td style="word-WRAP:break-word">
                                &nbsp;<%# Eval("Comments")%>
                            </td>
                            <td>
                                &nbsp;<asp:Label ID="lblHandleStatus" runat="server" Text='<%# Eval("HandleStatus")%>'></asp:Label>
                            </td>
                            <td>
                                &nbsp;<%# Eval("HandleTime","{0:d}")%>
                            </td>
                            <td style="word-break:break-all;">
                                &nbsp;<%# Eval("AdminRemark")%>
                            </td>
                            <td style="word-break:break-all;">
                                 &nbsp;<%# Eval("Remark")%>
                            </td>
                            <td>
                                &nbsp;<%# Eval("Operator") %>
                            </td>
                            <td>
<%--                             <a href='<%# "OrderDetails.aspx?OrderId="+Eval("OrderId") %>'>详情</a>--%>
                            <a href='<%# "ReplaceDetails.aspx?ReplaceId="+Eval("ReplaceId") %>'>详情</a>
                             <a href="javascript:void(0)" onclick="return CheckReplace(this.title)" runat="server" id="lkbtnCheckReplace" visible="false" title='<%# Eval("OrderId") %>'>确认换货</a>
                              <a href="javascript:void(0)" onclick="DoReceive(this.title,this)" runat="server" id="lkbtnReceive" visible="false" title='<%# Eval("ReplaceId") %>'>领取</a>
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

    <!--确认换货--->
    <div id="CheckReplace" style="display: none;">
        <div class="frame-content"  style="margin-top:-20px;">
            <p>
                <em>执行本操作前确保：<br />1.已收到买家寄还回来的货品，并确认无误；
                    </em></p>
             <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                  <tr>
                    <td align="right" width="30%">订单号:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderId" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">订单金额:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderTotal" runat="server" /></td>
                  </tr>
                  <tr>
                    <td align="right">换货备注:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblComments" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">联系人:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblContacts" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">电子邮件:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblEmail" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">联系电话:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblTelephone" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">联系地址:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblAddress" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">邮政编码:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="replace_lblPostCode" runat="server"></asp:Label></td>
                  </tr>
                </table>
        
            <p>
                <span class="frame-span frame-input100" style=" width:70px;  ">管理员备注:</span> <span>
                    <asp:TextBox ID="replace_txtAdminRemark" runat="server" CssClass="forminput" style="margin-left:10px;" /></span></p>
           
            <div style="text-align: center; padding-top:10px;">
                <input type="button" id="Button6" onclick="javascript:acceptReplace();" class="submit_DAqueding"
                    value="确认换货" />
                &nbsp;
                <input type="button" id="Button7" onclick="javascript:refuseReplace();" class="submit_DAqueding"
                    value="拒绝换货" />
            </div>
        </div>
    </div>

    <!--导出换货申请单-->
    <div id="OrderReplace" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnSelExport" onclick="javascript: ExReplaceOrders();" class="submit_DAqueding"
                value="选中单号导出" />
            &nbsp;
                <input type="button" id="btnTimeExport" onclick="javascript: ExReplaceOrdersTime();" class="submit_DAqueding"
                    value="按时间段导出" />
            &nbsp;
        </div>
    </div>
    <!--导出退换详情-->
      <div id="OrderReplaceDetails" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnSelExport" onclick="javascript: ExReplaceOrdersDetails();" class="submit_DAqueding"
                value="选中单号导出" />
            &nbsp;
                <input type="button" id="btnTimeExport" onclick="javascript: ExReplaceOrdersDetailsTime();" class="submit_DAqueding"
                    value="按时间段导出" />
            &nbsp;
        </div>
    </div>

    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="确认换货" />
        <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="拒绝换货" />

        <asp:Button ID="btnExcelOrderReplace" runat="server" CssClass="submit_DAqueding" Text="导出退货申请单(按选中单号)" />&nbsp;
        <asp:Button ID="btnExcelOrderReplaceTime" runat="server" CssClass="submit_DAqueding" Text="导出退货申请单(按时间段)" />&nbsp;

        <asp:Button ID="btnExcelOrderReplaceDetails" runat="server" CssClass="submit_DAqueding" Text="导出退货详情(按选中单号)" />&nbsp;
        <asp:Button ID="btnExcelOrderReplaceDetailsTime" runat="server" CssClass="submit_DAqueding" Text="导出换货详情(按时间段)" />&nbsp;
    </div>

    <div id="displaySourceOrder_div" style="display:none">
    <div class="frame-content"></div>
    </div>
<script type="text/javascript">
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

    function DoReceive(ReplaceId, ele) {
        var curr = $(ele);
        $.ajax({
            url: "/admin/sales/OrderProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "OrderReplaceReceive", ReplaceId: ReplaceId },
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

    //导出换货申请单
    function OrderReplace(elem) {
        var $elem = $(elem);
        var lefttop = $elem.offset();
        var dialog = ShowMessageDialogOrder("导出退款申请单", "orderReplace", "OrderReplace");
        lefttop.left = lefttop.left + $elem.outerWidth() + 2;
        dialog.DOM.wrap.css(lefttop);
    }

    /**导出换货申请单开始**/
    function ExReplaceOrders() {
        var ReplaceIds = "";
        $("input:checked[name='CheckBoxGroup']").each(function (index, domEle) {
            var tr = $(domEle).parent().parent().css("display");
            if (tr != "none") {
                ReplaceIds += $(this).val() + ",";
            }
        });
        if (ReplaceIds == "") {
            alert("请选择要导出的换货申请单");
        } else {
            $("#ctl00_contentHolder_btnExcelOrderReplace").trigger("click");
        }
    }
    function ExReplaceOrdersTime() {
        var startTime = $('#ctl00_contentHolder_calendarStartDate').val();
        var endTime = $('#ctl00_contentHolder_calendarEndDate').val();
        if (startTime == '' || endTime == '') {
            alert("请选择时间区域");
        } else {
            $("#ctl00_contentHolder_btnExcelOrderReplaceTime").trigger("click");
        }
    }
    /**导出换货申请单结束**/
    function OrderReplaceDetails(elem) {
        var $elem = $(elem);
        var lefttop = $elem.offset();
        var dialog = ShowMessageDialogOrder("导出退换详情", "orderReplaceDetails", "OrderReplaceDetails");
        lefttop.left = lefttop.left + $elem.outerWidth() + 2;
        dialog.DOM.wrap.css(lefttop);
    }

    function ExReplaceOrdersDetails() {
        var ReplaceIds = "";
        $("input:checked[name='CheckBoxGroup']").each(function (index, domEle) {
            var tr = $(domEle).parent().parent().css("display");
            if (tr != "none") {
                ReplaceIds += $(this).val() + ",";
            }
        });
        if (ReplaceIds == "") {
            alert("请选择要导出的换货申请单");
        } else {
            $("#ctl00_contentHolder_btnExcelOrderReplaceDetails").trigger("click");
        }
    }
    function ExReplaceOrdersDetailsTime() {
        var startTime = $('#ctl00_contentHolder_calendarStartDate').val();
        var endTime = $('#ctl00_contentHolder_calendarEndDate').val();
        if (startTime == '' || endTime == '') {
            alert("请选择时间区域");
        } else {
            $("#ctl00_contentHolder_btnExcelOrderReplaceDetailsTime").trigger("click");
        }
    }
</script>
</asp:Content>
