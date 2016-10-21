<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.SiteContent" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>商城基本设置</h1>
        <span>商城基本信息设置</span>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
          <ul>
            <li><span class="formitemtitle Pw_198"><em >*</em>商城名称：</span>
              <asp:TextBox ID="txtSiteName" CssClass="forminput formwidth" runat="server"  />
              <p id="txtSiteNameTip" runat="server">商城名称为必填项，长度限制在60字符以内</p>
            </li>
            <li><span class="formitemtitle Pw_198">商城标志：</span>              
              <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />
              <asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" style=" margin-left:5px;"/>              
                <table width="800" border="0" cellspacing="0" style="clear:both;" class="Pa_198">
                  <tr>
                    <td valign="middle" style="padding-top:10px; padding-left:200px;"><Hi:HiImage ID="imgLogo" runat="server" Width="180" Height="40" />&nbsp;<Hi:ImageLinkButton ID="btnDeleteLogo"  runat="server" Text="删除" IsShow="true" /></td>
                  </tr>
                  <tr><td align="left"></td></tr>
                </table>
            </li>
            <li style="padding-top:5px;"><span class="formitemtitle Pw_198"><em >*</em>网店授权域名：</span>
              <span class="float">http://</span><asp:TextBox ID="txtDomainName" CssClass="forminput" runat="server"></asp:TextBox>
              <p id="txtDomainNameTip" runat="server">只有在此域名下，系统才是授权状态，某些收费功能才能使用</p>
            </li>
            <li> <span class="formitemtitle Pw_198">是否开启伪静态：</span>
              <Hi:YesNoRadioButtonList ID="radEnableHtmRewrite" runat="server" RepeatLayout="Flow" />              
            </li>    
            <li><span class="formitemtitle Pw_198">自定义页尾：</span>
              <span style="display:block; float:left;width:78%;height:300px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fkFooter" runat="server" Width="98%" Height="300px"/></span>
            </li>
               <li><span class="formitemtitle Pw_198">会员注册协议：</span>

            <span style="display:block; float:left;width:78%;height:300px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fckRegisterAgreement" ImportLib="false" runat="server" Width="98%" Height="300px"/></span>
            </li>
            <h2 class="clear">系统接口设置</h2>
            <li><span class="formitemtitle Pw_198">安全校验码：</span>
           <asp:Literal ID="litKeycode" runat="server" />
            <p id="P1" runat="server">系统开放的接口，需要用到此安全校验码进行签名验证，如果安全校验码泄漏，请联系客户人员更换</p>
            </li>
            
            <h2 class="clear">商品设置</h2>
            <li><span class="formitemtitle Pw_198">商品价格精确到小数点后位数：</span>
              <span class="formselect"><Hi:DecimalLengthDropDownList ID="dropBitNumber" runat="server" /></span>
            </li>
            <li><span class="formitemtitle Pw_198">默认商品图片：</span>
                 <div class="Pa_198 Pg_8"><Hi:ImageUploader runat="server" ID="uploader1" /></div>              
            </li>
            <li><span class="formitemtitle Pw_198">“您的价”重命名为：</span>
                <asp:TextBox Id="txtNamePrice" runat="server" CssClass="forminput"></asp:TextBox>
                <span id="txtNamePriceTip" runat="server"></span>
            </li> 
       
            <h2 class="clear">会员设置</h2>
            <li><span class="formitemtitle Pw_198">是否开启前台购买：</span>
            <Hi:YesNoRadioButtonList ID="radiIsOpenSiteSale" runat="server" RepeatLayout="Flow" />  
            </li>            
              <h2 class="clear"><table><tr><td>在线客服设置</td></tr></table></h2>
              <li class="clear"></li>
                 
            <li> <span class="formitemtitle Pw_198">在线客服：</span>
                <span style="display:block; float:left;width:78%;height:300px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fcOnLineServer" runat="server" Width="98%" ImportLib="false" Height="300px"/></span>
            </li>
          </ul>
          <div style="clear:both"></div>
           <ul class="btntf Pa_198">
            <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
	       </ul>
        </div>
      </div>
</div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtSiteName', 1, 60, false, null, '商城名称为必填项，长度限制在60字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtDomainName', 1,128, false, '', '商城域名必须控制在128个字符以内'))
       
        initValid(new InputValidator('ctl00_contentHolder_txtTQCode', 0, 4000, true, null, '请在这里填入您获取的网页客服代码'))
        initValid(new SelectValidator('ctl00_contentHolder_dropBitNumber', false, '设置商品的价格经过运算后数值精确到小数点后几位，超出将四舍五入。'))
        initValid(new InputValidator('ctl00_contentHolder_txtNamePrice', 1, 10, true, null, '您的价长度不能超过10个字符'))
       
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>

