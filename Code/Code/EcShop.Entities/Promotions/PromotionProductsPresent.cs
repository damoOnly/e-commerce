using System;
using System.Collections.Generic;
namespace EcShop.Entities.Promotions
{
	[System.Serializable]
    public class PromotionProductsPresent
	{
        /// <summary>
        /// �ID
        /// </summary>
		public int ActivityId
		{
			get;
			set;
		}
        /// <summary>
        /// ��ƷID
        /// </summary>
        public int ProductId
        {
            get;
            set;
        }
	}
}
