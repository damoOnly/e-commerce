<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SitesManagement.aspx.cs" Inherits="EcShop.UI.Web.Admin.SitesManagement" %>

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
            <h1>站点管理</h1>
        </div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="AddSites.aspx" class="submit_jia">添加新站点</a>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="search clearfix">
                <ul>
                    <li><span>站点名称：</span>
                        <span>
                            <asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton" />
                    </li>
                </ul>
            </div>
            <UI:Grid ID="grdSupplier" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="SitesId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="站点名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                        <ItemTemplate>
                            <asp:Label ID="lblSitesName" runat="server" Text='<%# Eval("SitesName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSitesName" runat="server" Text='<%# Eval("SitesName") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="站点编码" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("Code") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="是否默认站点" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblIsDefault" runat="server" Text='<%# Eval("IsDefault") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtIsDefault" runat="server" Text='<%# Eval("IsDefault") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="排序" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblSort" runat="server" Text='<%# Eval("Sort") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSort" runat="server" Text='<%# Eval("Sort") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="备注描述" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <span class="submit_bianji">
                                <asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditSites.aspx?SitesId="+Eval("SitesId")%>'></asp:HyperLink></span>
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

