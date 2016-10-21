<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddCountDownManager.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddCountDownManager" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="groupbuy.helper.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/06.gif" width="32" height="32" /></em>
                <h1>添加限时管理</h1>
                <span>填写限时管理详细信息</span>
            </div>

            <div class="formitem validator5" style="padding-left: 15px;">
                <ul>
                    <li></li>
                    <li><span class="formitemtitle Pw_140">活动标题：</span>
                         <asp:TextBox ID="txtTitle" runat="server" CssClass="forminput" />
                    </li>
                    <li><span class="formitemtitle Pw_140"><em>*</em>开始日期：</span>
                        <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarStartDate" Visible="false" /><abbr class="formselect"><Hi:HourDropDownList ID="drophours" runat="server" Style="margin-left: 5px;" /></abbr>
                        <p id="P3">当达到开始日期时，活动会自动变为正在参与活动状态。</p>
                    </li>
                    <li><span class="formitemtitle Pw_140"><em>*</em>结束日期：</span>
                        <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarEndDate" Visible="false" /><abbr class="formselect"><Hi:HourDropDownList ID="HourDropDownList1" runat="server" Style="margin-left: 5px;" /></abbr>
                        <p id="P2">当达到结束日期时，活动会结束。</p>
                    </li>
                    <li> <span class="formitemtitle Pw_140">活动图片：</span>
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />
                    </li>
                    <li> <span class="formitemtitle Pw_140">活动图片连接地址：</span>
                        <asp:TextBox ID="txtActiveImgUrl" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtCompanyUrlTip">活动图片连接地址必须以http://开头，长度限制在100个字符以内'</p>
                    </li>
                </ul>
                <ul class="btn Pa_100 clear">
                    <li>
                        <div style="float:left"><asp:Button ID="btnAddCountDown" runat="server" Text="添加"  CssClass="submit_DAqueding" /></div>&nbsp;
                  
                        <div class="submit_DAqueding"  style="text-align:center;float:left; margin-left:10px;" onclick="javascript:window.location.href='CountDownsManager.aspx'">
                              <a href="#" style="color:white" >返回</a>
	                    </div>
                    </li>
                    
                </ul>
            </div>
        </div>
    </div>

</asp:Content>
