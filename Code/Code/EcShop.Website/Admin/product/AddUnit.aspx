<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.product.AddUnit" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>添加计量单位</h1>
                <span>新增计量单位</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pg_29"><em>*</em>海关代码：</span>
                        <asp:TextBox ID="txtHSJoinID" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtHSJoinIDTip">海关代码不能为空，长度限制在4个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pg_29"><em>*</em>单位名称：</span>
                        <asp:TextBox ID="txtUnitName" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtUnitNameTip">计量单位名称不能为空，长度限制在50个字符以内</p>
                    </li>
                </ul>
                <ul class="btntf Pa_100 clear">
                    <asp:Button ID="btnSave" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" runat="server" />
                    <asp:Button ID="btnAddUnit" OnClientClick="return PageIsValid();" Text="保存并继续添加" CssClass="submit_jixu" runat="server" />
                </ul>
            </div>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtUnitName', 1, 50, false, null, '计量单位名称不能为空，长度限制在50个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtHSJoinID', 1, 4, false, null, '海关代码不能为空，长度限制在4个字符以内'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>

