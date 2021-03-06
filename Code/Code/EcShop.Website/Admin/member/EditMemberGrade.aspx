﻿<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMemberGrade.aspx.cs" Inherits="EcShop.UI.Web.Admin.EditMemberGrade" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>编辑会员等级</h1>
            <span>使用会员等级区分买家的级别，不同级别的买家可以享受不同的折扣率.. </span>
          </div>
      <div class="formitem validator4">
        <ul>
          <li> <span class="formitemtitle Pw_110"><em >*</em>会员等级名称：</span>
            <asp:TextBox ID="txtRankName" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtRankNameTip">会员等级名称不能为空，长度限制在20字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>积分满足点数：</span>
            <asp:TextBox ID="txtPoint" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtPointTip">设置会员的积分达到多少分以后自动升级到此等级，为大于等于0的整数</p>
          </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>会员等级价格：</span>
             <span><table width="400" border="0" cellspacing="0">
                          <tr>
                            <td width="70">一口价  ×</td>
                            <td width="39"><asp:TextBox ID="txtValue" CssClass="forminput" Width="90px" runat="server"  /></td>
                            <td width="25" align="center"> %</td>
                            <td width="269" align="center">&nbsp;</td>
                          </tr>
                        </table>
                    </span><br />
		    <p id="ctl00_contentHolder_txtValueTip">等级折扣不能为空，且是数字</p>
          </li>
          <li><span class="formitemtitle Pw_110">备注：</span>
            <asp:TextBox ID="txtRankDesc" runat="server" TextMode="MultiLine"  CssClass="forminput" Width="450" Height="120"></asp:TextBox>
            <p id="ctl00_contentHolder_txtRankDescTip"></p>
          </li>
      </ul>
      <ul class="btn Pa_110">
        <asp:Button ID="btnSubmitMemberRanks" OnClientClick="return PageIsValid();" Text="确 定" CssClass="submit_DAqueding" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
        <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtRankName', 1, 60, false, null, '会员等级名称不能为空，长度限制在60个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtPoint', 1, 10, false, '-?[0-9]\\d*', '设置会员的积分达到多少分以后自动升级到此等级，为大于等于0的整数'));
                appendValid(new NumberRangeValidator('ctl00_contentHolder_txtPoint', 0, 2147483647, '设置会员的积分达到多少分以后自动升级到此等级，为大于等于0的整数'));
                initValid(new InputValidator('ctl00_contentHolder_txtValue', 1, 10, false, '-?[0-9]\\d*', '等级折扣为不能为空，且是整数'));
                appendValid(new NumberRangeValidator('ctl00_contentHolder_txtValue', 1, 1000, '等级折扣必须在1-1000之间'));
                initValid(new InputValidator('ctl00_contentHolder_txtRankDesc', 0, 100, true, null, '备注的长度限制在100个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>