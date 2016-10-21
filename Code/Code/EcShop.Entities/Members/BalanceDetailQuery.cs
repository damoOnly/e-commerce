using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Members
{
	public class BalanceDetailQuery : Pagination
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
		public int? UserId
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
	}
}
