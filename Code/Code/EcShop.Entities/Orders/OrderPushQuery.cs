using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Orders
{
	public class OrderPushQuery : Pagination
	{
        public string PushPlatform { get; set; }

		public DateTime? StartDate
		{
			get;
			set;
		}
		public DateTime? EndDate
		{
			get;
			set;
		}
		public int? PushStatus
		{
			get;
			set;
		}
		public int? ExpressStatus
		{
			get;
			set;
		}

	}
}
