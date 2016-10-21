<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.PO.POItemAdd" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css?v=20150819" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/01.gif" width="32" height="32" /></em>
                <h1>添加采购订单</h1>
            </div>
            <div class="formitem validator4">
                <ul>
                    <li><span class="formitemtitle Pw_198">外箱条形码：</span>
                        <asp:TextBox ID="txtBoxBarCode" runat="server" CssClass="forminput" Width="173" MaxLength="50"></asp:TextBox>
                        <p></p>
                    </li>
                    <li><span class="formitemtitle Pw_198">订单数量：</span>
                        <asp:TextBox ID="txtExpectQuantity" runat="server" CssClass="forminput" Width="173"></asp:TextBox><em>*</em>
                        <p id="<%=txtExpectQuantity.ClientID%>Tip">
                        </p>
                    </li>
                    <%--<li><span class="formitemtitle Pw_198">实际数量：</span>
                        <asp:TextBox ID="txtPracticalQuantity" runat="server" CssClass="forminput" Width="173"></asp:TextBox>
                        <p id="<%=txtPracticalQuantity.ClientID%>Tip">
                        </p>
                    </li>--%>
                    <li><span class="formitemtitle Pw_198">是否样品：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlIsSample" runat="server" Width="173">
                                <asp:ListItem Value="0">否</asp:ListItem>
                                <asp:ListItem Value="1">是</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                        <p></p>
                    </li>

                    <li><span class="formitemtitle Pw_198">生产日期：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarManufactureDate" runat="server" CssClass="forminput" />
                        <p></p>
                    </li>
                    <li><span class="formitemtitle Pw_198">有效日期：</span>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarEffectiveDate" runat="server" CssClass="forminput" />
                        <p></p>
                    </li>
                    <li><span class="formitemtitle Pw_198">生产批号：</span>
                        <asp:TextBox ID="txtBatchNumber" runat="server" CssClass="forminput" Width="173" MaxLength="50"></asp:TextBox>
                        <p></p>
                    </li>

                    <li><span class="formitemtitle Pw_198">商品总净重：</span>
                        <asp:TextBox ID="txtNetWeight" runat="server" CssClass="forminput" Width="173"></asp:TextBox>（kg）
                        <p id="<%=txtNetWeight.ClientID%>Tip">
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">商品总毛重：</span>
                        <asp:TextBox ID="txtRoughWeight" runat="server" CssClass="forminput" Width="173"></asp:TextBox>（kg）
                        <p id="<%=txtRoughWeight.ClientID%>Tip">
                        </p>
                    </li>

                    <li><span class="formitemtitle Pw_198">币别：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ddlCurrency" runat="server" Width="173">
                            </asp:DropDownList>
                        </abbr>
                        <p></p>
                    </li>

                    <li><span class="formitemtitle Pw_198">原币单价：</span>
                        <asp:TextBox ID="txtOriginalCurrencyPrice" runat="server" CssClass="forminput" Width="173" onblur="onblurRate()"></asp:TextBox>
                        <p id="<%=txtOriginalCurrencyPrice.ClientID%>Tip">
                        </p>
                    </li>
                    
                    <li><span class="formitemtitle Pw_198">成本价：</span>
                        <asp:TextBox ID="txtCostPrice" runat="server" CssClass="forminput" Width="173"></asp:TextBox>
                        <p id="<%=txtCostPrice.ClientID%>Tip">
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">销售价：</span>
                        <asp:TextBox ID="txtSalePrice" runat="server" CssClass="forminput" Width="173" onblur="onblurRate()"></asp:TextBox>
                        <p id="<%=txtSalePrice.ClientID%>Tip">
                        </p>
                    </li>
                    <asp:HiddenField ID="hidRate" runat="server" />
                    <li style="display:none;"><span class="formitemtitle Pw_198">汇率：</span>
                        <asp:TextBox ID="txtRate" runat="server" CssClass="forminput" Width="173" ReadOnly="true" BackColor="#c0c0c0"></asp:TextBox>
                        <p id="<%=txtRate.ClientID%>Tip">
                            无需填写，自动根据原币单价/销售价计算
                        </p>
                    </li>

                    <li><span class="formitemtitle Pw_198">装箱规格：</span>
                        <asp:TextBox ID="txtCartonSize" runat="server" CssClass="forminput" Width="173"></asp:TextBox>
                        <p></p>
                    </li>
                    <li><span class="formitemtitle Pw_198">箱子尺寸：</span>
                        <asp:TextBox ID="txtCartonMeasure" runat="server" CssClass="forminput" Width="173" MaxLength="50"></asp:TextBox>
                        <p></p>
                    </li>
                    <li><span class="formitemtitle Pw_198">箱数：</span>
                        <asp:TextBox ID="txtCases" runat="server" CssClass="forminput" Width="173"></asp:TextBox>
                        <p id="<%=txtCases.ClientID%>Tip">
                        </p>
                    </li>
                </ul>
                <ul class="btn Pw_198 clearfix">
                    <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();"
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
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('<%=txtExpectQuantity.ClientID%>', 0, 9, false, '[1-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('<%=txtExpectQuantity.ClientID%>', 0, 9999999, '输入的数值格式错误'));

            initValid(new InputValidator('<%=txtRate.ClientID%>', 1, 10, true, '(0|(0+(\\.[0-9]{1,4}))|[1-9]\\d*(\\.\\d{1,4})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('<%=txtRate.ClientID%>', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('<%=txtRoughWeight.ClientID%>', 1, 10, true, '(0|(0+(\\.[0-9]{1,4}))|[1-9]\\d*(\\.\\d{1,4})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('<%=txtRoughWeight.ClientID%>', 0, 99999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('<%=txtNetWeight.ClientID%>', 1, 10, true, '(0|(0+(\\.[0-9]{1,4}))|[1-9]\\d*(\\.\\d{1,4})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('<%=txtNetWeight.ClientID%>', 0, 99999999, '输入的数值超出了系统表示范围'));

            
            initValid(new InputValidator('<%=txtOriginalCurrencyPrice.ClientID%>', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('<%=txtOriginalCurrencyPrice.ClientID%>', 0, 99999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('<%=txtCostPrice.ClientID%>', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('<%=txtCostPrice.ClientID%>', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('<%=txtSalePrice.ClientID%>', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new MoneyRangeValidator('<%=txtSalePrice.ClientID%>', 0, 99999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('<%=txtCases.ClientID%>', 0, 9, true, '[1-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('<%=txtCases.ClientID%>', 0, 9999999, '输入的数值格式错误'));
            
        }
        $(document).ready(function () {
            InitValidators();
        });

        //计算汇率
        function onblurRate() {
            if ($("#<%=txtOriginalCurrencyPrice.ClientID%>").val().length > 0 && $("#<%=txtSalePrice.ClientID%>").val().length > 0) {
                $("#<%=txtRate.ClientID%>").val(($("#<%=txtOriginalCurrencyPrice.ClientID%>").val() / $("#<%=txtSalePrice.ClientID%>").val()).toFixed(2));
                $("#<%=hidRate.ClientID%>").val(($("#<%=txtOriginalCurrencyPrice.ClientID%>").val() / $("#<%=txtSalePrice.ClientID%>").val()).toFixed(2));
            }
            else {
                $("#<%=txtRate.ClientID%>").val(0);
                $("#<%=hidRate.ClientID%>").val(0);
            }
        }
    </script>
</asp:Content>
