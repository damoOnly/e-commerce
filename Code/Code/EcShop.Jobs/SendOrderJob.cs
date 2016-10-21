using Ecdev.Weixin.MP.Domain;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Membership.Context;
using EcShop.Messages;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Xml;
namespace EcShop.Jobs
{
    /// <summary>
    /// 发送订单销售统计情况
    /// </summary>
    public class SendOrderJob : IJob
	{
		private int failureInterval = 15;
		private int numberOfTries = 5;
        private Database database;
		public void Execute(XmlNode node)
		{
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string reviewContent = string.Empty;
            if (null != node)
            {
                XmlAttribute reviewContentAttribute = node.Attributes["reviewContent"];

                if (reviewContentAttribute != null)
                {
                     reviewContent = reviewContentAttribute.Value;
                }

            }
            if (string.IsNullOrEmpty(reviewContent))
            {
                reviewContent = "23";
            }
            if (!string.IsNullOrEmpty(masterSettings.IsRunTimes))
            {
                reviewContent = masterSettings.IsRunTimes;
            }
            string[] arr = reviewContent.Replace('，',',').Split(',');
            foreach(string times in arr)
            {
                int tims = 0;
                int.TryParse(times, out tims);
                if (DateTime.Now.Hour == tims)
                {
                   
                    if (masterSettings != null && masterSettings.IsSendOrderOpen == "1")//是否订单推送
                    {
                        #region 脚本
                        string sql = @"--总订单
                                        select 
                                               count(1) as 'count',
                                               sum(OrderTotal) as 'SumPrice' ,
                                               1 as 'type',
                                               (select count(1) from  Ecshop_Orders with(nolock) where OrderDate>@StartTime and  OrderDate<@endTime )  as 'SplitCount'
                                             from Ecshop_Orders with(nolock) 
                                            where OrderDate>@StartTime and  OrderDate<@endTime  and isnull(SourceOrderId,'')=''
                                            union all
                                            --已付款
                                            select count(1) as 'count',sum(OrderTotal) as 'SumPrice' ,2 as 'type',
                                             (select count(1) from  Ecshop_Orders with(nolock) where OrderDate>@StartTime and  OrderDate<@endTime  and  OrderStatus>=2 and OrderStatus!=4  )  as 'SplitCount'
                                             from Ecshop_Orders with(nolock) 
                                            where  OrderDate>@StartTime and  OrderDate<@endTime and  OrderStatus>=2 and OrderStatus!=4  and isnull(SourceOrderId,'')=''
                                            --订单来源
                                            select case  SourceOrder when 1  then 'PC'
                                            when 3 then '微信' 
                                            when 11 then 'Android'
                                            when 12 then 'IOS' else '其他' end 'SourceOrder' ,count(1) as 'Count',3 as 'type'  from Ecshop_Orders  with(nolock)
                                            where OrderDate>@StartTime and  OrderDate<@endTime  and isnull(SourceOrderId,'')=''
                                            group by SourceOrder 
                                            union all
                                            select case  SourceOrder when 1  then 'PC'
                                            when 3 then '微信' 
                                            when 11 then 'Android'
                                            when 12 then 'IOS' else '其他' end 'SourceOrder' ,count(1) as 'Count' ,4 as 'type' from Ecshop_Orders with(nolock)   
                                            where OrderDate>@StartTime  and  OrderDate<@endTime and  OrderStatus>=2 and OrderStatus!=4
                                            group by SourceOrder;
                                            --注册帐号信息
                                            select count(1) as 'count' from [dbo].[aspnet_Members] as a
                                            inner join [dbo].[aspnet_Users] as b on a.userId=b.userId
                                            where b.CreateDate between @StartTime and @endTime";
                        #endregion

                        #region 参数
                        this.database = DatabaseFactory.CreateDatabase();
                        DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);

                        int day = 1;
                        int.TryParse(masterSettings.SendOrderDay, out day);

                        int Sarthour = 18;

                        int.TryParse(masterSettings.SendOrderStartTime, out Sarthour);

                        string StartTime = DateTime.Now.AddDays(-day).ToString("yyyy-MM-dd");
                        StartTime = Convert.ToDateTime(StartTime).AddHours(Sarthour).ToString();

                        int endhour = 18;
                        int.TryParse(masterSettings.SendOrderEndTime, out endhour);

                        string EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                        EndTime = Convert.ToDateTime(EndTime).AddHours(Sarthour).ToString();

                        #endregion

                        this.database.AddInParameter(sqlStringCommand, "@StartTime", DbType.String, StartTime);
                        this.database.AddInParameter(sqlStringCommand, "@endTime", DbType.String, EndTime);

                        #region 拼接邮件内容
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("<div style=\"font-size:14px\">统计时间:{0}至{1}<br/> &nbsp;&nbsp;&nbsp;&nbsp;", StartTime, EndTime);

                        using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["type"].ToString() == "1")
                                {
                                    sb.AppendFormat("总订单数：{0}个<br/> 实际订单总数： {1}个<br/> &nbsp;&nbsp;&nbsp;&nbsp;总金额：   {2}元<br /><br/>&nbsp;", dataReader["SplitCount"].ToString(), dataReader["count"], dataReader["SumPrice"]);
                                }
                                if (dataReader["type"].ToString() == "2")
                                {
                                    sb.AppendFormat("付款总订单数：{0}个<br/>  实际已付款订单数： {1}个<br/>&nbsp;&nbsp;已付款金额： {2}元<br /><br/>", dataReader["SplitCount"].ToString(), dataReader["count"], dataReader["SumPrice"]);
                                }
                            }
                            dataReader.NextResult();
                            string Pay = " 已付款总订单分布:";
                            string NoPay = "实际订单分布:";
                            while (dataReader.Read())
                            {
                                if (dataReader["type"].ToString() == "4") //已经付款
                                {
                                    Pay += dataReader["SourceOrder"] + ":" + dataReader["Count"] + "；";
                                }
                                if (dataReader["type"].ToString() == "3") //已经付款
                                {
                                    NoPay += dataReader["SourceOrder"] + ":" + dataReader["Count"] + "；";
                                }
                            }
                            sb.AppendFormat(Pay + "<br/>");
                            sb.AppendFormat(NoPay + "<br/>");
                            dataReader.NextResult();
                            while (dataReader.Read())
                            {
                                sb.AppendFormat("     注册会员数：{0}<br/>", dataReader["count"]);
                            }
                        }
                        sb.AppendFormat("</div>");
                        #endregion

                        string[] emialarr = masterSettings.SendOrderEmail.Replace("，", ",").Split(',');
                        for (int i = 0; i < emialarr.Length; i++)
                        {
                            bool sendrest = Messenger.OrderSaleRpt(sb.ToString(), emialarr[i].ToString());
                            ErrorLog.Write("【销售统计分析发送邮件结果】执行结果：" + sendrest);
                        }

                     
                    }
                }
            }

		}
	}
}
