using EcShop.Entities.Promotions;
using System;
namespace EcShop.Entities.Sales
{
    public class SkuInfo
	{
        /// <summary>
        /// 商品规格ID
        /// </summary>
        public string SkuId
		{
			get;
			set;
		}

        /// <summary>
        /// 购买数量
        /// </summary>
        public int BuyQty
        {
            get;
            set;
        }
	}


    public class SkuItemInfo
    {
        public SkuInfo[] skuInfo
        {
            get;
            set;
        }
    }
}
