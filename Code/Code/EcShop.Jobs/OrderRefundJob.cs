using Alipay;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Domain;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using WxPayAPI;

namespace EcShop.Jobs
{
    public class OrderRefundJob : IJob
    {
        public void Execute(XmlNode node)
        {
            DataTable dt=TradeHelper.GetRefundOrderList();
            if (dt!=null)
            {
                foreach(DataRow rows in dt.Rows)
                {
                    string rest = OrderRefund(rows);
                    ErrorLog.Write("退款执行结果："+rest);
                    Thread.Sleep(1000*60);
                }
            }
        }

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected string OrderRefund(DataRow order)
        {
            try
            {
                ErrorLog.Write("进入退款中：" + order["PaymentType"].ToString());
                #region  微信退款
                if (order["PaymentType"].ToString().IndexOf("微信") >= 0)//微信退款
                {
                    ErrorLog.Write("【微信退款】开始：" + order["OrderId"].ToString());
                    PackageInfo packageInfo = new PackageInfo();
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
                    packageInfo.OutTradeNo = order["OrderId"].ToString();
                    packageInfo.OutRefundNo = order["OrderId"].ToString();//退款ID默认为订单号
                    decimal totle = 0;
                    decimal.TryParse(order["OrderTotal"].ToString(), out totle);
                    packageInfo.RefundFee = (decimal)Math.Round(totle, 2) * 100;
                    packageInfo.TotalFee = (decimal)Math.Round(totle, 2) * 100;
                    ErrorLog.Write("【微信退款】退款参数：" + Newtonsoft.Json.JsonConvert.SerializeObject(packageInfo));
                    string ret = string.Empty;
                    int type = 1;
                    if (order["SourceOrder"].ToString() == "11" || order["SourceOrder"].ToString()=="12")
                    {
                        type = 2;
                    }

                    ret = Refund.Run(order["GatewayOrderId"].ToString(), order["OrderId"].ToString(), packageInfo.TotalFee.ToString(), packageInfo.RefundFee.ToString(), type);
                    //ret = payClient.RequestRefund(packageInfo);
                    ErrorLog.Write("【微信退款】" + order["OrderId"].ToString() + "退款结果：" + ret);
                    if (string.IsNullOrEmpty(ret) || ret.ToUpper().IndexOf("SUCCESS") >= 0) //退款成功
                    {
                        Action ac = new Action(() =>
                        {
                            TradeHelper.RefundOrder(order["OrderId"].ToString());//恢复库存及现金卷
                            TradeHelper.RefundOrder_Split(order["OrderId"].ToString(), order["SourceOrderId"].ToString());
                            TradeHelper.RefundSuccess(order["OrderId"].ToString(), 1);
                        });
                        ac.BeginInvoke(null, ac);
                        return "退款成功！";
                    }
                    return "微信退款失败，请重试或联系客服退款！";
                }
                #endregion

                #region 支付宝退款
                if (order["PaymentType"].ToString().IndexOf("支付宝") >= 0)//支付宝退款
                {
                    //退款批次号
                    string batch_no = order["OrderId"].ToString();
                    //必填，每进行一次即时到账批量退款，都需要提供一个批次号，必须保证唯一性
                    //退款请求时间
                    string refund_date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //必填，格式为：yyyy-MM-dd hh:mm:ss
                    //退款总笔数
                    string batch_num = "1";
                    //必填，即参数detail_data的值中，“#”字符出现的数量加1，最大支持1000笔（即“#”字符出现的最大数量999个）
                    //单笔数据集
                    decimal totle = 0;
                    decimal.TryParse(order["OrderTotal"].ToString(), out totle);
                    string detail_data = order["GatewayOrderId"] + "^" + totle + "^协商退款";
                    //必填，格式详见“4.3 单笔数据集参数说明”
                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    Config.Key = "vec1v8gshy4mo8lz735boyupyuhotjct";
                    //把请求参数打包成数组
                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("partner", "2088121134306407");
                    sParaTemp.Add("_input_charset", "utf-8");
                    sParaTemp.Add("service", "refund_fastpay_by_platform_nopwd");
                    sParaTemp.Add("batch_no", batch_no);
                    sParaTemp.Add("refund_date", refund_date);
                    sParaTemp.Add("batch_num", batch_num);
                    sParaTemp.Add("detail_data", detail_data);
                    ErrorLog.Write("【支付宝退款】退款参数：" + Newtonsoft.Json.JsonConvert.SerializeObject(sParaTemp));
                    //建立请求
                    string sHtmlText = Submit.BuildRequest(sParaTemp);
                    ErrorLog.Write("【支付宝退款】" + order["OrderId"].ToString() + ";海美生活订单退款：" + sHtmlText);

                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        xmlDoc.LoadXml(sHtmlText);
                        string strXmlResponse = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                        if (strXmlResponse.ToLower() == "t")//退款成功
                        {

                            Action ac = new Action(() =>
                            {
                                //if (string.IsNullOrEmpty(order.SourceOrderId.ToString()))//非拆单
                                //{
                                TradeHelper.RefundOrder(order["OrderId"].ToString().ToString());//恢复库存及现金卷
                                //}
                                //else
                                //{
                                TradeHelper.RefundOrder_Split(order["OrderId"].ToString(), order["SourceOrderId"].ToString());
                                //}
                                TradeHelper.RefundSuccess(order["OrderId"].ToString(), 1);
                            });
                            ac.BeginInvoke(null, ac);

                            //Action ac = new Action(() =>
                            //{
                            //    if (string.IsNullOrEmpty(order["SourceOrderId"].ToString()))//非拆单
                            //    {
                            //        TradeHelper.RefundOrder(order["OrderId"].ToString());//恢复库存及现金卷
                            //    }
                            //    else
                            //    {
                            //        TradeHelper.RefundOrder_Split(order["OrderId"].ToString(), order["SourceOrderId"].ToString());
                            //    }
                            //    TradeHelper.RefundSuccess(order["OrderId"].ToString(), 1);
                            //});
                            //ac.BeginInvoke(null, ac);
                            return "Ok";
                        }
                    }
                    catch (Exception exp)
                    {
                        ErrorLog.Write("【支付宝退款】" + exp.Message, exp);
                    }

                }
                #endregion
                ErrorLog.Write("【支付宝退款】" + order["OrderId"].ToString() + "退款异常，请重试或联系客服退款！");
                return "退款异常，请重试或联系客服退款！";
            }
            catch (Exception ee)
            {
                ErrorLog.Write("【退款异常】" + Newtonsoft.Json.JsonConvert.SerializeObject(order), ee);
                return "退款执行异常";
            }
        }
    }
}
