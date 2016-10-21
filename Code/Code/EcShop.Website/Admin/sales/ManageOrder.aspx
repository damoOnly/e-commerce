<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ManageOrder.aspx.cs" Inherits="EcShop.UI.Web.Admin.ManageOrder" %>

<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>

<%@ Import Namespace="EcShop.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div>
        <img src="../images/lightbulb.png" /><literal>此处的订单编号查询不受订单状态限制，历史订单列表只显示三个月前的订单</literal>
    </div>
    <div class="optiongroup mainwidth">
        <ul>
            <li id="anchors0">
                <asp:HyperLink ID="hlinkAllOrder" runat="server"><span>所有订单</span></asp:HyperLink></li>
            <li id="anchors1">
                <asp:HyperLink ID="hlinkNotPay" runat="server" Text=""><span>等待买家付款</span></asp:HyperLink></li>
            <li id="anchors2">
                <asp:HyperLink ID="hlinkYetPay" runat="server" Text=""><span>等待发货</span></asp:HyperLink></li>
            <li id="anchors3">
                <asp:HyperLink ID="hlinkSendGoods" runat="server" Text=""><span>已发货</span></asp:HyperLink></li>
            <li id="anchors5">
                <asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>成功订单</span></asp:HyperLink></li>
            <li id="anchors4">
                <asp:HyperLink ID="hlinkClose" runat="server" Text=""><span>已关闭</span></asp:HyperLink></li>
            <li id="anchors99">
                <asp:HyperLink ID="hlinkHistory" runat="server" Text=""><span>历史订单</span></asp:HyperLink></li>
        </ul>
    </div>
    <!--选项卡-->
    <div class="dataarea mainwidth">
        <!--搜索-->
        <style>
            .searcharea ul li {
                padding: 5px 0px;
            }

            a {
                text-decoration: none;
                cursor: pointer;
            }

            .thorderItem {
                font-size: 12px;
                font-weight: normal;
            }

            .dataarea .datalist table td table td.thorderItemtd, .dataarea .datalist table td table th, .dataarea .datalist table .thorderItemtd {
                padding: 0;
                border: none;
            }

            .lblSourdeOrder {
                padding-left: 50px;
            }

            .calendarDate {
                width: 100px;
            }

            .selhour {
                height: 30px;
                margin-left: 2px;
            }
        </style>
        <div class="searcharea clearfix br_search">
            <ul>
                <li><span>
                    <Hi:OrderTimeTypeDropDownList runat="server" ID="ddListOrderTimeType" Width="120" CssClass="forminput" />
                    &nbsp;</span>
                    <span>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput calendarDate" />

                        <Hi:HourDropDownList runat="server" AllowNull="true" ID="ddListStartHour" Width="55" CssClass="forminput selhour" />
                    </span><span class="Pg_1010">至</span> <span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput calendarDate" />
                        <Hi:HourDropDownList runat="server" AllowNull="true" ID="ddListEndHour" Width="55" CssClass="forminput selhour" />
                    </span></li>
                <li style="margin-left: 13px;"><span>会员名：</span><span>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />
                </span></li>
                <li><span>订单编号：</span><span>
                    <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                        runat="server" Style="display: none;"></asp:Label>
                    <asp:Label ID="lblOrderTimeType"
                        runat="server" Style="display: none;"></asp:Label>
                </span></li>
                <li><span>商品名称：</span><span>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" />
                </span></li>
                <li style="margin-left: 23px; _margin-left: 12px;"><span>收货人：</span><span>
                    <asp:TextBox ID="txtShopTo" runat="server" CssClass="forminput"></asp:TextBox>
                </span></li>
                <li style="margin-left: 23px; _margin-left: 12px;"><span>手机：</span><span>
                    <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput"></asp:TextBox>
                </span></li>
                <li><span>打印状态：</span><span>
                    <abbr class="formselect">
                        <asp:DropDownList runat="server" ID="ddlIsPrinted" Width="107" />
                    </abbr>
                </span></li>
                <li><span>供货商：</span><span>
                    <abbr class="formselect">
                        <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" NullToDisplay="请选择" Width="107"></Hi:SupplierDropDownList>
                    </abbr>
                </span></li>
                <li><span>配送方式：</span><span>
                    <abbr class="formselect">
                        <Hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList" Width="100" />
                    </abbr>
                </span></li>
                <li style="margin-left: 23px; _margin-left: 12px;"><span>运单号：</span><span>
                    <asp:TextBox ID="txtShipOrderNumber" runat="server" CssClass="forminput"></asp:TextBox>
                </span></li>
                <li><span>站点：</span><span>
                    <abbr class="formselect">
                        <Hi:SitesDropDownList runat="server" AllowNull="true" ID="sitesDropDownList" Width="100" />
                    </abbr>
                </span></li>

                <li>
                    <abbr class="formselect">
                        <Hi:RegionSelector runat="server" ID="dropRegion" />
                    </abbr>
                </li>
                <li>
                    <abbr class="formselect">
                        <Hi:SourceOrderDrowpDownList runat="server" ID="dropsourceorder" NullToDisplay="订单来源"></Hi:SourceOrderDrowpDownList>
                    </abbr>
                </li>
                <li>
                    <abbr class="formselect">
                        <Hi:PayerIdStatusDownList runat="server" ID="ddlpayerIdStatus" NullToDisplay="实名认证"></Hi:PayerIdStatusDownList>
                    </abbr>
                </li>
                <li>
                    <abbr class="formselect">
                      <asp:DropDownList ID="DDL_IsRefund" runat="server">
                                     <asp:ListItem Value="-1">退款状态</asp:ListItem>
                                     <asp:ListItem Value="3">未退款</asp:ListItem>
                                     <asp:ListItem Value="1">已退款</asp:ListItem>
                                     <asp:ListItem Value="2">退款中</asp:ListItem>
                       </asp:DropDownList>
                    </abbr>
                </li>
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
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
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
                                <Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" /></span>
                        <span class="printorder"><a href="javascript:printPosts()">批量打印快递单</a></span> <span
                            class="printorder"><a href="javascript:printGoods()">批量打印发货单</a></span> <span class="downproduct">
                                <a href="javascript:downOrder()">下载配货单</a></span><%--<span class="printorder">
                                    <a href="javascript:downclearOrder()">清关订单导出</a></span>--%> <span class="sendproducts"><a href="javascript:batchSend()"
                                        onclick="">批量发货</a></span><span class="uploadExcel"><a href="javascript:uploadExcel()" onclick="">批量上传发货单</a></span>
                        <span class="allocateToStore"><a href="javascript:allocateToStore()" onclick="">分配订单</a></span>
                    </li>
                </ul>
            </div>
        </div>
        <input type="hidden" id="hidOrderId" runat="server" />

        <!--数据列表区域-->
        <div class="datalist">
            <asp:DataList ID="dlstOrders" runat="server" DataKeyField="OrderId" Width="100%">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td width="24%" class="td_right td_left">会员名
                            </td>
                            <td width="20%" class="td_right td_left">收货人
                            </td>
                            <td width="18%" class="td_right td_left">支付方式
                            </td>
                            <td width="14%" class="td_right td_left">订单实收款(元)
                            </td>
                            <td width="14%" class="td_right td_left">订单状态
                            </td>
                            <td width="10%" class="td_left td_right_fff">操作
                            </td>

                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="td_bg">
                        <td>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />订单编号：<%#Eval("OrderId") %><Hi:SourceOrderLable runat="server" SourceOrder='<%#Eval("SourceOrder") %>' StoreId='<%#Eval("StoreId") %>'></Hi:SourceOrderLable>
                            <asp:Literal ID="group" runat="server" Text='<%# Convert.ToInt32(Convert.IsDBNull(Eval("GroupBuyId"))?"0":Eval("GroupBuyID"))>0?"(团)":"" %>' />

                            <label runat="server" class="lblSourdeOrder"><%# string.IsNullOrEmpty(Eval("SourceOrderId").ToString()) ? "":(Eval("SourceOrderId").ToString().IndexOf(",") > 0 ? "<a onclick='ShowSourceOrder(this,\""+(Eval("SourceOrderId").ToString())+"\")'>合单</a>": "<a onclick='ShowSourceOrder(this,\""+(Eval("SourceOrderId").ToString())+"\")'>分单</a>") %></label>
                        </td>
                        <td>提交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true"
                            runat="server"></Hi:FormatedTimeLabel>
                        </td>
                        <td>
                            <%# (bool)(Convert.IsDBNull(Eval("IsPrinted")) ? false : Eval("IsPrinted"))? "已打印" : "未打印"%>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                            <%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "物流单号：" + Eval("ShipOrderNumber")%>
                        </td>
                        <td align="right">&nbsp;
                            <a href="javascript:RemarkOrder('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Eval("ManagerMark") %>', '<%#Eval("ManagerRemark")==null?"":Eval("ManagerRemark").ToString().Replace("&#39;","\\&#39;").Replace("&quot;","\\&quot;") %>');">
                                <Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="0" border="0" cellspacing="0" class="thorderItemtd" height="30px" style="background: rgb(246, 250, 241);">
                                <tr>
                                    <td class="thorderItemtd"><a href="<%# Eval("UserId").ToString()!="1100"?"javascript:DialogFrame('"+Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}",Eval("UserId")))+"','查看会员',null,null)":"javascript:void(0)" %>">
                                        <%#Eval("UserName")%></a>
                                        <Hi:WangWangConversations runat="server" ID="WangWangConversations1" WangWangAccounts='<%#Eval("Wangwang") %>' />
                                    </td>
                                    <td class="thorderItemtd"><%#Eval("ShipTo") %>&nbsp;</td>
                                </tr>
                            </table>
                            <table width="0" border="0" cellspacing="0" class="tbOrders">
                                <tr>
                                    <th colspan="2" class="thorderItem" style="border-bottom: 1px #e0dcce solid; width: 300px;">商品名称
                                   <a onclick="OrderItemsDisplay(this)" id="moreOrderItems" style="float: right;">更多</a>
                                    </th>
                                    <th class="thorderItem" style="border-bottom: 1px #e0dcce solid; width: 90px; text-align: right;">商品单价(元)</th>
                                    <th class="thorderItem" style="border-bottom: 1px #e0dcce solid; width: 90px; text-align: right;">购买数量</th>
                                    <th class="thorderItem" style="border-bottom: 1px #e0dcce solid; width: 90px; text-align: right;">税率</th>
                                    <th class="thorderItem" style="border-bottom: 1px #e0dcce solid; width: 90px; text-align: right;">金额&nbsp;</th>
                                </tr>
                                <asp:Repeater ID="repeterOrderItems" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td width="50px" align="left"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                                <Hi:ListImage ID="HiImage2" runat="server" DataField="ThumbnailsUrl" Width="40px" /></a>
                                            </td>
                                            <td width="180px" align="left"><span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                                <%# Eval("ItemDescription")%></a></span> <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' />
                                                    <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
                                            </td>
                                            <td align="right">
                                                <Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' />
                                            </td>
                                            <td align="right">
                                                <asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
                                            <td align="right">
                                                <asp:Literal runat="server" ID="litTaxRate" Text='<%# Eval("TaxRate", "{0:0%}")%>' /></td>
                                            <td align="right"><strong class="colorG">
                                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>

                        <td style="color: #0B5BA5">
                            <%#Eval("PaymentType") %>&nbsp;
                        </td>
                        <td style="font-weight: bold; font-family: Arial;">
                            <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
                            <span class="Name">&nbsp;
                                <asp:HyperLink ID="lkbtnEditPrice" runat="server" NavigateUrl='<%# "EditOrder.aspx?OrderId="+ Eval("OrderId") %>'
                                    Text="修改价格" Visible="false"></asp:HyperLink></span>
                        </td>
                        <td style="color: #0B5BA5; text-align:center">&nbsp;
                            <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' PayDate='<%# Eval("PayDate") %>' runat="server" SourceOrderIdCode='<%# Eval("SourceOrderId") %>' />
                            <span class="Name"><a href='<%# "OrderDetails.aspx?OrderId="+Eval("OrderId") %>'>详情</a></span>
                            <samp class="Name">
                                 <%# Eval("IsRefund").ToString()=="1"?"已退款":""%>
                                 </br><Hi:ImageLinkButton ID="lkbtnrefund" runat="server" Text="执行退款" CommandArgument='<%# Eval("OrderId") %>'
                                    CommandName="CONFIRM_refund" OnClientClick="return Confirmrefund()" Visible="false"
                                    ForeColor="Red"></Hi:ImageLinkButton><br />

                            </samp>
                        </td>
                        <td>&nbsp;
                            <span class="Name">
                                <Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="我已线下收款" CommandArgument='<%# Eval("OrderId") %>'
                                    CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false"
                                    ForeColor="Red"></Hi:ImageLinkButton><br />
                                <a href="javascript:CloseOrder('<%#Eval("OrderId") %>');">
                                    <asp:Literal runat="server" ID="litCloseOrder" Visible="false" Text="关闭订单" /></a>
                                <asp:Label CssClass="submit_faihuo" ID="lkbtnSendGoods" Visible="false" runat="server"> 
                                <a style="color:Red" href="javascript:DialogFrame('<%# "sales/SendOrderGoods.aspx?OrderId="+ Eval("OrderId") %>','订单发货',null,null, function () {  location.reload(); })">发货</a> </asp:Label>
                                <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="完成订单"
                                    CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认要完成该订单吗？"
                                    Visible="false" ForeColor="Red" />
                                <a style="color: Red"></a><a href="javascript:void(0)" onclick="return CheckRefund(this.title)"
                                    runat="server" id="lkbtnCheckRefund" visible="false" title='<%# Eval("OrderId") %>'>确认退款</a>
                                <div style="display: none">
                                    <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server"
                                        id="lkbtnCheckReturn" visible="false" title='<%# Eval("OrderId") %>'>确认退货</a>
                                    <a href="javascript:void(0)" onclick="return CheckReplace(this.title)" runat="server"
                                        id="lkbtnCheckReplace" visible="false" title='<%# Eval("OrderId") %>'>确认换货</a>&nbsp;
                                </div>

                                <Hi:ImageLinkButton ID="lkbtnSetOrderNoPay" IsShow="true" runat="server" Text="设置为未支付订单"
                                    CommandArgument='<%# Eval("OrderId") %>' CommandName="SetOrderNoPay" DeleteMsg="确认要将该订单设为未支付"
                                    Visible="false" ForeColor="Red" />

                            </span>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
            <div class="instantstat clearfix" id="divSendOrders">
                注：订单状态列中“(团)”字的代表团购订单
            </div>
            <div class="blank5 clearfix">
            </div>

            <div class="bottomBatchHandleArea clearfix">
                <div class="batchHandleArea">
                    <ul>
                        <li class="Pg_10 clearfix">当前页总计：<span class="fonts"><asp:Label ID="lblPageCount" runat="server"></asp:Label></span></li>
                    </ul>
                    <ul>
                        <li class="Pg_10 clearfix">当前查询结果合计：<span class="fonts"><asp:Label ID="lblSearchCount" runat="server"></asp:Label></span></li>
                    </ul>
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
    <!--关闭订单--->
    <div id="closeOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em>
            </p>
            <p>
                <span class="frame-span frame-input110">关闭该订单的理由:</span>
                <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" />
            </p>
        </div>
    </div>
    <!--编辑备注--->
    <style>
        .frame-content {
            margin-top: -20px;
        }
    </style>
    <div id="RemarkOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input100">订单号：</span><span id="spanOrderId"></span>
            </p>
            <p>
                <span class="frame-span frame-input100">提交时间：</span><span id="lblOrderDateForRemark"></span>
            </p>
            <p>
                <span class="frame-span frame-input100">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel
                    ID="lblOrderTotalForRemark" runat="server" /></strong>
            </p>
            <span class="frame-span frame-input100">标志：<em>*</em></span><Hi:OrderRemarkImageRadioButtonList
                runat="server" ID="orderRemarkImageForRemark" />
            <p>
                <span>备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server"
                    Width="300" Height="50" />
            </p>
        </div>
    </div>
    <div id="DownOrder" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnorderph" onclick="javascript: Setordergoods();" class="submit_DAqueding"
                value="选中订单导出" />
            &nbsp;
                <input type="button" id="btnorderAllph" onclick="javascript: Setordergoods2();" class="submit_DAqueding"
                    value="按时间段导出" />
            &nbsp;
            <input type="button" id="Button1" onclick="javascript: Setproductgoods();" class="submit_DAqueding"
                value="商品配货表" /><p>
                    <%--      导出内容只包括等待发货状态的订单。--%>
                    <br />
                    选中订单导出导出选中的订单，按时间段导出当前选择的时间段查询出来的所有订单。
            <br />
                    订单配货表不会合并相同的商品,商品配货表则会合并相同的商品。
                </p>
        </div>
    </div>
    <!--清关-->
    <div id="DownClearOrder" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnclearorder" onclick="javascript: ClearOrderGoods();" class="submit_DAqueding"
                value="选中订单导出" />
            &nbsp;
                <input type="button" id="btnclearorderAll" onclick="javascript: ClearOrderGoods2();" class="submit_DAqueding"
                    value="按时间段导出" />
            &nbsp;
            <input type="button" id="btnclearorderph" onclick="javascript: ClearOrderproductgoods();" class="submit_DAqueding"
                value="商品配货表" /><p>
                    <%--       导出订单只包括清单中的订单。--%>
                    <br />
                    选中订单导出导出选中的订单，按时间段导出当前选择的时间段查询出来的所有订单。
            <br />
                    订单配货表不会合并相同的商品,商品配货表则会合并相同的商品。
                </p>
        </div>
    </div>

    <!--新增上传excel发货--->
    <!---上传excel批量发货--->
    <%--        <div id="UploadExcelForSend" style="display: none;">
<div class="aui_state_focus" style="position: fixed; left: 693px; top: 107px; width: auto; z-index: 1989;"><div class="aui_outer"><table class="aui_border"><tbody><tr><td class="aui_nw"></td><td class="aui_n"></td><td class="aui_ne"></td></tr><tr><td class="aui_w"></td><td class="aui_c"><div class="aui_inner"><table class="aui_dialog"><tbody><tr><td colspan="2" class="aui_header"><div class="aui_titleBar"><div class="aui_title" style="cursor: move;">批量上传发货单</div><a class="aui_close  uploadExcelForSend_close" href="javascript:uploadExcelForSendClose();">×</a></div></td></tr><tr><td class="aui_icon" style="display: none;"><div class="aui_iconBg" style="background: none;"></div></td><td class="aui_main" style="width: auto; height: auto;"><div class="aui_content" style="padding: 20px 25px;">
        <div class="frame-content" style="text-align: center;">
            <asp:FileUpload ID="excelFile" runat="server" />          
             &nbsp;
            &nbsp;<br/>
                <asp:Button ID="btnBatchSend" runat="server" CssClass="submit_DAqueding" Text="上传" />
                <p>上传固定格式的excel表格。</p>
        </div>
    </div></td></tr><tr><td colspan="2" class="aui_footer"><div class="aui_buttons" style="display: none;"></div></td></tr></tbody></table></div></td><td class="aui_e"></td></tr><tr><td class="aui_sw"></td><td class="aui_s"></td><td class="aui_se" style="cursor: se-resize;"></td></tr></tbody></table></div></div>
    </div>--%>
    <!--确认退款--->
    <div id="CheckRefund" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：<br />
                    1.买家已付款完成，并确认无误；
                    2.确认买家的申请退款方式。</em>
            </p>

            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                <tr>
                    <td align="right" width="30%">订单号:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">订单金额:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblOrderTotal" runat="server" /></td>
                </tr>
                <tr>
                    <td align="right">买家退款方式:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblRefundType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">退款原因:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblRefundRemark" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系人:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblContacts" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">电子邮件:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblEmail" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系电话:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblTelephone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系地址:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">退优惠券:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:RadioButtonList ID="radBtnList" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="否" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    </asp:RadioButtonList></td>
                </tr>
            </table>

            <p>
                <span class="frame-span frame-input100" style="margin-right: 10px;">管理员备注:</span> <span>
                    <asp:TextBox ID="txtAdminRemark" runat="server" CssClass="forminput" Width="243" /></span>
            </p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button2" onclick="javascript: acceptRefund();" class="submit_DAqueding"
                    value="确认退款" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript: refuseRefund();" class="submit_DAqueding"
                    value="拒绝退款" />
            </div>
        </div>
    </div>
    <!--确认退货--->
    <div id="CheckReturn" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：<br />
                    1.已收到买家寄换回来的货品，并确认无误；
                    2.确认买家的申请退款方式。</em>
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
                    <td align="left" class="bd_td">&nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" /></td>
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
                <span class="frame-span frame-input100" style="margin-right: 10px;">管理员备注:</span> <span>
                    <asp:TextBox ID="return_txtAdminRemark" runat="server" CssClass="forminput" Width="243" /></span>
            </p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button4" onclick="javascript: acceptReturn();" class="submit_DAqueding"
                    value="确认退货" />
                &nbsp;
                <input type="button" id="Button5" onclick="javascript: refuseReturn();" class="submit_DAqueding"
                    value="拒绝退货" />
            </div>
        </div>
    </div>
    <!--确认换货--->
    <div id="CheckReplace" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：<br />
                    1.已收到买家寄还回来的货品，并确认无误；
                </em>
            </p>

            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                <tr>
                    <td align="right" width="30%">订单号:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderId" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">订单金额:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderTotal" runat="server" /></td>
                </tr>
                <tr>
                    <td align="right">换货备注:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblComments" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系人:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblContacts" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">电子邮件:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblEmail" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系电话:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblTelephone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">联系地址:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblAddress" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">邮政编码:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblPostCode" runat="server"></asp:Label></td>
                </tr>
            </table>


            <p>
                <span class="frame-span frame-input100" style="margin-right: 10px;">管理员备注:</span> <span>
                    <asp:TextBox ID="replace_txtAdminRemark" runat="server" CssClass="forminput" Width="243" /></span>
            </p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button6" onclick="javascript: acceptReplace();" class="submit_DAqueding"
                    value="确认换货" />
                &nbsp;
                <input type="button" id="Button7" onclick="javascript: refuseReplace();" class="submit_DAqueding"
                    value="拒绝换货" />
            </div>
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
        <asp:Button ID="btnCloseOrder" runat="server" CssClass="submit_DAqueding" Text="关闭订单" />
        <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="确认退款" />
        <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="拒绝退款" />
        <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="确认退货" />
        <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="拒绝退货" />
        <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="确认换货" />
        <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="拒绝换货" />
        <asp:Button runat="server" ID="btnRemark" Text="编辑备注信息" CssClass="submit_DAqueding" />
        <asp:Button ID="btnOrderGoods" runat="server" CssClass="submit_DAqueding" Text="订单配货表" />&nbsp;
        <asp:Button ID="btnOrderGoods2" runat="server" CssClass="submit_DAqueding" Text="订单配货表2" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="submit_DAqueding" />&nbsp;
        <asp:Button ID="btnClearOrder1" runat="server" CssClass="submit_DAqueding" Text="清关订单" />&nbsp;
        <asp:Button ID="btnClearOrder2" runat="server" CssClass="submit_DAqueding" Text="清关订单2" />&nbsp;
        <asp:Button ID="btnClearOrder3" runat="server" CssClass="submit_DAqueding" Text="清关订单配货表" />&nbsp;
    </div>


    <div id="displaySourceOrder_div" style="display: none">
        <div class="frame-content" style="padding-top: 10px;" id="divframe-content">
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js?v=20150819" type="text/javascript"></script>
    <script type="text/javascript">
        var formtype = "";

        function ConfirmPayOrder() {
            return confirm("如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态\n\n此操作成功完成以后，订单的当前状态将变为已付款状态，确认客户已付款？");
        }
        function Confirmrefund() {
            return confirm("您确定要执行退款吗？");
        }

        function ShowOrderState() {
            var status;
            if (navigator.appName.indexOf("Explorer") > -1) {

                status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

            } else {

                status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

            }
            if (status != "0") {
                document.getElementById("anchors0").className = 'optionstar';
            }
            if (status != "99") {
                document.getElementById("anchors99").className = 'optionend';
            }
            document.getElementById("anchors" + status).className = 'menucurrent';
        }

        $(document).ready(function () {
            ShowOrderState();

            $("#ctl00_contentHolder_calendarEndDate").bind("change", function () {
                var calendarStartDate = $("#ctl00_contentHolder_calendarStartDate").val();
                var startHour = $("#ctl00_contentHolder_ddListStartHour").val();
                var calendarEndDate = $("#ctl00_contentHolder_calendarEndDate").val();
                var endHour = $("#ctl00_contentHolder_ddListEndHour").val();
                if (calendarEndDate != "" && calendarStartDate != ""
                    && calendarEndDate < calendarStartDate) {
                    alert("结束时间不能大于开始时间");
                    $("#ctl00_contentHolder_calendarEndDate").focus();
                    $("#ctl00_contentHolder_calendarEndDate").val("");
                }
            });

        });


        //备注信息
        function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            arrytext = null;
            formtype = "remark";
            $("#ctl00_contentHolder_lblOrderTotalForRemark").html(OrderTotal);
            $("#ctl00_contentHolder_hidOrderId").val(OrderId);
            $("#spanOrderId").html(OrderId);
            $("#lblOrderDateForRemark").html(OrderDate);

            for (var i = 0; i <= 5; i++) {
                if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark) {
                    setArryText("ctl00_contentHolder_orderRemarkImageForRemark_" + i, "true");
                    $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", true);
                }
                else {
                    $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", false);
                }

            }
            setArryText("ctl00_contentHolder_txtRemark", managerRemark);
            DialogShow("修改备注", 'updateremark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
        }

        function CloseOrder(orderId) {
            arrytext = null;
            formtype = "close";
            $("#ctl00_contentHolder_hidOrderId").val(orderId);
            DialogShow("关闭订单", 'closeframe', 'closeOrder', 'ctl00_contentHolder_btnCloseOrder');
        }

        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                alert("请选择关闭的理由");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }

        // 批量打印发货单
        function printGoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要打印的订单");
            }
            else {
                var url = "/Admin/sales/BatchPrintSendOrderGoods.aspx?OrderIds=" + orderIds;
                window.open(url, "批量打印发货单", "width=700, top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=n o, status=no");
            }
        }

        //批量发货
        function batchSend() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要发货的订单");
            }
            else {
                DialogFrame("sales/BatchSendOrderGoods.aspx?OrderIds=" + orderIds, "批量发货", null, null, function () { location.reload(); });
            }
        }
        //上传excel
        function batchSendByExcel() {
            DialogFrame("sales/BathSendOrderByExcel.aspx", "批量发货", 500, 250, function () { location.reload(); });
        }
        function Setordergoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要下载配货单的订单");
            } else {
                $("#ctl00_contentHolder_btnOrderGoods").trigger("click");
            }
        }
        function Setordergoods2() {
            var startTime = $('#ctl00_contentHolder_calendarStartDate').val();
            var endTime = $('#ctl00_contentHolder_calendarEndDate').val();
            if (startTime == '' || endTime == '') {
                alert("请选择时间区域");
            } else {
                $("#ctl00_contentHolder_btnOrderGoods2").trigger("click");
            }
        }
        function Setproductgoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要下载配货单的订单");
            } else {
                $("#ctl00_contentHolder_btnProductGoods").trigger("click");
            }
        }

        /**清关订单开始**/
        function ClearOrderGoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要下载的清关订单");
            } else {
                $("#ctl00_contentHolder_btnClearOrder1").trigger("click");
            }
        }
        function ClearOrderGoods2() {
            var startTime = $('#ctl00_contentHolder_calendarStartDate').val();
            var endTime = $('#ctl00_contentHolder_calendarEndDate').val();
            if (startTime == '' || endTime == '') {
                alert("请选择时间区域");
            } else {
                $("#ctl00_contentHolder_btnClearOrder2").trigger("click");
            }
        }
        function ClearOrderproductgoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要下载的清关订单");
            } else {
                $("#ctl00_contentHolder_btnClearOrder3").trigger("click");
            }
        }
        /**清关订单结束**/


        //批量打印快递单
        function printPosts() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要打印的订单");
            }
            else {
                var url = "sales/BatchPrintData.aspx?OrderIds=" + orderIds;
                DialogFrame(url, "批量打印快递单", null, null);
            }
        }

        //验证
        function validatorForm() {
            switch (formtype) {
                case "remark":
                    arrytext = null;
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
                case "close":
                    return ValidationCloseReason();
                    break;
            };
            return true;
        }
        // 下载配货单
        function downOrder() {
            //var orderIds = "";
            //$("input:checked[name='CheckBoxGroup']").each(function () {
            //    orderIds += $(this).val() + ",";
            //}
            // );
            //if (orderIds == "") {
            //    alert("请选要下载配货单的订单");
            //}
            ShowMessageDialog("下载配货批次表", "downorder", "DownOrder");
        }

        //导出清关订单
        function downclearOrder() {
            //var orderIds = "";
            //$("input:checked[name='CheckBoxGroup']").each(function () {
            //    orderIds += $(this).val() + ",";
            //}
            // );
            //if (orderIds == "") {
            //    alert("请选要下载配货单的订单");
            //}
            ShowMessageDialog("导出清关订单", "downclearOrder", "DownClearOrder");
        }


        function uploadExcel() {
            batchSendByExcel();
            //$('#UploadExcelForSend').show();
            //ShowMessageDialog("批量发货", "uploadExcel", "UploadExcelForSend");
        }
        function allocateToStore() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要分配的订单");
            }
            else {
                DialogFrame("sales/allocateOrderToStore.aspx?OrderIds=" + orderIds, "分配订单到实体店", null, null, function () { location.reload(); });
            }

        }
        function uploadExcelForSendClose() {
            $("#UploadExcelForSend").hide();
        }
        $(function () {
            $(".datalist img[src$='tui.gif']").each(function (item, i) {
                $parent_link = $(this).parent();
                $parent_link.attr("href", "javascript:DialogFrame('sales/" + $parent_link.attr("href") + "','退款详细信息',null,null);");
            });
        });

        function OrderItemsHidden() {
            //debugger;
            $(".tbOrders").each(function (index, ele) {
                $(ele).find("> tbody >tr:gt(1)").hide();
                if ($(ele).find("> tbody >tr").length < 3) {
                    $(".tbOrders>tbody>tr>th>a")[index].style.display = 'none';
                } else if ($(ele).find("> tbody >tr").length >= 3) {
                    $(".tbOrders>tbody>tr>th>a")[index].style.display = '';
                }
            })
        }

        $(document).ready(function () {
            //明细默认显示一条
            OrderItemsHidden();

            /*debugger;
            var options ="";
            for (var i = 0; i <= 24; i++)
            {
                if (i == 0) {
                    options += "<option values='" + i + "' selected>" + i + "</option>";
                } else {
                    options += "<option values='" + i + "'>" + i + "</option>";
                }
            }
            $("#ctl00_contentHolder_selHour").html(options);
            $("#ctl00_contentHolder_selEHour").html(options);*/
        });

        //点击更多显示全部
        function OrderItemsDisplay(ele) {
            var $tr = $(ele).closest("tr").nextAll();

            $tr.each(function (index, tr) {
                if (index > 0) {
                    $(tr).toggle();
                }
            })
        }

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

                $("#divframe-content").html(str);
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
    </script>
</asp:Content>
