using Ecdev.Plugins;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Domain;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace EcShop.UI.Web.Handler
{
    public class WebService : System.Web.Services.WebService
    {
        string result = "";
        const string appSecret = "12345678";

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string WMS(string method, string sign, string timestamp, string data)
        {
            sign = HttpUtility.UrlDecode(sign, Encoding.GetEncoding("utf-8"));
            data = HttpUtility.UrlDecode(data, Encoding.GetEncoding("utf-8"));
            if (GetSign(appSecret, data) != sign)
            {
                return ReturnXML("0001", "验证失败", "0", "");
            }
            switch (method.ToLower())
            {
                case "putskudata":
                    PutSKUData(data);
                    break;

                case "adjustskudata":
                    AdjustSKUData(data);
                    break;

                case "delivergoodsdata":
                    DeliverGoodsData(data);
                    break;
                case "updatedeliverstatus":
                    UpdateDeliverStatus(data);
                    break;

                case "setgrossweight":
                    SetGrossWeight(data);
                    break;

            }
            return result;
        }

        /// <summary>
        /// EMS推送接口
        /// </summary>
        /// <param name="data"></param>
        [WebMethod]
        public string EMSExpressPushAPI()
        {
            //data = HttpUtility.UrlDecode(data, Encoding.GetEncoding("utf-8"));

            WMSHelper.SaveLog("EMSExpressPushAPI", "", "推送开始", "info", "in");

            var inputStream = HttpContext.Current.Request.InputStream;            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(inputStream);

            //WMSHelper.SaveLog("EMSExpressPushAPI", data, "调用方法", "info", "in");
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.LoadXml(data);
            if (xmlDocument == null)
            {
                result = EMSReturnXML("0", "", "格式错误");
                WMSHelper.SaveLog("EMSExpressPushAPI", "null", "xml文档格式错误", "error", "in");
                return result;
            }

            XmlNode xmlOrderList = xmlDocument.SelectSingleNode("listexpressmail");

            if (xmlOrderList == null)
            {
                result = EMSReturnXML("0", "xml格式错误", "xml格式错误");
                WMSHelper.SaveLog("EMSExpressPushAPI", "listexpressmail is null", "不包含订单信息", "error", "in");
                return result;
            }
            string ShipOrderNumbers = string.Empty;
            try
            {
                //提取xml数据
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode orderNode in xmlOrderList.SelectNodes("expressmail"))
                {
                    string ShipOrderNumber = orderNode.SelectSingleNode("mailnum").InnerText;           //快递单号
                    ShipOrderNumbers += ShipOrderNumber + ",";
                    string action = orderNode.SelectSingleNode("action").InnerText;                     //业务动作
                    string ShipOrderStatus = string.Empty;
                    //快递已发货状态
                    if (action == "50")
                    {
                        ShipOrderStatus = "2";
                    }
                    //客户已签收
                    if (action == "10")
                    {
                        ShipOrderStatus = "3";
                    }
                    if (!string.IsNullOrEmpty(ShipOrderStatus))
                    {
                        dic.Add(ShipOrderNumber, ShipOrderStatus);
                    }

                }

                if (dic.Count > 0)
                {
                    //根据推送的数据更新数据库
                    OrderHelper.UpdateShipOrderStatusByShipOrderNumber(dic);
                    WMSHelper.SaveLog("EMSExpressPushAPI", dic.Count.ToString(), "操作成功", "info", "in");
                    //return "success";
                }

                else
                {
                    WMSHelper.SaveLog("EMSExpressPushAPI", dic.Count.ToString(), "不包含订单信息", "error", "in");
                    //return "1";
                }
                result = EMSReturnXML("1", "", "推送成功");
            }

            catch (Exception ex)
            {
                result = EMSReturnXML("0", ShipOrderNumbers, ex.Message);
                WMSHelper.SaveLog("EMSExpressPushAPI", "catch", ex.ToString(), "error", "in");
            }
            return result;
        }

        /// <summary>
        /// 圆通推送接口
        /// </summary>
        /// <param name="data"></param>
        [WebMethod]
        public string YTOExpressPushAPI()
        {
            //data = HttpUtility.UrlDecode(data, Encoding.GetEncoding("utf-8"));
            XmlDocument xmlDocument = new XmlDocument();
            //WMSHelper.SaveLog("YTOExpressPushAPI", "Start", "调用方法", "info", "in");
            try
            {
                StreamReader reader = new StreamReader(HttpContext.Current.Request.InputStream);
                string xmlData = reader.ReadToEnd();
                string[] xmlArr = xmlData.Split('&');
                string[] xmlDataArr = xmlArr[0].Split('=');
                string data = HttpUtility.UrlDecode(xmlDataArr[1], Encoding.GetEncoding("utf-8"));
                xmlDocument.LoadXml(data);

                if (xmlDocument == null)
                {
                    //result = YTReturnXML("0001", "xml文档格式错误", "0");
                    WMSHelper.SaveLog("YTOExpressPushAPI", xmlDocument.ToString(), "xml文档格式错误", "error", "in");
                    return "0";
                }

                XmlNode xmlOrderList = xmlDocument.SelectSingleNode("UpdateInfo");
                if (xmlOrderList == null)
                {
                    //result = ReturnXML("0001", "不包含订单信息", "0", "");
                    WMSHelper.SaveLog("YTOExpressPushAPI", xmlDocument.ToString(), "不包含订单信息", "error", "in");
                    return "1";
                }

                //提取xml数据
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string ShipOrderNumber = xmlOrderList.SelectSingleNode("txLogisticID").InnerText;           //快递单号
                string action = xmlOrderList.SelectSingleNode("infoContent").InnerText;                     //业务动作
                string ShipOrderStatus = string.Empty;
                //快递已发货状态
                if (action == "DEPARTURE")
                {
                    ShipOrderStatus = "2";
                }
                //客户已签收
                if (action == "SIGNED")
                {
                    ShipOrderStatus = "3";
                }
                if (!string.IsNullOrEmpty(ShipOrderStatus))
                {
                    dic.Add(ShipOrderNumber, ShipOrderStatus);
                }

                if (dic.Count > 0)
                {
                    //根据推送的数据更新数据库
                    OrderHelper.UpdateShipOrderStatusByShipOrderNumber(dic);
                    result = YTOReturnXML(ShipOrderNumber, "true");
                    WMSHelper.SaveLog("YTOExpressPushAPI", xmlDocument.ToString(), "操作成功", "info", "in");
                }

                else
                {
                    //result = ReturnXML("0001", "不包含订单信息", "0", "");
                    WMSHelper.SaveLog("YTOExpressPushAPI", xmlDocument.ToString(), "不包含订单信息", "error", "in");
                }
            }

            catch (Exception ex)
            {
                result = YTOReturnXML("error", "false");
                WMSHelper.SaveLog("YTOExpressPushAPI", xmlDocument.ToString(), "异常：" + ex.ToString(), "error", "in");
            }
            //HttpContext.Current.Response.Write(result);
            return result;
        }


        /// <summary>
        /// 入库回传接口
        /// </summary>
        /// <param name="apptoken"></param>
        /// <param name="appkey"></param>
        /// <param name="sign"></param>
        /// <param name="timestamp"></param>
        /// <param name="data"></param>
        private void PutSKUData(string data)
        {

            WMSHelper.SaveLog("PutSKUData", data, "调用方法", "info", "in");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);

            if (xmlDocument == null)
            {
                result = ReturnXML("0001", "xml文档格式错误", "0", "");
                WMSHelper.SaveLog("PutSKUData", data, "xml文档格式错误", "error", "in");
                return;
            }

            XmlNode skulist = xmlDocument.SelectSingleNode("xmldata/skulist");

            if (skulist == null)
            {
                result = ReturnXML("0001", "不包含入库信息", "0", "");
                WMSHelper.SaveLog("PutSKUData", data, "不包含入库信息", "error", "in");
                return;
            }

            try
            {
                List<SimpleSKUUpdateInfo> list = new List<SimpleSKUUpdateInfo>();
                foreach (XmlNode skuNode in skulist.SelectNodes("item"))
                {
                    SimpleSKUUpdateInfo info = new SimpleSKUUpdateInfo();
                    info.SkuId = skuNode.SelectSingleNode("skuid").InnerText;
                    info.Amount = int.Parse(skuNode.SelectSingleNode("amount").InnerText);
                    list.Add(info);
                }

                bool flag = false;
                if (list.Count > 0)
                {
                    List<ErrorSimpleSKUUpdateInfo> errorlist;
                    flag = ProductHelper.AddSkusStock(list, out errorlist);

                    if (flag && errorlist.Count == 0)
                    {
                        result = ReturnXML("0000", "操作成功", "1", "");
                        WMSHelper.SaveLog("PutSKUData", data, "操作成功", "info", "in");
                        //商品入库状态及数量修改
                        UpdateEntrepotStatus(data, xmlDocument, list, flag);
                        return;
                    }
                    else if (flag && errorlist.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (ErrorSimpleSKUUpdateInfo info in errorlist)
                        {
                            sb.Append("<resultInfo>");
                            sb.AppendFormat("<skuid>{0}</skuid>", info.SkuId);
                            sb.AppendFormat("<amount>{0}</amount>", info.Amount);
                            sb.Append("<errorcode>0001</errorcode>");
                            sb.Append("<errordescr>sku不存在</errordescr>");
                            sb.Append("</resultInfo>");
                        }
                        result = ReturnXML("0001", "部分操作成功", "2", sb.ToString());
                        WMSHelper.SaveLog("PutSKUData", data, "部分操作成功" + sb.ToString(), "info", "in");
                        return;
                    }

                    else
                    {
                        result = ReturnXML("0001", "数据库操作异常", "0", "");
                        WMSHelper.SaveLog("PutSKUData", data, "数据库操作异常", "error", "in");
                    }
                }

                else
                {
                    result = ReturnXML("0001", "不包含入库信息", "0", "");
                    WMSHelper.SaveLog("PutSKUData", data, "不包含入库信息", "error", "in");
                    return;
                }

                
            }

            catch (Exception ex)
            {
                result = ReturnXML("0001", ex.ToString(), "0", "");
                //ErrorLog.Write(ex.ToString());
                WMSHelper.SaveLog("PutSKUData", data, ex.ToString(), "error", "in");
                return;
            }


        }

        /// <summary>
        /// 商品入库状态及数量修改
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlDocument"></param>
        /// <param name="list"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private bool UpdateEntrepotStatus(string data, XmlDocument xmlDocument, List<SimpleSKUUpdateInfo> list, bool flag)
        {
            try
            {
                XmlNode skulist = xmlDocument.SelectSingleNode("xmldata");
                //商品入库状态修改
                string PONumber = skulist.SelectSingleNode("PONumber").InnerText;
                if (!string.IsNullOrEmpty(PONumber))
                {
                    result = ReturnXML("0000", "采购入库准备：PONumber", "1", "");
                    WMSHelper.SaveLog("PutSKUData", data, "采购入库准备", "info", "in");

                    flag = ProductHelper.UpdateSkusStock(PONumber, list);
                    if (flag)
                    {
                        result = ReturnXML("0000", "采购入库成功", "1", "");
                        WMSHelper.SaveLog("PutSKUData", data, "采购入库成功", "info", "in");
                    }
                    else
                    {
                        result = ReturnXML("0001", "采购入库失败", "2", "");
                        WMSHelper.SaveLog("PutSKUData", data, "采购入库失败", "info", "in");
                    }
                }
            }
            catch (Exception ex)
            {
                WMSHelper.SaveLog("PutSKUData", data, "采购入库失败:" + ex.Message, "error", "in");
            }
            return flag;
        }

        /// <summary>
        /// 发货回传接口
        /// </summary>
        /// <param name="data"></param>
        private void DeliverGoodsData(string data)
        {
            WMSHelper.SaveLog("DeliverGoodsData", data, "调用方法", "info", "in");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);

            if (xmlDocument == null)
            {
                result = ReturnXML("0001", "xml文档格式错误", "0", "");
                WMSHelper.SaveLog("DeliverGoodsData", data, "xml文档格式错误", "error", "in");

                return;
            }




            XmlNode orderlistxml = xmlDocument.SelectSingleNode("xmldata/orderlist");

            if (orderlistxml == null)
            {
                result = ReturnXML("0001", "不包含订单信息", "0", "");
                WMSHelper.SaveLog("DeliverGoodsData", data, "不包含订单信息", "error", "in");
                return;
            }

            try
            {
                List<ErrorDeliverGoods> errorlist = new List<ErrorDeliverGoods>();
                foreach (XmlNode orderNode in orderlistxml.SelectNodes("item"))
                {



                    string orderid = orderNode.SelectSingleNode("orderid").InnerText;
                    string ShipOrderNumber = orderNode.SelectSingleNode("ShipOrderNumber").InnerText;
                    string ExpressCompanyName = orderNode.SelectSingleNode("ExpressCompanyName").InnerText;
                    string ExpressCompanyCode = orderNode.SelectSingleNode("ExpressCompanyCode ").InnerText;

                    string strExpressDate = orderNode.SelectSingleNode("ExpressDate").InnerText;

                    #region 参数判断
                    DateTime ExpressDate;

                    if (!DateTime.TryParse(strExpressDate, out ExpressDate))
                    {
                        ErrorDeliverGoods erroinfo = new ErrorDeliverGoods();
                        erroinfo.ExpressCompanyCode = ExpressCompanyCode;
                        erroinfo.orderid = orderid;
                        erroinfo.ExpressCompanyName = ExpressCompanyName;
                        erroinfo.ExpressDate = strExpressDate;
                        erroinfo.ShipOrderNumber = ShipOrderNumber;
                        erroinfo.errorcode = "0001";
                        erroinfo.errordescr = "日期格式不正确";

                        errorlist.Add(erroinfo);

                        continue;
                    }

                    if (string.IsNullOrEmpty(orderid))
                    {
                        ErrorDeliverGoods erroinfo = new ErrorDeliverGoods();
                        erroinfo.ExpressCompanyCode = ExpressCompanyCode;
                        erroinfo.orderid = orderid;
                        erroinfo.ExpressCompanyName = ExpressCompanyName;
                        erroinfo.ExpressDate = strExpressDate;
                        erroinfo.ShipOrderNumber = ShipOrderNumber;
                        erroinfo.errorcode = "0001";
                        erroinfo.errordescr = "订单id不能为空";

                        errorlist.Add(erroinfo);

                        continue;
                    }

                    if (string.IsNullOrEmpty(ExpressCompanyCode))
                    {
                        ErrorDeliverGoods erroinfo = new ErrorDeliverGoods();
                        erroinfo.ExpressCompanyCode = ExpressCompanyCode;
                        erroinfo.orderid = orderid;
                        erroinfo.ExpressCompanyName = ExpressCompanyName;
                        erroinfo.ExpressDate = strExpressDate;
                        erroinfo.ShipOrderNumber = ShipOrderNumber;
                        erroinfo.errorcode = "0001";
                        erroinfo.errordescr = "快递公司编号不能为空";

                        errorlist.Add(erroinfo);

                        continue;
                    }

                    if (string.IsNullOrEmpty(ShipOrderNumber))
                    {
                        ErrorDeliverGoods erroinfo = new ErrorDeliverGoods();
                        erroinfo.ExpressCompanyCode = ExpressCompanyCode;
                        erroinfo.orderid = orderid;
                        erroinfo.ExpressCompanyName = ExpressCompanyName;
                        erroinfo.ExpressDate = strExpressDate;
                        erroinfo.ShipOrderNumber = ShipOrderNumber;
                        erroinfo.errorcode = "0001";
                        erroinfo.errordescr = "快递单号不能为空";

                        errorlist.Add(erroinfo);

                        continue;
                    }

                    //if (string.IsNullOrEmpty(ExpressCompanyName))
                    //{
                    //    ErrorDeliverGoods erroinfo = new ErrorDeliverGoods();
                    //    erroinfo.ExpressCompanyCode = ExpressCompanyCode;
                    //    erroinfo.orderid = orderid;
                    //    erroinfo.ExpressCompanyName = ExpressCompanyName;
                    //    erroinfo.ExpressDate = strExpressDate;
                    //    erroinfo.ShipOrderNumber = ShipOrderNumber;
                    //    erroinfo.errorcode = "0001";
                    //    erroinfo.errordescr = "快递公司名称不能为空";

                    //    errorlist.Add(erroinfo);

                    //    continue;
                    //}






                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderid);
                    if (orderInfo == null)
                    {
                        ErrorDeliverGoods erroinfo1 = new ErrorDeliverGoods();
                        erroinfo1.ExpressCompanyCode = ExpressCompanyCode;
                        erroinfo1.orderid = orderid;
                        erroinfo1.ExpressCompanyName = ExpressCompanyName;
                        erroinfo1.ExpressDate = strExpressDate;
                        erroinfo1.ShipOrderNumber = ShipOrderNumber;
                        erroinfo1.errorcode = "0001";
                        erroinfo1.errordescr = "订单不存在";

                        errorlist.Add(erroinfo1);

                        continue;
                    }

                    if (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid)
                    {
                        ErrorDeliverGoods erroinfo1 = new ErrorDeliverGoods();
                        erroinfo1.ExpressCompanyCode = ExpressCompanyCode;
                        erroinfo1.orderid = orderid;
                        erroinfo1.ExpressCompanyName = ExpressCompanyName;
                        erroinfo1.ExpressDate = strExpressDate;
                        erroinfo1.ShipOrderNumber = ShipOrderNumber;
                        erroinfo1.errorcode = "0001";
                        erroinfo1.errordescr = "订单状态不是已付款";

                        errorlist.Add(erroinfo1);

                        continue;
                    }

                    #endregion

                    ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNodeByKuaidi100Code(ExpressCompanyCode);
                    string getExpressCompanyName = "";

                    if (expressCompanyInfo != null)
                    {
                        getExpressCompanyName = expressCompanyInfo.Name;
                    }
                    ShippingModeInfo shippingMode = SalesHelper.GetShippingModeByCompany(getExpressCompanyName);
                    if (shippingMode != null)
                    {
                        orderInfo.RealShippingModeId = shippingMode.ModeId;
                        orderInfo.RealModeName = shippingMode.Name;

                        orderInfo.ShippingModeId = shippingMode.ModeId;
                        orderInfo.ModeName = shippingMode.Name;
                    }

                    orderInfo.ExpressCompanyAbb = ExpressCompanyCode;
                    orderInfo.ExpressCompanyName = getExpressCompanyName;
                    orderInfo.ShipOrderNumber = ShipOrderNumber;

                    orderInfo.ShippingDate = ExpressDate;
                    if (OrderHelper.WMSSendGoods(orderInfo))
                    {
                        SendNoteInfo sendNoteInfo = new SendNoteInfo();
                        sendNoteInfo.NoteId = Globals.GetGenerateId();
                        sendNoteInfo.OrderId = orderInfo.OrderId;
                        sendNoteInfo.Operator = orderInfo.Username;
                        sendNoteInfo.Remark = "后台" + sendNoteInfo.Operator + "发货成功";
                        OrderHelper.SaveSendNote(sendNoteInfo);
                        this.PaySendGoodsNotice(orderInfo);
                        this.SendGoodsMessage(orderInfo);
                        orderInfo.OnDeliver();
                    }

                    else
                    {
                        ErrorDeliverGoods erroinfo2 = new ErrorDeliverGoods();
                        erroinfo2.ExpressCompanyCode = ExpressCompanyCode;
                        erroinfo2.orderid = orderid;
                        erroinfo2.ExpressCompanyName = ExpressCompanyName;
                        erroinfo2.ExpressDate = strExpressDate;
                        erroinfo2.ShipOrderNumber = ShipOrderNumber;
                        erroinfo2.errorcode = "0001";
                        erroinfo2.errordescr = "发货失败";

                        errorlist.Add(erroinfo2);


                    }
                }

                if (errorlist.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ErrorDeliverGoods info in errorlist)
                    {
                        sb.Append("<resultInfo>");
                        sb.AppendFormat("<orderid>{0}</orderid>", info.orderid);
                        sb.AppendFormat("<ExpressCompanyName>{0}</ExpressCompanyName>", info.ExpressCompanyName);
                        sb.AppendFormat("<ExpressCompanyCode>{0}</ExpressCompanyCode>", info.ExpressCompanyCode);
                        sb.AppendFormat("<ExpressDate>{0}</ExpressDate>", info.ExpressDate);
                        sb.AppendFormat("<errorcode>{0}</errorcode>", info.errorcode);
                        sb.AppendFormat("<errordescr>{0}</errordescr>", info.errordescr);
                        sb.Append("</resultInfo>");
                    }

                    WMSHelper.SaveLog("DeliverGoodsData", data, "部分操作成功:" + sb.ToString(), "info", "in");
                    result = ReturnXML("0001", "部分操作成功", "2", sb.ToString());
                    
                    return;
                }

                else
                {
                    WMSHelper.SaveLog("DeliverGoodsData", data, "操作成功", "info", "in");
                    result = ReturnXML("0000", "操作成功", "0", "");
                    
                    return;
                }


            }
            catch (Exception ex)
            {

                WMSHelper.SaveLog("DeliverGoodsData", data, ex.ToString(), "error", "in");
                result = ReturnXML("0001", this.FilterXMLCode(ex.ToString()), "0", "");
                
                return;
            }


        }


        private void AdjustSKUData(string data)
        {
            WMSHelper.SaveLog("AdjustSKUData", data, "调用方法", "info", "in");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);

            if (xmlDocument == null)
            {
                result = ReturnXML("0001", "xml文档格式错误", "0", "");
                WMSHelper.SaveLog("AdjustSKUData", data, "xml文档格式错误", "error", "in");
                return;
            }




            XmlNode skulist = xmlDocument.SelectSingleNode("xmldata/skulist");

            if (skulist == null)
            {
                result = ReturnXML("0001", "不包含入库信息", "0", "");
                WMSHelper.SaveLog("AdjustSKUData", data, "xml不包含入库信息", "error", "in");
                return;
            }

            try
            {
                List<SimpleSKUUpdateInfo> list = new List<SimpleSKUUpdateInfo>();
                foreach (XmlNode skuNode in skulist.SelectNodes("item"))
                {
                    SimpleSKUUpdateInfo info = new SimpleSKUUpdateInfo();
                    info.SkuId = skuNode.SelectSingleNode("skuid").InnerText;
                    info.Amount = int.Parse(skuNode.SelectSingleNode("amount").InnerText);
                    list.Add(info);
                }

                bool flag = false;


                if (list.Count > 0)
                {
                    List<ErrorSimpleSKUUpdateInfo> errorlist;
                    flag = ProductHelper.AdjustSkusStock(list, out errorlist);

                    if (flag && errorlist.Count == 0)
                    {
                        result = ReturnXML("0000", "操作成功", "1", "");
                        WMSHelper.SaveLog("AdjustSKUData", data, "操作成功", "info", "in");
                        return;
                    }
                    else if (flag && errorlist.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (ErrorSimpleSKUUpdateInfo info in errorlist)
                        {
                            sb.Append("<resultInfo>");
                            sb.AppendFormat("<skuid>{0}</skuid>", info.SkuId);
                            sb.AppendFormat("<amount>{0}</amount>", info.Amount);
                            sb.AppendFormat("<errorcode>{0}</errorcode>", info.errorcode);
                            sb.AppendFormat("<errordescr>{0}</errordescr>", info.errordescr);
                            sb.Append("</resultInfo>");
                        }
                        result = ReturnXML("0001", "部分操作成功", "2", sb.ToString());
                        WMSHelper.SaveLog("AdjustSKUData", data, "部分操作成功:" + sb.ToString(), "info", "in");
                        return;
                    }

                    else
                    {
                        result = ReturnXML("0001", "数据库操作异常", "0", "");
                        WMSHelper.SaveLog("AdjustSKUData", data, "数据库操作异常", "error", "in");
                        return;
                    }
                }

                else
                {
                    result = ReturnXML("0001", "不包含库存同步信息", "0", "");
                    WMSHelper.SaveLog("AdjustSKUData", data, "不包含库存同步信息", "error", "in");
                    return;
                }
            }

            catch (Exception ex)
            {
                result = ReturnXML("0001", ex.ToString(), "0", "");
                WMSHelper.SaveLog("AdjustSKUData", data, ex.ToString(), "error", "in");
                return;
            }


        }


        /// <summary>
        /// 毛重回传
        /// </summary>
        /// <param name="data"></param>
        private void UpdateDeliverStatus(string data)
        {
            WMSHelper.SaveLog("UpdateDeliverStatus", data, "调用方法", "info", "in");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);

            if (xmlDocument == null)
            {
                result = ReturnXML("0001", "xml文档格式错误", "0", "");
                WMSHelper.SaveLog("UpdateDeliverStatus", data, "xml文档格式错误", "error", "in");
                return;
            }


            XmlNode xmlOrderList = xmlDocument.SelectSingleNode("xmldata/orderlist");

            if (xmlOrderList == null)
            {
                result = ReturnXML("0001", "不包含订单信息", "0", "");
                WMSHelper.SaveLog("UpdateDeliverStatus", data, "不包含订单信息", "error", "in");
                return;
            }

            try
            {
                List<DeliverStatusInfo> list = new List<DeliverStatusInfo>();
                foreach (XmlNode orderNode in xmlOrderList.SelectNodes("item"))
                {
                    DeliverStatusInfo info = new DeliverStatusInfo();
                    info.OrderId = orderNode.SelectSingleNode("orderid").InnerText;
                    info.OrderStatus = int.Parse(orderNode.SelectSingleNode("OrderStatus").InnerText);
                    info.Describe = orderNode.SelectSingleNode("Describe").InnerText;
                    info.Warehouse = orderNode.SelectSingleNode("Warehouse").InnerText;
                    info.UpdateDate = Convert.ToDateTime(orderNode.SelectSingleNode("UpdateDate").InnerText);
                    info.ShipOrderNumber = orderNode.SelectSingleNode("ShipOrderNumber").InnerText;

                    ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNodeByKuaidi100Code(orderNode.SelectSingleNode("ExpressCompanyName").InnerText);
                    string getExpressCompanyName = "";

                    if (expressCompanyInfo != null)
                    {
                        getExpressCompanyName = expressCompanyInfo.Name;
                    }

                    info.ExpressCompanyName = getExpressCompanyName;


                    list.Add(info);

                }

                bool flag = false;

                if (list.Count > 0)
                {
                    List<ErrorDeliverStatusInfo> errorlist;
                    flag = OrderHelper.AddDeliverStatus(list, out errorlist);

                    if (flag && errorlist.Count == 0)
                    {
                        result = ReturnXML("0000", "操作成功", "1", "");
                        WMSHelper.SaveLog("UpdateDeliverStatus", data, "操作成功", "info", "in");
                        return;
                    }
                    else if (flag && errorlist.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (ErrorDeliverStatusInfo info in errorlist)
                        {
                            sb.Append("<resultInfo>");
                            sb.AppendFormat("<orderid>{0}</orderid>", info.OrderId);
                            sb.AppendFormat("<OrderStatus>{0}</OrderStatus>", info.OrderStatus);
                            sb.AppendFormat("<Describe>{0}</Describe>", info.Describe);
                            sb.AppendFormat("<Warehouse>{0}</Warehouse>", info.Warehouse);
                            sb.AppendFormat("<ShipOrderNumber>{0}</ShipOrderNumber>", info.ShipOrderNumber);
                            sb.AppendFormat("<ExpressCompanyName>{0}</ExpressCompanyName>", info.ExpressCompanyName);
                            //sb.AppendFormat("<UpdateDate>{0}</UpdateDate>", info.UpdateDate.ToString("YYYY-MM-DD HH:MM:SS"));
                            sb.AppendFormat("<errorcode>{0}</errorcode>", info.errorcode);
                            sb.AppendFormat("<errordescr>{0}</errordescr>", info.errordescr);
                            sb.Append("</resultInfo>");
                        }
                        result = ReturnXML("0001", "部分操作成功", "2", sb.ToString());
                        WMSHelper.SaveLog("UpdateDeliverStatus", data, "部分操作成功:" + sb.ToString(), "info", "in");
                        return;
                    }

                    else
                    {
                        result = ReturnXML("0001", "数据库操作异常", "0", "");
                        WMSHelper.SaveLog("UpdateDeliverStatus", data, "数据库操作异常", "error", "in");
                        return;
                    }
                }

                else
                {
                    result = ReturnXML("0001", "不包含订单信息", "0", "");
                    WMSHelper.SaveLog("UpdateDeliverStatus", data, "不包含订单信息", "error", "in");
                    return;
                }



            }

            catch (Exception ex)
            {
                result = ReturnXML("0001", ex.ToString(), "0", "");
                WMSHelper.SaveLog("UpdateDeliverStatus", data, ex.ToString(), "error", "in");

                return;
            }


        }


        private void SetGrossWeight(string data)
        {
            WMSHelper.SaveLog("SetGrossWeight", data, "调用方法", "info", "in");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);

            if (xmlDocument == null)
            {
                result = ReturnXML("0001", "xml文档格式错误", "0", "");
                WMSHelper.SaveLog("SetGrossWeight", data, "xml文档格式错误", "error", "in");
                return;
            }

            XmlNode skulist = xmlDocument.SelectSingleNode("xmldata/skulist");

            if (skulist == null)
            {
                result = ReturnXML("0001", "不包含入库信息", "0", "");
                WMSHelper.SaveLog("SetGrossWeight", data, "不包含商品信息", "error", "in");
                return;
            }

            try
            {
                List<SimpleSKUUpdateInfo> list = new List<SimpleSKUUpdateInfo>();
                foreach (XmlNode skuNode in skulist.SelectNodes("item"))
                {
                    SimpleSKUUpdateInfo info = new SimpleSKUUpdateInfo();
                    info.SkuId = skuNode.SelectSingleNode("skuid").InnerText;
                    decimal grossweight;
                    decimal.TryParse(skuNode.SelectSingleNode("grossweight").InnerText,out grossweight);
                    info.GrossWeight = grossweight;
                    list.Add(info);
                }

                bool flag = false;
                if (list.Count > 0)
                {
                    List<ErrorSimpleSKUUpdateInfo> errorlist;
                    flag = ProductHelper.SetGrossWeight(list, out errorlist);

                    if (flag && errorlist.Count == 0)
                    {
                        result = ReturnXML("0000", "操作成功", "1", "");
                        WMSHelper.SaveLog("SetGrossWeight", data, "操作成功", "info", "in");
                        return;
                    }
                    else if (flag && errorlist.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (ErrorSimpleSKUUpdateInfo info in errorlist)
                        {
                            sb.Append("<resultInfo>");
                            sb.AppendFormat("<skuid>{0}</skuid>", info.SkuId);
                            sb.AppendFormat("<grossweight>{0}</grossweight>", info.GrossWeight);
                            sb.Append("<errorcode>0001</errorcode>");
                            sb.Append("<errordescr>sku不存在</errordescr>");
                            sb.Append("</resultInfo>");
                        }
                        result = ReturnXML("0001", "部分操作成功", "2", sb.ToString());
                        WMSHelper.SaveLog("SetGrossWeight", data, "部分操作成功" + sb.ToString(), "info", "in");
                        return;
                    }

                    else
                    {
                        result = ReturnXML("0001", "数据库操作异常", "0", "");
                        WMSHelper.SaveLog("SetGrossWeight", data, "数据库操作异常", "error", "in");
                    }
                }

                else
                {
                    result = ReturnXML("0001", "不包含商品信息", "0", "");
                    WMSHelper.SaveLog("SetGrossWeight", data, "不包含商品信息", "error", "in");
                    return;
                }


            }

            catch (Exception ex)
            {
                result = ReturnXML("0001", ex.ToString(), "0", "");
                //ErrorLog.Write(ex.ToString());
                WMSHelper.SaveLog("SetGrossWeight", data, ex.ToString(), "error", "in");
                return;
            }
        }



        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMessage"></param>
        /// <param name="returnFlag"></param>
        /// <returns></returns>
        private string ReturnXML(string returnCode, string returnDesc, string returnFlag, string resultInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Response>");
            sb.Append("<return>");
            sb.AppendFormat("<returnCode>{0}</returnCode>", returnCode);
            sb.AppendFormat("<returnDesc>{0}</returnDesc>", returnDesc);
            sb.AppendFormat("<returnFlag>{0}</returnFlag>", returnFlag);
            if (returnFlag == "2")
            {
                sb.Append(resultInfo);
            }
            sb.Append("</return>");
            sb.Append("</Response>");

            string str = sb.ToString();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(str);
            return str;
        }

        /// <summary>
        /// EMS推送接口返回值
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="failmailnums"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private string EMSReturnXML(string isSuccess, string failmailnums,string remark)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<response>");
            sb.AppendFormat("<success>{0}</success>", isSuccess);
            sb.AppendFormat("<failmailnums>{0}</failmailnums>", failmailnums);
            sb.AppendFormat("<remark>{0}</remark>", remark);
            sb.Append("</response>");

            string str = sb.ToString();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(str);
            str = HttpUtility.UrlDecode(str, Encoding.GetEncoding("utf-8"));
            return str;
        }

        /// <summary>
        /// EMS推送接口返回值
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="failmailnums"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private string YTOReturnXML(string txLogisticID, string success)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<response>");
            sb.AppendFormat("<logisticProviderID>YTO</logisticProviderID>");
            sb.AppendFormat("<txLogisticID>{0}</txLogisticID>", txLogisticID);
            sb.AppendFormat("<success>{0}</success>", success);
            sb.Append("</response>");

            string str = HttpUtility.UrlDecode(sb.ToString(), Encoding.GetEncoding("utf-8"));
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.LoadXml(str);
            //Server.UrlEncode(str);
            return str;
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="appSecret"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetSign(string appSecret, string data)
        {
            data = HttpUtility.UrlDecode(data, Encoding.GetEncoding("utf-8"));
            string input = appSecret + data + appSecret;

            string strResult = "";
            strResult = MD5(input);


            strResult = strResult.ToLower();
            strResult = EncodingString(strResult, System.Text.Encoding.UTF8);

            strResult = strResult.ToUpper();




            return strResult;

        }

        /// <summary>
        /// http://www.hmtest.com/handler/WebService.asmx/TestSign?appSecret=12345678&data=%3Cxmldata%3E%3Cskulist%3E%3Citem%3E%3Cskuid%3E10001_0%3C/skuid%3E%3Cgrossweight%3E105%3C/grossweight%3E%3C/item%3E%3C/skulist%3E%3C/xmldata%3E
        /// </summary>
        /// <param name="appSecret"></param>
        /// <param name="data"><xmldata><skulist><item><skuid>10001_0</skuid><grossweight>105</grossweight></item></skulist></xmldata></param>
        /// <returns></returns>
        [WebMethod]
        public string TestSign(string appSecret, string data)
        {
            data = HttpUtility.UrlDecode(data, Encoding.GetEncoding("utf-8"));
            string input = appSecret + data + appSecret;

            string strResult = "";
            strResult = MD5(input);
            //md5加密  32位小写
            //MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            //byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            //// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            //for (int i = 0; i < s.Length; i++)
            //{
            //    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 

            //    strResult = strResult + s[i].ToString("x");

            //}

            strResult = strResult.ToLower();
            strResult = EncodingString(strResult, System.Text.Encoding.UTF8);

            strResult = strResult.ToUpper();




            return strResult;

        }


        ///<summary>
        /// MD5加密
        /// </summary>
        /// <param name="toCryString">被加密字符串</param>
        /// <returns>加密后的字符串</returns>
        private string MD5(string toCryString)
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


        /// <summary>
        /// 使用缺省的代码页将字符串使用base64算法加密
        /// </summary>
        /// <param name="SourceString">待加密的字符串</param>
        /// <returns>加密后的文本字符串</returns>
        public static string EncodingString(string SourceString)
        {
            return EncodingString(SourceString, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 支付接口发货通知
        /// </summary>
        /// <param name="orderInfo"></param>
        private void PaySendGoodsNotice(OrderInfo orderInfo)
        {
            try
            {
                if (orderInfo.Gateway == "Ecdev.plugins.payment.weixinrequest")
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
                    payClient.DeliverNotify(new DeliverInfo
                    {
                        TransId = orderInfo.GatewayOrderId,
                        OutTradeNo = orderInfo.OrderId,
                        OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
                    {
                        PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
                        if (paymentMode != null && !string.IsNullOrEmpty(paymentMode.Settings) && paymentMode.Settings != "1hSUSkKQ/ENo0JDZah8KKQweixin")
                        {
                            PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
							{
								paymentMode.Gateway
							})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
							{
								paymentMode.Gateway
							})), "");
                            paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(string.Format("WMS发货回传通知支付接口出错，订单id{0},支付接口{1},错误信息{2}", orderInfo.OrderId, orderInfo.Gateway, ex.ToString()));
            }
        }


        /// <summary>
        /// 用户发货信息通知
        /// </summary>
        /// <param name="orderInfo"></param>
        private void SendGoodsMessage(OrderInfo orderInfo)
        {
            try
            {
                int num = orderInfo.UserId;
                if (num == 1100)
                {
                    num = 0;
                }
                IUser user = Users.GetUser(num);
                Messenger.OrderShipping(orderInfo, user);
            }

            catch (Exception ex)
            {
                ErrorLog.Write(string.Format("WMS发货回传用户信息通知出错，订单id{0},错误信息{1}", orderInfo.OrderId, ex.ToString()));
            }
        }

        private string FilterXMLCode(string code)
        {
            return code.Replace(">", "&gt;").Replace("<", "&lt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace(" ", "");
        }
        [WebMethod]
        public string GetPOCDInfo(string formId)
        {
            string str = "";
            if (formId.Length != 18)
            {
                str = "{\"message\":\"参数长度不对\"}";
            }
            else
            {
                str = Newtonsoft.Json.JsonConvert.SerializeObject(PurchaseOrderHelper.GetPOCDInfo(formId));
            }

            return str;
        }
        [WebMethod]
        public string SetPOCDStatus(string formId, int POStatus, string PORemark, string CommonNum)
        {
            string str = "";
            if (formId.Length != 18)
            {
                str = "{\"message\":\"参数长度不对\"}";
            }
            else
            {
                Encoding utf8 = Encoding.UTF8;
                if (PurchaseOrderHelper.SetPOCDStatus(formId, POStatus, HttpUtility.UrlDecode(PORemark, utf8), CommonNum))
                {
                    str = "{\"message\":\"更新成功\"}";
                }
                else
                { 
                    str = "{\"message\":\"更新失败\"}"; 
                }
            }
            return str;
        }

        [WebMethod]
        public string GetPOCDList(string formIDList)
        {
            string str = "";
            string formID="";
            if (formIDList.Length ==0)
            {
                str = "{\"message\":\"参数长度不对\"}";
            }
            else
            {
                POList list = new POList();
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<POList>(formIDList);

                foreach (formList formList in list.formIDList)
                {
                    formID += "'" + formList.formId + "',";
                    //formID +=  formList.formId + ",";
                }
                formID = formID.TrimEnd(',');
                str=Newtonsoft.Json.JsonConvert.SerializeObject(PurchaseOrderHelper.GetPOCDList(formID));
            }
            return str;
        }

    }

    public class POList
    {
        public List<formList> formIDList
        {
            get;
            set;
        }
    }

    public class formList
    {
        public string formId
        {
            get;
            set;
        }
    }
}
