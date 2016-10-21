using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Promotions
{
	public class BundlingInfoQuery : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}
        public int? SupplierId
        {
            get;
            set;
        }
	}
}
