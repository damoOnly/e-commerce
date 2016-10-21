<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SMSPromotions.aspx.cs" Inherits="EcShop.UI.Web.Admin.SMSPromotions" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>营销短信
            </h1>
            <span>群发短信内容给客户
            </span>
        </div>
        <div class="formitem">
            <ul>
                <li><span class="formitemtitle Pw_140">发送内容：</span>

                </li>
                <li>
                    <asp:TextBox ID="txtSubject" TextMode="MultiLine" Rows="8" Columns="50" runat="server" CssClass="forminput fontlengthtop"></asp:TextBox>
                </li>
                <li class="clear"></li>
            </ul>
            <br />
            <ul class="btntf Pa_140">

                <li>
                    <asp:Button ID="btnSend" runat="server" OnClientClick="return TestCheck();" Text="放入发送短信队列" CssClass="submit_DAqueding inbnt submit_jixu "></asp:Button>
                </li>
            </ul>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <script type="text/javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtSubject', 1, 500, false, null, '内容长度限制不能大于500个字符'));
        }
        function TestCheck() {

            if ($("#ctl00_contentHolder_txtSubject").val().length == 0) {
                alert("请输入发送内容");
                return false;
            }

            if (confirm("确定要群发短信吗？")) {

                return true;
            }
            else {
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
