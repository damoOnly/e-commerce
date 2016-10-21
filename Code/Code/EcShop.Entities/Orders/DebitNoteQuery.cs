using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Orders
{
	public class DebitNoteQuery : Pagination
	{
		public string OrderId
		{
			get;
			set;
		}
	}
}
