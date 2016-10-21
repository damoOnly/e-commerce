using System;
namespace EcShop.Entities.Sales
{
	public class AdminStatisticsInfo : StatisticsInfo
	{
		public decimal MembersBalance
		{
			get;
			set;
		}
		public decimal BalanceTotal
		{
			get
			{
				return this.MembersBalance;
			}
		}
		public decimal ProfitTotal
		{
			get
			{
				return base.OrderProfitToday;
			}
		}
		public int MemberBlancedrawRequest
		{
			get;
			set;
		}
		public int TodayFinishOrder
		{
			get;
			set;
		}
		public int YesterdayFinishOrder
		{
			get;
			set;
		}
		public decimal OrderPriceYesterDay
		{
			get;
			set;
		}
		public int UserNewAddYesterToday
		{
			get;
			set;
		}
		public int TotalMembers
		{
			get;
			set;
		}
		public int TotalProducts
		{
			get;
			set;
		}
		public decimal OrderPriceMonth
		{
			get;
			set;
		}
	}
}
