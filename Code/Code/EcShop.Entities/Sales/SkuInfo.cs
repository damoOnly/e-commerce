using EcShop.Entities.Promotions;
using System;
namespace EcShop.Entities.Sales
{
    public class SkuInfo
	{
        /// <summary>
        /// ��Ʒ���ID
        /// </summary>
        public string SkuId
		{
			get;
			set;
		}

        /// <summary>
        /// ��������
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
