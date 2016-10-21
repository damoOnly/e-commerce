<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageLotteryActivity.aspx.cs" Inherits="EcShop.UI.Web.Admin.vshop.ManageLotteryActivity" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1><asp:Literal ID="LitTitle" runat="server">大转盘活动管理</asp:Literal>
        </h1>
        
     <span>您可以在此管理好您的<asp:Literal ID="Litdesc" runat="server"></asp:Literal>活动，并在自定义回复中使用它们。</span></div>
    <!-- 添加按钮-->
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">
    	<div class="searcharea clearfix br_search singbtn">
             <span class="signicon"></span>
			 <a  id="addactivity" runat="server">添加新活动</a>
		</div>
          <asp:Repeater ID="rpMaterial" runat="server" 
            onitemcommand="rpMaterial_ItemCommand" >
                    <HeaderTemplate>
                        <table border="0" cellspacing="0" width="80%" cellpadding="0">
                            <tr class="table_title">
                                <td>
                                  活动名称
                                </td>
                                <td>
                                  活动关键字
                                </td>
                                <td>活动开始时间</td>
                                  <td>活动结束时间
                                </td>
                                <td>
                                    操作
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                              <%#Eval("ActivityName")%><br />
                              <%#GetUrl(Eval("activityId"))%>
                            </td>
                             <td>
                              <%#Eval("ActivityKey")%>
                            </td>
                            <td><%#Eval("StartTime")%></td>
                            <td>
                             <%#Eval("EndTime")%>
                            </td>
                          <td> <a href='UpdateLotteryActivity.aspx?typeid=<%=type%>&id=<%#Eval("ActivityID")%>'>编辑</a> 
                              <asp:LinkButton ID="Lkbtndel" CommandName="del" CommandArgument='<%#Eval("ActivityID")%>' runat="server"  OnClientClick="javascript:return confirm('确定要执行删除操作吗？删除后将不可以恢复')">删除</asp:LinkButton>
                              <a href='PrizeRecord.aspx?id=<%#Eval("ActivityID")%>'>查看中奖信息</a> 
                              </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table></FooterTemplate>
                </asp:Repeater>        
                
    </div>
    <!--数据列表底部功能区域-->

      <div class="page">
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
  </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
