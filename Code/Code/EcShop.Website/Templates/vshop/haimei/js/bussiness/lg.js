// JavaScript Document

function BindUser() {

	var username = $.trim($("#txtUserName").val()),
	   password = $.trim($("#txtPassword").val());

	if (!username || username.length < 2)
		alert_h('用户名不能为空并且至少要2个字符');
	else if (!password || password.length < 6)
		alert_h('密码不能为空并且至少要6个字符');
	else {


		$.ajax({
			url: "/API/VshopProcess.ashx",
			type: 'post', dataType: 'json', timeout: 10000,
			data: { action: "BindPCUser", openId: getParam("sessionId"), userName: username, password: password, nickName: getParam("nickname") },
			success: function (resultData) {
				if (resultData.Status == "OK") {
					alert_h("登录成功！");
					window.setTimeout(function(){
						returnUrl = getParam("returnUrl");
						if (returnUrl != "") {
							if (returnUrl.toLowerCase().indexOf("logout.aspx") > -1){
								location.href = "MemberCenter.aspx";
							}else{
								location.href = returnUrl;
							}
						}
						else{
							location.href = "MemberCenter.aspx";
						}
					},100);					
				}
				else if (resultData.Status == "-1") {
					alert_h("用户名不存在, 请重试");
				}
				else if (resultData.Status == "2")
					alert_h("您要绑定的用户已经被系统禁止登录");
				else {
					alert_h("用户名或密码错误, 请重试");
				}
			}
		});
	}
}
function RegisterUser() {
	var username = $.trim($("#txtRegisterUserName").val()),
	   email = $.trim($('#text_email_domain').val()),
	   password = $.trim($("#text_password").val());
	var checked = $("#agreen")[0].checked;
	if(!checked){
		alert_h('必须同意协议');
		return false;
	}
	var emailReg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
	if (!username || username.length < 2)
		alert_h('用户名不能为空并且至少要2个字符');
	else if (!email)
		alert_h('邮箱地址不能为空');
	else if (!emailReg.test(email)) {
		alert_h('邮箱地址格式不正确');
	}
	else if (!password || password.length < 6)
		alert_h('密码不能为空并且至少要6个字符');
	else {
		$.ajax({
			url: "/API/VshopProcess.ashx",
			type: 'post', dataType: 'json', timeout: 10000,
			data: { action: "RegisterUser", openId: getParam("sessionId"), userName: username, email: email, password: password, nickName: getParam("nickname") },
			success: function (resultData) {
				if (resultData.Status == "OK") {					
					alert_h("注册成功！");
					window.setTimeout(function(){
						returnUrl = getParam("returnUrl");
						if (returnUrl != "" && returnUrl.toLowerCase().indexOf("login.aspx") == -1 && returnUrl.toLowerCase().indexOf("logout.aspx") == -1) {
							location.href = returnUrl;
						}
						else{
							location.href = "MemberCenter.aspx";
						}
					},100);	
				}
				else if (resultData.Status == "-1") {
					alert_h("用户名已被注册过, 请重试");
				}
				else if (resultData.Status == "-2") {
					alert_h("邮箱已经被注册过, 请重试");
				}
				else {
					alert_h("注册失败, 请重试");
				}
			}
		});
	}
}
$(function(){
	//看是否 是登录 或者 注册	
    setLgStatus(getParam("action"));
	//登录
	$("#btnLogin").on("click",function(){
		BindUser();
	})
	//注册
	$("#btnRegisterUser").on("click", function(){
		var theme = "disabled";
		if($(this).hasClass(theme)){
			return;
		}
		RegisterUser();
	}); 
	//点击 去登录 按钮
	$("#aLogin").on("click",function(){
		setLgStatus("login");
	})
	//点击 去注册 按钮
	$("#aRegister").bind("click", function () { 
		setLgStatus("register");
	});
	//同意 协议
	$("#agreen").bind("change", function () { 
		var checked = this.checked;
		var theme = "disabled";
		var $btn = $("#btnRegisterUser");		
		if(checked){
			 $btn.removeClass(theme);
		}else{
			 $btn.addClass(theme);
		}		
	});
	//按下 回车键
	$("#divRegister input").on("keypress",function(e){
		//回车键
		if(e.keyCode == 13){
			RegisterUser();
		}
	})
	$("#divLogin input").on("keypress",function(e){
		//回车键
		if(e.keyCode == 13){
			BindUser();
		}
	})
	
})
//看是否 是登录 或者 注册
function setLgStatus(action){	
	var $registerPanel = $("#divRegister");
	var $loginPanel = $("#divLogin");
	var $header = $(".page-tit .title");
	var $title = $("title");
	if (action != "" && action.toLowerCase() == "register") {
		$registerPanel.show();
		$loginPanel.hide();
		$header.text("注册");
		$title.text("注册");
	}else {
		$registerPanel.hide();
		$loginPanel.show();
		$header.text("登录");
		$title.text("登录");
	}
}