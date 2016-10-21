using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.VShop
{
	public class LotteryActivityQuery : Pagination
	{
		public LotteryActivityType ActivityType
		{
			get;
			set;
		}
	}
}
