<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageSupplier.aspx.cs" Inherits="EcShop.UI.Web.Admin.ManageSupplier" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>供货商</h1>
        </div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="AddSupplier.aspx" class="submit_jia">添加新供货商</a>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="search clearfix">
                <ul>
                    <li><span>供货商名称：</span>
                        <span>
                            <asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton" />
                    </li>
                </ul>
            </div>
            <UI:Grid ID="grdSupplier" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="SupplierId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="供货商名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                        <ItemTemplate>
                            <asp:Label ID="lblTypeName" runat="server" Text='<%# Eval("SupplierName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTypeName" runat="server" Text='<%# Eval("SupplierName") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField  HeaderText="供货商编码" DataField="SupplierCode" NullDisplayText="-" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%" />
                    <asp:BoundField  HeaderText="店铺名称" DataField="ShopName" NullDisplayText="-" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%" />                
                    <asp:TemplateField HeaderText="电话" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPhone" runat="server" Text='<%# Eval("Phone") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="手机" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblMobile" runat="server" Text='<%# Eval("Mobile") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMobile" runat="server" Text='<%# Eval("Mobile") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <span class="submit_bianji">
                                <asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditSupplier.aspx?SupplierId="+Eval("SupplierId")%>'></asp:HyperLink></span>
                            <span class="submit_shanchu">
                                <Hi:ImageLinkButton ID="lkbDelete" IsShow="true" runat="server" CommandName="Delete" Text="删除" /></span>
                                <span class="submit_createuser">
                                    <a  class="createuser"  href="javascript:addManager(<%# Eval("SupplierId") %>);">添加管理员</a></span>
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
        <script type="text/javascript" language="javascript">            
            function addManager(supplierId) {
                $.ajax({
                    url: "/admin/sales/OrderProcess.ashx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: { action: "CheckIsPrivilege", rightname: "SupplierAddManager" },
                    success: function (resultData) {
                        if (resultData.Status == "OK") {
                            DialogFrame("store/AddManager.aspx?SupplierId=" + supplierId, "新建管理员", 800, 600, function () { location.href = location.href; });
                        }
                        else {
                            alert("没有添加管理员的权限");
                            return false;
                        }
                    }
                })
            }
        </script>
</asp:Content>


