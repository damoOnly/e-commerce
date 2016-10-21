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
using Microsoft.Practices.EnterpriseLibrary.Data;
using EcShop.Core.ErrorLog;
using EcShop.ControlPanel.Store;

namespace WMS.Jobs
{
    public class SKUJob : IJob
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

        public void Execute(System.Xml.XmlNode node)
        {
            ErrorLog.Write("执行了SKUJob");
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
                XmlAttribute mobilesAttribute = node.Attributes["mobiles"];

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

                this.SendSKUData();
            }
        }


        private void SendSKUData()
        {
            ErrorLog.Write("执行了SKUJob->SendSKUData");
            try
            {

                ProductBrowseQuery query = new ProductBrowseQuery();
                query.PageIndex = pageIndex;
                query.PageSize = 100;
                query.DateContrastType = dateContrastType;
                query.DateContrastValue = dateContrastValue;
                query.SendWMSCount = sendWMSCount;

                ErrorLog.Write("执行了SKUJob->SendSKUData");
                DataTable dt = (DataTable)ProductBrowser.GetWMSBrowseProductList(query).Data;

                if (dt != null && dt.Rows.Count > 0)
                {
                    pageIndex++;
                    flag = true;

                    string skuData = CreateSKUData(dt);
                    //string skuData="<xmldata><header><CustomerID>61</CustomerID><SKU>3500_0</SKU><Active_Flag>Y</Active_Flag><Descr_C>法国MEDICOX雪亮+白晳精华 30ML</Descr_C><Descr_E></Descr_E><GrossWeight></GrossWeight><NetWeight>0.00</NetWeight><Cube></Cube><Price>100.00</Price><SKULength></SKULength><SKUWidth></SKUWidth><SKUHigh></SKUHigh><CycleClass>A</CycleClass></header></xmldata>";//

                    //WMS 无法正常接收部分字符
                    skuData = skuData.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("+", "＋");
                    string tempdata = appSecret + skuData + appSecret;

                    string md5tempdata = WMSHelper.MD5(tempdata);
                    string basetempdata = WMSHelper.EncodingString(md5tempdata.ToLower(), System.Text.Encoding.UTF8);
                    sign = System.Web.HttpUtility.UrlEncode(basetempdata.ToUpper(), System.Text.Encoding.UTF8);


                    //
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//YYYY-MM-DD HH:MM:SS

                    string postData = "method=putSKUData&client_customerid=" + customerid + "&client_db=" + client_db + "&messageid=SKU&apptoken=" + apptoken + "&appkey=" + appkey + "&sign=" + sign + "&timestamp=" + timestamp;

                    //.NET和java UrlEncode处理机制不一样，加号"+"在java里面会被替换成空格，需要转换2次
                    skuData = System.Web.HttpUtility.UrlEncode(skuData);
                    //skuData = System.Web.HttpUtility.UrlEncode(skuData);

                    ErrorLogs("url=" + url + ";postData=" + postData + "&data=" + tempdata);
                    //
                    WMSHelper.SaveLog("putSKUData", tempdata, "调用方法", "info", "out");
                    //
                    string sendResult = WMSHelper.PostData(url, postData + "&data=" + skuData);
                    string tempResult = System.Web.HttpUtility.UrlDecode(sendResult);


                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tempResult);
                    XmlNode node = xmlDocument.SelectSingleNode("Response/return/returnCode");
                    XmlNode nodeFlag = xmlDocument.SelectSingleNode("Response/return/returnFlag");
                    //先全部修改成成功
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //更新订单表同步wms成功状态
                        this.UpdateSKUSendStatus(dt.Rows[i]["ProductId"].ToString(), "1");
                    }
                    //
                    //0
                    if (node.InnerText != "0000")
                    {
                        ErrorLogs("调用WMS推送SKU失败:" + tempResult + "\n");
                        WMSHelper.SaveLog("putSKUData", "", "返回结果：" + tempResult, "error", "in");
                        if (nodeFlag.InnerText == "0")
                        {
                            //node.InnerText == "0001" &&
                            //全部失败
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //更新订单表同步wms成功状态
                                this.UpdateSKUSendStatus(dt.Rows[i]["ProductId"].ToString(), "0");
                            }
                        }
                        else if (nodeFlag.InnerText == "2")
                        {
                            //node.InnerText == "0001" &&
                            foreach (XmlNode infonode in xmlDocument.SelectNodes("Response/return/resultInfo"))
                            {
                                string skuId = infonode.SelectSingleNode("SKU").InnerText;
                                //OrderHelper.UpdateOrderWMSStatus(orderno, 0);
                                this.UpdateSKUSendStatusBySKUId(skuId, "0");
                            }
                        }

                        SmsHelper.QueueSMS(mobiles, "系统在" + DateTime.Now.ToString("yyyy年MM月HH点mm分ss秒") + "调用WMS推送SKU(putSKUData)接口失败，详情请查询推送日志", 3);
                    }
                }
                else
                {
                    flag = false;
                }

            }
            catch (Exception ex)
            {
                ErrorLogs("出现异常：" + ex.Message + "\n");
                SmsHelper.QueueSMS(mobiles, "系统在" + DateTime.Now.ToString("yyyy年MM月HH点mm分ss秒") + "调用WMS推送SKU(putSKUData)接口发生异常，详情请查询推送日志", 3);
            }
            if (flag)
            {
                SendSKUData();
            }
        }

        public string CreateSKUData(System.Data.DataTable dt)
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
                        strData.AppendFormat("<CustomerID><![CDATA[{0}]]></CustomerID>", "SINCNET");//客户ID,固定值SINCNET
                        strData.AppendFormat("<SKU><![CDATA[{0}]]></SKU>", dataRow["SkuId"].ToString().Trim());//SKU
                        strData.AppendFormat("<Active_Flag><![CDATA[{0}]]></Active_Flag>", "Y");//激活标记,默认为”Y”
                        strData.AppendFormat("<Alternate_SKU2>{0}</Alternate_SKU2>", dataRow["SupplierId"].ToString().Trim());//供货商ID
                        strData.AppendFormat("<Descr_C><![CDATA[{0}]]></Descr_C>", dataRow["ProductName"].ToString().Trim() + dataRow["strAttName"].ToString().Trim());//中文描述
                        strData.AppendFormat("<Descr_E>{0}</Descr_E>", "");//英文描述
                        strData.AppendFormat("<GrossWeight>{0}</GrossWeight>", Math.Round(Convert.ToDecimal(dataRow["GrossWeight"]), 2).ToString().Trim());//毛重量                        
                        strData.AppendFormat("<NetWeight>{0}</NetWeight>", Math.Round(Convert.ToDecimal(dataRow["Weight"]), 2).ToString().Trim());//净重量
                        strData.AppendFormat("<Cube>{0}</Cube>", "");//体积
                        strData.AppendFormat("<Price>{0}</Price>", Math.Round(Convert.ToDecimal(dataRow["SalePrice"]), 2).ToString().Trim());//价格

                        strData.AppendFormat("<SKULength>{0}</SKULength>", "");//长
                        strData.AppendFormat("<SKUWidth>{0}</SKUWidth>", "");//宽
                        strData.AppendFormat("<SKUHigh>{0}</SKUHigh>", "");//高
                        strData.AppendFormat("<CycleClass><![CDATA[{0}]]></CycleClass>", "A");//循环级别 （A 、B、 C ）
                        strData.AppendFormat("<RESERVECODE><![CDATA[{0}]]></RESERVECODE>", "IN");//固定值：IN
                        strData.AppendFormat("<Alternate_SKU1><![CDATA[{0}]]></Alternate_SKU1>", dataRow["BarCode"].ToString().Trim());//商品条码

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
                string path = AppDomain.CurrentDomain.BaseDirectory + @"/LogSkuSendWMS";
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


        protected void UpdateSKUSendStatus(string productId, string sendStatus)
        {
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("update  Ecshop_Products set IsSendWMS=@sendStatus,SendWMSCount=ISNULL(SendWMSCount,0)+1  WHERE ProductId= @productId ;");

            database.AddInParameter(sqlStringCommand, "sendStatus", DbType.String, sendStatus);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.String, productId);
            database.ExecuteNonQuery(sqlStringCommand);
        }
        protected void UpdateSKUSendStatusBySKUId(string skuId, string sendStatus)
        {
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("update  Ecshop_Products set IsSendWMS=@sendStatus,SendWMSCount=ISNULL(SendWMSCount,0)+1  WHERE ProductId= (SELECT ProductId FROM dbo.Ecshop_SKUs WHERE SkuId=@skuId ) ;");

            database.AddInParameter(sqlStringCommand, "sendStatus", DbType.String, sendStatus);
            database.AddInParameter(sqlStringCommand, "skuId", DbType.String, skuId);
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}
