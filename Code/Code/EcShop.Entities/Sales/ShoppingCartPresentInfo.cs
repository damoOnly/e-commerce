using EcShop.Entities.Promotions;
using System;
namespace EcShop.Entities.Sales
{
    public class ShoppingCartPresentInfo
	{
		public string OrderId
		{
			get;
			set;
		}
		public string SkuId
		{
			get;
			set;
		}
		public int ProductId
		{
			get;
			set;
		}

        public string ProductName
        {
            get;
            set;
        }
		public string SKU
		{
			get;
			set;
		}

        public int ShipmentQuantity
        {
            get;
            set;
        }
        public decimal CostPrice
        {
            get;
            set;
        }

        public decimal ItemListPrice
        {
            get;
            set;
        }

        public decimal ItemAdjustedPrice
        {
            get;
            set;
        }

        public string ItemDescription
        {
            get;
            set;
        }
		public string ThumbnailUrl40
		{
			get;
			set;
		}
		
		public string SkuContent
		{
			get;
			set;
		}
		public int PromotionId
		{
			get;
			set;
		}
		public string PromotionName
		{
			get;
			set;
		}
		public decimal SubTotal
		{
			get
			{
				return this.ItemAdjustedPrice * this.ShipmentQuantity;
			}
		}

        public int StoreId
        {
            get;
            set;
        }
        public int SupplierId
        {
            get;
            set;
        }
        public string SupplierName
        {
            get;
            set;
        }

        public string ShopName
        {
            get;
            set;
        }
        public string ShopOwner
        {
            get;
            set;
        }

        public int FactStock { get; set; }

        /// <summary>
        /// 赠送数量
        /// </summary>
        public int DiscountValue
        {
            get;
            set;
        }

        public int IsAscend
        {
            get;
            set;
        }

        /// <summary>
        /// 活动商品ID
        /// </summary>
        public int PromotionProductId
        {
            get;
            set;
        }
	}
}
