using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Core;
using EcShop.SqlDal;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Members;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Promotions;
using EcShop.SqlDal.Sales;
using System;
using System.Collections.Generic;
using System.Data;

namespace EcShop.SaleSystem.Member
{
    using EcShop.Core.ErrorLog;
    using EcShop.Entities;//修改1
    using EcShop.Entities.VShop;
    using EcShop.Membership.Context;
    using EcShop.SqlDal.VShop;
    using EcShop.SqlDal.WMS;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Xml;
    public static class TradeHelper
    {
         /// <summary>
        /// 锁定取消退款
        /// </summary>
        /// <param name="SoureCode">来源单号</param>
        /// <param name="OrderId">订单号</param>
        /// <param name="OrderStatus">订单状态（是否拆单）</param>
        /// <returns></returns>
        public static bool ChangeRefundType(string OrderId)
        {
            return new OrderDao().ChangeRefundType(OrderId);
        }
        /// <summary>
        /// 获取退款列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRefundOrderList()
        {
            return new OrderDao().GetRefundOrderList();
        }
          /// <summary>
       /// 退款成功，修改退款状态
       /// </summary>
       /// <param name="SoureCode"></param>
       /// <param name="OrderId"></param>
       /// <param name="OrderStatus"></param>
       /// <returns></returns>
        public static bool RefundSuccess(string OrderId, int type)
        {
            return new OrderDao().RefundSuccess(OrderId,type);
        }
        /// <summary>
        /// 锁定订单
        /// </summary>
        /// <param name="OrderId">订单号</param>
        /// <param name="type">1：子单，2：母单</param>
        /// <returns></returns>
        public static bool ChangeRefundType(string OrderId, int type)
        {
            return new OrderDao().ChangeRefundType(OrderId, type);
        }
       /// <summary>
       /// 检查订单是否可退款
       /// </summary>
       /// <param name="OrderId"></param>
       /// <param name="type">1：子单，2：母单</param>
       /// <returns></returns>
        public  static bool CheckOrderIsRefund(string OrderId, int type, int delaytime)
        {
            return new OrderDao().CheckOrderIsRefund(OrderId, type, delaytime);
        }
        public static DbQueryResult GetUserPoints(int pageIndex)
        {
            return new PointDetailDao().GetUserPoints(pageIndex);
        }
        public static DbQueryResult GetUserPoints(int userId, int pageIndex, int pageSize)
        {
            return new PointDetailDao().GetUserPoints(userId, pageIndex, pageSize);
        }

        public static DataTable GetUserCoupons(int userId)
        {
            return new CouponDao().GetUserCoupons(userId, 0);
        }
        public static DbQueryResult GetUserCouponInfo(UserCouponQuery query)
        {
            return new CouponDao().GetUserCouponInfo(query);
        }

        public static DbQueryResult GetUserVoucher(UserVoucherQuery query)
        {
            return new VoucherDao().GetUserVoucher(query);
        }

        public static DataTable GetChangeCoupons()
        {
            return new CouponDao().GetChangeCoupons();
        }
        public static bool PointChageCoupon(int couponId, int needPoint, int currentPoint)
        {
            Member member = HiContext.Current.User as Member;
            PointDetailInfo pointDetailInfo = new PointDetailInfo();
            pointDetailInfo.OrderId = string.Empty;
            pointDetailInfo.UserId = member.UserId;
            pointDetailInfo.TradeDate = DateTime.Now;
            pointDetailInfo.TradeType = PointTradeType.ChangeCoupon;
            pointDetailInfo.Increased = null;
            pointDetailInfo.Reduced = new int?(needPoint);
            pointDetailInfo.Points = currentPoint - needPoint;
            bool result;
            if (pointDetailInfo.Points < 0)
            {
                result = false;
            }
            else
            {
                if (new PointDetailDao().AddPointDetail(pointDetailInfo))
                {
                    CouponItemInfo couponItemInfo = new CouponItemInfo();
                    couponItemInfo.CouponId = couponId;
                    couponItemInfo.UserId = new int?(member.UserId);
                    couponItemInfo.UserName = member.Username;
                    couponItemInfo.EmailAddress = member.Email;
                    couponItemInfo.ClaimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    couponItemInfo.GenerateTime = DateTime.Now;
                    Users.ClearUserCache(member);
                    if (new CouponDao().SendClaimCodes(couponItemInfo))
                    {
                        result = true;
                        return result;
                    }
                }
                result = false;
            }
            return result;
        }
        public static bool ExitCouponClaimCode(string claimCode)
        {
            return new CouponDao().ExitCouponClaimCode(claimCode);
        }
        public static int AddClaimCodeToUser(string claimCode, int userId)
        {
            return new CouponDao().AddClaimCodeToUser(claimCode, userId);
        }

        public static bool ExitVoucherClaimCode(string claimCode)
        {
            return new VoucherDao().ExitVoucherClaimCode(claimCode);
        }

        public static int AddVoucherClaimCodeToUser(string claimCode, int userId)
        {
            return new VoucherDao().AddVoucherClaimCodeToUser(claimCode, userId);
        }


        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }
        public static DataTable GetOrderItemThumbnailsUrl(string orderId)
        {
            return new OrderDao().GetOrderItemThumbnailsUrl(orderId);
        }


        public static DbQueryResult GetUserOrder(int userId, OrderQuery query)
        {
            return new OrderDao().GetMyUserOrder(userId, query);
        }

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return new GroupBuyDao().GetGroupBuy(groupBuyId);
        }
        public static CountDownInfo GetCountDownBuy(int CountDownId)
        {
            return new CountDownDao().GetCountDownInfo(CountDownId);
        }
        public static int GetOrderCount(int groupBuyId)
        {
            return new GroupBuyDao().GetOrderCount(groupBuyId);
        }
        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return new GroupBuyDao().SetGroupBuyEndUntreated(groupBuyId);
        }
        public static bool UserPayOrder(OrderInfo order, bool isBalancePayOrder)
        {
            bool flag = false;
            OrderDao orderDao = new OrderDao();
            order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            order.PayDate = DateTime.Now;
            flag = orderDao.UpdateOrder(order, null);
            if (flag)
            {
                Member member = Users.GetUser(order.UserId, false) as Member;
                BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
                if (isBalancePayOrder && member != null)
                {
                    decimal balance = member.Balance - order.GetTotal();
                    balanceDetailDao.InsertBalanceDetail(new BalanceDetailInfo
                    {
                        UserId = order.UserId,
                        UserName = order.Username,
                        TradeDate = DateTime.Now,
                        TradeType = TradeTypes.Consume,
                        Expenses = new decimal?(order.GetTotal()),
                        Balance = balance,
                        Remark = string.Format("对订单{0}付款", order.OrderId)
                    });
                }
                //
                #region 2015-08-19 TH 新增
                orderDao.DebuctFactStock(order.OrderId);
                #endregion

                //orderDao.UpdatePayOrderStock(order.OrderId);
                //ProductDao productDao = new ProductDao();
                //foreach (LineItemInfo current in order.LineItems.Values)
                //{
                //    ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                //    productDetails.SaleCounts += current.Quantity;
                //    productDetails.ShowSaleCounts += current.Quantity;
                //    //productDao.UpdateProduct(productDetails, null);
                //    productDao.UpdateProductsSaleCounts(productDetails, null);
                //}

                //根据订单id更新商品库存
                orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);


                TradeHelper.UpdateUserAccount(order);
            }
            else
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                ErrorLog.Write(string.Format("支付成功后，更新订单失败,订单信息：{0}", json));
            }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="isBalancePayOrder"></param>
        /// <param name="operationType">1表示用户支付了需要拆单的母单，2表示用户支付了将要合并的订单</param>
        /// <returns></returns>
        public static bool UserPayOrder(OrderInfo order, bool isBalancePayOrder, int operationType)
        {
            bool flag = false;
            OrderDao orderDao = new OrderDao();
            order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            order.PayDate = DateTime.Now;
            //flag = orderDao.UpdateOrder(order, null);
            if (operationType == 1)
            {
                flag = orderDao.UpdateOrderWhereHasChild(order.OrderId, order.GatewayOrderId);
            }
            else if (operationType == 2)
            {
                order.OrderStatus = OrderStatus.UnpackOrMixed;
                flag = orderDao.UpdateOrder(order, null);
            }
            else if (operationType == 0)
            {
                flag = orderDao.UpdateOrder(order, null);
            }

            if (flag)
            {
                //扣除实际库存
                orderDao.DebuctFactStock(order.OrderId);
                //更新销量
                orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);
                TradeHelper.UpdateUserAccount(order);
                Member member = Users.GetUser(order.UserId, false) as Member;
                BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
                if (isBalancePayOrder && member != null)
                {
                    decimal balance = member.Balance - order.GetTotal();
                    balanceDetailDao.InsertBalanceDetail(new BalanceDetailInfo
                    {
                        UserId = order.UserId,
                        UserName = order.Username,
                        TradeDate = DateTime.Now,
                        TradeType = TradeTypes.Consume,
                        Expenses = new decimal?(order.GetTotal()),
                        Balance = balance,
                        Remark = string.Format("对订单{0}付款", order.OrderId)
                    });
                    //orderDao.UpdatePayOrderStock(order.OrderId);
                    //ProductDao productDao = new ProductDao();
                    //foreach (LineItemInfo current in order.LineItems.Values)
                    //{
                    //    ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                    //    productDetails.SaleCounts += current.Quantity;
                    //    productDetails.ShowSaleCounts += current.Quantity;
                    //    //productDao.UpdateProduct(productDetails, null);
                    //    productDao.UpdateProductsSaleCounts(productDetails, null);


                    //}

                    //根据订单id更新商品库存
                    //orderDao.UpdatePayOrderProductSaleCount(order.OrderId, null);

                    //TradeHelper.UpdateUserAccount(order);
                }
            }

            else
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                ErrorLog.Write(string.Format("支付成功后，更新订单失败,订单信息：{0}", json));
            }

            return flag;
        }

        private static void UpdateUserAccount(OrderInfo order)
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
                    decimal orderReferralDeduct = TradeHelper.GetOrderReferralDeduct(order);
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
                            decimal value = TradeHelper.GetOrderSubReferralDeduct(order);
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
                        decimal value = TradeHelper.GetOrderSubMemberDeduct(order);
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
        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            bool result = false;
            if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
            {
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = DateTime.Now;
                result = new OrderDao().UpdateOrder(order, null);
            }
            return result;
        }

        /// <summary>
        /// 关闭订单改变状态，还原库存，还原优惠券，现金券的使用放在同一个事务中
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static bool CloseOrder(string orderId)
        {
            OrderDao orderDao = new OrderDao();
            OrderInfo orderInfo = orderDao.GetOrderInfo(orderId);
            VoucherDao voucherDao = new VoucherDao();
            CouponDao couponDao = new CouponDao();
            bool result;
            if (orderInfo.CheckAction(OrderActions.SELLER_CLOSE))
            {

                orderInfo.OrderStatus = OrderStatus.Closed;

                bool flag = false;
                Database database = DatabaseFactory.CreateDatabase();
                using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                    try
                    {
                        OrderDao orderdao = new OrderDao();
                        if (!orderDao.UpdateOrder(orderInfo, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }

                        if (!orderDao.UpdateRefundOrderStock(orderInfo.OrderId, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }

                        if (voucherDao.GetVoucherCounts(orderInfo.OrderId) > 0)
                        {
                            if (!voucherDao.RevertVoucher(orderInfo.OrderId, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = false;
                                return result;
                            }
                        }
                        if (couponDao.GetCouponCounts(orderInfo.OrderId) > 0)
                        {
                            if (!couponDao.RevertCoupon(orderInfo.OrderId, dbTransaction))
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
                result = flag;
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 关闭订单，还原库存和优惠券、现金券。
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="closeType"></param>
        /// <returns></returns>
        public static bool CloseOrder(string orderId, CloseOrderType closeType)
        {
            return new OrderDao().CloseOrder(orderId, closeType);
        }
        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="closeType"></param>
        /// <returns></returns>
        public static bool RefundOrder(string orderId)
        {
            return new OrderDao().RefundOrder(orderId);
        }
          /// <summary>
        /// 子单退款，还原优惠卷和库存
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="closeType"></param>
        /// <returns></returns>
        public static bool RefundOrder_Split(string orderId, string SoureCode)
        {
            return new OrderDao().RefundOrder_Split(orderId, SoureCode);
        }
        public static int LogicDeleteOrder(string orderId, int userStatus)
        {
            return new OrderDao().LogicDeleteOrder(orderId, userStatus);
        }

        public static int RemoveOrder(string orderIds)
        {
            return new OrderDao().DeleteOrders(orderIds);
        }

        /// <summary>
        /// 获取3分钟内的订单数以及当天的订单总数。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderCountIn3Min"></param>
        /// <param name="todayOrderCount"></param>
        public static void GetOrderCount4MaliciousOrder(int userId, out int orderCountIn3Min, out int todayOrderCount)
        {
            new OrderDao().GetOrderCount4MaliciousOrder(userId, out orderCountIn3Min, out todayOrderCount);
        }
        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            return order.CheckAction(OrderActions.SELLER_MODIFY_TRADE) && new OrderDao().UpdateOrder(order, null);
        }

        public static bool ModifyOrderPaymentType(OrderInfo order,string orderId)
        {
            return order.CheckAction(OrderActions.SELLER_MODIFY_TRADE) && new OrderDao().ModifyOrderPaymentType(order,orderId, null);
        }

        //public static bool UpdateUserAccount(string orderId)
        //{
        //    OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
        //    int num = HiContext.Current.User.UserId;
        //    if (num == 1100)
        //    {
        //        num = 0;
        //    }
        //    IUser user = Users.GetUser(num, false);
        //    Member member = user as Member;
        //    if (member != null)
        //    {
        //        PointDetailInfo pointDetailInfo = new PointDetailInfo();
        //        pointDetailInfo.OrderId = orderId;
        //        pointDetailInfo.UserId = member.UserId;
        //        pointDetailInfo.TradeDate = DateTime.Now;
        //        pointDetailInfo.TradeType = PointTradeType.Bounty;
        //        pointDetailInfo.Increased = new int?(orderInfo.Points);
        //        pointDetailInfo.Points = orderInfo.Points + member.Points;
        //        if (pointDetailInfo.Points > 2147483647 || pointDetailInfo.Points < 0)
        //        {
        //            pointDetailInfo.Points = 2147483647;
        //        }
        //        PointDetailDao pointDetailDao = new PointDetailDao();
        //        pointDetailDao.AddPointDetail(pointDetailInfo);
        //        MemberDao memberDao = new MemberDao();
        //        memberDao.UpdateMemberAccount(orderInfo.GetTotal(), member.UserId);
        //        int historyPoint = pointDetailDao.GetHistoryPoint(member.UserId);
        //        memberDao.ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
        //    }
        //    Users.ClearUserCache(user);
        //}


        /// <summary>
        /// 得到取消订单数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string getCancelData(string orderNo, int supplierId, string reason)
        {
            StringBuilder strData = new StringBuilder();
            strData.Append("<xmldata>");
            try
            {
                strData.Append("<data>");
                strData.Append("<ordernos>");
                strData.AppendFormat("<OrderNo>{0}</OrderNo>", orderNo);//订单号
                strData.AppendFormat("<OrderType>{0}</OrderType>", "CM");//CM
                strData.AppendFormat("<CustomerID>{0}</CustomerID>", supplierId.ToString());//货主ID
                strData.AppendFormat("<WarehouseID>{0}</WarehouseID>", "WH01");//仓库ID 固定：WH01
                strData.AppendFormat("<Reason>{0}</Reason>", reason);//取消原因
                strData.Append("</ordernos>");
                strData.Append("</data>");
            }
            catch
            {
            }
            strData.Append("</xmldata>");
            return strData.ToString().Trim();
        }

        /// <summary>
        /// 文本日志
        /// </summary>
        /// <param name="list"></param>
        public static void ErrorLogCancelData(string remark)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"/LogCancelData";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string FileNames = DateTime.Now.ToString("yyyyMMdd");
                StreamWriter sw = new StreamWriter(path + @"/" + FileNames + ".log", true);

                sw.Write(remark);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }


        /// <summary>
        /// 申请退款 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        /// <param name="refundType"></param>
        /// <returns></returns>
        public static bool ApplyForRefund(string orderId, string remark, int refundType, out string flagMsg)
        {
            string tempflagMsg = "";
            #region WMS取消订单验证
            //
            string apiUrl = System.Configuration.ConfigurationSettings.AppSettings["APIUrl"];
            string client_customerid = System.Configuration.ConfigurationSettings.AppSettings["client_customerid"];
            string apptoken = System.Configuration.ConfigurationSettings.AppSettings["apptoken"];
            string client_db = System.Configuration.ConfigurationSettings.AppSettings["client_db"];
            string appkey = System.Configuration.ConfigurationSettings.AppSettings["appkey"];
            string appSecret = System.Configuration.ConfigurationSettings.AppSettings["appSecret"];

            int wmsCancelSucess = 0;
            bool result = false;
            if (apiUrl != null && apiUrl != "" && client_customerid != null && client_customerid != "" && apptoken != null && apptoken != "" && client_db != null && client_db != null && appkey != null && appkey != "" && appSecret != null && appSecret != "")
            {
                OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
                if (orderInfo == null)
                {
                    //IsSendWMS
                    flagMsg = "订单不存在";
                    return result;
                }
                if (remark == "")
                {
                    remark = "买家取消订单";
                }
                if (orderInfo.IsSendWMS == 1)
                {
                    string skuData = getCancelData(orderId, orderInfo.SupplierId, remark);

                    string tempdata = appSecret + skuData.Replace("\n", "") + appSecret;

                    string md5tempdata = WMSHelpers.MD5(tempdata);
                    string basetempdata = WMSHelpers.EncodingString(md5tempdata.ToLower(), System.Text.Encoding.UTF8);
                    string sign = System.Web.HttpUtility.UrlEncode(basetempdata.ToUpper(), System.Text.Encoding.UTF8);

                    //
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string postData = "method=cancelSOData&client_customerid=" + client_customerid + "&client_db=" + client_db + "&messageid=SOC&apptoken=" + apptoken + "&appkey=" + appkey + "&sign=" + sign + "&timestamp=" + timestamp;

                    //.NET和java UrlEncode处理机制不一样，加号"+"在java里面会被替换成空格，需要转换2次
                    skuData = System.Web.HttpUtility.UrlEncode(skuData);
                    skuData = System.Web.HttpUtility.UrlEncode(skuData);

                    ErrorLogCancelData("url=" + apiUrl + ";postData=" + postData + "&data=" + tempdata + "\n");
                    //数据库日志
                    WMSHelpers.SaveLog("cancelSOData", tempdata, "调用方法", "info", "out");

                    string sendResult = WMSHelpers.PostData(apiUrl, postData + "&data=" + skuData);
                    string tempResult = System.Web.HttpUtility.UrlDecode(sendResult);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tempResult);
                    foreach (XmlNode node in xmlDocument.SelectNodes("Response/return/returnCode"))
                    {
                        if (node.InnerText != "0000")
                        {
                            wmsCancelSucess = -1;
                            ErrorLogCancelData("调用WMS取消订单失败:" + tempResult + "\n");
                            WMSHelpers.SaveLog("cancelSOData", "", "返回结果：" + tempResult, "error", "in");
                            foreach (XmlNode node1 in xmlDocument.SelectNodes("Response/return/returnDesc"))
                            {
                                if (node1.InnerText.ToString().Contains("订单不存在"))
                                {
                                    //#W订单不存在,取消失败
                                    //订单在WMS不存在时，可以直接取消
                                    wmsCancelSucess = 0;
                                }
                                else
                                {
                                    tempflagMsg = node1.InnerText;
                                }
                            }
                        }
                    }
                }
                else if (orderInfo.IsSendWMS == 0)
                {
                    wmsCancelSucess = 0;
                }
            }

            //
            #endregion

            if (wmsCancelSucess == 0)
            {
                result = new RefundDao().ApplyForRefund(orderId, remark, refundType);
                if (result)
                {
                    //退款订单需要减除已累计的积分和消费金额
                    OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
                    int num = orderInfo.UserId;
                    if (num == 1100)
                    {
                        num = 0;
                    }

                    IUser user = Users.GetUser(num, false);
                    Member member = user as Member;
                    if (member != null)
                    {
                        PointDetailInfo pointDetailInfo = new PointDetailInfo();
                        pointDetailInfo.OrderId = orderId;
                        pointDetailInfo.UserId = member.UserId;
                        pointDetailInfo.TradeDate = DateTime.Now;
                        pointDetailInfo.TradeType = PointTradeType.Bounty;
                        pointDetailInfo.Increased = new int?(orderInfo.Points);
                        pointDetailInfo.Points = orderInfo.Points + member.Points;
                        if (pointDetailInfo.Points > 2147483647 || pointDetailInfo.Points < 0)
                        {
                            pointDetailInfo.Points = 2147483647;
                        }
                        PointDetailDao pointDetailDao = new PointDetailDao();
                        pointDetailDao.UpdatePointDetail(pointDetailInfo);

                    }
                    Users.ClearUserCache(user);
                }

            }
            flagMsg = tempflagMsg;
            return result;

        }
        public static bool CanRefund(string orderId)
        {
            return new RefundDao().CanRefund(orderId);
        }
        public static bool ApplyForReturn(string orderId, string remark, int refundType)
        {
            return new ReturnDao().ApplyForReturn(orderId, remark, refundType);
        }
        public static bool ApplyForReturn(string orderId, string remark, int refundType, string LogisticsCompany, string LogisticsId)
        {
            return new ReturnDao().ApplyForReturn(orderId, remark, refundType, LogisticsCompany, LogisticsId);
        }
        public static bool ApplyForPartReturn(string orderId, string remark, int refundType, string skuIds)//新添加 部分退货
        {
            return new ReturnDao().ApplyForPartReturn(orderId, remark, refundType, skuIds);
        }
        public static bool CanReturn(string orderId)
        {
            return new ReturnDao().CanReturn(orderId);
        }
        public static bool ApplyForReplace(string orderId, string remark)
        {
            return new ReplaceDao().ApplyForReplace(orderId, remark);
        }
        public static bool CanReplace(string orderId)
        {
            return new ReplaceDao().CanReplace(orderId);
        }
        public static IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
        {
            return new PaymentModeDao().GetPaymentModes(payApplicationType);
        }
        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }
        public static DbQueryResult GetRefundApplys(RefundApplyQuery query)
        {
            return new RefundDao().GetRefundApplys(query);
        }

        public static DataSet NewGetRefundApplys(RefundApplyQuery query)
        {
            return new RefundDao().NewGetRefundApplys(query);
        }
        public static DataTable GetRefundApplysTable(int refundId)
        {
            return new RefundDao().GetRefundApplysTable(refundId);
        }
        public static DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        {
            return new ReturnDao().GetReturnsApplys(query);
        }

        public static DataSet NewGetReturnsApplys(ReturnsApplyQuery query)
        {
            return new ReturnDao().NewGetReturnsApplys(query);
        }
        public static decimal GetRefundMoney(OrderInfo order, out decimal refundMoney)
        {
            return new ReturnDao().GetRefundMoney(order, out refundMoney);
        }
        public static DataTable GetReturnsApplysTable(int returnsId)
        {
            return new ReturnDao().GetReturnsApplysTable(returnsId);
        }
        public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        {
            return new ReplaceDao().GetReplaceApplys(query);
        }


        public static DataSet NewGetReplaceApplys(ReplaceApplyQuery query)
        {
            return new ReplaceDao().NewGetReplaceApplys(query);
        }


        public static DataTable GetReplaceApplysTable(int replaceId)
        {
            return new ReplaceDao().GetReplaceApplysTable(replaceId);
        }
        public static bool SaveDebitNote(DebitNoteInfo note)
        {
            return new DebitNoteDao().SaveDebitNote(note);
        }
        public static bool CheckIsUnpack(string orderId)
        {
            return new OrderDao().CheckIsUnpack(orderId);
        }

        public static bool UpdateOrderRefundInfo(RefundInfo refundInfo)
        {
            return new RefundDao().UpdateOrderRefundInfo(refundInfo);
        }

        public static bool UpdateOrderReplaceInfo(ReplaceInfo replaceInfo)
        {
            return new ReplaceDao().UpdateOrderReplaceInfo(replaceInfo);
        }

        public static bool UpdateOrderReturnsInfo(ReturnsInfo returnsInfo)
        {
            return new ReturnDao().UpdateOrderReturnsInfo(returnsInfo);
        }
        public static string GetProvinceName(long IpAddress)
        {
            return new BannerDao().GetProvinceByIp(IpAddress);
        }

        public static bool ShareDeal(ShareInfo share)
        {
            return new ShareDao().ShareDeal(share);
        }
        public static bool CreateReturnsEntityAndAdd(string orderId, string remark, int returnType, string strSkuIds, string quantityList)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(quantityList))
            {
                return false;
            }
            string[] strQuantities = quantityList.Split(',');
            List<int> quantities = new List<int>();
            string[] skuIds = strSkuIds.Split(',');
            for (int i = 0; i < strQuantities.Length; i++)
            {
                try
                {
                    int quantity = int.Parse(strQuantities[i]);
                    if (quantity <= 0)
                    {
                        return false;
                    }
                    quantities.Add(quantity);
                }
                catch
                {
                    return false;
                }
            }
            if (skuIds.Length != quantities.Count)
            {
                return false;
            }
            ReturnsInfo returnsInfo = new ReturnsInfo();
            returnsInfo.OrderId = orderId;
            returnsInfo.Comments = remark;
            returnsInfo.ApplyForTime = DateTime.Now;
            returnsInfo.RefundMoney = 0;
            returnsInfo.HandleStatus = 0;
            returnsInfo.RefundType = returnType;
            returnsInfo.ReturnsLineItem = new List<OrderAppFormItems>();

            //赋值退货明细
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
            for (int i = 0; i < skuIds.Length; i++)
            {
                if (orderInfo.LineItems.ContainsKey(skuIds[i]))
                {
                    LineItemInfo lineItem = orderInfo.LineItems[skuIds[i]];
                    OrderAppFormItems returnsLineItem = new OrderAppFormItems();
                    returnsLineItem.SkuId = lineItem.SkuId;
                    returnsLineItem.ApplyType = 1;//退货单类型,2表示为退换类型
                    returnsLineItem.ProductId = lineItem.ProductId;
                    returnsLineItem.Quantity = quantities[i];//退货时间
                    returnsLineItem.ItemAdjustedPrice = lineItem.ItemAdjustedPrice;
                    returnsLineItem.ItemDescription = lineItem.ItemDescription;
                    returnsLineItem.ThumbnailsUrl = lineItem.ThumbnailsUrl;
                    returnsLineItem.ItemWeight = lineItem.ItemWeight;

                    returnsLineItem.SKUContent = lineItem.SKUContent;
                    returnsLineItem.PromotionId = lineItem.PromotionId;
                    returnsLineItem.PromotionName = lineItem.PromotionName;
                    returnsLineItem.TaxRate = lineItem.TaxRate;
                    returnsLineItem.TemplateId = lineItem.TemplateId;
                    returnsLineItem.storeId = lineItem.storeId;
                    returnsLineItem.SupplierId = lineItem.SupplierId;
                    returnsInfo.ReturnsLineItem.Add(returnsLineItem);
                }
            }
            return new ReturnDao().CreateReturn(returnsInfo);
        }
        /// <summary>
        /// VShop申请退货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        /// <param name="returnType"></param>
        /// <param name="strSkuIds"></param>
        /// <param name="quantityList"></param>
        /// <param name="logisticsCompany">物流公司</param>
        /// <param name="logisticsId">物流单号</param>
        /// <returns></returns>
        public static bool CreateReturnsEntityAndAdd(string orderId, string remark, int returnType, string strSkuIds, string quantityList, string logisticsCompany, string logisticsId)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(quantityList))
            {
                return false;
            }
            string[] strQuantities = quantityList.Split(',');
            List<int> quantities = new List<int>();
            string[] skuIds = strSkuIds.Split(',');
            for (int i = 0; i < strQuantities.Length; i++)
            {
                try
                {
                    int quantity = !string.IsNullOrEmpty(strQuantities[i]) ? int.Parse(strQuantities[i]) : 0;
                    if (quantity <= 0)
                    {
                        return false;
                    }
                    quantities.Add(quantity);
                }
                catch
                {
                    return false;
                }
            }
            if (skuIds.Length != quantities.Count)
            {
                return false;
            }
            ReturnsInfo returnsInfo = new ReturnsInfo();
            returnsInfo.OrderId = orderId;
            returnsInfo.Comments = remark;
            returnsInfo.ApplyForTime = DateTime.Now;
            returnsInfo.RefundMoney = 0;
            returnsInfo.HandleStatus = 0;
            returnsInfo.RefundType = returnType;
            returnsInfo.LogisticsCompany = logisticsCompany;
            returnsInfo.LogisticsId = logisticsId;
            returnsInfo.ReturnsLineItem = new List<OrderAppFormItems>();

            //赋值退货明细
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
            for (int i = 0; i < skuIds.Length; i++)
            {
                if (orderInfo.LineItems.ContainsKey(skuIds[i]))
                {
                    LineItemInfo lineItem = orderInfo.LineItems[skuIds[i]];
                    OrderAppFormItems returnsLineItem = new OrderAppFormItems();
                    if (lineItem.Quantity < quantities[i])
                    {
                        return false;
                    }
                    returnsLineItem.SkuId = lineItem.SkuId;
                    returnsLineItem.ApplyType = 1;//退货单类型,2表示为退换类型
                    returnsLineItem.ProductId = lineItem.ProductId;
                    returnsLineItem.Quantity = quantities[i];//退货时间
                    returnsLineItem.ItemAdjustedPrice = lineItem.ItemAdjustedPrice;
                    returnsLineItem.ItemDescription = lineItem.ItemDescription;
                    returnsLineItem.ThumbnailsUrl = lineItem.ThumbnailsUrl;
                    returnsLineItem.ItemWeight = lineItem.ItemWeight;

                    returnsLineItem.SKUContent = lineItem.SKUContent;
                    returnsLineItem.PromotionId = lineItem.PromotionId;
                    returnsLineItem.PromotionName = lineItem.PromotionName;
                    returnsLineItem.TaxRate = lineItem.TaxRate;
                    returnsLineItem.TemplateId = lineItem.TemplateId;
                    returnsLineItem.storeId = lineItem.storeId;
                    returnsLineItem.SupplierId = lineItem.SupplierId;
                    returnsInfo.ReturnsLineItem.Add(returnsLineItem);
                }
            }
            return new ReturnDao().CreateReturn(returnsInfo);
        }
        public static bool CreateReplaceEntityAndAdd(string orderId, string remark, string strSkuIds, string quantityList)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(quantityList))
            {
                return false;
            }
            string[] strQuantities = quantityList.Split(',');
            List<int> quantities = new List<int>();
            string[] skuIds = strSkuIds.Split(',');
            for (int i = 0; i < strQuantities.Length; i++)
            {
                try
                {
                    int quantity = int.Parse(strQuantities[i]);
                    if (quantity <= 0)
                    {
                        return false;
                    }
                    quantities.Add(quantity);
                }
                catch
                {
                    return false;
                }
            }
            if (skuIds.Length != quantities.Count)
            {
                return false;
            }
            ReplaceInfo replaceInfo = new ReplaceInfo();
            replaceInfo.OrderId = orderId;
            replaceInfo.Comments = remark;
            replaceInfo.ApplyForTime = DateTime.Now;
            replaceInfo.HandleStatus = 0;
            replaceInfo.ReplaceLineItem = new List<OrderAppFormItems>();

            //赋值退货明细
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
            for (int i = 0; i < skuIds.Length; i++)
            {
                if (orderInfo.LineItems.ContainsKey(skuIds[i]))
                {
                    LineItemInfo lineItem = orderInfo.LineItems[skuIds[i]];
                    OrderAppFormItems replaceLineItem = new OrderAppFormItems();
                    replaceLineItem.SkuId = lineItem.SkuId;
                    replaceLineItem.ApplyType = 2;//退货单类型,2表示为退换类型
                    replaceLineItem.ProductId = lineItem.ProductId;
                    replaceLineItem.Quantity = quantities[i];//退货时间
                    replaceLineItem.ItemAdjustedPrice = lineItem.ItemAdjustedPrice;
                    replaceLineItem.ItemDescription = lineItem.ItemDescription;
                    replaceLineItem.ThumbnailsUrl = lineItem.ThumbnailsUrl;
                    replaceLineItem.ItemWeight = lineItem.ItemWeight;

                    replaceLineItem.SKUContent = lineItem.SKUContent;
                    replaceLineItem.PromotionId = lineItem.PromotionId;
                    replaceLineItem.PromotionName = lineItem.PromotionName;
                    replaceLineItem.TaxRate = lineItem.TaxRate;
                    replaceLineItem.TemplateId = lineItem.TemplateId;
                    replaceLineItem.storeId = lineItem.storeId;
                    replaceLineItem.SupplierId = lineItem.SupplierId;
                    replaceInfo.ReplaceLineItem.Add(replaceLineItem);
                }
            }
            return new ReplaceDao().CreateReturn(replaceInfo);
        }
        public static bool CreateReplaceEntityAndAdd(string orderId, string remark, string strSkuIds, string quantityList, string logisticsCompany, string logisticsId)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(quantityList))
            {
                return false;
            }
            string[] strQuantities = quantityList.Split(',');
            List<int> quantities = new List<int>();
            string[] skuIds = strSkuIds.Split(',');
            for (int i = 0; i < strQuantities.Length; i++)
            {
                try
                {
                    int quantity = int.Parse(strQuantities[i]);
                    if (quantity <= 0)
                    {
                        return false;
                    }
                    quantities.Add(quantity);
                }
                catch
                {
                    return false;
                }
            }
            if (skuIds.Length != quantities.Count)
            {
                return false;
            }
            ReplaceInfo replaceInfo = new ReplaceInfo();
            replaceInfo.OrderId = orderId;
            replaceInfo.Comments = remark;
            replaceInfo.ApplyForTime = DateTime.Now;
            replaceInfo.HandleStatus = 0;
            replaceInfo.LogisticsCompany = logisticsCompany;
            replaceInfo.LogisticsId = logisticsId;
            replaceInfo.ReplaceLineItem = new List<OrderAppFormItems>();

            //赋值退货明细
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
            for (int i = 0; i < skuIds.Length; i++)
            {
                if (orderInfo.LineItems.ContainsKey(skuIds[i]))
                {
                    LineItemInfo lineItem = orderInfo.LineItems[skuIds[i]];
                    OrderAppFormItems replaceLineItem = new OrderAppFormItems();
                    replaceLineItem.SkuId = lineItem.SkuId;
                    replaceLineItem.ApplyType = 2;//退货单类型,2表示为退换类型
                    replaceLineItem.ProductId = lineItem.ProductId;
                    replaceLineItem.Quantity = quantities[i];//退货时间
                    replaceLineItem.ItemAdjustedPrice = lineItem.ItemAdjustedPrice;
                    replaceLineItem.ItemDescription = lineItem.ItemDescription;
                    replaceLineItem.ThumbnailsUrl = lineItem.ThumbnailsUrl;
                    replaceLineItem.ItemWeight = lineItem.ItemWeight;

                    replaceLineItem.SKUContent = lineItem.SKUContent;
                    replaceLineItem.PromotionId = lineItem.PromotionId;
                    replaceLineItem.PromotionName = lineItem.PromotionName;
                    replaceLineItem.TaxRate = lineItem.TaxRate;
                    replaceLineItem.TemplateId = lineItem.TemplateId;
                    replaceLineItem.storeId = lineItem.storeId;
                    replaceLineItem.SupplierId = lineItem.SupplierId;
                    replaceInfo.ReplaceLineItem.Add(replaceLineItem);
                }
            }
            return new ReplaceDao().CreateReturn(replaceInfo);
        }
        public static ReturnsInfo GetReturnsInfo(int returnsId)//获得退货单详情
        {
            return new ReturnDao().GetReturnsInfo(returnsId);
        }
        public static ReplaceInfo GetReplaceInfo(int replaceId)//获得退换单详情
        {
            return new ReplaceDao().GetReplaceInfo(replaceId);
        }
        public static List<string> GetOrderHandleReason(OrderHandleReasonType type)
        {
            return new OrderDao().GetOrderHandleReason(type);
        }


        /// <summary>
        /// 更新退款申请商品入库状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool UpdateRefundOrderReturntatus(string orderId, string remark)
        {
            return new RefundDao().UpdateRefundOrderReturntatus(orderId, remark);
        }
    }

    public static class WMSHelpers
    {
        public static string BuildSign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
        {
            return Sign(CreateLinkstring(dicArray) + key, sign_type, _input_charset);
        }

        public static string CreateLinkstring(Dictionary<string, string> dicArray)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dicArray)
            {
                builder.Append(pair.Key + "=" + pair.Value + "&");
            }
            int length = builder.Length;
            builder.Remove(length - 1, 1);
            return builder.ToString();
        }

        public static Dictionary<string, string> Parameterfilter(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in dicArrayPre)
            {
                if ((((pair.Key.ToLower() != "sign") && (pair.Key.ToLower() != "sign_type")) && (pair.Value != "")) && (pair.Value != null))
                {
                    string key = pair.Key.ToLower();
                    dictionary.Add(key, pair.Value);
                }
            }
            return dictionary;
        }

        public static string PostData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }

        public static string Sign(string prestr, string sign_type, string _input_charset)
        {
            StringBuilder builder = new StringBuilder(0x20);
            if (sign_type.ToUpper() == "MD5")
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return builder.ToString();
        }


        ///<summary>
        /// MD5加密
        /// </summary>
        /// <param name="toCryString">被加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string toCryString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(toCryString, "MD5");
        }

        public static string MD51(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(str));
            string str2 = "";
            for (int i = 0; i < result.Length; i++)
            {
                str2 += string.Format("{0:x}", result[i]);
            }
            return str2;
        }


        /// <summary>
        /// 将字符串使用base64算法加密
        /// </summary>
        /// <param name="SourceString">待加密的字符串</param>
        /// <param name="Ens">System.Text.Encoding 对象，如创建中文编码集对象：
        /// System.Text.Encoding.GetEncoding("gb2312")</param>
        /// <returns>编码后的文本字符串</returns>
        public static string EncodingString(string SourceString, System.Text.Encoding Ens)
        {
            return Convert.ToBase64String(Ens.GetBytes(SourceString));
        }


        /// <summary>
        /// 使用缺省的代码页将字符串使用base64算法加密
        /// </summary>
        /// <param name="SourceString">待加密的字符串</param>
        /// <returns>加密后的文本字符串</returns>
        public static string EncodingString(string SourceString)
        {
            return EncodingString(SourceString, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 记录WMS日志
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="param">参数</param>
        /// <param name="logcontent">日志内容</param>
        /// <param name="logtype">日志类型 info error</param>
        /// <param name="methodtype">方法类型  in out </param>
        /// <returns></returns>
        public static bool SaveLog(string method, string param, string logcontent, string logtype, string methodtype)
        {
            return new WMSLogDao().SaveLog(method, param, logcontent, logtype, methodtype);
        }

    }
}
