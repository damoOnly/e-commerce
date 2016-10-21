<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EcShop.UI.Web.Admin.Login" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Import Namespace="EcShop.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<Hi:HeadContainer ID="HeadContainer1" runat="server" />
<Hi:PageTitle ID="PageTitle1" runat="server" />
<link href="css/login.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../Utility/jquery-1.6.4.min.js"></script>
</head>
<body class="lg-body">
<div class="top-bg"></div>
<div class="layout fix">
    <div class="logo-wrap fix"><img class="logo-img" src="images/haimei_logo.png" /></div>
    <h1 class="title">商城管理后台</h1>
    <div class="lg-section fix">
        <div class="s-left">
            <form id="form1" runat="server">
                <ul class="lg-ul">
                    <li class="fix">
                        <label>用户名：</label>
                        <div class="input-con"><img src="images/user.png">
                            <asp:TextBox ID="txtAdminName" CssClass="inputbox" runat="server"></asp:TextBox>
                        </div>
                    </li>
                    <li class="fix">
                        <label>密码：</label>
                        <div class="input-con"><img src="images/lock.png">
                            <asp:TextBox ID="txtAdminPassWord" CssClass="inputbox" runat="server" TextMode="Password" />
                        </div>
                    </li>
                    <li runat="server" id="imgCode" visible="false">
                        <label>验证码：</label>
                        <div class="input-con">
                            <asp:TextBox ID="txtCode" runat="server" class="inputbox codeinput" MaxLength="4"></asp:TextBox>
                            <a class="codeimg"  href="javascript:void(0);"><img id="img_txtCode" src="" alt="" class="check-img" style="margin-top:10px;" /><img id="imgVerifyCode" src="../VerifyCodeImage.aspx" alt="验证码" class="codeimg-i" style="margin-bottom:-8px;" onclick="refreshCode();" title="看不清，下一个"/></a> </div>
                    </li>
                    <span class="lg-tip">
                    <asp:Literal  ID="lblStatus" runat="server" Visible="false" ></asp:Literal>
                    </span>
                    <li class="fix lastInput">
                        <asp:HiddenField ID="ErrorTimes" runat="server" Value="0" />
                        <asp:Button ID="btnAdminLogin" runat="server" Text="登录" CssClass="submit-btn" />
                    </li>
                </ul>
            </form>
        </div>
    </div>
    <div class="lg-copyright">@2012-2015 版权所有 海美生活</div>
</div>
<script language="javascript" type="text/javascript">
        function refreshCode() {
            var img = document.getElementById("imgVerifyCode");
            if (img != null) {
                var currentDate = new Date();
                img.src = '<%= Globals.ApplicationPath + "/VerifyCodeImage.aspx?t=" %>' + currentDate.getTime();
            }
        }
        $(document).ready(function () {
            $("#img_txtCode").hide();
            $("#txtCode").keyup(function () {
                var value = $(this).val();
                if (value.length < 4) {
                    $("#img_txtCode").hide();
                    temp = "";
                }
                else if (value.length == 4) {
                    if (temp != value) {
                        $("#img_txtCode").show();
                        $.ajax({
                            url: "Login.aspx",
                            type: 'post', dataType: 'json', timeout: 10000,
                            data: {
                                isCallback: "true",
                                code: $("#txtCode").val()
                            },
                            async: false,
                            success: function (resultData) {
                                var flag = resultData.flag;
                                if (flag == "1") {
                                    $("#img_txtCode").attr("src", "images/true.gif");
                                }
                                else if (flag == "0") {
                                    $("#img_txtCode").attr("src", "images/false.gif");
                                }
                            }
                        });
                    }
                    temp = value;
                }
            });
        });
    </script>
</body>
</html>
