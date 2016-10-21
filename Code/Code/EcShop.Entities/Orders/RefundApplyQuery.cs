using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Orders
{
	public class RefundApplyQuery : Pagination
	{
		public string OrderId
		{
			get;
			set;
		}
		public int? HandleStatus
		{
			get;
			set;
		}
		public int? UserId
		{
			get;
			set;
		}
        public string StratTime
        {
            get;
            set;
        }

        public string EndTime
        {
            get;
            set;
        }

        public string Operator
        {
            get;
            set;
        }

        public int? SupplierId
        {
            get; 
            set;
        }
	}
}
