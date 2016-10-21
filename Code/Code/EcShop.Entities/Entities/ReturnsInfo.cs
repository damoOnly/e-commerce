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
        public string Username  //����������ȡ������
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
        public string CellPhone//�ֻ�����
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
        /// ������˾
        /// </summary>
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string LogisticsId { get; set; }
        /// <summary>
        /// ���ù���
        /// </summary>
        public string FeeAffiliation { get; set; }
        /// <summary>
        /// ��ط���
        /// </summary>
        public decimal CustomsClearanceFee { get; set; }
        /// <summary>
        /// ��ݷ���
        /// </summary>
        public decimal ExpressFee { get; set; }

    }
}
