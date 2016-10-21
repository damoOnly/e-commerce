using System;
using System.Collections.Generic;
namespace EcShop.Entities.Promotions
{
	[System.Serializable]
    public class OrderPresents
	{
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
        public decimal ItemCostPrice
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
        public string ThumbnailsUrl
        {
            get;
            set;
        }

        public string SKUContent
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
        public int storeId
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



        public decimal GetSubTotal()
        {
            return this.ItemAdjustedPrice * this.ShipmentQuantity;
        }



        private int singeQty = 1;

        public int SingeQty
        {
            get { return singeQty; }
            set { singeQty = value; }
        }


        public string OrderId
        {
            get;
            set;
        }


        public OrderPresents CopySingleItem()
        {
            OrderPresents newItemInfo = new OrderPresents();
            newItemInfo.ItemAdjustedPrice = this.ItemAdjustedPrice;
            newItemInfo.ItemCostPrice = this.ItemCostPrice;
            newItemInfo.ItemDescription = this.ItemDescription;
            newItemInfo.ItemListPrice = this.ItemListPrice;
            newItemInfo.ProductId = this.ProductId;
            newItemInfo.PromotionId = this.PromotionId;
            newItemInfo.PromotionName = this.PromotionName;
            newItemInfo.ShipmentQuantity = 1;
            newItemInfo.SKU = this.SKU;
            newItemInfo.SKUContent = this.SKUContent;
            newItemInfo.SkuId = this.SkuId;
            newItemInfo.ThumbnailsUrl = this.ThumbnailsUrl;
            newItemInfo.SupplierId = this.SupplierId;
            newItemInfo.SupplierName = this.SupplierName;
            return newItemInfo;
        }
	}
}
