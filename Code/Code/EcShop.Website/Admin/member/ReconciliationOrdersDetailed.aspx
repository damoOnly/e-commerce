<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.ReconciliationOrdersDetailed"  CodeBehind="ReconciliationOrdersDetailed.aspx.cs"  MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
      <em><img src="../images/04.gif" width="32" height="32" /></em>
      <h1>对账订单明细报表</h1>
      <span>统计对账订单明细信息</span>
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
				<asp:Button ID="btnQueryBalanceDetails" runat="server" Text="查询" class="searchbutton"/>
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
	        <UI:Grid ID="grdBalanceDetails" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="3200px">
                    <Columns>
                        <asp:TemplateField HeaderText="订单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("OrderId")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="子订单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("ChildOrderId")%>
                            </ItemTemplate>
                        </asp:TemplateField>			                		    			      
	                    <asp:TemplateField HeaderText="付款日期" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("TradingTime") %>
                            </ItemTemplate>
                        </asp:TemplateField>
	                 
	                    <Hi:MoneyColumnForAdmin HeaderText="实际金额" DataField="ActualAmount" HeaderStyle-CssClass="td_right td_left"/>
	                    <Hi:MoneyColumnForAdmin HeaderText="收款金额" DataField="TotalAmount" HeaderStyle-CssClass="td_right td_left"/>
	                    <Hi:MoneyColumnForAdmin HeaderText="退款金额" DataField="RefundAmount" HeaderStyle-CssClass="td_right td_left"/>
                       
                        <asp:TemplateField HeaderText="买家昵称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%#Eval("RealName")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="拍单时间" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("OrderDate") %>
                            </ItemTemplate>
                        </asp:TemplateField>
	                    <Hi:MoneyColumnForAdmin HeaderText="商品总金额" DataField="Amount" HeaderStyle-CssClass="td_right td_left"/>
	                    <Hi:MoneyColumnForAdmin HeaderText="订单金额" DataField="OrderTotal" HeaderStyle-CssClass="td_right td_left"/>
	                    <Hi:MoneyColumnForAdmin HeaderText="优惠金额" DataField="DiscountAmount" HeaderStyle-CssClass="td_right td_left"/>
                        <Hi:MoneyColumnForAdmin HeaderText="税费" DataField="Tax" HeaderStyle-CssClass="td_right td_left"/>
                        <Hi:MoneyColumnForAdmin HeaderText="运费" DataField="AdjustedFreight" HeaderStyle-CssClass="td_right td_left"/>
                        <Hi:MoneyColumnForAdmin HeaderText="优惠券" DataField="CouponValue" HeaderStyle-CssClass="td_right td_left"/>
                        <Hi:MoneyColumnForAdmin HeaderText="现金劵" DataField="VoucherValue" HeaderStyle-CssClass="td_right td_left"/>
                        <Hi:MoneyColumnForAdmin HeaderText="货到付款服务费" DataField="PayCharge" HeaderStyle-CssClass="td_right td_left"/>
                        <Hi:MoneyColumnForAdmin HeaderText="扣点" DataField="DeductFee" HeaderStyle-CssClass="td_right td_left"/>
                        <Hi:MoneyColumnForAdmin HeaderText="成本价" DataField="CostPrice" HeaderStyle-CssClass="td_right td_left"/>

                        <asp:TemplateField HeaderText="订单备注" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ManagerRemark") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                          <asp:TemplateField HeaderText="买家留言" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Remark") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                          <asp:TemplateField HeaderText="收货地址" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Address") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                          <asp:TemplateField HeaderText="收货人名称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ShipTo") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                          <asp:TemplateField HeaderText="州/省" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Province") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="城市" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("City") %>
                            </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="区" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Area") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="邮编" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ZipCode") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="联系电话" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("TelPhone") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                          <asp:TemplateField HeaderText="手机" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("CellPhone") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="买家选择物流" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ModeName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                          <asp:TemplateField HeaderText="订单状态" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("OrderStatus") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="发货快递单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ShipOrderNumber") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="退货快递单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ReturnsExpressNumber") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ItemDescription") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="商品条码" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("SKU") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="购买数量" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Quantity") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="退货数量" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ReturnQuantity") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="售价" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ItemListPrice") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="销售价格" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ItemAdjustedPrice") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="发货仓库" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("ShipWarehouseName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="货主名称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("SupplierName") %>
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

