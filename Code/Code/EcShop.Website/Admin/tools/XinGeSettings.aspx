<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="XinGeSettings.aspx.cs" Inherits="EcShop.UI.Web.Admin.XinGeSettings" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .Pw_140 {
  width: 150px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>信鸽设置
            </h1>
            <span>配置您所获取信鸽平台的信息，系统将使用这些信息自动为客户发送信鸽消息。
            </span>
            <asp:Label ID="lbNum" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul id="pluginContainer" class="attributeContent2">
                    <li style="margin-bottom: 10px;"><span class="formitemtitle Pw_140">发送方式：</span>
                        <select id="ddlSms" name="ddlSms"></select>
                    </li>
                    <li rowtype="attributeTemplate" style="display: none; margin-bottom: 10px;"><span class="formitemtitle Pw_140">$Name$：</span>
                        $Input$
                    </li>
                </ul>
                <ul class="btntf Pa_140">
                    <asp:Button ID="btnSaveXinGeSettings" runat="server" OnClientClick="return Save();" Text="保 存" CssClass="submit_DAqueding float" />
                </ul>
            </div>
            <div class="formitem">
                <ul>
                    <li style="margin-bottom: 10px;"><span class="formitemtitle Pw_140">会员账号：</span>
                        <asp:TextBox ID="txtTestAccoun" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_140">发送内容：</span>
                        <asp:TextBox ID="txtTestSubject" runat="server" CssClass="forminput"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle Pw_140">设备类型：</span>
                        <asp:RadioButton GroupName="phone" ID="android"  runat="server" Checked="true" Text="Android"/>
                        <asp:RadioButton GroupName="phone" ID="iphone"  runat="server" Text="iOS" /><asp:CheckBox ID="cbxEnv" runat="server" Text="发送到生产环境(iOS)" />
                    </li>
                    <li class="clear"></li>
                </ul>
                <ul class="btntf Pa_140">
                    <asp:Button ID="btnTestSend" runat="server" OnClientClick="return TestCheck();" Text="测试发送" CssClass="submit_DAqueding inbnt"></asp:Button>
                </ul>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <asp:HiddenField runat="server" ID="txtSelectedName" />
    <asp:HiddenField runat="server" ID="txtConfigData" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/plugin.js" />
    <script type="text/javascript">
        $(document).ready(function () {
            pluginContainer = $("#pluginContainer");
            templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
            dropPlugins = $("#ddlSms");
            selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
          configDataCtl = $("#<%=txtConfigData.ClientID %>");

          // 绑定短信类型列表
          $(dropPlugins).append($("<option value=\"\">-请选择发送方式-</option>"));
          $.ajax({
              url: "PluginHandler.aspx?type=xingesend&action=getlist",
              type: 'GET',
              async: false,
              dataType: 'json',
              timeout: 10000,
              success: function (resultData) {
                  if (resultData.qty == 0)
                      return;

                  $.each(resultData.items, function (i, item) {
                      //if (item.FullName == "ecdev.plugins.sms.haidao") {
                      //if (item.FullName == $(selectedNameCtl).val())
                      $(dropPlugins).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.FullName, item.DisplayName)));
                      //else
                      //    $(dropPlugins).append($(String.format("<option value=\"{0}\">{1}</option>", item.FullName, item.DisplayName)));
                      //  }
                  });
              },
              error: function (XMLHttpRequest, textStatus, errorThrown) {
                  alert(XMLHttpRequest.status);
                  alert(XMLHttpRequest.readyState);
                  alert(textStatus);
              }
          });

          $(dropPlugins).bind("change", function () { SelectPlugin("XingePush"); });
          //$(dropPlugins).attr("disabled", "disabled");

          if ($(selectedNameCtl).val().length > 0) {
              SelectPlugin("XinGeSend");
          }
      });

      function TestCheck() {
          if ($(dropPlugins).val() == "") {
              alert("请先选择发送方式并填写配置信息");
              return false;
          }
          if ($("#Appkey") != [] && $("#Appkey").val().length == 0) {
              alert("Appkey必须填");
              return false;
          }
          if ($("#Appsecret").val().length == 0) {
              alert("Appsecret必须填");
              return false;
          }
          if ($("#ctl00_contentHolder_txtTestCellPhone").val().length == 0) {
              alert("请输入接收手机号码");
              return false;
          }
          if ($("#ctl00_contentHolder_txtTestSubject").val().length == 0) {
              alert("请输入发送内容");
              return false;
          }
          $(dropPlugins).removeAttr("disabled");
          return true;
      }

      function Save() {
          if ($("#Appkey").val().length == 0) {
              alert("Appkey必须填");
              return false;
          }
          if ($("#Appsecret").val().length == 0) {
              alert("Appsecret必须填");
              return false;
          }
          $(dropPlugins).removeAttr("disabled");
          return true;
      }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
