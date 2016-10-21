using Ecdev.Weixin.MP;
using Ecdev.Weixin.Pay;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Membership.Context;
using LumenWorks.Framework.IO.Csv;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace EcShop.Jobs
{
    public class WXBillJob : IJob
    {
        private Database database;
        public WXBillJob()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        public void Execute(XmlNode node)
        {
            ErrorLog.Write("start go WXBillJob");
            DateTime curDate = DateTime.Now;
            string billDate = curDate.AddDays(-1).ToString("yyyyMMdd");
            string billInfo = string.Empty;
            IList<string> list = GetFailUpdatedLog();
            if (list.Count > 0)
            {
                foreach (string item in list)
                {
                    ErrorLog.Write("start go LoadFailedWXBill1");
                    try
                    {
                        billInfo = LoadWXBill(item);
                    }
                    catch (Exception ex)
                    {
                        UpdateBillJobLog(item, DateTime.Now, "连接微信服务器出错"+ex.ToString(), false);
                        continue;
                    }
                    SaveWXBill(billInfo, item, false);              
                }
                return;
            }
            if (curDate.Hour < 9)//微信服务器9点中才更新前一天的对账单
            {
                return;
            }
            if (curDate.Hour < 10)
            {
                if (curDate.Minute <= 30)
                {
                    return;
                }
            }
            string tLastUpdateDate = GetLastSucessTime();
            if (billDate == tLastUpdateDate)
            {
                return;
            }
            ErrorLog.Write("start go LoadWXBill2");
            try
            {
                ErrorLog.Write(billDate);
                billInfo = LoadWXBill(billDate);

            }     
            catch (Exception ex)
            {
                AddBillJobLog(billDate, DateTime.Now, DateTime.Now, "连接微信服务器出错"+ex.ToString(), false);
                ErrorLog.Write("连接微信服务器出错" + ex.ToString());
                return;
            }
            SaveWXBill(billInfo, billDate, true);
            //}
        }
        private string LoadWXBill(string billDate)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);

            string postData = payClient.BuildBillPackage(billDate, BillType.ALL);
            ErrorLog.Write(postData);
            return payClient.downloadBill(postData);
        }
        public bool SaveWXBill(string billInfo, string billDate, bool isFirst)
        {
            bool isSuccess = true;
            if(billInfo.Contains("No Bill Exist"))
            {
                 isSuccess = true;
                if (isFirst)
                {
                    AddBillJobLog(billDate, DateTime.Now, DateTime.Now, "没有订单数据" + billInfo, true);
                }
                else
                {
                    UpdateBillJobLog(billDate, DateTime.Now, "没有订单数据" + billInfo, true);
                }
                return isSuccess;
            }
            else if (billInfo.Contains("return_code"))
            {
                isSuccess = false;
                if (isFirst)
                {
                    AddBillJobLog(billDate, DateTime.Now, DateTime.Now, "同步对账单时，返回错误信息" + billInfo, false);
                }
                else
                {
                    UpdateBillJobLog(billDate, DateTime.Now, "同步对账单时，返回错误信息" + billInfo, false);
                }
                return isSuccess;
            }
            DataTable dtBillInfo = null;
            try
            {
                dtBillInfo = ReadCSV(billInfo);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                if (isFirst)
                {
                    AddBillJobLog(billDate, DateTime.Now, DateTime.Now, "数据异常" + billInfo, false);
                }
                else
                {
                    UpdateBillJobLog(billDate, DateTime.Now, "数据异常" + billInfo, false);
                }
                return isSuccess;
            }

            if (dtBillInfo == null )
            {
                isSuccess = false;
                if (isFirst)
                {
                    AddBillJobLog(billDate, DateTime.Now, DateTime.Now, "数据异常2" + billInfo, false);
                }
                else
                {
                    UpdateBillJobLog(billDate, DateTime.Now, "数据异常2" + billInfo, false);
                }
                return isSuccess;
            }
            int dtCount = dtBillInfo.Rows.Count;
            if (dtCount > 2)
            {
                dtBillInfo.Rows.RemoveAt(dtCount - 1);
                dtBillInfo.Rows.RemoveAt(dtCount - 2);

                SqlBulkCopy bcp = new SqlBulkCopy(this.database.ConnectionString);
                bcp.DestinationTableName = "[Ecshop_TradeDetails]";
                try
                {
                    bcp.WriteToServer(dtBillInfo);
                    if (isFirst)
                    {
                        AddBillJobLog(billDate, DateTime.Now, DateTime.Now, "对账单更新完成", true);
                    }
                    else
                    {
                        UpdateBillJobLog(billDate, DateTime.Now, "对账单恢复完成", true);
                    }
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    if (isFirst)
                    {
                        AddBillJobLog(billDate, DateTime.Now, DateTime.Now, "执行批量插入数据时出错" + ex.ToString(), false);
                    }
                    else
                    {
                        UpdateBillJobLog(billDate, DateTime.Now, "执行批量插入数据时出错", false);
                    }
                }
                finally
                {
                    bcp.Close();  
                }
            }
            return isSuccess;
        }
        private string GetLastSucessTime()
        {
            string tLastUpdateDate = string.Empty;
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT MAX(BillDate) from Ecshop_DownloadWXBillLog");
            Object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                tLastUpdateDate = obj.ToString();
            }
            return tLastUpdateDate;
        }
        private bool AddBillJobLog(string billDate, DateTime addTime, DateTime updateTime, string description, bool isSuccess)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_DownloadWXBillLog VALUES(@BillDate,@AddTime,@UpdateTime,@Description,@IsSuccess)");
            this.database.AddInParameter(sqlStringCommand, "BillDate", System.Data.DbType.String, billDate);
            this.database.AddInParameter(sqlStringCommand, "addTime", System.Data.DbType.DateTime, addTime);
            this.database.AddInParameter(sqlStringCommand, "updateTime", System.Data.DbType.DateTime, updateTime);
            this.database.AddInParameter(sqlStringCommand, "description", System.Data.DbType.String, description);
            this.database.AddInParameter(sqlStringCommand, "isSuccess", System.Data.DbType.Boolean, isSuccess);

            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret == 1)
            {
                return true;
            }
            return false;
        }
        private bool UpdateBillJobLog(string billDate, DateTime updateTime, string description, bool isSuccess)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_DownloadWXBillLog SET  UpdateTime=@UpdateTime,Description=@Description,IsSuccess=@IsSuccess WHERE BillDate=@BillDate");
            this.database.AddInParameter(sqlStringCommand, "BillDate", System.Data.DbType.String, billDate);
            this.database.AddInParameter(sqlStringCommand, "updateTime", System.Data.DbType.DateTime, updateTime);
            this.database.AddInParameter(sqlStringCommand, "description", System.Data.DbType.String, description);
            this.database.AddInParameter(sqlStringCommand, "isSuccess", System.Data.DbType.Boolean, isSuccess);

            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret == 1)
            {
                return true;
            }
            return false;
        }
        private IList<string> GetFailUpdatedLog()
        {
            IList<string> list = new List<string>();
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT BillDate from Ecshop_DownloadWXBillLog where IsSuccess=0");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    if (System.DBNull.Value != dataReader["BillDate"])
                    {
                        list.Add(dataReader["BillDate"].ToString());
                    }
                }
            }
            return list;
        }
        private DataTable ReadCSV(string billInfo)
        {
            if (string.IsNullOrEmpty(billInfo))
            {
                return null;
            }
            byte[] arrb = Encoding.UTF8.GetBytes(billInfo);
            MemoryStream stream = new MemoryStream(arrb);
            StreamReader sr = new StreamReader(stream);
            using (CsvReader csv = new CsvReader(sr, true))
            {
                DataTable dt = new DataTable();
                int columnCount = csv.FieldCount;
                dt.Columns.Add("col0");
                for (int i = 1; i < (columnCount + 1); i++)
                {
                    dt.Columns.Add("col" + i.ToString());
                }

                while (csv.ReadNextRecord())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = 1;
                    for (int i = 0; i < columnCount; i++)
                    {
                        try
                        {
                            dr[i + 1] = csv[i].ToString().Replace("`", "");
                        }
                        catch
                        {
                            dr[i + 1] = null;
                        }
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }
    }
}
