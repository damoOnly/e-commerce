<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralRequest.aspx.cs" Inherits="EcShop.UI.Web.Admin.ReferralRequest" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/05.gif" width="32" height="32" /></em>
  <h1>推广员审核</h1>
  <span>查看待审核的推广员</span>
</div>

		<!--数据列表区域-->
		<div class="datalist">
        		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li>                
                <span>用户名：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"/></span>
                </li>
                <li>
                 <span >联系人姓名：</span><span><asp:TextBox ID="txtRealName" runat="server" CssClass="forminput"/></span>
                 </li>
                <li>
                 <span >联系人电话：</span><span><asp:TextBox ID="txtCellphnoe" runat="server" CssClass="forminput"/></span>
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
                    <asp:TemplateField HeaderText="用户名" HeaderStyle-CssClass="td_right td_left">
                        <itemtemplate>			  
                            <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/EditMember.aspx?userId={0}", Eval("UserId")))%>'><asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label></a>                     
                        </itemtemplate>
                        </asp:TemplateField>
                        
                    <asp:TemplateField HeaderText="联系人" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("RealName")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="联系方式" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("CellPhone")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="下单量" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                           <%# Eval("OrderNumber")%>&nbsp;
                        </itemtemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                           <a href="javascript:void(0)" onclick="return CheckReferral('<%# Eval("UserId") %>', '<%# Eval("RealName") %>', '<%# Eval("CellPhone") %>', '<%# Eval("MSN") %>','<%# Eval("Address") %>', '<%# Eval("ReferralReason") %>')">审核</a>
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


<div id="CheckReferral" style="display: none;">
        <div class="frame-content" style="margin-top:-20px;">
            <p> 推广员审核</p>
             <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
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
                    <td align="right">申请理由:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblReferralReason" runat="server"></asp:Label></td>
                  </tr>
                </table>

            <p>
                <span class="frame-span frame-input100" style="margin-right:10px;">拒绝理由:</span> <span>
                    <asp:TextBox ID="txtRefusalReason" runat="server" CssClass="forminput" TextMode="MultiLine" Height="60" Width="243"/></span></p>
    
            <div style="text-align: center;padding-top:10px;">
                <input type="button" id="Button2" onclick="javascript:acceptRequest();" class="submit_DAqueding" value="审核通过" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript:refuse();" class="submit_DAqueding" value="拒绝通过" />
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidUserId" runat="server" />
        <input type="hidden" id="hidRefusalReason" runat="server" />
        <asp:Button ID="btnAccept" runat="server" CssClass="submit_DAqueding" Text="审核通过" />
        <asp:Button ID="btnRefuse" runat="server" CssClass="submit_DAqueding" Text="拒绝通过" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script language="javascript" type="text/javascript">
    function CheckReferral(userId, realName, cellPhone, weChat, address, referralReason) {
        $("#ctl00_contentHolder_hidUserId").val(userId);
        $("#ctl00_contentHolder_lblRealname").html(realName);
        $("#ctl00_contentHolder_lblCellphone").html(cellPhone);
        $("#ctl00_contentHolder_lblWeChat").html(weChat);
        $("#ctl00_contentHolder_lblAddress").html(address);
        $("#ctl00_contentHolder_lblReferralReason").html(referralReason);

        setArryText('ctl00_contentHolder_txtRefusalReason', '');

        ShowMessageDialog("审核", "checkReferral", "CheckReferral");
    }

    function acceptRequest() {
        $("#ctl00_contentHolder_btnAccept").trigger("click");
    }

    function refuse() {
        var refusalReason = $("#ctl00_contentHolder_txtRefusalReason").val();
        if (refusalReason.length == 0) {
            alert("请输入拒绝理由！");
            return false;
        }
        $("#ctl00_contentHolder_hidRefusalReason").val(refusalReason);
        $("#ctl00_contentHolder_btnRefuse").trigger("click");
    }
</script>
</asp:Content>