﻿
<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SearchAppProduct.aspx.cs" Inherits="EcShop.UI.Web.Admin.SearchAppProduct" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<!--选项卡-->
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
			  <div class="title">
  <em><img src="../images/01.gif" width="32" height="32" /></em>
  <h1>首页商品添加</h1>
  <span>首页商品查询，方便添加首页商品</span>
        </div>
		<!--搜索-->

			<!--结束-->
		
		
		<!--数据列表区域-->
	  <div class="datalist datalist-img">
	  		<div class="searcharea clearfix">
			    <ul>
				    <li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server"/></span></li>
				    <li><Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" /></li>
                    <li><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--"/></li>
				    <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
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
                            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                        </div>
                    </div>

                    <div class="blank8 clearfix"></div>
			        <div class="batchHandleArea">
				        <ul>
					        <li class="batchHandleButton">
					            <span class="signicon"></span>
					            <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					            <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                                <span class="submit_btnxiajia"><Hi:ImageLinkButton ID="btnAdd" runat="server" Text="添加" /></span>
                                           
                            </li>
				        </ul>
			        </div>
			  </div>

	   <UI:Grid ID="grdproducts" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="ProductId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField ItemStyle-Width="10%" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                        <itemtemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                        </itemtemplate>
                  </asp:TemplateField>   
                  <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="float:left; margin-right:10px;">
                                <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span></div>
                        </ItemTemplate>
                  </asp:TemplateField>
                    <Hi:MoneyColumnForAdmin HeaderText=" 价格" ItemStyle-Width="20%"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left" />
                  <asp:TemplateField HeaderText="库存" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>&nbsp;
                           <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>
   
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="bottomPageNumber clearfix">
	  <div class="pageNumber">
					<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
            </div>
			</div>
		</div>
</div>


	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
   
</asp:Content>