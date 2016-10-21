using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Members
{
    public class VendorSalesReportQuery : Pagination
	{
		public System.DateTime? StartDate
		{
			get;
			set;
		}
		public System.DateTime? EndDate
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

        public string SupplierName
		{
			get;
			set;
		}

	}
}
