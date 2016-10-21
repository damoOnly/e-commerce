using Ecdev.Plugins;
using EcShop.ControlPanel.Members;
using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Core.ErrorLog;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EcShop.UI.Web.Handler
{
    public class ResetPssswordHandler : System.Web.IHttpHandler
    {
        private string message = "";
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public void ProcessRequest(System.Web.HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "ExistUsername":
                    this.ExistUsername(context);
                    break;

                case "SendRegisterTel":
                    this.SendRegisterTel(context);
                    break;

                case "CheckTelVerifyCode":
                    this.CheckTelVerifyCode(context);
                    break;

                case "ResetPsssword":
                    this.ResetPsssword(context);
                    break;

                case"SendRegisterEmail":
                    this.SendRegisterEmail(context);
                    break;


            }
            context.Response.Write(this.message);
        }

        private void ExistUsername(System.Web.HttpContext context)
        {
            string text = context.Request["username"].ToLower();
            if (string.IsNullOrEmpty(text.ToLower()))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入要检验的账号名名\"}";
                return;
            }
            if (UserHelper.IsExistUserName(text))
            {
                this.message = "{\"success\":true,\"msg\":\"账户名验证通过\"}";
                return;
            }
            else
            {
                this.message = "{\"success\":false,\"msg\":\"账户名不存在\"}";
            }
        }

        private void SendRegisterTel(System.Web.HttpContext context)
        {
            string token = context.Request["token"];
            var cookie = context.Request.Cookies.Get("__RequestVerificationToken");
            string cellphone = context.Request["cellphone"];
            string userName = context.Request["username"].ToLower();

            string ipAddress = "";

            try
            {
                ipAddress = Globals.IPAddress;
            }
            catch (Exception ex)
            {
                ErrorLog.Write("SendRegisterTelX：" + ex.Message);
            }

            if (cookie != null && token != null)
            {
                System.Web.Helpers.AntiForgery.Validate(cookie.Value, token);
            }
            else
            {
                this.message = "{\"success\":false,\"msg\":\"非法调用\"}";
                ErrorLog.Write(string.Format("SendRegisterTelX: IP-{0}, Mobile-{1}", ipAddress, cellphone));
                ErrorLog.Write("SendRegisterTelX: " + this.message);
                return;
            }

            if (string.IsNullOrEmpty(cellphone))
            {
                this.message = "{\"success\":false,\"msg\":\"手机号码不允许为空\"}";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(cellphone, "^(13|14|15|17|18)\\d{9}$"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的手机号码\"}";
                return;
            }

            int result = UserHelper.IsCheckCellPhoneAndUserName(cellphone, userName);
            if (result <= 0)
            {
                this.message = "{\"success\":false,\"msg\":\"该账号并没有绑定该手机号码\"}";
                return;
            }
           
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
            {
                this.message = "{\"success\":false,\"msg\":\"手机服务未配置\"}";
                return;
            }
            this.HaiMeiSendMessage(siteSettings, cellphone);
        }

        public void CheckTelVerifyCode(System.Web.HttpContext context)
        {
            string cellphone = context.Request["cellphone"];
            string phoneVerifyCode = context.Request["CellphoneVerifyCode"].ToLower();
            string selectValue = context.Request["selectValue"];
            if (selectValue == "1")
            {
                if (string.IsNullOrEmpty(cellphone))
                {
                    this.message = "{\"success\":\"false\",\"msg\":\"请输入手机号码\"}";
                    return;
                }
                if (string.IsNullOrEmpty(phoneVerifyCode))
                {
                    this.message = "{\"success\":\"false\",\"msg\":\"请输入验证码\"}";
                    return;
                }

                if (!TelVerifyHelper.CheckVerify(cellphone, phoneVerifyCode))
                {
                    this.message = "{\"success\":false,\"msg\":\"手机验证码验证错误\"}";
                    return;
                }
                else
                {
                    this.message = "{\"success\":true,\"msg\":\"手机验证码验证成功\"}";
                }
            }

            if (selectValue == "2")
            {
                object obj = HiCache.Get(cellphone + "email");
                if (obj == null)
                {
                    this.message = "{\"success\":false,\"msg\":\"邮箱验证码验证错误\"}";
                    return;
                }

                if (phoneVerifyCode.ToLower() != obj.ToString().ToLower())
                {
                    this.message = "{\"success\":false,\"msg\":\"邮箱验证码验证错误\"}";
                    return;
                }

                else
                {
                    HiCache.Remove(cellphone + "email");
                    this.message = "{\"success\":true,\"msg\":\"邮箱验证码验证成功\"}";
                    return;
                }
            }
        }


        public void ResetPsssword(System.Web.HttpContext context)
        {
            string username = context.Request["userName"];
            string password = context.Request["password"];
            string cellphone = context.Request["cellphone"];
            string selectValue = context.Request["selectValue"];
            string cellVerifyCode = context.Request["cellVerifyCode"];
            if(String.IsNullOrEmpty(password))
            {
                this.message = "{\"success\":\"false\",\"msg\":\"密码不能为空\"}";
                return;
            }

            if (!string.IsNullOrEmpty(password) && password.Length < 6)
            {
                this.message = "{\"success\":\"false\",\"msg\":\"密码长度至少6位\"}";
                return;
            }


            if (selectValue == "1")
            {
                if (!TelVerifyHelper.CheckVerify(cellphone, cellVerifyCode))
                {
                    this.message = "{\"success\":false,\"msg\":\"手机验证码验证过期\"}";
                    return;
                }
            }
            Member member = Users.GetUser(0, username, false, true) as Member;
            if(member==null)
            {
                this.message = "{\"success\":\"false\",\"msg\":\"系统出错\"}";
                return;
            }

            if (member.ChangePasswordWithoutAnswer(password))
            {
                //Messenger.UserPasswordChanged(member, password);
                Member newmember = Users.GetUser(0, username, false, true) as Member;
                if (newmember != null)
                {
                    //设置缓存
                    Hashtable hashtable = Users.UserCache();
                    hashtable[Users.UserKey(username)] = newmember;

                    //cookie替换
                    string name = "Vshop-Member";
                    HttpCookie httpCookie2 = new HttpCookie("Vshop-Member");
                    httpCookie2.Value = Globals.UrlEncode(username);
                    httpCookie2.Expires = System.DateTime.Now.AddDays(7);
                    httpCookie2.Domain = HttpContext.Current.Request.Url.Host;
                    if (HttpContext.Current.Response.Cookies[name] != null)
                    {
                        HttpContext.Current.Response.Cookies.Remove(name);
                    }
                    HttpContext.Current.Response.Cookies.Add(httpCookie2);
                 
                }
                this.message = "{\"success\":\"true\",\"msg\":\"你已经成功的修改了登录密码\"}";
                return;
            }
            else
            {
                this.message = "{\"success\":\"false\",\"msg\":\"密码修改失败\"}";
                return;
            }
        }

         public void SendRegisterEmail(System.Web.HttpContext context)
        {
            string text = context.Request["email"];
            string username = context.Request["userName"];
            if (string.IsNullOrEmpty(text))
            {
                this.message = "{\"success\":false,\"msg\":\"邮箱账号不允许为空\"}";
                return;
            }

            if (string.IsNullOrEmpty(username))
            {
                this.message = "{\"success\":false,\"msg\":\"账户名不能为空\"}";
                return;
            }
            username = username.ToLower();

            if (text.Length > 256 || !System.Text.RegularExpressions.Regex.IsMatch(text, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的邮箱账号\"}";
                return;
            }

            int result = UserHelper.IsCheckEmialAndUserName(text, username);
            if (result <= 0)
            {
                this.message = "{\"success\":false,\"msg\":\"该账号并没有绑定该邮箱\"}";
                return;
            }


            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
            {
                this.message = "{\"success\":false,\"msg\":\"邮箱服务未配置\"}";
                return;
            }
            this.SendEmail(siteSettings, text);

        }



         private void SendEmail(SiteSettings settings, string email)
         {
             try
             {
                 string text = HiContext.Current.CreateVerifyCode(4);
                 ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings.EmailSettings));
                 string body = string.Format("尊敬的会员{0}您好：欢迎使用" + settings.SiteName + "系统，此次验证码为：{1},请在3分钟内完成验证", HiContext.Current.User.Username, text);
                 System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
                 {
                     IsBodyHtml = true,
                     Priority = System.Net.Mail.MailPriority.High,
                     SubjectEncoding = System.Text.Encoding.UTF8,
                     BodyEncoding = System.Text.Encoding.UTF8,
                     Body = body,
                     Subject = "来自" + settings.SiteName
                 };
                 mailMessage.To.Add(email);
                 EmailSender emailSender = EmailSender.CreateInstance(settings.EmailSender, configData.SettingsXml);
                 if (emailSender.Send(mailMessage, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
                 {
                     this.message = "{\"success\":true,\"msg\":\"发送邮件成功，请查收\"}";
                     HiCache.Insert(email + "email", text, 10800);
                 }
                 else
                 {
                     this.message = "{\"success\":false,\"msg\":\"发送邮件失败，请检查邮箱账号是否存在\"}";
                 }
             }
             catch (System.Exception)
             {
                 this.message = "{\"success\":false,\"msg\":\"发送失败，请检查邮箱账号是否存在\"}";
             }
         }
        private void HaiMeiSendMessage(SiteSettings settings, string cellphone)
        {
            try
            {
                string text = HiContext.Current.GenerateRandomNumber(4);
                ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings.SMSSettings));
                SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
                string text2 = string.Format(@"您好！您正在进行海美生活会员修改密码，本次的验证码为:{0}，请勿向任何人提供您收到的短信验证码，并尽快完成验证。", text);
                string text3;
               
                bool flag = sMSSender.Send(cellphone, text2, out text3);
                if (flag)
                {
                    //HiCache.Insert(HiContext.Current.User.UserId + "cellphone", text, 10800);
                    EcShop.Entities.Members.Verify verfyinfo = new Entities.Members.Verify();
                    verfyinfo.VerifyCode = text;
                    verfyinfo.CellPhone = cellphone.Trim();
                    EcShop.ControlPanel.Members.TelVerifyHelper.CreateVerify(verfyinfo);

                }
                this.message = "{\"success\":true,\"msg\":\"" + text3 + "\"}";
            }
            catch (System.Exception)
            {
                this.message = "{\"success\":false,\"msg\":\"未知错误\"}";
            }
        }

       
    }
}
