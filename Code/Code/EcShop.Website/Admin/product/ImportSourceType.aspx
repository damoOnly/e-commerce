<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ImportSourceType.aspx.cs" Inherits="EcShop.UI.Web.Admin.ImportSourceType" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>原产地管理</h1>
            <span></span>
        </div>
         <!-- 添加按钮-->
	    <div class="btn">
	        <a href="AddImportSourceType.aspx" runat="server"  class="submit_jia ibtn">添加原产品地</a>
        </div>   
        <div class="datalist">
            <%--<div style="margin-bottom:10px;"><a href="AddImportSourceType.aspx" runat="server"  class="submit_jia ibtn">添加原产品地</a></div>--%>
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>短名称：</span><span>
                        <asp:TextBox ID="txtCnArea" Width="74" runat="server" CssClass="forminput" /></span></li> 
                    <li><span>长名称：</span><span>
                        <asp:TextBox ID="txtEnArea" Width="74" runat="server" CssClass="forminput" /></span></li>
                    <li><span>描述：</span><span>
                        <asp:TextBox ID="txtRemark" Width="74" runat="server" CssClass="forminput" /></span></li>
                    <li><span>添加时间：</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                    <li></li>
                    <li>
                       <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix">
                <!--分页功能-->
                <div class="pageHandleArea">
                    <ul>
                       <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
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
                                <a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span> <span class="delete">
                                    <Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把原产地删除吗？" /></span>
                            
                        </li>
                    </ul>
                </div>
            </div>
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdImportSourceTypes" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ImportSourceId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ImportSourceId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="35px"
                        HeaderStyle-CssClass="td_right td_left" />
                    <asp:TemplateField ItemStyle-Width="42%" HeaderText="描述" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                                <span style=" float:left;"> <Hi:ListImage ID="ListImage1" runat="server" DataField="Icon" width="50px" height="50px"/></span>
                                <span class="Name" style="line-height:50px;"><%# Eval("Remark") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="长名称" ItemStyle-Width="250" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("EnArea") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="80" HeaderText="短名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("CnArea") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    

                    <asp:TemplateField HeaderText="海关代码" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("HSCode") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="是否最惠国" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("FavourableFlag").ToString().ToLower()=="true"?"是":"否" %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="10%" HeaderText="添加时间" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <span class="Name"><%# Eval("AddTime") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" ItemStyle-Width="180px" HeaderStyle-CssClass=" td_left td_right_fff">
                        <ItemTemplate> 
                          <span class="submit_bianji"><a href='<%# "EditImportSourceType.aspx?importSourceId="+Eval("ImportSourceId")%>')">编辑
                                    </a></span> 
                                        <span class="submit_shanchu">
                                        <Hi:ImageLinkButton ID="btnDel" CommandName="Delete" runat="server" Text="删除" IsShow="true"
                                            DeleteMsg="确定要把原产地删除吗?" />
                                        </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <div class="blank12 clearfix">
            </div>
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
    </div>
    <div>
        
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
       

    </script>
</asp:Content>
