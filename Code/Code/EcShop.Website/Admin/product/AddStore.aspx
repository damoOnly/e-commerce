<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddStore.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddStore" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
        <script type="text/javascript" language="javascript">

        function PageIsValid() {
            var supplierName = $.trim($("#ctl00_contentHolder_txtSupplierName").val());
            if (supplierName == "") {
                alert("门店名称不能为空");
                return false;
            }


            var address = $.trim($("#ctl00_contentHolder_txtAddress").val());
            if (address == "") {
                alert("详细地址不能为空");
                return false;
            }

            var phone = $.trim($("#ctl00_contentHolder_txtPhone").val());
            var regphone = /^(\d{3,4}\-)?[1-9]\d{6,7}$/;
            if (phone != "") {
                if (!regphone.test(phone)) {
                    alert("电话号码格式不正确");
                    return false;
                }
            }

            var mobile = $.trim($("#ctl00_contentHolder_txtMobile").val());
            var regmobile = /^1[3|4|5|7|8][0-9]\d{4,8}$/;
            if (mobile != "") {
                if (!regmobile.test(mobile)) {
                    alert("手机号码格式不正确");
                    return false;
                }
            }

            return true;
        }

    </script>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title clearfix">
                <em>
                    <img src="../images/05.gif" width="32" height="32" /></em>
                <h1>添加门面</h1>
            </div>
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>门面名称：</span>
                        <asp:TextBox ID="txtSupplierName" MaxLength="30" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtSupplierNameTip">门面名称不能为空，长度限制在30个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">地区：</span>
                        <abbr class="formselect">
                            <Hi:RegionSelector runat="server" ID="ddlReggion" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198"><em>*</em>详细地址：</span>
                        <asp:TextBox ID="txtAddress" MaxLength="50" CssClass="forminput" runat="server" Width="300px" />
                        <p id="ctl00_contentHolder_txtAddressTip">门面地址不能为空，长度限制在30个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">手机号码：</span>
                        <asp:TextBox ID="txtMobile" MaxLength="15" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtMobileTip">手机号码的长度限制在15个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">联系电话：</span>
                        <asp:TextBox ID="txtPhone" MaxLength="15" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtPhoneTip">电话号码的长度限制在15个字符以内</p>
                    </li>

                    <li><span class="formitemtitle Pw_198">备注：</span>
                        <asp:TextBox ID="txtDescription" MaxLength="50" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtDescriptionTip">备注长度限制在50个字符以内</p>
                    </li>
                </ul>
                <ul class="btntf Pa_100 clear">
                    <asp:Button ID="btnSave" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" runat="server" />
                </ul>
            </div>

        </div>
    </div>
</asp:Content>

