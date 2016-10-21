<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditActivity.aspx.cs" Inherits="EcShop.UI.Web.Admin.EditActivity" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/06.gif" width="32" height="32" /></em>
                <h1>
                    修改微报名</h1>
                <span>修改微报名信息</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100"><em>*</em>活动名称：</span>
                        <asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtNameTip"></p>
                    </li>
                    <li><span class="formitemtitle Pw_100"><em>*</em>开始时间：</span>
                        <UI:WebCalendar ID="txtStartDate" runat="server" CssClass="forminput" />
                    </li>
                    <li><span class="formitemtitle Pw_100"><em>*</em>结束时间：</span>
                        <UI:WebCalendar ID="txtEndDate" runat="server" CssClass="forminput" />
                    </li>
                    <li><span class="formitemtitle Pw_100">人数上限：</span>
                        <asp:TextBox ID="txtMaxValue" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtMaxValueTip">（不填写，则无上限。）</p>
                    </li>
                    <li><span class="formitemtitle Pw_100">活动简介：</span> 
                        <%--<Kindeditor:KindeditorControl id="txtDescription" runat="server" Width="550px"  height="200px" />--%>
                        <asp:TextBox ID="txtDescription" runat="server" Width="400px" height="100px" TextMode="MultiLine"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_100"><em>*</em>活动表单项：</span> 
                        <asp:TextBox ID="txtItem1" runat="server" CssClass="forminput"></asp:TextBox><p id="ctl00_contentHolder_txtItem1Tip"></p>
                    </li>
                    <li><span class="formitemtitle Pw_100">表单项二：</span>
                        <asp:TextBox ID="txtItem2" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_100">表单项三：</span>
                        <asp:TextBox ID="txtItem3" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_100">表单项四：</span>
                        <asp:TextBox ID="txtItem4" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_100">表单项五：</span>
                        <asp:TextBox ID="txtItem5" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>
                    <li style="display:none"><span class="formitemtitle Pw_100">结束说明：</span>
                        <%--<Kindeditor:KindeditorControl id="txtCloseRemark" runat="server" ImportLib="false" Width="550px"  height="200px" />--%>
                        <asp:TextBox ID="txtCloseRemark" runat="server" Width="400px" height="100px" TextMode="MultiLine"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_100"><em>*</em>关键字：</span> 
                        <asp:TextBox ID="txtKeys" runat="server" CssClass="forminput"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtKeysTip">（同时作为图文推送的标题来使用。）</p>
                    </li>
                    <li><span class="formitemtitle Pw_100">图文封面：</span> 
                        <div class="uploadimages">
                            <Hi:ImageUploader runat="server" ID="uploader1" />
                        </div>
						图片尺寸建议：320px * 200px
                    </li>
                </ul>
                <ul class="btn Pa_100 clear">
                    <asp:Button ID="btnEditActivity" runat="server" Text="修改" OnClientClick="return PageIsValid();" CssClass="submit_DAqueding" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 100, false, null, '必填 活动名称不能为空，在1至100个字符之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtKeys', 1, 50, false, null, '必填 关键字不能为空，在1至50个字符之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtMaxValue', 1, 10, true, '[0-9]\\d*', '人数上限必须是整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtItem1', 1, 20, false, null, '请至少填写一项'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
