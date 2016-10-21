<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.App.IosUpgradeList" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>Ios升级管理</h1>
            <span>设置Ios升级信息</span>
        </div>
        <!-- 添加按钮-->
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li>
                        <a class="submit_jia" href="EditIosVersion.aspx?Id=0">添加Ios版本</a>
                        <input runat="server" type="hidden" id="txtHid" />
                    </li>
                </ul>
            </div>
            <asp:Repeater ID="rptIosUpgrades" runat="server" onitemcommand="rptIosUpgrades_ItemCommand">
                <HeaderTemplate>
                    <!--头部模板,放表格开始及第一行标题-->
                    <table class="ts">
                        <!--插入表格时只需插入两行即可，显示数据时是根据数据库表循环显示的-->
                        <tr>
                            <th>版本号</th>
                            <th>版本描述</th>
                            <th>链接地址</th>
                            <th>是否强制升级</th>
                            <th>操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <!--项目模板，会进行循环显示，放置表格第二行-->
                    <tr>
                        <td>
                            <%#Decimal.Parse(Eval("Version").ToString()).ToString("0.00") %>
                            <!--HTMl中插入其他代码需要用<% %>括起来，Eval("数据库中的字段名")-->
                        </td>
                        <td>
                            <%#Eval("Description")%> </td>
                        <td>
                            <%#Eval("UpgradeUrl")%> </td>
                        <td>
                            <%#Eval("IsForcibleUpgrade")%></td>
                        <td>
                            <span class="submit_bianji"><a href="EditIosVersion.aspx?Id=<%# Eval("Id")%>" id="dd">编辑</a></span>
                            <span class="submit_shanchu">
                                <Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete" CommandArgument='<%#Eval("Id") %>' /></span>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <!--底部模板-->
                    </table>       
                    <!--表格结束部分-->
                </FooterTemplate>
            </asp:Repeater>

        </div>
        <!--数据列表底部功能区域-->
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

