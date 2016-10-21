<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdOrderList.aspx.cs" Inherits="EcShop.UI.Web.Admin.AdOrderList" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">


	<div class="dataarea mainwidth databody">
		<!--搜索-->
				  <div class="title">
  <em><img src="../images/06.gif" width="32" height="32" /></em>
  <h1>广告订单</h1>
  <span>广告订单列表</span>
</div>
	  <div class="datalist" style="clear:both">
              
	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
				<li><span>订单号：</span><span><asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /></span></li>
                <li><span>关键字：</span><span><asp:TextBox ID="txtKeyWord" runat="server" CssClass="forminput" /></span></li>
                <li><span>创建时间：</span></li>
                <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                </li>
				<li><asp:Button ID="btnSearch" runat="server" CssClass="searchbutton"　Text="查询" /></li>
                <li>
                    <p>
                        <asp:LinkButton ID="btnCreateReport" runat="server" Text="导出Excel" />
                    </p>
                </li>
		  </ul>
	  </div>

      <div class="functionHandleArea clearfix m_none">
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
	  </div>
		
		    <UI:Grid ID="grdCountDownsList" runat="server" ShowHeader="true" AutoGenerateColumns="false" SortOrderBy="DisplaySequence"  DataKeyNames="" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                 <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left" HeadWidth="15"/>
                 <asp:TemplateField HeaderText="订单号" HeaderStyle-CssClass="td_right td_left"  ItemStyle-Width="10%">
                       <ItemTemplate>
                             <asp:Label ID="lblOrderNO" Text='<%#Eval("OrderNo") %>' runat="server"></asp:Label>        
                        </ItemTemplate>
                 </asp:TemplateField>
                   <asp:TemplateField HeaderText="订单总金额"  ItemStyle-Width="8%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                            <Hi:FormatedMoneyLabel id="lblOrderTotal"  runat="server" Money='<%# Eval("OrderTotal") %>'></Hi:FormatedMoneyLabel>          
                       </ItemTemplate>
                 </asp:TemplateField>

                 <asp:TemplateField HeaderText="商品名称"  ItemStyle-Width="15%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                            <asp:Label ID="lblProductName" Text='<%#Eval("ProductName") %>' runat="server"></asp:Label>        
                       </ItemTemplate>
                 </asp:TemplateField>
                      <asp:TemplateField HeaderText="商品条码"  ItemStyle-Width="8%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                            <asp:Label ID="lblBarCode" Text='<%#Eval("BarCode") %>' runat="server"></asp:Label>        
                       </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="Sku编码"  ItemStyle-Width="8%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                            <asp:Label ID="lblsku" Text='<%#Eval("sku") %>' runat="server"></asp:Label>        
                       </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="商品单价"  ItemStyle-Width="5%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate> 
                            <Hi:FormatedMoneyLabel id="lblItemAdjustedPrice"  runat="server" Money='<%# Eval("ItemAdjustedPrice") %>'></Hi:FormatedMoneyLabel>     
                       </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="订单状态"  ItemStyle-Width="8%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                            <asp:Label ID="lblOrderStatus" Text='<%#Eval("OrderStatus") %>' runat="server"></asp:Label>        
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="支付类型"  ItemStyle-Width="8%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                             <asp:Label ID="lblPaymentType" Text='<%#Eval("PaymentType") %>' runat="server"></asp:Label>       
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="付款状态"  ItemStyle-Width="8%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                             <asp:Label ID="lblpaymentStatus" Text='<%#Eval("paymentStatus") %>' runat="server"></asp:Label>     
                       </ItemTemplate>
                 </asp:TemplateField>
                <asp:TemplateField HeaderText="订单创建时间"  ItemStyle-Width="15%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel ID="lblOrderTIme" Time='<%#Eval("OrderTIme") %>' runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                <asp:TemplateField HeaderText="订单修改时间"  ItemStyle-Width="15%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel ID="lblUpdateDate" Time='<%#Eval("UpdateDate") %>' runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
            </Columns>
        </UI:Grid>
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
	  <div class="pageNumber">
			<div class=pagination>
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
			</div>
			</div>
		</div>
      </div>

</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>
