<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.AppAliPaySet" CodeBehind="AppAliPaySet.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title  m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>手机支付宝收款信息设置</h1><div>
       
        <span>建议您尽量使用支付宝手机应用内支付。如果您已经申请了支付宝手机网页支付，并且只想使用该支付方式，您可以点击
        <a href="javascript:void(0)" onclick="showWapPay()">这里</a> 进行配置支付宝手机网页支付。</span>
        </div>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>

          <li><h2 class="colorE">支付宝手机应用内支付</h2></li>
            <li class="clearfix"><span>请设置好您的支付宝信息。 还没开通快捷支付(无线)？ <a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2013080604609654">立即免费申请开通支付宝快捷支付(无线)接口</a></span></li>
          
          <li class="clearfix"> <span class="formitemtitle Pw_198">是否开启：</span>
          <abbr class="formselect">
            <Hi:YesNoRadioButtonList ID="radEnableAppAliPay" runat="server" RepeatLayout="Flow" />
          </abbr>
          </li>   
          <li><span class="formitemtitle Pw_198">商家号：</span>
              <asp:TextBox ID="txtAppPartner" CssClass="forminput formwidth" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_198">商家密钥：</span>
              <asp:TextBox ID="txtAppKey" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_198">收款账户：</span>
              <asp:TextBox ID="txtAppAccount" CssClass="forminput formwidth" runat="server" />
            </li>       
            </ul>
            <ul id="ulwapAlipay">
            <li><h2 class="colorE">支付宝手机网页支付</h2></li>
            <li class="clearfix"><span>请设置好您的支付宝信息。 还没开通支付宝？ <a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2013080604609688">立即免费申请开通支付宝接口</a></span></li>
            <li class="clearfix"><span class="formitemtitle Pw_198">是否开启：</span>
                <abbr class="formselect">
            <Hi:YesNoRadioButtonList ID="radEnableWapAliPay" runat="server" RepeatLayout="Flow" />
          </abbr>
            </li>
            <li><span class="formitemtitle Pw_198">商家号：</span>
              <asp:TextBox ID="txtPartner" CssClass="forminput formwidth" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_198">商家密钥：</span>
              <asp:TextBox ID="txtKey" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_198">收款账户：</span>
              <asp:TextBox ID="txtAccount" CssClass="forminput formwidth" runat="server" />
            </li> 
	     </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnAdd" Text="保 存" onclick="btnOK_Click" CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        if ($("input[name='ctl00$contentHolder$radEnableWapAliPay']:checked").val() == "False")
            $("#ulwapAlipay").hide();
    });

function showWapPay() {
    $("#ulwapAlipay").show();
}
</script>
</asp:Content>

