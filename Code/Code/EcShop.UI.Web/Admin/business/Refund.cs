using EcShop.Core.ErrorLog;
using EcShop.Membership.Context;
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace WxPayAPI
{
    public class Refund
    {
        /***
        * 申请退款完整业务流程逻辑
        * @param transaction_id 微信订单号（优先使用）
        * @param out_trade_no 商户订单号
        * @param total_fee 订单总金额
        * @param refund_fee 退款金额
        * @return 退款结果（xml格式）
        */
        public static string Run(string transaction_id, string out_trade_no, string total_fee, string refund_fee, int type)
        {
            try
            {
                Log.Info("Refund", "Refund is processing...");

                WxPayData data = new WxPayData();
                if (!string.IsNullOrEmpty(transaction_id))//微信订单号存在的条件下，则已微信订单号为准
                {
                    data.SetValue("transaction_id", transaction_id);
                }
                else//微信订单号不存在，才根据商户订单号去退款
                {
                    data.SetValue("out_trade_no", out_trade_no);
                }

                WxPayConfig wx = new WxPayConfig(type);
              
                data.SetValue("total_fee", int.Parse(total_fee));//订单总金额
                data.SetValue("refund_fee", int.Parse(refund_fee));//退款金额
                data.SetValue("out_refund_no", out_trade_no);//随机生成商户退款单号
                data.SetValue("op_user_id", WxPayConfig.MCHID);//操作员，默认为商户号
                WxPayData rst = WxPayApi.Refund(data);//提交退款申请给API，接收返回数据

                Log.Info("Refund", "Refund process complete, result : " + rst.ToXml());
                string text = rst.ToPrintStr();
                #region 屏蔽代码
                //XmlDocument xmlDocument = new XmlDocument();
                //try
                //{
                //    xmlDocument.LoadXml(text);
                //}
                //catch (Exception ex)
                //{
                //    text = string.Format("微信获取信息错误doc.load：{0}", ex.Message) + text;
                //    ErrorLog.Write(text);
                //}
                //string result = string.Empty;
                //try
                //{
                //    if (xmlDocument == null)
                //    {
                //        result = text;
                //        return result;
                //    }
                //    XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
                //    if (xmlNode == null)
                //    {
                //        result = text;
                //        return result;
                //    }
                //    if (!(xmlNode.InnerText == "SUCCESS"))
                //    {
                //        text = string.Format("微信获取信息失败：{0}", text);
                //        ErrorLog.Write(text);
                //        result = xmlNode.InnerText;
                //        return result;
                //    }
                //    XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/result_code");
                //    if (!(xmlNode2.InnerText == "SUCCESS"))
                //    {
                //        result = xmlNode2.InnerText;
                //        xmlNode2 = xmlDocument.SelectSingleNode("xml/err_code_des");
                //        return result + ":" + xmlNode2.InnerText;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    text = string.Format("微信获取信息错误node.load：{0}", ex.Message) + text;
                //    ErrorLog.Write(text);
                //}

                #endregion

                return text;
            }
            catch (Exception ee)
            {
                return ee.Message.ToString();
            }
        }
    }
}