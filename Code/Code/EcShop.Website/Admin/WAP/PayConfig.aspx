<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.WAPShop.PayConfig" CodeBehind="PayConfig.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>微信收款信息设置</h1>
        <span>请设置好您的微信支付信息。 还没开通微信支付？ <a target="_blank" href="https://mp.weixin.qq.com/cgi-bin/readtemplate?t=news/open-app-apply-guide_tmpl&lang=zh_CN">立即免费申请开通微信支付接口</a></span>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
          <ul>
            <li><span class="formitemtitle Pw_100">AppId：</span>
              <asp:TextBox ID="txtAppId" CssClass="forminput formwidth" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_100">AppSecret：</span>
              <asp:TextBox ID="txtAppSecret" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_100">PartnerID：</span>
              <asp:TextBox ID="txtPartnerID" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_100">PartnerKey：</span>
              <asp:TextBox ID="txtPartnerKey" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_100">PaySignKey：</span>
              <asp:TextBox ID="txtPaySignKey" CssClass="forminput formwidth" runat="server" mo />
            </li> 
            <li><span class="formitemtitle Pw_100">开启微信付款：</span>
                <Hi:YesNoRadioButtonList ID="radEnableHtmRewrite" runat="server" RepeatLayout="Flow" />
            </li>
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

