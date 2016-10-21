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
using EcShop.Entities.Orders;
using EcShop.ControlPanel.Sales;
using EcShop.Entities;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Sales;
using EcShop.ControlPanel.Store;

namespace WMS.Jobs
{
    public class OrderSendJob : IJob
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

        private string expressCompanyCode = "YUANTONG";
        private string[] mobiles;

        private int pageIndex = 1;
        private bool flag = false;

        public void Execute(System.Xml.XmlNode node)
        {

            ErrorLog.Write("执行了OrderSendJob");
            pageIndex = 1;
            flag = false;
            if (null != node)
            {
                //暂停开始时间
                DateTime stopBeginDatetime = DateTime.Now;
                //暂停结束时间
                DateTime stopEndDatetime = DateTime.Now;

                XmlAttribute urlAttribute = node.Attributes["apiurl"];
                XmlAttribute customeridAttribute = node.Attributes["client_customerid"];
                XmlAttribute apptokenAttribute = node.Attributes["apptoken"];
                XmlAttribute appkeyAttribute = node.Attributes["appkey"];
                XmlAttribute appSecretAttribute = node.Attributes["appSecret"];
                XmlAttribute dateContrastTypeAttribute = node.Attributes["DateContrastType"];
                XmlAttribute dateContrastValueAttribute = node.Attributes["DateContrastValue"];
                XmlAttribute clientdbAttribute = node.Attributes["client_db"];
                XmlAttribute sendWMSCountAttribute = node.Attributes["SendWMSCount"];
                XmlAttribute stopBeginDatetimeAttribute = node.Attributes["stopBeginDatetime"];
                XmlAttribute stopEndDatetimeAttribute = node.Attributes["stopEndDatetime"];
                XmlAttribute expressCompanyCodeAttribute = node.Attributes["expressCompanyCode"];
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

                if (expressCompanyCodeAttribute != null)
                {
                    expressCompanyCode = expressCompanyCodeAttribute.Value;
                }

                if (mobilesAttribute != null)
                {
                    mobiles = mobilesAttribute.InnerText.Split(new char[] { ',' });
                }

                //暂停开始时间
                if (stopBeginDatetimeAttribute != null)
                {
                    try
                    {
                        stopBeginDatetime = DateTime.Parse(stopBeginDatetimeAttribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {

                    }
                }

                //暂停结束时间
                if (stopEndDatetimeAttribute != null)
                {
                    try
                    {
                        stopEndDatetime = DateTime.Parse(stopEndDatetimeAttribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                    }
                }
                //当前时间
                DateTime tempTime = DateTime.Now;
                if (tempTime < stopBeginDatetime || tempTime > stopEndDatetime)
                {
                    this.SendOrderData();
                }
                else
                {
                    ErrorLog.Write("暂停执行OrderSendJob");
                }
                
            }
        }


        private void SendOrderData()
        {
            ErrorLog.Write("执行了OrderSendJob->SendOrderData");
            try
            {
                OrderQuery orderQuery = new OrderQuery();
                orderQuery.PageIndex = pageIndex;
                orderQuery.PageSize = 100;
                orderQuery.DateContrastType = dateContrastType;
                orderQuery.DateContrastValue = dateContrastValue;
                orderQuery.SendWMSCount = sendWMSCount;

                //ErrorLog.Write("执行了OrderSendJob->SendOrderData.GetWMSUserOrders");
                OrdersInfo userOrders = OrderHelper.GetWMSUserOrders(orderQuery);
                DataTable dt = (DataTable)userOrders.OrderTbl;

                if (dt != null && dt.Rows.Count > 0)
                //if (true)
                {
                    ErrorLog.Write("执行了OrderSendJob->SendOrderData.GetWMSUserOrders返回查询结果:" + dt.Rows.Count);
                    pageIndex++;
                    flag = true;

                    string skuData = CreateSKUData(dt);
                    //string skuData = "<xmldata><header><OrderNo>201601060896873</OrderNo><OrderType>CM</OrderType><CustomerID>SINCNET</CustomerID><SOReference2></SOReference2><SOReference5>22</SOReference5><H_EDI_01></H_EDI_01><H_EDI_02></H_EDI_02><H_EDI_03>6.00</H_EDI_03><H_EDI_04>14.40</H_EDI_04><H_EDI_05>广东省，深圳市，龙岗区</H_EDI_05><ConsigneeID>何小娟</ConsigneeID><ConsigneeName>何小娟</ConsigneeName><C_Address1>坂田南坑街道办麒麟西七巷9栋812</C_Address1><C_Province>广东省</C_Province><C_City>深圳市</C_City><C_ZIP>000000</C_ZIP><CarrierID>YUANTONG</CarrierID><UserDefine2>何小娟</UserDefine2><C_Tel1>-</C_Tel1><C_Tel2>13692207500</C_Tel2><WarehouseID>WH01</WarehouseID><Notes></Notes><detailsItem><CustomerID>SINCNET</CustomerID><SKU>10084_0</SKU><QtyOrdered_Each>1</QtyOrdered_Each><UserDefine2>38.40</UserDefine2><UserDefine1>箱</UserDefine1><GrossWeight>0</GrossWeight></detailsItem></header><header><OrderNo>201601066096339</OrderNo><OrderType>CM</OrderType><CustomerID>SINCNET</CustomerID><SOReference2></SOReference2><SOReference5>-2</SOReference5><H_EDI_01></H_EDI_01><H_EDI_02></H_EDI_02><H_EDI_03>6.00</H_EDI_03><H_EDI_04>20.40</H_EDI_04><H_EDI_05>广东省，深圳市，宝安区</H_EDI_05><ConsigneeID>邓女士</ConsigneeID><ConsigneeName>邓女士</ConsigneeName><C_Address1>龙华新区长城里程家园4栋1单元104</C_Address1><C_Province>广东省</C_Province><C_City>深圳市</C_City><C_ZIP>518000</C_ZIP><CarrierID>YUANTONG</CarrierID><UserDefine2>邓女士</UserDefine2><C_Tel1></C_Tel1><C_Tel2>13602509805</C_Tel2><WarehouseID>WH01</WarehouseID><Notes></Notes><detailsItem><CustomerID>SINCNET</CustomerID><SKU>10055_0</SKU><QtyOrdered_Each>2</QtyOrdered_Each><UserDefine2>10.30</UserDefine2><UserDefine1>包</UserDefine1><GrossWeight>0</GrossWeight></detailsItem><detailsItem><CustomerID>SINCNET</CustomerID><SKU>10071_0</SKU><QtyOrdered_Each>2</QtyOrdered_Each><UserDefine2>11.90</UserDefine2><UserDefine1>包</UserDefine1><GrossWeight>0</GrossWeight></detailsItem></header><header><OrderNo>20160106174579511</OrderNo><OrderType>CM</OrderType><CustomerID>SINCNET</CustomerID><SOReference2></SOReference2><SOReference5>26</SOReference5><H_EDI_01></H_EDI_01><H_EDI_02></H_EDI_02><H_EDI_03>7.00</H_EDI_03><H_EDI_04>83.80</H_EDI_04><H_EDI_05>广东省，深圳市，福田区</H_EDI_05><ConsigneeID>林宏涛</ConsigneeID><ConsigneeName>林宏涛</ConsigneeName><C_Address1>福强路宝田苑A1502</C_Address1><C_Province>广东省</C_Province><C_City>深圳市</C_City><C_ZIP>000000</C_ZIP><CarrierID>YUANTONG</CarrierID><UserDefine2>林宏涛</UserDefine2><C_Tel1>-</C_Tel1><C_Tel2>13828811950</C_Tel2><WarehouseID>WH01</WarehouseID><Notes></Notes><detailsItem><CustomerID>SINCNET</CustomerID><SKU>10088_0</SKU><QtyOrdered_Each>1</QtyOrdered_Each><UserDefine2>38.40</UserDefine2><UserDefine1>箱</UserDefine1><GrossWeight>0</GrossWeight></detailsItem><detailsItem><CustomerID>SINCNET</CustomerID><SKU>10089_0</SKU><QtyOrdered_Each>1</QtyOrdered_Each><UserDefine2>38.40</UserDefine2><UserDefine1>箱</UserDefine1><GrossWeight>0</GrossWeight></detailsItem></header><header><OrderNo>20160106174579521</OrderNo><OrderType>CM</OrderType><CustomerID>SINCNET</CustomerID><SOReference2></SOReference2><SOReference5>26</SOReference5><H_EDI_01></H_EDI_01><H_EDI_02></H_EDI_02><H_EDI_03>6.00</H_EDI_03><H_EDI_04>42.20</H_EDI_04><H_EDI_05>广东省，深圳市，福田区</H_EDI_05><ConsigneeID>林宏涛</ConsigneeID><ConsigneeName>林宏涛</ConsigneeName><C_Address1>福强路宝田苑A1502</C_Address1><C_Province>广东省</C_Province><C_City>深圳市</C_City><C_ZIP>000000</C_ZIP><CarrierID>YUANTONG</CarrierID><UserDefine2>林宏涛</UserDefine2><C_Tel1>-</C_Tel1><C_Tel2>13828811950</C_Tel2><WarehouseID>WH01</WarehouseID><Notes></Notes><detailsItem><CustomerID>SINCNET</CustomerID><SKU>10142_0</SKU><QtyOrdered_Each>2</QtyOrdered_Each><UserDefine2>18.10</UserDefine2><UserDefine1>支</UserDefine1><GrossWeight>0</GrossWeight></detailsItem></header><header><OrderNo>201601062078357</OrderNo><OrderType>CM</OrderType><CustomerID>SINCNET</CustomerID><SOReference2></SOReference2><SOReference5>-2</SOReference5><H_EDI_01></H_EDI_01><H_EDI_02></H_EDI_02><H_EDI_03>6.00</H_EDI_03><H_EDI_04>6.90</H_EDI_04><H_EDI_05>广东省，湛江市，雷州市</H_EDI_05><ConsigneeID>林嘉豪</ConsigneeID><ConsigneeName>林嘉豪</ConsigneeName><C_Address1>雷州市第八中学</C_Address1><C_Province>广东省</C_Province><C_City>湛江市</C_City><C_ZIP>524200</C_ZIP><CarrierID>YUANTONG</CarrierID><UserDefine2>林嘉豪</UserDefine2><C_Tel1></C_Tel1><C_Tel2>18344175319</C_Tel2><WarehouseID>WH01</WarehouseID><Notes></Notes><detailsItem><CustomerID>SINCNET</CustomerID><SKU>10055_0</SKU><QtyOrdered_Each>3</QtyOrdered_Each><UserDefine2>10.30</UserDefine2><UserDefine1>包</UserDefine1><GrossWeight>0</GrossWeight></detailsItem></header></xmldata>";

                    //WMS 无法正常接收部分字符
                    skuData = skuData.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("+", "＋");
                    string tempdata = appSecret + skuData + appSecret;

                    string md5tempdata = WMSHelper.MD5(tempdata);
                    string basetempdata = WMSHelper.EncodingString(md5tempdata.ToLower(), System.Text.Encoding.UTF8);
                    sign = System.Web.HttpUtility.UrlEncode(basetempdata.ToUpper(), System.Text.Encoding.UTF8);
                    //
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//YYYY-MM-DD HH:MM:SS

                    string postData = "method=putSOData&client_customerid=" + customerid + "&client_db=" + client_db + "&messageid=SO&apptoken=" + apptoken + "&appkey=" + appkey + "&sign=" + sign + "&timestamp=" + timestamp;

                    //
                    skuData = System.Web.HttpUtility.UrlEncode(skuData);
                    //skuData = System.Web.HttpUtility.UrlEncode(skuData);

                    ErrorLogs("url=" + url + ";postData=" + postData + "&data=" + tempdata);
                    //数据库日志
                    WMSHelper.SaveLog("putSOData", tempdata, "调用方法", "info", "out");

                    string sendResult = WMSHelper.PostData(url, postData + "&data=" + skuData);
                    string tempResult = System.Web.HttpUtility.UrlDecode(sendResult);


                    //
                    ErrorLogs("接收WMS消息:" + tempResult + "\n");
                    WMSHelper.SaveLog("putSOData", "", "返回结果：" + tempResult, "error", "in");

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tempResult);
                    XmlNode node = xmlDocument.SelectSingleNode("Response/return/returnCode");
                    XmlNode nodeFlag = xmlDocument.SelectSingleNode("Response/return/returnFlag");

                    //先全部修改成成功
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //更新订单表同步wms成功状态
                        OrderHelper.UpdateOrderWMSStatus(dt.Rows[i]["OrderId"].ToString(), 1);
                    }
                    //
                    //0
                    if (node.InnerText != "0000")
                    {
                        ErrorLogs("调用WMS推送订单失败:" + tempResult + "\n");
                        WMSHelper.SaveLog("putSOData", "", "返回结果：" + tempResult, "error", "in");
                        if (nodeFlag.InnerText == "0")
                        {
                            //node.InnerText == "0001" &&
                            //全部失败
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //更新订单表同步wms成功状态
                                OrderHelper.UpdateOrderWMSStatus(dt.Rows[i]["OrderId"].ToString(), 0);
                            }
                        }
                        else if (nodeFlag.InnerText == "2")
                        {
                            //node.InnerText == "0001" &&
                            foreach (XmlNode infonode in xmlDocument.SelectNodes("Response/return/resultInfo"))
                            {
                                string orderno = infonode.SelectSingleNode("OrderNo").InnerText;
                                OrderHelper.UpdateOrderWMSStatus(orderno, 0);
                            }
                        }

                        SmsHelper.QueueSMS(mobiles, "系统在" + DateTime.Now.ToString("yyyy年MM月HH点mm分ss秒") + "调用WMS推送订单(putSOData)接口失败，详情请查询推送日志", 3);
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
                SmsHelper.QueueSMS(mobiles, "系统在" + DateTime.Now.ToString("yyyy年MM月HH点mm分ss秒") + "调用WMS推送订单(putSOData)接口发生异常，详情请查询推送日志", 3);
            }

            if (flag)
            {
                SendOrderData();
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
                        strData.AppendFormat("<OrderNo><![CDATA[{0}]]></OrderNo>", dataRow["OrderId"].ToString().Trim());//订单号
                        strData.AppendFormat("<OrderType><![CDATA[{0}]]></OrderType>", "CM");//CM
                        strData.AppendFormat("<ExpectedShipmentTime1><![CDATA[{0}]]></ExpectedShipmentTime1>"
                            , dataRow["PayDate"].ToString().Trim().Length > 0 ? Convert.ToDateTime(dataRow["PayDate"]).ToString("yyyy-MM-dd HH:mm:ss") : "");//付款时间
                        strData.AppendFormat("<CustomerID><![CDATA[{0}]]></CustomerID>", "SINCNET");//客户ID,固定值SINCNET
                        //strData.AppendFormat("<CustomerID>{0}</CustomerID>", dataRow["SupplierId"].ToString());//货主ID
                        strData.AppendFormat("<SOReference2><![CDATA[{0}]]></SOReference2>", dataRow["ShipOrderNumber"].ToString().Trim());//快递单号
                        strData.AppendFormat("<SOReference5>{0}</SOReference5>", dataRow["PaymentTypeId"].ToString().Trim());//付款方式
                        strData.AppendFormat("<H_EDI_01>{0}</H_EDI_01>", "");//预留字段
                        strData.AppendFormat("<H_EDI_02>{0}</H_EDI_02>", "");//优惠金额
                        strData.AppendFormat("<H_EDI_03>{0}</H_EDI_03>", Math.Round(Convert.ToDecimal(dataRow["AdjustedFreight"]), 2).ToString().Trim());//买家支付运费
                        strData.AppendFormat("<H_EDI_04>{0}</H_EDI_04>", Math.Round(Convert.ToDecimal(dataRow["OrderTotal"]), 2).ToString().Trim());//支付价（总价）
                        strData.AppendFormat("<H_EDI_05><![CDATA[{0}]]></H_EDI_05>", dataRow["ShippingRegion"].ToString().Trim());//分区
                        strData.AppendFormat("<ConsigneeID><![CDATA[{0}]]></ConsigneeID>", dataRow["ShipTo"].ToString().Trim());//收货人名称
                        strData.AppendFormat("<ConsigneeName><![CDATA[{0}]]></ConsigneeName>", dataRow["ShipTo"].ToString().Trim());//收货人名称
                        //strData.AppendFormat("<C_Address1><![CDATA[{0}]]></C_Address1>", dataRow["Address"].ToString().Trim());//收货人地址
                        strData.AppendFormat("<C_Address1><![CDATA[{0}]]></C_Address1>", dataRow["Address"].ToString().Trim());//收货人地址
                        string province = "";
                        string city = "";
                        if (dataRow["ShippingRegion"] != null && dataRow["ShippingRegion"].ToString() != "")
                        {
                            province = dataRow["ShippingRegion"].ToString().Split('，')[0];
                            city = dataRow["ShippingRegion"].ToString().Split('，')[1];
                        }
                        strData.AppendFormat("<C_Province><![CDATA[{0}]]></C_Province>", province);//收货人身份s
                        strData.AppendFormat("<C_City><![CDATA[{0}]]></C_City>", city);//收货人地址

                        string tempZip = "000000";
                        //过滤邮编格式不正确的数据，WMS无法接收
                        if (dataRow["ZipCode"].ToString().Length == 6)
                        {
                            tempZip = dataRow["ZipCode"].ToString();
                        }
                        strData.AppendFormat("<C_ZIP><![CDATA[{0}]]></C_ZIP>", tempZip);//邮编
                        string tempExpressCompany = "YUANTONG";
                        if (dataRow["ExpressCompanyName"] != null && dataRow["ExpressCompanyName"].ToString() != "")
                        {
                            ExpressCompanyInfo expressinfo = ExpressHelper.FindNode(dataRow["ExpressCompanyName"].ToString());
                            if (expressinfo != null)
                            {
                                tempExpressCompany = expressinfo.Kuaidi100Code;
                            }
                            tempExpressCompany = tempExpressCompany.ToUpper();
                        }

                        // Modified by Paul @20160126
                        //strData.AppendFormat("<CarrierID><![CDATA[{0}]]></CarrierID>", "YUANTONG");//快递公司,暂时用圆通  
                        strData.AppendFormat("<CarrierID><![CDATA[{0}]]></CarrierID>", expressCompanyCode);     // 改为从配置中读取，没有配置时默认为圆通
                        // Modified by Paul @20160126 (END)

                        strData.AppendFormat("<UserDefine2><![CDATA[{0}]]></UserDefine2>", dataRow["ShipTo"].ToString().Trim());//发件人ID
                        strData.AppendFormat("<C_Tel1><![CDATA[{0}]]></C_Tel1>", dataRow["TelPhone"].ToString().Trim());//收货人电话
                        strData.AppendFormat("<C_Tel2><![CDATA[{0}]]></C_Tel2>", dataRow["CellPhone"].ToString().Trim());//收货人电话
                        strData.AppendFormat("<WarehouseID><![CDATA[{0}]]></WarehouseID>", "WH01");//仓库ID 固定：WH01
                        strData.AppendFormat("<Notes><![CDATA[{0}]]></Notes>", dataRow["Remark"].ToString().Trim());//备注

                        DataTable orderInfo = OrderHelper.GetWMSOrderItems(dataRow["OrderId"].ToString().Trim());
                        if (orderInfo != null)
                        {
                            for (int i = 0; i < orderInfo.Rows.Count; i++)
                            {
                                int conversionRelation = Int32.Parse(orderInfo.Rows[i]["ConversionRelation"].ToString());
                                if (conversionRelation == 0)
                                {
                                    conversionRelation = 1;
                                }
                                strData.Append("<detailsItem>");
                                strData.AppendFormat("<CustomerID><![CDATA[{0}]]></CustomerID>", "SINCNET");//货主 固定：SINCNET
                                strData.AppendFormat("<SKU><![CDATA[{0}]]></SKU>", orderInfo.Rows[i]["SkuId"]);//sku
                                strData.AppendFormat("<QtyOrdered_Each>{0}</QtyOrdered_Each>", conversionRelation * Int32.Parse(orderInfo.Rows[i]["Quantity"].ToString()));//订货数
                                strData.AppendFormat("<UserDefine2>{0}</UserDefine2>", Math.Round(Convert.ToDecimal(orderInfo.Rows[i]["ItemAdjustedPrice"].ToString()) / conversionRelation, 2).ToString());//销售单价
                                strData.AppendFormat("<UserDefine1><![CDATA[{0}]]></UserDefine1>", orderInfo.Rows[i]["Unit"].ToString().Trim());//单位
                                strData.AppendFormat("<GrossWeight>{0}</GrossWeight>", "0");//毛重


                                strData.Append("</detailsItem>");
                            }
                        }
                        //
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
                string path = AppDomain.CurrentDomain.BaseDirectory + @"/LogOrderSendWMS";
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
    }
}
