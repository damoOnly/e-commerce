<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UserbrowsehistoryManage.aspx.cs" Inherits="EcShop.UI.Web.Admin.UserbrowsehistoryManage" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>用户浏览历史记录管理</h1>
              <span>记录PC端和微信端商品详情的浏览历史记录</span>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="search clearfix">
                <ul>
                    <li><span>用户名：</span>
                        <span>
                            <asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton" />
                    </li>
                </ul>
            </div>
            <UI:Grid ID="grdBrowseHistory" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="HistoryId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="用户名" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="用户IP" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="lblUserIP" runat="server" Text='<%# Eval("UserIP") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品Id" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="来源平台" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblPlatType" runat="server" Text='<%#  Eval("PlatType").ToString()=="1" ? "PC端": Eval("PlatType").ToString()=="2"?"微信端":"其他" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="浏览时间" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblBrowseTime" runat="server" Text='<%# Eval("BrowseTime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="浏览次数" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblBrowerTimes" runat="server" Text='<%# Eval("BrowerTimes") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="IP" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblIP" runat="server" Text='<%# Eval("IP") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="商品类别ID" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblCategoryId" runat="server" Text='<%# Eval("CategoryId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <span class="submit_shanchu">
                                <Hi:ImageLinkButton ID="lkbDelete" IsShow="true" runat="server" CommandName="Delete" Text="删除" /></span>
                          
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </UI:Grid>
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
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

