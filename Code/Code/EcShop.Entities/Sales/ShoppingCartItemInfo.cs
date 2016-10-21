using EcShop.Entities.Promotions;
using System;
using System.Collections.Generic;
namespace EcShop.Entities.Sales
{
	public class ShoppingCartItemInfo
	{
		public int UserId
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
		public string SKU
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public decimal MemberPrice
		{
			get;
			set;
		}

        public decimal CountDownPrice
		{
			get;
			set;
		}

		public string ThumbnailUrl40
		{
			get;
			set;
		}
		public string ThumbnailUrl60
		{
			get;
			set;
		}
		public string ThumbnailUrl100
		{
			get;
			set;
		}

        public string ThumbnailUrl160
        {
            get;
            set;
        }

        public string ThumbnailUrl180
        {
            get;
            set;
        }

        public string ThumbnailUrl220
        {
            get;
            set;
        }

        /// <summary>
        /// ����
        /// </summary>
        public decimal Weight
        {
            get;
            set;
        }

        //ë��
        public decimal GrossWeight
        {
            get;
            set;
        }

		public string SkuContent
		{
			get;
			set;
		}
		public int Quantity
		{
			get;
			set;
		}
		public int PromotionId
		{
			get;
			set;
		}
		public PromoteType PromoteType
		{
			get;
			set;
		}
		public string PromotionName
		{
			get;
			set;
		}
		public decimal AdjustedPrice
		{
			get;
			set;
		}

        //�������
        public decimal PromotionPrice
        {
            set;
            get;
        }


		public int ShippQuantity
		{
			get;
			set;
		}
		public bool IsSendGift
		{
			get;
			set;
		}
		public decimal SubTotal
		{
			get
			{
				return this.AdjustedPrice * this.Quantity;
			}
		}


        private int singeQty = 1;

        public int SingeQty
        {
            get { return singeQty; }
            set { singeQty = value; }
        }

        public decimal SubNewTotal
        {
            get
            {
                decimal total = 0;
                if (PromoteType == PromoteType.SecondReducePrice || PromoteType == PromoteType.ProductDiscount)
                {
                    if (this.Quantity > 1)
                    {
                        switch (PromoteType)
                        {
                            //�ڶ�������
                            case PromoteType.SecondReducePrice:
                                total += PromotionPrice * SingeQty;
                                total += AdjustedPrice * (Quantity - SingeQty);
                                break;
                            //�ڶ����ۿ�
                            case PromoteType.ProductDiscount:
                                total += PromotionPrice * SingeQty;
                                total += AdjustedPrice * (Quantity - SingeQty);
                                break;
                        }
                    }
                    else
                    {
                        total += AdjustedPrice * Quantity;
                    }
                }
                else if (PromoteType == PromoteType.ProductPromotion && PromotionPrice > 0 && this.Quantity * AdjustedPrice >= PromotionPrice)
                {
                    total += PromotionPrice * SingeQty;
                    total += AdjustedPrice * (Quantity - SingeQty);
                }
                else
                {

                    total += AdjustedPrice * Quantity;
                }

                return total;
            }
        }

		public bool IsfreeShipping
		{
			get;
			set;
		}
		public decimal GetSubWeight()
		{
			return this.Weight * this.Quantity;
		}
        public decimal TaxRate
        {
            get;
            set;
        }
        public decimal MinTaxRate
        {
            get;
            set;
        }
        public decimal MaxTaxRate
        {
            get;
            set;
        }

        public string ComTaxRate
        {
            get { return GetExtendTaxRate(); }
        }
        public int TaxRateId
        {
            get;
            set;
        }
        public int TemplateId
        {
            get;
            set;
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

        public string SupplierImageUrl
        { get; set; }

        public string Logo
        {
            get;
            set;
        }


        /// <summary>
        /// Ʒ��Id
        /// </summary>
        public int BrandId
        {
            get;
            set;
        }

        /// <summary>
        /// ����Id
        /// </summary>
        public int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// �۵�
        /// </summary>
        public decimal DeductFee
        {
            get;
            set;
        }

        public bool IsCustomsClearance { get; set; }

        public int Stock { get; set; }


        public int BuyCardinality { get; set; }

        private IList<ShoppingCartPresentInfo> linePresentPro;
        public IList<ShoppingCartPresentInfo> LinePresentPro
        {
            get
            {
                if (this.linePresentPro == null)
                {
                    this.linePresentPro = new List<ShoppingCartPresentInfo>();
                }
                return this.linePresentPro;
            }
        }

        /// <summary>
        /// �������� 1��ͨ��2���
        /// </summary>
        public int SaleType
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

        public decimal GetTax()
        {
            decimal tax = 0;
            if(this.SaleType!=2)
            {
                tax = this.AdjustedPrice * this.TaxRate;
            }
            else
            {
                foreach(ProductsCombination item  in CombinationItemInfos)
                {
                    tax += item.Price * item.TaxRate * item.Quantity;
                }
            }

            return tax;
        }


         /// <summary>
        /// ��ȡ˰�ѷ�Χ
        /// </summary>
        /// <returns></returns>
        public string GetExtendTaxRate()
        {
            string currTaxRate = (this.MinTaxRate * 100).ToString("0")+"%" + "-" + (this.MaxTaxRate * 100).ToString("0")+"%";
            string resultTaxRate;
            if (this.MinTaxRate > 0)
            {
                resultTaxRate = this.MinTaxRate == this.MaxTaxRate ? (this.MinTaxRate * 100).ToString("0")+"%" : currTaxRate;
            }
            else if (this.MinTaxRate == 0 && this.MaxTaxRate > 0)
            {
                resultTaxRate = currTaxRate;
            }
            else
            {
                resultTaxRate = (this.TaxRate * 100).ToString("0")+"%";
            }

            return resultTaxRate;
        }
	}

    public class ShopCartInfo
    {
        public ShoppingCartItemInfo[] shoppingCartItemInfo
        {
            get;
            set;
        }
    }
}
