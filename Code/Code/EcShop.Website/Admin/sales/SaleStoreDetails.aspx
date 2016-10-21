<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SaleStoreDetails.aspx.cs" Inherits="EcShop.UI.Web.Admin.SaleStoreDetails" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
			  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>销量及库存量报表</h1>
  <span>查询一段时间内每个订单内的商品销售量及销售价，默认排序为售出时间由新到旧(注：统计的商品不包括成功退款订单中的商品)。</span>
</div>

	    <!--数据列表区域-->
	  <div class="datalist">
      		<!--搜索-->
		<!--结束-->
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
		 
          <li><span>成交时间段：</span><span><UI:WebCalendar ID="calendarStart" runat="server" class="forminput"/></span><span class="Pg_1010">至</span><span><UI:WebCalendar ID="calendarEnd" runat="server" class="forminput" IsStartTime="false" /></span></li>
                <li><span>商品名称：</span><span>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" />
                </span></li>
                <li><span>供货商：</span><span>
                    <abbr class="formselect">
                         <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" NullToDisplay="请选择" Width="107"></Hi:SupplierDropDownList>
                    </abbr>
                </span></li>
				<li><asp:Button ID="btnQuery" runat="server" Text="查询" class="searchbutton" /></li>
                <li>
                <p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p>
			</li>
			</ul>
	  </div>
      <div class="blank12 clearfix"></div>
	    <asp:GridView ID="grdOrderLineItem" runat="server"  AutoGenerateColumns="false" ShowHeader="true" AllowSorting="true"  GridLines="None" HeaderStyle-CssClass="table_title" >                                                        
                <Columns>  
                 <asp:BoundField HeaderText="商品代码" DataField="SkuId"  HeaderStyle-CssClass="td_right td_left" />                                                                                                                                                                                                                                                                                                                                                                
                    <asp:BoundField HeaderText="商品条码" DataField="BarCode" HeaderStyle-CssClass="td_right td_left"/>    
                   <asp:BoundField HeaderText="商品中文名" DataField="ProductName" ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left"/>    
                    <asp:BoundField HeaderText="商品英文名" DataField="EnglishName" ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left"/>    
                    <asp:BoundField HeaderText="商品规格" DataField="ItemNo" HeaderStyle-CssClass="td_right td_left" />
                    <asp:BoundField HeaderText="产地" DataField="CnArea" HeaderStyle-CssClass="td_right td_left" />
                    <asp:BoundField HeaderText="品牌" DataField="BrandName" HeaderStyle-CssClass="td_right td_left" />
                    <Hi:MoneyColumnForAdmin HeaderText="销售单价" DataField="SalePrice"  HeaderStyle-CssClass="td_right td_left"/>
                    <Hi:MoneyColumnForAdmin HeaderText="销售总额" DataField="Sumsales"  HeaderStyle-CssClass="td_right td_left"/>
                    <asp:BoundField HeaderText="销售数量" DataField="SalesNum" HeaderStyle-CssClass="td_right td_left" />
                    <asp:BoundField HeaderText="库存" DataField="Stock" HeaderStyle-CssClass="td_right td_left" />
                    <Hi:MoneyColumnForAdmin HeaderText="库存总额" DataField="InventoryCost"  HeaderStyle-CssClass="td_right td_left"/>
                    <asp:BoundField HeaderText="供货商" DataField="SupplierName" HeaderStyle-CssClass="td_right td_left" />
                    
                </Columns>
        </asp:GridView>
     
      <div class="blank12 clearfix"></div>
     
	  </div>
	  <!--数据列表底部功能区域-->
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
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
