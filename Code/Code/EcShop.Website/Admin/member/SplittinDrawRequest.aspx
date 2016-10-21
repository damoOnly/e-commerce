<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SplittinDrawRequest.aspx.cs" Inherits="EcShop.UI.Web.Admin.SplittinDrawRequest" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/05.gif" width="32" height="32" /></em>
  <h1>推广员结算申请</h1>
  <span>处理推广员的结算申请</span>
</div>

		<!--数据列表区域-->
		<div class="datalist">
        		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li>
                <span>选择时间段：</span>
                <span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" class="forminput"/></span>
                <span class="Pg_1010">至</span>
                <span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate" class="forminput"/></span>
                <span style=" margin-left:11px;">推广员：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"/></span>
              </li>
				<li>
				    <asp:Button ID="btnQuery" runat="server" class="searchbutton" Text="查询" />
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
		    <UI:Grid ID="grdBalanceDrawRequest" DataKeyNames="JournalNumber" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                <Columns>
                    <asp:TemplateField HeaderText="推广员" HeaderStyle-CssClass="td_right td_left">
                        <itemtemplate>			  
                            <asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label>         
                        </itemtemplate>
                        </asp:TemplateField>
                        
                    <asp:TemplateField HeaderText="申请时间" HeaderStyle-CssClass="td_right td_left" >
                        <itemtemplate>
                            <Hi:FormatedTimeLabel ID="lblTime" Time='<%# Eval("RequestDate")%>' runat="server" />
                        </itemtemplate>
                    </asp:TemplateField>
                    <Hi:MoneyColumnForAdmin HeaderText="申请提现金额" DataField="Amount" HeaderStyle-CssClass="td_right td_left" />
                    <Hi:MoneyColumnForAdmin HeaderText="可提现金额" DataField="Balance" HeaderStyle-CssClass="td_right td_left" />
                     <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                             <a href="javascript:void(0)" onclick="return CheckReferral('<%# Eval("JournalNumber") %>', '<%# Eval("Account") %>')">审核</a>       
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

    <div id="divCheckAccount" style="display: none;">
        <div class="frame-content" style="margin-top:-20px;">
            <p> 结算审核</p>
             <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                  <tr>
                    <td align="right" width="30%">收款账号:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:Label ID="lblAccount" runat="server"></asp:Label></td>
                  </tr>    
                  <tr>
                    <td align="right">结算备注:</td>
                    <td align="left"  class="bd_td">&nbsp;<asp:TextBox ID="txtManagerRemark" runat="server" CssClass="forminput" TextMode="MultiLine" Height="60" Width="243"/></td>
                  </tr>
                </table>    
            <div style="text-align: center;padding-top:10px;">
                <input type="button" id="Button2" onclick="javascript:acceptRequest();" class="submit_DAqueding" value="确认结算" />
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidJournalNumber" runat="server" />
        <input type="hidden" id="hidManagerRemark" runat="server" />
        <asp:Button ID="btnAccept" runat="server" CssClass="submit_DAqueding" Text="确认结算" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script language="javascript" type="text/javascript">
    function CheckReferral(journalNumber, account) {
        $("#ctl00_contentHolder_hidJournalNumber").val(journalNumber);
        $("#ctl00_contentHolder_lblAccount").html(account);

        setArryText('ctl00_contentHolder_txtManagerRemark', '');

        ShowMessageDialog("审核", "divCheckAccount", "divCheckAccount");
    }

    function acceptRequest() {
        var managerRemark = $("#ctl00_contentHolder_txtManagerRemark").val();
        if (managerRemark.length == 0) {
            alert("请输入收款备注！");
            return false;
        }
        $("#ctl00_contentHolder_hidManagerRemark").val(managerRemark);
        $("#ctl00_contentHolder_btnAccept").trigger("click");
    }
</script>
</asp:Content>