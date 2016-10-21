function GetSMSCode() {
    var token = document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value");
    var phonenumber =$.trim($("#txtPhoneNumber").val());
    var phonenumberReg = /^0?(13|15|18|14|17)[0-9]{9}$/;
    if (!phonenumber || phonenumber == "")
    {
        alert_h('手机号码不能为空');
        return;
    }
    if (!phonenumberReg.test(phonenumber)) {
        alert_h('手机号码格式不正确');
        return;
    }
    //GetTelCodeTime();

    $.ajax({
        type: "POST",
        url: "/Handler/MemberHandler.ashx?action=SendRegisterTel",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        data: { token: token, cellphone: phonenumber },
        success: function (result) {
            if (result.success) {

                GetTelCodeTime();
                //document.getElementById("btnGetTelCode")
            }
            else {
                alert_h(result.msg);
            }
        },
        error: function (a, b, c) {
            alert("error:" + a + ":" + b + ":" + c);
        }
    });
}


var wait = 60;

function GetTelCodeTime() {
    if (wait == 0) {
        $("#btnGetSMSCode").attr("disabled", false);
        $("#btnGetSMSCode").val("获取验证码");
        wait = 60;
    } else {
        wait--;
        $("#btnGetSMSCode").attr("disabled", true);
        $("#btnGetSMSCode").val("重新发送(" + wait + ")");
        setTimeout(GetTelCodeTime, 1000);
    }
}


function BindPhoneNumber()
{
    var phonenumber = $.trim($("#txtPhoneNumber").val());

    var smscode = $.trim($("#txtsmscode").val());

    var password = $.trim($("#txtPassword").val());

    var phonenumberReg = /^0?(13|15|18|14|17)[0-9]{9}$/;

    var invitecode = $.trim($("#txtinvitecode").val());
    if (!phonenumber || phonenumber == "") {
        alert_h('手机号码不能为空');
        return;
    }
    if (!phonenumberReg.test(phonenumber)) {
        alert_h('手机号码格式不正确');
        return;
    }

    if (!smscode || smscode == "") {
        alert_h('验证码不能为空');
        return;
    }

    if (!password || password.length < 6) {
        alert_h('密码不能为空并且至少要6个字符');
        return;
    }

    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "BindPhoneNumber", phonenumber: phonenumber, smscode: smscode, password: password, invitecode: invitecode },
        success: function (resultData) {
            if (resultData.success == true) {
                alert_h(resultData.msg);
                window.setTimeout(function () {
                        location.href = "MemberCenter.aspx";
                }, 1000);
            }
            else 
            {
                alert_h(resultData.msg);
            }
        },
        error: function (a, b, c) {
            alert("error:" + a + ":" + b + ":" + c);
        }
    });
}

$(function () {

    //获取手机验证码
    $("#btnGetSMSCode").on("click", function () {
        GetSMSCode();
    })
    //绑定手机号码
    $("#btnBindPhoneNumber").on("click", function () {
        BindPhoneNumber();
    });


    $("#divBindPhoneNumber input").on("keypress", function (e) {
        //回车键
        if (e.keyCode == 13) {
            BindUser();
        }
    })

})