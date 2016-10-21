using EcShop.Entities.Promotions;
using System;
namespace EcShop.UI.SaleSystem.CodeBehind.Common
{
	public class PromotionHelper
	{
		public static string GetShortName(PromoteType promotionType)
		{
			string result = "";
			switch (promotionType)
			{
			case PromoteType.Discount:
			case PromoteType.QuantityDiscount:
				result = "折";
				break;
			case PromoteType.Amount:
				result = "促";
				break;
			case PromoteType.Reduced:
				result = "减";
				break;
			case PromoteType.SentGift:
				break;
			case PromoteType.SentProduct:
				result = "送";
				break;
			default:
				switch (promotionType)
				{
				case PromoteType.FullAmountSentTimesPoint:
					result = "积分加倍";
					break;
				case PromoteType.FullAmountSentFreight:
					result = "包邮";
					break;
				}
				break;
			}
			return result;
		}
	}
}
