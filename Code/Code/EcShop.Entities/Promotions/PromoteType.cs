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
        ProductPromotion,//��Ʒ����
        SecondReducePrice,//�ڶ�������
        ProductDiscount = 9,//�ڶ�������
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
