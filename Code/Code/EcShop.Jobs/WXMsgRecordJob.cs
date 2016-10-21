using Ecdev.Weixin.MP.Api;
using Ecdev.Weixin.MP.Util;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace EcShop.Jobs
{
    public class WXMsgRecordJob : IJob
    {
        private static string wx_MsgRecord_URL = "https://api.weixin.qq.com/customservice/msgrecord/getrecord?access_token={0}";
        private Database database;
        public WXMsgRecordJob()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public void Execute(XmlNode node)
        {
            ErrorLog.Write("start go WXMsgRecordJob");
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            int pageSize = 50;
            bool update = false;
            WXLog log = WXHelper.GetWXLog("IsSuccess=0 ORDER BY StartTime ASC");
            if (log.Id != 0)
            {
                update = true;
                startTime = log.StartTime;
            }
            else
            {
                log = WXHelper.GetWXLog("IsSuccess=1 ORDER BY StartTime DESC");
                if (log.Id != 0)
                {
                    if (log.StartTime.Date.AddDays(1)== DateTime.Now.Date)
                    {
                        return;
                    }
                    startTime = log.EndTime.AddSeconds(1);
                }
                else
                {
                    startTime = startTime.AddDays(-1).Date;
                }
            }
            endTime = startTime.Date.AddDays(1).AddSeconds(-1);


            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            WebUtils webUtils=new WebUtils();
            string returnString = string.Empty;
            DataTable dtMsg = CreateEmptyDataTable();
            DataRow dr = null;

            for (int i = 0; i < 2147483647; i++)
            {
                StringBuilder strJson = new StringBuilder();
                strJson.Append("{");
                strJson.AppendFormat("\"endtime\":{0},", DataHelper.ConvertUniversalTime(endTime));
                strJson.AppendFormat("\"pageindex\":{0},", i+1);
                strJson.AppendFormat("\"pagesize\":{0},", pageSize);
                strJson.AppendFormat("\"starttime\":{0}", DataHelper.ConvertUniversalTime(startTime));
                strJson.Append("}");
                ErrorLog.Write("请求微信服务器，获取客服聊天记录"+strJson.ToString());
                returnString = webUtils.DoPost(string.Format(wx_MsgRecord_URL, TokenApi.GetToken_Message(masterSettings.WeixinAppId,masterSettings.WeixinAppSecret)), strJson.ToString());
                ErrorLog.Write(returnString);
                WXMsgPackage package = null;
                try
                {
                   package = Newtonsoft.Json.JsonConvert.DeserializeObject<WXMsgPackage>(returnString);
                }
                catch (Exception ex)
                {
                    ErrorLog.Write(ex.ToString());
                }

                if (package==null||package.RecordList == null || package.RecordList.Length == 0)
                {
                    break;
                }

                foreach (WXMsgRecord record in package.RecordList)
                {
                    dr = dtMsg.NewRow();
                    dr["Id"] = i;
                    dr["OpenId"] = record.OpenId;
                    dr["OperCode"] = record.OperCode;
                    dr["Text"] = record.Text;
                    dr["Worker"] = record.Worker;
                    if (string.IsNullOrEmpty(record.Worker)||record.Worker.Trim()=="")
                    {
                        dr["WorkerNo"] = "";
                    }
                    else
                    {
                        dr["WorkerNo"] = record.Worker.Substring(0, record.Worker.IndexOf('@'));   
                    }
                    DateTime d = DataHelper.ConvertTimeFromUniversal(record.UniversalTime);
                    dr["Time"] = d;
                    dr["HappenDate"] = d.Date;
                    dr["HappenMonth"] = d.ToString("yyyy-MM");
                    dtMsg.Rows.Add(dr);
                }
                //break;
            }
            WXLog newLog = new WXLog();
            newLog.AddTime = DateTime.Now;
            newLog.UpdateTime = DateTime.Now;
            newLog.StartTime = startTime;
            newLog.EndTime = endTime;
            newLog.Type = 1;
            if (dtMsg.Rows.Count <= 0)
            {
                newLog.IsSuccess = true;
                newLog.Remark = "当前查询时间没有记录";
                WXHelper.InsertLog(newLog);
                return;
            }
         
         bool b=   WXHelper.BathAddWXMsgRecord(dtMsg);
         
         ErrorLog.Write(Newtonsoft.Json.JsonConvert.SerializeObject(newLog));
         if (b)
         {
             newLog.IsSuccess = true;
             if (update)
             {
                 newLog.Id = log.Id;
                 newLog.Remark = "修复聊天记录成功";
                 WXHelper.UpdateLog(newLog);
             }
             else
             {
                 newLog.Remark = "更新聊天记录完成";
                 WXHelper.InsertLog(newLog);
             }
         }
         else
         {
             newLog.IsSuccess = false;
             if (update)
             {
                 newLog.Id = log.Id;
                 newLog.Remark = "修复聊天记录失败";
                 WXHelper.UpdateLog(newLog);
             }
             else
             {
                 newLog.Remark = "更新聊天记录失败";
                 WXHelper.InsertLog(newLog);
             }
         }
        }

        private DataTable CreateEmptyDataTable()
        {
            DataTable dtWXMsgRecord = new DataTable("Ecshop_WXMsgRecord");
            dtWXMsgRecord.Columns.Add("Id", typeof(Int32));
            dtWXMsgRecord.Columns.Add("OpenId",typeof(string));
            dtWXMsgRecord.Columns.Add("OperCode", typeof(string));
            dtWXMsgRecord.Columns.Add("Text", typeof(string));
            dtWXMsgRecord.Columns.Add("Time", typeof(DateTime));
            dtWXMsgRecord.Columns.Add("HappenDate", typeof(DateTime));
            dtWXMsgRecord.Columns.Add("HappenMonth", typeof(string));
            dtWXMsgRecord.Columns.Add("Worker", typeof(string));
            dtWXMsgRecord.Columns.Add("WorkerNo", typeof(string));

            return dtWXMsgRecord;
        }
    }
}
