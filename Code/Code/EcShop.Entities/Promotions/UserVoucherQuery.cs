using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Promotions
{
    public class UserVoucherQuery : Pagination
	{
		public int? UserID
		{
			get;
			set;
		}
		public int? Status
		{
			get;
			set;
		}
		public string ClaimCode
		{
			get;
			set;
		}

        public DateTime ClosingTime
		{
			get;
			set;
		}
        public int VoucherId
		{
			get;
			set;
		}

        public string Name
		{
			get;
			set;
		}

        public decimal Amount
		{
			get;
			set;
		}

        public DateTime StartTime
        {
            get;
            set;
        }

        public decimal DiscountValue
        {
            get;
            set;
        }
        public DateTime UsedTime
        {
            get;
            set;
        }

        public int VoucherStatus
        {
            get;
            set;
        }

	}
}
