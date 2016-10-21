<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.ProductSaleAsImportSource" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
				  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>按产地汇总商品销售情况</h1>
  <%--<span>查询一段时间内商品销售量和销售额，默认排序为销售量从高到低(注：统计的商品不包括成功退款订单中的商品)。</span>--%>
</div>
      <div>
        <!--数据列表区域-->
	  <div class="datalist">
      		<!--搜索-->
   <div class="searcharea clearfix ">
          <ul class="a_none_left">
           <li><span>起始日期：</span><span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" class="forminput"  /></span></li>
           <li><span>终止日期：</span><span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" class="forminput" /></span></li>
           <li>
                <abbr class="formselect">
                    <Hi:ImportSourceTypeDropDownList runat="server"  ID="ddlImportSourceType"  NullToDisplay="--请选择原产地--" Width="160"/>
                </abbr>
             </li>
            <li> <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/> </li>
             <li><p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p></li>
          </ul>
      </div>
		<!--结束-->
        <div class="blank12 clearfix"></div>
	  <UI:Grid ID="grdProductSaleAsImportSource" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                             <asp:BoundField HeaderText="原产地" DataField="CnArea" HeaderStyle-CssClass="td_right td_left"/>                                                                                                                                                                                                                                                                                                                                                                                                      
                            <asp:BoundField HeaderText="商品名称" DataField="ProductName" HeaderStyle-CssClass="td_right td_left"/>   
                             <asp:TemplateField HeaderText="商家编码" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuyCount"  runat="Server" Text='<%# Eval("SKU") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>   
                              <asp:TemplateField HeaderText="销售量" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuyCount"  runat="Server" Text='<%# Eval("ProductSaleCounts") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>  
                               <asp:TemplateField HeaderText="销售额" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("ProductSaleTotals") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </ItemTemplate>
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="利润" HeaderStyle-CssClass="td_right td_right_fff">
                                <ItemTemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%# Eval("ProductProfitsTotals") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </ItemTemplate>
                            </asp:TemplateField>   
                    </Columns>
                </UI:Grid>
                
      <div class="blank12 clearfix"></div>
    <div class=" VIPbg m_none colorG">此处的利润是指本站会员订单产生的利润。</div>
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
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

                  
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

