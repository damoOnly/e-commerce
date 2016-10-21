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
                <h1>添加原产地</h1>
                <span>填写原产地信息</span>
            </div>
            <div class="datafrom">
                <div class="formitem validator2">
                    <ul>
                        <li class="clearfix"><span class="formitemtitle Pw_110">描述：</span>
                            <asp:TextBox ID="txtRemark" runat="server" CssClass="forminput"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtRemarkTip">限定在60个字符</p>
                        </li>
                        <li><span class="formitemtitle Pw_110">图标：</span>
                            <div class="uploadimages">
                                <Hi:ImageUploader runat="server" ID="uploaderIcon" />
                            </div>
                            <p class="Pa_198 clearfix">
                                图片应小于120k，jpg,gif,jpeg,png或bmp格式。建议为500x500像素
                            </p>
                        </li>
                        <li class="clearfix" style="overflow:inherit;height:27px;"><span class="formitemtitle Pw_110">国家：</span>
                            <span class="field" style="width:300px">
                                <input type="text" name="txtCountry" id="txtCountry" placeholder="请输入国家" maxlength="20" />
                                <a href="javascript:void(0)" onclick="clearAutoCompleterCache()">清除缓存</a>
                                <asp:HiddenField runat="server" ID="hidCountryId" />
                                <asp:HiddenField runat="server" ID="hidCountry" />
                            </span>
                        </li>
                        <li class="clearfix"><span class="formitemtitle Pw_110">短名称：</span>
                            <asp:TextBox ID="txtCnArea" runat="server" CssClass="forminput"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtCnAreaTip">限定在60个字符</p>
                        </li>
                        <li><span class="formitemtitle Pw_110">长名称：</span>
                            <asp:TextBox ID="txtEnArea" runat="server" CssClass="forminput"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtEnAreaTip">限定在60个字符</p>
                        </li>

                        <%--<li> <span class="formitemtitle Pw_110">海关代码：</span>
               <asp:TextBox ID="txtHSCode" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtHSCodeTip">限定在50个字符</p>
            </li> --%>
                        <li><span class="formitemtitle Pw_110">是否最惠国：</span>
                            <asp:RadioButton runat="server" ID="radFavourableFlagY" GroupName="FavourableFlag" Text="是" Checked="true"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="radFavourableFlagN" GroupName="FavourableFlag" Text="否"></asp:RadioButton>
                        </li>

                        <li><span class="formitemtitle Pw_110">排序：</span>
                            <Hi:DisplaySequenceDropDownList runat="server" AllowNull="true" ID="ddListDisplaySequence" Width="174" Height="30" CssClass="forminput" />
                        </li>
                    </ul>
                    <ul class="btntf Pw_110">
                        <asp:Button ID="btnAdd" runat="server" Text="添加" OnClientClick="return PageIsValid();" CssClass="submit_DAqueding inbnt" />
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
            initValid(new InputValidator('ctl00_contentHolder_txtCnArea', 1, 60, false, null, '短名称,在1至60个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtEnArea', 1, 60, false, null, '长名称,在1至60个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtRemark', 1, 100, false, null, '描述,在1至100个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtHSCode', 1, 50, false, null, '海关代码,在1至50个字符之间'))
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


        //清除所有缓存：
        function clearAutoCompleterCache() {
            $('#txtCountry').autocompleter('clearCache');
            alert("清除成功");
        }
    </script>
</asp:Content>
