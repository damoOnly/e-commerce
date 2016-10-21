using EcShop.ControlPanel.Sales;
using EcShop.Core.Jobs;
using EcShop.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace EcShop.Jobs
{
    public class ExpressJob : IJob
    {
        int pageSize = 200;
        public void Execute(XmlNode node)
        {
            try
            {
                //获取订单总行数
                int orderCount = EcShop.ControlPanel.Sales.OrderHelper.GetDeliveredOrderCount();
                int pagesize = orderCount / pageSize + (orderCount % pageSize > 0 ? 1 : 0);
                for (int index = 1; index <= pagesize; index++)
                {
                    DataTable dt = EcShop.ControlPanel.Sales.OrderHelper.GetDeliveredOrderDt(pageSize, index);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string ExpressCompanyAbb = dt.Rows[i]["ExpressCompanyAbb"].ToString();
                            string ExpressNum = dt.Rows[i]["ShipOrderNumber"].ToString();
                            //循环获取订单的签收状态
                            List<object> expressInfo = ExpressHelper.GetExpressInfoByNum(ExpressCompanyAbb, ExpressNum);
                            if (expressInfo.Count > 0)
                            {
                                //foreach (object express in expressInfo)
                                //{
                                //    string Time = ExpressHelper.GetPropertyValue(express, "time").ToString();
                                //    string Content = ExpressHelper.GetPropertyValue(express, "context").ToString();
                                //}
                                string lastInfo = ExpressHelper.GetPropertyValue(expressInfo[expressInfo.Count - 1], "context").ToString();
                                if (lastInfo.Contains("签收"))
                                {
                                    dic.Add(ExpressNum, "3");
                                }
                            }
                        }
                        if (dic.Count > 0)
                        {
                            OrderHelper.UpdateShipOrderStatusByShipOrderNumber(dic);
                        }
                    }
                }
                //DataTable dt = EcShop.ControlPanel.Sales.OrderHelper.GetDeliveredOrderDt();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    Dictionary<string, string> dic = new Dictionary<string, string>();
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        string ExpressCompanyAbb = dt.Rows[i]["ExpressCompanyAbb"].ToString();
                //        string ExpressNum = dt.Rows[i]["ShipOrderNumber"].ToString();
                //        //循环获取订单的签收状态
                //        List<object> expressInfo = ExpressHelper.GetExpressInfoByNum(ExpressCompanyAbb, ExpressNum);
                //        if (expressInfo.Count > 0)
                //        {
                //            //foreach (object express in expressInfo)
                //            //{
                //            //    string Time = ExpressHelper.GetPropertyValue(express, "time").ToString();
                //            //    string Content = ExpressHelper.GetPropertyValue(express, "context").ToString();
                //            //}
                //            string lastInfo = ExpressHelper.GetPropertyValue(expressInfo[expressInfo.Count - 1], "context").ToString();
                //            if (lastInfo.Contains("签收"))
                //            {
                //                dic.Add(ExpressNum, "3");
                //            }
                //        }
                //    }
                //    OrderHelper.UpdateShipOrderStatusByShipOrderNumber(dic);
                //}
            }
            catch (Exception ex)
            {
                WMSHelper.SaveLog("定时获取运单状态：ExecuteJob", "", "异常：" + ex.ToString(), "error", "in");
            }
        }
    }
}
