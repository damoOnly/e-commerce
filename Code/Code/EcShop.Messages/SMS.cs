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
            ErrorLog.Write("��ʼ���Ͷ��ţ��������...");

			if (settings != null)
			{
				HiConfiguration config = HiConfiguration.GetConfig();
				Dictionary<System.Guid, SmsInfo> dictionary = new SMSQueueDao().DequeueSMS();
				IList<System.Guid> list = new List<System.Guid>();


                ErrorLog.Write("��ʼ���Ͷ��ţ��������Ͷ���...");

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
                            ErrorLog.Write(string.Format("���Ͷ��ţ��ֻ����룺{0}���������ݣ�{1}", currentSms.Mobile, currentSms.Body));
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
                            ErrorLog.Write(string.Format("���Ͷ����쳣���ֻ����룺{0}���������ݣ�{1}��ԭ��{2}", currentSms.Mobile, currentSms.Body, ex.Message));
                        }

                        if (sendStatus == SendStatus.Success)
						{
                            ErrorLog.Write(string.Format("���Ͷ��ųɹ�����ʼɾ�����У��ֻ����룺{0}���������ݣ�{1}", currentSms.Mobile, currentSms.Body));

							new SMSQueueDao().DeleteQueuedSMS(current);

							if (smsSendBatchSize != -1 && ++num >= smsSendBatchSize)
							{
								System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 15, 0));
								num = 0;
							}
						}
						else
						{
                            ErrorLog.Write(string.Format("���Ͷ���ʧ�ܣ��ֻ����룺{0}���������ݣ�{1}������ʧ�ܶ���", currentSms.Mobile, currentSms.Body));
							list.Add(current);
						}
					}

					if (list.Count > 0)
					{
                        ErrorLog.Write("���Ͷ��Ű���ʧ�ܺ��룬���÷��Ͷ���");
						new SMSQueueDao().QueueSendingFailure(list, failureInterval, maxNumberOfTries);
					}
				}
                else
                {
                    ErrorLog.Write("��ʼ���Ͷ��ţ����Ͷ��󴴽����ɹ�����������...");
                }
			}
		}
	}
}
