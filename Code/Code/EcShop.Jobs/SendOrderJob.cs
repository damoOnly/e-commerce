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
    /// ���Ͷ�������ͳ�����
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
            string[] arr = reviewContent.Replace('��',',').Split(',');
            foreach(string times in arr)
            {
                int tims = 0;
                int.TryParse(times, out tims);
                if (DateTime.Now.Hour == tims)
                {
                   
                    if (masterSettings != null && masterSettings.IsSendOrderOpen == "1")//�Ƿ񶩵�����
                    {
                        #region �ű�
                        string sql = @"--�ܶ���
                                        select 
                                               count(1) as 'count',
                                               sum(OrderTotal) as 'SumPrice' ,
                                               1 as 'type',
                                               (select count(1) from  Ecshop_Orders with(nolock) where OrderDate>@StartTime and  OrderDate<@endTime )  as 'SplitCount'
                                             from Ecshop_Orders with(nolock) 
                                            where OrderDate>@StartTime and  OrderDate<@endTime  and isnull(SourceOrderId,'')=''
                                            union all
                                            --�Ѹ���
                                            select count(1) as 'count',sum(OrderTotal) as 'SumPrice' ,2 as 'type',
                                             (select count(1) from  Ecshop_Orders with(nolock) where OrderDate>@StartTime and  OrderDate<@endTime  and  OrderStatus>=2 and OrderStatus!=4  )  as 'SplitCount'
                                             from Ecshop_Orders with(nolock) 
                                            where  OrderDate>@StartTime and  OrderDate<@endTime and  OrderStatus>=2 and OrderStatus!=4  and isnull(SourceOrderId,'')=''
                                            --������Դ
                                            select case  SourceOrder when 1  then 'PC'
                                            when 3 then '΢��' 
                                            when 11 then 'Android'
                                            when 12 then 'IOS' else '����' end 'SourceOrder' ,count(1) as 'Count',3 as 'type'  from Ecshop_Orders  with(nolock)
                                            where OrderDate>@StartTime and  OrderDate<@endTime  and isnull(SourceOrderId,'')=''
                                            group by SourceOrder 
                                            union all
                                            select case  SourceOrder when 1  then 'PC'
                                            when 3 then '΢��' 
                                            when 11 then 'Android'
                                            when 12 then 'IOS' else '����' end 'SourceOrder' ,count(1) as 'Count' ,4 as 'type' from Ecshop_Orders with(nolock)   
                                            where OrderDate>@StartTime  and  OrderDate<@endTime and  OrderStatus>=2 and OrderStatus!=4
                                            group by SourceOrder;
                                            --ע���ʺ���Ϣ
                                            select count(1) as 'count' from [dbo].[aspnet_Members] as a
                                            inner join [dbo].[aspnet_Users] as b on a.userId=b.userId
                                            where b.CreateDate between @StartTime and @endTime";
                        #endregion

                        #region ����
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

                        #region ƴ���ʼ�����
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("<div style=\"font-size:14px\">ͳ��ʱ��:{0}��{1}<br/> &nbsp;&nbsp;&nbsp;&nbsp;", StartTime, EndTime);

                        using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["type"].ToString() == "1")
                                {
                                    sb.AppendFormat("�ܶ�������{0}��<br/> ʵ�ʶ��������� {1}��<br/> &nbsp;&nbsp;&nbsp;&nbsp;�ܽ�   {2}Ԫ<br /><br/>&nbsp;", dataReader["SplitCount"].ToString(), dataReader["count"], dataReader["SumPrice"]);
                                }
                                if (dataReader["type"].ToString() == "2")
                                {
                                    sb.AppendFormat("�����ܶ�������{0}��<br/>  ʵ���Ѹ�������� {1}��<br/>&nbsp;&nbsp;�Ѹ���� {2}Ԫ<br /><br/>", dataReader["SplitCount"].ToString(), dataReader["count"], dataReader["SumPrice"]);
                                }
                            }
                            dataReader.NextResult();
                            string Pay = " �Ѹ����ܶ����ֲ�:";
                            string NoPay = "ʵ�ʶ����ֲ�:";
                            while (dataReader.Read())
                            {
                                if (dataReader["type"].ToString() == "4") //�Ѿ�����
                                {
                                    Pay += dataReader["SourceOrder"] + ":" + dataReader["Count"] + "��";
                                }
                                if (dataReader["type"].ToString() == "3") //�Ѿ�����
                                {
                                    NoPay += dataReader["SourceOrder"] + ":" + dataReader["Count"] + "��";
                                }
                            }
                            sb.AppendFormat(Pay + "<br/>");
                            sb.AppendFormat(NoPay + "<br/>");
                            dataReader.NextResult();
                            while (dataReader.Read())
                            {
                                sb.AppendFormat("     ע���Ա����{0}<br/>", dataReader["count"]);
                            }
                        }
                        sb.AppendFormat("</div>");
                        #endregion

                        string[] emialarr = masterSettings.SendOrderEmail.Replace("��", ",").Split(',');
                        for (int i = 0; i < emialarr.Length; i++)
                        {
                            bool sendrest = Messenger.OrderSaleRpt(sb.ToString(), emialarr[i].ToString());
                            ErrorLog.Write("������ͳ�Ʒ��������ʼ������ִ�н����" + sendrest);
                        }

                     
                    }
                }
            }

		}
	}
}
