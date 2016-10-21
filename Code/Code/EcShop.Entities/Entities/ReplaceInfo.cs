using System;
using System.Collections.Generic;
namespace EcShop.Entities
{
    public class ReplaceInfo
	{
        public int HandleStatus
        {
            get;
            set;
        }
		public int ReplaceId
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
        public DateTime ApplyForTime
        {
            get;
            set;
        }
        public string Comments
        {
            get;
            set;
        }
		public string Operator
		{
			get;
			set;
		}

        public System.DateTime ReceiveTime
        {
            get;
            set;
        }
        public List<OrderAppFormItems> ReplaceLineItem
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

        /// <summary>
        /// ������˾
        /// </summary>
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string LogisticsId { get; set; }
	}
}
