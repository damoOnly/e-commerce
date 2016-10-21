<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Referrals.aspx.cs" Inherits="EcShop.UI.Web.Admin.Referrals" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/05.gif" width="32" height="32" /></em>
  <h1>推广员列表</h1>
  <span>查看商城的推广员</span>
</div>

		<!--数据列表区域-->
		<div class="datalist">
        		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li>                
                <span>推广员：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"/></span>
                </li>
                <li>
                 <span >联系人姓名：</span><span><asp:TextBox ID="txtRealName" runat="server" CssClass="forminput"/></span>
                 </li>
                <li>
                 <span >联系人电话：</span><span><asp:TextBox ID="txtCellphnoe" runat="server" CssClass="forminput"/></span>
              </li>
               <li>
                 <span >我的上级：</span><span><asp:TextBox ID="txtReferralUsername" runat="server" CssClass="forminput"/></span>
              </li>
				<li>
				    <asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="查询" />
				</li>
			</ul>
	</div>
		
<!--结束-->
		
          <div class="functionHandleArea m_none">
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
		      <li class="batchHandleButton">&nbsp;</li>
	        </ul>
	      </div>
</div>
		    <UI:Grid ID="grdReferralRequest" DataKeyNames="UserId" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                <Columns>
                    <asp:TemplateField HeaderText="推广员" HeaderStyle-CssClass="td_right td_left">
                        <itemtemplate>			  
                            <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/EditMember.aspx?userId={0}", Eval("UserId")))%>'><asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label></a>                     
                        </itemtemplate>
                        </asp:TemplateField>
                        
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("RealName")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="手机号码" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("CellPhone")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="审核通过日期" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                            <Hi:FormatedTimeLabel ID="lblTime" Time='<%# Eval("ReferralAuditDate")%>' runat="server" />&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="直接推广订单量" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("ReferralOrderNumber")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="直接推广佣金" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("ReferralSplittin", "{0:f2}")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="所属上级" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("ReferralUsername")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="已发展下级数" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("SubNumber")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                           <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/SplittinDetails.aspx?userId={0}", Eval("UserId")))%>'  >佣金明细</a>
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
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>



    <div id="divReferralDetails" style="display: none;">
        <div class="frame-content" style="margin-top:-20px;">
            <p> 推广员详情</p>
             <table cellpadding="0" cellspacing="0" width="500px" border="0" class="fram-retreat">
                  <tr>
                    <td align="right" width="30%">姓名:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblRealname" runat="server"></asp:Label></td>
                  </tr>             
                  <tr>
                    <td align="right">联系电话:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblCellphone" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">微信账号:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblWeChat" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">联系地址:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">申请日期:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblRequetsDate" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">推广订单量:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblReferralOrderNumber" runat="server"></asp:Label></td>
                  </tr>
                  <tr>
                    <td align="right">推广订单金额:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblReferralExpenditure" runat="server"></asp:Label></td>
                  </tr>
                </table>
                <p></p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script language="javascript" type="text/javascript">
    function ReferralDetails(realName, cellPhone, weChat, address, referralDate, referralOrderNumber, referralExpenditure) {
        $("#ctl00_contentHolder_lblRealname").html(realName);
        $("#ctl00_contentHolder_lblCellphone").html(cellPhone);
        $("#ctl00_contentHolder_lblWeChat").html(weChat);
        $("#ctl00_contentHolder_lblAddress").html(address);
        $("#ctl00_contentHolder_lblRequetsDate").html(referralDate);
        $("#ctl00_contentHolder_lblReferralOrderNumber").html(referralOrderNumber);
        $("#ctl00_contentHolder_lblReferralExpenditure").html(referralExpenditure);

        ShowMessageDialog("详情", "divReferralDetails", "divReferralDetails");
    }
</script>
</asp:Content>