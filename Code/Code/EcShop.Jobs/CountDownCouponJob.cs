using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core.Jobs;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Xml;
namespace EcShop.Jobs
{
    public class CountDownCouponJob : IJob
	{
		private int failureInterval = 15;
		private int numberOfTries = 5;
		public void Execute(XmlNode node)
		{
            int curHours = 0;
            int curPoint = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (!string.IsNullOrWhiteSpace(masterSettings.CountDownCouponHours))
            {
                int.TryParse(masterSettings.CountDownCouponHours, out curHours);
            }
            int.TryParse(masterSettings.CountDownCouponPoint, out curPoint);
            // 判断是否是该点发送提醒短信
            if (DateTime.Now.Hour == curPoint)
            {
                DateTime curDate = DateTime.Now.AddHours(curHours);
                List<CountDownCoupons> list = new List<CountDownCoupons>();
                DataTable dtCoupons = CouponHelper.GetAllCountDownCoupons(curDate);
                if (dtCoupons != null && dtCoupons.Rows.Count > 0)
                {
                    foreach (DataRow r in dtCoupons.Rows)
                    {
                        CountDownCoupons cc = new CountDownCoupons();
                        string currcellphone = string.Empty;
                        string currcontent = string.Empty;
                        int value = 0;
                        if (r["CellPhone"] != null && !string.IsNullOrWhiteSpace(r["CellPhone"].ToString()))
                        {
                            currcellphone = r["CellPhone"].ToString();
                        }
                        if (r["DiscountValue"] != null && !string.IsNullOrWhiteSpace(r["DiscountValue"].ToString()))
                        {
                            value = Convert.ToInt32(r["DiscountValue"]);
                        }
                        if (r["ClosingTime"] != null && !string.IsNullOrWhiteSpace(r["ClosingTime"].ToString()) && value > 0)
                        {
                            currcontent = string.Format(masterSettings.CountDownCouponContent, value, r["ClosingTime"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(currcellphone) && !string.IsNullOrWhiteSpace(currcontent))
                        {
                            cc.CellPhone = currcellphone;
                            cc.Content = currcontent;
                            cc.ClaimCode = r["ClaimCode"].ToString();
                            list.Add(cc);
                        }

                    }
                    if (list.Count > 0)
                    {
                        SmsHelper.QueueSMS(list, 0, 2);
                    }
                }
            }
		}
	}
}
