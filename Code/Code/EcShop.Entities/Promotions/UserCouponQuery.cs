using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Promotions
{
	public class UserCouponQuery : Pagination
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
	}
}
