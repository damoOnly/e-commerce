using System;
namespace EcShop.Entities
{
	public class RefundInfo
	{
		public enum Handlestatus
		{
			Applied = 1,
			Refunded,
			Refused
		}
		public int RefundId
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public System.DateTime ApplyForTime
		{
			get;
			set;
		}
		public string RefundRemark
		{
			get;
			set;
		}
		public System.DateTime HandleTime
		{
			get;
			set;
		}
		public string AdminRemark
		{
			get;
			set;
		}
		public string Operator
		{
			get;
			set;
		}
		public RefundInfo.Handlestatus HandleStatus
		{
			get;
			set;
		}

        public int Status
        {
            get;
            set;
        }

        public System.DateTime ReceiveTime
        {
            get;
            set;
        }

        public int ProductReturnType
        {
            get;
            set;
        }

        public string ProductReturnRemark
        {
            get;
            set;
        }
	}
}
