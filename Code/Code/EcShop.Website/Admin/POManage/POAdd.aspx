<%@ Page Language="C#"  MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.PO.POAdd" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="/admin/css/pro-list.css" type="text/css" />
    <link rel="stylesheet" href="/admin/css/autocompleter.main.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/01.gif" width="32" height="32" /></em>
                <h1>添加采购订单</h1>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_198">PO单号：</span>
                        <asp:TextBox ID="txtPONumber" runat="server" CssClass="forminput" ReadOnly="true" Text="系统自动生成" Width="175"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_198">单据类型：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlPOType" runat="server" Width="175">
                                <asp:ListItem Value="1">采购订单</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">供应商：</span>
                        <abbr class="formselect">
                            <Hi:SupplierDropDownList runat="server" ID="ddlSupplier"  /><em>*</em>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">预计到货时间：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarExpectedTime" runat="server" CssClass="forminput" />
                    </li>
                    <li><span class="formitemtitle Pw_198">发船日期：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="DepartTime" runat="server" CssClass="forminput" />
                    </li>
                    <li><span class="formitemtitle Pw_198">到港日期：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="ArrivalTime" runat="server" CssClass="forminput" />
                    </li>
                    <li><span class="formitemtitle Pw_198">柜号：</span>
                        <asp:TextBox ID="ContainerNumber" runat="server" CssClass="forminput" Text=""></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_198">提单号：</span>
                        <asp:TextBox ID="BillNo" runat="server" CssClass="forminput" Text="" ></asp:TextBox>
                    </li>
                    <li class="clearfix" style="overflow:inherit;height:27px;"><span class="formitemtitle Pw_198">启运港（国家）：</span>
                        <span class="field" style="width:350px">
                            <input runat="server" type="text" name="txtCountry" id="txtCountry" placeholder="请输入国家" maxlength="20" style="width:175px" />
                            <a href="javascript:void(0)" onclick="clearAutoCompleterCache()">清除缓存</a>
                            <asp:HiddenField runat="server" ID="hidCountryId" />
                            <asp:HiddenField runat="server" ID="hidCountry" />
                        </span>
                    </li>
                    <li><span class="formitemtitle Pw_198">目的港：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlEndPort" runat="server" Width="175">
                                <%--<asp:ListItem Value="1">深圳前海湾保税港区</asp:ListItem>--%>
                            </asp:DropDownList>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">备注：&nbsp;</span>
                        <asp:TextBox ID="txtRemark" runat="server" CssClass="forminput" MaxLength="200" TextMode="MultiLine" Width="318" Height="120"></asp:TextBox>
                    </li>
                </ul>
                <ul class="btn Pw_198 clearfix">
                    <asp:Button ID="btnSave" runat="server"
                         Text="保 存" CssClass="submit_jixu inbnt m_none" />
                </ul>
            </div>

        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="/Admin/js/jquery.min.js"></script>
    <script src="/Admin/js/jquery.autocompleter.js"></script>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
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