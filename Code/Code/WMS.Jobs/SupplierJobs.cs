using EcShop.Core;
using EcShop.Core.Jobs;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
using EcShop.ControlPanel.Commodities;
using Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EcShop.Core.ErrorLog;
using EcShop.ControlPanel.Store;

namespace WMS.Jobs
{
    public class SupplierJobs : IJob
    {

        private int dateContrastType = 0;
        private int dateContrastValue = 1;
        private int sendWMSCount = 10;

        private string customerid = "";
        private string apptoken = "";
        private string appkey = "";
        private string appSecret = "";
        private string sign = "";
        private string timestamp = "";
        private string url = "";
        private string client_db = "";

        private string[] mobiles;

        private int pageIndex = 1;
        private bool flag = false;
        public void Execute(XmlNode node)
        {
            ErrorLog.Write("执行了SupplierJobs");
            pageIndex = 1;
            flag = false;
            if (null != node)
            {
                XmlAttribute urlAttribute = node.Attributes["apiurl"];
                XmlAttribute customeridAttribute = node.Attributes["client_customerid"];
                XmlAttribute apptokenAttribute = node.Attributes["apptoken"];
                XmlAttribute appkeyAttribute = node.Attributes["appkey"];
                XmlAttribute appSecretAttribute = node.Attributes["appSecret"];
                XmlAttribute dateContrastTypeAttribute = node.Attributes["DateContrastType"];
                XmlAttribute dateContrastValueAttribute = node.Attributes["DateContrastValue"];
                XmlAttribute clientdbAttribute = node.Attributes["client_db"];
                XmlAttribute sendWMSCountAttribute = node.Attributes["SendWMSCount"];
                XmlAttribute mobilesAttribute = node.Attributes["mobile"];

                if (dateContrastTypeAttribute != null)
                {
                    try
                    {
                        this.dateContrastType = int.Parse(dateContrastTypeAttribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.dateContrastType = 0;
                    }
                }
                if (dateContrastValueAttribute != null)
                {
                    try
                    {
                        this.dateContrastValue = int.Parse(dateContrastValueAttribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.dateContrastValue = 1;
                    }
                }

                if (sendWMSCountAttribute != null)
                {
                    try
                    {
                        this.sendWMSCount = int.Parse(sendWMSCountAttribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.sendWMSCount = 10;
                    }
                }

                if (urlAttribute != null)
                {
                    url = urlAttribute.Value;
                }
                if (customeridAttribute != null)
                {
                    customerid = customeridAttribute.Value;
                }
                if (apptokenAttribute != null)
                {
                    apptoken = apptokenAttribute.Value;
                }
                if (appkeyAttribute != null)
                {
                    appkey = appkeyAttribute.Value;
                }
                if (appSecretAttribute != null)
                {
                    appSecret = appSecretAttribute.Value;
                }
                if (clientdbAttribute != null)
                {
                    client_db = clientdbAttribute.Value;
                }

                if (mobilesAttribute != null)
                {
                    mobiles = mobilesAttribute.InnerText.Split(new char[] { ',' });
                }

                this.SendSupplierData();
            }
        }

        private void SendSupplierData()
        {
            ErrorLog.Write("执行了SupplierJobs -> SendSupplierData");
            try
            {
                string setdate = this.GetOrderSendCoupontime();

                SupplierQuery query = new SupplierQuery();
                query.PageIndex = pageIndex;
                query.PageSize = 100;
                query.DateContrastType = dateContrastType;
                query.DateContrastValue = dateContrastValue;
                query.DataVersion = setdate;
                query.SortBy = "DataVersion";

                ErrorLog.Write("执行了SupplierJobs -> GetWMSSupplier");

                DataTable dt = (DataTable)SupplierHelper.GetWMSSupplier(query).Data;

                ErrorLog.Write("执行了SupplierJobs -> GetWMSSupplier 查询完成");
                if (dt != null && dt.Rows.Count > 0)
                {
                    ErrorLog.Write("执行了SupplierJobs -> GetWMSSupplier 查询结果：" + dt.Rows.Count);

                    pageIndex++;
                    flag = true;

                    string skuData = CreateSupplierData(dt);
                    //WMS 无法正常接收部分字符
                    skuData = skuData.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("+", "＋");
                    string tempdata = appSecret + skuData + appSecret;
                    string md5tempdata = WMSHelper.MD5(tempdata);
                    string basetempdata = WMSHelper.EncodingString(md5tempdata.ToLower(), System.Text.Encoding.UTF8);
                    sign = System.Web.HttpUtility.UrlEncode(basetempdata.ToUpper(), System.Text.Encoding.UTF8);

                    //
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//YYYY-MM-DD HH:MM:SS

                    string postData = "method=putCustData&client_customerid=" + customerid + "&client_db=" + client_db + "&messageid=CUSTOMER&apptoken=" + apptoken + "&appkey=" + appkey + "&sign=" + sign + "&timestamp=" + timestamp;

                    //.NET和java UrlEncode处理机制不一样，加号"+"在java里面会被替换成空格，需要转换2次
                    skuData = System.Web.HttpUtility.UrlEncode(skuData);
                    //skuData = System.Web.HttpUtility.UrlEncode(skuData);

                    //文本日志
                    ErrorLogs("url=" + url + ";postData=" + postData + "&data=" + tempdata);
                    //数据库日志
                    WMSHelper.SaveLog("putCustData", tempdata, "调用方法", "info", "out");

                    string sendResult = WMSHelper.PostData(url, postData + "&data=" + skuData);
                    string tempResult = System.Web.HttpUtility.UrlDecode(sendResult);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tempResult);
                    XmlNode node = xmlDocument.SelectSingleNode("Response/return/returnCode");

                    if (node.InnerText != "0000")
                    {
                        ErrorLogs("调用WMS推送商家失败:" + tempResult + "\n");
                        WMSHelper.SaveLog("putCustData", "", "返回结果：" + tempResult, "error", "in");

                        SmsHelper.QueueSMS(mobiles, "系统在" + DateTime.Now.ToString("yyyy年MM月HH点mm分ss秒") + "调用WMS推送商家(putCustData)接口失败，详情请查询推送日志", 3);
                    }
                    else
                    {
                        //更新最后一个推送WMS客户档案时间戳
                        this.UpdateOrderSendCoupontime(dt.Rows[dt.Rows.Count - 1]["DataVersion"].ToString());
                    }

                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogs("出现异常：" + ex.Message);
                SmsHelper.QueueSMS(mobiles, "系统在" + DateTime.Now.ToString("yyyy年MM月HH点mm分ss秒") + "调用WMS推送商家(putCustData)接口发生异常，详情请查询推送日志", 3);
            }
            if (flag)
            {
                SendSupplierData();
            }
        }

        public string CreateSupplierData(System.Data.DataTable dt)
        {
            StringBuilder strData = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                strData.Append("<xmldata>");
                //
                foreach (System.Data.DataRow dataRow in dt.Rows)
                {
                    try
                    {
                        strData.Append("<header>");
                        strData.AppendFormat("<CustomerID>{0}</CustomerID>", dataRow["SupplierId"].ToString().Trim());//客户ID,供货商ID
                        strData.AppendFormat("<Customer_Type><![CDATA[{0}]]></Customer_Type>", "VE");//供应商固定值VE
                        strData.AppendFormat("<Descr_C><![CDATA[{0}]]></Descr_C>", dataRow["SupplierName"].ToString().Trim());//中文描述
                        strData.AppendFormat("<Descr_E><![CDATA[{0}]]></Descr_E>", "");//英文描述
                        strData.AppendFormat("<Address1><![CDATA[{0}]]></Address1>", "");//地址1   
                        strData.AppendFormat("<Address2><![CDATA[{0}]]></Address2>", "");//地址2 
                        strData.AppendFormat("<Address3><![CDATA[{0}]]></Address3>", "");//地址3 

                        strData.AppendFormat("<Country><![CDATA[{0}]]></Country>", "");//国家代码 
                        strData.AppendFormat("<Province><![CDATA[{0}]]></Province>", "");//省份 
                        strData.AppendFormat("<City><![CDATA[{0}]]></City>", "");//城市
                        strData.AppendFormat("<Contact1><![CDATA[{0}]]></Contact1>", dataRow["ShopOwner"].ToString().Trim());//联系人1
                        strData.AppendFormat("<Contact1_Tel1><![CDATA[{0}]]></Contact1_Tel1>", dataRow["Mobile"].ToString().Trim());//电话（手机号）
                        strData.AppendFormat("<Contact1_Tel2><![CDATA[{0}]]></Contact1_Tel2>", dataRow["Phone"].ToString().Trim());//电话（固话）
                        strData.AppendFormat("<Contact1_Fax><![CDATA[{0}]]></Contact1_Fax>", "");//传真
                        strData.AppendFormat("<Contact1_Title><![CDATA[{0}]]></Contact1_Title>", "");//职位1
                        strData.AppendFormat("<Contact1_Email><![CDATA[{0}]]></Contact1_Email>", "");//电子邮件地址1

                        strData.Append("</header>");
                    }
                    catch
                    {
                    }
                }
                strData.Append("</xmldata>");
            }
            return strData.ToString().Trim();
        }

        /// <summary>
        /// 文本日志
        /// </summary>
        /// <param name="list"></param>
        public static void ErrorLogs(string remark)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"/LogSupplierSendWMS";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string FileNames = DateTime.Now.ToString("yyyyMMdd");
                StreamWriter sw = new StreamWriter(path + @"/" + FileNames + ".log", true);

                sw.Write(remark);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }


        protected string GetOrderSendCoupontime()
        {
            string setdate = string.Empty;
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("select Value FROM Ecshop_SiteSetting WHERE [Key]= 'SupplierJobs';");
            object a = database.ExecuteScalar(sqlStringCommand);
            if (a == null)
            {
                setdate = "";
            }
            else
            {
                setdate = a.ToString();
            }

            return setdate;
        }

        protected void UpdateOrderSendCoupontime(string dataVersion)
        {

            //string setdate = string.Empty;
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("select Value FROM Ecshop_SiteSetting WHERE [Key]= 'SupplierJobs';");
            object a = database.ExecuteScalar(sqlStringCommand);
            if (a == null)
            {
                //setdate = Convert.ToInt64(dataVersion, 16).ToString();
                sqlStringCommand = database.GetSqlStringCommand("insert into Ecshop_SiteSetting([Key],[Value]) values('SupplierJobs',@Value)");
                database.AddInParameter(sqlStringCommand, "Value", DbType.String, dataVersion);
                database.ExecuteNonQuery(sqlStringCommand);
            }
            else
            {
                //setdate = Convert.ToInt64(dataVersion, 16).ToString();
                sqlStringCommand = database.GetSqlStringCommand("update  Ecshop_SiteSetting set Value=@Value  WHERE [Key]= 'SupplierJobs';");
                database.AddInParameter(sqlStringCommand, "Value", DbType.String, dataVersion);
                database.ExecuteNonQuery(sqlStringCommand);
            }


        }

    }
}
