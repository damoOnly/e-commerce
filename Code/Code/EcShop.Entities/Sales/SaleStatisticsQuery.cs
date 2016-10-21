using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Sales
{
	[System.Serializable]
	public class SaleStatisticsQuery : Pagination
	{
		public string QueryKey
		{
			get;
			set;
		}
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
        public int? BrandId
        {
            get;
            set;
        }

        public int? ImportSourceId
        {
            get;
            set;
        }

        public int? SupplierId
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
	}
}
