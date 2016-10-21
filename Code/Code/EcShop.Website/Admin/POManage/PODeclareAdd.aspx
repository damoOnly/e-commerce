<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.PO.PODeclareAdd" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/01.gif" width="32" height="32" /></em>
                <h1>申报入库信息</h1>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_198">PO单号：</span>
                        <asp:TextBox ID="txtPONumber" runat="server" CssClass="forminput" ReadOnly="true" Width="125" ></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_198">运输工具名称：</span>
                        <asp:TextBox ID="transname" runat="server" CssClass="forminput" Width="125" ></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_198">航班号：</span>
                        <asp:TextBox ID="voyage" runat="server" CssClass="forminput"  Width="125" ></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_198">提运单号：</span>
                        <asp:TextBox ID="cabinno" runat="server" CssClass="forminput"  Width="125" ></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_198">法定数量：</span>
                        <asp:TextBox ID="FriNum" runat="server" CssClass="forminput"  Width="125" ></asp:TextBox>
                    </li>

                    <li><span class="formitemtitle Pw_198">集装箱规格：</span><span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ContainerNumberType" runat="server" Width="125">
                                <asp:ListItem Value="-1">--请选择规格--</asp:ListItem>
                                <asp:ListItem Value="0">M</asp:ListItem>
                                <asp:ListItem Value="1">L</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span>
                    </li>
                    <li><span class="formitemtitle Pw_198">征免方式：</span>
                        <abbr class="formselect">
                            <Hi:GetTaxDropDownList runat="server" ID="GetTax"  /><em>*</em>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">用途：</span>
                        <abbr class="formselect">
                            <Hi:HSUserTypeDropDownList runat="server" ID="UserType"  /><em>*</em>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">运输方式：</span>
                        <abbr class="formselect">
                            <Hi:TransportTypeDropDownList runat="server" ID="TransportType"  /><em>*</em>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">业务类型：</span>
                        <abbr class="formselect">
                            <Hi:BusinessTypeDropDownList runat="server" ID="BusinessType"  /><em>*</em>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">包装种类：</span>
                        <abbr class="formselect">
                            <Hi:WrapTypeDropDownList runat="server" ID="WrapType"  /><em>*</em>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">成交方式：&nbsp;</span>
                        <abbr class="formselect">
                            <Hi:HSTradeTypeDropDownList runat="server" ID="TradeType"  /><em>*</em>
                        </abbr>                        
                    </li>
                    <li><span class="formitemtitle Pw_198">进口口岸：&nbsp;</span>
                        <abbr class="formselect">
                            <Hi:ApplyortDrowpDownList runat="server" ID="Applyort"  /><em>*</em>
                        </abbr>                        
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