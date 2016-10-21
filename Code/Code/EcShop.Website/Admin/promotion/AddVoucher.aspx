<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddVoucher.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddVoucher" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">

      <div class="columnright">
          <div class="title">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1>添加现金券</h1>
            <span>创建现金券信息</span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100"><em >*</em>现金券名称：</span>
          <asp:TextBox ID="txtVoucherName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtVoucherNameTip">名称不能为空，在1至50个字符之间</p>
          </li>
          <li>
             <span class="formitemtitle Pw_100">&nbsp;<em>*</em>满足金额：</span>
             <asp:TextBox ID="txtAmount" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtAmountTip">满足金额只能是数值，0.01-10000000，且不能超过2位小数</p>
          </li>
          <li><span class="formitemtitle Pw_100"><em >*</em>现金券金额：</span>
            <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtDiscountValueTip">现金券金额只能是数值，0.01-10000000，且不能超过2位小数</p>
          </li>
               <li>
            <span class="formitemtitle Pw_100">&nbsp;<em >*</em>开始日期：</span>
            <UI:WebCalendar ID="calendarStartDate" runat="server" cssclass="forminput" />       
          </li>     
          <li>
            <span class="formitemtitle Pw_100">&nbsp;<em >*</em>结束日期：</span>
            <UI:WebCalendar ID="calendarEndDate" runat="server" cssclass="forminput" />       
          </li>
            <li>
                  <span class="formitemtitle Pw_100">&nbsp;&nbsp;<em >*</em>有效期：</span>
                 <asp:TextBox ID="txtValidity" runat="server" CssClass="forminput"></asp:TextBox><span>&nbsp;天</span>
                 <p id="ctl00_contentHolder_txtValidityTip"> 有效期只能是数字，必须大于等于O</p>
            </li>     
         <%-- <li>
            <span class="formitemtitle Pw_100"><em >*</em>兑换需积分：</span>
            <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
            <p id="P1">兑换所需积分只能是数字，必须大于等于O,0表示不能兑换</p>
	      </li>  --%> 

                    <li>
                        <span class="formitemtitle Pw_100">发放方式：</span>
                        <input type="radio" name="rdoSendType" value="0" onclick="selectMode()" id="rdoManually" runat="server" />手动发送
                <input type="radio" name="rdoSendType" value="1" onclick="selectOverMoney()" id="rdoOverMoney" runat="server" />满金额赠券
                <input type="radio" name="rdoSendType" value="2" onclick="selectMode()" id="rdoRegist" runat="server" />注册赠券
                <input type="radio" name="rdoSendType" value="3" onclick="selectMode()" id="rdoLq" runat="server" />自助领券
                    </li>
                    <li id="hselectovermoney">
                        <span class="formitemtitle Pw_100">请输入订单需满足的金额：</span>
                        <asp:TextBox ID="txtOverMoney" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>

            <asp:Button ID="btnAddVouchers" runat="server" Text="添加" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding"  />
      </ul>
      </div>

      </div>
  </div>







</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

     <script type="text/javascript" language="javascript">
         function InitValidators() {
             initValid(new InputValidator('ctl00_contentHolder_txtVoucherName', 1, 60, false, null, '现金券的名称，在1至50个字符之间'));
             initValid(new InputValidator('ctl00_contentHolder_txtAmount', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '满足金额只能是数值，0.01-10000000，且不能超过2位小数'));
             appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAmount', 0.01, 10000000.00, '满足金额只能是数值，0.01-10000000，且不能超过2位小数'));
             initValid(new InputValidator('ctl00_contentHolder_txtDiscountValue', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '现金券金额只能是数值，0.01-10000000，且不能超过2位小数'));
             appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtDiscountValue', 0.01, 10000000.00, '现金券金额只能是数值，0.01-10000000，且不能超过2位小数'));
             initValid(new InputValidator('ctl00_contentHolder_txtCount', 0, 10, false, '-?[0-9]\\d*', '导出数量只能是数字，必须大于等O,0表示不导出'));
             appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCount', 0, 1000, '导出数量只能是数字，必须大于等O,小于1000，0表示不导出'));
             
         }

         function selectMode() {
             $("#hselectovermoney").css("display", "none");
         }

         function selectOverMoney() {
             $("#hselectovermoney").css("display", "");
         }
         $(document).ready(function () {
             if ($('#ctl00_contentHolder_rdoOverMoney').attr("checked") == "checked") {
                 selectOverMoney();
             }

             else {
                 selectMode();
             }
             InitValidators();
         });
</script>

</asp:Content>
