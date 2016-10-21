function DisplayCodeBtn() {
    $(".code-btn").removeAttr("disabled").addClass("code-btnBlue");
}

function DisabledCodeBtn() {
    $(".code-btn").attr("disabled", "disabled").removeClass("code-btnBlue");
}
var validateRegExp = {
    decmal: "^([+-]?)\\d*\\.\\d+$", //浮点数
    decmal1: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*$", //正浮点数
    decmal2: "^-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*)$", //负浮点数
    decmal3: "^-?([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0)$", //浮点数
    decmal4: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0$", //非负浮点数（正浮点数 + 0）
    decmal5: "^(-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*))|0?.0+|0$", //非正浮点数（负浮点数 + 0）
    intege: "^-?[1-9]\\d*$", //整数
    intege1: "^[1-9]\\d*$", //正整数
    intege2: "^-[1-9]\\d*$", //负整数
    num: "^([+-]?)\\d*\\.?\\d+$", //数字
    num1: "^[1-9]\\d*|0$", //正数（正整数 + 0）
    num2: "^-[1-9]\\d*|0$", //负数（负整数 + 0）
    ascii: "^[\\x00-\\xFF]+$", //仅ACSII字符
    chinese: "^[\\u4e00-\\u9fa5]+$", //仅中文
    color: "^[a-fA-F0-9]{6}$", //颜色
    date: "^\\d{4}(\\-|\\/|\.)\\d{1,2}\\1\\d{1,2}$", //日期
    email: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", //邮件
    idcard: "^[1-9]([0-9]{14}|[0-9]{17})$", //身份证
    ip4: "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$", //ip地址
    letter: "^[A-Za-z]+$", //字母
    letter_l: "^[a-z]+$", //小写字母
    letter_u: "^[A-Z]+$", //大写字母
    mobile: "^0?(13|15|18|14|17)[0-9]{9}$", //手机
    notempty: "^\\S+$", //非空
    password: "^.*[A-Za-z0-9\\w_-]+.*$", //密码
    fullNumber: "^[0-9]+$", //数字
    picture: "(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$", //图片
    qq: "^[1-9]*[1-9][0-9]*$", //QQ号码
    rar: "(.*)\\.(rar|zip|7zip|tgz)$", //压缩文件
    tel: "^[0-9\-()（）]{7,18}$", //电话号码的函数(包括验证国内区号,国际区号,分机号)
    url: "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$", //url
    username: "^[A-Za-z0-9_\\-\\u4e00-\\u9fa5\\@\\.]+$", //用户名
    deptname: "^[A-Za-z0-9_()（）\\-\\u4e00-\\u9fa5]+$", //单位名
    zipcode: "^\\d{6}$", //邮编
    realname: "^[A-Za-z\\u4e00-\\u9fa5]+$", // 真实姓名
    companyname: "^[A-Za-z0-9_()（）\\-\\u4e00-\\u9fa5]+$",
    companyaddr: "^[A-Za-z0-9_()（）\\#\\-\\u4e00-\\u9fa5]+$",
    companysite: "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&#=]*)?$"
};

//主函数
(function ($) {
    $.fn.jdValidate = function (option, callback, def) {
        var ele = this;
        var id = ele.attr("id");
        var type = ele.attr("type");
        var rel = ele.attr("rel");
        var _onFocus = $("#" + id + validateSettings.onFocus.container);
        var _succeed = $("#" + id + validateSettings.succeed.container);
        var _isNull = $("#" + id + validateSettings.isNull.container);
        var _error = $("#" + id + validateSettings.error.container);
        if (def == true) {
            var str = ele.val();
            var tag = ele.attr("sta");
            if (str == "" || str == "-1") {
                validateSettings.isNull.run({
                    prompts: option,
                    element: ele,
                    isNullEle: _isNull,
                    succeedEle: _succeed
                }, option.isNull);
            } else if (tag == 1 || tag == 2) {
                return;
            } else {
                callback({
                    prompts: option,
                    element: ele,
                    value: str,
                    errorEle: _error,
                    succeedEle: _succeed
                });
            }
        } else {
            if (typeof def == "string") {
                ele.val(def);
            }
            if (type == "checkbox" || type == "radio") {
                if (ele.attr("checked") == true) {
                    ele.attr("sta", validateSettings.succeed.state);
                }
            }
            switch (type) {
                case "text":
                case "password":
                    ele.bind("focus", function () {
                        var str = ele.val();
                        if (str == def) {
                            ele.val("");
                        }
                        if (id == "txtPassword") {
                            $("#pwdstrength").hide();
                        }
                        validateSettings.onFocus.run({
                            prompts: option,
                            element: ele,
                            value: str,
                            onFocusEle: _onFocus,
                            succeedEle: _succeed
                        }, option.onFocus);
                    })
                        .bind("blur", function () {
                            var str = ele.val();
                            if (str == "") {
                                ele.val(def);
                            }
                            if (validateRules.isNull(str)) {
                                validateSettings.isNull.run({
                                    prompts: option,
                                    element: ele,
                                    value: str,
                                    isNullEle: _isNull,
                                    succeedEle: _succeed
                                }, "");
                            } else {
                                callback({
                                    prompts: option,
                                    element: ele,
                                    value: str,
                                    errorEle: _error,
                                    isNullEle: _isNull,
                                    succeedEle: _succeed
                                });
                            }
                        });
                    break;
                default:
                    if (rel && rel == "select") {
                        ele.bind("change", function () {
                            var str = ele.val();
                            callback({
                                prompts: option,
                                element: ele,
                                value: str,
                                errorEle: _error,
                                isNullEle: _isNull,
                                succeedEle: _succeed
                            });
                        })
                    } else {
                        ele.bind("click", function () {
                            callback({
                                prompts: option,
                                element: ele,
                                errorEle: _error,
                                isNullEle: _isNull,
                                succeedEle: _succeed
                            });
                        })
                    }
                    break;
            }
        }
    }
})(jQuery);

//配置
var validateSettings = {
    onFocus: {
        state: null,
        container: "_error",
        style: "focus",
        run: function (option, str) {
            if (!validateRules.checkType(option.element)) {
                option.element.removeClass(validateSettings.INPUT_style2).addClass(validateSettings.INPUT_style1);
            }
            option.succeedEle.removeClass(validateSettings.succeed.style);
            option.onFocusEle.removeClass().addClass(validateSettings.onFocus.style).html(str);
        }
    },
    isNull: {
        state: 0,
        container: "_error",
        style: "null",
        run: function (option, str) {
            option.element.attr("sta", 0);
            if (!validateRules.checkType(option.element)) {
                if (str != "") {
                    option.element.removeClass(validateSettings.INPUT_style1).addClass(validateSettings.INPUT_style2);
                } else {
                    option.element.removeClass(validateSettings.INPUT_style2).removeClass(validateSettings.INPUT_style1);
                }
            }
            option.succeedEle.removeClass(validateSettings.succeed.style);
            option.isNullEle.removeClass().addClass(validateSettings.isNull.style).html(str);
        }
    },
    error: {
        state: 1,
        container: "_error",
        style: "error",
        run: function (option, str) {
            option.element.attr("sta", 1);
            if (!validateRules.checkType(option.element)) {
                option.element.removeClass(validateSettings.INPUT_style1).addClass(validateSettings.INPUT_style2);
            }
            option.succeedEle.removeClass(validateSettings.succeed.style);
            option.errorEle.removeClass().addClass(validateSettings.error.style).html(str);
        }
    },
    succeed: {
        state: 2,
        container: "_succeed",
        style: "succeed",
        run: function (option) {
            option.element.attr("sta", 2);
            option.errorEle.empty();

            if (!validateRules.checkType(option.element)) {
                option.element.removeClass(validateSettings.INPUT_style1).removeClass(validateSettings.INPUT_style2);
            }
            if (option.element.attr("id") == "schoolinput" && $("#schoolid").val() == "") {
                return;
            }
            if (option.isNullEle) {
                option.isNullEle.removeClass();
            }
            option.succeedEle.addClass(validateSettings.succeed.style);
        }
    },
    INPUT_style1: "highlight1",
    INPUT_style2: "highlight2"

};

//验证规则
var validateRules = {
    isNull: function (str) {
        return (str == "" || typeof str != "string");
    },
    betweenLength: function (str, _min, _max) {
        return (str.length >= _min && str.length <= _max);
    },
    isUid: function (str) {
        return new RegExp(validateRegExp.username).test(str);
    },
    fullNumberName: function (str) {
        return new RegExp(validateRegExp.fullNumber).test(str);
    },
    isPwd: function (str) {
        return /^.*([\W_a-zA-z0-9-])+.*$/i.test(str);
    },
    isPwd2: function (str1, str2) {
        return (str1 == str2);
    },
    isEmail: function (str) {
        return new RegExp(validateRegExp.email).test(str);
    },
    isTel: function (str) {
        return new RegExp(validateRegExp.tel).test(str);
    },
    isMobile: function (str) {
        return new RegExp(validateRegExp.mobile).test(str);
    },
    checkType: function (element) {
        return (element.attr("type") == "checkbox" || element.attr("type") == "radio" || element.attr("rel") == "select");
    },
    isChinese: function (str) {
        return new RegExp(validateRegExp.chinese).test(str);
    },
    isRealName: function (str) {
        return new RegExp(validateRegExp.realname).test(str);
    },
    isDeptname: function (str) {
        return new RegExp(validateRegExp.deptname).test(str);
    },
    isCompanyname: function (str) {
        return new RegExp(validateRegExp.companyname).test(str);
    },
    isCompanyaddr: function (str) {
        return new RegExp(validateRegExp.companyaddr).test(str);
    },
    isCompanysite: function (str) {
        return new RegExp(validateRegExp.companysite).test(str);
    }
};
//验证文本
var validatePrompt = {
    username: {
        onFocus: "4-20位字符组成",
        succeed: "",
        isNull: "请输入用户名",
        error: {
            beUsed: "该用户名已被使用，请更换或使用该用户名<a href='login.aspx' class='flk13'>登录</a>",
            badLength: "用户名长度只能在4-20位字符之间",
            badFormat: "用户名只能由中文、英文、数字及“_”、“-”、“@”、“.”组成",
            fullNumberName: "用户名不能全为数字"
        }
    },
    pwd: {
        onFocus: "6-16位字符，由英文、数字及标点符号组成",
        succeed: "",
        isNull: "请输入密码",
        error: {
            badLength: "密码长度只能在6-16位字符之间",
            badFormat: "密码只能由英文、数字及标点符号组成"
        }
    },
    pwd2: {
        onFocus: "请再次输入密码",
        succeed: "",
        isNull: "请输入密码",
        error: {
            badLength: "密码长度只能在6-16位字符之间",
            badFormat2: "两次输入密码不一致",
            badFormat1: "密码只能由英文、数字及标点符号组成"
        }
    },
    mail: {
        onFocus: "请输入常用的邮箱",
        succeed: "",
        isNull: "请输入邮箱",
        error: {
            beUsed: "该邮箱已被使用，请更换或使用该用户名<a href='login.aspx' class='flk13'>登录</a>",
            badFormat: "邮箱格式不正确",
            badLength: "您填写的邮箱过长，邮件地址只能在50个字符以内"
        }
    },
    moblie: {
        onFocus: "请输入手机号码",
        succeed: "",
        isNull: "请输入手机号码",
        error: {
            beUsed: "该号码已被使用，请更换或使用该手机号码<a href='login.aspx' class='flk13'>登录</a>",
            badFormat: "手机号码格式不正确",
            badLength: "手机号码长度不对"
        }
    },
    authcode: {
        onFocus: "请输入图片中的字符",
        succeed: "",
        isNull: "请输入验证码",
        error: "验证码错误"
    },
    protocol: {
        onFocus: "",
        succeed: "",
        isNull: "请先阅读并同意《商城用户协议》",
        error: ""
    },
    referrer: {
        onFocus: "如您注册并完成订单，推荐人有机会获得积分",
        succeed: "",
        isNull: "",
        error: ""
    },
    schoolinput: {
        onFocus: "您可以用简拼、全拼、中文进行校名模糊查找",
        succeed: "",
        isNull: "请填选学校名称",
        error: "请填选学校名称"
    },
    empty: {
        onFocus: "",
        succeed: "",
        isNull: "",
        error: ""
    },
    CellePhoneVerifyCode: {
        onFocus: "请输入短信验证码",
        succeed: "",
        isNull: "请输入短信验证码",
        error: {
            beUsed: "该短信验证码已过期或已使用",
            badFormat: "短信验证码格式不正确",
            badLength: "短信验证码长度不对"
        }
    },
    RecommendCode: {
        onFocus: "请输入邀请码",
        succeed: "",
        isNull: "请输入邀请码",
        error: {
            beUsed: "邀请码不正确"
        }
    }

};

var nameold, emailold, authcodeold, moblieold, moblieVerifyCodeold;
var namestate = false, emailstate = false, authcodestate = false, mobliestate = false, moblieVerifyCodestate = false;
//回调函数
var validateFunction = {
    username: function (option) {

        var format = validateRules.isUid(option.value);
        var length = validateRules.betweenLength(option.value, 4, 20);
        if (!length && format) {
            validateSettings.error.run(option, option.prompts.error.badLength);
        }
        else if (!length && !format) {
            validateSettings.error.run(option, option.prompts.error.badFormat);
        }
        else if (length && !format) {
            validateSettings.error.run(option, option.prompts.error.badFormat);
        }
//         else if (validateRules.fullNumberName(option.value)) {
//            validateSettings.error.run(option, option.prompts.error.fullNumberName);
//        } 
        else {
            if (!namestate || nameold != option.value) {
                if (nameold != option.value) {
                    nameold = option.value;
                    option.errorEle.html("<span style='color:#999'>检验中……</span>");
                    $.getJSON("Handler/MemberHandler.ashx?action=ExistUsername&username=" + escape(option.value) + "&r=" + Math.random(), function (date) {
                        if (!date.success) {
                            validateSettings.succeed.run(option);
                            namestate = true;
                        } else {
                            validateSettings.error.run(option, option.prompts.error.beUsed.replace("{1}", option.value));
                            namestate = false;
                        }
                    })
                }
                else {
                    validateSettings.error.run(option, option.prompts.error.beUsed.replace("{1}", option.value));
                    namestate = false;
                }
            }
            else {
                validateSettings.succeed.run(option);
            }
        }
    },
    pwd: function (option) {
        var str1 = option.value;
        var str2 = $("#txtPassword2").val();
        var format = validateRules.isPwd(option.value);
        var length = validateRules.betweenLength(option.value, 6, 16);
        $("#pwdstrength").hide();
        if (!length && format) {
            validateSettings.error.run(option, option.prompts.error.badLength);
        }
        else if (!length && !format) {
            validateSettings.error.run(option, option.prompts.error.badFormat);
        }
        else if (length && !format) {
            validateSettings.error.run(option, option.prompts.error.badFormat);
        }
        else {
            validateSettings.succeed.run(option);
            validateFunction.pwdstrength();
        }
        if (str2 == str1) {
            $("#txtPassword2").focus();
        }
    },
    pwd2: function (option) {
        var str1 = option.value;
        var str2 = $("#txtPassword").val();
        var length = validateRules.betweenLength(option.value, 6, 16);
        var format2 = validateRules.isPwd2(str1, str2);
        var format1 = validateRules.isPwd(str1);
        if (!length) {
            validateSettings.error.run(option, option.prompts.error.badLength);
        } else {
            if (!format1) {
                validateSettings.error.run(option, option.prompts.error.badFormat1);
            } else {
                if (!format2) {
                    validateSettings.error.run(option, option.prompts.error.badFormat2);
                }
                else {
                    validateSettings.succeed.run(option);
                }
            }
        }
    },
    mail: function (option) {
        var format = validateRules.isEmail(option.value);
        var format2 = validateRules.betweenLength(option.value, 0, 50);
        if (!format) {
            validateSettings.error.run(option, option.prompts.error.badFormat);
        } else {
            if (!format2) {
                validateSettings.error.run(option, option.prompts.error.badLength);
            } else {
                if (!emailstate || emailold != option.value) {
                    if (emailold != option.value) {
                        emailold = option.value;
                        option.errorEle.html("<span style='color:#999'>检验中……</span>");
                        $.getJSON("Handler/MemberHandler.ashx?action=ExistEmailAndUserName&email=" + escape(option.value) + "&r=" + Math.random(), function (date) {
                            if (date.success) {
                                validateSettings.succeed.run(option);
                                emailstate = true;
                            } else {
                                validateSettings.error.run(option, option.prompts.error.beUsed);
                                emailstate = false;
                            }
                        })
                    }
                    else {
                        validateSettings.error.run(option, option.prompts.error.beUsed);
                        emailstate = false;
                    }
                }
                else {
                    validateSettings.succeed.run(option);
                }
            }
        }
    },
    mobile: function (option) {
            var format = validateRules.isMobile(option.value);
            var format2 = validateRules.betweenLength(option.value, 0, 11);
            if (!format) {
                validateSettings.error.run(option, option.prompts.error.badFormat);
                DisabledCodeBtn();
            } else {
                if (!format2) {
                    validateSettings.error.run(option, option.prompts.error.badLength);
                    DisabledCodeBtn();
                } else {
                    if (!mobliestate || moblieold != option.value) {
                        if (moblieold != option.value) {
                            moblieold = option.value;
                            option.errorEle.html("<span style='color:#999'>检验中……</span>");
                            $.getJSON("Handler/MemberHandler.ashx?action=ExistCellPhoneAndUserName&cellPhone=" + escape(option.value) + "&r=" + Math.random(), function (date) {
                                if (date.success) {
                                    validateSettings.succeed.run(option);
                                    DisplayCodeBtn();
                                    mobliestate = true;
                                } else {
                                    validateSettings.error.run(option, option.prompts.error.beUsed);
                                    DisabledCodeBtn();
                                    mobliestate = false;
                                }
                            })
                        }
                        else {
                            validateSettings.error.run(option, option.prompts.error.beUsed);
                            DisabledCodeBtn();
                            mobliestate = false;
                        }
                    }
                    else {
                        validateSettings.succeed.run(option);
                        DisplayCodeBtn();
                    }
                }
            }
    },
    CellePhoneVerifyCode: function (option) {
            var format2 = validateRules.betweenLength(option.value, 0, 4);
                if (!format2) {
                    validateSettings.error.run(option, option.prompts.error.badLength);
                } else {
                    if (!moblieVerifyCodestate || moblieVerifyCodeold != option.value) {
                        if (moblieVerifyCodeold != option.value) {
                            moblieVerifyCodeold = option.value;
                            option.errorEle.html("<span style='color:#999'>检验中……</span>");
                            $.getJSON("Handler/MemberHandler.ashx?action=ExistTelphoneVerifyCode&CellphoneVerifyCode=" + escape(option.value) + "&Cellphone=" + $("#txtCellPhone").val() + "&r=" + Math.random(), function (date) {
                                if (date.success) {
                                    validateSettings.succeed.run(option);
                                    moblieVerifyCodestate = true;

                                } else {
                                    validateSettings.error.run(option, option.prompts.error.beUsed);
                                    moblieVerifyCodestate = false; 
                                }
                            })
                        }
                        else {
                            validateSettings.error.run(option, option.prompts.error.beUsed);
                            moblieVerifyCodestate = false;
                        }
                    }
                    else {
                        validateSettings.succeed.run(option);
                    }
             }
    },
    RecommendCode: function (option) {
        option.errorEle.html("<span style='color:#999'>检验中……</span>");
        $.getJSON("Handler/MemberHandler.ashx?action=ExistRecemmendCode&Code=" + escape(option.value) + "&r=" + Math.random(), function (date) {
            if (date.success) {
                validateSettings.succeed.run(option);
                $("#hiduseredId").val(date.UseredId);
                $("#hidRecemmendCode").val(date.RecemmendCode);
            } else {
                validateSettings.error.run(option, option.prompts.error.beUsed);
            }
        })
    },
    referrer: function (option) {
        var bool = validateRules.isNull(option.value);
        if (bool) {
            option.element.val("可不填");
            return;
        } else {
            validateSettings.succeed.run(option);
        }
    },
    schoolinput: function (option) {
        var bool = validateRules.isNull(option.value);
        if (bool) {
            validateSettings.error.run(option, option.prompts.error);
            return;
        } else {
            validateSettings.succeed.run(option);
        }
    },
    authcode: function (option) {
                if (!authcodestate || authcodeold != option.value) {
                    if (authcodeold != option.value) {
                        authcodeold = option.value;
                        option.errorEle.html("<span style='color:#999'>检验中……</span>");
                        $.getJSON("Handler/MemberHandler.ashx?action=CheckAuthcode&code=" + escape(option.value) + "&r=" + Math.random(), function(date) {
                            if (date.success) {
                                validateSettings.succeed.run(option);
                                authcodestate = true;
                            } else {
                                validateSettings.error.run(option, option.prompts.error);
                                authcodestate = false;
                            }
                        })
                    }
                    else {
                        validateSettings.error.run(option, option.prompts.error);
                        authcodestate = false;
                    }
                }
                else {
                    validateSettings.succeed.run(option);
                }
       
    },
    protocol: function (option) {
        if (option.element.attr("checked") == true) {
            option.element.attr("sta", validateSettings.succeed.state);
            option.errorEle.html("");
        } else {
            option.element.attr("sta", validateSettings.isNull.state);
            option.succeedEle.removeClass(validateSettings.succeed.style);
        }
    },
    pwdstrength: function () {

        var element = $("#pwdstrength");
        var value = $("#txtPassword").val();
        if (value.length >= 6 && validateRules.isPwd(value)) {
            $("#txtPassword_error").empty();
            element.show();

            var pattern_1 = /^.*([\W_])+.*$/i;
            var pattern_2 = /^.*([a-zA-Z])+.*$/i;
            var pattern_3 = /^.*([0-9])+.*$/i;
            var level = 0;

            if (value.length > 10) {
                level++;
            }

            if (pattern_1.test(value)) {
                level++;
            }

            if (pattern_2.test(value)) {
                level++;
            }

            if (pattern_3.test(value)) {
                level++;
            }

            if (level > 3) {
                level = 3;
            }

            switch (level) {
                case 1:
                    element.removeClass().addClass("strengthA");
                    break;
                case 2:
                    element.removeClass().addClass("strengthB");
                    break;
                case 3:
                    element.removeClass().addClass("strengthC");
                    break;
                default:
                    break;
            }
        } else {
            element.hide();
        }
    },
    checkGroup: function (elements) {
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].checked) {
                return true;
            }
        }
        return false;
    },
    checkSelectGroup: function (elements) {
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].value == -1) {
                return false;
            }
        }
        return true;
    },
    showPassword: function (type) {
        var v1 = $("#txtPassword").val(), s1 = $("#txtPassword").attr("sta"), c1 = document.getElementById("txtPassword").className, t1 = $("#txtPassword").attr("tabindex");
        var v2 = $("#txtPassword2").val(), s2 = $("#txtPassword2").attr("sta"), c2 = document.getElementById("txtPassword2").className, t2 = $("#txtPassword2").attr("tabindex");
        var P1 = $("<input type='" + type + "' value='" + v1 + "' sta='" + s1 + "' class='" + c1 + "' id='txtPassword' name='txtPassword' tabindex='" + t1 + "'/>");
        $("#txtPassword").after(P1).remove();
        $("#txtPassword").bind("keyup",
            function () {
                validateFunction.pwdstrength();
            }).jdValidate(validatePrompt.pwd, validateFunction.pwd)
            var P2 = $("<input type='" + type + "' value='" + v2 + "' sta='" + s2 + "' class='" + c2 + "' id='txtPassword2' name='txtPassword2' tabindex='" + t2 + "'/>");
        $("#txtPassword2").after(P2).remove();
        $("#txtPassword2").jdValidate(validatePrompt.pwd2, validateFunction.pwd2);
    },


    FORM_submit: function (elements) {
        var bool = true;
        for (var i = 0; i < elements.length; i++) {
            if ($(elements[i]).attr("sta") == 2) {
                bool = true;
            } else {
                bool = false;
                break;
            }
        }
        return bool;
    }
};



/**********form表单提交*************/

$.extend(validatePrompt, {
    realname: {
        onFocus: "2-20位字符，可由中文或英文组成",
        succeed: "",
        isNull: "请输入联系人姓名",
        error: {
            badLength: "联系人姓名长度只能在2-20位字符之间",
            badFormat: "联系人姓名只能由中文或英文组成"
        }
    },
    department: {
        onFocus: "",
        succeed: "",
        isNull: "请选择联系人所在部门",
        error: ""
    },
    tel: {
        onFocus: "请填写联系人常用的电话，以便于我们联系，如：0527-88105500",
        succeed: "",
        isNull: "请输入联系人固定电话",
        error: "电话格式错误，请重新输入"
    },
    mobile: {
        onFocus: "请输入您的手机号码",
        succeed: "",
        isNull: "请输入您的手机号码",
        error: "手机号格式错误，请重新输入"
    },
    companyname: {
        onFocus: "请填写工商局注册的全称。4-40位字符，可由中英文、数字及“_”、“-”、()、（）组成",
        succeed: "",
        isNull: "请输入公司名称",
        error: {
            badLength: "公司名称长度只能在4-40位字符之间",
            badFormat: "公司名称只能由中文、英文、数字及“_”、“-”、()、（）组成"
        }
    },
    companyarea: {
        onFocus: "请选择公司所在地",
        succeed: "",
        isNull: "请选择公司所在地",
        error: ""
    },
    companyaddr: {
        onFocus: "请详细填写公司经营地址　如：北京市海淀区苏州街20号银丰大厦B座3层",
        succeed: "",
        isNull: "请输入公司地址",
        error: {
            badLength: "公司地址长度只能在4-50位字符之间",
            badFormat: "公司地址只能由中文、英文、数字及“_”、“-”、()、（）、#组成"
        }
    },
    purpose: {
        onFocus: "",
        succeed: "",
        isNull: "请选择购买类型/用途",
        error: ""
    },
    companysite: {
        onFocus: "如：http://www.360buy.com",
        succeed: "",
        isNull: "请输入公司网址",
        error: {
            badLength: "公司网址长度只能在80位字符之内",
            badFormat: "公司网址格式不正确。应如：http://www.360buy.com"
        }
    }
});

$.extend(validateFunction, {
    realname: function (option) {
        var length = validateRules.betweenLength(option.value.replace(/[^\x00-\xff]/g, "**"), 2, 20);
        var format = validateRules.isRealName(option.value);
        if (!length) {
            validateSettings.error.run(option, option.prompts.error.badLength);
        } else {
            if (!format) {
                validateSettings.error.run(option, option.prompts.error.badFormat);
            }
            else {
                validateSettings.succeed.run(option);
            }
        }
    },
    department: function (option) {
        var bool = (option.value == -1);
        if (bool) {
            validateSettings.isNull.run(option, "");
        }
        else {
            validateSettings.succeed.run(option);
        }
    },
    tel: function (option) {
        var format = validateRules.isTel(option.value);
        if (!format) {
            validateSettings.error.run(option, option.prompts.error);
        }
        else {
            validateSettings.succeed.run(option);
        }
    },
    companyname: function (option) {
        var length = validateRules.betweenLength(option.value.replace(/[^\x00-\xff]/g, "**"), 4, 40);
        var format = validateRules.isCompanyname(option.value);
        if (!length) {
            validateSettings.error.run(option, option.prompts.error.badLength);
        }
        else {
            if (!format) {
                validateSettings.error.run(option, option.prompts.error.badFormat);
            } else {
                validateSettings.succeed.run(option);
            }
        }
    },
    companyarea: function (option) {
        var bool = (option.value == -1);
        if (bool) {
            validateSettings.isNull.run(option, "");
        }
        else {
            validateSettings.succeed.run(option);
        }
    },
    companyaddr: function (option) {
        var length = validateRules.betweenLength(option.value.replace(/[^\x00-\xff]/g, "**"), 4, 50);
        var format = validateRules.isCompanyaddr(option.value);
        if (!length) {
            validateSettings.error.run(option, option.prompts.error.badLength);
        } else {
            if (!format) {
                validateSettings.error.run(option, option.prompts.error.badFormat);
            }
            else {
                validateSettings.succeed.run(option);
            }
        }
    },
    purpose: function (option) {
        var purpose = $("input:checkbox[@name='purposetype']");
        if (validateFunction.checkGroup(purpose)) {
            validateSettings.succeed.run(option);
        } else {
            validateSettings.error.run(option, option.prompts.isNull);
        }
    },
    companysite: function (option) {
        var length = validateRules.betweenLength(option.value, 0, 80);
        var format = validateRules.isCompanysite(option.value);
        if (!length) {
            validateSettings.error.run(option, option.prompts.error.badLength);
        } else {
            if (!format) {
                validateSettings.error.run(option, option.prompts.error.badFormat);
            }
            else {
                validateSettings.succeed.run(option);
            }
        }
    },
    FORM_validate: function (regType) {
        
        /*$("#txtUserName").jdValidate(validatePrompt.username, validateFunction.username, true);
        $("#realname").jdValidate(validatePrompt.realname, validateFunction.realname, true);
        $("#department").jdValidate(validatePrompt.department, validateFunction.department, true);
        $("#tel").jdValidate(validatePrompt.tel, validateFunction.tel, true);
        $("#companyname").jdValidate(validatePrompt.companyname, validateFunction.companyname, true);
        $("#companyaddr").jdValidate(validatePrompt.companyaddr, validateFunction.companyaddr, true);
        $("#companysite").jdValidate(validatePrompt.companysite, validateFunction.companysite, true);
        $("#purpose").jdValidate(validatePrompt.purpose, validateFunction.purpose, true);
        return validateFunction.FORM_submit(["#txtUserName", "#txtPassword", "#txtPassword2", "#txtEmail", "#txtCellPhone"]);*/

        $("#txtPassword").jdValidate(validatePrompt.pwd, validateFunction.pwd, true)
        $("#txtPassword2").jdValidate(validatePrompt.pwd2, validateFunction.pwd2, true);

        if (regType == 1) {
            $("#txtCellPhone").jdValidate(validatePrompt.moblie, validateFunction.mobile, true);
            $("#txtTelphoneVerifyCode").jdValidate(validatePrompt.CellePhoneVerifyCode, validateFunction.CellePhoneVerifyCode, true);
            return validateFunction.FORM_submit(["#txtCellPhone", "#txtPassword", "#txtPassword2", "#txtTelphoneVerifyCode"]);
        }
        else if (regType == 2)
        {
            $("#txtEmail").jdValidate(validatePrompt.mail, validateFunction.mail, true);
            $("#txtNumber").jdValidate(validatePrompt.authcode, validateFunction.authcode, true);
            return validateFunction.FORM_submit(["#txtEmail", "#txtPassword", "#txtPassword2"]);
        }
    }
});

$(function () {

//如果手机号码为空,则验证码不可点击。
$("#txtCellPhone").bind("keyup", function () {
    var cellPhone = $(this).val();
    if (!cellPhone) {
        DisabledCodeBtn();
    }
});

//密码验证
$("#txtPassword").bind("keyup", function () {
    validateFunction.pwdstrength();
}).jdValidate(validatePrompt.pwd, validateFunction.pwd);
//二次密码验证
$("#txtPassword2").jdValidate(validatePrompt.pwd2, validateFunction.pwd2);


//邮箱验证(用户名)
$("#txtEmail").jdValidate(validatePrompt.mail, validateFunction.mail);
//验证码验证
$("#txtNumber").jdValidate(validatePrompt.authcode, validateFunction.authcode);

//手机验证码
$("#txtTelphoneVerifyCode").jdValidate(validatePrompt.CellePhoneVerifyCode, validateFunction.CellePhoneVerifyCode);


//键盘输入验证码验证
$("#txtNumber").bind('keyup', function (event) {
    if (event.keyCode == 13) {
        $("#register_btnRegister").click();
    }
});

//手机号码认证(用户名)
$("#txtCellPhone").jdValidate(validatePrompt.moblie, validateFunction.mobile);
    //注册类型（1为手机注册，2为邮箱注册）
$("#txtRecommendCode").jdValidate(validatePrompt.RecommendCode, validateFunction.RecommendCode);

var rgType = Number($(".rg-main .rg-tab .rg-tabhead li.selected").attr("regType"));
$("#regType").val(rgType);
if (rgType == 1) {
    $(".divNumber").css("display", "none");
    //默认下用户名(手机号码)框获得焦点
    //setTimeout(function () {
    //    $("#txtCellPhone").get(0).focus();
    //}, 0);
}
else if (rgType == 2) {
    $(".divNumber").css("display", "");
    //默认下用户名(邮箱)框获得焦点
    //setTimeout(function () {
    //    $("#txtEmail").get(0).focus();
    //}, 0);

}

    //推荐人用户名
    //$("#referrer").bind("keydown", function () {
    //    $(this).css({ "color": "#333333", "font-size": "14px" });
    //}).bind("keyup", function () {
    //    if ($(this).val() == "" || $(this).val() == "可不填") {
    //        $(this).css({ "color": "#999999", "font-size": "12px" });
    //    }
    //}).bind("blur", function () {
    //    if ($(this).val() == "" || $(this).val() == "可不填") {
    //        $(this).css({ "color": "#999999", "font-size": "12px" }).jdValidate(validatePrompt.referrer, validateFunction.referrer, "可不填");
    //    }
    //})
    //联系人姓名验证
    //$("#realname").jdValidate(validatePrompt.realname, validateFunction.realname);
    //部门验证
    //$("#department").jdValidate(validatePrompt.department, validateFunction.department);
    //固定电话验证
    //$("#tel").jdValidate(validatePrompt.tel, validateFunction.tel);
    //公司名称验证
    //$("#companyname").jdValidate(validatePrompt.companyname, validateFunction.companyname);
    //公司地址验证
    //$("#companyaddr").jdValidate(validatePrompt.companyaddr, validateFunction.companyaddr);
    //公司网址验证
    //$("#companysite").jdValidate(validatePrompt.companysite, validateFunction.companysite);
    //显示密码事件
    //$("#viewpwd").bind("click", function () {
    //    if ($(this).attr("checked") == true) {
    //        validateFunction.showPassword("text");
    //        $("#o-password").addClass("pwdbg");
    //    } else {
    //        validateFunction.showPassword("password");
    //        $("#o-password").removeClass("pwdbg");
    //    }
    //});
    //购买类型/用途验证
    //$("input:checkbox[@name='purposetype']").bind("click", function () {
    //    var value1 = $("#purpose").val();
    //    var value2 = $(this).val();
    //    if ($(this).attr("checked") == true) {
    //        if (value1.indexOf(value2) == -1) {
    //            $("#purpose").val(value1 + value2);
    //            $("#purpose").attr("sta", 2);
    //            $("#purpose_error").html("");
    //            $("#purpose_succeed").addClass("succeed");
    //        }
    //    } else {
    //        if (value1.indexOf(value2) != -1) {
    //            value1 = value1.replace(value2, "");
    //            $("#purpose").val(value1);
    //            if ($("#purpose").val() == "") {
    //                $("#purpose").attr("sta", 0);
    //                $("#purpose_succeed").removeClass("succeed");
    //            }
    //        }
    //    }
    //});
    //确认协议才能提交
     $("#chkAgree").click(function () {
        if ($("#chkAgree").is(":checked")!= true) {
            $("#register_btnRegister").attr({ "disabled": "disabled" });
            $("#register_btnRegister").addClass("disabled");
            $("#register_btnRegister").css("display","none");
        }
        else {
            $("#register_btnRegister").removeAttr("disabled");
            $("#register_btnRegister").removeClass("disabled");
            $("#register_btnRegister").css("display","");
        }
    });
    //表单提交验证和服务器请求
     $("#aspnetForm").submit(function () {
        //注册类型（1为手机注册，2为邮箱注册）
        var regType = Number($(".rg-main .rg-tab .rg-tabhead li.selected").attr("regType"));
        var flag = validateFunction.FORM_validate(regType);
        if (flag) {
            $(this).attr({ "disabled": "disabled" });
            $.ajax({
                type: "POST",
                url: "Handler/MemberHandler.ashx?action=RegisterMember",
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                data: $("#aspnetForm").serialize(),
                async:true,
                success: function (result)
                {
                    if (result.success)
                    {
                        if ($("#txtCellPhone").val()!=null)
                        {
                                _adwq.push([
                                '_setAction', '8kdper',
                                $("#txtCellPhone").val() //请填入注册用户名 或 注册用户ID (请强制转换成字符串类型 )
                                ]);
                        }
                        window.location.href = decodeURIComponent(result.msg);
                        //if (window.inter) {
                        //    window.clearTimeout(window.inter);
                        //}
                        //window.inter = window.setTimeout(function () {
                        //    window.location.href = "/default.aspx";
                        //}, 100);
                    } else {
                        $("#register_btnRegister").removeAttr("disabled");
                        refreshCode();
                        alert(result.msg);
                     }
                },
                error: function (a, b, c) {
                    alert(a + ":" + b + ":" + c);
                }
            });
            return false;
        } else {
            return false;
        }
    });
});
