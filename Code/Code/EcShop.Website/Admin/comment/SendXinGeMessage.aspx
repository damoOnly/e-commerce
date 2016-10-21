<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.SendXinGeMessage" MasterPageFile="~/Admin/Admin.Master" EnableSessionState="ReadOnly" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">


    <div class="dataarea mainwidth">
    <div class="areaform">
        <ul>

            <li><span class="formitemtitle Pw_100"><em>*</em>标题：</span>
                <asp:TextBox ID="txtTitle" runat="Server" CssClass="forminput"></asp:TextBox>
                <p id="txtTitleTip" runat="server">标题长度限制在1-60个字符内</p>
            </li>
            <li><span class="formitemtitle Pw_100"><em>*</em>内容：</span>
                <asp:TextBox ID="txtContent" Height="120" TextMode="MultiLine" runat="Server" Width="360"></asp:TextBox>
                <p id="ctl00_contentHolder_txtContentTip">内容长度限制在1-300个字符内</p>
            </li>


            <li><span class="formitemtitle Pw_100">推送类型：</span><input type="radio" name="rdoList" value="11" checked="true" id="rdoTopic" runat="server" />专题<input type="radio" name="rdoList" value="12"  id="rdoCountdown" runat="server" />限时抢购</li>

            <li><span class="formitemtitle Pw_100">关联id：</span>
                <asp:TextBox ID="txtRelateIds" runat="server" TextMode="MultiLine" Rows="8" Columns="50"></asp:TextBox>
                <p class="Pa_100 colorR">一行一个id</p>
            </li>

            <li><span class="formitemtitle Pw_100">推送环境(iOS有效)：</span><input type="radio" name="rdoEnvList" value="2" checked="true" id="rdoEnvDev"  runat="server" />开发环境<input type="radio" name="rdoEnvList" value="1"  id="rdoEnvProd" runat="server" />生产环境</li>
        </ul>
    </div>
    <div class="btn Pa_100">
        <asp:Button runat="server" ID="btnSend" Text="发 送" class="submit_DAqueding" />
    </div>
    <!--搜索-->
    <!--数据列表区域-->


    <!--数据列表底部功能区域-->
    </div>
	<div class="databottom"></div>




</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 60, false, null, '标题长度限制在1-60个字符内'))
            initValid(new InputValidator('ctl00_contentHolder_txtContent', 1, 300, false, null, '内容长度限制在1-300个字符内'))
        }
        $(document).ready(function () { InitValidators(); });
       
    </script>
</asp:Content>
