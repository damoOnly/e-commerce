using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Promotions;
using EcShop.SqlDal.Promotions;
using System;
namespace EcShop.ControlPanel.Promotions
{
	public static class GiftHelper
	{
		public static GiftActionStatus AddGift(GiftInfo gift)
		{
			Globals.EntityCoding(gift, true);
			return new GiftDao().CreateUpdateDeleteGift(gift, DataProviderAction.Create);
		}
		public static GiftActionStatus UpdateGift(GiftInfo gift)
		{
			Globals.EntityCoding(gift, true);
			return new GiftDao().CreateUpdateDeleteGift(gift, DataProviderAction.Update);
		}
		public static bool DeleteGift(int giftId)
		{
			GiftInfo gift = new GiftInfo
			{
				GiftId = giftId
			};
			GiftActionStatus giftActionStatus = new GiftDao().CreateUpdateDeleteGift(gift, DataProviderAction.Delete);
			return giftActionStatus == GiftActionStatus.Success;
		}
		public static DbQueryResult GetGifts(GiftQuery query)
		{
			return new GiftDao().GetGifts(query);
		}
		public static GiftInfo GetGiftDetails(int giftId)
		{
			return new GiftDao().GetGiftDetails(giftId);
		}
		public static bool UpdateIsDownLoad(int giftId)
		{
			return new GiftDao().UpdateIsDownLoad(giftId);
		}
	}
}
