<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SendVoucherToUsers.aspx.cs" Inherits="EcShop.UI.Web.Admin.SendVoucherToUsers" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">


    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom">
            <em>
                <img src="../images/06.gif" width="32" height="32" /></em>
            <h1>发送现金券</h1>
            <span>给商城会员发送现金券，您可以给满足筛选条件的所有会员发送，也可以给根据会员名称发送</span>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle Pw_100">发送对象：</span><input type="radio" name="rdoList" value="1" onclick="selectMemberName()" checked="true" id="rdoName" runat="server" />发送给指定名称的会员<input type="radio" name="rdoList" value="2" onclick="    selectCondition()" id="rdoRank" runat="server" />发送给指定的条件的会员</li>
                </ul>
                <ul id="membernameselect">
                    <li><span class="formitemtitle Pw_100">会员名：</span>
                        <asp:TextBox ID="txtMemberNames" runat="server" TextMode="MultiLine" Rows="8" Columns="50"></asp:TextBox>
                        <p class="Pa_100">一行一个会员名</p>
                    </li>
                </ul>
                <ul id="conditionselect">
                    <li><span class="formitemtitle Pw_100">会员等级：</span><Hi:MemberGradeDropDownList ID="rankList" class="formselect input100" runat="server" AllowNull="true" NullToDisplay="全部" /></li>
                    <li>
                        <span class="formitemtitle Pw_100">注册时间：从&nbsp;</span>
                        <UI:WebCalendar ID="registerFromDate" runat="server" CssClass="forminput" />
                        <span>&nbsp;到&nbsp;</span>
                        <UI:WebCalendar ID="registerToDate" runat="server" CssClass="forminput" />
                    </li>
                    <li><span class="formitemtitle Pw_100">&nbsp;地区：</span>
                        <abbr class="formselect">
                            <Hi:RegionSelector runat="server" ID="ddlReggion" />
                        </abbr>
                    </li>
                </ul>
                <ul class="btntf Pa_100">
                    <asp:Button runat="server" ID="btnSend" Text="发 送" class="submit_DAqueding inbnt" />
                </ul>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">

        function selectCondition() {
            $("#membernameselect").css("display", "none");
            $("#conditionselect").css("display", "");

        }


        function selectMemberName() {
            $("#membernameselect").css("display", "");
            $("#conditionselect").css("display", "none");

        }

        $(document).ready(function () {
            //if (document.getElementById(ctl00_contentHolder_rdoName).checked = "true") {
            //    selectMemberName();
            //}
            if ($('#ctl00_contentHolder_rdoName').attr("checked") == "checked") {
                selectMemberName();
            }
            else {
                selectCondition();
            }
        });
    </script>

</asp:Content>
