using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.HS;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Entities.Members;
using EcShop.SqlDal.HS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using EcShop.Core.ErrorLog;

namespace EcShop.ControlPanel.Commodities
{
    public sealed class HSCodeHelper
    {
        private HSCodeHelper()
        { }

        public static DbQueryResult GetHSCode(HSCodeQuery query)
        {
            return new HSCodeDao().GetHSCode(query);
        }

        public static DbQueryResult GetHSDocking(HSDockingQuery query)
        {
            return new HSCodeDao().GetHSDocking(query);
        }

        public static DbQueryResult GetHSDeclare(HSDeclareQuery query)
        {
            return new HSCodeDao().GetHSDeclare(query);
        }

        /// <summary>
        /// 查询订单认领状态
        /// </summary>
        /// <param name="orderid">单号</param>
        /// <returns>true:已认领</returns>
        public static bool GetOrderClaimStatus(string orderid)
        {
           return new HSCodeDao().GetOrderClaimStatus(orderid);
        }
        
        /// <summary>
        /// 查询订单认领人
        /// </summary>
        /// <param name="orderid">单号</param>
        /// <returns></returns>
        public static object GetOrderClaimUser(string orderid)
        {
            return new HSCodeDao().GetOrderClaimUser(orderid);
        }


        /// <summary>
        /// 重发四单
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="UserId">操作人</param>
        public static void ResendHSDocking(string orderid, int UserId)
        {
            new HSCodeDao().ResendHSDocking(orderid, UserId);
        }

        /// <summary>
        /// 作废某个订单明细
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="SkuId">SkuId</param>
        /// <param name="UserId">操作人</param>
        public static void ValidHSDocking(string orderid,string SkuId, int UserId)
        {
            new HSCodeDao().ValidHSDocking(orderid, SkuId, UserId);
        }

        

        public static void SetOrderStatus(string orderid, string status, string Remark, int UserId, string Username)
        {
            new HSCodeDao().SetOrderStatus(orderid, status, Remark, UserId, Username);
        }
        public static IList<HSCodeInfo> GetHSCode(string HS_CODE)
        {
            return new HSCodeDao().GetHSCode(HS_CODE);
        }
        public static IList<HSCodeInfo> GetHSCode()
        {
            return new HSCodeDao().GetHSCode();
        }

        public static bool Delete(int HS_CODE_ID)
        {
            //    ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            //IUser user = Users.GetUser(userId);
            bool flag = new HSCodeDao().DeleteHSCode(HS_CODE_ID);
            if (flag)
            {
                //Users.ClearUserCache(HS_CODE_ID);
                EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的海关编码", new object[]
                {
                    HS_CODE_ID
                }));
            }
            return flag;
        }

        public static IList<ElmentsInfo> GetElmentsInfo()
        {
            return new HSCodeDao().GetElments();
        }
        public static IList<ElmentsInfo> GetElmentsInfo(string HSElmentsName)
        {
            return new HSCodeDao().GetElments(HSElmentsName);
        }

        public static int AddHSCodeInfo(HSCodeInfo hscodeinfo)
        {
            return new HSCodeDao().AddHSCodeInfo(hscodeinfo);
        }

        public static DataSet GetHSCodeInfo(int HS_CODE_ID)
        {
            return new HSCodeDao().GetHSCodeInfo(HS_CODE_ID);
        }

        public static bool UpdateHSCodeInfo(HSCodeInfo hscodeinfo)
        {
            return new HSCodeDao().UpdateHSCodeInfo(hscodeinfo);
        }

        /// <summary>
        /// 根据海关编码获取数据 
        /// </summary>
        /// <param name="Code">海关编码</param>
        /// <returns></returns>
        public static DataTable GetHSCodeBuyCode(string Code)
        {
            return new HSCodeDao().GetHSCodeBuyCode(Code);
        }

        /// <summary>
        /// 获取海关编码对应申报要素数据 
        /// </summary>
        /// <param name="CodeId">海关编码Id</param>
        /// <returns></returns>
        public static DataTable GetHSCODEELMENTS(string CodeId, string ProductId)
        {
            return new HSCodeDao().GetHSCODEELMENTS(CodeId, ProductId);
        }

        /// <summary>
        /// 获取订单监控数据
        /// </summary>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <param name="Hour">结转时间</param>
        /// <returns></returns>
        public static DataSet GetOrderMonitoring(string StartDate, string EndDate, string Hour)
        {
            return new HSCodeDao().GetOrderMonitoring(StartDate, EndDate, Hour);
        }
        

        /// <summary>
        /// 订单保存数据库
        /// </summary>
        /// <returns></returns>
        public static void SaveHSDocking(int sType, string OrderId, string LogisticsNo, string OPacketsID, int Status, string Remark, string Data, string PayerID, string PayerName, string paymentBillAmount, string tradeNo)
        {
            new HSCodeDao().SaveHSDocking(sType, OrderId, LogisticsNo, OPacketsID, Status, Remark, Data, PayerID, PayerName, paymentBillAmount, tradeNo);
        }

        /// <summary>
        /// 获取需要三单对碰的订单及其信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetOrders()
        {
            return new HSCodeDao().GetOrders();
        }

        /// <summary>
        /// 获取需要三单对碰的运单及其信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLogs()
        {
            return new HSCodeDao().GetLogs();
        }

        public static DataTable GetOrderItems(string orderid)
        {
            return new HSCodeDao().GetOrderItems(orderid);
        }
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="ShipToName">收货人姓名</param>
        /// <param name="ShipToID">收货人身份证</param>
        /// <param name="ShipToaddress">收货人姓名地址</param>
        /// <param name="ShipToPhone">收货人电话</param>
        /// <param name="str">调用身份证验证接口返回的数据</param>
        /// <param name="runType">在没有强制要求四单对碰前设置0，强制要求身份证验证设置1</param>
        public static void SetPayerIDStatus(string orderid, string ShipToName, string ShipToID, string ShipToaddress, string ShipToPhone, string str, string runType)
        {
            HSCodeDao HSCodeDao = new HSCodeDao();
            //if (!str.Contains("404"))
            try
            {
                if (orderid.Contains("_"))
                {
                    orderid=orderid.Substring(0, orderid.IndexOf('_'));
                }
                HSCodeDao.SetPayerIDStatus("", orderid, "1", "已发送", "", "", "", runType);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(str);

                XmlNode node = xmlDocument.SelectSingleNode("result/code");

                if (node.InnerText == "200")
                {
                    XmlNode nodeName = xmlDocument.SelectSingleNode("result/retvalue/emResult/name");
                    XmlNode nodeOrderId = xmlDocument.SelectSingleNode("result/retvalue/emResult/orderId");
                    XmlNode nodeStatus = xmlDocument.SelectSingleNode("result/retvalue/emResult/status");
                    XmlNode nodeErrorcode = xmlDocument.SelectSingleNode("result/retvalue/emResult/errorcode");
                    XmlNode nodeErrormessage = xmlDocument.SelectSingleNode("result/retvalue/emResult/errormessage");
                    XmlNode nodeIDCardNum = xmlDocument.SelectSingleNode("result/retvalue/emResult/idCardNum");

                    if (nodeOrderId != null)
                    {
                        if (nodeOrderId.InnerText.Contains("_"))
                        {
                            nodeOrderId.InnerText = nodeOrderId.InnerText.Substring(0, nodeOrderId.InnerText.IndexOf('_'));
                        }
                    }

                    if (nodeStatus != null)
                    {
                        if (nodeStatus.InnerText.ToUpper() == "PASS")
                        {
                            HSCodeDao.SetPayerIDStatus(nodeName.InnerText, nodeOrderId.InnerText, "2", "已通过身份证验证!", nodeIDCardNum.InnerText, ShipToaddress, ShipToPhone, runType);
                        }
                        if (nodeStatus.InnerText.ToUpper() == "NOPASS")
                        {
                            HSCodeDao.SetPayerIDStatus(nodeName.InnerText, nodeOrderId.InnerText, "3", "未通过身份证验证!", nodeIDCardNum.InnerText, ShipToaddress, ShipToPhone, runType);
                        }
                        if (nodeStatus.InnerText.ToUpper() == "NOAUTH")
                        {
                            HSCodeDao.SetPayerIDStatus(nodeName.InnerText, nodeOrderId.InnerText, "3", "未通过身份证验证!", nodeIDCardNum.InnerText, ShipToaddress, ShipToPhone, runType);
                        }
                        
                    }
                    else
                    {
                        string error = "idCardNum is invalid.";
                        if (nodeName != null)
                        {
                            ShipToName = nodeName.InnerText;
                        }
                        if (nodeOrderId != null)
                        {
                            orderid = nodeOrderId.InnerText;
                        }
                        if (nodeIDCardNum != null)
                        {
                            ShipToID = nodeIDCardNum.InnerText;
                        }
                        if (nodeErrormessage != null)
                        { 
                            error=nodeErrormessage.InnerText;
                        }
                        HSCodeDao.SetPayerIDStatus(ShipToName, orderid, "3", error, ShipToID, ShipToaddress, ShipToPhone,runType);
                    }
                }
                else
                {
                    ErrorLog.Write("efindUrl interface fail");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write("身份证验证接口错误：" + str);
            }
        }

        public static DataSet GetOrderMonitoringItem(string StartDate, string EndDate, string Hour, string type, string status)
        {
            return new HSCodeDao().GetOrderMonitoringItem(StartDate, EndDate, Hour, type, status);
        }
    }
}
