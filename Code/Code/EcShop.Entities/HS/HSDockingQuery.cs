using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.HS
{
    public class HSDockingQuery : Pagination
    {
        /// <summary>
        /// 三单对接状态ID
        /// </summary>
        public int HS_Docking_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 订单报文ID
        /// </summary>
        public string OrderPacketsID
        {
            get;
            set;
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string OrderRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 运单号
        /// </summary>
        public string LogisticsNo
        {
            get;
            set;
        }
        /// <summary>
        /// 运单报文ID
        /// </summary>
        public string LogisticsPacketsID
        {
            get;
            set;
        }
        /// <summary>
        /// 运单状态
        /// </summary>
        public string LogisticsStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 运单备注
        /// </summary>
        public string LogisticsRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 支付状态
        /// </summary>
        public string PaymentStatus
        {
            get;
            set;

        }
        /// <summary>
        /// 支付备注
        /// </summary>
        public string PaymentRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证信息
        /// </summary>
        public string payerId
        {
            get;
            set;
        }
        /// <summary>
        /// 支付人姓名
        /// </summary>
        public string payerName
        {
            get;
            set;
        }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal paymentBillAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 支付信息编号
        /// </summary>
        public string tradeNo
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证验证状态
        /// </summary>
        public string payerIdStatus
        {
            get;
            set;
        }
    }
}
