﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CountDownsManager.aspx.cs" Inherits="EcShop.UI.Web.Admin.CountDownsManager" %>
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
  <h1> 限时抢购 </h1>
  <span>限时抢购管理</span>
</div>
		<!--结束-->
	    <div class="btn"  style="float:left;">
          <a href="AddCountDownManager.aspx" class="submit_jia">添加限时管理</a>
	    </div>

		<!--数据列表区域-->
	    <div class="datalist" style="clear:both">
              
	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
                
				<li><span>活动名称：</span><span><asp:TextBox ID="txtTitle" runat="server" CssClass="forminput" /></span></li>
				
				<li><asp:Button ID="btnSearch" runat="server" CssClass="searchbutton"　Text="查询" /></li>
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
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" IsShow="true" runat="server" >删除</Hi:ImageLinkButton></span>
                    </li>
				</ul>
            <span style="width:110px;height:25px; background:url(../images/icon.gif) no-repeat -100px -87px;float:right;">　　　
            <asp:LinkButton ID="btnOrder" runat="server" Text="保存排序" /></span>
			</div>
	  </div>
		
		    <UI:Grid ID="grdCountDownsList" runat="server" ShowHeader="true" AutoGenerateColumns="false" SortOrderBy="DisplaySequence"  DataKeyNames="CountDownCategoryId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                 <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left" HeadWidth="35"/>
                 <asp:TemplateField HeaderText="活动标题" HeaderStyle-CssClass="td_right td_left"  ItemStyle-Width="30%">
                       <ItemTemplate>
                        
                            <Hi:SubStringLabel id="lblHelpCategory" Field="Title" StrLength="60" StrReplace="..." runat="server"></Hi:SubStringLabel>
                                 
                           <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                        </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="开启时间"  ItemStyle-Width="15%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <div><%#Eval("StartTime") %></div>

                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="终止时间"  ItemStyle-Width="15%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                            <div><%#Eval("EndTime") %></div>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="活动图片"  ItemStyle-Width="10%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <div>
                               <img src="<%# Eval("AdImageUrl") %>"  title="活动图片"  style="height:50px; width:200px;"/>

                           </div>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="排序" HeaderStyle-CssClass="td_right td_left">
                     <ItemTemplate>
                        <asp:TextBox ID="txtSequence" runat="server" Text='<%# Eval("DisplaySequence") %>' Width="50px"></asp:TextBox>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作" ItemStyle-Width="15%" HeaderStyle-CssClass="td_left td_right_fff" >
                     <ItemTemplate>
                         <span class="submit_bianji"><a href='<%# "EditCountDownManager.aspx?CountDownCategoryId=" + Eval("CountDownCategoryId")%> '>编辑</a></span>
                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" IsShow="true" Text="删除"  CommandName="Delete" runat="server" /></span>
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
