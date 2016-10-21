<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SendOrderSettings.aspx.cs" Inherits="EcShop.UI.Web.Admin.SendOrderSettings" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
  <script type="text/javascript" language="javascript">
      function PageIsValid()
      {
          //var email = $.trim($("#ctl00_contentHolder_txtEmail").val());
          //var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
          //if (filter.test(email)) return true;
          //else {
          //    alert('邮箱格式不正确');
          //    return false;
          //}
          return true;
      }

    </script>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title clearfix">
                <em>
                    <img src="../images/05.gif" width="32" height="32" /></em>
                <h1>销售情况推送设置</h1>
            </div>
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>起始时间点：</span>
                        <asp:TextBox ID="txtStartTime" MaxLength="2" CssClass="forminput" runat="server"  Text="18"/>
                        <p id="ctl00_contentHolder_txtStartTime">只能是数字类型（小时）</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">终止时间点：</span>
                         <asp:TextBox ID="txtEndTime" MaxLength="2" CssClass="forminput" runat="server" Text="18" />
                        <p id="ctl00_contentHolder_txtEndTime">只能是数字类型（小时）</p>
                    </li>
                    <li><span class="formitemtitle Pw_198"><em>*</em>延迟天数：</span>
                        <asp:TextBox ID="txtDay" MaxLength="2" CssClass="forminput" runat="server"  Text="1"/>
                        <p id="ctl00_contentHolder_txtAddressTip">只能是数字类型（天）</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">收件邮箱地址：</span>
                        <asp:TextBox ID="txtEmail"  CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtEmail">将订单销售情况推送至此邮箱地址,多个邮箱用“,”分开，如：12@qq.com,32@qq.com</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">执行时间点：</span>
                        <asp:TextBox ID="txtRuntimes"  CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtRuntimes">指定整点，多时间点用“,”分开（如：8,9）</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">是否启动：</span>
                        <asp:RadioButtonList ID="RbIsRun" runat="server" RepeatDirection="Horizontal" Height="16px" Width="136px">
                            <asp:ListItem Value="1">启动</asp:ListItem>
                            <asp:ListItem Value="0">停止</asp:ListItem>
                        </asp:RadioButtonList>
                    </li>
                </ul>
                <ul class="btntf Pa_100 clear">
                    <asp:Button ID="btnSave" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" runat="server" />
                </ul>
            </div>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>