using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;

namespace EcShop.Jobs
{
    public class WTDJob : IJob
    {
        Database database = DatabaseFactory.CreateDatabase();

        string gateway = "";
        Hashtable codeTable = new Hashtable();
        Hashtable expressCodeTable = new Hashtable();

        public void Execute(XmlNode node)
        {
            Init();

            int minutes = 10;
            XmlAttribute minutesAttr = node.Attributes["minutes"];
            if (minutesAttr != null)
            {
                int.TryParse(minutesAttr.Value, out minutes);
            }

            int cancelOrder = 30;

            XmlAttribute pushOrderAttr = node.Attributes["cancelOrder"];
            if (pushOrderAttr != null)
            {
                int.TryParse(pushOrderAttr.Value, out cancelOrder);
            }

            XmlAttribute gatewayAttr = node.Attributes["gateway"];
            if (gatewayAttr != null)
            {
                gateway = gatewayAttr.Value;
            }

            ProcessWTD(gateway, minutes, cancelOrder);
        }

        private void ProcessWTD(string gateway, int minutes, int cancelOrder)
        {
            
            /*
             * 1.处理退款订单
             */


            /*
             * 2.查询订单状态
             */

            /*
             * 3.推送新订单
             */
            ProcessOrderPush();

            /**/
            /**/
        }

        private void ProcessOrderCancel()
        {

        }

        private void ProcessOrderPush()
        {
            OrderQuery orderQuery = new OrderQuery();

            orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
            orderQuery.PageIndex = 1;
            orderQuery.PageSize = 100;

            int num = 0;

            Globals.EntityCoding(orderQuery, true);

            DataSet tradeOrders = OrderHelper.GetTradeOrders(orderQuery, out num);

            foreach (DataRow row in tradeOrders.Tables[0].Rows)
            {
                string orderId = row["OrderId"].ToString();

                WTDOrder order = new WTDOrder();

                order.orderId = orderId;
                order.orderDate = ((DateTime)row["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                order.addr = row["ShippingRegion"].ToString() + " " + row["Address"].ToString();
                string shippingRegion = row["ShippingRegion"].ToString();    //广东省，深圳市，罗湖区
                string[] region = shippingRegion.Split(new char[] { '，' });
                order.province = region.Length > 0 ? region[0] : "";
                order.city = region.Length > 1 ? region[1] : "";
                order.area = region.Length > 2 ? region[2] : "";
                order.name = row["ShipTo"].ToString();
                order.cardNo = row["IdentityCard"].ToString();
                if (order.cardNo == "")
                {
                    order.cardNo = "41132819811229002X";    // 测试处理，上线去掉!!!
                }
                order.cardType = "0";
                order.phone = row["CellPhone"].ToString();
                if (string.IsNullOrEmpty(order.phone))
                {
                    order.phone = row["TelPhone"].ToString();
                }
                order.expressCode = GetExpressCode(row["ExpressCompanyAbb"].ToString());
                order.freight = decimal.Parse(row["AdjustedFreight"].ToString());
                order.insuredFee = 0;          
                order.busiMode = "BBC";
                order.whCode = "FLC";
                order.portCode = "5141";

                decimal total = 0M;

                DataRow[] childRows = row.GetChildRows("OrderRelation");

                for (int i = 0; i < childRows.Length; i++)
                {
                    DataRow skuRow = childRows[i];

                    string skuContent = Globals.HtmlEncode(skuRow["SKUContent"].ToString());

                    WTDOrderItem orderItem = new WTDOrderItem();

                    orderItem.price = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString());
                    orderItem.productId = skuRow["SKU"].ToString();
                    orderItem.productName = skuRow["ItemDescription"].ToString();
                    orderItem.qty = int.Parse(skuRow["Quantity"].ToString());
                    orderItem.total = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString()) * int.Parse(skuRow["Quantity"].ToString());
                    total += orderItem.total;

                    order.orderItemList.orderItems.Add(orderItem);
                }

                order.total = total + order.freight;

                // 
                string callParams = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                string result = DoCall("wtdex.trade.order.add", callParams);

                try
                {
                    WTDResponse errorResult = JsonConvert.DeserializeObject<WTDResponse>(result);

                    if (errorResult != null)
                    {
                        string errorDesc = errorResult.response.desc;
                        if (errorResult.response.code != "1")
                        {
                            errorDesc = errorDesc + ": ";

                            for (int i = 0; i < errorResult.response.errorCount; i++ )
                            {
                                var err = errorResult.response.errorInfoList.errorInfos[i];

                                string desc = string.Format("err {0}: ,code: {1}, msg: {2}\r\n", i, err.errCode, err.errMsg);
                            }
                        }

                        OrderHelper.InsertOrderPush("WTD", order.orderId, int.Parse(errorResult.response.code), errorDesc);
                    }
                    else
                    {
                        OrderHelper.InsertOrderPush("WTD", order.orderId, -1, result);
                    }
                }
                catch (Exception ex)
                {
                    OrderHelper.InsertOrderPush("WTD", order.orderId, -1, ex.Message);
                }
            }
        }

        private void ProcessOrderQuery()
        {

            /*
            param.Add("method", "wtdex.trade.express.get");

            string orderId = "11001000345610S0004";
            param.Add("params", string.Format("{{\"expressNo\":null,\"state\":null,\"orderId\":\"{0}\"}}", orderId));*/

            int pageIndex = 1;
            int pageSize = 50;

            DbQueryResult result = OrderHelper.GetPushOrders("WTD", 1, 0, pageIndex, pageSize);

            int num = 0;

            if (result != null)
            {
                int totalRecords = result.TotalRecords;

                if (totalRecords > 0)
                {
                    int pageCount = totalRecords / pageSize;
                    if (totalRecords % pageSize == 0)
                    {
                        pageCount++;
                    }

                    DataTable dt = result.Data as DataTable;


                }
            }
        }

        private void ProcessOrderExpress(DataTable dt)
        {
            if (dt != null)
            {
                List<string> orderIds = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string orderId = row["OrderId"].ToString();

                    orderIds.Add(orderId);
                }

                string callParams = string.Format("{{\"expressNo\":null,\"state\":null,\"orderId\":\"{0}\"}}", string.Join<string>(",", orderIds));

                string result = DoCall("wtdex.trade.express.get", callParams);

                try
                {
                    WTDOrderExpressQueryResponse responseResult = JsonConvert.DeserializeObject<WTDOrderExpressQueryResponse>(result);

                    if (responseResult != null)
                    {
                        if (responseResult.response.code != "1")
                        {
                            Dictionary<string, string> orders = new Dictionary<string, string>();

                            for (int i = 0; i < responseResult.response.orderList.orders.Count; i++)
                            {
                                var current = responseResult.response.orderList.orders[i];

                                orders.Add(current.orderId, current.expreeNo);
                            }

                            OrderHelper.UpdatePushOrders("WTD", orders);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private string GetExpressCode(string expressAbb)
        {
            return "sf";

            if (expressCodeTable.ContainsKey(expressAbb))
            {
                return expressCodeTable[expressAbb].ToString();
            }

            return string.Empty;
        }

        private void Init()
        {
            // express code mapping
            expressCodeTable = new Hashtable();

            expressCodeTable.Add("shufeng", "SF");
            expressCodeTable.Add("yuantong", "YT");

            // response code
            codeTable = new Hashtable();
            codeTable.Add("1", "操作成功");
            codeTable.Add("0", "操作失败");
            codeTable.Add("2000", "参数不合法");
            codeTable.Add("2001", "企业不存在");
            codeTable.Add("2002", "无访问权限");
            codeTable.Add("2003", "token错误");
            codeTable.Add("2004", "token已失效");
            codeTable.Add("2005", "密码错误");
            codeTable.Add("2006", "ip不合法");
            codeTable.Add("2007", "未查询到相关数据");
            codeTable.Add("3000", "请求参数为空");
            codeTable.Add("3001", "企业代号为空");
            codeTable.Add("3002", "企业密码为空");
            codeTable.Add("3003", "token为空");
            codeTable.Add("3004", "企业订单编号为空");
            codeTable.Add("3005", "运单号为空");
            codeTable.Add("5005", "系统运行发生异常");

            codeTable.Add("110", "请求参数为空");
            codeTable.Add("111", "订单已经存在");
            codeTable.Add("112", "订单商品数量在1个以上并且订单总价超过1000");
            codeTable.Add("113", "收件人名不能含有“先生”、“小姐”字眼");
            codeTable.Add("114", "商品编码和商品库存不一致");
            codeTable.Add("115", "商品库存不足");
            codeTable.Add("116", "数价不符(数量乘以单价不等于总价)");
            codeTable.Add("117", "单价超限制");
            codeTable.Add("118", "收件人联系电话必需为数字");
            codeTable.Add("119", "身份证不合法");
            codeTable.Add("120", "收货地址不规范");
            codeTable.Add("121", "商品没有备案");
            codeTable.Add("122", "订单已经提交海关进境");
            codeTable.Add("123", "未配置仓库信息或者库存信息");
            codeTable.Add("124", "商品不存在");
            codeTable.Add("125", "新增时数据已经存在");
            codeTable.Add("126", "仓库编码错误");
            codeTable.Add("127", "海关编码错误");
            codeTable.Add("128", "业务类型错误");
            codeTable.Add("129", "存在相同的商品编码");
            codeTable.Add("130", "物流公司编码错误");
            codeTable.Add("131", "商品没有备案号");
            codeTable.Add("132", "库存不足");
            codeTable.Add("505", "系统运行异常");

            codeTable.Add("10100", "收到订单信息");
            codeTable.Add("10200", "进境清关");
            codeTable.Add("10300", "分拣包装");
            codeTable.Add("10400", "保税仓出区");
            codeTable.Add("10500", "国内派送");
            codeTable.Add("10600", "收货成功");
            codeTable.Add("20100", "收到订单信息");
            codeTable.Add("20200", "海外仓待入库");
            codeTable.Add("20023", "待拣货");
            codeTable.Add("20026", "拣货中");
            codeTable.Add("20300", "海外仓出库");
            codeTable.Add("20400", "国际运输");
            codeTable.Add("20500", "货已到岸");
            codeTable.Add("20600", "进境清关");
            codeTable.Add("20060", "等待出区");
            codeTable.Add("20700", "国内派送");
            codeTable.Add("50", "已发货");
            codeTable.Add("3036", "在途中");
            codeTable.Add("44", "正在派件");
            codeTable.Add("8000", "签收人是：×××");
            codeTable.Add("80", "派送成功");
            codeTable.Add("70", "派件不成功");
            codeTable.Add("070", "拒收");

        }

        private string DoCall(string method, string callParam)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();

            string appkey = "955c2d2c-b606-48b0-be08-d76ed965fc2d",
                format = "json", timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), token = "45f4f1dc-e5a6-41c0-b715-fbf8889b3495", v = "1.0";
            string secret = "ef9c1a0a-ffb5-4112-8bec-c51f3b9beaaf";

            param.Add("appkey", appkey);
            param.Add("token", token);
            param.Add("timestamp", timestamp);
            param.Add("v", v);
            param.Add("format", format);
            param.Add("method", method);
            param.Add("params", callParam);
            string sign = Sign.GetPasswordEncoded(secret + appkey + format + method + callParam + timestamp + token + v + secret);
            param.Add("sign", sign);

            return DoPost(gateway, param);
        }

        private string DoPost(string url, SortedDictionary<string, string> param)
        {
            string result = string.Empty;

            try
            {
                string postData = "";

                StringBuilder sbParam = new StringBuilder();

                foreach (var current in param)
                {
                    if (sbParam.Length > 0)
                    {
                        sbParam.Append("&");
                    }

                    sbParam.AppendFormat("{0}={1}", current.Key, current.Value.ToString());
                }

                postData = sbParam.ToString();

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
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = responseStream;

                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(responseStream, CompressionMode.Decompress);
                        }

                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(responseStream, CompressionMode.Decompress);
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
                result = string.Format("获取信息错误：{0}", exception.Message);
            }

            return result;
        }
    }

    public class Sign
    {
        public static string GetPasswordEncoded(string password)
        {
            return EncodeBase64(ToMD5(password));
        }


        private static string EncodeBase64(string password)
        {
            string encode = "";
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = password;
            }
            return encode;
        }

        private static string ToMD5(string password)
        {
            byte[] result = Encoding.UTF8.GetBytes(password);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }
    }

    #region 订单推送

    public class WTDErrorInfo
    {
        public string errCode { get; set; }
        public string errPath { get; set; }
        public string errMsg { get; set; }

    }

    public class WTDErrorInfoList
    {
        public WTDErrorInfoList()
        {
            errorInfos = new List<WTDErrorInfo>();
        }

        public List<WTDErrorInfo> errorInfos;
    }

    public class WTDResponseResult
    {
        public WTDResponseResult()
        {
            errorInfoList = new WTDErrorInfoList();
        }
        public string code { get; set; }
        public string desc { get; set; }
        public int updateCount { get; set; }
        public int errorCount { get; set; }
        public WTDErrorInfoList errorInfoList { get; set; }
    }

    public class WTDResponse
    {
        public WTDResponseResult response { get; set; }
    }

    public class WTDOrder
    {
        public WTDOrder()
        {
            orderItemList = new WTDOrderItemList();
            extraInfoList = new WTDExtraInfoList();
        }
        public string orderId { get; set; }
        public string orderDate { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string addr { get; set; }
        public string zipCode { get; set; }
        public string cardType { get; set; }
        public string cardNo { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string invoiceHead { get; set; }
        public string invoiceType { get; set; }
        public string invoice { get; set; }
        public decimal total { get; set; }
        public decimal freight { get; set; }
        public decimal insuredFee { get; set; }
        public string seller { get; set; }
        public string notes { get; set; }
        public string portCode { get; set; }
        public string ieFlag { get; set; }
        public string busiMode { get; set; }
        public string pickMode { get; set; }
        public string whCode { get; set; }
        public string expressCode { get; set; }
        public string expressNo { get; set; }
        public string destCity { get; set; }
        public WTDOrderItemList orderItemList { get; set; }
        public WTDExtraInfoList extraInfoList { get; set; }

    }

    public class WTDOrderItem
    {
        public string productId { get; set; }
        public string productName { get; set; }
        public int qty { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
        public string isGroup { get; set; }
        public string groupBarcode { get; set; }

    }

    public class WTDOrderItemList
    {
        public WTDOrderItemList()
        {
            orderItems = new List<WTDOrderItem>();
        }

        public List<WTDOrderItem> orderItems;
    }

    public class WTDExtraInfo
    {
        public string code { get; set; }
        public string userDefined { get; set; }
        public string userDefined1 { get; set; }
        public string userDefined2 { get; set; }
        public string userDefined3 { get; set; }
        public string userDefined4 { get; set; }
        public string userDefined5 { get; set; }
        public string userDefined6 { get; set; }
        public string userDefined7 { get; set; }
        public string userDefined8 { get; set; }
        public string userDefined9 { get; set; }

    }

    public class WTDExtraInfoList
    {
        public WTDExtraInfoList()
        {
            extraInfos = new List<WTDExtraInfo>();
        }

        public List<WTDExtraInfo> extraInfos;
    }

    #endregion

    #region 订单删除



    #endregion

    #region 订单查询

    public class WTDOrderQueryResponseResult
    {
        public WTDOrderQueryResponseResult()
        {
            orderList = new WTDOrderQueryList();
        }
        public string code { get; set; }
        public string desc { get; set; }
        public int count { get; set; }
        public WTDOrderQueryList orderList { get; set; }

    }

    public class WTDOrderQueryList
    {
        public WTDOrderQueryList()
        {
            orders = new List<WTDOrderQuery_Order>();
        }

        public List<WTDOrderQuery_Order> orders;
    }

    public class WTDOrderQueryResponse
    {
        public WTDOrderQueryResponseResult response { get; set; }
    }

    public class WTDOrderQuery_Order
    {
        public string orderId { get; set; }
        public string orderDate { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string addr { get; set; }
        public string zipCode { get; set; }
        public string cardType { get; set; }
        public string cardNo { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string invoiceHead { get; set; }
        public string invoiceType { get; set; }
        public string invoice { get; set; }
        public decimal freight { get; set; }
        public decimal insuredFee { get; set; }
        public string seller { get; set; }
        public string notes { get; set; }
        public string expressName { get; set; }
        public string expreeNo { get; set; }
        public string busiMode { get; set; }
        public string whcode { get; set; }
        public string state { get; set; }
        public string wtdOrderId { get; set; }
        public decimal packageWt { get; set; }
        public WTDOrderQuery_OrderItemList orderItemList { get; set; }

    }

    public class WTDOrderQuery_OrderItemList
    {
        public List<WTDOrderQuery_OrderItem> orderItems;
    }

    public class WTDOrderQuery_OrderItem
    {
        public string productId { get; set; }
        public string productName { get; set; }
        public int qty { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
    }

    #endregion

    #region 发货查询

    public class WTDOrderExpressQueryResponseResult
    {
        public WTDOrderExpressQueryResponseResult()
        {
            orderList = new WTDOrderExpressQueryList();
        }
        public string code { get; set; }
        public string desc { get; set; }
        public int count { get; set; }
        public WTDOrderExpressQueryList orderList { get; set; }

    }

    public class WTDOrderExpressQueryList
    {
        public WTDOrderExpressQueryList()
        {
            orders = new List<WTDOrderExpressQuery_Order>();
        }

        public List<WTDOrderExpressQuery_Order> orders;
    }

    public class WTDOrderExpressQueryResponse
    {
        public WTDOrderExpressQueryResponseResult response { get; set; }
    }

    public class WTDOrderExpressQuery_Order
    {
        public string orderId { get; set; }
        public string expreeNo { get; set; }

    }

    public class WTDOrderExpressRouteQueryResult
    {
        public WTDOrderExpressRouteQueryResult()
        {
            orderList = new WTDOrderExpressRouteQueryList();
        }
        public string code { get; set; }
        public string desc { get; set; }
        public int count { get; set; }
        public WTDOrderExpressRouteQueryList orderList { get; set; }

    }

    public class WTDOrderExpressRouteQueryList
    {
        public WTDOrderExpressRouteQueryList()
        {
            orders = new List<WTDOrderExpressRouteQuery_ExpressRoute>();
        }

        public List<WTDOrderExpressRouteQuery_ExpressRoute> orders;
    }

    public class WTDOrderExpressRouteQueryResponse
    {
        public WTDOrderExpressRouteQueryResult response { get; set; }
    }

    public class WTDOrderExpressRouteQuery_ExpressRoute
    {
        public string expressName { get; set; }
        public string expreeNo { get; set; }
        public WTDOrderExpressRouteQuery_ExpressRouteItemList expressRouteItemList { get; set; }

    }

    public class WTDOrderExpressRouteQuery_ExpressRouteItemList
    {
        public List<WTDOrderExpressRouteQuery_ExpressRouteItem> expressRouteItems;
    }

    public class WTDOrderExpressRouteQuery_ExpressRouteItem
    {
        public string state { get; set; }
        public string notes { get; set; }
        public string optime { get; set; }
        public string opercode { get; set; }
    }

    #endregion
}
