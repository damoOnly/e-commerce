using EcShop.Entities;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace EcShop.SqlDal.Sales
{
    public class ShoppingCartDao
    {
        private Database database;
        public ShoppingCartDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        public ShoppingCartInfo GetShoppingCart(Member member)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT * FROM Ecshop_ShoppingCarts WHERE UserId = @UserId;SELECT * FROM Ecshop_GiftShoppingCarts gc JOIN Ecshop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId;
select pp.ProductId as PromotionProductId, s.SkuId,ppp.ProductId,pro.ProductName,s.SKU,0 as ShipmentQuantity,s.CostPrice,s.SalePrice as ItemListPrice,0 as ItemAdjustedPrice,'' as SKUContent,
s.FactStock,p.ActivityId as PromotionId,p.Name as PromotionName,p.IsAscend,case when isnull(p.IsAscend,0) = 0 then p.DiscountValue when p.IsAscend = 1 then c.Quantity /p.Condition *  p.DiscountValue end  as DiscountValue,pro.ThumbnailUrl40,0 as storeId,pro.SupplierId,sp.SupplierName,sp.ShopName,sp.ShopOwner 
from Ecshop_PromotionProductsPresent   ppp 
inner join Ecshop_Promotions p  on ppp.ActivityId = p.ActivityId 
inner join Ecshop_PromotionProducts pp on ppp.ActivityId = pp.ActivityId 
left join Ecshop_SKUs s on s.ProductId = ppp.ProductId 
left join Ecshop_Skus ss on ss.ProductId = pp.ProductId 
left join Ecshop_Products pro on pro.ProductId = ppp.ProductId 
left join Ecshop_Supplier sp on pro.SupplierId = sp.SupplierId 
inner join Ecshop_ShoppingCarts  c on c.skuid = ss.skuid and userid =@UserId ");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                //Member member = HiContext.Current.User as Member;
                while (dataReader.Read())
                {
                    int storeId = 0;
                    if (dataReader["storeId"] != System.DBNull.Value)
                    {
                        storeId = (int)dataReader["storeId"];
                    }
                    ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(member, (string)dataReader["SkuId"], (int)dataReader["Quantity"], storeId);
                    if (cartItemInfo != null)
                    {
                        shoppingCartInfo.LineItems.Add(cartItemInfo);
                    }
                }
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    ShoppingCartGiftInfo shoppingCartGiftInfo = DataMapper.PopulateGiftCartItem(dataReader);
                    shoppingCartGiftInfo.Quantity = (int)dataReader["Quantity"];
                    shoppingCartInfo.LineGifts.Add(shoppingCartGiftInfo);
                }
                // 促销活动(商品)中的赠送商品
                if (dataReader.NextResult())
                {
                    while (dataReader.Read())
                    {
                        ShoppingCartPresentInfo present = DataMapper.PopulatePresentCartItem(dataReader);
                        shoppingCartInfo.LinePresentPro.Add(present);
                    }
                }
            }
            return shoppingCartInfo;
        }

        public ShoppingCartInfo GetShoppingCart(int userId)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ShoppingCarts WHERE UserId = @UserId;SELECT * FROM Ecshop_GiftShoppingCarts gc JOIN Ecshop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                Member member = HiContext.Current.User as Member;
                while (dataReader.Read())
                {
                    int storeId = 0;
                    if (dataReader["storeId"] != System.DBNull.Value)
                    {
                        storeId = (int)dataReader["storeId"];
                    }
                    ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(member, (string)dataReader["SkuId"], (int)dataReader["Quantity"], storeId);
                    if (cartItemInfo != null)
                    {
                        shoppingCartInfo.LineItems.Add(cartItemInfo);
                    }
                }
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    ShoppingCartGiftInfo shoppingCartGiftInfo = DataMapper.PopulateGiftCartItem(dataReader);
                    shoppingCartGiftInfo.Quantity = (int)dataReader["Quantity"];
                    shoppingCartInfo.LineGifts.Add(shoppingCartGiftInfo);
                }
            }
            return shoppingCartInfo;
        }

        public ShoppingCartItemInfo GetCartItemInfo(Member member, string skuId, int quantity, int storeId)
        {
            ShoppingCartItemInfo shoppingCartItemInfo = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                if (dataReader.Read())
                {
                    shoppingCartItemInfo = new ShoppingCartItemInfo();
                    shoppingCartItemInfo.SkuId = skuId;
                    shoppingCartItemInfo.ShippQuantity = quantity;
                    shoppingCartItemInfo.Quantity = quantity;
                    shoppingCartItemInfo.ProductId = (int)dataReader["ProductId"];
                    shoppingCartItemInfo.BuyCardinality = 1;

                    if (dataReader["BuyCardinality"] != System.DBNull.Value)
                    {
                        shoppingCartItemInfo.BuyCardinality = (int)dataReader["BuyCardinality"];
                    }

                    shoppingCartItemInfo.Stock = 0;
                    if (DBNull.Value != dataReader["Stock"])
                    {
                        shoppingCartItemInfo.Stock = Convert.ToInt32(dataReader["Stock"]);
                    }
                    if (dataReader["SKU"] != DBNull.Value)
                    {
                        shoppingCartItemInfo.SKU = (string)dataReader["SKU"];
                    }
                    shoppingCartItemInfo.Name = (string)dataReader["ProductName"];
                    if (DBNull.Value != dataReader["Weight"])
                    {
                        shoppingCartItemInfo.Weight = (decimal)dataReader["Weight"];
                    }
                    shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = (decimal)dataReader["SalePrice"]);
                    if (DBNull.Value != dataReader["CountDownPrice"])
                    {
                        shoppingCartItemInfo.CountDownPrice = (decimal)dataReader["CountDownPrice"];
                    }
                    else
                    {
                        shoppingCartItemInfo.CountDownPrice = 0m;
                    }
                   
                    if (DBNull.Value != dataReader["ThumbnailUrl40"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl40 = dataReader["ThumbnailUrl40"].ToString();
                    }
                    if (DBNull.Value != dataReader["ThumbnailUrl60"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl60 = dataReader["ThumbnailUrl60"].ToString();
                    }
                    if (DBNull.Value != dataReader["ThumbnailUrl100"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl100 = dataReader["ThumbnailUrl100"].ToString();
                    }

                    if (DBNull.Value != dataReader["ThumbnailUrl160"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl160 = dataReader["ThumbnailUrl160"].ToString();
                    }

                    if (DBNull.Value != dataReader["ThumbnailUrl180"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl180 = dataReader["ThumbnailUrl180"].ToString();
                    }

                    if (DBNull.Value != dataReader["ThumbnailUrl220"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl220 = dataReader["ThumbnailUrl220"].ToString();
                    }

                    if (DBNull.Value != dataReader["IsfreeShipping"])
                    {
                        shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(dataReader["IsfreeShipping"]);
                    }
                    if (DBNull.Value != dataReader["TaxRate"])
                    {
                        shoppingCartItemInfo.TaxRate = Convert.ToDecimal(dataReader["TaxRate"]);
                    }
                    if (DBNull.Value != dataReader["MinTaxRate"])
                    {
                        shoppingCartItemInfo.MinTaxRate = Convert.ToDecimal(dataReader["MinTaxRate"]);
                    }
                    if (DBNull.Value != dataReader["MaxTaxRate"])
                    {
                        shoppingCartItemInfo.MaxTaxRate = Convert.ToDecimal(dataReader["MaxTaxRate"]);
                    }
                  
                    if (DBNull.Value != dataReader["TaxRateId"])
                    {
                        shoppingCartItemInfo.TaxRateId = Convert.ToInt32(dataReader["TaxRateId"]);
                    }
                    if (DBNull.Value != dataReader["TemplateId"])
                    {
                        shoppingCartItemInfo.TemplateId = Convert.ToInt32(dataReader["TemplateId"]);
                    }
                    if (DBNull.Value != dataReader["SupplierId"])
                    {
                        shoppingCartItemInfo.SupplierId = Convert.ToInt32(dataReader["SupplierId"]);
                    }
                    if (DBNull.Value != dataReader["ShopName"])
                    {
                        shoppingCartItemInfo.SupplierName = dataReader["ShopName"].ToString();
                    }

                    if (DBNull.Value != dataReader["Logo"])
                    {
                        shoppingCartItemInfo.Logo = dataReader["Logo"].ToString();
                    }


                    shoppingCartItemInfo.IsCustomsClearance = false;
                    if (DBNull.Value != dataReader["IsCustomsClearance"])
                    {
                        shoppingCartItemInfo.IsCustomsClearance = Convert.ToBoolean(dataReader["IsCustomsClearance"]);
                    }
                    shoppingCartItemInfo.StoreId = storeId;//门店id

                    //品牌Id
                    if (DBNull.Value != dataReader["BrandId"])
                    {
                        shoppingCartItemInfo.BrandId = Convert.ToInt32(dataReader["BrandId"]);
                    }
                    //供应商Id
                    if (DBNull.Value != dataReader["CategoryId"])
                    {
                        shoppingCartItemInfo.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
                    }

                    // 销售类型
                    if(DBNull.Value != dataReader["SaleType"])
                    {
                        shoppingCartItemInfo.SaleType = Convert.ToInt32(dataReader["SaleType"]);
                    }

                    string text = string.Empty;
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["AttributeName"]) && dataReader["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["ValueStr"]))
                            {
                                object obj = text;
                                text = string.Concat(new object[]
								{
									obj,
									dataReader["AttributeName"],
									"：",
									dataReader["ValueStr"],
									"; "
								});
                            }
                            shoppingCartItemInfo.DeductFee = decimal.Parse(dataReader["DeductFee"] == DBNull.Value ? "0" : dataReader["DeductFee"].ToString());
                        }
                    }
                    shoppingCartItemInfo.SkuContent = text;
                    PromotionInfo promotionInfo = null;
                    if (dataReader.NextResult() && dataReader.Read())
                    {
                        promotionInfo = DataMapper.PopulatePromote(dataReader);
                    }
                    if (promotionInfo != null)
                    {
                        shoppingCartItemInfo.PromoteType = promotionInfo.PromoteType;
                        switch (promotionInfo.PromoteType)
                        {
                            case PromoteType.Discount:
                                shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice * promotionInfo.DiscountValue;
                                break;
                            case PromoteType.Amount:
                                shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                shoppingCartItemInfo.AdjustedPrice = promotionInfo.DiscountValue;
                                break;
                            case PromoteType.Reduced:
                                shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice - promotionInfo.DiscountValue;
                                break;
                            case PromoteType.QuantityDiscount:
                                if (shoppingCartItemInfo.Quantity >= (int)promotionInfo.Condition)
                                {
                                    shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                    shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                    shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice * promotionInfo.DiscountValue;
                                }
                                break;
                            case PromoteType.SentGift:
                                shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                shoppingCartItemInfo.IsSendGift = true;
                                break;
                            case PromoteType.SentProduct:
                                if (shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition >= 1)
                                {
                                    shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                    shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                    shoppingCartItemInfo.ShippQuantity = shoppingCartItemInfo.Quantity + shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition * (int)promotionInfo.DiscountValue;
                                }
                                break;
                            case PromoteType.PresentProduct:
                                if (shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition >= 1)
                                {
                                    shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                    shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                }
                                break;
                            //单品满减
                            case PromoteType.ProductPromotion:
                                if (shoppingCartItemInfo.MemberPrice * shoppingCartItemInfo.Quantity >= promotionInfo.Condition)
                                {
                                    shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                    shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                    shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice;
                                    shoppingCartItemInfo.PromotionPrice = shoppingCartItemInfo.MemberPrice - promotionInfo.DiscountValue;
                                    shoppingCartItemInfo.PromoteType = PromoteType.ProductPromotion;
                                }
                                break;
                            //第二件减价
                            case PromoteType.SecondReducePrice:
                                if (shoppingCartItemInfo.Quantity >= 2 && shoppingCartItemInfo.MemberPrice * shoppingCartItemInfo.Quantity >= (promotionInfo.Condition))
                                {
                                    shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                    shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                    shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice;
                                    shoppingCartItemInfo.PromotionPrice = shoppingCartItemInfo.MemberPrice - promotionInfo.DiscountValue;
                                    shoppingCartItemInfo.PromoteType = PromoteType.SecondReducePrice;

                                }
                                break;
                            //第二件打折
                            case PromoteType.ProductDiscount:
                                if (shoppingCartItemInfo.Quantity >= 2)
                                {
                                    shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
                                    shoppingCartItemInfo.PromotionName = promotionInfo.Name;
                                    shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice;
                                    shoppingCartItemInfo.PromotionPrice = shoppingCartItemInfo.MemberPrice * promotionInfo.DiscountValue;
                                    shoppingCartItemInfo.PromoteType = PromoteType.ProductDiscount;
                                }
                                break;
                        }
                    }
                    // 促销活动(商品)中的赠送商品
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            ShoppingCartPresentInfo present = DataMapper.PopulatePresentCartItem(dataReader);
                            shoppingCartItemInfo.LinePresentPro.Add(present);
                        }
                    }

                    // 组合商品明细
                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            ProductsCombination combination = DataMapper.PopulateCombinationCartItem(dataReader);
                            shoppingCartItemInfo.CombinationItemInfos.Add(combination);
                        }
                    }
                }
            }

            if (shoppingCartItemInfo != null && shoppingCartItemInfo.CombinationItemInfos != null 
                && shoppingCartItemInfo.CombinationItemInfos.Count > 1)
            {
                var SumBeforePrice = shoppingCartItemInfo.CombinationItemInfos.Take(shoppingCartItemInfo.CombinationItemInfos.Count -1).Sum(t => t.Price*t.Quantity);

                if (shoppingCartItemInfo.CountDownPrice - SumBeforePrice > 0)
                {
                    int qty = shoppingCartItemInfo.CombinationItemInfos.LastOrDefault().Quantity;

                    decimal lastPrice = Math.Floor(((shoppingCartItemInfo.CountDownPrice - SumBeforePrice) / qty) * 100) / 100;

                    shoppingCartItemInfo.CombinationItemInfos.LastOrDefault().Price = lastPrice;
                }
                
            }

            return shoppingCartItemInfo;
        }
        public AddCartItemStatus AddLineItem(Member member, string skuId, int quantity, int storeId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "storeId", DbType.Int32, storeId);
            AddCartItemStatus result;
            if (this.database.ExecuteNonQuery(storedProcCommand) > 0)
            {
                result = AddCartItemStatus.Successed;
            }
            else
            {
                result = AddCartItemStatus.ProductNotExists;
            }
            return result;
        }
        public void RemoveLineItem(int userId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public void RemoveLineItems(int userId, List<string> skuIds)
        {
            if (skuIds.Count > 0)
            {
                string skus = "'" + string.Join("', '", skuIds) + "'";

                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Ecshop_ShoppingCarts WHERE UserId = @UserId AND SkuId IN ({0})", skus));
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);

                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public void UpdateLineItemQuantity(Member member, string skuId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public void ClearShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ShoppingCarts WHERE UserId = @UserId; DELETE FROM Ecshop_GiftShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        /// <summary>
        /// 清除用户的购物车中已经下单的商品，未结算的商品保留
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="skuIds">未结算的商品</param>
        public void ClearPartShoppingCart(int userId, string skuIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Ecshop_ShoppingCarts WHERE UserId = @UserId  and SkuId not in ({0}); DELETE FROM Ecshop_GiftShoppingCarts WHERE UserId = @UserId", skuIds));
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        /// <summary>
        /// 清除用户的购物车中已经下单的商品，未结算的商品保留
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="skuIds">未结算的商品</param>
        public void ClearShoppingCartBySkus(int userId, string skuIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Ecshop_ShoppingCarts WHERE UserId = @UserId  and SkuId IN ({0}); DELETE FROM Ecshop_GiftShoppingCarts WHERE UserId = @UserId", skuIds));
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public bool AddGiftItem(int giftId, int quantity, PromoteType promotype)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF  EXISTS(SELECT GiftId FROM Ecshop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType) UPDATE Ecshop_GiftShoppingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType; ELSE INSERT INTO Ecshop_GiftShoppingCarts(UserId, GiftId, Quantity, AddTime,PromoType) VALUES (@UserId, @GiftId, @Quantity, @AddTime,@PromoType)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_GiftShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public int GetGiftItemQuantity(PromoteType promotype)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(Quantity),0) as Quantity FROM Ecshop_GiftShoppingCarts WHERE UserId = @UserId AND PromoType=@PromoType");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = int.Parse(dataReader["Quantity"].ToString());
                }
            }
            return result;
        }
        public void RemoveGiftItem(int giftId, PromoteType promotype)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}
