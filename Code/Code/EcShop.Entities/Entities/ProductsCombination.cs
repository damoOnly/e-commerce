using System;
using System.Collections.Generic;
namespace EcShop.Entities
{
    public class ProductsCombination
	{
        public int ProductId
        {
            get;
            set;
        }
        public string SkuId
		{
			get;
			set;
		}
        public string CombinationSkuId
		{
			get;
			set;
		}
        public string SKU
        {
            get;
            set;
        }
        public int Quantity
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }
        public string ProductName
        {
            get;
            set;
        }
        public string ThumbnailsUrl
        {
            get;
            set;
        }
        public decimal Weight
        {
            get;
            set;
        }
        public string SKUContent 
        {
            get;
            set;
        }

        public DateTime? AddTime
        {
            get;
            set;
        }

        public decimal TaxRate
        {
            get;
            set;
        }
	}
}
