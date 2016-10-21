using EcShop.Entities.Promotions;
using System;
using System.Collections.Generic;
namespace EcShop.Entities.Orders
{
	public class LineItemInfo
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
		public int Quantity
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
		public decimal ItemWeight
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
        public decimal TaxRate
        {
            get;
            set;
        }
		public PromoteType PromoteType
		{
			get;
			set;
		}
        public int TemplateId
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

        /// <summary>
        /// 扣点
        /// </summary>
        public decimal DeductFee
        {
            get;
            set;
        }


		public decimal GetSubTotal()
		{
			return this.ItemAdjustedPrice * this.Quantity;
		}



        private int singeQty = 1;

        public int SingeQty
        {
            get { return singeQty; }
            set { singeQty = value; }
        }

        public decimal GetNewSubTotal()
        {
            decimal total = 0;
            if (this.PromoteType == PromoteType.ProductPromotion || PromoteType == PromoteType.SecondReducePrice || PromoteType == PromoteType.ProductDiscount)
            {
                switch (PromoteType)
                {
                    case PromoteType.ProductPromotion:
                        total += this.PromotionPrice * SingeQty;
                        total += this.ItemAdjustedPrice * (this.Quantity - SingeQty);
                        break;
                    //第二件减价
                    case PromoteType.SecondReducePrice:
                        total += this.PromotionPrice * SingeQty;
                        total += this.ItemAdjustedPrice * (this.Quantity - SingeQty);
                        break;
                    //第二件折扣
                    case PromoteType.ProductDiscount:
                        total += this.PromotionPrice * SingeQty;
                        total += this.ItemAdjustedPrice * (this.Quantity - SingeQty);
                        break;
                }
            }
            else
            {
                total += this.ItemAdjustedPrice * this.Quantity;
            }

            return total;
        }


        public string OrderId
        {
            get;
            set;
        }

        public OrderStatus OrderStatus
        {
            get;
            set;
        }

        public decimal PromotionPrice
        {
            get;
            set;
        }

        private List<ProductsCombination> combinationItemInfos;
        public List<ProductsCombination> CombinationItemInfos
        {
            get
            {
                if (this.combinationItemInfos == null)
                {
                    this.combinationItemInfos = new List<ProductsCombination>();
                }
                return this.combinationItemInfos;
            }
            set
            {
                this.combinationItemInfos = value;
            }
        }

        public LineItemInfo CopySingleItem()
        {
            LineItemInfo newItemInfo = new LineItemInfo();
            newItemInfo.ItemAdjustedPrice = this.ItemAdjustedPrice;
            newItemInfo.ItemCostPrice = this.ItemCostPrice;
            newItemInfo.ItemDescription = this.ItemDescription;
            newItemInfo.ItemListPrice = this.ItemListPrice;
            newItemInfo.ItemWeight = this.ItemWeight;
            newItemInfo.ProductId = this.ProductId;
            newItemInfo.PromoteType = this.PromoteType;
            newItemInfo.PromotionId = this.PromotionId;
            newItemInfo.PromotionName = this.PromotionName;
            newItemInfo.Quantity = 1;
            newItemInfo.ShipmentQuantity = 1;
            newItemInfo.SKU = this.SKU;
            newItemInfo.SKUContent = this.SKUContent;
            newItemInfo.SkuId = this.SkuId;
            newItemInfo.TaxRate = this.TaxRate;
            newItemInfo.TemplateId = this.TemplateId;
            newItemInfo.ThumbnailsUrl = this.ThumbnailsUrl;
            newItemInfo.SupplierId = this.SupplierId;
            newItemInfo.SupplierName = this.SupplierName;
            newItemInfo.PromotionPrice = this.PromotionPrice;
            newItemInfo.CombinationItemInfos = this.CombinationItemInfos;
            newItemInfo.Tax = this.Tax;
            return newItemInfo;
        }

        public decimal Tax
        {
            get;

            set;
        }


        /// <summary>
        /// 计算税费
        /// </summary>
        /// <returns></returns>
        public decimal CalculateTax()
        {
            decimal tax=0M;
            if(this.combinationItemInfos.Count>0)
            {
                foreach(ProductsCombination item in combinationItemInfos)
                {
                    tax += item.Quantity * item.TaxRate * item.Price;
                }
            }

            else
            {
                tax=this.TaxRate * this.ItemAdjustedPrice;
            }

            return tax;
        }
	}
}
