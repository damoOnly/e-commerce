<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.VendorSalesReport" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core"%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
      <em><img src="../images/04.gif" width="32" height="32" /></em>
      <h1>供应商销售报表</h1>
      <span>统计供应商销售付款信息</span>
  </div>

	    <!--数据列表区域-->
	  <div class="datalist">
      		<!--搜索-->
		<!--结束-->
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
			<li>
			    <span>供应商名称：</span><asp:TextBox ID="txtSupplierName" runat="server" class="forminput"></asp:TextBox>
			</li>
             <li><span>付款时间从：</span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server"  class="forminput" /></li>
            <li><span>至：</span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate"   class="forminput"/></li>
		    <li>
				<asp:Button ID="btnQuerySupplierDetails" runat="server" Text="查询" class="searchbutton"/>
            </li>
            <li>
                <p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p>
			</li>
			</ul>
	  </div>
      <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea" style="float:left;">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
		
			</div>
			
			<div class="pageNumber" style="float:right;">
					<div class="pagination">
              <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
			</div>
			<!--结束-->
	  </div>
	     <UI:Grid ID="grdSupplierDetails" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="供应商名称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("SupplierName")%>
                            </ItemTemplate>
                        </asp:TemplateField>		                		    			      
	                    <asp:TemplateField HeaderText="销售总金额" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("OrderTotal")%>
                            </ItemTemplate>
                        </asp:TemplateField>
	                    <asp:TemplateField HeaderText = "总订单数" HeaderStyle-CssClass="td_right td_left">
	                        <ItemTemplate>			               
	                            <%#Eval("OrderCount") %>         
	                        </ItemTemplate>
	                    </asp:TemplateField>
                         <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                                <ItemTemplate>		                            
			                       <span class="Name  submit_chakan"><asp:HyperLink runat="server" ID="lkBalanceDetails" Text="明细" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/member/VendorSalesDetails.aspx?SupplierId={0}", Eval("SupplierId")))%>' /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                    </Columns>
                </UI:Grid>             
	    
	    <div class="blank12 clearfix"></div>
     
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
      </div>

</div>

</asp:Content>

