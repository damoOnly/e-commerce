using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Commodities
{
	public class ProductTypeQuery : Pagination
	{
		public string TypeName
		{
			get;
			set;
		}
	}
}
