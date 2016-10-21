<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ReplyReceivedMessages.aspx.cs" Inherits="EcShop.UI.Web.Admin.ReplyReceivedMessages" %>
<%@ Import Namespace="EcShop.Core"%>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom"> 
      <em><img src="../images/07.gif" width="32" height="32" /></em>
      <h1>回复客户消息</h1>
    <span>回复客户站内信消息</span></div>
    <div class="datalist">
      <table width="200" border="0" cellspacing="0" >
        <tr class="table_title">
          <td colspan="3" class="td_right td_right_fff"><span class="Name float"><a href="#"><asp:Label ID="litAddresser" runat="server"></asp:Label></a></span>  <Hi:FormatedTimeLabel ID="litDate" ShopTime="true" runat="server"  /></td>
        </tr>
        <tr>
          <td width="9%" align="right"><strong>标题：</strong></td>
          <td width="91%" colspan="2">  <asp:Literal ID="litTitle" runat="server" ></asp:Literal></td>
        </tr>
        <tr >
          <td style="border:none;" align="right"><strong>内容：</strong></td>
          <td colspan="2" style="border:none;"><span class="line">
          <textarea id="txtContent" runat="server" name="txtContent" style="color:#A0A0A0;width:400px" readonly="readonly" rows="10" ></textarea></span></td>
        </tr>
        </table>
      <div class="Settlement"></div>
       
      <table width="200" border="0" cellspacing="0" >
      <tr>
          <td width="9%" align="right" style="border:none;"><strong>标题:</strong></td>
          <td width="91%"  style="border:none;"><asp:TextBox ID="txtTitle" runat ="server" CssClass="forminput bor" ></asp:TextBox></td>
        </tr>
        <tr>
        <td>&nbsp;</td>
        <td> <p id="txtTitleTip" runat="server">标题长度限制在1-60个字符内</p></td>
        </tr>
        <tr>
          <td width="9%" align="right" style="border:none;"><strong>回复:</strong></td>
          <td width="91%" style="border:none;">
          <textarea id="txtContes" runat="server"   name="txtContes" rows="10" cols="54" style="width:400px;height:160px;"></textarea></td>
        </tr>
        <tr>
        <td>&nbsp;</td>
        <td> <p id="txtContesTip" runat="server">回复长度限制在1-300个字符内</p></td>
        </tr>
        <tr>
          <td align="right" >&nbsp;</td>
          <td ><asp:Button ID="btnReplyReplyReceivedMessages" OnClientClick="return PageIsValid();" runat="server" Text="回复" CssClass="submit_DAqueding"  /></td>
        </tr>
      </table>
      
  </div>
 
  </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
       <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 60, false, null, '必填 回复标题不能为空，长度限制在60个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtContes', 1, 300, false, null, '必填 回复不能为空，长度限制在300个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>