using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Members;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
namespace EcShop.ControlPanel.Sales
{
    public static class OrderHelper
    {
        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }

        public static DataTable GetOrderItems(string orderId)
        {
            return new OrderDao().GetOrderItems(orderId);
        }

        public static DataTable GetWMSOrderItems(string orderId)
        {
            return new OrderDao().GetWMSOrderItems(orderId);
        }
         /// <summary>
        /// 根据订单号查询是否已申请报告
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public static bool CheckOrderIsbg(string OrderId)
        {
            return new OrderDao().CheckOrderIsbg(OrderId);
        }

        public static DbQueryResult GetOrders(OrderQuery query)
        {
            return new OrderDao().GetOrders(query);
        }

        public static OrdersInfo GetUserOrders(OrderQuery query)
        {
            return new OrderDao().GetUserOrders(query);
        }


        public static OrdersInfo GetWMSUserOrders(OrderQuery query)
        {
            return new OrderDao().GetWMSUserOrders(query);
        }


        /// <summary>
        /// 是否已经推送到WMS
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="isSendWMS">默认0，已推送1，只有推送WMS时会使用，其他业务不使用</param>
        /// <returns></returns>
        public static bool UpdateOrderWMSStatus(string orderId, int isSendWMS)
        {
            return new OrderDao().UpdateOrderWMSStatus(orderId, isSendWMS);
        }



        public static void SetOrderShipNumber(string[] orderIds, string startNumber, string ExpressCom = "")
        {
            string text = startNumber;
            for (int i = 0; i < orderIds.Length; i++)
            {
                if (i != 0)
                {
                    text = OrderHelper.GetNextExpress(ExpressCom, text);
                }
                new OrderDao().EditOrderShipNumber(orderIds[i], text);
            }
        }
        private static string GetNextExpress(string ExpressCom, string strno)
        {
            string text = ExpressCom.ToLower();
            string result;
            if (text != null)
            {
                if (text == "ems")
                {
                    result = OrderHelper.getEMSNext(strno);
                    return result;
                }
                if (text == "顺丰快递")
                {
                    result = OrderHelper.getSFNext(strno);
                    return result;
                }
                if (text == "宅急送")
                {
                    result = OrderHelper.getZJSNext(strno);
                    return result;
                }
            }
            result = (long.Parse(strno) + 1L).ToString();
            return result;
        }
        private static string getSFNext(string sfno)
        {
            int[] array = new int[12];
            int[] array2 = new int[12];
            List<char> list = sfno.ToList<char>();
            string value = sfno.Substring(0, 11);
            string text = string.Empty;
            if (sfno.Substring(0, 1) == "0")
            {
                text = "0" + (Convert.ToInt64(value) + 1L).ToString();
            }
            else
            {
                text = (Convert.ToInt64(value) + 1L).ToString();
            }
            for (int i = 0; i < 12; i++)
            {
                array[i] = int.Parse(list[i].ToString());
            }
            List<char> list2 = text.ToList<char>();
            for (int i = 0; i < 11; i++)
            {
                array2[i] = int.Parse(text[i].ToString());
            }
            if (array2[8] - array[8] == 1 && array[8] % 2 == 1)
            {
                if (array[11] - 8 >= 0)
                {
                    array2[11] = array[11] - 8;
                }
                else
                {
                    array2[11] = array[11] - 8 + 10;
                }
            }
            else
            {
                if (array2[8] - array[8] == 1 && array[8] % 2 == 0)
                {
                    if (array[11] - 7 >= 0)
                    {
                        array2[11] = array[11] - 7;
                    }
                    else
                    {
                        array2[11] = array[11] - 7 + 10;
                    }
                }
                else
                {
                    if ((array[9] == 3 || array[9] == 6) && array[10] == 9)
                    {
                        if (array[11] - 5 >= 0)
                        {
                            array2[11] = array[11] - 5;
                        }
                        else
                        {
                            array2[11] = array[11] - 5 + 10;
                        }
                    }
                    else
                    {
                        if (array[10] == 9)
                        {
                            if (array[11] - 4 >= 0)
                            {
                                array2[11] = array[11] - 4;
                            }
                            else
                            {
                                array2[11] = array[11] - 4 + 10;
                            }
                        }
                        else
                        {
                            if (array[11] - 1 >= 0)
                            {
                                array2[11] = array[11] - 1;
                            }
                            else
                            {
                                array2[11] = array[11] - 1 + 10;
                            }
                        }
                    }
                }
            }
            return text + array2[11].ToString();
        }
        private static string getEMSNext(string emsno)
        {
            long num = Convert.ToInt64(emsno.Substring(2, 8));
            if (num < 99999999L)
            {
                num += 1L;
            }
            string str = num.ToString().PadLeft(8, '0');
            string emsno2 = emsno.Substring(0, 2) + str + emsno.Substring(10, 1);
            return emsno.Substring(0, 2) + str + OrderHelper.getEMSLastNum(emsno2) + emsno.Substring(11, 2);
        }
        private static string getEMSLastNum(string emsno)
        {
            List<char> list = emsno.ToList<char>();
            int num = int.Parse(list[2].ToString()) * 8;
            num += int.Parse(list[3].ToString()) * 6;
            num += int.Parse(list[4].ToString()) * 4;
            num += int.Parse(list[5].ToString()) * 2;
            num += int.Parse(list[6].ToString()) * 3;
            num += int.Parse(list[7].ToString()) * 5;
            num += int.Parse(list[8].ToString()) * 9;
            num += int.Parse(list[9].ToString()) * 7;
            num = 11 - num % 11;
            if (num == 10)
            {
                num = 0;
            }
            else
            {
                if (num == 11)
                {
                    num = 5;
                }
            }
            return num.ToString();
        }
        private static string getZJSNext(string zjsno)
        {
            long num = Convert.ToInt64(zjsno) + 11L;
            if (num % 10L > 6L)
            {
                num -= 7L;
            }
            return num.ToString().PadLeft(zjsno.Length, '0');
        }
        public static bool SetOrderShipNumber(string orderId, string startNumber)
        {
            OrderDao orderDao = new OrderDao();
            OrderInfo orderInfo = orderDao.GetOrderInfo(orderId);
            orderInfo.ShipOrderNumber = startNumber;
            return orderDao.UpdateOrder(orderInfo, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="payStatus">支付状态，1支付中，2支付完成，0未支付，-1支付失败</param>
        /// <returns></returns>
        public static bool SetOrderPayStatus(string orderId, int payStatus)
        {
            OrderDao orderDao = new OrderDao();
            return orderDao.UpdateOrderPayStatus(orderId, payStatus);
        }

        public static bool UpdateOrderStatus(string orderId, int orderStatus)
        {
            OrderDao orderDao = new OrderDao();
            return orderDao.UpdateOrderStatus(orderId, orderStatus);
        }

        public static bool SetOrderPayStatus(string orderId, int payStatus, int paymentTypeId, string paymentTypeName, string gateway, string gatewayOrderId)
        {
            OrderDao orderDao = new OrderDao();
            return orderDao.UpdateOrderPayStatus(orderId, payStatus, paymentTypeId, paymentTypeName, gateway, gatewayOrderId);
        }

        /// <summary>
        /// 微信扫码支付完成时调用 只有订单状态为等待买家付款时才能修改
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="orderStatus">支付完成是调用状态为2 默认为2</param>
        /// <returns></returns>
        public static bool UpdateOrderStatus(string orderId, string transaction_id, int orderStatus = 2)
        {
            OrderDao orderDao = new OrderDao();
            return orderDao.UpdateOrderStatus(orderId, transaction_id, orderStatus);
        }
        public static void SetOrderPrinted(string[] orderIds, bool isPrinted)
        {
            OrderDao orderDao = new OrderDao();
            for (int i = 0; i < orderIds.Length; i++)
            {
                string text = orderIds[i];
                if (!string.IsNullOrEmpty(text))
                {
                    OrderInfo orderInfo = orderDao.GetOrderInfo(text);
                    orderInfo.IsPrinted = true;
                    orderDao.UpdateOrder(orderInfo, null);
                }
            }
        }
        public static System.Data.DataTable GetSendGoodsOrders(string orderIds)
        {
            return new OrderDao().GetSendGoodsOrders(orderIds);
        }
        public static int DeleteOrders(string orderIds)
        {
            int num = new OrderDao().DeleteOrders(orderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[]
				{
					orderIds
				}));
            }
            return num;
        }
        public static bool CloseTransaction(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool result;
            OrderDao orderdao = new OrderDao();
            VoucherDao voucherDao = new VoucherDao();
            CouponDao couponDao = new CouponDao();
            if (order.CheckAction(OrderActions.SELLER_CLOSE))
            {
                order.OrderStatus = OrderStatus.Closed;

                bool flag = false;
                Database database = DatabaseFactory.CreateDatabase();
                using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                    try
                    {
                        if (!orderdao.UpdateOrder(order, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }

                        if (!orderdao.UpdateRefundOrderStock(order.OrderId, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }

                        if (voucherDao.GetVoucherCounts(order.OrderId) > 0)
                        {
                            if (!voucherDao.RevertVoucher(order.OrderId, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = false;
                                return result;
                            }
                        }
                        if (couponDao.GetCouponCounts(order.OrderId) > 0)
                        {
                            if (!couponDao.RevertCoupon(order.OrderId, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = false;
                                return result;
                            }
                        }

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
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "关闭了订单“{0}”", new object[]
					{
						order.OrderId
					}));
                }
                result = flag;
            }
            else
            {
                result = false;
            }
            return result;
        }
        private static void ReducedPoint(OrderInfo order, Member member)
        {
            PointDetailInfo pointDetailInfo = new PointDetailInfo();
            pointDetailInfo.OrderId = order.OrderId;
            pointDetailInfo.UserId = member.UserId;
            pointDetailInfo.TradeDate = DateTime.Now;
            pointDetailInfo.TradeType = PointTradeType.Refund;
            pointDetailInfo.Reduced = new int?(order.Points);
            pointDetailInfo.Points = member.Points - pointDetailInfo.Reduced.Value;
            new PointDetailDao().AddPointDetail(pointDetailInfo);
        }
        public static void UpdateUserAccount(OrderInfo order)
        {
            int num = order.UserId;
            if (num == 1100)
            {
                num = 0;
            }
            IUser user = Users.GetUser(num, false);
            Member member = user as Member;
            if (member != null)
            {
                PointDetailInfo pointDetailInfo = new PointDetailInfo();
                pointDetailInfo.OrderId = order.OrderId;
                pointDetailInfo.UserId = member.UserId;
                pointDetailInfo.TradeDate = DateTime.Now;
                pointDetailInfo.TradeType = PointTradeType.Bounty;
                pointDetailInfo.Increased = new int?(order.Points);
                pointDetailInfo.Points = order.Points + member.Points;
                if (pointDetailInfo.Points > 2147483647 || pointDetailInfo.Points < 0)
                {
                    pointDetailInfo.Points = 2147483647;
                }
                PointDetailDao pointDetailDao = new PointDetailDao();
                pointDetailDao.AddPointDetail(pointDetailInfo);
                MemberDao memberDao = new MemberDao();
                memberDao.UpdateMemberAccount(order.GetTotal(), member.UserId);
                int historyPoint = pointDetailDao.GetHistoryPoint(member.UserId);
                memberDao.ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
            }
            ReferralDao referralDao = new ReferralDao();
            if (order.ReferralUserId > 0)
            {
                Member member2 = Users.GetUser(order.ReferralUserId) as Member;
                if (member2 != null && member2.ReferralStatus == 2)
                {
                    decimal orderReferralDeduct = OrderHelper.GetOrderReferralDeduct(order);
                    if (orderReferralDeduct > 0m)
                    {
                        referralDao.AddSplittinDetail(new SplittinDetailInfo
                        {
                            OrderId = order.OrderId,
                            UserId = member2.UserId,
                            UserName = member2.Username,
                            SubUserId = order.UserId,
                            IsUse = false,
                            TradeDate = DateTime.Now,
                            TradeType = SplittingTypes.ReferralDeduct,
                            Income = new decimal?(orderReferralDeduct),
                            Balance = referralDao.GetUserUseSplittin(member2.UserId),
                            Remark = string.Concat(new object[]
							{
								"购买者是：",
								order.Username,
								" 订单金额：",
								order.GetTotal()
							})
                        });
                    }
                    if (member2.ReferralUserId.HasValue)
                    {
                        Member member3 = Users.GetUser(member2.ReferralUserId.Value) as Member;
                        if (member3 != null && member3.ReferralStatus == 2)
                        {
                            decimal value = OrderHelper.GetOrderSubReferralDeduct(order);
                            referralDao.AddSplittinDetail(new SplittinDetailInfo
                            {
                                OrderId = order.OrderId,
                                UserId = member3.UserId,
                                UserName = member3.Username,
                                SubUserId = member2.UserId,
                                IsUse = false,
                                TradeDate = DateTime.Now,
                                TradeType = SplittingTypes.SubReferralDeduct,
                                Income = new decimal?(value),
                                Balance = referralDao.GetUserUseSplittin(member3.UserId),
                                Remark = string.Concat(new object[]
								{
									"下级推广员是：",
									member2.Username,
									" 订单金额：",
									order.GetTotal()
								})
                            });
                        }
                    }
                }
            }
            else
            {
                if (member != null && member.ReferralUserId.HasValue)
                {
                    Member member3 = Users.GetUser(member.ReferralUserId.Value) as Member;
                    if (member3 != null && member3.ReferralStatus == 2)
                    {
                        decimal value = OrderHelper.GetOrderSubMemberDeduct(order);
                        referralDao.AddSplittinDetail(new SplittinDetailInfo
                        {
                            OrderId = order.OrderId,
                            UserId = member3.UserId,
                            UserName = member3.Username,
                            SubUserId = order.UserId,
                            IsUse = false,
                            TradeDate = DateTime.Now,
                            TradeType = SplittingTypes.SubMemberDeduct,
                            Income = new decimal?(value),
                            Balance = referralDao.GetUserUseSplittin(member3.UserId),
                            Remark = string.Concat(new object[]
							{
								"下级会员是：",
								member.Username,
								" 订单金额：",
								order.GetTotal()
							})
                        });
                    }
                }
            }
            Users.ClearUserCache(user);
        }




        private static decimal GetOrderReferralDeduct(OrderInfo order)
        {
            decimal num = 0m;
            ProductDao productDao = new ProductDao();
            foreach (LineItemInfo current in order.LineItems.Values)
            {
                num += productDao.GetProductReferralDeduct(current.ProductId) * current.GetSubTotal() / 100m;
            }
            return num;
        }
        private static decimal GetOrderSubReferralDeduct(OrderInfo order)
        {
            decimal num = 0m;
            ProductDao productDao = new ProductDao();
            foreach (LineItemInfo current in order.LineItems.Values)
            {
                num += productDao.GetProductSubReferralDeduct(current.ProductId) * current.GetSubTotal() / 100m;
            }
            return num;
        }
        private static decimal GetOrderSubMemberDeduct(OrderInfo order)
        {
            decimal num = 0m;
            ProductDao productDao = new ProductDao();
            foreach (LineItemInfo current in order.LineItems.Values)
            {
                num += productDao.GetProductSubMemberDeduct(current.ProductId) * current.GetSubTotal() / 100m;
            }
            return num;
        }
        public static bool UpdateOrderShippingMode(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool result;
            if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE))
            {
                bool flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的配送方式", new object[]
					{
						order.OrderId
					}));
                }
                result = flag;
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 修改未发货订单货号。
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="productId"></param>
        /// <param name="newSku"></param>
        /// <returns></returns>
        public static bool UpdateOrderItemSku(string orderId, string skuId, int productId, string newSku)
        {
            return new OrderDao().UpdateOrderItemSku(orderId, skuId, productId, newSku);
        }
        /// <summary>
        /// 修改未发货订单供货商。
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="productId"></param>
        /// <param name="newSku"></param>
        /// <returns></returns>
        public static bool UpdateOrderItemSupplier(string orderId, string skuId, int productId, int supplierId)
        {
            return new OrderDao().UpdateOrderItemSupplier(orderId, skuId, productId, supplierId);
        }
        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool result;
            if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE))
            {
                bool flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的支付方式", new object[]
					{
						order.OrderId
					}));
                }
                result = flag;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public static bool MondifyAddress(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool result;
            if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS))
            {
                bool flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的收货地址", new object[]
					{
						order.OrderId
					}));
                }
                result = flag;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public static bool SaveRemark(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.RemarkOrder);
            OrderDao dao = new OrderDao();
            bool flag = dao.AddRemark(order);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[]
				{
					order.OrderId
				}));
            }
            return flag;
        }
        public static bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            return new OrderDao().SetOrderShippingMode(orderIds, realShippingModeId, realModeName);
        }
        public static bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            return new OrderDao().SetOrderExpressComputerpe(orderIds, expressCompanyName, expressCompanyAbb);
        }
        public static List<OrderRemark> GetOrderRemarks(string orderId)
        {
            return new OrderDao().GetOrderRemarks(orderId);
        }
        public static bool ConfirmPay(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.CofimOrderPay);
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            bool flag = false;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                    {
                        OrderDao orderDao = new OrderDao();
                        order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
                        order.PayDate = DateTime.Now;
                        if (!orderDao.UpdateOrder(order, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }
                        // 扣商品库存
                        orderDao.DebuctFactStock(order.OrderId);
                        // 反写销售库存
                        ProductDao productDao = new ProductDao();
                        // 明细
                        //foreach (LineItemInfo current in order.LineItems.Values)
                        //{
                        //    ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                        //    productDetails.SaleCounts += current.Quantity;
                        //    productDetails.ShowSaleCounts += current.Quantity;
                        //    //productDao.UpdateProduct(productDetails, dbTransaction);
                        //    productDao.UpdateProductsSaleCounts(productDetails, dbTransaction);
                        //}

                        //根据订单id更新商品库存
                        orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);


                        // 赠品
                        foreach (var current in order.PresentProducts)
                        {
                            ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                            productDetails.SaleCounts += current.ShipmentQuantity;
                            productDetails.ShowSaleCounts += current.ShipmentQuantity;
                            //productDao.UpdateProduct(productDetails, dbTransaction);
                            productDao.UpdateProductsSaleCounts(productDetails, dbTransaction);
                        }
                        OrderHelper.UpdateUserAccount(order);
                        EventLogs.WriteOperationLog(Privilege.CofimOrderPay, string.Format(CultureInfo.InvariantCulture, "确认收款编号为\"{0}\"的订单", new object[]
				{
					order.OrderId
				}));
                    }
                    dbTransaction.Commit();
                    flag = true;
                }
                catch
                {
                    dbTransaction.Rollback();
                    flag = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单商品", new object[]
				{
					order.OrderId
				}));
            }
            result = flag;
            return result;
        }
        public static bool ManagerConfirmPay(OrderInfo order, bool isBalancePayOrder, bool hasChildOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.CofimOrderPay);
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            bool flag = false;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                    {
                        OrderDao orderDao = new OrderDao();
                        if (!orderDao.UpdateOrderWhereHasChild(order.OrderId, order.GatewayOrderId))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }
                        // 扣商品库存
                        orderDao.DebuctFactStock(order.OrderId);
                        // 反写销售库存
                        ProductDao productDao = new ProductDao();
                        // 明细
                        //foreach (LineItemInfo current in order.LineItems.Values)
                        //{
                        //    ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                        //    productDetails.SaleCounts += current.Quantity;
                        //    productDetails.ShowSaleCounts += current.Quantity;
                        //    //productDao.UpdateProduct(productDetails, dbTransaction);
                        //    productDao.UpdateProductsSaleCounts(productDetails, dbTransaction);
                        //}

                        //根据订单id更新商品库存
                        orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);


                        // 赠品
                        foreach (var current in order.PresentProducts)
                        {
                            ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                            productDetails.SaleCounts += current.ShipmentQuantity;
                            productDetails.ShowSaleCounts += current.ShipmentQuantity;
                            //productDao.UpdateProduct(productDetails, dbTransaction);
                            productDao.UpdateProductsSaleCounts(productDetails, dbTransaction);
                        }
                        OrderHelper.UpdateUserAccount(order);
                        EventLogs.WriteOperationLog(Privilege.CofimOrderPay, string.Format(CultureInfo.InvariantCulture, "确认收款编号为\"{0}\"的订单", new object[]
				{
					order.OrderId
				}));
                    }
                    dbTransaction.Commit();
                    flag = true;
                }
                catch
                {
                    dbTransaction.Rollback();
                    flag = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单商品", new object[]
				{
					order.OrderId
				}));
            }
            result = flag;
            return result;
        }
        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_FINISH_TRADE))
            {
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = DateTime.Now;
                flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的订单", new object[]
					{
						order.OrderId
					}));
                }
            }
            return flag;
        }
        public static bool SendGoods(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderSendGoods);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                OrderDao orderDao = new OrderDao();
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                if (order.ShippingDate == DateTime.MinValue)
                {
                    order.ShippingDate = DateTime.Now;
                }
                flag = orderDao.UpdateOrder(order, null);
                if (flag)
                {
                    if (order.Gateway.ToLower() == "ecdev.plugins.payment.podrequest")
                    {
                        orderDao.UpdatePayOrderStock(order.OrderId);
                        //foreach (LineItemInfo current in order.LineItems.Values)
                        //{
                        //    ProductDao productDao = new ProductDao();
                        //    ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                        //    productDetails.SaleCounts += current.Quantity;
                        //    productDetails.ShowSaleCounts += current.Quantity;
                        //    //productDao.UpdateProduct(productDetails, null);
                        //    productDao.UpdateProductsSaleCounts(productDetails, null);
                        //}

                        //根据订单id更新商品库存
                        orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);
                        OrderHelper.UpdateUserAccount(order);
                    }
                    EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[]
					{
						order.OrderId
					}));
                }
            }
            return flag;
        }


        public static bool WMSSendGoods(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                OrderDao orderDao = new OrderDao();
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                if (order.ShippingDate == DateTime.MinValue)
                {
                    order.ShippingDate = DateTime.Now;
                }
                flag = orderDao.UpdateOrder(order, null);
                if (flag)
                {
                    if (order.Gateway.ToLower() == "ecdev.plugins.payment.podrequest")
                    {
                        orderDao.UpdatePayOrderStock(order.OrderId);
                        //foreach (LineItemInfo current in order.LineItems.Values)
                        //{
                        //    ProductDao productDao = new ProductDao();
                        //    ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                        //    productDetails.SaleCounts += current.Quantity;
                        //    productDetails.ShowSaleCounts += current.Quantity;
                        //    //productDao.UpdateProduct(productDetails, null);
                        //    productDao.UpdateProductsSaleCounts(productDetails, null);
                        //}

                        //根据订单id更新商品库存
                        orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);

                        OrderHelper.UpdateUserAccount(order);
                    }
                    EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[]
					{
						order.OrderId
					}));
                }
            }
            return flag;
        }

        public static bool UpdateOrderAmount(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"订单的金额", new object[]
					{
						order.OrderId
					}));
                }
            }
            return flag;
        }
        public static bool DeleteLineItem(string sku, OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            bool flag;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    order.LineItems.Remove(sku);
                    if (!new LineItemDao().DeleteLineItem(sku, order.OrderId, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    if (!new OrderDao().UpdateOrder(order, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    dbTransaction.Commit();
                    flag = true;
                }
                catch
                {
                    dbTransaction.Rollback();
                    flag = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单商品", new object[]
				{
					order.OrderId
				}));
            }
            result = flag;
            return result;
        }
        public static bool UpdateLineItem(string sku, OrderInfo order, int quantity)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            bool flag;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    order.LineItems[sku].Quantity = quantity;
                    order.LineItems[sku].ShipmentQuantity = quantity;
                    order.LineItems[sku].ItemAdjustedPrice = order.LineItems[sku].ItemListPrice;
                    if (!new LineItemDao().UpdateLineItem(order.OrderId, order.LineItems[sku], dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    if (!new OrderDao().UpdateOrder(order, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    dbTransaction.Commit();
                    flag = true;
                }
                catch (Exception var_4_D0)
                {
                    dbTransaction.Rollback();
                    flag = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单号为\"{0}\"的订单商品数量", new object[]
				{
					order.OrderId
				}));
            }
            result = flag;
            return result;
        }
        public static int GetSkuStock(string skuId)
        {
            SKUItem skuItem = new SkuDao().GetSkuItem(skuId);
            int result;
            if (skuItem == null)
            {
                result = 0;
            }
            else
            {
                result = skuItem.Stock;
            }
            return result;
        }
        public static bool DeleteOrderGift(OrderInfo order, int giftId)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    OrderGiftDao orderGiftDao = new OrderGiftDao();
                    OrderGiftInfo orderGift = orderGiftDao.GetOrderGift(giftId, order.OrderId);
                    order.Gifts.Remove(orderGift);
                    if (!orderGiftDao.DeleteOrderGift(order.OrderId, orderGift.GiftId, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                    }
                    else
                    {
                        if (!new OrderDao().UpdateOrder(order, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                        }
                        else
                        {
                            dbTransaction.Commit();
                            EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单礼品", new object[]
							{
								order.OrderId
							}));
                            result = true;
                        }
                    }
                }
                catch
                {
                    dbTransaction.Rollback();
                    result = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            return result;
        }
        public static DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            return new OrderGiftDao().GetOrderGifts(query);
        }
        public static DbQueryResult GetGifts(GiftQuery query)
        {
            return new OrderGiftDao().GetGifts(query);
        }
        public static bool ClearOrderGifts(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            bool flag;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    order.Gifts.Clear();
                    if (!new OrderGiftDao().ClearOrderGifts(order.OrderId, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    if (!new OrderDao().UpdateOrder(order, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    dbTransaction.Commit();
                    flag = true;
                }
                catch
                {
                    dbTransaction.Rollback();
                    flag = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "清空了订单号为\"{0}\"的订单礼品", new object[]
				{
					order.OrderId
				}));
            }
            result = flag;
            return result;
        }
        public static bool AddOrderGift(OrderInfo order, GiftInfo giftinfo, int quantity, int promotype)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            bool flag3;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
                    orderGiftInfo.OrderId = order.OrderId;
                    orderGiftInfo.Quantity = quantity;
                    orderGiftInfo.GiftName = giftinfo.Name;
                    decimal arg_5C_0 = orderGiftInfo.CostPrice;
                    bool flag = 1 == 0;
                    orderGiftInfo.CostPrice = Convert.ToDecimal(giftinfo.CostPrice);
                    orderGiftInfo.GiftId = giftinfo.GiftId;
                    orderGiftInfo.ThumbnailsUrl = giftinfo.ThumbnailUrl40;
                    orderGiftInfo.PromoteType = promotype;
                    bool flag2 = false;
                    foreach (OrderGiftInfo current in order.Gifts)
                    {
                        if (giftinfo.GiftId == current.GiftId)
                        {
                            flag2 = true;
                            current.Quantity = quantity;
                            current.PromoteType = promotype;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        order.Gifts.Add(orderGiftInfo);
                    }
                    if (!new OrderGiftDao().AddOrderGift(order.OrderId, orderGiftInfo, quantity, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    if (!new OrderDao().UpdateOrder(order, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                        return result;
                    }
                    dbTransaction.Commit();
                    flag3 = true;
                }
                catch
                {
                    dbTransaction.Rollback();
                    flag3 = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            if (flag3)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "成功的为订单号为\"{0}\"的订单添加了礼品", new object[]
				{
					order.OrderId
				}));
            }
            result = flag3;
            return result;
        }
        public static IList<GiftInfo> GetGiftList(GiftQuery query)
        {
            return new OrderGiftDao().GetGiftList(query);
        }
        public static System.Data.DataSet GetTradeOrders(OrderQuery query, out int records, int delaytime=30)
        {
            return new OrderDao().GetTradeOrders(query, out records, delaytime);
        }
        public static System.Data.DataSet GetServiceOrders(ServiceOrderQuery query, out int records)
        {
            return new OrderDao().GetServiceOrders(query, out records);
        }
        public static System.Data.DataTable GetServiceOrder(string orderId)
        {
            return new OrderDao().GetServiceOrder(orderId);
        }

        public static bool SendAPIGoods(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                OrderDao orderDao = new OrderDao();
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                order.ShippingDate = DateTime.Now;
                flag = orderDao.UpdateOrder(order, null);
                if (flag)
                {
                    if (order.Gateway.ToLower() == "ecdev.plugins.payment.podrequest")
                    {
                        orderDao.UpdatePayOrderStock(order.OrderId);
                        //foreach (LineItemInfo current in order.LineItems.Values)
                        //{
                        //    ProductDao productDao = new ProductDao();
                        //    ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                        //    productDetails.SaleCounts += current.Quantity;
                        //    productDetails.ShowSaleCounts += current.Quantity;
                        //    //productDao.UpdateProduct(productDetails, null);
                        //    productDao.UpdateProductsSaleCounts(productDetails, null);
                        //}

                        //根据订单id更新商品库存
                        orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);

                        OrderHelper.UpdateUserAccount(order);
                    }
                    EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[]
					{
						order.OrderId
					}));
                }
            }
            return flag;
        }
        public static System.Data.DataSet GetTradeOrders(string orderId)
        {
            return new OrderDao().GetTradeOrders(orderId);
        }
        public static System.Data.DataSet GetOrdersAndLines(string orderIds)
        {
            return new OrderDao().GetOrdersAndLines(orderIds);
        }
        public static System.Data.DataSet GetOrderGoods(string orderIds)
        {
            return new OrderDao().GetOrderGoods(orderIds);
        }
        public static System.Data.DataSet GetOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType)
        {
            return new OrderDao().GetOrderGoods(startPayTime, endPayTime, orderIds, orderStatus, orderTimeType);
        }
        public static System.Data.DataSet GetOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType, int supplierId)
        {
            return new OrderDao().GetOrderGoods(startPayTime, endPayTime, orderIds, orderStatus, orderTimeType, supplierId);
        }
        public static System.Data.DataSet GetClearOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType, int supplierId)
        {
            return new OrderDao().GetClearOrderGoods(startPayTime, endPayTime, orderIds, orderStatus, orderTimeType, supplierId);
        }
        public static System.Data.DataSet GetClearOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType)
        {
            return new OrderDao().GetClearOrderGoods(startPayTime, endPayTime, orderIds, orderStatus, orderTimeType);
        }
        public static System.Data.DataSet GetProductGoods(string orderIds)
        {
            return new OrderDao().GetProductGoods(orderIds);
        }
        public static System.Data.DataSet GetProductGoods(string orderIds, int supplierId)
        {
            return new OrderDao().GetProductGoods(orderIds, supplierId);
        }

        public static System.Data.DataSet GetClearProductGoods(string orderIds, int orderStatus)
        {
            return new OrderDao().GetClearProductGoods(orderIds, orderStatus);
        }


        public static bool CheckRefund(OrderInfo order, string Operator, string adminRemark, int refundType, decimal refundMoney, bool accept, bool isReurnCoupon)
        {
            //ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplyAccept);
            bool result;
            if (order.OrderStatus == OrderStatus.Refunded)
            {
                RefundDao refundDao = new RefundDao();
                bool flag = refundDao.UpdateOrderRefund(order, Operator, adminRemark, refundType, refundMoney, accept);
                return flag;
            }
            if (order.OrderStatus != OrderStatus.ApplyForRefund)
            {
                result = false;
            }
            else
            {
                RefundDao refundDao = new RefundDao();
                bool flag = refundDao.CheckRefund(order, Operator, adminRemark, refundType, refundMoney, accept, isReurnCoupon);
                if (flag)
                {
                    if (accept)
                    {
                        IUser user = Users.GetUser(order.UserId, false);

                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            OrderHelper.ReducedPoint(order, user as Member);
                            new ReferralDao().RemoveNoUseSplittin(order.OrderId);
                            Users.ClearUserCache(user);
                        }

                        /*Member member = user as Member;
                        if (member != null)
                        {
                            PointDetailInfo pointDetailInfo = new PointDetailInfo();
                            pointDetailInfo.OrderId = order.OrderId;
                            pointDetailInfo.UserId = member.UserId;
                            pointDetailInfo.TradeDate = DateTime.Now;
                            pointDetailInfo.TradeType = PointTradeType.Bounty;
                            pointDetailInfo.Increased = new int?(order.Points);
                            pointDetailInfo.Points = order.Points + member.Points;
                            if (pointDetailInfo.Points > 2147483647 || pointDetailInfo.Points < 0)
                            {
                                pointDetailInfo.Points = 2147483647;
                            }
                            PointDetailDao pointDetailDao = new PointDetailDao();
                            //退款订单需要减除已累计的积分
                            pointDetailDao.UpdatePointDetail(pointDetailInfo);
                            new ReferralDao().RemoveNoUseSplittin(order.OrderId);
                            Users.ClearUserCache(user);
                        }*/


                        //款订单需要减除已累计的消费金额
                        new MemberDao().UpdateUserStatistics(order.UserId, refundMoney, true);

                        refundDao.UpdateRefundOrderStock(order.OrderId);

                        //2015-08-19
                        refundDao.AddOrderFactStock(order.OrderId);

                        refundDao.UpdateRefundOrderProductSaleCount(order.OrderId, null);
                    }
                    if (accept && order.GroupBuyId > 0)
                    {
                        EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的扣除违约金后退款", new object[]
						{
							order.OrderId
						}));
                    }
                    else
                    {
                        EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了全额退款", new object[]
						{
							order.OrderId
						}));
                    }
                }
                result = flag;
            }
            return result;
        }
        /// <summary>
        /// 检查当前用户是否已验证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool Checkisverify(int userId)
        {
            return new MemberDao().Checkisverify(userId);
        }
         /// <summary>
        /// 修改会员资料信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cellPhone"></param>
        /// <param name="IdentityCard"></param>
        /// <returns></returns>
        public static bool UpdateMemberInfo(int userId, string cellPhone, string IdentityCard,string RealName)
        {
            return new MemberDao().UpdateMemberInfo(userId, cellPhone, IdentityCard, RealName);
        }
        public static void GetRefundType(string orderId, out int refundType, out string remark)
        {
            new RefundDao().GetRefundType(orderId, out refundType, out remark);
        }
        public static bool CheckReturn(OrderInfo order, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            //ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
            bool result;
            if (order.OrderStatus == OrderStatus.Returned)
            {
                bool flag = new ReturnDao().UpdateReturn(order.OrderId, Operator, refundMoney, adminRemark, refundType, accept);
                return flag;
            }
            if (order.OrderStatus != OrderStatus.ApplyForReturns)
            {
                result = false;
            }
            else
            {
                bool flag = new ReturnDao().CheckReturn(order.OrderId, Operator, refundMoney, adminRemark, refundType, accept);
                if (flag)
                {
                    if (accept)
                    {
                        order.RefundAmount = refundMoney;
                        IUser user = Users.GetUser(order.UserId, false);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            OrderHelper.ReducedPoint(order, user as Member);//部分退货，未处理，todo
                            new ReferralDao().RemoveNoUseSplittin(order.OrderId);
                            Users.ClearUserCache(user);
                        }
                        new MemberDao().UpdateUserStatistics(order.UserId, refundMoney, false);
                    }
                    EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了退货", new object[]
					{
						order.OrderId
					}));
                }
                result = flag;
            }
            return result;
        }
        /// <summary>
        /// admin确认退货 增加 快递费用，清关费用，快递费归属
        /// </summary>
        /// <param name="order"></param>
        /// <param name="Operator"></param>
        /// <param name="refundMoney"></param>
        /// <param name="expressFee">快递费用</param>
        /// <param name="customsClearanceFee">清关费用</param>
        /// <param name="feeAffiliation">快递费归属</param>
        /// <param name="adminRemark"></param>
        /// <param name="refundType"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public static bool CheckReturn(OrderInfo order, string Operator, decimal refundMoney, decimal expressFee, decimal customsClearanceFee, string feeAffiliation, string adminRemark, int refundType, bool accept)
        {
            //ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
            bool result;
            if (order.OrderStatus == OrderStatus.Returned)
            {
                bool flag = new ReturnDao().UpdateReturn(order.OrderId, Operator, refundMoney, expressFee, customsClearanceFee, feeAffiliation, adminRemark, refundType, accept);
                return flag;
            }
            if (order.OrderStatus != OrderStatus.ApplyForReturns)
            {
                result = false;
            }
            else
            {
                bool flag = new ReturnDao().CheckReturn(order.OrderId, Operator, refundMoney, expressFee, customsClearanceFee, feeAffiliation, adminRemark, refundType, accept);
                if (flag)
                {
                    if (accept)
                    {
                        order.RefundAmount = refundMoney;
                        IUser user = Users.GetUser(order.UserId, false);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            OrderHelper.ReducedPoint(order, user as Member);//部分退货，未处理，todo
                            new ReferralDao().RemoveNoUseSplittin(order.OrderId);
                            Users.ClearUserCache(user);
                        }
                        new MemberDao().UpdateUserStatistics(order.UserId, refundMoney, false);
                    }
                    EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了退货", new object[]
					{
						order.OrderId
					}));
                }
                result = flag;
            }
            return result;
        }
        public static void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
        {
            new ReturnDao().GetRefundTypeFromReturn(orderId, out refundType, out remark);
        }
        public static bool CheckReplace(string orderId, string adminRemark, bool accept)
        {
            //ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApply);
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            return orderInfo.OrderStatus == OrderStatus.ApplyForReplacement && new ReplaceDao().CheckReplace(orderId, adminRemark, accept);
        }
        public static string GetReplaceComments(string orderId)
        {
            return new ReplaceDao().GetReplaceComments(orderId);
        }
        public static DbQueryResult GetRefundApplys(RefundApplyQuery query)
        {
            return new RefundDao().GetRefundApplys(query);
        }
        public static DbQueryResult GetRefundApplysMoney(RefundApplyQuery query)
        {
            return new RefundDao().GetRefundApplysMoney(query);
        }
        /// <summary>
        /// 导出退款单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet ExportRefundApplys(RefundApplyQuery query)
        {
            return new RefundDao().ExportRefundApplys(query);
        }
        public static bool DelRefundApply(string[] refundIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
            bool result = true;
            count = 0;
            RefundDao refundDao = new RefundDao();
            for (int i = 0; i < refundIds.Length; i++)
            {
                string text = refundIds[i];
                if (!string.IsNullOrEmpty(text))
                {
                    if (refundDao.DelRefundApply(int.Parse(text)))
                    {
                        count++;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
        public static DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        {
            return new ReturnDao().GetReturnsApplys(query);
        }
        public static DbQueryResult GetReturnsApplysMoney(ReturnsApplyQuery query)
        {
            return new ReturnDao().GetReturnsApplysMoney(query);
        }

        /// <summary>
        /// 导出退款单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet ExportReturnsApplys(ReturnsApplyQuery query)
        {
            return new ReturnDao().ExportReturnsApplys(query);
        }
        public static bool DelReturnsApply(string[] returnsIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
            bool result = true;
            count = 0;
            ReturnDao returnDao = new ReturnDao();
            for (int i = 0; i < returnsIds.Length; i++)
            {
                string text = returnsIds[i];
                if (!string.IsNullOrEmpty(text))
                {
                    if (returnDao.DelReturnsApply(int.Parse(text)))
                    {
                        count++;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
        public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        {
            return new ReplaceDao().GetReplaceApplys(query);
        }
        public static bool DelReplaceApply(string[] replaceIds, out int count)
        {
            bool result = true;
            count = 0;
            ReplaceDao replaceDao = new ReplaceDao();
            for (int i = 0; i < replaceIds.Length; i++)
            {
                string text = replaceIds[i];
                if (!string.IsNullOrEmpty(text))
                {
                    if (replaceDao.DelReplaceApply(int.Parse(text)))
                    {
                        count++;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
        public static bool SaveDebitNote(DebitNoteInfo note)
        {
            return new DebitNoteDao().SaveDebitNote(note);
        }
        public static DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            return new DebitNoteDao().GetAllDebitNote(query);
        }
        public static bool DelDebitNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            DebitNoteDao debitNoteDao = new DebitNoteDao();
            for (int i = 0; i < noteIds.Length; i++)
            {
                string text = noteIds[i];
                if (!string.IsNullOrEmpty(text))
                {
                    flag &= debitNoteDao.DelDebitNote(text);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }
        public static bool SaveSendNote(SendNoteInfo note)
        {
            return new SendNoteDao().SaveSendNote(note);
        }
        public static DbQueryResult GetAllSendNote(RefundApplyQuery query)
        {
            return new SendNoteDao().GetAllSendNote(query);
        }
        public static bool DelSendNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            SendNoteDao sendNoteDao = new SendNoteDao();
            for (int i = 0; i < noteIds.Length; i++)
            {
                string text = noteIds[i];
                if (!string.IsNullOrEmpty(text))
                {
                    flag &= sendNoteDao.DelSendNote(text);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }
        public static decimal GetChildOrderTotal(string masterOrderId)
        {
            return new OrderDao().GetChildOrderTotal(masterOrderId);
        }
        public static RefundInfo GetRefundByOrderId(string orderId)
        {
            return new RefundDao().GetByOrderId(orderId);
        }
        public static bool AllocateOrderToStore(string orderIds, int storeId)
        {
            return new OrderDao().AllocateOrderToStore(orderIds, storeId);
        }

        public static DataSet GetExcelOrderRefund(string startApplyForTime, string endApplyForTime, string refundIds, int handleStatus)
        {
            return new RefundDao().GetExcelOrderRefund(startApplyForTime, endApplyForTime, refundIds, handleStatus);
        }
        public static DataSet GetExcelOrderRefundDetails(string startApplyForTime, string endApplyForTime, string refundIds, int handleStatus)
        {
            return new RefundDao().GetExcelOrderRefundDetails(startApplyForTime, endApplyForTime, refundIds, handleStatus);
        }

        public static DataSet GetExcelOrdeReplace(string startApplyForTime, string endApplyForTime, string replaceIds, int handleStatus)
        {
            return new ReplaceDao().GetExcelOrdeReplace(startApplyForTime, endApplyForTime, replaceIds, handleStatus);
        }
        public static DataSet GetExcelOrdeReplaceDetails(string startApplyForTime, string endApplyForTime, string replaceIds, int handleStatus)
        {
            return new ReplaceDao().GetExcelOrdeReplaceDetails(startApplyForTime, endApplyForTime, replaceIds, handleStatus);
        }
        public static DataSet GetExcelOrderReturns(string startApplyForTime, string endApplyForTime, string returnsIds, int handleStatus)
        {
            return new ReturnDao().GetExcelOrderReturns(startApplyForTime, endApplyForTime, returnsIds, handleStatus);
        }
        public static DataSet GetExcelOrderReturnsDetails(string startApplyForTime, string endApplyForTime, string returnsIds, int handleStatus)
        {
            return new ReturnDao().GetExcelOrderReturnsDetails(startApplyForTime, endApplyForTime, returnsIds, handleStatus);
        }
        public static ReturnsInfo GetReturnsInfo(int returnsId)//获得退货单详情
        {
            return new ReturnDao().GetReturnsInfo(returnsId);
        }
        public static decimal GetReturnsAmount(int aid)//获得退回商品金额
        {
            return new ReturnDao().GetReturnsAmount(aid);
        }
        public static ReplaceInfo GetReplaceInfo(int replaceId)//获得退换单详情
        {
            return new ReplaceDao().GetReplaceInfo(replaceId);
        }

        public static IList<OrderInfo> GetOrderList(string date, int top)
        {
            return new OrderDao().GetOrderList(date, top);
        }

        public static decimal GetOrderAmount(string orderId)
        {
            return new OrderDao().GetOrderAmount(orderId);
        }

        #region 订单推送
        public static bool InsertOrderPush(string pushPlatform, string orderId, int status, string message)
        {
            return new OrderDao().InsertOrderPush(pushPlatform, orderId, status, message);
        }

        public static DbQueryResult GetPushOrders(string pushPlatform, int? status, int? expressStatus, int pageIndex, int pageSize)
        {
            return new OrderDao().GetPushOrders(pushPlatform, status, expressStatus, pageIndex, pageSize);
        }

        #endregion


        public static void DebuctFactStock(string orderId)
        {
            new OrderDao().DebuctFactStock(orderId);
        }

        public static bool UpdatePushOrders(string pushPlatform, Dictionary<string, string> orders)
        {
            //return new OrderDao().UpdatePushOrders(pushPlatform, orders);
            /*
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            if ((orderInfo.GroupBuyId <= 0 || orderInfo.GroupBuyStatus == GroupBuyStatus.Success) && ((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && orderInfo.Gateway == "Ecdev.plugins.payment.podrequest") || orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid) && num2 > 0 && !string.IsNullOrEmpty(textBox.Text.Trim()) && textBox.Text.Trim().Length <= 20)
            {
                ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(num2, true);
                orderInfo.RealShippingModeId = shippingMode.ModeId;
                orderInfo.RealModeName = shippingMode.Name;
                ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(listItem2.Value);
                orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
                orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
                orderInfo.ShipOrderNumber = textBox.Text;
                if (OrderHelper.SendGoods(orderInfo))
                {
                    SendNoteInfo sendNoteInfo = new SendNoteInfo();
                    sendNoteInfo.NoteId = Globals.GetGenerateId() + num;
                    sendNoteInfo.OrderId = orderId;
                    sendNoteInfo.Operator = HiContext.Current.User.Username;
                    sendNoteInfo.Remark = "后台" + sendNoteInfo.Operator + "发货成功";
                    OrderHelper.SaveSendNote(sendNoteInfo);
                    if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
                    {
                        PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
                        if (paymentMode != null)
                        {
                            PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
									{
										paymentMode.Gateway
									})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
									{
										paymentMode.Gateway
									})), "");
                            paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                        }
                    }
                    if (!string.IsNullOrEmpty(orderInfo.TaobaoOrderId))
                    {
                        try
                        {
                            string requestUriString = string.Format("http://vip.ecdev.cn/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}&Host={3}", new object[]
									{
										orderInfo.TaobaoOrderId,
										expressCompanyInfo.TaobaoCode,
										orderInfo.ShipOrderNumber,
										HiContext.Current.SiteUrl
									});
                            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
                            webRequest.GetResponse();
                        }
                        catch
                        {
                        }
                    }
                    int num3 = orderInfo.UserId;
                    if (num3 == 1100)
                    {
                        num3 = 0;
                    }
                    IUser user = Users.GetUser(num3);
                    Messenger.OrderShipping(orderInfo, user);
                    orderInfo.OnDeliver();
                    num++;
                }
            }
            */
            return true;
        }


        public static bool AddDeliverStatus(List<DeliverStatusInfo> list, out List<ErrorDeliverStatusInfo> errorlist)
        {
            return new DeliverDao().AddDeliverStatus(list, out errorlist);
        }

        /// <summary>
        /// 根据快递单号更新订单状态（对外接口，供快递公司调用）
        /// </summary>
        /// <param name="orderShip"></param>
        /// <returns></returns>
        public static void UpdateShipOrderStatusByShipOrderNumber(Dictionary<string, string> orderShip)
        {
            new OrderDao().UpdateShipOrderStatusByShipOrderNumber(orderShip);
        }

         /// <summary>
        /// 保存广告订单
        /// </summary>
        /// <param name="adInfo"></param>
        /// <returns></returns>
        public static bool InsertAdOrderInfo(orders adInfo,string jsonStr)
        {
            return new OrderDao().InsertAdOrderInfo(adInfo,jsonStr);
        }
           /// <summary>
        /// 根据单号获取订单明细
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public static DataTable GetOrderItemInfo(string OrderId)
        {
            return new OrderDao().GetOrderItemInfo(OrderId);
        }
        /// <summary>
        /// 根据时间查询广告订单
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="StartTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static DataTable SelectAdOrderInfo(int cid, DateTime StartTime, DateTime endTime, int type)
        {
            return new OrderDao().SelectAdOrderInfo(cid, StartTime, endTime, type);
        }

        /// <summary>
        /// 获取已发货的订单数
        /// </summary>
        /// <returns></returns>
        public static int GetDeliveredOrderCount()
        { 
            return new OrderDao().GetDeliveredOrderCount();
        }

        public static DataTable GetDeliveredOrderDt(int pageSize,int pageIndex)
        {
            return new OrderDao().GetDeliveredOrderDt(pageSize, pageIndex);
        }
    }
}
