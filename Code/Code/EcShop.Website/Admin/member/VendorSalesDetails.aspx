<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="VendorSalesDetails.aspx.cs" Inherits="EcShop.UI.Web.Admin.VendorSalesDetails" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1 class="title_height"><strong class="colorE"></strong>供应商订单交易明细</h1>
  </div>		
  <div class="searcharea clearfix">
	  <ul>
         <li>
	        <span>订单编码：</span><asp:TextBox ID="txtOrderId" runat="server" class="forminput"></asp:TextBox>
		</li>
        <li>
	        <span>商品名称：</span><asp:TextBox ID="txtProductName" runat="server" class="forminput"></asp:TextBox>
		</li>
           <li>
	        <span>商品编码：</span><asp:TextBox ID="txtProductCode" runat="server" class="forminput"></asp:TextBox>
		</li>
	    <li>
                <span>付款时间段：</span>
                <span><UI:WebCalendar CalendarType="StartDate" CssClass="forminput" ID="calendarStart" runat="server" /></span>
                <span class="Pg_1010">至</span>
                <span><UI:WebCalendar CalendarType="EndDate" CssClass="forminput" ID="calendarEnd" runat="server" /></span>
        </li>
      
        <li>
				<asp:Button ID="btnQuerySupplierDetails" runat="server" Text="查询" class="searchbutton"/>
        </li>
	 </ul>
	</div>		
<!--结束-->		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
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
</div>
		<!--数据列表区域-->
		<div class="datalist">
		    <UI:Grid ID="grdBalanceDetails" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                            <Columns>
                                <asp:BoundField HeaderText="流水号" DataField="JournalNumber" HeaderStyle-CssClass="td_right td_left" />	
                           		<asp:TemplateField HeaderText="订单编号" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                      <%#Eval("OrderId") %> 
                                    </ItemTemplate>
                               </asp:TemplateField>	  		                		    			      
			                    <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                       <%#Eval("ProductName") %> 
                                    </ItemTemplate>
                                </asp:TemplateField>
			                    <asp:TemplateField HeaderText = "商品编码" HeaderStyle-CssClass="td_right td_left" >
			                        <ItemTemplate>			               
			                             <%#Eval("ProductCode") %> 
			                        </ItemTemplate>
			                    </asp:TemplateField>
                                 <asp:TemplateField HeaderText = "调整金额" HeaderStyle-CssClass="td_right td_left" >
			                        <ItemTemplate>			               
			                             <%#Eval("ItemAdjustedPrice") %> 
			                        </ItemTemplate>
			                    </asp:TemplateField>
                                <asp:TemplateField HeaderText = "供应商名称" HeaderStyle-CssClass="td_right td_left" >
			                        <ItemTemplate>			               
			                             <%#Eval("SupplierName") %> 
			                        </ItemTemplate>
			                    </asp:TemplateField>
                                <asp:TemplateField HeaderText = "支付时间" HeaderStyle-CssClass="td_right td_left" >
			                        <ItemTemplate>			               
			                             <%#Eval("PayDate") %> 
			                        </ItemTemplate>
			                    </asp:TemplateField> 
	                        </Columns> 
	                      </UI:Grid>		
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
		</div>
	</div>
     
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>