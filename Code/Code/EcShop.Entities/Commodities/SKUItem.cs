using System;
using System.Collections.Generic;
namespace EcShop.Entities.Commodities
{
	public class SKUItem : System.IComparable
	{
		private Dictionary<int, int> skuItems;
		private Dictionary<int, decimal> memberPrices;
		public Dictionary<int, int> SkuItems
		{
			get
			{
				Dictionary<int, int> skuItems;
				if ((skuItems = this.skuItems) == null)
				{
					skuItems = (this.skuItems = new Dictionary<int, int>());
				}
				return skuItems;
			}
		}
		public Dictionary<int, decimal> MemberPrices
		{
			get
			{
				Dictionary<int, decimal> memberPrices;
				if ((memberPrices = this.memberPrices) == null)
				{
					memberPrices = (this.memberPrices = new Dictionary<int, decimal>());
				}
				return memberPrices;
			}
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
        
        /// <summary>
        /// 净重
        /// </summary>
		public decimal Weight
		{
			get;
			set;
		}

        /// <summary>
        /// 毛重：如毛重为0,则取净重
        /// </summary>
        public decimal GrossWeight
        {
            get;
            set;
        }

        //销售库存
		public int Stock
		{
			get;
			set;
		}

        //商品库存
        public int FactStock
        {
            get;
            set;
        }

        public int WMSStock
        {
            get;
            set;
        }

		public decimal CostPrice
		{
			get;
			set;
		}
		public decimal SalePrice
		{
			get;
			set;
		}

        public decimal DeductFee
        {
            get;
            set;
        }

        /// <summary>
        /// 备案编号
        /// </summary>
        public string ProductRegistrationNumber
        {
            get;
            set;
        }


        /// <summary>
        /// 料件号
        /// </summary>
        public string LJNo
        {
            get;
            set;
        }

        public string ValueStr
        {
            get;
            set;
        }
        

		public int CompareTo(object obj)
		{
			SKUItem sKUItem = obj as SKUItem;
			int result;
			if (sKUItem == null)
			{
				result = -1;
			}
			else
			{
				if (sKUItem.SkuItems.Count != this.SkuItems.Count)
				{
					result = -1;
				}
				else
				{
					foreach (int current in sKUItem.SkuItems.Keys)
					{
						if (sKUItem.SkuItems[current] != this.SkuItems[current])
						{
							result = -1;
							return result;
						}
					}
					result = 0;
				}
			}
			return result;
		}
	}
}
