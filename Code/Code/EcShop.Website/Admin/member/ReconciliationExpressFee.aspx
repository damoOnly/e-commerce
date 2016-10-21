<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.ReconciliationExpressFee"  CodeBehind="ReconciliationExpressFee.aspx.cs"  MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
      <em><img src="../images/04.gif" width="32" height="32" /></em>
      <h1>快递费用报表</h1>
      <span>统计订单快递明细信息</span>
  </div>
	  <!--数据列表区域-->
	  <div class="datalist">
      		<!--搜索-->


      <div class="searcharea clearfix ">
			<ul class="a_none_left">
			 <li><span>供应商：</span>
                 <span>
                    <abbr class="formselect">
                        <asp:DropDownList runat="server" ID="dllSupper" Width="107" />
                    </abbr>
                </span>
            </li>

             <li><span>交易时间从：</span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server"  class="forminput" /></li>
            <li><span>至：</span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate"   class="forminput"/></li>
		    <li>
				<asp:Button ID="btnQueryExpressFeeDetails" runat="server" Text="查询" class="searchbutton"/>
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
      <div style="width:100%;overflow:auto; ">
	        <UI:Grid ID="grdExpressFeeDetails" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="供应商" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("SupplierName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="订单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("SourceOrderId")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="子订单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("OrderId")%>
                            </ItemTemplate>
                        </asp:TemplateField>			                		    			      
	                    <asp:TemplateField HeaderText="交易日期" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("TradingTime") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="发货时间" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ShippingDate") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="快递公司" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ExpressCompanyName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="快递单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ShipOrderNumber")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="费用" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("AdjustedFreight") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                      </Columns>
                </UI:Grid>             
	    
	    <div class="blank12 clearfix"></div>
        </div>
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

