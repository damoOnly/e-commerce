<%@ Page Language="C#" AutoEventWireup="true" Inherits="EcShop.UI.Web.Admin.AddHSCode" MasterPageFile="~/Admin/Admin.Master" MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
	        <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	            <h1>添加新的海关编码</h1>
            </div>
        
        <div class="formitem validator4">
            <%--<ul>--%>
                <table width="100%" runat="server">
                    <tr>
                        <td width="50%">
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>海关编码：</span>
                                    <asp:TextBox ID="txtHSCode" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtHSCodeTip" runat="server" style="width: 270px;">海关编码长度限制在1-10个字符之间</p>
                                </li>
                             </ul>
                        </td>
                        <td width="50%">
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>海关编码名称：</span>
                                    <asp:TextBox ID="txtHSCodeName" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtHSCodeNameTip" runat="server" style="width: 270px;">海关编码名称长度限制在1-128个字符之间</p>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>最惠国税率：</span>
                                    <asp:TextBox ID="txtLowRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtLowRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>普通国税率：</span>
                                    <asp:TextBox ID="txtHighRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtHighRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>出口税率：</span>
                                    <asp:TextBox ID="txtOutRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtOutRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>增值税率：</span>
                                    <asp:TextBox ID="txtTaxRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtTaxRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>退税率：</span>
                                    <asp:TextBox ID="txtTslRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtTslRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110">单位1：</span>
                                    <span class="formselect">
                                        <Hi:UnitDropDownList runat="server" CssClass="productType" ID="dropUnit1" Width="153px" />
                                    </span>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110">单位2：</span>
                                    <span class="formselect">
                                        <Hi:UnitDropDownList runat="server" CssClass="productType" ID="dropUnit2" Width="153px" />
                                    </span>
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110">海关监管条件：</span>
                                    <asp:TextBox ID="txtControlMa" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtControlMaTip" runat="server" style="width: 270px;">长度限制在1-30个字符之间</p>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>临时进口税率：</span>
                                    <asp:TextBox ID="txtTempInRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtTempInRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>临时出口税率：</span>
                                    <asp:TextBox ID="txtTempOutRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtTempOutRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110">海关说明：</span>
                                    <asp:TextBox ID="txtNote" TextMode="MultiLine" Width="200" Height="40" runat="server" ></asp:TextBox>
                                    <p id="txtNoteTip" runat="server" style="width: 270px;">备注的长度限制在0-100个字符之间</p>
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110">商检监管条件：</span>
                                    <asp:TextBox ID="txtControlInspection" TextMode="MultiLine" Width="200" Height="40" runat="server" ></asp:TextBox>
                                    <p id="txtControlInspectionTip" runat="server" style="width: 270px;">长度限制在1-30个字符之间</p>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>
                                    <span class="formitemtitle Pw_110"><em >*</em>消费税率：</span>
                                    <asp:TextBox ID="txtConsumptionRate" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                    <p id="txtConsumptionRateTip" runat="server" style="width: 270px;">税率长度限制为18个字符以内，且必须为正小数</p>
                                </li>
                            </ul>
                        </td>
                        <%--<td>
                            <li>
                                <span class="formitemtitle Pw_110"><em >*</em>香港海关编码：</span>
                                <asp:TextBox ID="TextBox15" CssClass="forminput" runat="server" Width="200"></asp:TextBox>
                                <p id="P15" runat="server">长度限制在1-30个字符之间</p>
                            </li>
                        </td>--%>

                    </tr>
                </table>

   <%--         </ul>--%>
<%--            <div class="search clearfix">
                <ul>
				    <li><span>申报要素：</span>
				        <span><asp:TextBox ID="codeSearchText" runat="server" CssClass="forminput" /></span>
				        <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				    </li>
			    </ul>
            </div>--%>
            <table>
                <tr>
                    <td class="auto-style1" rowspan="5" align="center">
                         <div class="search clearfix">
                            <ul>
                                <li><span>申报要素：</span>
				                    <span><asp:TextBox ID="elementsSearchText" runat="server" CssClass="forminput" /></span>
				                    <asp:Button ID="btnElementsSearchButton" runat="server" Text="查询" class="searchbutton"/>
                                    <asp:HiddenField ID="elmentsJson" runat="server" /> 
		                        </li>
			                </ul>
                          </div>
                    </td>
                </tr>
                
            </table>
            <table>
                 <tr>
                    <td>
                      <Hi:ElmentsListBox ID="elmentsListBox" SelectionMode="Multiple" Width="350px" Height="300px" runat="server"></Hi:ElmentsListBox>
                      <%--<select id="elmentsSource" multiple="multiple" style="width:350px;height:300px"></select>--%>
                    </td>
                    <td colspan="2" style="width:65px;vertical-align:top;" align="center">
                        <table style="height:300px;width:65px;border:1px;">
                            <tr>
                                <td >
                                    <a href="javascript:void(0)" onclick="selElments();">添加>></a><br/>
                                    <a href="javascript:void(0)" onclick="removeElments();">删除<<</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top;height:300px;width:400px;">
                    <div style="height:300px;overflow-y:auto;border:solid #808080 1px;">
                    <table id="elmentsList">
                        <thead>
                            <tr >
                                <td style="border:solid #808080 1px;">
                                    <input type="checkbox" id="all" name="all" onclick="selall();"/>
                                </td>
                                <td width="170px" style="border:solid #808080 1px;">申报要素
                                </td>
                                <td width="230px" style="border:solid #808080 1px;">要素内容  
                                </td>
                            </tr>
                        </thead>
                        <tbody >
                        </tbody>
                    </table>
                    </div>
                    <!--  <select  multiple="multiple" ></select> -->      
                    </td>  
                </tr>
            </table>

              <ul class="btntf Pa_100 clear">
                <asp:Button ID="btnSave" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" runat="server"/>

              </ul>
        </div>
    </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
         <script type="text/javascript" language="javascript">
             function InitValidators() {
                 initValid(new InputValidator('ctl00_contentHolder_txtHSCode', 1, 10, false, null, '海关编码长度限制在1-10个字符之间'));
                 initValid(new InputValidator('ctl00_contentHolder_txtHSCodeName', 1, 128, false, null, '海关编码名称长度限制在1-128个字符之间'));
                 initValid(new InputValidator('ctl00_contentHolder_txtControlMa', 1, 30, true, null, '长度限制在1-30个字符之间'));
                 initValid(new InputValidator('ctl00_contentHolder_txtControlInspection', 1, 30, true, null, '长度限制在1-30个字符之间'));

                 initValid(new InputValidator('ctl00_contentHolder_txtLowRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
                 initValid(new InputValidator('ctl00_contentHolder_txtHighRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
                 initValid(new InputValidator('ctl00_contentHolder_txtOutRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
                 initValid(new InputValidator('ctl00_contentHolder_txtTaxRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
                 initValid(new InputValidator('ctl00_contentHolder_txtTslRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
                 initValid(new InputValidator('ctl00_contentHolder_txtTempInRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
                 initValid(new InputValidator('ctl00_contentHolder_txtTempOutRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
                 initValid(new InputValidator('ctl00_contentHolder_txtConsumptionRate', 1, 18, false, '^[0]+([.]{1}[0-9]+){0,1}$', '税率长度限制为18个字符以内，且必须为正小数'));
             }
             $(document).ready(function () {
                 InitValidators();
                 var data = $("#<%=elmentsJson.ClientID%>").val();
                 data = $.parseJSON(data);

                 if (data != null && data.success != "NO") {

                     $.each(data, function (index, item) {

                         var s = [];

                         s.push("<tr>");
                         s.push("<td id=\"test\" style=\"border:solid #808080 1px;\"><input type='hidden' name='elmentsID' value=\"" + item.HS_ELMENTS_ID + "\" /><input type=\"checkbox\"  value=\"" + item.HS_ELMENTS_ID + "\"></td>");
                         s.push("<input type='hidden' name=\"elmentsName_" + item.HS_ELMENTS_ID + "\" value=\"" + item.HS_ELMENTS_NAME + "\" /><td title=\"" + item.HS_ELMENTS_NAME + "\" width=\"170px\" style=\"border:solid #808080 1px;\" >" + show_string(item.HS_ELMENTS_NAME) + "</td>");
                         s.push("<td><input type=\"text\" width=\"230px\" style=\"border:solid #808080 1px;\" id=\"in_" + index + "\" name=\"elmentsDesc_" + item.HS_ELMENTS_ID + "\"  keyid=\"" + item.HS_ELMENTS_ID + "\" value=\"" + item.HS_ELMENTS_DESC + "\"></td>");
                         s.push("</tr>");

                         $("#elmentsList").append(s.join(""));
                     });
                 }
                 else {
                     alert("加载申报要素失败！" + data.MSG);
                 }
             });
             //添加选中的
             var selElments = function () {

                 if ($("#ctl00_contentHolder_elmentsListBox").find("option:selected").length < 1) {
                     alert("请至少选择一个");
                     return;
                 }

                 $.each($("#ctl00_contentHolder_elmentsListBox").find("option:selected"), function () {
                     var s = [];
                     var obj = this;
                     var b = true;
                     var cb = $("#elmentsList").find(":checkbox");
                     for (var i = 0; i < cb.length; i++) {
                         if (cb.eq(i).attr("value") == $(obj).val()) {
                             b = false;
                             break;
                         }
                     }
                     if (b) {
                         s.push("<tr>");
                         s.push("<td id=\"test\" style=\"border:solid #808080 1px;\"><input type='hidden' name='elmentsID' value=\"" + $(obj).val() + "\" /><input type=\"checkbox\"  value=\"" + $(obj).val() + "\"></td>");
                         s.push("<input type='hidden' name=\"elmentsName_" + $(obj).val() + "\" value=\"" + $(obj).text() + "\" /><td title=\"" + $(obj).text() + "\" width=\"170px\" style=\"border:solid #808080 1px;\" >" + show_string($(obj).text()) + "</td>");
                         s.push("<td><input type=\"text\" width=\"230px\" style=\"border:solid #808080 1px;\" id=\"in_" + i + "\" name=\"elmentsDesc_" + $(obj).val() + "\"  keyid=\"" + $(obj).val() + "\"></td>");
                         s.push("</tr>");
                     }
                     $("#elmentsList").append(s.join(""));
                 });
             }
             //移除选中的
             var removeElments = function () {
                 if ($("#elmentsList").find(":checkbox:checked").length < 1) {
                     alert("请至少选择一个");
                     return;
                 }
                 $("#elmentsList").find(":checkbox:checked").parent("td").parent("tr").remove();
             }

             var selall = function () {
                 if (document.getElementById("all").checked) {
                     $.each($("#elmentsList").find(":checkbox"), function () {
                         $(this).attr("checked", true);
                     })
                 } else {
                     $.each($("#elmentsList").find(":checkbox"), function () {
                         $(this).removeAttr("checked");
                     })
                 }
             }
             var show_string = function (val) {
                 if (val.length <= 10)
                     return val;
                 else
                     return val.substring(0, 10);
             }
       </script>

    
</asp:Content>