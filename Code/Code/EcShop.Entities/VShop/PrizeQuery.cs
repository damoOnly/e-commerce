using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.VShop
{
	public class PrizeQuery : Pagination
	{
		public LotteryActivityType ActivityType
		{
			get;
			set;
		}
		public int ActivityId
		{
			get;
			set;
		}
		public bool IsPrize
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public string CellPhone
		{
			get;
			set;
		}
	}
}
