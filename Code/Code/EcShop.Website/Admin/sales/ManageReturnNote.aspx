﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageReturnNote.aspx.cs" Inherits="EcShop.UI.Web.Admin.sales.ManageReturnNote" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="EcShop.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <!--搜索-->
        <div class="title">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1>
               退货单</h1>
            <span>对退货单进行管理</span>
        </div>
        <div class="datalist">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>订单编号：</span><span>
                        <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                            runat="server" Style="display: none;"></asp:Label>
                    </span>
                    </li>
                    <li><span>退款时间：</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
                    </li>
                     <li>
                            <p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p>
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
                                </span></li>
                    </ul>
                </div>
            </div>
            <input type="hidden" id="hidOrderId" runat="server" />
            <!--数据列表区域-->
            <div class="datalist">
                <asp:DataList ID="dlstReturnNote" runat="server" DataKeyField="ReturnsId" Width="100%">
                    <HeaderTemplate>
                        <table width="0" border="0" cellspacing="0" style="TABLE-LAYOUT: fixed">
                            <tr class="table_title">
                                <td width="5%" class="td_right td_left">
                                    选择
                                </td>
                                <td width="8%" class="td_right td_left">
                                    退货单号
                                </td>
                                <td width="8%" class="td_right td_left">
                                    订单号
                                </td>
                                <td width="6%" class="td_right td_left">
                                    会员
                                </td>
                                <td width="15%" class="td_right td_left">
                                    退货备注
                                </td>
                                <td width="8%" class="td_right td_left">
                                    退款金额(元)
                                </td>
                                <td width="10%" class="td_right td_left">
                                    退货时间
                                </td>
                                 <td width="6%" class="td_right td_left">
                                    操作人
                                </td>
                                <td width="15%" class="td_left td_right">
                                    管理员备注
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ReturnsId") %>' />
                            </td>
                            <td>
                                <%# Eval("ReturnsId")%>
                            </td>
                            <td>
                                <%#Eval("OrderId") %>
                            </td>
                            <td>
                                <%# Eval("Username")%>
                            </td>     
                             <td style="word-WRAP:break-word">
                                <%# Eval("Comments")%>
                            </td>
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("RefundMoney") %>' runat="server" />
                            </td>                     
                            <td>
                                <%# Eval("HandleTime")%>
                            </td>
                            <td style="word-break:break-all;">
                                <%# Eval("Operator")%>
                            </td>
                            <td style="word-break:break-all;">
                                <%# Eval("AdminRemark")%>
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
</asp:Content>
