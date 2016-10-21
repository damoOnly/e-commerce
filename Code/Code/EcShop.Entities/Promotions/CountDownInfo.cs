using System;
namespace EcShop.Entities.Promotions
{
	public class CountDownInfo
	{
		public int CountDownId
		{
			get;
			set;
		}
		public int ProductId
		{
			get;
			set;
		}
		public decimal CountDownPrice
		{
			get;
			set;
		}
		public System.DateTime StartDate
		{
			get;
			set;
		}
		public System.DateTime EndDate
		{
			get;
			set;
		}
		public int MaxCount
		{
			get;
			set;
		}
		public string Content
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}

        public int? SupplierId
        {
            get;
            set;
        }
        public int ActiveTime
        {
            get;
            set;
        }
        public int PlanCount { get; set; }
        //public int SaleCount { get; set; }
	}
}
