<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UsedVouchers.aspx.cs" Inherits="EcShop.UI.Web.Admin.UsedVouchers" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/06.gif" width="32" height="32" /></em>
      <h1>现金券号码 </h1>
     <span>你可以在此查看系统所有的现金券号码及使用情况</span></div>
     
        <div class="searcharea clearfix br_search">
			<ul class="a_none_left">           
				<li><span>现金券名称：</span><span><asp:TextBox ID="txtVoucherName" runat="server" CssClass="forminput" /></span></li>
					<li><span>订单号：</span><span><asp:TextBox ID="txtOrderID" runat="server" CssClass="forminput" /></span></li>		
						<li><span>使用状态：</span><span class="formselect"><asp:DropDownList ID="Dpstatus" runat="server" style=" width:150px;">
                            <asp:ListItem Value="">=请选择=</asp:ListItem>
                            <asp:ListItem Value="1">已使用</asp:ListItem>
                            <asp:ListItem Value="0">未使用</asp:ListItem>
                        </asp:DropDownList>
                        </span></li>		
				<li><asp:Button ID="btnSearch" runat="server" CssClass="searchbutton"　Text="查询" 
                        onclick="btnSearch_Click" /></li>
		  </ul>
	  </div>
    
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">
    
   <UI:Grid ID="grdVouchers" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="VoucherId" HeaderStyle-CssClass="table_title" GridLines="None"  SortOrderBy="VoucherId" SortOrder="DESC">
                        <Columns>  
                               <asp:TemplateField HeaderText="现金券名称"  HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                     <Hi:SubStringLabel ID="lblVoucherName" StrLength="60" StrReplace="..."  Field="Name" runat="server" ></Hi:SubStringLabel>
                                  </ItemTemplate>
                               </asp:TemplateField>
                                     <asp:BoundField DataField="ClaimCode" HeaderText="现金券号码" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>    
                                      <asp:BoundField DataField="UserName" HeaderText="用户名" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>
                               <asp:TemplateField HeaderText="使用时间" HeaderStyle-CssClass="td_right td_left">
                                    <itemtemplate>
                                   &nbsp; <Hi:FormatedTimeLabel ID="lblUsedTimes" Time='<%#Eval("UsedTime")%>' runat="server" ></Hi:FormatedTimeLabel>
                                    </itemtemplate>
                                </asp:TemplateField>
                                
                                    <asp:TemplateField HeaderText="过期时间" HeaderStyle-CssClass="td_right td_left" >
                                    <itemtemplate>
                                  <%#IsVoucherEnd(Eval("ClosingTime")) %>
                                    </itemtemplate>
                                </asp:TemplateField>
                       
                         
                           
                                <asp:TemplateField HeaderText="订单号" HeaderStyle-CssClass="td_right td_left">
                            
                                    <ItemTemplate>
                                      <span class="submit_tongyii"> 
                                       <a href="javascript:DialogFrame('<%# Globals.GetAdminAbsolutePath("sales/OrderDetails.aspx?OrderId="+Eval("orderid")) %>','订单详细信息',null,null)"><%#Eval("orderid") %></a></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
                     </UI:Grid>
    
      <div class="blank5 clearfix"></div>
       <div class="page">
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
			</div>
			</div>
		</div>
		</div>
    <!--数据列表底部功能区域-->
  </div>
  </div>
</asp:Content>
