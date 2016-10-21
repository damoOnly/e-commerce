<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TFTPaySet.aspx.cs" Inherits="EcShop.UI.Web.Admin.App.TFTPaySet" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>腾付通收款信息设置</h1>
        <span>请设置好您的腾付通信息。</span>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
          <ul>
            <li><h2 class="colorE">盛付通手机网页支付</h2></li>
           <li class="clearfix"><span class="formitemtitle Pw_198">是否开启：</span>
                <abbr class="formselect">
            <Hi:YesNoRadioButtonList ID="radEnableAppTFTPay" runat="server" RepeatLayout="Flow" />
          </abbr>
            </li>
            <li><span class="formitemtitle Pw_198">商户证书：</span>
              <asp:TextBox ID="txtCert" CssClass="forminput formwidth" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_198">商户密码：</span>
              <asp:TextBox ID="txtKey" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_198">支付公钥路径：</span>
              <asp:TextBox ID="txtPublicKey" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_198">支付网关地址：</span>
              <asp:TextBox ID="txtGateway" CssClass="forminput formwidth" runat="server" />
            </li> 
   <%--         <li><span class="formitemtitle Pw_198">设置须知：</span>
                填写完成以上配置后，盛付通接口将自动启用
            </li>--%>
          </ul>
          <div style="clear:both"></div>
           <ul class="btntf Pa_198">
            <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="submit_DAqueding inbnt" 
                   OnClientClick="return PageIsValid();" onclick="btnOK_Click" />
	       </ul>
        </div>
      </div>
</div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>