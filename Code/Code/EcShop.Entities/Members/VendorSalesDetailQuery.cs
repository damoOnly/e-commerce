using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Members
{
    public class VendorSalesDetailQuery : Pagination
	{
		public System.DateTime? FromDate
		{
			get;
			set;
		}
		public System.DateTime? ToDate
		{
			get;
			set;
		}
		public TradeTypes TradeType
		{
			get;
			set;
		}
		public SplittingTypes SplittingTypes
		{
			get;
			set;
		}

        public string OrderId
		{
			get;
			set;
		}
        public string ProductName
		{
			get;
			set;
		}
        public string ProductCode
		{
			get;
			set;
		}
        public string SupplierId
        {
            get;
            set;
        }

	}
}
