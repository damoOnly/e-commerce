using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Orders
{
	public class OrderGiftQuery : Pagination
	{
		public string OrderId
		{
			get;
			set;
		}
	}
}
