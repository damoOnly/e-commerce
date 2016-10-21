<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MembersSelect.aspx.cs" Inherits="EcShop.UI.Web.Admin.MembersSelect" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>会员选择</h1>
  <span>选择会员收货信息</span>
</div>
		<!--搜索-->
	
		<!--数据列表区域-->
		<div class="datalist">
			<div class="searcharea clearfix br_search">
			<ul>
		    <li>
                <span>会员名：</span>
                <span><asp:TextBox ID="txtSearchText" CssClass="forminput" runat="server" /></span>
           </li>
          	<li>
                <span>会员真实姓名：</span>
                <span><asp:TextBox ID="txtRealName" CssClass="forminput" runat="server" /></span>
           </li>
            <li>
                <span>手机号码：</span>
                <span><asp:TextBox ID="txtCellPhone" CssClass="forminput" runat="server" /></span>
           </li>
			<li>
				<asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
			</li>
			</ul>
	  </div>
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	          
	      </div>
		 <div class="pageNumber" style="float:right;"> 
		     <div class="pagination"><UI:Pager runat="server" ShowTotalPages="false" ID="pager" /></div>
        </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
                  <span class="submit_btnxiajia">
                      <a id="btnAdd" href="javascript:void(0)">添加</a>
                 </span>
              </li>
	        </ul>
	      </div>
	   </div>
		    <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ShippingId") %>' class="cb_ShippingId" />
                            <div id="shippingIdStr" style="display:none;">
                                 {"RegionId":<%#Eval("RegionId") %>,"UserId":<%#Eval("UserId") %>,"UserName":"<%#Eval("UserName") %>","ShippingId":<%#Eval("ShippingId") %>,"ShipTo":"<%#Eval("ShipTo") %>","Address":"<%#Eval("Address") %>","Zipcode":"<%#Eval("Zipcode") %>","CellPhone":"<%#Eval("CellPhone") %>","IdentityCard":"<%#Eval("IdentityCard") %>"}
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderText="用户名" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                              <span class="Name"><asp:Literal ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' /></span>
                             </itemtemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="真实姓名" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                             <span class="Name"><asp:Literal ID="lblRealname" runat="server" Text='<%# Eval("Realname") %>' /></span>
                             </itemtemplate>
                       </asp:TemplateField>
                        <asp:TemplateField HeaderText="邮箱"  HeaderStyle-CssClass="td_right td_left" ShowHeader="true">
                        <ItemTemplate><div></div><%# Eval("Email")%>&nbsp;</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="收货人姓名" SortExpression="ShipTo" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                             <span class="Name"><asp:Literal ID="lblShipTo" runat="server" Text='<%# Eval("ShipTo") %>' /></span>
                             </itemtemplate>
                       </asp:TemplateField>
                        <asp:TemplateField HeaderText="详细地址"  HeaderStyle-CssClass="td_right td_left" ShowHeader="true">
                            <ItemTemplate><div style="word-wrap: break-word;width:400px;"><%# Eval("Address")%></div>&nbsp;</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="邮政编码"  HeaderStyle-CssClass="td_right td_left" ShowHeader="true">
                          <ItemTemplate><div></div><%# Eval("Zipcode")%>&nbsp;</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="手机号"  HeaderStyle-CssClass="td_right td_left" ShowHeader="true">
                          <ItemTemplate><div></div><%# Eval("CellPhone")%>&nbsp;</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="身份证号码"  HeaderStyle-CssClass="td_right td_left" ShowHeader="true">
                          <ItemTemplate><div></div><%# Eval("IdentityCard")%>&nbsp;</ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>               
            <div class="blank12 clearfix"></div>
            </div>
		 <!--数据列表底部功能区域-->
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination" style="width:auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">	
<script type="text/javascript" language="javascript" src="/Utility/membersSelect.js"></script>
</asp:Content>  
