using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;

using Ecdev.Plugins;
using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SqlDal.Store;

namespace EcShop.Messages
{
    public static class SMS
	{
		internal static void EnqueuSMS(SmsInfo smsInfo, SiteSettings settings)
		{
			if (smsInfo != null && !string.IsNullOrEmpty(smsInfo.Mobile))
			{
				new SMSQueueDao().QueueSMS(smsInfo);
			}
		}
		public static void SendQueuedSMS(int failureInterval, int maxNumberOfTries, SiteSettings settings)
		{
            ErrorLog.Write("开始发送短信，进入队列...");

			if (settings != null)
			{
				HiConfiguration config = HiConfiguration.GetConfig();
				Dictionary<System.Guid, SmsInfo> dictionary = new SMSQueueDao().DequeueSMS();
				IList<System.Guid> list = new List<System.Guid>();


                ErrorLog.Write("开始发送短信，创建发送对象...");

                SMSSender sender = Messenger.CreateSMSSender(settings);

				if (sender != null)
				{
					int num = 0;
					int smsSendBatchSize = 500;

					foreach (System.Guid current in dictionary.Keys)
					{
                        SmsInfo currentSms = dictionary[current];

                        string msg = "";
                        SendStatus sendStatus = SendStatus.Fail;

                        try
                        {
                            ErrorLog.Write(string.Format("发送短信，手机号码：{0}，短信内容：{1}", currentSms.Mobile, currentSms.Body));
                            if(currentSms.type==2)
                            {
                                sendStatus = Messenger.SendSMS(currentSms.Mobile, currentSms.Body, settings,currentSms.type, out msg);
                            }
                            else
                            {
                                sendStatus = Messenger.SendSMS(currentSms.Mobile, currentSms.Body, settings, out msg);
                            }
                            
                        }
                        catch(Exception ex)
                        {
                            ErrorLog.Write(string.Format("发送短信异常，手机号码：{0}，短信内容：{1}，原因：{2}", currentSms.Mobile, currentSms.Body, ex.Message));
                        }

                        if (sendStatus == SendStatus.Success)
						{
                            ErrorLog.Write(string.Format("发送短信成功，开始删除队列，手机号码：{0}，短信内容：{1}", currentSms.Mobile, currentSms.Body));

							new SMSQueueDao().DeleteQueuedSMS(current);

							if (smsSendBatchSize != -1 && ++num >= smsSendBatchSize)
							{
								System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 15, 0));
								num = 0;
							}
						}
						else
						{
                            ErrorLog.Write(string.Format("发送短信失败，手机号码：{0}，短信内容：{1}，加入失败队列", currentSms.Mobile, currentSms.Body));
							list.Add(current);
						}
					}

					if (list.Count > 0)
					{
                        ErrorLog.Write("发送短信包含失败号码，重置发送队列");
						new SMSQueueDao().QueueSendingFailure(list, failureInterval, maxNumberOfTries);
					}
				}
                else
                {
                    ErrorLog.Write("开始发送短信，发送对象创建不成功，请检查配置...");
                }
			}
		}
	}
}
