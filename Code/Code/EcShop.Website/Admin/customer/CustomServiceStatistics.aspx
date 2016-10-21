<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.CustomServiceStatistics" MasterPageFile="~/Admin/Admin.Master" %>
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
  <h1>客服数据统计</h1>
  <span>查询一段时间内客服的服务信息</span>
</div>
      <div>
        <!--数据列表区域-->
	  <div class="datalist">
          	<!--分页功能-->
		<div class="pageHandleArea">
				<ul>
				   <input type="hidden" runat="server" id="hidPageSize" />
				   <input type="hidden" runat="server" id="hidPageIndex" />
				</ul>
		  </div>
      		<!--搜索-->
   <div class="searcharea clearfix ">
          <ul class="a_none_left">
            <li><span>分析方式：</span><span class="formselect"><select runat="server" name="selType" id="selType"><option value="1">按日期统计</option><option value="2">按月统计</option></select></span></li>
           <li><span>起始日期：</span><span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" class="forminput"  /></span></li>
           <li><span>终止日期：</span><span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" class="forminput" /></span></li>
<%--              <li><span  class="formselect">客服：</span><span><asp:DropDownList ID="dropStaffs" runat="server"></asp:DropDownList></span></li>--%>
            <li> <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/> </li>
             <li><p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" /></p></li>
          </ul>
      </div>
		<!--结束-->
        <div class="blank12 clearfix"></div>
	  <UI:Grid ID="grdCustomServiceStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>                                                                                                                  
                             <asp:TemplateField HeaderText="日期" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblOperDate"  runat="Server" Text='<%# Eval("operdate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>   
                             <asp:TemplateField HeaderText="工号" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkerNo"  runat="Server" Text='<%# Eval("workerno") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>   
                              <asp:TemplateField HeaderText="接待人数" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceptionPerson"  runat="Server" Text='<%# Eval("ReceptionPerson") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>  
                              <asp:TemplateField HeaderText="首次响应时间/秒" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="lblFirstReply"  runat="Server" Text='<%# Eval("FirstReply") %>' />
                                </ItemTemplate>
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="平均响应时间/秒" HeaderStyle-CssClass="td_right td_left">
                               <ItemTemplate>
                                    <asp:Label ID="lblAverageReply"  runat="Server" Text='<%# Eval("AverageReply") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField HeaderText="回复次数" HeaderStyle-CssClass="td_right td_left">
                               <ItemTemplate>
                                    <asp:Label ID="lblReplyCount"  runat="Server" Text='<%# Eval("ReplyCount") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>   
                    </Columns>
                </UI:Grid>
                
      <div class="blank12 clearfix"></div>
    <div class=" VIPbg m_none colorG"></div>
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

