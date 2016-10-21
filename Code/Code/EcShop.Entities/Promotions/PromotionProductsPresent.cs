using System;
using System.Collections.Generic;
namespace EcShop.Entities.Promotions
{
	[System.Serializable]
    public class PromotionProductsPresent
	{
        /// <summary>
        /// 活动ID
        /// </summary>
		public int ActivityId
		{
			get;
			set;
		}
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId
        {
            get;
            set;
        }
	}
}
