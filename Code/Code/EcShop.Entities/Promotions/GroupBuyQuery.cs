using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Promotions
{
	public class GroupBuyQuery : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}
		public int State
		{
			get;
			set;
		}
        public int? SupplierId
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string OrderId
        {
            get;
            set;
        }
        public string starttime
        {
            get;
            set;
        }
        public string endtime
        {
            get;
            set;
        }
        public string keyword
        {
            get;
            set;
        }
	}
}
