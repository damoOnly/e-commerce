using System;
using System.Collections.Generic;
using EcShop.Core.Entities;

namespace EcShop.Entities.Orders
{
	public class ServiceOrderQuery : Pagination
	{
		public List<OrderStatus> Status
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
		public int? PaymentType
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

	}
}
