using System;
using System.Collections.Generic;
namespace EcShop.Entities
{
    public class ReturnsInfo
    {
        public int HandleStatus
        {
            get;
            set;
        }
        public int ReturnsId
        {
            get;
            set;
        }
        public string OrderId
        {
            get;
            set;
        }

        public string Operator
        {
            get;
            set;
        }
        public string Comments
        {
            get;
            set;
        }
        public DateTime ApplyForTime
        {
            get;
            set;
        }
        public int RefundType
        {
            get;
            set;
        }
        public decimal RefundMoney
        {
            get;
            set;
        }
        public System.DateTime ReceiveTime
        {
            get;
            set;
        }
        public List<OrderAppFormItems> ReturnsLineItem
        {
            get;
            set;
        }
        public string Username  //关联订单获取的属性
        {
            get;
            set;
        }
        public string EmailAddress
        {
            get;
            set;
        }
        public string RealName
        {
            get;
            set;
        }
        public string ZipCode
        {
            get;
            set;
        }
        public string CellPhone//手机号码
        {
            get;
            set;
        }
        public decimal GetAmount()
        {
            decimal amount = 0M;
            if (ReturnsLineItem != null)
            {
                foreach (OrderAppFormItems item in ReturnsLineItem)
                {
                    amount += item.ItemAdjustedPrice * item.Quantity;
                }
            }
            return amount;
        }
        /// <summary>
        /// 物流公司
        /// </summary>
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsId { get; set; }
        /// <summary>
        /// 费用归属
        /// </summary>
        public string FeeAffiliation { get; set; }
        /// <summary>
        /// 清关费用
        /// </summary>
        public decimal CustomsClearanceFee { get; set; }
        /// <summary>
        /// 快递费用
        /// </summary>
        public decimal ExpressFee { get; set; }

    }
}
