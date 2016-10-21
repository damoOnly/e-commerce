<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddImportSourceType.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddImportSourceType" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="/admin/css/pro-list.css" type="text/css" />
    <link rel="stylesheet" href="/admin/css/autocompleter.main.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="areacolumn clearfix validator4">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/06.gif" width="32" height="32" /></em>
                <h1>���ԭ����</h1>
                <span>��дԭ������Ϣ</span>
            </div>
            <div class="datafrom">
                <div class="formitem validator2">
                    <ul>
                        <li class="clearfix"><span class="formitemtitle Pw_110">������</span>
                            <asp:TextBox ID="txtRemark" runat="server" CssClass="forminput"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtRemarkTip">�޶���60���ַ�</p>
                        </li>
                        <li><span class="formitemtitle Pw_110">ͼ�꣺</span>
                            <div class="uploadimages">
                                <Hi:ImageUploader runat="server" ID="uploaderIcon" />
                            </div>
                            <p class="Pa_198 clearfix">
                                ͼƬӦС��120k��jpg,gif,jpeg,png��bmp��ʽ������Ϊ500x500����
                            </p>
                        </li>
                        <li class="clearfix" style="overflow:inherit;height:27px;"><span class="formitemtitle Pw_110">���ң�</span>
                            <span class="field" style="width:300px">
                                <input type="text" name="txtCountry" id="txtCountry" placeholder="���������" maxlength="20" />
                                <a href="javascript:void(0)" onclick="clearAutoCompleterCache()">�������</a>
                                <asp:HiddenField runat="server" ID="hidCountryId" />
                                <asp:HiddenField runat="server" ID="hidCountry" />
                            </span>
                        </li>
                        <li class="clearfix"><span class="formitemtitle Pw_110">�����ƣ�</span>
                            <asp:TextBox ID="txtCnArea" runat="server" CssClass="forminput"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtCnAreaTip">�޶���60���ַ�</p>
                        </li>
                        <li><span class="formitemtitle Pw_110">�����ƣ�</span>
                            <asp:TextBox ID="txtEnArea" runat="server" CssClass="forminput"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtEnAreaTip">�޶���60���ַ�</p>
                        </li>

                        <%--<li> <span class="formitemtitle Pw_110">���ش��룺</span>
               <asp:TextBox ID="txtHSCode" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtHSCodeTip">�޶���50���ַ�</p>
            </li> --%>
                        <li><span class="formitemtitle Pw_110">�Ƿ���ݹ���</span>
                            <asp:RadioButton runat="server" ID="radFavourableFlagY" GroupName="FavourableFlag" Text="��" Checked="true"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="radFavourableFlagN" GroupName="FavourableFlag" Text="��"></asp:RadioButton>
                        </li>

                        <li><span class="formitemtitle Pw_110">����</span>
                            <Hi:DisplaySequenceDropDownList runat="server" AllowNull="true" ID="ddListDisplaySequence" Width="174" Height="30" CssClass="forminput" />
                        </li>
                    </ul>
                    <ul class="btntf Pw_110">
                        <asp:Button ID="btnAdd" runat="server" Text="���" OnClientClick="return PageIsValid();" CssClass="submit_DAqueding inbnt" />
                    </ul>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="/Admin/js/jquery.min.js"></script>
    <script src="/Admin/js/jquery.autocompleter.js"></script>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtCnArea', 1, 60, false, null, '������,��1��60���ַ�֮��'))
            initValid(new InputValidator('ctl00_contentHolder_txtEnArea', 1, 60, false, null, '������,��1��60���ַ�֮��'))
            initValid(new InputValidator('ctl00_contentHolder_txtRemark', 1, 100, false, null, '����,��1��100���ַ�֮��'))
            initValid(new InputValidator('ctl00_contentHolder_txtHSCode', 1, 50, false, null, '���ش���,��1��50���ַ�֮��'))
        }
        $(document).ready(function () {
            InitValidators();
            $('#txtCountry').autocompleter({
                // marker for autocomplete matches
                highlightMatches: true,
                // object to local or url to remote search
                source: "/Handler/HelpHandler.ashx?action=GetCountry",
                // custom template
                template: '{{ label }} <span>({{ Ename }})</span>',
                // abort source if empty field
                empty: false,
                // max results
                limit: 5,
                callback: function (value, index, selected) {
                    if (selected) {
                        $("#<%=txtCnArea.ClientID%>").val(selected.label);
                        $("#<%=txtCnArea.ClientID%>").focus();
                        $("#<%=txtEnArea.ClientID%>").val(selected.label);
                        $("#<%=txtEnArea.ClientID%>").focus();
                        $("#<%=hidCountry.ClientID%>").val(selected.label);
                        $("#<%=hidCountryId.ClientID%>").val(selected.HSCode);
                    }
                }
            });
        });


        //������л��棺
        function clearAutoCompleterCache() {
            $('#txtCountry').autocompleter('clearCache');
            alert("����ɹ�");
        }
    </script>
</asp:Content>
