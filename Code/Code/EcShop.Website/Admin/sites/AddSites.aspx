<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddSites.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddSites" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
        <script type="text/javascript" language="javascript">

            function PageIsValid()
            {
                var Sitename = $.trim($("#ctl00_contentHolder_txtSitesName").val());
                if (Sitename == "")
                {
                    alert("站点名称不能为空");
                    return false;
                }
                var SitenCode = $.trim($("#ctl00_contentHolder_txtsitCode").val());
                if (SitenCode == "") {
                    alert("站点编码不能为空");
                    return false;
                }
                return true;
        }

    </script>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title clearfix">
                <em><img src="../images/05.gif" width="32" height="32" /></em>
                <h1>添加站点</h1>
            </div>
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle Pw_198"><em>*</em>站点名称：</span>
                        <asp:TextBox ID="txtSitesName" MaxLength="30" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtSitesNameTip">站点名称不能为空，长度限制在30个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198"><em>*</em>站点编码：</span>
                        <asp:TextBox ID="txtsitCode" MaxLength="10" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtsitCodeTip">站点名称不能为空，长度限制在10个字符以内</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">地区：</span>
                        <abbr class="formselect">
                           <Hi:RegionSelectoNoCountys   runat="server" ID="ddlRegions"   />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198"><em>*</em>默认站点：</span>
                        <asp:RadioButtonList ID="Rb_IsDefault" runat="server" RepeatDirection="Horizontal" >
                            <asp:ListItem Text="否" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="是" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </li>
                    <li><span class="formitemtitle Pw_198">排序：</span>
                        <asp:TextBox ID="txtSort" MaxLength="15" CssClass="forminput" runat="server" />
                        <p id="ctl00_contentHolder_txtSortTip">只能输入数字</p>
                    </li>
              
                    <li><span class="formitemtitle Pw_198">备注：</span>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"  CssClass="forminput" Width="450" Height="120" />
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

