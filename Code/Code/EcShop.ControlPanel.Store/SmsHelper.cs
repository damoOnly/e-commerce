using EcShop.Entities.Comments;
using EcShop.Entities.Store;
using EcShop.SqlDal.Store;
using System;
using System.Collections.Generic;

namespace EcShop.ControlPanel.Store
{
    public static class SmsHelper
    {
        public static bool QueueSMS(SmsInfo message)
        {
            return new SMSQueueDao().QueueSMS(message);
        }

        public static bool QueueSMS(string mobile, string body, int priority)
        {
            SmsInfo message = new SmsInfo();

            message.SmsId = Guid.NewGuid();
            message.Mobile = mobile;
            message.Subject = "";
            message.Body = body;
            message.Priority = priority;

            return QueueSMS(message);
        }

        public static bool QueueSMS(string mobile, string body, int priority, int type)
        {
            SmsInfo message = new SmsInfo();

            message.SmsId = Guid.NewGuid();
            message.Mobile = mobile;
            message.Subject = "";
            message.Body = body;
            message.Priority = priority;
            message.type = type;
            return QueueSMS(message);
        }

        public static bool QueueSMS(string mobile, string body, int priority, int type,string claimCode)
        {
            SmsInfo message = new SmsInfo();

            message.SmsId = Guid.NewGuid();
            message.Mobile = mobile;
            message.Subject = "";
            message.Body = body;
            message.Priority = priority;
            message.type = type;
            message.ClaimCode = claimCode;
            return QueueSMS(message);
        }

        public static void QueueSMS(string[] mobiles, string body, int priority)
        {
            foreach(string current in mobiles)
            {
                QueueSMS(current, body, priority);
            }
        }


        /// <summary>
        /// type默认为0，为2时走营销短信通道
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="body"></param>
        /// <param name="priority"></param>
        /// <param name="type"></param>
        public static void QueueSMS(string[] mobiles, string body, int priority,int type)
        {
            if (mobiles != null && mobiles.Length > 0)
            {
                foreach (string current in mobiles)
                {
                    QueueSMS(current, body, priority, type);
                }
            }
        }

        /// <summary>
        /// type默认为0，为2时走营销短信通道
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="body"></param>
        /// <param name="priority"></param>
        /// <param name="type"></param>
        public static void QueueSMS(List<CountDownCoupons> list, int priority, int type)
        {
            if (list != null && list.Count> 0)
            {
                foreach (var item in list)
                {
                    QueueSMS(item.CellPhone, item.Content, priority, type, item.ClaimCode);
                }
            }
        }
    }
}
