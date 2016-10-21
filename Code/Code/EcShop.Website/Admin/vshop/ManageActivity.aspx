<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageActivity.aspx.cs"
    Inherits="EcShop.UI.Web.Admin.ManageActivity" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/06.gif" width="32" height="32" /></em>
            <h1>
                微报名列表
            </h1>
            <span>您可以在此管理好您的微报名，并在自定义回复中使用它们。</span></div>
        <!-- 添加按钮-->
        <div class="btn">
            <a href="AddActivity.aspx" class="submit_jia">添加微报名</a>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <UI:Grid ID="grdActivity" runat="server" AutoGenerateColumns="False" Width="100%"
                DataKeyNames="ActivityId" HeaderStyle-CssClass="table_title" GridLines="None" SortOrderBy="ActivityId"
                SortOrder="DESC">
                <Columns>
                    <asp:TemplateField HeaderText="活动名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <Hi:SubStringLabel ID="lblCouponName" StrLength="60" StrReplace="..." Field="Name" runat="server"></Hi:SubStringLabel><br />
                            <%#GetUrl(Eval("activityId"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Keys" HeaderText="关键字" HeaderStyle-CssClass="td_right td_left">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="报名人数" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <a href="javascript:void(0);" onclick="javascript:DialogFrame('vshop/ActivityDetail.aspx?id=<%# Eval("ActivityId") %>', '报名详细', 800, null);"><%#Eval("CurrentValue")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="活动时间" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="width: 200px;">
                                <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("StartDate")%>' FormatDateTime="yyyy-MM-dd" runat="server"></Hi:FormatedTimeLabel>
                                至 <Hi:FormatedTimeLabel ID="lblClosingTimes" Time='<%#Eval("EndDate")%>' FormatDateTime="yyyy-MM-dd" runat="server"></Hi:FormatedTimeLabel>    
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="250">
                        <ItemTemplate>
                            <span class="submit_bianji">
                                <a href='/admin/vshop/EditActivity.aspx?id=<%# Eval("ActivityId")%>'>编辑</a>
                                <a href="javascript:void(0);" onclick="javascript:DialogFrame('vshop/ActivityDetail.aspx?id=<%# Eval("ActivityId") %>', '报名详细', 800, null);">查看报名人数</a>
                            </span><span class="submit_shanchu">
                                <Hi:ImageLinkButton ID="lkbDelete" runat="server" CommandName="Delete" Text="删除"
                                    OnClientClick="javascript:return confirm('确定要执行删除操作吗？删除后将不可以恢复')"></Hi:ImageLinkButton>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
        </div>
        <div class="blank5 clearfix">
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
        <!--数据列表底部功能区域-->
    </div>
</asp:Content>
