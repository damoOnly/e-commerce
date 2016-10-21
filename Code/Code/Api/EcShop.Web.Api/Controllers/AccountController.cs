using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using EcShop.Core;
using EcShop.Entities.Members;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;

using EcShop.Web.Api.ApiException;

using EcShop.SqlDal.Members;

using EcShop.Membership.ASPNETProvider;
using EcShop.Messages;

using EcShop.Web.Api.Model.RequestJsonParams;
using EcShop.Web.Api.Model.Result;

using EcShop.Web.Api.Utility;
using EcShop.SaleSystem.Member;
using System.Data;
using Ecdev.Plugins;
using EcShop.ControlPanel.Members;
using EcShop.Entities;
using EcShop.ControlPanel.Promotions;
using EcShop.Entities.Promotions;
using System.Configuration;
using EcShop.ControlPanel.Comments;

namespace EcShop.Web.Api.Controllers
{
    public class AccountController : EcdevApiController
    {
        MemberDao memberDao = null;

        public AccountController()
        {
            memberDao = new MemberDao();
        }

        [HttpPost]
        public IHttpActionResult SendSMSCode(JObject request)
        {
            Logger.WriterLogger("Account.SendSMSCode, Params: " + request.ToString(), LoggerType.Info);

            ParamSendSMSCode param = new ParamSendSMSCode();

            try
            {
                param = request.ToObject<ParamSendSMSCode>();
            }
            catch
            {
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());

            }

            string accessToken = param.accessToken;
            string sessionKey = "";
            string sessionSecret = "";

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string mobile = param.Mobile;
            int cType = param.CType;

            if (string.IsNullOrEmpty(mobile))
            {
                return base.JsonFaultResult(new FaultInfo(40100, "手机号码为空"), request.ToString());
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(mobile, "^(13|14|15|17|18)\\d{9}$"))
            {
                return base.JsonFaultResult(new FaultInfo(40100, "请输入正确的手机号码"), request.ToString());
            }

            if (cType == 1 && UserHelper.IsExistCellPhoneAndUserName(mobile) > 0)
            {
                return base.JsonFaultResult(new FaultInfo(40100, "已经存在相同的手机号码"), request.ToString());
            }

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            SiteSettings settings = HiContext.Current.SiteSettings;
            if (!settings.SMSEnabled || string.IsNullOrEmpty(settings.SMSSettings))
            {
                return base.JsonFaultResult(new FaultInfo(40999, "手机服务未配置"), request.ToString());
            }

            //生成随机短信验证码
            string verifyCode = HiContext.Current.GenerateRandomNumber(4);
            ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings.SMSSettings));
            SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
            string smsContent = string.Format(SMS_FORART, verifyCode);
            string msg;

            bool flag = sMSSender.Send(mobile, smsContent, out msg);
            if (flag)
            {
                // 保存到数据库
                Verify verfyinfo = new Verify();
                verfyinfo.VerifyCode = verifyCode;
                verfyinfo.CellPhone = mobile;
                verfyinfo.CType = cType;
                TelVerifyHelper.CreateVerify(verfyinfo);

                Verify verifyto = new TelVerifyDao().GetVerify(mobile);

                if (verifyto != null && verifyto.CType == cType && verifyto.VerifyCode == verifyCode)
                {
                    StandardResult<string> okResult = new StandardResult<string>();
                    okResult.code = 0;
                    okResult.msg = "成功返回短信验证码";
                    okResult.data = base.Encrypt(verifyCode, sessionKey, sessionSecret);

                    return base.JsonActionResult(okResult);
                }

                return base.JsonFaultResult(new CommonException(40205).GetMessage(), request.ToString());

            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult Register(JObject request)
        {
            Logger.WriterLogger("Account.Register, Params: " + request.ToString(), LoggerType.Info);

            ParamRegister param = new ParamRegister();

            try
            {
                param = request.ToObject<ParamRegister>();
            }
            catch
            {
                //throw new CommonException(40100);
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;
            string sessionKey = "";
            string sessionSecret = "";

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string username = param.username;
            string code = param.code;

            string password = param.password;
            int accountType = param.accountType;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            string decryptUsername = username;
            string decryptPassword = password;
            string decryptCode = code;

            if ((platform == 3 && base.AndroidIsEncryption) || (platform == 2 && base.IOSIsEncryption) || (platform == 1 && base.PCIsEncryption))
            {
                decryptUsername = base.Decrypt(username, sessionKey, sessionSecret);      //TODO 解密
                decryptPassword = base.Decrypt(password, sessionKey, sessionSecret);

                //decryptCode = base.Decrypt(code, sessionKey, sessionSecret);
            }

            string email = "";
            string mobile = "";
            switch (accountType)
            {
                case 1:
                    break;
                case 2:
                    mobile = decryptUsername;
                    //email = decryptUsername + "@mail.haimylife.com";

                    if (string.IsNullOrEmpty(decryptUsername))
                    {
                        return base.JsonFaultResult(new FaultInfo(40100, "手机号码为空"), request.ToString());
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(decryptUsername, "^(13|14|15|17|18)\\d{9}$"))
                    {
                        return base.JsonFaultResult(new FaultInfo(40100, "请输入正确的手机号码"), request.ToString());
                    }
                    if (UserHelper.IsExistCellPhoneAndUserName(decryptUsername) > 0)
                    {
                        return base.JsonFaultResult(new FaultInfo(40100, "已经存在相同的手机号码"), request.ToString());
                    }

                    Verify verifyto = new TelVerifyDao().GetVerify(decryptUsername, 1);
                    if (!(verifyto != null && verifyto.VerifyCode == decryptCode))
                    {
                        return base.JsonFaultResult(new CommonException(40205).GetMessage(), request.ToString());
                    }
                    break;
                case 3:
                    email = decryptUsername;
                    break;
            }

            string msg = "";

            int provinceId = 0;
            int userId = 0;

            string sessionId = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(decryptUsername) || string.IsNullOrEmpty(decryptPassword))
            {
                return base.JsonFaultResult(new FaultInfo(40200, "缺少必填参数"), request.ToString());
            }

            //注册类型为3时检测邮箱
            if (accountType == 3)
            {
                if (string.IsNullOrEmpty(email.Trim()))
                {
                    return base.JsonFaultResult(new FaultInfo(40200, "邮箱帐号不能为空"), request.ToString());
                }
                if (email.Length > 256 || !System.Text.RegularExpressions.Regex.IsMatch(email, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
                {
                    return base.JsonFaultResult(new FaultInfo(40200, "错误的邮箱帐号"), request.ToString());
                }
            }

            //判断邀请码是否存在
            if(!string.IsNullOrWhiteSpace(param.recemmendCode))
            {
                if (!MemberHelper.IsExsitRecommendCode(param.recemmendCode.Trim(),0))
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "邀请码错误，请填写正确的邀请码或者选择不填"), request.ToString());
                }
            }

            Member member = new Member(UserRole.Member);
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            member.SessionId = Globals.GetGenerateId();
            member.Username = decryptUsername;

            member.Password = decryptPassword;
            member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
            member.TradePassword = decryptPassword;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            member.MembershipUser.SessionId = sessionId;
            member.CellPhone = mobile;
            member.CreateDate = DateTime.Now;

            //member.MembershipUser.MobilePIN = mobile;
            // 手机注册默认验证手机号码
            if (accountType == 2)
            {
                member.CellPhoneVerification = true;
            }

            if (accountType == 3)
            {
                member.Email = email;
            }

            member.UserType = this.CovertToUserType(param.platform);

            CreateUserStatus createUserStatus = MemberProcessor.CreateMember(member);

            if (createUserStatus == CreateUserStatus.DuplicateUsername || createUserStatus == CreateUserStatus.DisallowedUsername)
            {
                msg = "已经存在该用户名";
            }
            if (createUserStatus == CreateUserStatus.DuplicateEmailAddress)
            {
                msg = "已经存在相同的邮箱";
            }
            if (createUserStatus == CreateUserStatus.DuplicateCellphone)
            {
                msg = "已经存在相同的手机号码";
            }

            if (createUserStatus == CreateUserStatus.Created)
            {
                Messenger.UserRegister(member, decryptPassword);
                member.OnRegister(new UserEventArgs(member.Username, decryptPassword, null));

                long timestamp = long.Parse(DateTime.Now.AddSeconds(USER_CACHE_KEEP_TIME).ToString("yyyyMMddHHmmss"));

                string authenTicket = SecurityUtil.MD5Encrypt(sessionId + timestamp.ToString() + sessionSecret).ToLower();
                string authenUserId = sessionId.ToLowerGuid();

                CacheUserSet(authenUserId, authenTicket, timestamp, member.UserId);

                #region ==判断是否有优惠卷
                if (!MemberHelper.NewUserSendRegisterCoupon(member))
                {
                    msg = "注册优惠卷赠送失败";
                    return base.JsonFaultResult(new FaultInfo(40200, msg), request.ToString());
                }
                #endregion
                #region == 判断是否有邀请码
                string sourcechars = ConfigurationManager.AppSettings["sourcechars"];
                string newsourcechars = ConfigurationManager.AppSettings["newsourcechars"];
                string rcode = param.recemmendCode;
                if (!string.IsNullOrWhiteSpace(rcode))
                {
                    string currcode = BaseConvertHelper.BaseConvert(rcode, newsourcechars, sourcechars);
                    string useredId = MemberHelper.GetUserIdByRecommendCode(currcode);
                    if (!string.IsNullOrWhiteSpace(useredId) && !string.IsNullOrWhiteSpace(currcode))
                    {
                        // 插入到邀请码记录表
                        MemberHelper.AddRecommendCodeRecord(member.UserId, Convert.ToInt32(useredId), currcode, rcode);
                    }
                }
                #endregion
                StandardResult<RegisterOrLoginResult> okResult = new StandardResult<RegisterOrLoginResult>();
                okResult.code = 0;
                okResult.msg = "注册成功，恭喜您，50元现金券已经打到您的海美账户了，现在就去购物吧！";
                okResult.data = new RegisterOrLoginResult()
                {
                    DisplayName = decryptUsername,
                    AuthenTicket = authenTicket,
                    AuthenUserId = authenUserId,
                    Timestamp = timestamp
                };

                return base.JsonActionResult(okResult);
            }
            else
            {
                msg = "注册用户失败，" + msg;
            }

            return base.JsonFaultResult(new FaultInfo(40200, msg), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult Login(JObject request)
        {
            Logger.WriterLogger("Account.Login, Params: " + request.ToString(), LoggerType.Info);

            ParamLogin param = new ParamLogin();

            try
            {
                try
                {
                    param = request.ToObject<ParamLogin>();
                }
                catch
                {
                    //throw new CommonException(40100);
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }

                string username = param.username;
                string password = param.password;
                int channel = param.channel;
                int platform = param.platform;
                string ver = param.ver;

                // 保存访问信息
                base.SaveVisitInfo("", channel, platform, ver);

                //ThrowParamException(username, password);

                string decryptUsername = username;
                string decryptPassword = password;

                if ((platform == 3 && base.AndroidIsEncryption) || (platform == 2 && base.IOSIsEncryption) || (platform == 1 && base.PCIsEncryption))
                {
                    decryptUsername = base.Decrypt(username, sessionKey, sessionSecret);      //TODO 解密
                    decryptPassword = base.Decrypt(password, sessionKey, sessionSecret);
                }

                string msg = "";

                //if (member == null || member.IsAnonymous)
                //{
                //    msg = "用户名或密码错误";
                //}
                //if (member.ParentUserId.HasValue && member.ParentUserId.Value != 0)
                //{
                //    msg = "您不是本站会员，请您进行注册";
                //}

                string sessionId = "";
                int userId = 0;

                if (string.IsNullOrEmpty(decryptUsername) || string.IsNullOrEmpty(decryptPassword))
                {
                    return base.JsonFaultResult(new FaultInfo(40200, "缺少必填参数"), request.ToString());
                }

                Logger.WriterLogger("开始获取用户信息...");
                Member member = Users.GetUser(0, decryptUsername, false, true) as Member;
                Logger.WriterLogger("获取用户信息成功...");

                if (member == null)
                {
                    return base.JsonFaultResult(new FaultInfo(40200, "用户名无效"), request.ToString());
                }

                member.Password = decryptPassword;
                LoginUserStatus loginUserStatus = MemberProcessor.ValidLogin(member);
                if (loginUserStatus != LoginUserStatus.Success)
                {
                    return base.JsonFaultResult(new FaultInfo(40200, "密码有误"), request.ToString());
                }

                member.OnLogin();
                sessionId = member.MembershipUser.SessionId;

                Logger.WriterLogger("开始更新SessionId...");
                member.SessionId = UserHelper.UpdateSessionId(member.UserId);
                Logger.WriterLogger("SessionId更新成功...");
                Logger.WriterLogger("开始更新用户信息...");
                Users.UpdateUser(member);
                Logger.WriterLogger("更新用户信息成功...");

                long timestamp = long.Parse(DateTime.Now.AddSeconds(USER_CACHE_KEEP_TIME).ToString("yyyyMMddHHmmss"));

                string authenTicket = SecurityUtil.MD5Encrypt(sessionId + timestamp.ToString() + sessionSecret).ToLower();
                string authenUserId = sessionId.ToLowerGuid();

                CacheUserSet(authenUserId, authenTicket, timestamp, userId);

                StandardResult<RegisterOrLoginResult> okResult = new StandardResult<RegisterOrLoginResult>();
                okResult.code = 0;
                okResult.msg = "登录成功";
                okResult.data = new RegisterOrLoginResult()
                {
                    DisplayName = decryptUsername,
                    AuthenTicket = authenTicket,
                    AuthenUserId = authenUserId,
                    Timestamp = timestamp
                };

                return base.JsonActionResult(okResult);
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }

        [HttpPost]
        public IHttpActionResult ChangePassword(JObject request)
        {
            Logger.WriterLogger("Account.ChangePassword, Params: " + request.ToString(), LoggerType.Info);

            ParamChangePassword param = new ParamChangePassword();

            try
            {
                try
                {
                    param = request.ToObject<ParamChangePassword>();
                }
                catch
                {
                    //throw new CommonException(40100);
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }

                string newPassword = param.NewPassword;
                string password = param.Password;
                int channel = param.channel;
                int platform = param.platform;
                string ver = param.ver;

                // 保存访问信息
                base.SaveVisitInfo(param.UserId, channel, platform, ver);

                //ThrowParamException(username, password);

                string decryptNewPassword = newPassword;
                string decryptPassword = password;

                if ((platform == 3 && base.AndroidIsEncryption) || (platform == 2 && base.IOSIsEncryption))
                {
                    decryptNewPassword = base.Decrypt(newPassword, sessionKey, sessionSecret);      //TODO 解密
                    decryptPassword = base.Decrypt(password, sessionKey, sessionSecret);
                }

                string msg = "";


                string sessionId = param.UserId.ToSeesionId();
                int userId = 0;

                Member member = base.GetMember(sessionId);

                if (member != null)
                {
                    member.Password = decryptPassword;

                    if (member.ChangePassword(decryptPassword, decryptNewPassword))
                    {
                        long timestamp = long.Parse(DateTime.Now.AddSeconds(USER_CACHE_KEEP_TIME).ToString("yyyyMMddHHmmss"));

                        string authenTicket = SecurityUtil.MD5Encrypt(sessionId + timestamp.ToString() + sessionSecret).ToLower();
                        string authenUserId = sessionId.ToLowerGuid();

                        CacheUserSet(authenUserId, authenTicket, timestamp, userId);

                        StandardResult<RegisterOrLoginResult> okResult = new StandardResult<RegisterOrLoginResult>();
                        okResult.code = 0;
                        okResult.msg = "修改密码成功";
                        okResult.data = new RegisterOrLoginResult()
                        {
                            DisplayName = member.Username,
                            AuthenTicket = authenTicket,
                            AuthenUserId = authenUserId,
                            Timestamp = timestamp
                        };

                        return base.JsonActionResult(okResult);
                    }

                    return base.JsonFaultResult(new FaultInfo(40202, "原密码错误"), request.ToString());
                }
                else
                {
                    return base.JsonFaultResult(new FaultInfo(40201, "会员信息不存在"), request.ToString());
                }
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }

        [HttpPost]
        public IHttpActionResult ResetPassword(JObject request)
        {
            Logger.WriterLogger("Account.ResetPassword, Params: " + request.ToString(), LoggerType.Info);

            ParamResetPassword param = new ParamResetPassword();

            try
            {
                try
                {
                    param = request.ToObject<ParamResetPassword>();
                }
                catch
                {
                    //throw new CommonException(40100);
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }

                string mobile = param.Mobile;
                //短信验证码
                string code = param.Code;

                if (string.IsNullOrEmpty(mobile))
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "手机号码为空"), request.ToString());
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(mobile, "^(13|14|15|17|18)\\d{9}$"))
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "请输入正确的手机号码"), request.ToString());
                }

                string password = param.Password;
                int channel = param.channel;
                int platform = param.platform;
                string ver = param.ver;

                // 保存访问信息
                base.SaveVisitInfo(param.UserId, channel, platform, ver);

                //ThrowParamException(username, password);
                string passwordSalt = Users.GenerateSalt();

                string decryptPassword = password;
                string decryptCode = code;

                if ((platform == 3 && base.AndroidIsEncryption) || (platform == 2 && base.IOSIsEncryption))
                {
                    decryptPassword = base.Decrypt(password, sessionKey, sessionSecret);

                    //decryptCode = base.Decrypt(code, sessionKey, sessionSecret);
                }

                Verify verifyto = new TelVerifyDao().GetVerify(mobile, 2);

                if (!(verifyto != null && verifyto.VerifyCode == decryptCode))
                {
                    return base.JsonFaultResult(new CommonException(40205).GetMessage(), request.ToString());
                }

                string msg = "";

                string sessionId = param.UserId.ToSeesionId();
                int userId = 0;

                Member member = new Member(UserRole.Member);

                if (member.ResetPassword(mobile, decryptPassword, passwordSalt))
                {
                    long timestamp = long.Parse(DateTime.Now.AddSeconds(USER_CACHE_KEEP_TIME).ToString("yyyyMMddHHmmss"));

                    string authenTicket = SecurityUtil.MD5Encrypt(sessionId + timestamp.ToString() + sessionSecret).ToLower();
                    string authenUserId = sessionId.ToLowerGuid();

                    CacheUserSet(authenUserId, authenTicket, timestamp, userId);

                    StandardResult<RegisterOrLoginResult> okResult = new StandardResult<RegisterOrLoginResult>();
                    okResult.code = 0;
                    okResult.msg = "重置密码成功";
                    okResult.data = new RegisterOrLoginResult()
                    {
                        DisplayName = member.Username,
                        AuthenTicket = authenTicket,
                        AuthenUserId = authenUserId,
                        Timestamp = timestamp
                    };

                    return base.JsonActionResult(okResult);
                }

                return base.JsonFaultResult(new FaultInfo(40202, msg), request.ToString());
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }

        [HttpPost]
        public IHttpActionResult TicketRefresh(JObject request)
        {
            Logger.WriterLogger("Account.TicketRefresh, Params: " + request.ToString(), LoggerType.Info);

            ParamTicketRefresh param = new ParamTicketRefresh();

            try
            {
                try
                {
                    param = request.ToObject<ParamTicketRefresh>();
                }
                catch
                {
                    //throw new CommonException(40100);
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }

                string signature = param.Signature;
                string timespan = param.Timestamp;
                string ticket = param.Ticket;
                int channel = param.channel;
                int platform = param.platform;
                string ver = param.ver;

                // 保存访问信息
                base.SaveVisitInfo(param.UserId, channel, platform, ver);

                //ThrowParamException(username, password);

                string v = Utility.SecurityUtil.MD5Encrypt(param.UserId + timespan + sessionSecret);

                if (signature.ToLower() == v.ToLower())
                {
                    long timestamp = long.Parse(DateTime.Now.AddSeconds(USER_CACHE_KEEP_TIME).ToString("yyyyMMddHHmmss"));

                    string authenTicket = SecurityUtil.MD5Encrypt(param.UserId + timestamp.ToString() + sessionSecret).ToLower();
                    string authenUserId = param.UserId;
                    int userId = GetUserId(param.UserId);

                    CacheUserSet(authenUserId, authenTicket, timestamp, userId);

                    string displayName = "";
                    Member member = base.GetMember(param.UserId.ToSeesionId());

                    if (member != null)
                    {
                        displayName = member.Username;
                    }

                    StandardResult<RegisterOrLoginResult> okResult = new StandardResult<RegisterOrLoginResult>();
                    okResult.code = 0;
                    okResult.msg = "登录成功";
                    okResult.data = new RegisterOrLoginResult()
                    {
                        DisplayName = displayName,
                        AuthenTicket = authenTicket,
                        AuthenUserId = authenUserId,
                        Timestamp = timestamp
                    };

                    return base.JsonActionResult(okResult);
                }

                return base.JsonFaultResult(new CommonException(40203).GetMessage(), request.ToString());
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }

        [HttpPost]
        public IHttpActionResult Logout(JObject request)
        {
            Logger.WriterLogger("Account.Logout, Params: " + request.ToString(), LoggerType.Info);

            ParamUserBase param = new ParamUserBase();

            try
            {
                param = request.ToObject<ParamUserBase>();
            }
            catch
            {
                //throw new CommonException(40100);
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            // 保存访问信息
            base.SaveVisitInfo(param.UserId, param.channel, param.platform, param.ver);

            string accessToken = param.accessToken;
            string sessionKey = "";
            string sessionSecret = "";

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string sessionId = param.UserId.ToSeesionId();

            CacheUserRemove(sessionId);

            StandardResult<string> okResult = new StandardResult<string>();
            okResult.code = 0;
            okResult.msg = "成功登出";
            okResult.data = "";

            return base.JsonActionResult(okResult);
        }

        [HttpPost]
        public IHttpActionResult ThirdPartyLogin(JObject request)
        {
            return null;
        }


        [HttpPost]
        public IHttpActionResult ThirdPartyBind(JObject request)
        {
            ParamThirdPartyBind param = new ParamThirdPartyBind();

            try
            {
                try
                {
                    param = request.ToObject<ParamThirdPartyBind>();
                }
                catch
                {
                    //throw new CommonException(40100);
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }

                string openIdType = param.OpenIdType;
                string openId = param.OpenId;

                string nickname = param.Nickname;
                string gender = param.Gender;
                string avatar = param.Avatar;
                string country = param.Country;
                string province = param.Province;
                string city = param.City;

                int channel = param.channel;
                int platform = param.platform;
                string ver = param.ver;

                // 保存访问信息
                base.SaveVisitInfo("", channel, platform, ver);

                //ThrowParamException(username, password);

                //如果用户不存在则先创建用户
                IUser user;
                bool isExists = UserHelper.BindUsersMemberUsersInRoles(openIdType, openId, UserHelper.GenerateSalt(), nickname, avatar, GetProvinceId(province, city, country), new Nullable<int>(), out user);


                Member member = Users.GetUser(0, user.Username, false, true) as Member;

                member.OnLogin();
                string sessionId = member.MembershipUser.SessionId;
                member.SessionId = UserHelper.UpdateSessionId(member.UserId);
                Users.UpdateUser(member);

                long timestamp = long.Parse(DateTime.Now.AddSeconds(USER_CACHE_KEEP_TIME).ToString("yyyyMMddHHmmss"));

                string authenTicket = SecurityUtil.MD5Encrypt(sessionId + timestamp.ToString() + sessionSecret).ToLower();
                string authenUserId = sessionId.ToLowerGuid();

                if ((platform == 2 || platform == 3) && Util.ConvertVer(ver) > 110)
                {
                    if (isExists)
                    {
                        CacheUserSet(authenUserId, authenTicket, timestamp, member.UserId);

                        StandardResult<BindThirdPartyResult> okResult = new StandardResult<BindThirdPartyResult>();
                        okResult.code = 0;
                        okResult.msg = "登录成功";
                        okResult.data = new BindThirdPartyResult()
                        {
                            DisplayName = user.Username,
                            AuthenTicket = authenTicket,
                            AuthenUserId = authenUserId,
                            Timestamp = timestamp,
                            flag = false,
                            openId = openId
                        };

                        return base.JsonActionResult(okResult);
                    }

                    else
                    {
                        StandardResult<BindThirdPartyResult> okResult = new StandardResult<BindThirdPartyResult>();
                        okResult.code = 0;
                        okResult.msg = "进入跳转界面";
                        okResult.data = new BindThirdPartyResult()
                        {
                            DisplayName = user.Username,
                            AuthenTicket = authenTicket,
                            AuthenUserId = authenUserId,
                            Timestamp = timestamp,
                            flag = true,
                            openId = openId
                        };

                        return base.JsonActionResult(okResult);
                    }

                }

                else
                {

                    CacheUserSet(authenUserId, authenTicket, timestamp, member.UserId);

                    StandardResult<RegisterOrLoginResult> okResult = new StandardResult<RegisterOrLoginResult>();
                    okResult.code = 0;
                    okResult.msg = "登录成功";
                    okResult.data = new RegisterOrLoginResult()
                    {
                        DisplayName = user.Username,
                        AuthenTicket = authenTicket,
                        AuthenUserId = authenUserId,
                        Timestamp = timestamp
                    };

                    return base.JsonActionResult(okResult);
                }
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }

        [HttpPost]
        public IHttpActionResult ThirdAccountBindOperate(JObject request)
        {
            ThirdAccountBind param = new ThirdAccountBind();

            try
            {
                try
                {
                    param = request.ToObject<ThirdAccountBind>();
                }
                catch
                {
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }

                string openIdType = param.OpenIdType;
                string openId = param.OpenId;

                string cellPhone = param.CellPhone;
                string code = param.Code;
                string password = param.Password;
                string ver = param.ver;
                string recemmendCode = param.RecemmendCode;

                StandardResult<RegisterOrLoginResult> okResult = new StandardResult<RegisterOrLoginResult>();
                // 验证手机号码是否存在
                if (UserHelper.IsExistUserName(cellPhone))
                {
                    okResult.code = 3;
                    okResult.msg = "该手机号码已经存在，不能再绑定";
                    return base.JsonActionResult(okResult);
                }
                // 验证码验证
                Verify verifyto = new TelVerifyDao().GetVerify(cellPhone, 1);
                if (!(verifyto != null && verifyto.VerifyCode == code))
                {
                    okResult.code = 4;
                    okResult.msg = "验证码验证失败";
                    return base.JsonActionResult(okResult);
                }
                // 获取当前注册用户
                //this.openId = this.Page.Request.QueryString["sessionId"];
                Member member = null;
                if (!string.IsNullOrEmpty(openId))
                {
                    member = (Users.GetUserByOpenId(openId) as Member);
                }

                //判断邀请码是否存在
                if (!string.IsNullOrWhiteSpace(recemmendCode))
                {
                    if (!MemberHelper.IsExsitRecommendCode(recemmendCode.Trim().ToUpper(), member.UserId))
                    {
                        okResult.code = 5;
                        okResult.msg = "邀请码错误，请填写正确的邀请码或者选择不填";
                        return base.JsonActionResult(okResult);
                    }
                     
                }


                member.Username = cellPhone;
                member.Password = password;
                member.CellPhone = cellPhone;
                bool isSendCoupon = false;
                if (MemberHelper.UpdateUserNameCoupon(member, recemmendCode.Trim().ToUpper(), out isSendCoupon))
                {
                    okResult.code = 1;
                    if (isSendCoupon)
                    {
                        okResult.msg = "绑定成功，恭喜您，50元现金券已经打到您的海美账户了，现在就去购物吧！";
                    }
                    else
                    {
                        okResult.msg = "绑定成功";
                    }
                    
                }
                else
                {
                    okResult.code = 2;
                    okResult.msg = "绑定失败";
                }

                return base.JsonActionResult(okResult);
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }

        #region Private
        private LoginUserStatus ValidateUser(string username, string password, out string sessionId, out int userId, out string passwordSalt)
        {
            userId = 0;
            passwordSalt = "";
            sessionId = "";

            LoginUserStatus result;
            DataTable dtUser = memberDao.GetUserByUsername(username);

            if (dtUser == null)
            {
                result = LoginUserStatus.UnknownError;
            }
            else
            {
                bool isApproved = (bool)dtUser.Rows[0]["IsApproved"];

                if (!isApproved)
                {
                    result = LoginUserStatus.AccountPending;
                }
                else
                {
                    bool isLockedOut = (bool)dtUser.Rows[0]["IsLockedOut"];

                    if (isLockedOut)
                    {
                        result = LoginUserStatus.AccountLockedOut;
                    }
                    else
                    {
                        string dbPassword = (string)dtUser.Rows[0]["Password"];
                        passwordSalt = (string)dtUser.Rows[0]["PasswordSalt"];
                        int passwordFormat = (int)dtUser.Rows[0]["PasswordFormat"];
                        string encodePassword = UserHelper.EncodePassword(System.Web.Security.MembershipPasswordFormat.Hashed, password, passwordSalt);
                        if (encodePassword != dbPassword)
                        {
                            result = LoginUserStatus.InvalidCredentials;
                        }
                        else
                        {
                            sessionId = dtUser.Rows[0]["SessionId"].ToString();
                            userId = (int)dtUser.Rows[0]["UserId"];

                            result = LoginUserStatus.Success;
                        }
                    }
                }
            }
            return result;
        }

        private long GetProvinceId(string province, string city, string country)
        {
            long ip = 0L;
            if (!string.IsNullOrEmpty(country) || !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(province))
            {
                ip = RegionHelper.GetRegionId(country, city, province);
            }

            return ip;
        }

        /// <summary>
        /// 根据platform获取注册用户类型
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        private UserType CovertToUserType(int platform)
        {
            UserType userType = UserType.PC;
            switch (platform)
            {
                case 1:
                    userType = UserType.PC;
                    break;
                case 2:
                    userType = UserType.IOS;
                    break;
                case 3:
                    userType = UserType.Android;
                    break;
                case 4:
                    userType = UserType.Wap;
                    break;
                default:
                    userType = UserType.PC;
                    break;
            }

            return userType;
        }
        #endregion
    }
}
