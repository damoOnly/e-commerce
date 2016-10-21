<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SplittinDrawRecord.aspx.cs" Inherits="EcShop.UI.Web.Admin.SplittinDrawRecord" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/05.gif" width="32" height="32" /></em>
  <h1>历史结算</h1>
  <span>查看推广员的历史结算记录</span>
</div>

		<!--数据列表区域-->
		<div class="datalist">
        		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li>
                <span>选择时间段：</span>
                <span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" class="forminput"/></span>
                <span class="Pg_1010">至</span>
                <span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate" class="forminput"/></span>
                <span style=" margin-left:11px;">推广员：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"/></span>
              </li>
				<li>
				    <asp:Button ID="btnQuery" runat="server" class="searchbutton" Text="查询" />
				</li>
			</ul>
	</div>
		
<!--结束-->
		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		  <div class="pageNumber"> 
		    <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
          </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">&nbsp;</li>
	        </ul>
	      </div>
</div>
		    <UI:Grid ID="grdBalanceDrawRequest" DataKeyNames="JournalNumber" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                <Columns>
                    <asp:TemplateField HeaderText="推广员" HeaderStyle-CssClass="td_right td_left">
                        <itemtemplate>			  
                            <asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label>         
                        </itemtemplate>
                        </asp:TemplateField>
                        
                    
                    <Hi:MoneyColumnForAdmin HeaderText="申请提现金额" DataField="Amount" HeaderStyle-CssClass="td_right td_left" />
                    <asp:TemplateField HeaderText="申请日期" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                            <Hi:FormatedTimeLabel ID="lblTime" Time='<%# Eval("RequestDate")%>' runat="server" />
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="结算日期" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                            <Hi:FormatedTimeLabel ID="lblTime" Time='<%# Eval("AccountDate")%>' runat="server" />
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                            <%# Eval("ManagerRemark")%>
                        </itemtemplate>
                    </asp:TemplateField>
                    </Columns> 
              </UI:Grid>
		  
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>