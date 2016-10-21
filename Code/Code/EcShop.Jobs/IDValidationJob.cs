using EcShop.ControlPanel.Commodities;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace EcShop.Jobs
{
    public class IDValidationJob : IJob
    {
        private string efindUrl;
        private string runType;
        public void Execute(XmlNode node)
        {
            ErrorLog.Write("执行了IDValidationJob");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            
            if (!string.IsNullOrEmpty(masterSettings.efindUrl))
            {
                efindUrl = masterSettings.efindUrl;
            }

            if (!string.IsNullOrEmpty(masterSettings.efindRunType))
            {
                runType = masterSettings.efindRunType;
            }

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetStoredProcCommand("cp_GetOrdersStatus_IdentityCardValidate");

            database.AddInParameter(sqlStringCommand, "runType", DbType.Int32, int.Parse(runType));

            DataTable dt = database.ExecuteDataSet(sqlStringCommand).Tables[0];

            CreateString(dt);
        }

        private void CreateString(DataTable dt)
        {
            
            if (dt.Rows.Count > 0)
            { 
                foreach (DataRow dr in dt.Rows)
                {
                    string orderid = dr["orderid"].ToString();
                    string ShipToID = dr["IdentityCard"].ToString();
                    string ShipToName = dr["ShipTo"].ToString();
                    string ShipToaddress = dr["address"].ToString();
                    string ShipToPhone = dr["CellPhone"].ToString();
                    //orderNo  订单号
                    //userName 姓名
                    //idCareNum 身份证号码
                    //phoneNum 收货人手机
                    //address 收货地址
                    string postData = "orderNo="+orderid+"&userName="+ShipToName+"&idCareNum="+ShipToID+"&phoneNum="+ShipToPhone+"&address="+ShipToaddress;
                    ErrorLog.Write("efindUrl send data : " + postData);

                    string str = SendData(efindUrl, postData);

                    ErrorLog.Write("efindUrl return data : " + str);

                    HSCodeHelper.SetPayerIDStatus(orderid, ShipToName, ShipToID, ShipToaddress, ShipToPhone, str, runType);
                }
            }
        }

       
        public string SendData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream myResponseStream = response.GetResponseStream();

                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                    str = myStreamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }
    }
}
