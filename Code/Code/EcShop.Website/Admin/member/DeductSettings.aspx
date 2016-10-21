<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DeductSettings.aspx.cs" Inherits="EcShop.UI.Web.Admin.DeductSettings" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtReferralDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置直接推广佣金'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtReferralDeduct', 0, 100, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置下级会员佣金'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSubMemberDeduct', 0, 100, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubReferralDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置下级推广员佣金'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSubReferralDeduct', 0, 100, '输入的数值超出了系统表示范围'));

        }
        $(document).ready(function () {
            InitValidators();
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1 class="title_line">分佣规则设置 </h1>
            <span class="font">推广员可以通过分享商品被购买后获取直接推广佣金，也可以通过发展下级会员来获取下级会员的购买佣金，还可以通过发展下级推广员来获取下级推广员的佣金</span>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <div class="emailhead">
<p>推广员可通过推广链接/二维码发展会员，并与其形成上下级关系。买家通过非推广链接方式进入商城注册会员后，属于平台会员，无上级推广员，后续也不可再绑定</p>
<p>会员未通过分享链接进入商城时产生的订单，其绑定推广员的上级推广员不享受任何佣金。当已有绑定推广员的会员通过其他推广员分享的链接产生订单时，该订单仅对分享链接的推广员产生有效佣金</p>
<p></p>
</div>
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>直接推广佣金：</span>
                        <asp:TextBox ID="txtReferralDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <p id="txtReferralDeductTip" runat="server">推广员分享链接产生有效订单时享受的佣金比例</p>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>下级会员佣金：</span>
                        <asp:TextBox ID="txtSubMemberDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <p id="txtSubMemberDeductTip" runat="server">下级会员未通过分享链接进入商城时产生的订单，推广员可享受的佣金比例</p>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>下级推广员佣金：</span>
                        <asp:TextBox ID="txtSubReferralDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <p id="txtSubReferralDeductTip" runat="server">推广员分享链接产生有效订单时，其上级推广员也可获得相应的佣金比例</p>
                    </li>
                </ul>
                <div style="clear: both"></div>
                <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
