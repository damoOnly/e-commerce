﻿<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SupplierAddOrderPromotion.aspx.cs" Inherits="EcShop.UI.Web.Admin.SupplierAddOrderPromotion" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="cc1" TagName="PromotionView" Src="~/Admin/promotion/Ascx/PromotionView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_promotionView_txtPromoteSalesName', 1, 60, false, null, '促销活动的名称，在1至60个字符之间'));
        }

        $(document).ready(function () {
            InitValidators();
            SelectPromoteType();
            ShowPromotion(false);
            $("input[type='radio'][name='radPromoteType']").bind("click", function () { ShowPromotion(true); });
        });

        function SelectPromoteType() {
            var promoteType = $("#ctl00_contentHolder_txtPromoteType").val();
            if (promoteType == 11)
                $("#radPromoteType_FullAmountDiscount").attr("checked", true);
            else if (promoteType == 12)
                $("#radPromoteType_FullAmountReduced").attr("checked", true);
            else if (promoteType == 13)
                $("#radPromoteType_FullQuantityDiscount").attr("checked", true);
            else if (promoteType == 14)
                $("#radPromoteType_FullQuantityReduced").attr("checked", true);
            else if (promoteType == 15)
                $("#radPromoteType_FullAmountSentGift").attr("checked", true);
            else if (promoteType == 16)
                $("#radPromoteType_FullAmountSentTimesPoint").attr("checked", true);
            else if (promoteType == 17)
                $("#radPromoteType_FullAmountSentFreight").attr("checked", true);
        }

        function ShowPromotion(isClick) {
            $("#liPromoteTypeDiscount").hide();
            if (isClick) {
                $("#ctl00_contentHolder_txtCondition").val("");
                $("#ctl00_contentHolder_txtDiscountValue").val("");
            }

            if ($("#radPromoteType_FullAmountDiscount").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足金额(元)：")
                $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
            }
            else if ($("#radPromoteType_FullAmountReduced").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足金额(元)：")
                $("#lblDiscountValueTip").html("立减金额(元)：");
            }
            else if ($("#radPromoteType_FullQuantityDiscount").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足数量(件)：")
                $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
            }
            else if ($("#radPromoteType_FullQuantityReduced").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足数量(件)：")
                $("#lblDiscountValueTip").html("立减金额(元)：");
            }
            else if ($("#radPromoteType_FullAmountSentGift").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#spCondition").show();
                $("#spDiscountValue").hide();
                $("#lblConditionTip").html("满足金额(元)：");
            }
            else if ($("#radPromoteType_FullAmountSentTimesPoint").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足金额(元)：");
                $("#lblDiscountValueTip").html("倍数：");
            }
            else if ($("#radPromoteType_FullAmountSentFreight").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#spCondition").show();
                $("#spDiscountValue").hide();
                $("#lblConditionTip").html("满足金额(元)：");
            }
        }

        function Valid() {
            var promoteType = $("input[type='radio'][name='radPromoteType']:checked").val();
            var condition = $("#ctl00_contentHolder_txtCondition").val();
            var discountValue = $("#ctl00_contentHolder_txtDiscountValue").val();
            var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");
            var numexp = new RegExp("^[0-9]\\d*$", "i");

            if (promoteType == undefined) {
                alert("请选择促销活动类型！")
                return false;
            }

            $("#ctl00_contentHolder_txtPromoteType").val(promoteType);

            if (promoteType == 11) {
                if (condition.length == 0) {
                    alert("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
                if (discountValue.length == 0) {
                    alert("请输入折扣值(一般在0.01-1之间)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                    return false;
                }
                var num = parseFloat(discountValue);
                if (num < 0.01 || num > 1) {
                    alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                    return false;
                }
            }
            else if (promoteType == 12) {
                if (condition.length == 0) {
                    alert("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
                if (discountValue.length == 0) {
                    alert("请输入立减金额(元)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("输入立减金额(元)有误(必须是数值)，请重新输入正确的立减金额！");
                    return false;
                }
            }
            else if (promoteType == 13) {
                if (condition.length == 0) {
                    alert("请输入满足数量！");
                    return false;
                }
                if (!numexp.test(condition)) {
                    alert("输入满足数量有误(必须是正数)，请重新输入正确的满足数量！");
                    return false;
                }
                if (discountValue.length == 0) {
                    alert("请输入折扣值(一般在0.01-1之间)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                    return false;
                }
                var num = parseFloat(discountValue);
                if (num < 0.01 || num > 1) {
                    alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                    return false;
                }
            }
            else if (promoteType == 14) {
                if (condition.length == 0) {
                    alert("请输入满足数量！");
                    return false;
                }
                if (!numexp.test(condition)) {
                    alert("输入满足数量有误(必须是正数)，请重新输入正确的满足数量！");
                    return false;
                }

                if (discountValue.length == 0) {
                    alert("请输入立减金额！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("立减金额有误(必须是数值)，请重新输入正确的立减金额！");
                    return false;
                }
            }
            else if (promoteType == 15) {
                if (condition.length == 0) {
                    alert("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
            }
            else if (promoteType == 16) {
                if (condition.length == 0) {
                    alert("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
                if (discountValue.length == 0 || isNaN(parseFloat(discountValue)) || parseFloat(discountValue) <= 1) {
                    alert("请输入倍数,倍数必须为数值且大于1！");
                    return false;
                }
                //if (!numexp.test(condition) || isNaN(parseFloat(condition)) || parseFloat(condition) <= 1) {
                //    alert("输入的倍数有误(必须是数值，且数值大于1)，请重新输入倍数！");
                //    return false;
                //}
            }
            else if (promoteType == 17) {
                if (condition.length == 0) {
                    alert("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    alert("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
            }

            if (!PageIsValid())
                return false;
            if ($("#ctl00_contentHolder_promotionView_calendarStartDate").val() == "") {
                alert("请选择促销活动开始时间！")
                return false;
            }
            if ($("#ctl00_contentHolder_promotionView_calendarEndDate").val() == "") {
                alert("请选择促销活动结束时间！")
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix validator4">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/07.gif" width="32" height="32" /></em>
                <h1 id="h1" runat="server">添加订单促销活动</h1>
                <span id="span1" runat="server">添加订单促销活动</span>
            </div>
            <div class="datafrom">
                <div class="formitem validator4">
                    <ul>
                        <li><span class="formitemtitle Pw_110"><em>*</em>促销活动类型：</span>
                            <Hi:PromoteTypeRadioButtonList IsProductPromote="false" runat="server" ID="radPromoteType" />
                            <Hi:TrimTextBox runat="server" ID="txtPromoteType" Style="display: none;"></Hi:TrimTextBox>
                        </li>
                        <li id="liPromoteTypeDiscount" style="display: none;">
                            <table>
                                <tr>

                                    <td>
                                        <div id="spCondition">
                                            <span id="lblConditionTip" class="formitemtitle Pw_110"></span>
                                            <asp:TextBox ID="txtCondition" runat="server" CssClass="forminput"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="spDiscountValue">
                                            <span id="lblDiscountValueTip" style="clear: none; width: 230px;" class="formitemtitle Pw_110"></span>
                                            <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>




                        </li>
                        <cc1:PromotionView runat="server" ID="promotionView" />
                    </ul>
                    <ul class="btntf Pa_110 clear" style="margin-left: 8px;">
                        <asp:Button ID="btnAdd" runat="server" Text="添加" OnClientClick="return Valid();" CssClass="submit_DAqueding" />
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
