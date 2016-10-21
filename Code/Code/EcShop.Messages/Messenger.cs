using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Entities.Orders;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SqlDal.Store;
using Ecdev.Plugins;
using Ecdev.Weixin.MP.Api;
using Ecdev.Weixin.MP.Domain;
using System;
using System.Net.Mail;
using System.Text;
using Ecdev.Plugins.Integration;
using Ecdev.Plugins.Integration.Xinge;
using System.Collections.Generic;
using EcShop.Core.ErrorLog;

namespace EcShop.Messages
{
    public static class Messenger
    {
        internal static bool SendMail(System.Net.Mail.MailMessage email, EmailSender sender)
        {
            string text;
            return Messenger.SendMail(email, sender, out text);
        }
        internal static bool SendMail(System.Net.Mail.MailMessage email, EmailSender sender, out string msg)
        {
            bool result;
            try
            {
                msg = "";
                result = sender.Send(email, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding));
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 发送销售情况
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user"></param>
        public static bool OrderSaleRpt(string body, string Email)
        {
            SiteSettings siteSettings = SettingsManager.GetMasterSettings(true);
            string text;
            SendStatus sendStatus = Messenger.SendMail("销售情况统计", body, Email, siteSettings, out text);
            if (sendStatus == SendStatus.NoProvider || sendStatus == SendStatus.ConfigError)
            {
                return false;
            }
            if (sendStatus == SendStatus.Fail)
            {
                return false;
            }
            if (sendStatus == SendStatus.Success)
            {
                return true;
            }
            return false;
        }
        public static SendStatus SendMail(string subject, string body, string emailTo, SiteSettings settings, out string msg)
        {
            msg = "";
            SendStatus result;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body) || string.IsNullOrEmpty(emailTo) || subject.Trim().Length == 0 || body.Trim().Length == 0 || emailTo.Trim().Length == 0)
            {
                result = SendStatus.RequireMsg;
            }
            else
            {
                if (settings == null || !settings.EmailEnabled)
                {
                    result = SendStatus.NoProvider;
                }
                else
                {
                    EmailSender emailSender = Messenger.CreateEmailSender(settings, out msg);
                    if (emailSender == null)
                    {
                        result = SendStatus.ConfigError;
                    }
                    else
                    {
                        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
                        {
                            IsBodyHtml = true,
                            Priority = System.Net.Mail.MailPriority.High,
                            Body = body.Trim(),
                            Subject = subject.Trim()
                        };
                        mailMessage.To.Add(emailTo);
                        result = (Messenger.SendMail(mailMessage, emailSender, out msg) ? SendStatus.Success : SendStatus.Fail);
                    }
                }
            }
            return result;
        }
        public static SendStatus SendMail(string subject, string body, string[] cc, string[] bcc, SiteSettings settings, out string msg)
        {
            msg = "";
            SendStatus result;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body) || subject.Trim().Length == 0 || body.Trim().Length == 0 || ((cc == null || cc.Length == 0) && (bcc == null || bcc.Length == 0)))
            {
                result = SendStatus.RequireMsg;
            }
            else
            {
                if (settings == null || !settings.EmailEnabled)
                {
                    result = SendStatus.NoProvider;
                }
                else
                {
                    EmailSender emailSender = Messenger.CreateEmailSender(settings, out msg);
                    if (emailSender == null)
                    {
                        result = SendStatus.ConfigError;
                    }
                    else
                    {
                        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
                        {
                            IsBodyHtml = true,
                            Priority = System.Net.Mail.MailPriority.High,
                            Body = body.Trim(),
                            Subject = subject.Trim()
                        };
                        if (cc != null && cc.Length > 0)
                        {
                            for (int i = 0; i < cc.Length; i++)
                            {
                                string addresses = cc[i];
                                mailMessage.CC.Add(addresses);
                            }
                        }
                        if (bcc != null && bcc.Length > 0)
                        {
                            for (int i = 0; i < bcc.Length; i++)
                            {
                                string addresses = bcc[i];
                                mailMessage.Bcc.Add(addresses);
                            }
                        }
                        result = (Messenger.SendMail(mailMessage, emailSender, out msg) ? SendStatus.Success : SendStatus.Fail);
                    }
                }
            }
            return result;
        }
        internal static EmailSender CreateEmailSender(SiteSettings settings)
        {
            string text;
            return Messenger.CreateEmailSender(settings, out text);
        }
        internal static EmailSender CreateEmailSender(SiteSettings settings, out string msg)
        {
            EmailSender result;
            try
            {
                msg = "";
                if (!settings.EmailEnabled)
                {
                    result = null;
                }
                else
                {
                    result = EmailSender.CreateInstance(settings.EmailSender, HiCryptographer.Decrypt(settings.EmailSettings));
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                result = null;
            }
            return result;
        }
        public static SendStatus SendSMS(string phoneNumber, string message, SiteSettings settings, out string msg)
        {
            msg = "";
            SendStatus result;
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message) || phoneNumber.Trim().Length == 0 || message.Trim().Length == 0)
            {
                result = SendStatus.RequireMsg;
            }
            else
            {
                if (settings == null || !settings.SMSEnabled)
                {
                    result = SendStatus.NoProvider;
                }
                else
                {
                    SMSSender sMSSender = Messenger.CreateSMSSender(settings, out msg);
                    if (sMSSender == null)
                    {
                        result = SendStatus.ConfigError;
                    }
                    else
                    {
                        result = (sMSSender.Send(phoneNumber, message, out msg) ? SendStatus.Success : SendStatus.Fail);
                    }
                }
            }
            return result;
        }


        public static SendStatus SendSMS(string phoneNumber, string message, SiteSettings settings,int type, out string msg)
        {
            msg = "";
            SendStatus result;
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message) || phoneNumber.Trim().Length == 0 || message.Trim().Length == 0)
            {
                result = SendStatus.RequireMsg;
            }
            else
            {
                if (settings == null || !settings.SMSEnabled)
                {
                    result = SendStatus.NoProvider;
                }
                else
                {
                    SMSSender sMSSender = Messenger.CreateSMSSender(settings, out msg);
                    if (sMSSender == null)
                    {
                        result = SendStatus.ConfigError;
                    }
                    else
                    {
                        result = (sMSSender.Send(phoneNumber, message,type, out msg) ? SendStatus.Success : SendStatus.Fail);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 发送信鸽消息
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="template"></param>
        /// <param name="deviceToken">用户apptoken</param>
        /// <param name="facilityType">app类型 1：android 2：IOS</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static SendStatus SendXinge(IUser user, SiteSettings settings, EcShop.Entities.Store.MessageTemplate template, int facilityType, XinGeMsgType messageType, object msgContent, out string msg, decimal amount = 0)
        {
            msg = "";
            SendStatus result;
            if (settings == null || settings.XinGeEnabled == false || template == null || string.IsNullOrWhiteSpace(template.XinGeBody) || string.IsNullOrWhiteSpace(template.XinGeSubject))
            {
                result = SendStatus.ConfigError;
                return result;
            }
            else
            {
                XingePush xingePush = Messenger.CreateXingePush(settings, out msg);
                if (xingePush == null)
                {
                    result = SendStatus.ConfigError;
                }
                else
                {
                    MessageIOS messageIOS = null;
                    Message messageAndroid = null;
                    Ret ret = null;

                    string content = template.XinGeBody;

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        content = "";
                    }

                    #region

                    switch (messageType)
                    {
                        case XinGeMsgType.ChangeLoginPassword:
                        case XinGeMsgType.ChangeTradePassword:
                        case XinGeMsgType.FindLoginPassword:
                            string[] obj = msgContent as string[];
                            content = content.Replace("$SiteName$", settings.SiteName.Trim());
                            content = content.Replace("$Username$", obj[0]);
                            content = content.Replace("$Email$", obj[1]);
                            content = content.Replace("$Password$", obj[2]);
                            content = content.Replace("$DealPassword$", obj[3]);
                            break;
                        case XinGeMsgType.OrderClose:
                        case XinGeMsgType.OrderCreate:
                        case XinGeMsgType.OrderPay:
                        case XinGeMsgType.OrderRefund:
                        case XinGeMsgType.OrderShipments:
                            OrderInfo order = msgContent as OrderInfo;
                            content = content.Replace("$SiteName$", settings.SiteName.Trim());
                            content = content.Replace("$Username$", order.Username);
                            content = content.Replace("$OrderId$", order.OrderId);
                            content = content.Replace("$Total$", order.GetTotal().ToString("F"));
                            content = content.Replace("$Memo$", order.Remark);
                            content = content.Replace("$Shipping_Type$", order.ModeName);
                            content = content.Replace("$Shipping_Name$", order.ShipTo);
                            content = content.Replace("$Shipping_Addr$", order.Address);
                            content = content.Replace("$Shipping_Zip$", order.ZipCode);
                            content = content.Replace("$Shipping_Phone$", order.TelPhone);
                            content = content.Replace("$Shipping_Cell$", order.CellPhone);
                            content = content.Replace("$Shipping_Email$", order.EmailAddress);
                            content = content.Replace("$Shipping_Billno$", order.ShipOrderNumber);
                            if (amount > 0)
                            {
                                //content = content.Replace("$RefundMoney$", order.RefundAmount.ToString("F"));
                                content = content.Replace("$RefundMoney$", amount.ToString("F"));
                            }
                            content = content.Replace("$CloseReason$", order.CloseReason);
                            break;
                        case XinGeMsgType.RegistUser:
                            break;
                    }
                    #endregion

                    #region
                    MessageT_TypeEnum type;
                    MessageTypeEnum action;
                    string title = template.XinGeSubject;

                    switch (template.MessageType)
                    {
                        case "ChangedDealPassword":
                            action = MessageTypeEnum.ChangedDealPassword;
                            type = MessageT_TypeEnum.UserType;
                            //title = "会员修改交易密码通知";
                            break;
                        case "ChangedPassword":
                            action = MessageTypeEnum.ChangedPassword;
                            type = MessageT_TypeEnum.UserType;
                            //title = "会员修改登录密码通知";
                            break;
                        case "ForgottenPassword":
                            action = MessageTypeEnum.ForgottenPassword;
                            type = MessageT_TypeEnum.UserType;
                            //title = "会员找回登录密码通知";
                            break;
                        case "NewUserAccountCreated":
                            action = MessageTypeEnum.NewUserAccountCreated;
                            type = MessageT_TypeEnum.UserType;
                            //title = "会员注册通知";
                            break;
                        case "OrderClosed":
                            action = MessageTypeEnum.OrderClosed;
                            type = MessageT_TypeEnum.OrderType;
                           //title = "订单已关闭";
                            break;
                        case "OrderCreated":
                            action = MessageTypeEnum.OrderCreated;
                            type = MessageT_TypeEnum.OrderType;
                            //title = "订单已创建";
                            break;
                        case "OrderPayment":
                            action = MessageTypeEnum.OrderPayment;
                            type = MessageT_TypeEnum.OrderType;
                            //title = "订单已支付";
                            break;
                        case "OrderRefund":
                            action = MessageTypeEnum.OrderRefund;
                            type = MessageT_TypeEnum.OrderType;
                            //title = "订单已退款";
                            break;
                        case "OrderShipping":
                            action = MessageTypeEnum.OrderShipping;
                            type = MessageT_TypeEnum.OrderType;
                            //title = "订单已发货";
                            break;
                        default:
                            action = MessageTypeEnum.NoType;
                            type = MessageT_TypeEnum.NoType;
                            //title = "通知";
                            break;
                    }

                    int contentId = InsertMessageContent(title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)type, type == MessageT_TypeEnum.OrderType ? (msgContent as OrderInfo).OrderId : "0", (int)action);
                    int messageId = InsertMessageBox(contentId, "系统", user.Username, user.MembershipUser.UserId, 1);

                    Dictionary<string, object> customContent = new Dictionary<string, object>();
                    customContent.Add("t",  (int)type);
                    customContent.Add("a", (int)action);
                    customContent.Add("id", type == MessageT_TypeEnum.OrderType?(msgContent as OrderInfo).OrderId:"0");
                    customContent.Add("cid", messageId);

                    #endregion

                    /*发送信鸽消息*/
                    //判断为IOS

                    messageIOS = new MessageIOS();
                    messageIOS.ExpireTime = 86400;
                    messageIOS.AlertStr = content;
                    messageIOS.Badge = 1;
                    messageIOS.Sound = "beep.wav";
                    AcceptTime acceptTime = new AcceptTime(0, 0, 23, 59);
                    messageIOS.AcceptTimes.Add(acceptTime);
                    messageIOS.CustomContent = customContent;

                    try
                    {
                        if(user == null)
                        {
                           ErrorLog.Write("SendXinge:->USer为空");
                        }
                        else
                        {
                            if(user.MembershipUser == null)
                            {
                               ErrorLog.Write("SendXinge:->user.MembershipUser为空");
                            }
                            else
                            {
                                if(string.IsNullOrEmpty(user.MembershipUser.SessionId))
                                {
                                    ErrorLog.Write("SendXinge:->user.MembershipUser.SessionId为空");   
                                }
                            }
                        }

                        string account = user.MembershipUser.SessionId.Replace("-", "").ToLower();

                        //推送消息给单个账号
                        ErrorLog.Write("Messenger.cs:SendXinGe()->Before- AccountId:" + account + ",content:" + content + ",t:" + (int)type + ",a:" + (int)action + ",id:" + (type == MessageT_TypeEnum.OrderType ? (msgContent as OrderInfo).OrderId : "0") + ",cid:" + contentId);

                        ret = xingePush.PushSingleAccount(0, account, messageIOS, XingeConfig.IOSENV_PROD);
                        ErrorLog.Write("Messenger.cs:SendXinGe()->IOS Send- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);

                        messageAndroid = new Message();
                        messageAndroid.MessageType = Ecdev.Plugins.Integration.Xinge.MessageType.TYPE_NOTIFICATION;
                        messageAndroid.Title = title;
                        messageAndroid.Content = content;
                        messageAndroid.CustomContent = customContent;

                        ret = xingePush.PushSingleAccount(0, account, messageAndroid);
                        ErrorLog.Write("Messenger.cs:SendXinGe()->Android Send- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);
                    
                        if (ret == null || ret.ret_code != 0)
                        {
                            return result = SendStatus.Success;
                        }
                        else
                        {
                            msg = ret.ret_code + ":" + ret.err_msg;
                            return SendStatus.NoProvider;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Write("SendXinge:->ex:"+ex.Message);   
                    }
                }
            }
            return result = SendStatus.NoProvider;
        }

        public static void SendXinge(string[] users, string content, string title, MessageT_TypeEnum type, int action, int messageId)
        {
            SiteSettings settings = HiContext.Current.SiteSettings;
            string msg;
            if (settings == null || settings.XinGeEnabled == false)
            {
                return;
            }
            else
            {
                if (users == null || users.Length == 0)
                {
                    return;
                }

                XingePush xingePush = Messenger.CreateXingePush(settings, out msg);
                if (xingePush == null)
                {
                    return;
                }
                else
                {
                    /*
                    // result = (sMSSender.Send(phoneNumber, message, out msg) ? SendStatus.Success : SendStatus.Fail);
                    Payload pl = null;//= new Payload("这是一个简单的alert");
                    Msg_IOS mios = null;//= new Msg_IOS(pl);
                    Msg_Android mandroid = null;
                    int contentId = InsertMessageContent(title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)type, messageId.ToString(), (int)action);
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("t", (int)type);
                    dic.Add("a", (int)action);
                    dic.Add("id", messageId);
                    dic.Add("cid", contentId);
                    pl = new Payload(title + ":" + content);
                    mios = new Msg_IOS(pl);
                    mios.custom_content = dic;
                    mandroid = new Msg_Android_TouChuan(title, XinGeConfig.message_type_info)
                    {
                        content = content,
                        custom_content = dic
                    };
                    List<string> list = new List<string>(users);
                    //foreach (string user in users)
                    //{
                    //    list.Add(user);
                    //}

                    ErrorLog.Write("Messenger.cs:SendXinGe()->Before- AccountIds:" + string.Join(",", users) + ",content:" + content + ",T:" + (int)type + ",A:" + (int)action + ",CId:" + contentId + ",MessageId:" + messageId);
                    //推送消息给多个账号
                    //信鸽推送一次推送账号小于100，判断信鸽推送消息数量 大于100 分批发送
                    if (list.Count > 100)
                    {
                        int count = list.Count % 100 == 0 ? list.Count / 100 : list.Count / 100 + 1;
                        int i = 0;
                        while (i < count)
                        {
                            Ret ret = xingePush.PushAccountList(list.GetRange(i * 100, 100), mios, XinGeConfig.IOSENV_PROD);
                            if (i == 0) ErrorLog.Write("Messenger.cs:SendXinGe()->IOS- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);
                            ret = xingePush.PushAccountList(list.GetRange(i * 100, 100), mandroid);
                            if (i == 0) ErrorLog.Write("Messenger.cs:SendXinGe()->Android- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);
                            i++;
                        }

                    }
                    else
                    {
                        Ret ret = xingePush.PushAccountList(list, mios, XinGeConfig.IOSENV_PROD);
                        ErrorLog.Write("Messenger.cs:SendXinGe()->IOS- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);
                        ret = xingePush.PushAccountList(list, mandroid);
                        ErrorLog.Write("Messenger.cs:SendXinGe()->Android- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);
                    }
                    */
                }
            }
        }
        private static XingePush CreateXingePush(SiteSettings settings, out string msg)
        {
            XingePush result;
            try
            {
                msg = "";
                if (!settings.XinGeEnabled)
                {
                    result = null;
                }
                else
                {
                    result = XingePush.CreateInstance(settings.XinGeSender, HiCryptographer.Decrypt(settings.XinGeSettings));
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                result = null;
            }
            return result;
        }


        /// <summary>
        /// 关联id推送信鸽
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public static void SendXinGeByRelateIds(string[] ids, string content, string title, int type, int action, int env)
        {
            ErrorLog.Write("Messenger.cs:SendXinGe()->Before- All:" + string.Join(",", ids) + ",content:" + content + ",t:" + type + ",a:" + action);

            SiteSettings settings = HiContext.Current.SiteSettings;
            string msg;
            if (settings == null || settings.XinGeEnabled == false)
            {
                return;
            }
            else
            {
                if (ids == null || ids.Length == 0)
                {
                    return;
                }
                XingePush xingePush = Messenger.CreateXingePush(settings, out msg);
                if (xingePush == null)
                {
                    return;
                }
                else
                {
                    foreach (string id in ids)
                    {
                        MessageIOS messageIOS = null;
                        Message messageAndroid = null;
                        Ret ret = null;

                        Dictionary<string, object> customContent = new Dictionary<string, object>();
                        customContent.Add("t", type);
                        customContent.Add("a", action);
                        customContent.Add("id", id);

                        //判断为IOS
                        messageIOS = new MessageIOS();
                        messageIOS.ExpireTime = 86400;
                        messageIOS.AlertStr = content;
                        messageIOS.Badge = 1;
                        messageIOS.Sound = "beep.wav";
                        AcceptTime acceptTime = new AcceptTime(0, 0, 23, 59);
                        messageIOS.AcceptTimes.Add(acceptTime);
                        messageIOS.CustomContent = customContent;

                        //判断为Android
                        messageAndroid = new Message();
                        messageAndroid.MessageType = Ecdev.Plugins.Integration.Xinge.MessageType.TYPE_NOTIFICATION;
                        messageAndroid.Title = title;
                        messageAndroid.Content = content;
                        messageAndroid.CustomContent = customContent;

                        //推送消息所有设备
                        ret = xingePush.PushAllDevice(0, messageIOS, (env == 1 ? XingeConfig.IOSENV_PROD : XingeConfig.IOSENV_DEV));
                        //ret = xingePush.PushSingleAccount(0, "f1aeeb188b9649e1beb7946a14cd4e34", messageIOS, XingeConfig.IOSENV_DEV);
                        ErrorLog.Write("Messenger.cs:SendXinGe()->iOS Send- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);

                        ret = xingePush.PushAllDevice(0, messageAndroid);
                        //List<string> accounts = new List<string>();
                        //accounts.Add("3c711c6c766740af95e56e39ed08e6f7");
                        //accounts.Add("ffc58b16a0c6435984c0ea749194aebf");
                        //ret = xingePush.PushAccountList(0, accounts, messageAndroid);
                        ErrorLog.Write("Messenger.cs:SendXinGe()->Android Send- ret_code:" + ret.ret_code + ",err_msg:" + ret.err_msg);
                    }
                }
            }
        }

        /// <summary>
        /// 将信鸽消息保存至数据库并返回保存的ContentId
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="createTime"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static int InsertMessageContent(string title, string content, string createTime, int contentType, string code, int actionId)
        {
            return new MessageDao().InsertMessageContent(title, content, createTime, contentType, code, actionId);
        }
        public static int InsertMessageBox(int contentId, string sernder, string accepter, int userId, int messageType)
        {
            return new MessageDao().InsertMessageBox(contentId, sernder, accepter, userId, messageType);
        }
        public static SendStatus SendSMS(string[] phoneNumbers, string message, SiteSettings settings, out string msg)
        {
            msg = "";
            SendStatus result;
            if (phoneNumbers == null || string.IsNullOrEmpty(message) || phoneNumbers.Length == 0 || message.Trim().Length == 0)
            {
                result = SendStatus.RequireMsg;
            }
            else
            {
                if (settings == null || !settings.SMSEnabled)
                {
                    result = SendStatus.NoProvider;
                }
                else
                {
                    SMSSender sMSSender = Messenger.CreateSMSSender(settings, out msg);
                    if (sMSSender == null)
                    {
                        result = SendStatus.ConfigError;
                    }
                    else
                    {
                        result = (sMSSender.Send(phoneNumbers, message, out msg) ? SendStatus.Success : SendStatus.Fail);
                    }
                }
            }
            return result;
        }
        internal static SMSSender CreateSMSSender(SiteSettings settings)
        {
            string text;
            return Messenger.CreateSMSSender(settings, out text);
        }
        internal static SMSSender CreateSMSSender(SiteSettings settings, out string msg)
        {
            SMSSender result;
            try
            {
                msg = "";
                if (!settings.SMSEnabled)
                {
                    result = null;
                }
                else
                {
                    result = SMSSender.CreateInstance(settings.SMSSender, HiCryptographer.Decrypt(settings.SMSSettings));
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                result = null;
            }
            return result;
        }
        public static SendStatus SendInnerMessage(SiteSettings settings, string subject, string message, string sendto)
        {
            SendStatus result;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message) || subject.Trim().Length == 0 || message.Trim().Length == 0)
            {
                result = SendStatus.RequireMsg;
            }
            else
            {
                if (settings == null)
                {
                    result = SendStatus.NoProvider;
                }
                else
                {
                    result = (new MessageDao().SendMessage(subject, message, sendto) ? SendStatus.Success : SendStatus.Fail);
                }
            }
            return result;
        }
        private static TemplateMessage GenerateWeixinMessageWhenOrderCreate(string templateId, SiteSettings settings, EcShop.Entities.Orders.OrderInfo order, IUser user)
        {
            TemplateMessage result = null;
            if (!string.IsNullOrWhiteSpace(user.OpenId))
            {
                string weixinToken = settings.WeixinToken;
                result = new TemplateMessage
                {
                    Url = settings.SiteUrl + "/Vshop/MemberOrderDetails.aspx?OrderId=" + order.OrderId,
                    TemplateId = templateId,
                    Touser = user.OpenId,
                    Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = "您的订单已提交成功!"
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderID",
						Value = order.OrderId,
                        
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderMoneySum",
							Color = "#ff3300",
							Value = "￥" + order.GetTotal().ToString("F2")
						}     
					}
                };
            }
            return result;
        }
        private static TemplateMessage GenerateWeixinMessageWhenPasswordChange(string templateId, SiteSettings settings, IUser user, string passowordType, string newpassword)
        {
            TemplateMessage result;
            if (string.IsNullOrWhiteSpace(user.OpenId))
            {
                result = null;
            }
            else
            {
                string weixinToken = settings.WeixinToken;
                TemplateMessage templateMessage = new TemplateMessage
                {
                    Url = "",
                    TemplateId = templateId,
                    Touser = user.OpenId,
                    Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = "亲爱的会员,您的" + passowordType + "密码修改成功!"
						},
						new TemplateMessage.MessagePart
						{
							Name = "keyword1",
							Value = user.Username
						},
						new TemplateMessage.MessagePart
						{
							Name = "keyword2",
							Value = newpassword
						},
						new TemplateMessage.MessagePart
						{
							Name = "remark",
							Value = ""
						}
					}
                };
                result = templateMessage;
            }
            return result;
        }
        private static TemplateMessage GenerateWeixinMessageWhenFindPassword(string templateId, SiteSettings settings, IUser user, string password)
        {
            TemplateMessage result;
            if (string.IsNullOrWhiteSpace(user.OpenId))
            {
                result = null;
            }
            else
            {
                string weixinToken = settings.WeixinToken;
                TemplateMessage templateMessage = new TemplateMessage
                {
                    Url = "",
                    TemplateId = templateId,
                    Touser = user.OpenId,
                    Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = "您好,您的账号信息如下"
						},
						new TemplateMessage.MessagePart
						{
							Name = "keyword1",
							Value = user.Username
						},
						new TemplateMessage.MessagePart
						{
							Name = "keyword2",
							Value = password
						},
						new TemplateMessage.MessagePart
						{
							Name = "remark",
							Value = "请妥善保管。"
						}
					}
                };
                result = templateMessage;
            }
            return result;
        }
        private static TemplateMessage GenerateWeixinMessageWhenOrderClose(string templateId, SiteSettings settings, IUser user, EcShop.Entities.Orders.OrderInfo order, string reason)
        {
            TemplateMessage result;
            if (string.IsNullOrWhiteSpace(user.OpenId))
            {
                result = null;
            }
            else
            {
                string weixinToken = settings.WeixinToken;
                string value = "";
                if (order.OrderStatus == EcShop.Entities.Orders.OrderStatus.Finished || order.OrderStatus == EcShop.Entities.Orders.OrderStatus.SellerAlreadySent || (order.OrderStatus == EcShop.Entities.Orders.OrderStatus.WaitBuyerPay && order.Gateway == "ecdev.plugins.payment.podrequest") || order.OrderStatus == EcShop.Entities.Orders.OrderStatus.BuyerAlreadyPaid || order.OrderStatus == EcShop.Entities.Orders.OrderStatus.Refunded)
                {
                    value = order.PayDate.ToString("M月d日 HH:mm:ss");
                }
                TemplateMessage templateMessage = new TemplateMessage
                {
                    Url = "",
                    TemplateId = templateId,
                    Touser = user.OpenId,
                    Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = "亲,您的订单已关闭，请核对"
						},
						new TemplateMessage.MessagePart
						{
							Name = "transid",
							Value = order.OrderId
						},
						new TemplateMessage.MessagePart
						{
							Name = "fee",
							Color = "#ff3300",
							Value = "￥" + order.GetTotal().ToString("F2")
						},
						new TemplateMessage.MessagePart
						{
							Name = "pay_time",
							Value = value
						},
						new TemplateMessage.MessagePart
						{
							Name = "remark",
							Color = "#000000",
							Value = "关闭原因：" + reason
						}
					}
                };
                result = templateMessage;
            }
            return result;
        }
        private static TemplateMessage GenerateWeixinMessageWhenOrderPay(string templateId, SiteSettings settings, IUser user, EcShop.Entities.Orders.OrderInfo order, decimal fee)
        {
            TemplateMessage result;
            if (string.IsNullOrWhiteSpace(user.OpenId))
            {
                result = null;
            }
            else
            {
                string text = "";
                foreach (EcShop.Entities.Orders.LineItemInfo current in order.LineItems.Values)
                {
                    text = text + ((text == "") ? "" : ",") + current.ItemDescription;
                }
                string weixinToken = settings.WeixinToken;
                TemplateMessage templateMessage = new TemplateMessage
                {
                    Url = "",
                    TemplateId = templateId,
                    Touser = user.OpenId,
                    Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = "亲,您的订单" + order.OrderId + "支付成功,我们将于3-5个工作日为您发货"
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderMoneySum",
							Color = "#ff3300",
							Value = "￥" + fee.ToString("F2")
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderProductName",
							Value = text
						},
						new TemplateMessage.MessagePart
						{
							Name = "remark",
							Value = ""
						}
					}
                };
                result = templateMessage;
            }
            return result;
        }
        private static TemplateMessage GenerateWeixinMessageWhenOrderRefund(string templateId, SiteSettings settings, IUser user, EcShop.Entities.Orders.OrderInfo order, decimal amount)
        {
            TemplateMessage result;
            if (string.IsNullOrWhiteSpace(user.OpenId) || order == null)
            {
                result = null;
            }
            else
            {
                string weixinToken = settings.WeixinToken;
                string text = "";
                foreach (EcShop.Entities.Orders.LineItemInfo current in order.LineItems.Values)
                {
                    text = text + ((text == "") ? "" : ",") + current.ItemDescription;
                }
                TemplateMessage templateMessage = new TemplateMessage
                {
                    Url = "",
                    TemplateId = templateId,
                    Touser = user.OpenId,
                    Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = "亲,您的订单号为" + order.OrderId + "的订单已经退款"
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderProductPrice",
							Color = "#ff3300",
							Value = "￥" + amount.ToString("F2")
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderProductName",
							Value = text
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderName",
							Value = order.OrderId
						},
						new TemplateMessage.MessagePart
						{
							Name = "remark",
							Value = ""
						}
					}
                };
                result = templateMessage;
            }
            return result;
        }
        private static TemplateMessage GenerateWeixinMessageWhenOrderSend(string templateId, SiteSettings settings, IUser user, EcShop.Entities.Orders.OrderInfo order)
        {
            TemplateMessage result;
            if (string.IsNullOrWhiteSpace(user.OpenId))
            {
                result = null;
            }
            else
            {
                string weixinToken = settings.WeixinToken;
                string text = "";
                foreach (EcShop.Entities.Orders.LineItemInfo current in order.LineItems.Values)
                {
                    text = text + ((text == "") ? "" : ",") + current.ItemDescription;
                }
                TemplateMessage templateMessage = new TemplateMessage
                {
                    Url = "",
                    TemplateId = templateId,
                    Touser = user.OpenId,
                    Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = "亲,您的订单号 " + order.OrderId + " 已经发货"
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderProductPrice",
							Value = order.GetAmount().ToString("f2")
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderProductName",
							Value = text
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderAddress",
							Value = order.ShippingRegion + "-" + order.Address
						},
						new TemplateMessage.MessagePart
						{
							Name = "orderName",
							Value = order.OrderId
						},
						new TemplateMessage.MessagePart
						{
							Name = "remark",
							Value = "快递公司：" + order.ExpressCompanyName + "  物流编号:" + order.ShipOrderNumber
						}
					}
                };
                result = templateMessage;
            }
            return result;
        }
        public static void UserRegister(IUser user, string createPassword)
        {
            if (user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("NewUserAccountCreated");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, createPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    Messenger.Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage, null);
                }
            }
        }
        public static void UserPasswordChanged(IUser user, string changedPassword)
        {
            if (user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedPassword");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, changedPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenPasswordChange(template.WeixinTemplateId, siteSettings, user, "登录", changedPassword);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.ChangeLoginPassword, new string[] { user.Username, user.Email, changedPassword, "" }, out msg);
                    Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        public static void UserPasswordForgotten(IUser user, string resetPassword)
        {
            if (user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("ForgottenPassword");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, resetPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenFindPassword(template.WeixinTemplateId, siteSettings, user, resetPassword);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.FindLoginPassword, new string[] { user.Username, user.Email, resetPassword, "" }, out msg);
                    Messenger.Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        public static void UserDealPasswordChanged(IUser user, string changedDealPassword)
        {
            if (user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedDealPassword");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, null, changedDealPassword, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenPasswordChange(template.WeixinTemplateId, siteSettings, user, "交易", changedDealPassword);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.ChangeTradePassword, new string[] { user.Username, user.Email, "", changedDealPassword }, out msg);
                    Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        public static void OrderCreated(EcShop.Entities.Orders.OrderInfo order, IUser user)
        {
            if (order != null && user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderCreated");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, order.GetTotal(), order.Remark, order.ModeName, order.ShipTo, order.Address, order.ZipCode, order.TelPhone, order.CellPhone, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderCreate(template.WeixinTemplateId, siteSettings, order, user);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.OrderCreate, order, out msg);
                    Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        public static void OrderPayment(IUser user, EcShop.Entities.Orders.OrderInfo order, decimal amount)
        {
            if (user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderPayment");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, amount, null, null, null, null, null, null, null, null, null, 0m, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderPay(template.WeixinTemplateId, siteSettings, user, order, amount);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.OrderPay, order, out msg);
                    Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        public static void OrderShipping(EcShop.Entities.Orders.OrderInfo order, IUser user)
        {
            if (order != null && user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderShipping");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, order.GetTotal(), order.Remark, order.RealModeName, order.ShipTo, order.Address, order.ZipCode, order.TelPhone, order.CellPhone, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason,order.ExpressCompanyName, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderSend(template.WeixinTemplateId, siteSettings, user, order);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.OrderShipments, order, out msg);
                    Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        public static void OrderRefund(IUser user, EcShop.Entities.Orders.OrderInfo order, decimal amount)
        {
            if (user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderRefund");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, 0m, null, null, null, null, null, null, null, null, null, amount, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderRefund(template.WeixinTemplateId, siteSettings, user, order, amount);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.OrderRefund, order, out msg, amount);
                    Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        public static void OrderClosed(IUser user, EcShop.Entities.Orders.OrderInfo order, string reason)
        {
            if (user != null)
            {
                EcShop.Entities.Store.MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderClosed");
                if (template != null)
                {
                    System.Net.Mail.MailMessage email = null;
                    string innerSubject = null;
                    string innerMessage = null;
                    string smsMessage = null;
                    SiteSettings siteSettings = HiContext.Current.SiteSettings;
                    Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, 0m, null, null, null, null, null, null, null, null, null, 0m, reason, template, out email, out smsMessage, out innerSubject, out innerMessage);
                    TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderClose(template.WeixinTemplateId, siteSettings, user, order, reason);
                    string msg;
                    Messenger.SendXinge(user, siteSettings, template, 0, XinGeMsgType.OrderClose, order, out msg);
                    Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage);
                }
            }
        }
        private static void Send(EcShop.Entities.Store.MessageTemplate template, SiteSettings settings, IUser user, bool sendFirst, System.Net.Mail.MailMessage email, string innerSubject, string innerMessage, string smsMessage, TemplateMessage templateMessage)
        {
            Member member = user as Member;
            if (template.SendEmail && email != null)
            {
                if (member.EmailVerification == true)
                {
                    if (sendFirst)
                    {
                        EmailSender emailSender = Messenger.CreateEmailSender(settings);
                        if (emailSender == null || !Messenger.SendMail(email, emailSender))
                        {
                            Emails.EnqueuEmail(email, settings);
                        }
                    }
                    else
                    {
                        Emails.EnqueuEmail(email, settings);
                    }
                }
            }
            if (template.SendSMS)
            {
                string userCellPhone = Messenger.GetUserCellPhone(user);
                if (!string.IsNullOrEmpty(userCellPhone))
                {
                    string text;
                    Messenger.SendSMS(userCellPhone, smsMessage, settings, out text);
                }
            }
            if (template.SendInnerMessage)
            {
                Messenger.SendInnerMessage(settings, innerSubject, innerMessage, user.Username);
            }
            if (template.SendWeixin && !string.IsNullOrWhiteSpace(template.WeixinTemplateId) && templateMessage != null)
            {
                string token_Message = TokenApi.GetToken_Message(settings.WeixinAppId, settings.WeixinAppSecret);
                TemplateApi.SendMessage(token_Message, templateMessage);
            }
            //判断是否发送信鸽消息
            if (template.XinGeSend)
            {
                // Messenger.SendXinge(null, null, settings, out text);
            }
        }
        private static string GetUserCellPhone(IUser user)
        {
            string result;
            if (user == null)
            {
                result = null;
            }
            else
            {
                if (user is Member)
                {
                    result = ((Member)user).CellPhone;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        private static void GenericUserMessages(SiteSettings settings, string username, string userEmail, string password, string dealPassword, EcShop.Entities.Store.MessageTemplate template, out System.Net.Mail.MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
        {
            email = null;
            smsMessage = null;
            string text;
            innerMessage = (text = null);
            innerSubject = text;
            if (template != null && settings != null)
            {
                if (template.SendEmail && settings.EmailEnabled)
                {
                    email = Messenger.GenericUserEmail(template, settings, username, userEmail, password, dealPassword);
                }
                if (template.SendSMS && settings.SMSEnabled)
                {
                    smsMessage = Messenger.GenericUserMessageFormatter(settings, template.SMSBody, username, userEmail, password, dealPassword);
                }
                if (template.SendInnerMessage)
                {
                    innerSubject = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageSubject, username, userEmail, password, dealPassword);
                    innerMessage = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageBody, username, userEmail, password, dealPassword);
                }
            }
        }
        private static System.Net.Mail.MailMessage GenericUserEmail(EcShop.Entities.Store.MessageTemplate template, SiteSettings settings, string username, string userEmail, string password, string dealPassword)
        {
            System.Net.Mail.MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
            System.Net.Mail.MailMessage result;
            if (emailTemplate == null)
            {
                result = null;
            }
            else
            {
                emailTemplate.Subject = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Subject, username, userEmail, password, dealPassword);
                emailTemplate.Body = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Body, username, userEmail, password, dealPassword);
                result = emailTemplate;
            }
            return result;
        }
        private static string GenericUserMessageFormatter(SiteSettings settings, string stringToFormat, string username, string userEmail, string password, string dealPassword)
        {
            stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
            stringToFormat = stringToFormat.Replace("$Username$", username.Trim());
            if (!string.IsNullOrEmpty(userEmail))
            {
               stringToFormat = stringToFormat.Replace("$Email$", userEmail.Trim());
            }
            stringToFormat = stringToFormat.Replace("$Password$", password);
            stringToFormat = stringToFormat.Replace("$DealPassword$", dealPassword);
            return stringToFormat;
        }
        private static void GenericOrderMessages(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason, EcShop.Entities.Store.MessageTemplate template, out System.Net.Mail.MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
        {
            email = null;
            smsMessage = null;
            string text;
            innerMessage = (text = null);
            innerSubject = text;
            if (template != null && settings != null)
            {
                if (template.SendEmail && settings.EmailEnabled)
                {
                    email = Messenger.GenericOrderEmail(template, settings, username, userEmail, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                }
                if (template.SendSMS && settings.SMSEnabled)
                {
                    smsMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.SMSBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                }
                if (template.SendInnerMessage)
                {
                    innerSubject = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                    innerMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                }
            }
        }

        private static void GenericOrderMessages(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason,string expresscompany, EcShop.Entities.Store.MessageTemplate template, out System.Net.Mail.MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
        {
            email = null;
            smsMessage = null;
            string text;
            innerMessage = (text = null);
            innerSubject = text;
            if (template != null && settings != null)
            {
                if (template.SendEmail && settings.EmailEnabled)
                {
                    email = Messenger.GenericOrderEmail(template, settings, username, userEmail, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                }
                if (template.SendSMS && settings.SMSEnabled)
                {
                    smsMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.SMSBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason,expresscompany);
                }
                if (template.SendInnerMessage)
                {
                    innerSubject = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason,expresscompany);
                    innerMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason,expresscompany);
                }
            }
        }


        private static System.Net.Mail.MailMessage GenericOrderEmail(EcShop.Entities.Store.MessageTemplate template, SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
        {
            System.Net.Mail.MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
            System.Net.Mail.MailMessage result;
            if (emailTemplate == null)
            {
                result = null;
            }
            else
            {
                emailTemplate.Subject = Messenger.GenericOrderMessageFormatter(settings, username, emailTemplate.Subject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                emailTemplate.Body = Messenger.GenericOrderMessageFormatter(settings, username, emailTemplate.Body, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
                result = emailTemplate;
            }
            return result;
        }
        private static string GenericOrderMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
        {
            stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
            stringToFormat = stringToFormat.Replace("$Username$", username);
            stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
            stringToFormat = stringToFormat.Replace("$Total$", total.ToString("F"));
            stringToFormat = stringToFormat.Replace("$Memo$", memo);
            stringToFormat = stringToFormat.Replace("$Shipping_Type$", shippingType);
            stringToFormat = stringToFormat.Replace("$Shipping_Name$", shippingName);
            stringToFormat = stringToFormat.Replace("$Shipping_Addr$", shippingAddress);
            stringToFormat = stringToFormat.Replace("$Shipping_Zip$", shippingZip);
            stringToFormat = stringToFormat.Replace("$Shipping_Phone$", shippingPhone);
            stringToFormat = stringToFormat.Replace("$Shipping_Cell$", shippingCell);
            stringToFormat = stringToFormat.Replace("$Shipping_Email$", shippingEmail);
            stringToFormat = stringToFormat.Replace("$Shipping_Billno$", shippingBillno);
            stringToFormat = stringToFormat.Replace("$RefundMoney$", refundMoney.ToString("F"));
            stringToFormat = stringToFormat.Replace("$CloseReason$", closeReason);
            return stringToFormat;
        }

        private static string GenericOrderMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason,string expresscompany)
        {
            stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
            stringToFormat = stringToFormat.Replace("$Username$", username);
            stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
            stringToFormat = stringToFormat.Replace("$Total$", total.ToString("F"));
            stringToFormat = stringToFormat.Replace("$Memo$", memo);
            stringToFormat = stringToFormat.Replace("$Shipping_Type$", shippingType);
            stringToFormat = stringToFormat.Replace("$Shipping_Name$", shippingName);
            stringToFormat = stringToFormat.Replace("$Shipping_Addr$", shippingAddress);
            stringToFormat = stringToFormat.Replace("$Shipping_Zip$", shippingZip);
            stringToFormat = stringToFormat.Replace("$Shipping_Phone$", shippingPhone);
            stringToFormat = stringToFormat.Replace("$Shipping_Cell$", shippingCell);
            stringToFormat = stringToFormat.Replace("$Shipping_Email$", shippingEmail);
            stringToFormat = stringToFormat.Replace("$Shipping_Billno$", shippingBillno);
            stringToFormat = stringToFormat.Replace("$RefundMoney$", refundMoney.ToString("F"));
            stringToFormat = stringToFormat.Replace("$CloseReason$", closeReason);
            stringToFormat = stringToFormat.Replace("$Shipping_ExpressCompany$",expresscompany);
            return stringToFormat;
        }
    }
}
