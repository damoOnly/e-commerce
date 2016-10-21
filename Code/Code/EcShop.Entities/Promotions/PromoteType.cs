using System;
namespace EcShop.Entities.Promotions
{
	public enum PromoteType
	{
		NotSet,
		Discount,
		Amount,
		Reduced,
		QuantityDiscount,
		SentGift,
		SentProduct,
        ProductPromotion,//单品满减
        SecondReducePrice,//第二件减价
        ProductDiscount = 9,//第二件打折
		FullAmountDiscount = 11,
		FullAmountReduced,
		FullQuantityDiscount,
		FullQuantityReduced,
		FullAmountSentGift,
		FullAmountSentTimesPoint,
		FullAmountSentFreight,  
        PresentProduct = 10
	}
}
