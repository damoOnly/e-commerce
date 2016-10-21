using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Members;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Promotions;
using EcShop.SqlDal.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using EcShop.Core.ErrorLog;
using EcShop.SaleSystem.Catalog;

namespace EcShop.SaleSystem.Shopping
{
    public static class ShoppingProcessor
    {
        public static IList<string> GetSkuIdsBysku(string sku)
        {
            return new SkuDao().GetSkuIdsBysku(sku);
        }
        public static System.Data.DataTable GetProductInfoBySku(string skuId)
        {
            return new SkuDao().GetProductInfoBySku(skuId);
        }
        public static SKUItem GetProductAndSku(int productId, string options)
        {
            return new SkuDao().GetProductAndSku(HiContext.Current.User as Member, productId, options);
        }
        public static System.Data.DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
        {
            return new SkuDao().GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
        }
        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return new ShippingModeDao().GetShippingModes();
        }
        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return new ShippingModeDao().GetShippingMode(modeId, includeDetail);
        }
        public static ShippingModeInfo GetShippingMode(int TemplateId)
        {
            return new ShippingModeDao().GetShippingMode(TemplateId);
        }
        public static List<ShippingModeInfo> GetAllShippingMode()
        {
            return new ShippingModeDao().GetAllShippingMode();
        }
        public static IList<string> GetExpressCompanysByMode(int modeId)
        {
            return new ShippingModeDao().GetExpressCompanysByMode(modeId);
        }
        public static IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
        {
            return new PaymentModeDao().GetPaymentModes(payApplicationType);
        }
        public static PaymentModeInfo GetPaymentModeByModeId(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }
        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return new PaymentModeDao().GetPaymentMode(gateway);
        }
        public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isGroupBuy, bool isCountDown, bool isSignBuy, int userId)
        {
            OrderInfo result;
            if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count == 0)
            {
                result = null;
            }
            else
            {
                OrderInfo orderInfo = new OrderInfo();
                if (HiContext.Current.ReferralUserId != userId)
                {
                    orderInfo.ReferralUserId = HiContext.Current.ReferralUserId;
                }
                orderInfo.Points = shoppingCart.GetPoint();
                orderInfo.ReducedPromotionId = shoppingCart.ReducedPromotionId;
                orderInfo.ReducedPromotionName = shoppingCart.ReducedPromotionName;
                orderInfo.ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount;
                orderInfo.IsReduced = shoppingCart.IsReduced;
                orderInfo.SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId;
                orderInfo.SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName;
                orderInfo.IsSendTimesPoint = shoppingCart.IsSendTimesPoint;
                orderInfo.TimesPoint = shoppingCart.TimesPoint;
                orderInfo.FreightFreePromotionId = shoppingCart.FreightFreePromotionId;
                orderInfo.FreightFreePromotionName = shoppingCart.FreightFreePromotionName;
                orderInfo.IsFreightFree = shoppingCart.IsFreightFree;

                string text = string.Empty;
                if (shoppingCart.LineItems.Count > 0)
                {
                    foreach (ShoppingCartItemInfo current in shoppingCart.LineItems)
                    {
                        text += string.Format("'{0}',", current.SkuId);
                    }
                }
                Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
                if (!string.IsNullOrEmpty(text))
                {
                    text = text.Substring(0, text.Length - 1);
                    dictionary = new SkuDao().GetCostPriceForItems(text);
                }
                if (shoppingCart.LineItems.Count > 0)
                {
                    foreach (ShoppingCartItemInfo current in shoppingCart.LineItems)
                    {
                        decimal itemCostPrice = 0m;
                        if (dictionary.ContainsKey(current.SkuId))
                        {
                            itemCostPrice = dictionary[current.SkuId];
                        }
                        LineItemInfo lineItemInfo = new LineItemInfo();
                        lineItemInfo.SkuId = current.SkuId;
                        lineItemInfo.ProductId = current.ProductId;
                        lineItemInfo.SKU = current.SKU;
                        lineItemInfo.Quantity = current.Quantity;
                        lineItemInfo.ShipmentQuantity = current.ShippQuantity;
                        lineItemInfo.ItemCostPrice = itemCostPrice;
                        lineItemInfo.ItemListPrice = current.MemberPrice;
                        lineItemInfo.ItemAdjustedPrice = current.SubNewTotal / current.Quantity;//算平均价 以前为current.AdjustedPrice;
                        lineItemInfo.ItemDescription = current.Name;
                        lineItemInfo.ThumbnailsUrl = current.ThumbnailUrl40;
                        lineItemInfo.ItemWeight = current.Weight;
                        lineItemInfo.SKUContent = current.SkuContent;
                        lineItemInfo.PromotionId = current.PromotionId;
                        lineItemInfo.PromotionName = current.PromotionName;
                        lineItemInfo.TemplateId = current.TemplateId;
                        lineItemInfo.storeId = current.StoreId;
                        lineItemInfo.SupplierId = current.SupplierId;
                        lineItemInfo.SupplierName = current.SupplierName;//供货商
                        lineItemInfo.TaxRate = current.TaxRate;
                        lineItemInfo.PromoteType = current.PromoteType;
                        lineItemInfo.PromotionPrice = current.PromotionPrice;
                        lineItemInfo.DeductFee = current.DeductFee;//扣点

                        // 组合商品
                        lineItemInfo.CombinationItemInfos = current.CombinationItemInfos;

                        lineItemInfo.Tax = lineItemInfo.CalculateTax();
                        orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
                    }
                }
                orderInfo.Tax = 0.00m;
                orderInfo.InvoiceTitle = "";
                if (shoppingCart.LineGifts.Count > 0)
                {
                    foreach (ShoppingCartGiftInfo current2 in shoppingCart.LineGifts)
                    {
                        OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
                        orderGiftInfo.GiftId = current2.GiftId;
                        orderGiftInfo.GiftName = current2.Name;
                        orderGiftInfo.Quantity = current2.Quantity;
                        orderGiftInfo.ThumbnailsUrl = current2.ThumbnailUrl40;
                        orderGiftInfo.PromoteType = current2.PromoType;
                        orderGiftInfo.CostPrice = current2.CostPrice;
                        orderInfo.Gifts.Add(orderGiftInfo);
                    }
                }
                // 促销活动(商品)中的赠送商品
                if (shoppingCart.LinePresentPro != null && shoppingCart.LinePresentPro.Count > 0)
                {
                    orderInfo.PresentProducts = shoppingCart.LinePresentPro;
                }
                result = orderInfo;
            }
            return result;
        }


        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="orderInfo">订单信息</param>
        /// <param name="isChildAndFirstChildThenOrOriginalOrder"></param>
        /// <param name="isSplitOrder">是否扣销售库存（主订单才扣）</param>
        /// <returns></returns>
        public static bool CreateOrder(OrderInfo orderInfo, bool isChildAndFirstChildThenOrOriginalOrder, bool isSplitOrder)
        {
            //System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            //sw1.Start();
            bool result;
            if (orderInfo == null)
            {
                result = false;
            }
            else
            {
                bool flag = false;
                Database database = DatabaseFactory.CreateDatabase();
                using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                    try
                    {
                        OrderDao orderdao = new OrderDao();
                        ErrorLog.Write("Ready,OrderInfo is coming");
                        if (!orderdao.CreatOrder(orderInfo, dbTransaction, isChildAndFirstChildThenOrOriginalOrder))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }
                        if (orderInfo.LineItems.Count > 0)
                        {
                            ErrorLog.Write("Ready,OrderItems is coming");
                            if (!new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTransaction, orderInfo.PresentProducts))
                            {
                                dbTransaction.Rollback();
                                result = false;
                                return result;
                            }

                            //// 促销活动(商品)中的赠送商品
                            //if (orderInfo.PresentProducts != null && orderInfo.PresentProducts.Count > 0)
                            //{
                            //    if (!new LineItemDao().AddOrderPresents(orderInfo.OrderId, orderInfo.PresentProducts, dbTransaction))
                            //    {
                            //        dbTransaction.Rollback();
                            //        result = false;
                            //        return result;
                            //    }
                            //}
                        }

                        if (isSplitOrder)
                        {
                            //扣销售库存 2015-08-19
                            if (!orderdao.UpdatePayOrderStock(orderInfo.OrderId, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = false;
                                return result;
                            }

                            if (!string.IsNullOrEmpty(orderInfo.CouponCode))
                            {
                                if (!new CouponDao().AddCouponUseRecord(orderInfo, dbTransaction))
                                {
                                    dbTransaction.Rollback();
                                    result = false;
                                    return result;
                                }
                            }

                            if (!string.IsNullOrEmpty(orderInfo.VoucherCode))
                            {
                                if (!new VoucherDao().UpdateVoucherItemByUsed(orderInfo, dbTransaction))
                                {
                                    dbTransaction.Rollback();
                                    result = false;
                                    return result;
                                }
                            }
                        }

                        if (orderInfo.Gifts.Count > 0)
                        {
                            OrderGiftDao orderGiftDao = new OrderGiftDao();
                            foreach (OrderGiftInfo current in orderInfo.Gifts)
                            {
                                if (!orderGiftDao.AddOrderGift(orderInfo.OrderId, current, 0, dbTransaction))
                                {
                                    dbTransaction.Rollback();
                                    result = false;
                                    return result;
                                }
                            }
                        }

                        //isSplitOrder 主订单时把订单id写入优惠券和现金券
                        //if (!string.IsNullOrEmpty(orderInfo.CouponCode))
                        //{
                        //    if (!new CouponDao().AddCouponUseRecord(orderInfo, dbTransaction))
                        //    {
                        //        dbTransaction.Rollback();
                        //        result = false;
                        //        return result;
                        //    }
                        //}

                        //if (!string.IsNullOrEmpty(orderInfo.VoucherCode))
                        //{
                        //    if (!new VoucherDao().UpdateVoucherItemByUsed(orderInfo, dbTransaction))
                        //    {
                        //        dbTransaction.Rollback();
                        //        result = false;
                        //        return result;
                        //    }
                        //}
                        dbTransaction.Commit();
                        flag = true;
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection.Close();
                    }
                }
                result = flag;
            }

            //sw1.Stop();
            //ErrorLog.Write("执行CreateOrder 的时间:" + sw1.ElapsedMilliseconds.ToString());
            return result;
        }

        public static bool CutNeedPoint(int needPoint, string orderId)
        {
            Member member = HiContext.Current.User as Member;
            PointDetailInfo pointDetailInfo = new PointDetailInfo();
            pointDetailInfo.OrderId = orderId;
            pointDetailInfo.UserId = member.UserId;
            pointDetailInfo.TradeDate = DateTime.Now;
            pointDetailInfo.TradeType = PointTradeType.Change;
            pointDetailInfo.Reduced = new int?(needPoint);
            pointDetailInfo.Points = member.Points - needPoint;
            if (pointDetailInfo.Points > 2147483647 || pointDetailInfo.Points < 0)
            {
                pointDetailInfo.Points = 0;
            }
            bool result;
            if (new PointDetailDao().AddPointDetail(pointDetailInfo))
            {
                Users.ClearUserCache(member);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public static int CountDownOrderCount(int productid)
        {
            return new LineItemDao().CountDownOrderCount(productid);
        }

        public static int CountDownOrderCount(int productid, int userId, int countDownId)
        {
            return new LineItemDao().CountDownOrderCount(productid, userId, countDownId);
        }

        /// <summary>
        /// 查找用户购买限时抢购的商品的已购数量
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int CountDownOrderCount(int productid, int userid)
        {
            return new LineItemDao().CountDownOrderCount(productid, userid);
        }

        /// <summary>
        /// 获取限时抢购的商品的已购数量
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public static int AllCountDownOrderCount(int productid)
        {
            return new LineItemDao().AllCountDownOrderCount(productid);
        }

        /// <summary>
        /// 获取限时抢购的商品的已购数量
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public static int AllCountDownOrderCount(int productid, int countDownId)
        {
            return new LineItemDao().AllCountDownOrderCount(productid, countDownId);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }
         /// <summary>
        /// 获取子单列表
        /// </summary>
        /// <param name="SourceOrderId"></param>
        /// <returns></returns>
        public static List<OrderApiCode> Getlistofchildren(string SourceOrderId)
        {
            return new OrderDao().Getlistofchildren(SourceOrderId);
        }

        public static bool UpdateRefundOrderStock(string orderId)
        {
            return new OrderDao().UpdateRefundOrderStock(orderId);
        }

        public static OrderExpress GetOrderExpressInfo(string orderId)
        {
            return new OrderDao().GetOrderExpressInfo(orderId);
        }

        public static List<OrderInfo> GetChildOrdersBySourceOrder(string sourceOrderId)
        {
            return new OrderDao().GetChildOrdersBySourceOrder(sourceOrderId);
        }
        public static DataTable GetOrderItems(string orderId)
        {
            return new OrderDao().GetOrderItems(orderId);
        }
        public static System.Data.DataTable GetYetShipOrders(int days)
        {
            return new OrderDao().GetYetShipOrders(days);
        }
        public static CouponInfo GetCoupon(string couponCode)
        {
            return new CouponDao().GetCoupon(couponCode);
        }
        public static System.Data.DataTable GetCoupon(decimal orderAmount)
        {
            return new CouponDao().GetCoupon(orderAmount);
        }

        public static System.Data.DataTable GetCoupon(decimal orderAmount, string bindId, string categoryId)
        {
            return new CouponDao().GetCoupon(orderAmount, bindId, categoryId);
        }

        public static DataTable GetCoupon(int userId, decimal orderAmount)
        {
            return new CouponDao().GetCoupon(userId, orderAmount);
        }
        /// <summary>
        /// 根据现金券明细号码获取现金券模板
        /// </summary>
        /// <param name="voucherCode"></param>
        /// <returns></returns>
        public static VoucherInfo GetVoucher(string voucherCode)
        {
            return new VoucherDao().GetVoucherDetails(voucherCode);
        }


        /// <summary>
        /// 根据现金券明细号码和密码获取现金券模板
        /// </summary>
        /// <param name="voucherCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static VoucherInfo GetVoucher(string voucherCode, string password)
        {
            return new VoucherDao().GetVoucherDetails(voucherCode, password);
        }


        /// <summary>
        /// 获取所有可用的代金券
        /// </summary>
        /// <param name="orderAmount">当前订单总金额</param>
        /// <returns></returns>
        public static System.Data.DataTable GetVoucher(decimal orderAmount)
        {
            return new VoucherDao().GetVouchersByCurUser(orderAmount);
        }

        /// <summary>
        /// 根据号码和密码获取代金券
        /// </summary>
        /// <param name="code">代金券号码</param>
        /// <param name="pwd">代金券密码</param>
        /// <param name="orderAmount">金额</param>
        /// <returns>可享受的折扣</returns>
        public static decimal GetVoucherAmount(string code, string pwd, decimal orderAmount)
        {
            return new VoucherDao().GetVoucherAmount(code, pwd, orderAmount);
        }

        /// <summary>
        /// 根据号码和密码获取代金券
        /// </summary>
        /// <param name="code">代金券号码</param>
        /// <param name="pwd">代金券密码</param>
        /// <param name="orderAmount">金额</param>
        /// <returns>可享受的折扣</returns>
        public static VoucherInfo GetVoucherByItem(string code, string pwd, decimal orderAmount)
        {
            return new VoucherDao().GetVoucherByItem(code, pwd, orderAmount);
        }

        public static decimal GetChildOrderTotal(string masterOrderId)
        {
            return new OrderDao().GetChildOrderTotal(masterOrderId);
        }
        public static int GetProductStock(string skuId)
        {
            //if (string.IsNullOrEmpty(skuId))
            //    return 0;
            //return new ProductDao().GetProductStock(skuId);
            return new SkuDao().GetMinStockBySku(skuId);
        }
        public static PaymentModeInfo GetPaymentMode(int modeId)//获取支付方式
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }
        public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
        {
            CouponInfo result;
            if (string.IsNullOrEmpty(claimCode))
            {
                result = null;
            }
            else
            {
                CouponInfo coupon = ShoppingProcessor.GetCoupon(claimCode);
                if (coupon != null)
                {
                    if ((coupon.Amount.HasValue && coupon.Amount > 0m && orderAmount >= coupon.Amount.Value) || ((!coupon.Amount.HasValue || coupon.Amount == 0m) && orderAmount > coupon.DiscountValue))
                    {
                        result = coupon;
                        return result;
                    }
                }
                result = null;
            }
            return result;
        }
        public static System.Data.DataTable UseCoupon(decimal orderAmount)
        {
            return ShoppingProcessor.GetCoupon(orderAmount);
        }
        public static System.Data.DataTable UseCoupon(int userId, decimal orderAmount)
        {
            return ShoppingProcessor.GetCoupon(userId, orderAmount);
        }
        public static VoucherInfo UseVoucher(decimal orderAmount, string claimCode)
        {
            VoucherInfo result;
            if (string.IsNullOrEmpty(claimCode))
            {
                result = null;
            }
            else
            {
                VoucherInfo voucher = ShoppingProcessor.GetVoucher(claimCode);
                if (voucher != null)
                {
                    if ((voucher.Amount.HasValue && voucher.Amount > 0m && orderAmount >= voucher.Amount.Value) || ((!voucher.Amount.HasValue || voucher.Amount == 0m) && orderAmount > voucher.DiscountValue))
                    {
                        result = voucher;
                        return result;
                    }
                }
                result = null;
            }
            return result;
        }

        public static VoucherInfo UseVoucher(decimal orderAmount, string claimCode, string password)
        {
            VoucherInfo result;
            if (string.IsNullOrEmpty(claimCode) || string.IsNullOrEmpty(password))
            {
                result = null;
            }
            else
            {
                VoucherInfo voucher = ShoppingProcessor.GetVoucher(claimCode, password);
                if (voucher != null)
                {
                    if ((voucher.Amount.HasValue && voucher.Amount > 0m && orderAmount >= voucher.Amount.Value) || ((!voucher.Amount.HasValue || voucher.Amount == 0m) && orderAmount > voucher.DiscountValue))
                    {
                        result = voucher;
                        return result;
                    }
                }
                result = null;
            }
            return result;
        }
        public static System.Data.DataTable UseVoucher(decimal orderAmount)
        {
            return ShoppingProcessor.GetVoucher(orderAmount);
        }
        public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal result = 0m;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            int value = 1;
            if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddWeight.HasValue && shippingModeInfo.AddWeight.Value > 0m)
            {
                if ((totalWeight - shippingModeInfo.Weight) % shippingModeInfo.AddWeight == 0m)
                {
                    value = Convert.ToInt32(Math.Truncate((totalWeight - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value));
                }
                else
                {
                    value = Convert.ToInt32(Math.Truncate((totalWeight - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)) + 1;
                }
            }
            if (shippingModeInfo.ModeGroup == null || shippingModeInfo.ModeGroup.Count == 0)
            {
                if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
                {
                    result = value * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
                }
                else
                {
                    result = shippingModeInfo.Price;
                }
            }
            else
            {
                int? num = null;
                foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
                {
                    foreach (ShippingRegionInfo current2 in current.ModeRegions)
                    {
                        if (topRegionId == current2.RegionId)
                        {
                            num = new int?(current2.GroupId);
                            break;
                        }
                    }
                    if (num.HasValue)
                    {
                        if (totalWeight > shippingModeInfo.Weight)
                        {
                            result = value * current.AddPrice + current.Price;
                        }
                        else
                        {
                            result = current.Price;
                        }
                        break;
                    }
                }
                if (!num.HasValue)
                {
                    if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
                    {
                        result = value * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
                    }
                    else
                    {
                        result = shippingModeInfo.Price;
                    }
                }
            }
            return result;
        }
        public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
        {
            decimal result;
            if (!paymentModeInfo.IsPercent)
            {
                result = paymentModeInfo.Charge;
            }
            else
            {
                result = cartMoney * (paymentModeInfo.Charge / 100m);
            }
            return result;
        }
        public static bool CheckIsCustomsClearance(string productIds)
        {
            if (string.IsNullOrEmpty(productIds))
            {
                return false;
            }
            IList<int> list = new ProductDao().GetProductIsCustomsClearance(productIds);
            foreach (int item in list)
            {
                if (item == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public static DateTime GetUserLastOrderPCustomsClearanceDate(int userId)
        {
            return new OrderDao().GetUserLastOrderPCustomsClearanceDate(userId);
        }
        public static bool CheckIsCanMergeOrder(int newTemplateId, decimal tax, int userId)
        {
            if (newTemplateId == 0 || userId == 0 || tax > 50)
            {
                return false;
            }
            return new OrderDao().GetOrderIdIsCanMerge(newTemplateId, tax, userId).Length > 0;
        }
        public static string GenerateOrderId()
        {
            string text = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                text += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + text;
        }
        /// <summary>
        /// 拆单
        /// </summary>
        /// <param name="sourceOrder">原单</param>
        /// <returns></returns>
        public static bool UnpackOrder(OrderInfo orderinfo, ref decimal unpackedTaxTotal, ref decimal unpackedOrderTotal)
        {
            OrderDao orderDB = new OrderDao();
            string orderId = orderinfo.OrderId;
            int orerIdIndex = 1;
            List<string> listOrder = new List<string>();//已经添加了的订单
            if (orderinfo.LineItems.Count == 0)
            {
                return false;
            }
            List<LineItemInfo> tempListItemInfo = new List<LineItemInfo>();
            List<LineItemInfo> ListLineItemInfo = null;
            foreach (var item in orderinfo.LineItems)
            {
                tempListItemInfo.Add(item.Value);
            }
            //按运费模版分组
            var tempListItemInfoGroups = tempListItemInfo.GroupBy(a => a.TemplateId).ToList();
            foreach (var diffTemplateItem in tempListItemInfoGroups)
            {
                ListLineItemInfo = new List<LineItemInfo>();
                tempListItemInfo = diffTemplateItem.Select(a => a).ToList();
                //排序
                tempListItemInfo = tempListItemInfo.OrderBy(a => a.TaxRate * a.ItemAdjustedPrice).ToList();

                foreach (var item in tempListItemInfo)
                {
                    if (item.Quantity >= 1)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            LineItemInfo newItemInfo = new LineItemInfo(); //浅拷贝  
                            newItemInfo.ItemAdjustedPrice = item.ItemAdjustedPrice;
                            newItemInfo.ItemCostPrice = item.ItemCostPrice;
                            newItemInfo.ItemDescription = item.ItemDescription;
                            newItemInfo.ItemListPrice = item.ItemListPrice;
                            newItemInfo.ItemWeight = item.ItemWeight;
                            newItemInfo.ProductId = item.ProductId;
                            newItemInfo.PromoteType = item.PromoteType;
                            newItemInfo.PromotionId = item.PromotionId;
                            newItemInfo.PromotionName = item.PromotionName;
                            newItemInfo.Quantity = 1;
                            newItemInfo.ShipmentQuantity = 1;
                            newItemInfo.SKU = item.SKU;
                            newItemInfo.SKUContent = item.SKUContent;
                            newItemInfo.SkuId = item.SkuId;
                            newItemInfo.TaxRate = item.TaxRate;
                            newItemInfo.TemplateId = item.TemplateId;
                            newItemInfo.ThumbnailsUrl = item.ThumbnailsUrl;
                            newItemInfo.SupplierId = item.SupplierId;
                            newItemInfo.SupplierName = item.SupplierName;
                            ListLineItemInfo.Add(newItemInfo);
                        }
                    }
                }
                if (ListLineItemInfo.Count == 1 && tempListItemInfoGroups.Count == 1)
                {
                    return false;
                }
                //分组
                List<List<LineItemInfo>> lineItemInfoGroups = new List<List<LineItemInfo>>();

                GetLineItemInfoGroups(ref lineItemInfoGroups, ListLineItemInfo);

                if (lineItemInfoGroups.Count <= 1 && tempListItemInfoGroups.Count == 1)//情景1:所有商品税费>50
                {
                    return false;
                }
                //生成orderinfo

                //获取用户最新的订单清关时间
                DateTime pCustomsClearanceDate = orderDB.GetUserLastOrderPCustomsClearanceDate(orderinfo.UserId);
                foreach (var item in lineItemInfoGroups)
                {
                    decimal tax = 0m;
                    OrderInfo order = orderinfo;

                    order.PCustomsClearanceDate = pCustomsClearanceDate.AddDays(orerIdIndex);
                    orderinfo.LineItems.Clear();
                    Dictionary<string, LineItemInfo> dicLineItemInfo = order.LineItems;
                    foreach (LineItemInfo goods in item)
                    {
                        if (dicLineItemInfo.ContainsKey(goods.SkuId))
                        {
                            dicLineItemInfo[goods.SkuId].Quantity++;
                            dicLineItemInfo[goods.SkuId].ShipmentQuantity++;
                        }
                        else
                        {
                            dicLineItemInfo.Add(goods.SkuId, goods);
                        }
                        tax += goods.ItemAdjustedPrice * (goods.TaxRate);//计算税费
                    }
                    order.Freight = orderinfo.Freight;
                    order.AdjustedFreight = orderinfo.AdjustedFreight;
                    order.OrderType = (int)OrderType.Normal;
                    if (orderinfo.IsCustomsClearance == 1)
                    {
                        order.IsCustomsClearance = 1;
                    }
                    else
                    {
                        order.IsCustomsClearance = 0;
                    }

                    if (orerIdIndex != 1)
                    {
                        order.CouponName = null;
                        order.CouponAmount = 0;
                        order.CouponCode = null;
                        order.CouponValue = 0;

                        order.VoucherName = null;
                        order.VoucherAmount = 0;
                        order.VoucherCode = null;
                        order.VoucherValue = 0;

                        order.Freight = 0;//运费不拆分,给第一个订单
                        order.AdjustedFreight = 0;
                        order.Gifts.Clear();
                    }
                    order.OriginalTax = tax;
                    if (tax <= 50)
                    {
                        order.Tax = 0;
                    }
                    else
                    {
                        order.Tax = tax;
                    }
                    order.SourceOrderId = orderId;
                    order.OrderId = orderId + orerIdIndex;
                    order.OrderStatus = OrderStatus.UnpackOrMixed;

                    unpackedTaxTotal += order.Tax;
                    unpackedOrderTotal += order.GetTotal();
                    if (!CreateOrder(order, orerIdIndex == 1, false))
                    {
                        #region 删除已经添加的订单
                        StringBuilder orderIds = new StringBuilder();
                        int listOrderCount = listOrder.Count;
                        for (int i = 0; i < listOrderCount; i++)
                        {
                            if (i == (listOrderCount - 1))
                            {
                                orderIds.AppendFormat("'{0}'", listOrder[i]);
                            }
                            else
                            {
                                orderIds.AppendFormat("'{0}',", listOrder[i]);
                            }
                        }
                        if (orderIds.ToString() != "")
                        {
                            orderDB.DeleteOrders(orderIds.ToString());
                        }
                        #endregion

                        return false;
                    }
                    orerIdIndex++;
                    listOrder.Add(order.OrderId);
                }
            }
            return true;
        }

        /// <summary>
        /// 拆单
        /// </summary>
        /// <param name="orderinfo">订单信息</param>
        /// <param name="unpackedTaxTotal">拆单的总税费</param>
        /// <param name="unpackedOrderTotal">拆单的订单总额</param>
        /// <param name="masterOrderId">主订单Id</param>
        /// <param name="unpackedFreight">拆单的运费</param>
        /// <returns></returns>
        public static bool UnpackOrder(OrderInfo orderinfo, ref decimal unpackedTaxTotal, ref decimal unpackedOrderTotal, string masterOrderId, ref decimal unpackedFreight)
        {
            OrderDao orderDB = new OrderDao();
            string orderId = orderinfo.OrderId;
            int orerIdIndex = 1;

            if (orderinfo.LineItems.Count == 0)
            {
                return false;
            }
            List<LineItemInfo> tempListItemInfo = new List<LineItemInfo>();
            List<LineItemInfo> ListLineItemInfo = null;

            orderinfo.LineItems.ToList().ForEach(t =>
            {
                tempListItemInfo.Add(t.Value);
            });

            //按运费模版分组
            var tempListItemInfoGroups = tempListItemInfo.GroupBy(a => a.TemplateId).ToList();

            foreach (var diffTemplateItem in tempListItemInfoGroups)
            {
                ListLineItemInfo = new List<LineItemInfo>();
                //加上条件：去掉单品为1000元的 .Where(a=> a.ItemAdjustedPrice <= 1000) 大于1000的直接生成一个订单
                tempListItemInfo = diffTemplateItem.Select(a => a).ToList();

                //根据订单的税费排序，必须，后面寻找组合用到
                tempListItemInfo = tempListItemInfo.OrderBy(a => a.TaxRate * a.ItemAdjustedPrice).ToList();

                foreach (var item in tempListItemInfo)
                {
                    if (item.Quantity >= 1)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            LineItemInfo newItemInfo = new LineItemInfo(); //浅拷贝  
                            newItemInfo.ItemAdjustedPrice = item.ItemAdjustedPrice;
                            newItemInfo.ItemCostPrice = item.ItemCostPrice;
                            newItemInfo.ItemDescription = item.ItemDescription;
                            newItemInfo.ItemListPrice = item.ItemListPrice;
                            newItemInfo.ItemWeight = item.ItemWeight;
                            newItemInfo.ProductId = item.ProductId;
                            newItemInfo.PromoteType = item.PromoteType;
                            newItemInfo.PromotionId = item.PromotionId;
                            newItemInfo.PromotionName = item.PromotionName;
                            newItemInfo.Quantity = 1;
                            newItemInfo.ShipmentQuantity = 1;
                            newItemInfo.SKU = item.SKU;
                            newItemInfo.SKUContent = item.SKUContent;
                            newItemInfo.SkuId = item.SkuId;
                            newItemInfo.TaxRate = item.TaxRate;
                            newItemInfo.TemplateId = item.TemplateId;
                            newItemInfo.ThumbnailsUrl = item.ThumbnailsUrl;
                            newItemInfo.SupplierId = item.SupplierId;
                            newItemInfo.SupplierName = item.SupplierName;
                            newItemInfo.PromotionPrice = item.PromotionPrice;
                            ListLineItemInfo.Add(newItemInfo);
                        }
                    }
                }

                if (ListLineItemInfo.Count == 1
                    && tempListItemInfoGroups.Count == 1
                    && string.IsNullOrEmpty(masterOrderId))
                {
                    return false;
                }

                List<List<LineItemInfo>> lineItemInfoGroups = new List<List<LineItemInfo>>();//分组


                //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                //sw.Start();                

                GetLineItemInfoGroups(ref lineItemInfoGroups, ListLineItemInfo);

                //sw.Stop();
                //ErrorLog.Write("执行GetLineItemInfoGroups 的时间:" + sw.ElapsedMilliseconds.ToString());


                if (lineItemInfoGroups.Count <= 1
                    && tempListItemInfoGroups.Count == 1
                    && string.IsNullOrEmpty(masterOrderId))//情景1:所有商品税费>50
                {
                    return false;
                }

                //已经添加了的订单
                List<string> listOrder = new List<string>();

                DateTime pCustomsClearanceDate = orderDB.GetUserLastOrderPCustomsClearanceDate(orderinfo.UserId); //获取用户最新的订单清关时间

                foreach (var item in lineItemInfoGroups)
                {
                    decimal tax = 0m;

                    //订单克隆，为了处理腾付通支付
                    OrderInfo order = (OrderInfo)orderinfo.Clone();

                    order.PCustomsClearanceDate = pCustomsClearanceDate.AddDays(orerIdIndex);
                    orderinfo.LineItems.Clear();
                    Dictionary<string, LineItemInfo> dicLineItemInfo = order.LineItems;
                    foreach (LineItemInfo goods in item)
                    {
                        if (dicLineItemInfo.ContainsKey(goods.SkuId))
                        {
                            dicLineItemInfo[goods.SkuId].Quantity++;
                            dicLineItemInfo[goods.SkuId].ShipmentQuantity++;
                        }
                        else
                        {
                            dicLineItemInfo.Add(goods.SkuId, goods);
                        }
                        tax += goods.ItemAdjustedPrice * (goods.TaxRate);//计算税费
                    }
                    order.Freight = orderinfo.Freight;
                    order.AdjustedFreight = orderinfo.AdjustedFreight;
                    order.OrderType = (int)OrderType.Normal;
                    if (orderinfo.IsCustomsClearance == 1)
                    {
                        order.IsCustomsClearance = 1;
                    }
                    else
                    {
                        order.IsCustomsClearance = 0;
                    }

                    if (orerIdIndex != 1)
                    {
                        order.ReducedPromotionId = 0;
                        order.ReducedPromotionAmount = 0;
                        order.ReducedPromotionName = "";
                        order.IsReduced = false;
                        order.CouponName = null;
                        order.CouponAmount = 0;
                        order.CouponCode = null;
                        order.CouponValue = 0;

                        order.VoucherName = null;
                        order.VoucherAmount = 0;
                        order.VoucherCode = null;
                        order.VoucherValue = 0;

                        order.Freight = 0;//运费不拆分,给第一个订单
                        order.AdjustedFreight = 0;
                        order.Gifts.Clear();
                    }
                    order.OriginalTax = tax;
                    if (tax <= 50)
                    {
                        order.Tax = 0;
                    }
                    else
                    {
                        order.Tax = tax;
                    }
                    order.SourceOrderId = masterOrderId;
                    order.OrderId = orderId + orerIdIndex;
                    order.OrderStatus = OrderStatus.UnpackOrMixed;

                    unpackedFreight += order.Freight;

                    unpackedTaxTotal += order.Tax;
                    unpackedOrderTotal += order.GetTotal();
                    //System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
                    //sw1.Start();
                    if (!CreateOrder(order, orerIdIndex == 1, false))
                    {
                        #region 删除已经添加的订单
                        StringBuilder orderIds = new StringBuilder();
                        int listOrderCount = listOrder.Count;
                        for (int i = 0; i < listOrderCount; i++)
                        {
                            if (i == (listOrderCount - 1))
                            {
                                orderIds.AppendFormat("'{0}'", listOrder[i]);
                            }
                            else
                            {
                                orderIds.AppendFormat("'{0}',", listOrder[i]);
                            }
                        }
                        if (orderIds.ToString() != "")
                        {
                            orderDB.DeleteOrders(orderIds.ToString());
                        }
                        #endregion

                        return false;
                    }
                    //sw1.Stop();
                    //ErrorLog.Write("执行循环CreateOrder(order, orerIdIndex == 1, false) 的时间:" + sw1.ElapsedMilliseconds.ToString());
                    orerIdIndex++;
                    listOrder.Add(order.OrderId);
                }
            }
            return true;
        }

        public static bool UnpackOrderBySupplier(OrderInfo masterOrder, ref decimal unpackedTaxTotal, ref decimal unpackedOrderTotalMoney, ref decimal unpackedFreight)//按供应商拆单
        {
            //商品件数
            if (masterOrder.LineItems.Count < 1)
            {
                return false;
            }

            //商品的购买总数量
            int goodsCount = masterOrder.LineItems.Values.Sum(p => p.Quantity);
            if (goodsCount <= 1)
            {
                return false;
            }

            ////1件商品，只有一个供应商，直接按运费模板分组
            //if (masterOrder.LineItems.Count == 1)
            //{
            //    return UnpackOrder(masterOrder, ref unpackedTaxTotal, ref unpackedOrderTotalMoney, masterOrder.OrderId, ref unpackedFreight);
            //}

            //新的订单集合
            List<OrderInfo> listOrderInfo = new List<OrderInfo>();
            OrderInfo orderInfo = null;
            int index = 0;

            //根据供应商分组
            var tempListItemInfoGroups = masterOrder.LineItems.Values.GroupBy(a => a.SupplierId);
            //只有一个供应商，直接按运费模板分组
            if (tempListItemInfoGroups.Count() == 1)
            {
                return UnpackOrder(masterOrder, ref unpackedTaxTotal, ref unpackedOrderTotalMoney, masterOrder.OrderId, ref unpackedFreight);
            }

            //获取收货地址信息
            ShippingAddressInfo memberDefaultShippingAddressInfo = GetShippingAddress(masterOrder.ShippingId);

            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();

            foreach (var diffSupplierIdItem in tempListItemInfoGroups)
            {
                //to to 过滤掉单品价格大于1000的商品 Where(a=>a.ItemAdjustedPrice <= 1000),大于1000的直接生成一个订单
                List<LineItemInfo> diffSupplierIdLineItemInfo = diffSupplierIdItem.ToList();
                //foreach (var item in diffSupplierIdItem)
                //{
                //    var itemmmm = item;
                //}
                int tempsupplierId = diffSupplierIdItem.Key;
                #region 构造订单信息
                index++;
                orderInfo = new OrderInfo();
                orderInfo.OrderId = masterOrder.OrderId + index;
                orderInfo.OrderDate = DateTime.Now;
                orderInfo.ReferralUserId = masterOrder.ReferralUserId;
                orderInfo.UserId = masterOrder.UserId;
                orderInfo.Username = masterOrder.Username;
                orderInfo.Wangwang = masterOrder.Wangwang;
                orderInfo.RealName = masterOrder.RealName;
                orderInfo.EmailAddress = masterOrder.EmailAddress;
                orderInfo.Remark = masterOrder.Remark;
                orderInfo.AdjustedDiscount = masterOrder.AdjustedDiscount;
                orderInfo.OrderStatus = masterOrder.OrderStatus;
                orderInfo.ShippingRegion = masterOrder.ShippingRegion;
                orderInfo.Address = masterOrder.Address;
                orderInfo.ZipCode = masterOrder.ZipCode;
                orderInfo.ShipTo = masterOrder.ShipTo;
                orderInfo.TelPhone = masterOrder.TelPhone;
                orderInfo.CellPhone = masterOrder.CellPhone;
                orderInfo.ShipToDate = masterOrder.ShipToDate;
                orderInfo.ShippingModeId = masterOrder.ShippingModeId;
                orderInfo.ModeName = masterOrder.ModeName;
                orderInfo.RegionId = masterOrder.RegionId;
                orderInfo.Freight = masterOrder.Freight;
                orderInfo.AdjustedFreight = masterOrder.AdjustedFreight;
                orderInfo.ShipOrderNumber = masterOrder.ShipOrderNumber;
                // orderInfo.Weight
                orderInfo.ExpressCompanyName = masterOrder.ExpressCompanyName;
                orderInfo.ExpressCompanyAbb = masterOrder.ExpressCompanyAbb;
                orderInfo.PaymentTypeId = masterOrder.PaymentTypeId;
                orderInfo.PaymentType = masterOrder.PaymentType;
                orderInfo.PayCharge = masterOrder.PayCharge;
                orderInfo.RefundStatus = masterOrder.RefundStatus;
                orderInfo.Gateway = masterOrder.Gateway;
                //orderInfo.OrderTotal
                orderInfo.Points = masterOrder.Points;
                // orderInfo.OrderCostPrice
                //orderInfo.OrderProfit
                //orderInfo.Amount
                orderInfo.ReducedPromotionId = masterOrder.ReducedPromotionId;//促销
                orderInfo.ReducedPromotionName = masterOrder.ReducedPromotionName;
                orderInfo.ReducedPromotionAmount = masterOrder.ReducedPromotionAmount;
                orderInfo.IsReduced = masterOrder.IsReduced;
                orderInfo.SentTimesPointPromotionId = masterOrder.SentTimesPointPromotionId;
                orderInfo.SentTimesPointPromotionName = masterOrder.SentTimesPointPromotionName;
                orderInfo.TimesPoint = masterOrder.TimesPoint;
                orderInfo.IsSendTimesPoint = masterOrder.IsSendTimesPoint;
                orderInfo.FreightFreePromotionId = masterOrder.FreightFreePromotionId;
                orderInfo.FreightFreePromotionName = masterOrder.FreightFreePromotionName;
                orderInfo.IsFreightFree = masterOrder.IsFreightFree;
                orderInfo.CouponName = masterOrder.CouponName;
                orderInfo.CouponCode = masterOrder.CouponCode;//优惠券
                orderInfo.CouponAmount = masterOrder.CouponAmount;
                orderInfo.CouponValue = masterOrder.CouponValue;

                orderInfo.VoucherName = masterOrder.VoucherName;
                orderInfo.VoucherCode = masterOrder.VoucherCode;//现金券
                orderInfo.VoucherAmount = masterOrder.VoucherAmount;
                orderInfo.VoucherValue = masterOrder.VoucherValue;

                orderInfo.IsCustomsClearance = masterOrder.IsCustomsClearance;
                orderInfo.PCustomsClearanceDate = masterOrder.PCustomsClearanceDate;
                orderInfo.OrderType = masterOrder.OrderType;
                orderInfo.OriginalTax = masterOrder.OriginalTax;
                orderInfo.IdentityCard = masterOrder.IdentityCard;
                orderInfo.GatewayOrderId = masterOrder.GatewayOrderId;
                orderInfo.SiteId = masterOrder.SiteId;

                orderInfo.TaobaoOrderId = masterOrder.TaobaoOrderId;
                orderInfo.GroupBuyId = masterOrder.GroupBuyId;
                orderInfo.NeedPrice = masterOrder.NeedPrice;
                orderInfo.CountDownBuyId = masterOrder.CountDownBuyId;
                orderInfo.BundlingID = masterOrder.BundlingID;
                orderInfo.BundlingPrice = masterOrder.BundlingPrice;
                orderInfo.OrderSource = masterOrder.OrderSource;
                orderInfo.InvoiceTitle = masterOrder.InvoiceTitle;
                orderInfo.SourceOrderId = masterOrder.SourceOrderId;
                orderInfo.SupplierId = tempsupplierId;
                #endregion

                //订单明细
                diffSupplierIdLineItemInfo.ForEach(a => { orderInfo.LineItems.Add(a.SkuId, a); });
                //礼品忽略
                //优惠券记录表更新 忽略
                //重算运费和税金                
                decimal freight = GetFreight(diffSupplierIdLineItemInfo, memberDefaultShippingAddressInfo);//重算运费
                decimal tax = (diffSupplierIdItem != null && diffSupplierIdItem.Count() > 0) ? diffSupplierIdItem.Sum(p => p.ItemAdjustedPrice * p.TaxRate * p.Quantity) : 0;

                unpackedFreight += freight;

                orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                orderInfo.Tax = tax <= 50 ? 0 : tax;
                orderInfo.OriginalTax = tax;
                listOrderInfo.Add(orderInfo);
            }

            //sw.Stop();
            //ErrorLog.Write("执行循环tempListItemInfoGroups 的时间:" + sw.ElapsedMilliseconds.ToString());

            if (listOrderInfo.Count > 0)
            {
                int orderCount = listOrderInfo.Count;
                decimal partReducedPromotionAmount = 0;
                decimal partCouponValue = 0;
                decimal partVoucherValue = 0;    //Added by Paul @20150729
                for (int i = 0; i < orderCount; i++)
                {
                    decimal tmpReducedPromotionAmount = Math.Round(masterOrder.ReducedPromotionAmount / orderCount, 2);
                    decimal tmpCouponValue = Math.Round(masterOrder.CouponValue / orderCount, 2);
                    decimal tmpVoucherValue = Math.Round(masterOrder.VoucherValue / orderCount, 2);
                    partCouponValue += tmpCouponValue;
                    partVoucherValue += tmpVoucherValue;
                    partReducedPromotionAmount += tmpReducedPromotionAmount;
                    listOrderInfo[i].ReducedPromotionAmount = partReducedPromotionAmount;//促销活动减免金额
                    listOrderInfo[i].CouponValue = tmpCouponValue;//优惠券减免金额
                    listOrderInfo[i].VoucherValue = tmpVoucherValue;//现金券减免金额
                }
                if (partReducedPromotionAmount != masterOrder.ReducedPromotionAmount)
                {
                    listOrderInfo[orderCount - 1].ReducedPromotionAmount += masterOrder.ReducedPromotionAmount - partReducedPromotionAmount;
                }
                if (partCouponValue != masterOrder.CouponValue)
                {
                    listOrderInfo[orderCount - 1].CouponValue += (masterOrder.CouponValue - partCouponValue);
                }
                if (partVoucherValue != masterOrder.VoucherValue)
                {
                    listOrderInfo[orderCount - 1].VoucherValue += (masterOrder.VoucherValue - partVoucherValue);
                }

                //System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
                //sw1.Start();
                foreach (OrderInfo orderItem in listOrderInfo)
                {
                    decimal unpackedTax = 0m;
                    decimal unpackedOrderTotal = 0m;
                    decimal unpackFreight = 0m;
                    UnpackOrder(orderItem, ref unpackedTaxTotal, ref unpackedOrderTotal, masterOrder.OrderId, ref unpackFreight);
                    unpackFreight = 0m;
                    unpackedTaxTotal += unpackedTax;
                    unpackedOrderTotalMoney += unpackedOrderTotal;
                }
                //sw1.Stop();
                //ErrorLog.Write("执行循环listOrderInfo 的时间:" + sw1.ElapsedMilliseconds.ToString());

            }
            return true;
        }
        /// <summary>
        /// 创建子订单（不根据供货商Id，也不根据运费模版拆单）
        /// </summary>
        /// <param name="masterOrder"></param>
        /// <param name="unpackedTaxTotal"></param>
        /// <param name="unpackedOrderTotalMoney"></param>
        /// <param name="unpackedFreight"></param>
        /// <returns></returns>
        public static bool CreateChildOrders(OrderInfo masterOrder, ref decimal unpackedTaxTotal, ref decimal unpackedOrderTotalMoney, ref decimal unpackedFreight)
        {
            List<LineItemInfo> items = masterOrder.LineItems.Values.ToList();
            List<LineItemInfo> tempItems = new List<LineItemInfo>();
            foreach (var item in items)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    LineItemInfo newItemInfo = item.CopySingleItem();
                    tempItems.Add(newItemInfo);
                }
            }
            List<List<LineItemInfo>> unpackedItems = GetChildCollections(tempItems);
            List<OrderInfo> childOrders = new List<OrderInfo>();
            int index = 1;
            DateTime customsClearanceDate = GetUserLastOrderPCustomsClearanceDate(masterOrder.UserId); //获取用户最新的订单清关时间
            ShippingAddressInfo memberDefaultShippingAddressInfo = GetShippingAddress(masterOrder.ShippingId);
            decimal productTotalAmount = masterOrder.LineItems.Values.Sum(p => p.Quantity * p.ItemAdjustedPrice);
            foreach (var item in unpackedItems)
            {
                OrderInfo orderInfo = masterOrder.ToChildOrder();
                orderInfo.OrderId = masterOrder.OrderId + index;
                orderInfo.PCustomsClearanceDate = customsClearanceDate.AddDays(index);
                foreach (var item0 in item)
                {
                    if (orderInfo.LineItems.ContainsKey(item0.SkuId))
                    {
                        orderInfo.LineItems[item0.SkuId].Quantity += item0.Quantity;
                        orderInfo.LineItems[item0.SkuId].ShipmentQuantity += item0.Quantity;
                        continue;
                    }
                    orderInfo.LineItems.Add(item0.SkuId, item0);
                }
                //orderInfo.OriginalTax = orderInfo.LineItems.Values.Sum(p => p.ItemAdjustedPrice * p.TaxRate * p.Quantity);//关税

                orderInfo.OriginalTax = orderInfo.LineItems.Values.Sum(p => p.Tax * p.Quantity);
                orderInfo.Tax = orderInfo.OriginalTax > 50 ? orderInfo.OriginalTax : 0;
                unpackedTaxTotal += orderInfo.Tax;

                //修改运费计算，如果拆单前是免邮的，那么拆单后订单也是免邮的
                if (masterOrder.IsFreightFree)
                {
                    decimal freight = 0m;
                    orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                    unpackedFreight += freight;
                }
                else
                {
                    decimal freight = GetFreight(orderInfo.LineItems.Values.ToList(), memberDefaultShippingAddressInfo);//运费
                    orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                    unpackedFreight += freight;
                }
                //decimal freight = GetFreight(orderInfo.LineItems.Values.ToList(), memberDefaultShippingAddressInfo);//运费
                //orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                //unpackedFreight += freight;

                decimal productAmount = orderInfo.LineItems.Values.Sum(p => p.Quantity * p.ItemAdjustedPrice);
                if (masterOrder.VoucherValue > 0)
                {
                    orderInfo.VoucherValue = Math.Round(masterOrder.VoucherValue * productAmount / productTotalAmount, 2);
                }
                if (masterOrder.CouponValue > 0)
                {
                    orderInfo.CouponValue = Math.Round(masterOrder.CouponValue * productAmount / productTotalAmount, 2);
                }
                if (masterOrder.ReducedPromotionAmount > 0)
                {
                    orderInfo.ReducedPromotionAmount = Math.Round(masterOrder.ReducedPromotionAmount * productAmount / productTotalAmount, 2);
                    orderInfo.IsReduced = true;
                }

                orderInfo.OrderStatus = OrderStatus.UnpackOrMixed;
                orderInfo.OrderType = (int)OrderType.Normal;
                orderInfo.PresentProducts = masterOrder.PresentProducts;
                childOrders.Add(orderInfo);
                index++;
            }
            if (childOrders.Count > 1)
            {
                OrderInfo lastOrder = childOrders[childOrders.Count - 1];
                lastOrder.VoucherValue = masterOrder.VoucherValue - childOrders.Take(childOrders.Count - 1).Sum(p => p.VoucherValue);
                lastOrder.CouponValue = masterOrder.CouponValue - childOrders.Take(childOrders.Count - 1).Sum(p => p.CouponValue);
                lastOrder.ReducedPromotionAmount = masterOrder.ReducedPromotionAmount - childOrders.Take(childOrders.Count - 1).Sum(p => p.ReducedPromotionAmount);
            }

            foreach (var order in childOrders)
            {
                unpackedOrderTotalMoney += order.GetTotal();
                CreateOrder(order, true, false);
            }
            return true;
        }
        public static bool mergeOrder(OrderInfo orderinfo)
        {
            OrderDao orderDB = new OrderDao();
            int templateId = 0;
            int index = 0;
            foreach (var lineItem in orderinfo.LineItems)
            {
                if (index == 0)
                {
                    templateId = lineItem.Value.TemplateId;
                }
                else
                {
                    if (templateId != lineItem.Value.TemplateId)
                    {
                        return false;
                    }
                }
                index++;
            }

            string pOrderId = orderDB.GetOrderIdIsCanMerge(templateId, orderinfo.Tax, orderinfo.UserId);
            if (string.IsNullOrEmpty(pOrderId))
            {
                return false;
            }
            OrderInfo pOrderInfo = orderDB.GetOrderInfo(pOrderId);

            OrderInfo newOrderInfo = orderinfo;
            foreach (var item in pOrderInfo.LineItems)
            {
                if (newOrderInfo.LineItems.ContainsKey(item.Key))//同样skuid合并可能重量、运费、税率变化？
                {
                    newOrderInfo.LineItems[item.Key].Quantity += item.Value.Quantity;
                }
                else
                {
                    newOrderInfo.LineItems.Add(item.Key, item.Value);
                }
            }
            newOrderInfo.SourceOrderId = newOrderInfo.OrderId + "," + pOrderInfo.OrderId;
            string orderId = orderinfo.OrderId;
            newOrderInfo.OrderId = GenerateOrderId();
            newOrderInfo.OrderType = (int)OrderType.Normal;
            newOrderInfo.Tax += pOrderInfo.Tax;
            newOrderInfo.AdjustedFreight += pOrderInfo.AdjustedFreight;
            newOrderInfo.Freight += pOrderInfo.AdjustedFreight;
            newOrderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            newOrderInfo.OrderDate = DateTime.Now;
            newOrderInfo.PayDate = DateTime.Now;
            if (CreateOrder(newOrderInfo, true, false))
            {
                orderDB.UpdateChildOrderStatusForMerge(orderId, pOrderInfo.OrderId);
                return true;
            }
            return false;
        }    // 合单

        /// <summary>
        /// 税<50 金额<1000组合；
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="newModel"></param>
        public static void GetLineItemInfoByFilterTaxLltFive(List<LineItemInfo> modelList, ref List<LineItemInfo> newModel)
        {
            var tempTax = 0.0m;//临时税收值
            var tempAmount = 0.0m;//临时商品金额；
            List<LineItemInfo> goodNotInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 不符合的记录；
            List<LineItemInfo> goodInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 符合的记录；
            foreach (var item in modelList)
            {
                tempTax += item.ItemAdjustedPrice * item.TaxRate * item.Quantity;
                tempAmount += item.ItemAdjustedPrice * item.Quantity;
                if (tempAmount <= 1000 && tempTax <= 50)
                {
                    goodInLineItemInfo.Add(item);
                }
                else
                {
                    goodNotInLineItemInfo.Add(item);
                }
            }
            if (goodNotInLineItemInfo != null && goodNotInLineItemInfo.Count() > 0)
            {
                GetLineItemInfoByFilterTaxLltFive(goodNotInLineItemInfo, ref newModel);
            }
            newModel.AddRange(goodInLineItemInfo);

        }

        /// <summary>
        /// 税>50 金额<=1000组合；
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="newModel"></param>
        public static void GetLineItemInfoByFilterGltFiveTaxAndAmountLltQ(List<LineItemInfo> modelList, ref List<LineItemInfo> newModel)
        {

            var tempAmount = 0.0m;//临时商品金额；
            List<LineItemInfo> goodNotInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 不符合的记录；
            List<LineItemInfo> goodInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 符合的记录；
            foreach (var item in modelList)
            {
                tempAmount += item.ItemAdjustedPrice * item.Quantity;
                if (tempAmount <= 1000)
                {
                    goodInLineItemInfo.Add(item);
                }
                else
                {
                    goodNotInLineItemInfo.Add(item);
                }
            }
            if (goodNotInLineItemInfo != null && goodNotInLineItemInfo.Count() > 0)
            {
                GetLineItemInfoByFilterGltFiveTaxAndAmountLltQ(goodNotInLineItemInfo, ref newModel);
            }
            newModel.AddRange(goodInLineItemInfo);

        }

        #region Old

        ///// <summary>
        /////  找到满足条件的订单大集合
        ///// </summary>
        ///// <param name="lineItemInfoGroups">订单大集合</param>
        ///// <param name="ListLineItemInfo">订单集合</param>
        //public static void GetLineItemInfoGroups(ref List<List<LineItemInfo>> lineItemInfoGroups, List<LineItemInfo> ListLineItemInfo)    //拆分orderItem
        //{
        //    if (ListLineItemInfo == null || ListLineItemInfo.Count < 1)
        //    {
        //        return;
        //    }
        //    if (ListLineItemInfo.Count == 1)
        //    {
        //        lineItemInfoGroups.Add(ListLineItemInfo);
        //        return;
        //    }

        //    #region 2015-09-08之前 lin
        //    //查找出税费<50&&商品总额<=1000的 商品数据；
        //    //List<LineItemInfo> goodLineItemInfo = new List<LineItemInfo>();
        //    //List<LineItemInfo> goodNotInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 不符合的记录；
        //    //List<LineItemInfo> goodInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 符合的记录；
        //    //goodLineItemInfo = ListLineItemInfo.Where(c => c.TaxRate * c.ItemAdjustedPrice < 50 && c.ItemAdjustedPrice * c.Quantity <= 1000).Select(c => c).ToList();

        //    //if (goodLineItemInfo != null && goodLineItemInfo.Count() > 0)
        //    //{
        //    //    //全部为理想状态，所有商品加起来 满足理想化条件，直接合为一单；
        //    //    if (goodLineItemInfo.Sum(c => c.TaxRate * c.ItemAdjustedPrice) <= 50 && goodLineItemInfo.Sum(c => c.ItemAdjustedPrice * c.Quantity) <= 1000)
        //    //    {
        //    //        lineItemInfoGroups.Add(goodLineItemInfo);
        //    //        return;
        //    //    }
        //    //    else
        //    //    {
        //    //        //按商品金额最大的优先组合
        //    //        goodLineItemInfo = goodLineItemInfo.OrderByDescending(c => c.ItemAdjustedPrice).ToList();
        //    //        GetLineItemInfoByFilterTaxLltFive(goodLineItemInfo, ref goodInLineItemInfo);
        //    //        //将符合条件 金额<=100 &&税值<=50的 返回；
        //    //        lineItemInfoGroups.Add(goodInLineItemInfo);
        //    //        goodLineItemInfo = new List<LineItemInfo>();

        //    //    }
        //    //}
        //    //// 查找商品金额超过1000的；
        //    //goodLineItemInfo = ListLineItemInfo.Where(c => c.ItemAdjustedPrice * c.Quantity > 1000).Select(c => c).ToList();
        //    //if (goodLineItemInfo != null && goodLineItemInfo.Count() > 0)
        //    //{
        //    //    lineItemInfoGroups.Add(goodLineItemInfo);
        //    //}
        //    //// 查找商品金额<1000的 税率>50
        //    //goodLineItemInfo = ListLineItemInfo.Where(c => c.TaxRate * c.ItemAdjustedPrice > 50 && c.ItemAdjustedPrice * c.Quantity <= 1000).Select(c => c).ToList();
        //    //if (goodLineItemInfo != null && goodLineItemInfo.Count() > 0)
        //    //{
        //    //    //按商品金额最大的优先组合
        //    //    goodLineItemInfo = goodLineItemInfo.OrderByDescending(c => c.ItemAdjustedPrice).ToList();
        //    //    GetLineItemInfoByFilterGltFiveTaxAndAmountLltQ(goodLineItemInfo, ref goodInLineItemInfo);
        //    //    //将符合条件 金额<=1000 &&税值>50的 返回；
        //    //    lineItemInfoGroups.Add(goodInLineItemInfo);
        //    //    goodLineItemInfo = new List<LineItemInfo>();
        //    //}
        //    #endregion

        //    LineItemInfo tempLineItemInfo = null;
        //    List<structTax> structTax = new List<structTax>();
        //    tempLineItemInfo = ListLineItemInfo[0];
        //    ListLineItemInfo.RemoveAt(0);

        //    List<List<LineItemInfo>> ListLineItemInfocollections = GetChildCollections(ListLineItemInfo);//穷举

        //    int k = 0;
        //    ListLineItemInfo.Add(tempLineItemInfo);
        //    foreach (List<LineItemInfo> item in ListLineItemInfocollections)
        //    {
        //        item.Add(tempLineItemInfo);
        //        decimal tmpTax = 0m;
        //        decimal productTotal = 0m;
        //        item.ForEach(a => { tmpTax += a.TaxRate * a.ItemAdjustedPrice; productTotal += a.Quantity * a.ItemAdjustedPrice; });
        //        structTax.Add(new structTax(k, 50 - tmpTax, item.Count, productTotal));
        //        k++;
        //    }
        //    //分组的id,有10000个组合，每一个组合做了个编号
        //    var groupId = 0;
        //    List<structTax> conformTax = (from s
        //                                                    in structTax
        //                                  where s.tax >= 0 && s.tax < 50 && s.productTotal <= 1000
        //                                  select s).OrderBy(a => a.tax).ThenByDescending(a => a.itemCount).ToList();

        //    if (conformTax != null && conformTax.Count > 0)//理想组合，税费<50&&商品总额<=1000
        //    {
        //        groupId = conformTax.FirstOrDefault().index;
        //        lineItemInfoGroups.Add(ListLineItemInfocollections[groupId]);
        //        ListLineItemInfocollections[groupId].ForEach(a => ListLineItemInfo.Remove(a));
        //    }
        //    else//商品税费大于50||商品税率为0||商品总额>1000
        //    {
        //        if (ListLineItemInfo.Count > 0)
        //        {
        //            List<structTax> badTax = (from s in structTax where s.productTotal <= 1000 select s).OrderByDescending(a => a.itemCount).ToList();
        //            if (badTax != null && badTax.Count > 0)//税率>50 &&商品总额<=1000
        //            {
        //                groupId = badTax.FirstOrDefault().index;
        //                lineItemInfoGroups.Add(ListLineItemInfocollections[groupId]);
        //                //找了合适的组合了，就把这个组合内的所有商品从大集合中去掉
        //                ListLineItemInfocollections[groupId].ForEach(a => ListLineItemInfo.Remove(a));
        //            }
        //            else//税率>50 &&商品总额>1000
        //            {
        //                foreach (LineItemInfo lastItem in ListLineItemInfo)
        //                {
        //                    List<LineItemInfo> lastGroupLineItemInfo = new List<LineItemInfo>() { lastItem };
        //                    lineItemInfoGroups.Add(lastGroupLineItemInfo);
        //                }
        //                ListLineItemInfo = null;
        //            }
        //        }
        //    }
        //    GetLineItemInfoGroups(ref lineItemInfoGroups, ListLineItemInfo);
        //}

        #endregion

        /// <summary>
        ///  找到满足条件的订单大集合
        /// </summary>
        /// <param name="lineItemInfoGroups">订单大集合</param>
        /// <param name="ListLineItemInfo">订单集合</param>
        public static void GetLineItemInfoGroups(ref List<List<LineItemInfo>> lineItemInfoGroups, List<LineItemInfo> ListLineItemInfo)    //拆分orderItem
        {
            if (ListLineItemInfo == null || ListLineItemInfo.Count < 1)
            {
                return;
            }
            if (ListLineItemInfo.Count == 1)
            {
                lineItemInfoGroups.Add(ListLineItemInfo);
                return;
            }

            #region 2015-09-08之前 lin
            //查找出税费<50&&商品总额<=1000的 商品数据；
            //List<LineItemInfo> goodLineItemInfo = new List<LineItemInfo>();
            //List<LineItemInfo> goodNotInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 不符合的记录；
            //List<LineItemInfo> goodInLineItemInfo = new List<LineItemInfo>(); // 出税费<50&&商品总额<=1000中组合时 符合的记录；
            //goodLineItemInfo = ListLineItemInfo.Where(c => c.TaxRate * c.ItemAdjustedPrice < 50 && c.ItemAdjustedPrice * c.Quantity <= 1000).Select(c => c).ToList();

            //if (goodLineItemInfo != null && goodLineItemInfo.Count() > 0)
            //{
            //    //全部为理想状态，所有商品加起来 满足理想化条件，直接合为一单；
            //    if (goodLineItemInfo.Sum(c => c.TaxRate * c.ItemAdjustedPrice) <= 50 && goodLineItemInfo.Sum(c => c.ItemAdjustedPrice * c.Quantity) <= 1000)
            //    {
            //        lineItemInfoGroups.Add(goodLineItemInfo);
            //        return;
            //    }
            //    else
            //    {
            //        //按商品金额最大的优先组合
            //        goodLineItemInfo = goodLineItemInfo.OrderByDescending(c => c.ItemAdjustedPrice).ToList();
            //        GetLineItemInfoByFilterTaxLltFive(goodLineItemInfo, ref goodInLineItemInfo);
            //        //将符合条件 金额<=100 &&税值<=50的 返回；
            //        lineItemInfoGroups.Add(goodInLineItemInfo);
            //        goodLineItemInfo = new List<LineItemInfo>();

            //    }
            //}
            //// 查找商品金额超过1000的；
            //goodLineItemInfo = ListLineItemInfo.Where(c => c.ItemAdjustedPrice * c.Quantity > 1000).Select(c => c).ToList();
            //if (goodLineItemInfo != null && goodLineItemInfo.Count() > 0)
            //{
            //    lineItemInfoGroups.Add(goodLineItemInfo);
            //}
            //// 查找商品金额<1000的 税率>50
            //goodLineItemInfo = ListLineItemInfo.Where(c => c.TaxRate * c.ItemAdjustedPrice > 50 && c.ItemAdjustedPrice * c.Quantity <= 1000).Select(c => c).ToList();
            //if (goodLineItemInfo != null && goodLineItemInfo.Count() > 0)
            //{
            //    //按商品金额最大的优先组合
            //    goodLineItemInfo = goodLineItemInfo.OrderByDescending(c => c.ItemAdjustedPrice).ToList();
            //    GetLineItemInfoByFilterGltFiveTaxAndAmountLltQ(goodLineItemInfo, ref goodInLineItemInfo);
            //    //将符合条件 金额<=1000 &&税值>50的 返回；
            //    lineItemInfoGroups.Add(goodInLineItemInfo);
            //    goodLineItemInfo = new List<LineItemInfo>();
            //}
            #endregion

            //LineItemInfo tempLineItemInfo = null;
            //List<structTax> structTax = new List<structTax>();
            //tempLineItemInfo = ListLineItemInfo[0];
            //ListLineItemInfo.RemoveAt(0);

            List<List<LineItemInfo>> ListLineItemInfocollections = GetChildCollections(ListLineItemInfo);//穷举
            lineItemInfoGroups.AddRange(ListLineItemInfocollections);
            //int k = 0;
            //ListLineItemInfo.Add(tempLineItemInfo);
            //foreach (List<LineItemInfo> item in ListLineItemInfocollections)
            //{
            //    item.Add(tempLineItemInfo);
            //    decimal tmpTax = 0m;
            //    decimal productTotal = 0m;
            //    item.ForEach(a => { tmpTax += a.TaxRate * a.ItemAdjustedPrice; productTotal += a.Quantity * a.ItemAdjustedPrice; });
            //    structTax.Add(new structTax(k, 50 - tmpTax, item.Count, productTotal));
            //    k++;
            //}
            ////分组的id,有10000个组合，每一个组合做了个编号
            //var groupId = 0;
            //List<structTax> conformTax = (from s
            //                                                in structTax
            //                              where s.tax >= 0 && s.tax < 50 && s.productTotal <= 1000
            //                              select s).OrderBy(a => a.tax).ThenByDescending(a => a.itemCount).ToList();

            //if (conformTax != null && conformTax.Count > 0)//理想组合，税费<50&&商品总额<=1000
            //{
            //    groupId = conformTax.FirstOrDefault().index;
            //    lineItemInfoGroups.Add(ListLineItemInfocollections[groupId]);
            //    ListLineItemInfocollections[groupId].ForEach(a => ListLineItemInfo.Remove(a));
            //}
            //else//商品税费大于50||商品税率为0||商品总额>1000
            //{
            //    if (ListLineItemInfo.Count > 0)
            //    {
            //        List<structTax> badTax = (from s in structTax where s.productTotal <= 1000 select s).OrderByDescending(a => a.itemCount).ToList();
            //        if (badTax != null && badTax.Count > 0)//税率>50 &&商品总额<=1000
            //        {
            //            groupId = badTax.FirstOrDefault().index;
            //            lineItemInfoGroups.Add(ListLineItemInfocollections[groupId]);
            //            //找了合适的组合了，就把这个组合内的所有商品从大集合中去掉
            //            ListLineItemInfocollections[groupId].ForEach(a => ListLineItemInfo.Remove(a));
            //        }
            //        else//税率>50 &&商品总额>1000
            //        {
            //            foreach (LineItemInfo lastItem in ListLineItemInfo)
            //            {
            //                List<LineItemInfo> lastGroupLineItemInfo = new List<LineItemInfo>() { lastItem };
            //                lineItemInfoGroups.Add(lastGroupLineItemInfo);
            //            }
            //            ListLineItemInfo = null;
            //        }
            //    }
            //}
            //GetLineItemInfoGroups(ref lineItemInfoGroups, ListLineItemInfo);
        }

        /// <summary>
        /// 穷举所有组合
        /// </summary>
        /// <param name="list">订单大集合</param>
        /// <returns></returns>
        public static List<List<LineItemInfo>> GetChildCollections(List<LineItemInfo> list)
        {
            //<< 左移相当于乘，左移一位相当于乘2 demo:var t = 1 << 2;t= 4,最大为2的30次方法，就是list.Count最大为30,否则就会报错    
            //var subsets = from m in Enumerable.Range(0, 1 << list.Count)
            //              select (from i in Enumerable.Range(0, list.Count)
            //                      where (m & (1 << i)) != 0
            //                      select list[i]).ToList();
            //return subsets.ToList();

            List<List<LineItemInfo>> result = new List<List<LineItemInfo>>();
            var priceMoreThan1000Items = list.Where(p => p.ItemAdjustedPrice >= 1000);
            var tempList = new List<LineItemInfo>();
            foreach (var item in priceMoreThan1000Items)
            {
                result.Add(new List<LineItemInfo> { item });
                tempList.Add(item);
            }
            foreach (var item in tempList)
            {
                list.Remove(item);
            }
            //var taxMoreThan50Items = list.OrderBy(p => p.SkuId).Where(p => p.ItemAdjustedPrice * p.TaxRate > 50).ToList();
            var taxMoreThan50Items = list.OrderBy(p => p.SkuId).Where(p => p.Tax > 50).ToList();
            SplitSubItemListByMoreThan50Tax(taxMoreThan50Items, ref result);
            foreach (var item in taxMoreThan50Items)
            {
                list.Remove(item);
            }

            //list = list.OrderBy(p => p.SkuId).ThenBy(p => p.TaxRate * p.ItemAdjustedPrice).ThenBy(p => p.ItemAdjustedPrice).ToList();
            //list = list.OrderBy(p => p.SkuId).ThenBy(p => p.Tax).ThenBy(p => p.ItemAdjustedPrice).ToList();

            list = list.OrderByDescending(p => p.Tax).ThenByDescending(p => p.ItemAdjustedPrice).ThenBy(p => p.SkuId).ToList();
            SplitSubItemListByTax(list, ref result);
            return result;
        }

        /// <summary>
        /// 单个关税大于50的商品组合，只要总价不超过1000组成1个单。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="result"></param>
        private static void SplitSubItemListByMoreThan50Tax(List<LineItemInfo> list, ref List<List<LineItemInfo>> result)
        {
            if (list == null || list.Count <= 0)
            {
                return;
            }

            List<LineItemInfo> tempItems = new List<LineItemInfo>();
            int itemCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                tempItems.Add(list[i]);
                if (tempItems.Sum(p => p.ItemAdjustedPrice) > 1000)
                {
                    itemCount = i;
                    tempItems.RemoveAt(i);
                    break;
                }
            }
            itemCount = itemCount == 0 ? list.Count : itemCount;
            result.Add(tempItems);
            list = list.Skip(itemCount).ToList();
            SplitSubItemListByMoreThan50Tax(list, ref result);
        }

        /// <summary>
        ///  单个关税小于50且单价小于1000的商品组单。
        ///  根据区间税款之和分段截取List LineItemInfo对象。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="result"></param>
        private static void SplitSubItemListByTax(List<LineItemInfo> list, ref List<List<LineItemInfo>> result)
        {
            if (list == null || list.Count <= 0)
            {
                return;
            }

            //if (list.Count == 1 || (list.Sum(p => p.ItemAdjustedPrice * p.TaxRate) <= 50 && list.Sum(p => p.ItemAdjustedPrice) <= 1000))
            //{
            //    result.Add(list);
            //    return;
            //}

            if (list.Count == 1 || (list.Sum(p => p.Tax) <= 50 && list.Sum(p => p.ItemAdjustedPrice) <= 1000))
            {
                result.Add(list);
                return;
            }

            //List<LineItemInfo> tempItems = new List<LineItemInfo>();
            //int itemCount = 0;
            //for (int i = 0; i < list.Count; i++)
            //{
            //    tempItems.Add(list[i]);
            //    //if (tempItems.Sum(p => p.ItemAdjustedPrice * p.TaxRate) > 50 || tempItems.Sum(p => p.ItemAdjustedPrice) > 1000)
            //    //{
            //    //    itemCount = i;
            //    //    tempItems.RemoveAt(i);
            //    //    break;
            //    //}

            //    if (tempItems.Sum(p => p.Tax) > 50 || tempItems.Sum(p => p.ItemAdjustedPrice) > 1000)
            //    {
            //        itemCount = i;
            //        tempItems.RemoveAt(i);
            //        break;
            //    }

                
            //}
            //itemCount = itemCount == 0 ? list.Count : itemCount;
            //result.Add(tempItems);
            //list = list.Skip(itemCount).ToList();
            //SplitSubItemListByTax(list, ref result);

            List<LineItemInfo> tempItems = new List<LineItemInfo>();

            tempItems.Add(list[0]);

            list.RemoveAt(0);

            for (int i = list.Count-1; i >=0; i--)
            {
                tempItems.Add(list[i]);
                if (tempItems.Sum(p => p.Tax) > 50 || tempItems.Sum(p => p.ItemAdjustedPrice) > 1000)
                {
                    tempItems.RemoveAt(tempItems.Count-1);
                    break;
                }

                else
                {
                    list.RemoveAt(i);
                }


            }
            result.Add(tempItems);
            SplitSubItemListByTax(list, ref result);
        }

        /// <summary>
        /// 分拆单商品多件情况（不限制数量）
        /// </summary>
        /// <param name="list">订单大集合</param>
        /// <returns></returns>
        public static List<List<LineItemInfo>> GetChildCollectionsOneSku(List<LineItemInfo> list)
        {
            List<List<LineItemInfo>> result = new List<List<LineItemInfo>>();

            if (list.Count == 0)
                return null;

            if (list.Count == 1)
            {
                return new List<List<LineItemInfo>>() { list };
            }

            decimal price = list[0].ItemAdjustedPrice;
            decimal taxRate = list[0].TaxRate;
            decimal tax = price * taxRate;

            int perCount = 1;
            perCount = (int)(1000M / price);

            if (tax == 0M || tax > 50M)        //不需要税，按数量分组 或者 每个商品的税都超过 50,
            {
                if (perCount >= list.Count)
                {
                    result.Add(list);
                }

                List<LineItemInfo> items = new List<LineItemInfo>();

                for (int i = 0; i < list.Count; i += perCount)
                {
                    items = new List<LineItemInfo>();

                    for (int n = i; n < list.Count; n++)
                    {
                        items.Add(list[n]);
                    }

                    result.Add(items);
                }
            }
            else if (tax > 0M && tax <= 50)         //需要税，需要时考虑税和价格
            {
                int taxPerCount = (int)(50M / tax);

                if (taxPerCount < perCount)
                    perCount = taxPerCount;

                List<LineItemInfo> items = new List<LineItemInfo>();

                for (int i = 0; i < list.Count; i += perCount)
                {
                    items = new List<LineItemInfo>();

                    for (int n = i; n < list.Count; n++)
                    {
                        items.Add(list[n]);
                    }

                    result.Add(items);
                }
            }

            return result;
        }

        public static bool CheckCanBuyingWithQuantity(OrderInfo order)
        {
            return true;
        }

        public static IList<string> GetPayingOrder()
        {
            return new OrderDao().GetPayingOrder();
        }
        public static IDictionary<string, string> GetWxPayingOrder()
        {
            return new OrderDao().GetWxPayingOrder();
        }

        public static bool CheckOrderSupplier(OrderInfo orderinfo)
        {
            Dictionary<string, LineItemInfo> lineItems = orderinfo.LineItems;
            return false;
        }
        /// <summary>
        /// 拆单时批量生成订单，运费、优惠信息在第一个订单，使用请注意
        /// </summary>
        /// <param name="orderInfoList">添加订单列表</param>
        /// <returns></returns>
        public static bool BathCreatOrder(List<OrderInfo> orderInfoList)
        {
            bool result;
            if (orderInfoList == null || orderInfoList.Count < 2)
            {
                result = false;
            }
            else
            {
                bool flag = false;
                Database database = DatabaseFactory.CreateDatabase();
                using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                    int index = 0;
                    try
                    {
                        foreach (OrderInfo orderInfo in orderInfoList)
                        {
                            bool isChildAndFirstChildThenOrOriginalOrder = true;

                            if (index != 0)
                            {
                                isChildAndFirstChildThenOrOriginalOrder = false;
                            }
                            index++;
                            if (!new OrderDao().CreatOrder(orderInfo, dbTransaction, isChildAndFirstChildThenOrOriginalOrder))
                            {
                                dbTransaction.Rollback();
                                result = false;
                                return result;
                            }
                            if (orderInfo.LineItems.Count > 0)
                            {
                                if (!new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTransaction))
                                {
                                    dbTransaction.Rollback();
                                    result = false;
                                    return result;
                                }
                            }
                            if (orderInfo.Gifts.Count > 0)
                            {
                                OrderGiftDao orderGiftDao = new OrderGiftDao();
                                foreach (OrderGiftInfo current in orderInfo.Gifts)
                                {
                                    if (!orderGiftDao.AddOrderGift(orderInfo.OrderId, current, 0, dbTransaction))
                                    {
                                        dbTransaction.Rollback();
                                        result = false;
                                        return result;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(orderInfo.CouponCode))
                            {
                                if (!new CouponDao().AddCouponUseRecord(orderInfo, dbTransaction))
                                {
                                    dbTransaction.Rollback();
                                    result = false;
                                    return result;
                                }
                            }
                            if (!string.IsNullOrEmpty(orderInfo.VoucherCode))
                            {
                                if (!new VoucherDao().UpdateVoucherItemByUsed(orderInfo, dbTransaction))
                                {
                                    dbTransaction.Rollback();
                                    result = false;
                                    return result;
                                }
                            }
                        }
                        dbTransaction.Commit();
                        flag = true;
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                        dbConnection.Close();
                        throw;
                    }
                    finally
                    {

                    }
                    //dbConnection.Close();
                }
                result = flag;
            }
            return result;
        }
        public static bool UpdateWillSplitOrder(string orderId, decimal tax, decimal orderTotal, decimal unpackedFreight)
        {
            return new OrderDao().UpdateWillSplitOrder(orderId, tax, orderTotal, unpackedFreight);
        }

        public static bool UpdateWillSplitNewOrder(string orderId, decimal tax, decimal orderTotal)
        {
            return new OrderDao().UpdateWillSplitNewOrder(orderId, tax, orderTotal);
        }

        public static decimal GetFreight(List<LineItemInfo> listItemInfo, ShippingAddressInfo shippingAddressInfo)
        {
            decimal freight = 0;
            Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
            foreach (LineItemInfo item in listItemInfo)
            {
                if (item.TemplateId > 0)
                {
                    if (dictShippingMode.ContainsKey(item.TemplateId))
                    {
                        dictShippingMode[item.TemplateId] += item.ItemWeight * item.Quantity;
                    }
                    else
                    {
                        dictShippingMode.Add(item.TemplateId, item.ItemWeight * item.Quantity);
                    }
                }
            }

            foreach (var item in dictShippingMode)
            {
                ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);//运费模版
                //ShippingModeInfo shippingMode = shippingModeInfoList.FirstOrDefault(p => p.TemplateId == item.Key);
                freight += ShoppingProcessor.CalcFreight(shippingAddressInfo.RegionId, item.Value, shippingMode);//计算运费
            }
            return freight;
        }
        public static decimal GetTax(List<LineItemInfo> listItemInfo)
        {
            decimal tax = 0m;
            if (listItemInfo != null)
            {
                if (listItemInfo.Count > 0)
                {
                    foreach (var item in listItemInfo)
                    {
                        tax += item.ItemAdjustedPrice * item.TaxRate * item.Quantity;
                    }
                }
            }
            return tax;
        }
        public static ShippingAddressInfo GetShippingAddress(int shippingId)
        {
            return new ShippingAddressDao().GetShippingAddress(shippingId);
        }
        public static DataTable GetNoWriteBackOrders()
        {
            return new OrderDao().GetNoWriteBackOrders();
        }
        public static decimal CalcShoppingCartFreight(ShoppingCartInfo shoppingCartInfo, int RegionId)//考虑多供应商必须分组
        {
            if (shoppingCartInfo == null
                || shoppingCartInfo.LineItems.Count == 0)
            {
                return 0;
            }
            decimal freight = 0m;
            var ShoppingCartInfoGroups = shoppingCartInfo.LineItems.GroupBy(a => a.SupplierId);
            List<ShoppingCartItemInfo> listShoppingCartItemInfo = null;
            Dictionary<int, decimal> dictShippingMode = null;
            foreach (var ShoppingCartInfoGroup in ShoppingCartInfoGroups)
            {
                listShoppingCartItemInfo = ShoppingCartInfoGroup.Select(a => a).ToList();
                dictShippingMode = new Dictionary<int, decimal>();
                foreach (ShoppingCartItemInfo item in listShoppingCartItemInfo)
                {
                    //修改运费计算  lindy
                    if ((!shoppingCartInfo.IsFreightFree && !item.IsfreeShipping))
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
                foreach (var item in dictShippingMode)//模拟分单，计算运费
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                    freight += ShoppingProcessor.CalcFreight(RegionId, item.Value, shippingMode);
                }
            }
            return freight;
        }

        public static IList<ShoppingCartItemInfo> GetSkuList(string skuIds)
        {
            return new ProductDao().GetSkuList(skuIds);
        }

        public static int GetSkusStock(int productId, int attributeId, int valueId)
        {
            return new SkuDao().GetSkusStock(productId, attributeId, valueId);
        }

        public static int CheckIsFirstOrder(int userId, int sourceOrder)
        {
            return new OrderDao().CheckIsFirstOrder(userId, sourceOrder);
        }

        public static void CheckIsFirstOrder(int userId, int orderSource, string orderId, out int result)
        {
            new OrderDao().CheckIsFirstOrder(userId, orderSource, orderId, out result);
        }

        public static bool ChangeOrderPaymentType(OrderInfo order)
        {
            return new OrderDao().ChangeOrderPaymentType(order);
        }

        /// <summary>
        /// 判断是否是特定二级类型的商品使用了优惠券
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="cmbCoupCode"></param>
        /// <returns></returns>
        public static string GetCurrentCategoryDesc(ShoppingCartInfo shoppingCart, string cmbCoupCode, int userId = 0)
        {
            string result = string.Empty;
            CouponInfo couponInfo = ShoppingProcessor.GetCoupon(cmbCoupCode);
            if (couponInfo != null)
            {
                ShoppingCartInfo curShopppingCart = new ShoppingCartInfo();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                string[] categoriesId = masterSettings.CurrentCategoryId.Split(',');
                
                if (shoppingCart != null && shoppingCart.LineItems.Count > 0)
                {
                    // 单个商品
                    if (shoppingCart.LineItems.Count == 1)
                    {

                        CategoryInfo cur = CategoryBrowser.GetCategory(shoppingCart.LineItems.FirstOrDefault().CategoryId);
                        if (cur.ParentCategoryId.HasValue && categoriesId.Contains(cur.ParentCategoryId.ToString()))
                        {
                            result =  masterSettings.CurrentCategoryDesc;
                        }
                    }
                    else if (shoppingCart.LineItems.Count > 1)
                    {
                        // 总金额
                        decimal total = shoppingCart.GetNewTotal();//shoppingCart.GetNewTotalIncludeTax();
                        //string curids = string.Join(",", shoppingCart.LineItems.Select(t => t.CategoryId).Distinct().ToArray());
                        Dictionary<string, string> dic = CategoryBrowser.GetSecondCategoriesById(userId > 0 ? userId : HiContext.Current.User.UserId);
                       
                        if (dic != null && dic.Count > 0)
                        {
                            List<ShoppingCartItemInfo> curshop = new List<ShoppingCartItemInfo>();
                            foreach (var d in dic)
                            {
                                ShoppingCartItemInfo shop = new ShoppingCartItemInfo();
                                if (categoriesId.Contains(d.Value))
                                {
                                    shop = shoppingCart.LineItems.Where(m => m.ProductId.ToString() == d.Key).FirstOrDefault();
                                    if (shop != null)
                                    {
                                        curShopppingCart.LineItems.Add(shop);
                                    }
                                }
                            }
                        }
                        // 包含特定类型的商品金额
                        decimal nowtotal = curShopppingCart.GetNewTotal();//curShopppingCart.GetNewTotalIncludeTax();                        
                        if ((total - nowtotal) < couponInfo.Amount)
                        {
                            result = masterSettings.CurrentCategoryDesc;
                        }
                    }
                }
            }
            return result;
        }
        
    }
    class structTax
    {
        //索引
        public int index;
        //税费
        public decimal tax;
        //记录数
        public int itemCount;
        //订单总金额
        public decimal productTotal;
        public structTax(int index, decimal tax, int itemCount, decimal productTotal)
        {
            this.index = index;
            this.tax = tax;
            this.itemCount = itemCount;
            this.productTotal = productTotal;
        }
    }
}

