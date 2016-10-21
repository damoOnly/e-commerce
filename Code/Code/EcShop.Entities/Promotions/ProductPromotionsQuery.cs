using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Promotions
{
    public class ProductPromotionsQuery
    {
        public Pagination Page
        {
            get;
            set;
        }
        public ProductPromotionsQuery()
		{
			this.Page = new Pagination();
		}
        public string BeginTime
        {
            get;
            set;
        }
        public string EndTime
        {
            get;
            set;
        }
        public Boolean IsPromotion
        {
            get;
            set;
        }
        public Boolean IsWholesale
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
