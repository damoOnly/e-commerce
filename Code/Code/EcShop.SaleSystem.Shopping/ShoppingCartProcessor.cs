using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Promotions;
using EcShop.SqlDal.Sales;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EcShop.SaleSystem.Shopping
{
	public static class ShoppingCartProcessor
	{
        public static ShoppingCartInfo GetShoppingCart()
        {
            Member member = HiContext.Current.User as Member;
            return GetShoppingCart(member);
        }
        public static ShoppingCartInfo GetShoppingCart(Member member)
        {
            // 支持 App api
            //Member member = HiContext.Current.User as Member;
			ShoppingCartInfo result;
			if (member != null)
			{
				ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(member);
				if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count == 0)
				{
					result = null;
				}
				else
				{
					decimal reducedPromotionAmount = 0m;
					PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(member, shoppingCart.GetAmount(), shoppingCart.GetQuantity(), out reducedPromotionAmount);
					if (reducedPromotion != null)
					{
						shoppingCart.ReducedPromotionId = reducedPromotion.ActivityId;
						shoppingCart.ReducedPromotionName = reducedPromotion.Name;
						shoppingCart.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCart.IsReduced = true;
					}
					PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(member, shoppingCart.GetTotal(), PromoteType.FullAmountSentGift);
					if (sendPromotion != null)
					{
						shoppingCart.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCart.SendGiftPromotionName = sendPromotion.Name;
						shoppingCart.IsSendGift = true;
					}
					PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(member, shoppingCart.GetTotal(), PromoteType.FullAmountSentTimesPoint);
					if (sendPromotion2 != null)
					{
						shoppingCart.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCart.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCart.IsSendTimesPoint = true;
						shoppingCart.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(member, shoppingCart.GetTotal(), PromoteType.FullAmountSentFreight);
					if (sendPromotion3 != null)
					{
						shoppingCart.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCart.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCart.IsFreightFree = true;
					}
					result = shoppingCart;
				}
			}
			else
			{
				result = new CookieShoppingDao().GetShoppingCart();
			}
			return result;
		}

        public static ShoppingCartInfo GetPartShoppingCartInfo(string skuIds) //获取顾客选择的待下单的购物车信息
        {
            if (!string.IsNullOrEmpty(skuIds))
            {
               skuIds = EcShop.Core.Globals.UrlDecode(skuIds);
            }
            
            string[] skuIdsArray = skuIds.Split(',');
            HashSet<string> hsSkuIds = new HashSet<string>();
            for (int i = 0; i < skuIdsArray.Length; i++)
            {
                if (skuIdsArray[i].Trim().Length == 0)
                {
                    continue;
                }
                hsSkuIds.Add(skuIdsArray[i]);
            }
            ShoppingCartInfo result = GetShoppingCart();
            if (hsSkuIds.Count == 0)
            {
                return result;
            }
            List<ShoppingCartItemInfo> listShoppingCartItemInfo = new List<ShoppingCartItemInfo>();
            if (result != null)
            {
                int count = result.LineItems.Count;
                for (int i = 0; i < count; i++)
                {
                    if(hsSkuIds.Contains(result.LineItems[i].SkuId))
                    {
                        listShoppingCartItemInfo.Add(result.LineItems[i]);
                    }
                }
                foreach (ShoppingCartItemInfo item in listShoppingCartItemInfo)
                {
                    result.LineItems.Remove(item);
                }
                Member member = HiContext.Current.User as Member;
                if (member != null)
                {
                    if (result.LineItems.Count == 0 && result.LineGifts.Count == 0)
                    {
                        result = null;
                    }
                    else
                    {
                        decimal reducedPromotionAmount = 0m;
                        PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(member, result.GetAmount(), result.GetQuantity(), out reducedPromotionAmount);
                        if (reducedPromotion != null)
                        {
                            result.ReducedPromotionId = reducedPromotion.ActivityId;
                            result.ReducedPromotionName = reducedPromotion.Name;
                            result.ReducedPromotionAmount = reducedPromotionAmount;
                            result.IsReduced = true;
                        }
                        else
                        {
                            result.ReducedPromotionId = 0;
                            result.ReducedPromotionName = "";
                            result.ReducedPromotionAmount = 0m;
                            result.IsReduced = false;
                        }
                        PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(member, result.GetTotal(), PromoteType.FullAmountSentGift);
                        if (sendPromotion != null)
                        {
                            result.SendGiftPromotionId = sendPromotion.ActivityId;
                            result.SendGiftPromotionName = sendPromotion.Name;
                            result.IsSendGift = true;
                        }
                        else
                        {
                            result.SendGiftPromotionId = 0;
                            result.SendGiftPromotionName = "";
                            result.IsSendGift = false;
                        }
                        PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(member, result.GetTotal(), PromoteType.FullAmountSentTimesPoint);
                        if (sendPromotion2 != null)
                        {
                            result.SentTimesPointPromotionId = sendPromotion2.ActivityId;
                            result.SentTimesPointPromotionName = sendPromotion2.Name;
                            result.IsSendTimesPoint = true;
                            result.TimesPoint = sendPromotion2.DiscountValue;
                        }
                        else
                        {
                            result.SentTimesPointPromotionId = 0;
                            result.SentTimesPointPromotionName = "";
                            result.IsSendTimesPoint = false;
                            result.TimesPoint = 0m;
                        }
                        PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(member, result.GetTotal(), PromoteType.FullAmountSentFreight);
                        if (sendPromotion3 != null)
                        {
                            result.FreightFreePromotionId = sendPromotion3.ActivityId;
                            result.FreightFreePromotionName = sendPromotion3.Name;
                            result.IsFreightFree = true;
                        }
                        else
                        {
                            result.FreightFreePromotionId = 0;
                            result.FreightFreePromotionName = "";
                            result.IsFreightFree = false;
                        }
                    }
                }
            }
            return result;

        }
        public static ShoppingCartInfo GetShoppingCartInfoBySkus(Member member, string skuIds) //获取顾客选择的待下单的购物车信息
        {
            string[] skuIdsArray = skuIds.Split(',');

            HashSet<string> hsSkuIds = new HashSet<string>();

            for (int i = 0; i < skuIdsArray.Length; i++)
            {
                if (skuIdsArray[i].Trim().Length == 0)
                {
                    continue;
                }
                hsSkuIds.Add(skuIdsArray[i]);
            }

            ShoppingCartInfo result = GetShoppingCart(member);

            //判断购物车是否为空
            if(result==null)
            {
                return result;
            }

            if (hsSkuIds.Count == result.LineItems.Count)    //如果传进来的Sku个数与数据库一致，则返回所有
            {
                return result;
            }

            if (result != null)
            {
                List<ShoppingCartItemInfo> listShoppingCartItemInfo = new List<ShoppingCartItemInfo>();

                int count = result.LineItems.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!hsSkuIds.Contains(result.LineItems[i].SkuId))
                    {
                        listShoppingCartItemInfo.Add(result.LineItems[i]);
                    }
                }

                foreach (ShoppingCartItemInfo item in listShoppingCartItemInfo)
                {
                    result.LineItems.Remove(item);
                }

                if (member != null)
                {
                    if (result.LineItems.Count == 0 && result.LineGifts.Count == 0)
                    {
                        result = null;
                    }
                    else
                    {
                        decimal reducedPromotionAmount = 0m;
                        PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(member, result.GetAmount(), result.GetQuantity(), out reducedPromotionAmount);
                        if (reducedPromotion != null)
                        {
                            result.ReducedPromotionId = reducedPromotion.ActivityId;
                            result.ReducedPromotionName = reducedPromotion.Name;
                            result.ReducedPromotionAmount = reducedPromotionAmount;
                            result.IsReduced = true;
                        }
                        else
                        {
                            result.IsReduced = false;
                        }
                        PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(member, result.GetTotal(), PromoteType.FullAmountSentGift);
                        if (sendPromotion != null)
                        {
                            result.SendGiftPromotionId = sendPromotion.ActivityId;
                            result.SendGiftPromotionName = sendPromotion.Name;
                            result.IsSendGift = true;
                        }
                        else
                        {
                            result.IsSendGift = false;
                        }
                        PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(member, result.GetTotal(), PromoteType.FullAmountSentTimesPoint);
                        if (sendPromotion2 != null)
                        {
                            result.SentTimesPointPromotionId = sendPromotion2.ActivityId;
                            result.SentTimesPointPromotionName = sendPromotion2.Name;
                            result.IsSendTimesPoint = true;
                            result.TimesPoint = sendPromotion2.DiscountValue;
                        }
                        else
                        {
                            result.IsSendTimesPoint = false;
                        }
                        PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(member, result.GetTotal(), PromoteType.FullAmountSentFreight);
                        if (sendPromotion3 != null)
                        {
                            result.FreightFreePromotionId = sendPromotion3.ActivityId;
                            result.FreightFreePromotionName = sendPromotion3.Name;
                            result.IsFreightFree = true;
                        }
                        else
                        {
                            result.IsFreightFree = false;
                        }
                    }
                }
            }

            return result;

        }
		public static ShoppingCartInfo GetCookieShoppingCart()
		{
			return new CookieShoppingDao().GetShoppingCart();
		}
        public static AddCartItemStatus AddLineItem(string skuId, int quantity, int storeId)
        {
            Member member = HiContext.Current.User as Member;
            return AddLineItem(member, skuId, quantity, storeId);
        }

        public static AddCartItemStatus AddLineItem(Member member, string skuId, int quantity, int storeId)
		{
			//Member member = HiContext.Current.User as Member;
			if (quantity <= 0)
			{
				quantity = 1;
			}
			AddCartItemStatus result;
			if (member != null)
			{
                result = new ShoppingCartDao().AddLineItem(member, skuId, quantity, storeId);
			}
			else
			{
                result = new CookieShoppingDao().AddLineItem(skuId, quantity, storeId);
			}
			return result;
		}
		public static void ConvertShoppingCartToDataBase(ShoppingCartInfo shoppingCart)
		{
			Member member = HiContext.Current.User as Member;
			if (member != null)
			{
				ShoppingCartDao shoppingCartDao = new ShoppingCartDao();
				if (shoppingCart.LineItems.Count > 0)
				{
					foreach (ShoppingCartItemInfo current in shoppingCart.LineItems)
					{
						shoppingCartDao.AddLineItem(member, current.SkuId, current.Quantity,current.StoreId);
					}
				}
				if (shoppingCart.LineGifts.Count > 0)
				{
					foreach (ShoppingCartGiftInfo current2 in shoppingCart.LineGifts)
					{
						shoppingCartDao.AddGiftItem(current2.GiftId, current2.Quantity, (PromoteType)current2.PromoType);
					}
				}
			}
		}
        public static void RemoveLineItem(Member member, string skuId)
        {
            if (member == null)
            {
                new CookieShoppingDao().RemoveLineItem(skuId);
            }
            else
            {
                new ShoppingCartDao().RemoveLineItem(member.UserId, skuId);
            }
        }
		public static void RemoveLineItem(string skuId)
		{
			if (HiContext.Current.User.IsAnonymous)
			{
				new CookieShoppingDao().RemoveLineItem(skuId);
			}
			else
			{
				new ShoppingCartDao().RemoveLineItem(HiContext.Current.User.UserId, skuId);
			}
		}

        public static void RemoveLineItems(Member member, List<string> skuIds)
        {
            new ShoppingCartDao().RemoveLineItems(member.UserId, skuIds);
        }

        public static void UpdateLineItemQuantity(string skuId, int quantity)
        {
            Member member = HiContext.Current.User as Member;
            UpdateLineItemQuantity(member, skuId, quantity);
        }
		public static void UpdateLineItemQuantity(Member member, string skuId, int quantity)
		{
			//Member member = HiContext.Current.User as Member;
			if (quantity <= 0)
			{
				ShoppingCartProcessor.RemoveLineItem(skuId);
			}
			if (member == null)
			{
				new CookieShoppingDao().UpdateLineItemQuantity(skuId, quantity);
			}
			else
			{
				new ShoppingCartDao().UpdateLineItemQuantity(member, skuId, quantity);
			}
		}
		public static bool AddGiftItem(int giftId, int quantity, PromoteType promotype)
		{
			bool result;
			if (HiContext.Current.User.IsAnonymous)
			{
				result = new CookieShoppingDao().AddGiftItem(giftId, quantity);
			}
			else
			{
				result = new ShoppingCartDao().AddGiftItem(giftId, quantity, promotype);
			}
			return result;
		}
		public static int GetGiftItemQuantity(PromoteType promotype)
		{
			return new ShoppingCartDao().GetGiftItemQuantity(promotype);
		}
		public static void RemoveGiftItem(int giftId, PromoteType promotype)
		{
			if (HiContext.Current.User.IsAnonymous)
			{
				new CookieShoppingDao().RemoveGiftItem(giftId);
			}
			else
			{
				new ShoppingCartDao().RemoveGiftItem(giftId, promotype);
			}
		}
		public static void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype)
		{
			Member member = HiContext.Current.User as Member;
			if (quantity <= 0)
			{
				ShoppingCartProcessor.RemoveGiftItem(giftId, promotype);
			}
			if (member == null)
			{
				new CookieShoppingDao().UpdateGiftItemQuantity(giftId, quantity);
			}
			else
			{
				new ShoppingCartDao().UpdateGiftItemQuantity(giftId, quantity, promotype);
			}
		}
        public static void ClearShoppingCart(int userId)
        {
            new ShoppingCartDao().ClearShoppingCart(userId);
        }
		public static void ClearShoppingCart()
		{
			if (HiContext.Current.User.IsAnonymous)
			{
				new CookieShoppingDao().ClearShoppingCart();
			}
			else
			{
				new ShoppingCartDao().ClearShoppingCart(HiContext.Current.User.UserId);
			}
		}
        public static void ClearPartShoppingCart(string skuIds)//删除购物车部分商品
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                new CookieShoppingDao().ClearShoppingCart();
            }
            else
            {
                if (!string.IsNullOrEmpty(skuIds) && skuIds!=",")
                {
                    StringBuilder stringBuilder=new StringBuilder();
                    string[] strSkus = skuIds.Split(',');
                    for (int i = 0; i < strSkus.Length; i++)
                    {
                        if (i == (strSkus.Length-1))
                        {
                            stringBuilder.AppendFormat("'{0}'", strSkus[i]);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("'{0}',", strSkus[i]);
                        }
                    }
                    new ShoppingCartDao().ClearPartShoppingCart(HiContext.Current.User.UserId,stringBuilder.ToString());
                }
                else
                {
                    ClearShoppingCart();
                }
            }
        }
        public static void ClearPartShoppingCart(int userId, string skuIds)//删除购物车部分商品
        {
            if (!string.IsNullOrEmpty(skuIds) && skuIds != ",")
            {
                if (skuIds.StartsWith(","))
                {
                    skuIds = skuIds.Substring(1);
                }

                if (skuIds.EndsWith(","))
                {
                    skuIds = skuIds.Substring(0, skuIds.Length - 1);
                }

                skuIds = "'" + skuIds.Replace(",", "', '") + "'";

                new ShoppingCartDao().ClearShoppingCartBySkus(userId, skuIds);
            }
            else
            {
                ClearShoppingCart(userId);
            }
        }
		public static void ClearCookieShoppingCart()
		{
			new CookieShoppingDao().ClearShoppingCart();
		}


        /// <summary>
        /// 获取skuid最小库存,如果含有组合商品，则组合商品的库存为其中商品数量的库存除以组合系数的最小值
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
		public static int GetSkuStock(string skuId)
		{
            //SKUItem skuItem = new SkuDao().GetSkuItem(skuId);
            //int result;
            //if (skuItem != null)
            //{
            //    result = skuItem.Stock;
            //}
            //else
            //{
            //    result = 0;
            //}

            //修改获取库存方法，添加组合商品逻辑

            int result = new SkuDao().GetMinStockBySku(skuId);

			return result;
		}
        public static ShoppingCartInfo GetGroupBuyShoppingCart(string productSkuId, int buyAmount,int storeId)
        {
            Member member = HiContext.Current.User as Member;
            return GetGroupBuyShoppingCart(member, productSkuId, buyAmount, storeId);
        }
		public static ShoppingCartInfo GetGroupBuyShoppingCart(Member member, string productSkuId, int buyAmount,int storeId)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(member, productSkuId, buyAmount, storeId);
			ShoppingCartInfo result;
			if (cartItemInfo == null)
			{
				result = null;
			}
			else
			{
				GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(cartItemInfo.ProductId);
				if (productGroupBuyInfo == null || productGroupBuyInfo.StartDate > DateTime.Now || productGroupBuyInfo.Status != GroupBuyStatus.UnderWay)
				{
					result = null;
				}
				else
				{
					ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = cartItemInfo.SkuId;
					shoppingCartItemInfo.ProductId = cartItemInfo.ProductId;
					shoppingCartItemInfo.SKU = cartItemInfo.SKU;
					shoppingCartItemInfo.Name = cartItemInfo.Name;
					shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = productGroupBuyInfo.GroupBuyConditions[0].Price);
					shoppingCartItemInfo.SkuContent = cartItemInfo.SkuContent;
					ShoppingCartItemInfo arg_ED_0 = shoppingCartItemInfo;
					shoppingCartItemInfo.ShippQuantity = buyAmount;
					arg_ED_0.Quantity = buyAmount;
					shoppingCartItemInfo.Weight = cartItemInfo.Weight;
					shoppingCartItemInfo.ThumbnailUrl40 = cartItemInfo.ThumbnailUrl40;
					shoppingCartItemInfo.ThumbnailUrl60 = cartItemInfo.ThumbnailUrl60;
					shoppingCartItemInfo.ThumbnailUrl100 = cartItemInfo.ThumbnailUrl100;
					shoppingCartItemInfo.IsfreeShipping = cartItemInfo.IsfreeShipping;
					shoppingCartInfo.LineItems.Add(shoppingCartItemInfo);
					result = shoppingCartInfo;
				}
			}
			return result;
		}
        public static ShoppingCartInfo GetCountDownShoppingCart(string productSkuId, int buyAmount,int storeId)
        {
            Member member = HiContext.Current.User as Member;
            return GetCountDownShoppingCart(member, productSkuId, buyAmount, storeId);
        }
		public static ShoppingCartInfo GetCountDownShoppingCart(Member member, string productSkuId, int buyAmount,int storeId)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(member, productSkuId, buyAmount, storeId);
			ShoppingCartInfo result;
			if (cartItemInfo == null)
			{
				result = null;
			}
			else
			{
				CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(cartItemInfo.ProductId);

				if (countDownInfo == null)
				{
					result = null;
				}
				else
				{
					ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = cartItemInfo.SkuId;
					shoppingCartItemInfo.ProductId = cartItemInfo.ProductId;
					shoppingCartItemInfo.SKU = cartItemInfo.SKU;
					shoppingCartItemInfo.Name = cartItemInfo.Name;
					shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = countDownInfo.CountDownPrice);
					shoppingCartItemInfo.SkuContent = cartItemInfo.SkuContent;	
	
                    //修改数量
					shoppingCartItemInfo.ShippQuantity = buyAmount;
                    shoppingCartItemInfo.Quantity = buyAmount;
					shoppingCartItemInfo.Weight = cartItemInfo.Weight;
					shoppingCartItemInfo.ThumbnailUrl40 = cartItemInfo.ThumbnailUrl40;
					shoppingCartItemInfo.ThumbnailUrl60 = cartItemInfo.ThumbnailUrl60;
					shoppingCartItemInfo.ThumbnailUrl100 = cartItemInfo.ThumbnailUrl100;
					shoppingCartItemInfo.IsfreeShipping = cartItemInfo.IsfreeShipping;
                    shoppingCartItemInfo.TaxRate = cartItemInfo.TaxRate;
                    shoppingCartItemInfo.MaxTaxRate = cartItemInfo.MaxTaxRate;
                    shoppingCartItemInfo.MinTaxRate = cartItemInfo.MinTaxRate;
                    shoppingCartItemInfo.TemplateId = cartItemInfo.TemplateId;

                    if (cartItemInfo != null && cartItemInfo.CombinationItemInfos != null
                        && cartItemInfo.CombinationItemInfos.Count > 0)
                    {
                        shoppingCartItemInfo.CombinationItemInfos = cartItemInfo.CombinationItemInfos;
                    }

					shoppingCartInfo.LineItems.Add(shoppingCartItemInfo);

                    //订单满额包邮
                    PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(member, (countDownInfo.CountDownPrice)*buyAmount, PromoteType.FullAmountSentFreight);
                    if (sendPromotion3 != null)
                    {
                        shoppingCartInfo.FreightFreePromotionId = sendPromotion3.ActivityId;
                        shoppingCartInfo.FreightFreePromotionName = sendPromotion3.Name;
                        shoppingCartInfo.IsFreightFree = true;
                    }

                    

					result = shoppingCartInfo;
				}
			}
			return result;
		}
        public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount, int storeId)
        {
            Member member = HiContext.Current.User as Member;
            return GetShoppingCart(member, productSkuId, buyAmount, storeId);
        }
        public static ShoppingCartInfo GetShoppingCart(Member member, string productSkuId, int buyAmount, int storeId)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            //Member member = HiContext.Current.User as Member;
            ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(member, productSkuId, buyAmount, storeId);
			ShoppingCartInfo result;
			if (cartItemInfo == null)
			{
				result = null;
			}
			else
			{
				shoppingCartInfo.LineItems.Add(cartItemInfo);
				if (member != null)
				{
					decimal reducedPromotionAmount = 0m;
					PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(member, shoppingCartInfo.GetAmount(), shoppingCartInfo.GetQuantity(), out reducedPromotionAmount);
					if (reducedPromotion != null)
					{
						shoppingCartInfo.ReducedPromotionId = reducedPromotion.ActivityId;
						shoppingCartInfo.ReducedPromotionName = reducedPromotion.Name;
						shoppingCartInfo.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCartInfo.IsReduced = true;
					}
					PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(member, shoppingCartInfo.GetTotal(), PromoteType.FullAmountSentGift);
					if (sendPromotion != null)
					{
						shoppingCartInfo.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCartInfo.SendGiftPromotionName = sendPromotion.Name;
						shoppingCartInfo.IsSendGift = true;
					}
					PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(member, shoppingCartInfo.GetTotal(), PromoteType.FullAmountSentTimesPoint);
					if (sendPromotion2 != null)
					{
						shoppingCartInfo.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCartInfo.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCartInfo.IsSendTimesPoint = true;
						shoppingCartInfo.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(member, shoppingCartInfo.GetTotal(), PromoteType.FullAmountSentFreight);
					if (sendPromotion3 != null)
					{
						shoppingCartInfo.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCartInfo.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCartInfo.IsFreightFree = true;
					}
				}
                // 促销活动(商品)中的赠送商品
                if (cartItemInfo.LinePresentPro != null && cartItemInfo.LinePresentPro.Count > 0)
                {
                    shoppingCartInfo.LinePresentPro = cartItemInfo.LinePresentPro;
                }
				result = shoppingCartInfo;
			}
			return result;
		}
		public static ShoppingCartInfo GetShoppingCart(int Boundlingid, int buyAmount)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			List<BundlingItemInfo> bundlingItemsByID = ProductBrowser.GetBundlingItemsByID(Boundlingid);
			Member member = HiContext.Current.User as Member;
			ShoppingCartDao shoppingCartDao = new ShoppingCartDao();
			foreach (BundlingItemInfo current in bundlingItemsByID)
			{
				ShoppingCartItemInfo cartItemInfo = shoppingCartDao.GetCartItemInfo(member, current.SkuId, buyAmount * current.ProductNum,0);//未添加门店id
				if (cartItemInfo != null)
				{
					shoppingCartInfo.LineItems.Add(cartItemInfo);
				}
			}
			return shoppingCartInfo;
		}
        /// <summary>
        /// 计算运费，海美生活海购运费计算方式
        /// </summary>
        /// <param name="shoppingCartItemInfo">购物车信息</param>
        /// <param name="shippingAddressInfo">收货地址</param>
        /// <param name="isGroupBuy"></param>
        /// <returns>返回运费</returns>
        public static decimal GetFreight(ShoppingCartInfo shoppingCartItemInfo,ShippingAddressInfo shippingAddressInfo, bool isGroupBuy)
        {
            int totalQuantity = 0;
            decimal freight = 0;
            Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
            foreach (ShoppingCartItemInfo item in shoppingCartItemInfo.LineItems)
            {
                totalQuantity += item.Quantity;
                if ((!shoppingCartItemInfo.IsFreightFree || !item.IsfreeShipping || isGroupBuy))
                {
                    if (item.TemplateId > 0)
                    {
                        if (dictShippingMode.ContainsKey(item.TemplateId))
                        {
                            dictShippingMode[item.TemplateId] += item.Weight * item.Quantity;
                        }
                        else
                        {
                            dictShippingMode.Add(item.TemplateId, item.Weight * item.Quantity);
                        }
                    }
                }
            }
            foreach (var item in dictShippingMode)
            {
                ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                freight += ShoppingProcessor.CalcFreight(shippingAddressInfo.RegionId, item.Value, shippingMode);
            }
            return freight;
        }


        public static decimal GetFreight(ShoppingCartInfo shoppingCartItemInfo, int RegionId, bool isGroupBuy)
        {
            int totalQuantity = 0;
            decimal freight = 0;
            Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
            foreach (ShoppingCartItemInfo item in shoppingCartItemInfo.LineItems)
            {
                totalQuantity += item.Quantity;
                if ((!shoppingCartItemInfo.IsFreightFree || !item.IsfreeShipping || isGroupBuy))
                {
                    if (item.TemplateId > 0)
                    {
                        if (dictShippingMode.ContainsKey(item.TemplateId))
                        {
                            dictShippingMode[item.TemplateId] += item.Weight * item.Quantity;
                        }
                        else
                        {
                            dictShippingMode.Add(item.TemplateId, item.Weight * item.Quantity);
                        }
                    }
                }
            }
            foreach (var item in dictShippingMode)
            {
                ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                freight += ShoppingProcessor.CalcFreight(RegionId, item.Value, shippingMode);
            }
            return freight;
        }

    }
}
