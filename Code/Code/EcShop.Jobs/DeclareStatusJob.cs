using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Security;
using System.Xml;

namespace EcShop.Jobs
{
    public class DeclareStatusJob : IJob
    {
        private string customerid = "";
        private string apptoken = "";
        private string appkey = "";
        private string appSecret = "";
        private string sign = "";
        private string timestamp = "";
        private string url = "";
        private string client_db = "";
        public void Execute(System.Xml.XmlNode node)
        {

            ErrorLog.Write("执行了DeclareStatusJob");
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


                //if (dateContrastTypeAttribute != null)
                //{
                //    try
                //    {
                //        this.dateContrastType = int.Parse(dateContrastTypeAttribute.Value, CultureInfo.InvariantCulture);
                //    }
                //    catch
                //    {
                //        this.dateContrastType = 0;
                //    }
                //}
                //if (dateContrastValueAttribute != null)
                //{
                //    try
                //    {
                //        this.dateContrastValue = int.Parse(dateContrastValueAttribute.Value, CultureInfo.InvariantCulture);
                //    }
                //    catch
                //    {
                //        this.dateContrastValue = 1;
                //    }
                //}

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

                Database database = DatabaseFactory.CreateDatabase();

                System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("exec cp_HSDocking_GetDeclareStatus");

                DataTable dt = database.ExecuteDataSet(sqlStringCommand).Tables[0];
                this.SendDeclareStatusData(dt);
            }
        }

        private void SendDeclareStatusData(DataTable dt)
        {
            ErrorLog.Write("执行了DeclareStatusJob->SendStatusData");
            try
            {
                if (dt.Rows.Count > 0)
                {
                    string sendData = CreateDeclareStatusData(dt);
                    string tempdata = appSecret + sendData.Replace("\n", "") + appSecret;
                    string md5tempdata = MD5(tempdata);
                    string basetempdata = EncodingString(md5tempdata.ToLower(), System.Text.Encoding.UTF8);
                    sign = System.Web.HttpUtility.UrlEncode(basetempdata.ToUpper(), System.Text.Encoding.UTF8);
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//YYYY-MM-DD HH:MM:SS

                    string postData = "method=orderAllowed&client_customerid=" + customerid + "&client_db=" + client_db + "&messageid=106_A&apptoken=" + apptoken + "&appkey=" + appkey + "&sign=" + sign + "&timestamp=" + timestamp;

                    sendData = System.Web.HttpUtility.UrlEncode(sendData);
                    sendData = System.Web.HttpUtility.UrlEncode(sendData);

                    ErrorLog.Write("url=" + url + ";postData=" + postData + "&data=" + tempdata);
                    string sendResult = "";
                    try
                    {
                        sendResult = PostData(url, postData + "&data=" + sendData);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //发送WMS成功
                            this.UpdateDeclareStatus(dt.Rows[i]["OrderId"].ToString(), "1","已传送WMS");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Write("PastData error message :" + ex.Message);
                    }

                    string tempResult = System.Web.HttpUtility.UrlDecode(sendResult);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tempResult);
                    XmlNode node = xmlDocument.SelectSingleNode("Response/return/returnCode");
                    XmlNode nodeFlag = xmlDocument.SelectSingleNode("Response/return/returnFlag");
                    XmlNode nodeDesc = xmlDocument.SelectSingleNode("Response/return/returnDesc");

                    if (node.InnerText == "0000")
                    {
                        if (nodeFlag.InnerText == "1")
                        {
                            //node.InnerText == "0001" &&
                            //全部失败
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //WMS接收成功
                                this.UpdateDeclareStatus(dt.Rows[i]["OrderId"].ToString(), "2", nodeDesc.InnerText);
                            }
                        }
                    }
                    else
                    {
                        ErrorLog.Write("调用WMS推送申报失败:" + tempResult + "\n");
                        //WMSHelper.SaveLog("putSKUData", "", "返回结果：" + tempResult, "error", "in");
                        if (nodeFlag.InnerText == "0" || nodeFlag.InnerText == "3")
                        {
                            //node.InnerText == "0001" &&
                            //全部失败
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //更新订单表同步wms成功状态
                                this.UpdateDeclareStatus(dt.Rows[i]["OrderId"].ToString(), "3", nodeDesc.InnerText);
                            }
                        }
                        if (nodeFlag.InnerText == "2")
                        {
                            ArrayList orderList = new ArrayList();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                orderList.Add(dt.Rows[i]["OrderId"].ToString());
                            }
                            foreach (XmlNode infonode in xmlDocument.SelectNodes("Response/return/resultInfo"))
                            {
                                string OrderNo = infonode.SelectSingleNode("OrderNo").InnerText;
                                string errordescr = infonode.SelectSingleNode("errordescr").InnerText;
                                //OrderHelper.UpdateOrderWMSStatus(orderno, 0);
                                this.UpdateDeclareStatus(OrderNo, "3", errordescr);
                                orderList.Remove(OrderNo);
                                UpdateDeclareStatus(OrderNo, "3", errordescr);
                            }
                            foreach (string orderid in orderList)
                            {
                                UpdateDeclareStatus(orderid, "2", nodeDesc.InnerText);
                            }

                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                ErrorLog.Write("SendDeclareStatusData error message :" + ex.Message);
            }
        }

        protected void UpdateDeclareStatus(string orderid, string sendStatus, string desc)
        {
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("update  hs_docking set IsSendWMS=@IsSendWMS, DescWMS=@DescWMS,SendWMSDate=getdate() WHERE OrderId= @OrderId ;");

            database.AddInParameter(sqlStringCommand, "IsSendWMS", DbType.String, int.Parse(sendStatus));
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            database.AddInParameter(sqlStringCommand, "DescWMS", DbType.String, desc);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        private string CreateDeclareStatusData(DataTable dt)
        {
            StringBuilder strData = new StringBuilder();
            strData.Append("<xmldata>");
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    strData.Append("<data>");
                    strData.AppendFormat("<OrderNo>{0}</OrderNo>", dr["OrderId"].ToString());//订单ID
                    strData.AppendFormat("<OrderType>{0}</OrderType>", "CM");
                    strData.AppendFormat("<CustomerID>{0}</CustomerID>", "SINCNET");
                    strData.AppendFormat("<WarehouseID>{0}</WarehouseID>", "WH01");

                    if (dr["DeclareStatus"].ToString() == "2")
                    {
                        strData.AppendFormat("<isClear>{0}</isClear>", "Y");//是否出库放行
                    }
                    strData.Append("</data>");
                }
                catch (Exception ex)
                { 
                    
                }
            }

            strData.Append("</xmldata>");
            return strData.ToString().Trim();
        }


        ///<summary>
        /// MD5加密
        /// </summary>
        /// <param name="toCryString">被加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string toCryString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(toCryString, "MD5");
        }

        /// <summary>
        /// 将字符串使用base64算法加密
        /// </summary>
        /// <param name="SourceString">待加密的字符串</param>
        /// <param name="Ens">System.Text.Encoding 对象，如创建中文编码集对象：
        /// System.Text.Encoding.GetEncoding("gb2312")</param>
        /// <returns>编码后的文本字符串</returns>
        public static string EncodingString(string SourceString, System.Text.Encoding Ens)
        {
            return Convert.ToBase64String(Ens.GetBytes(SourceString));
        }

        public static string PostData(string url, string postData)
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
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
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
