using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Sales;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace EcShop.Jobs
{
    public class OrderReviewJob : IJob
    {
        private Database database;
        public void Execute(XmlNode node)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.database = DatabaseFactory.CreateDatabase();
            AutoCloseOrders(masterSettings.CloseOrderDays);

            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Orders SET FinishDate = getdate(), OrderStatus = 5 WHERE OrderStatus=3 AND ShippingDate <= @ShippingDate;");
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", System.Data.DbType.DateTime, DateTime.Now.AddDays(-masterSettings.FinishOrderDays));
            this.database.ExecuteNonQuery(sqlStringCommand);
            this.ProcessorSplittin(masterSettings);

            string setdate = GetOrderSendCoupontime();

            IList<OrderInfo> orderList = OrderHelper.GetOrderList(setdate, 10);
            if (orderList.Count > 0)
            {

                this.OrderSendCoupon(orderList);

                this.OrderSendVoucher(orderList);
                string enddate = orderList[orderList.Count - 1].FinishDate.ToString();
                UpdateOrderSendCoupontime(enddate);
            }
        }

        /// <summary>
        /// 关闭到期未付款的订单（还原库存、优惠券，现金券）
        /// </summary>
        /// <param name="closeOrderDays"></param>
        private void AutoCloseOrders(int closeOrderDays)
        {
            List<string> orderIdList = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId FROM dbo.Ecshop_Orders  WHERE OrderStatus=1 AND OrderDate <= @OrderDate;");
            this.database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddHours(-closeOrderDays));

            IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
            while (dataReader.Read())
            {
                if (dataReader["OrderId"] == null || string.IsNullOrEmpty(dataReader["OrderId"].ToString()))
                {
                    continue;
                }

                orderIdList.Add(dataReader["OrderId"].ToString());
            }

            foreach (string orderId in orderIdList)
            {
                bool result = TradeHelper.CloseOrder(orderId, CloseOrderType.Auto);
                if (!result)
                {
                    ErrorLog.Write("【到期未付款】系统未能关闭订单：" + orderId);
                }
            }
        }

        private void ProcessorSplittin(SiteSettings siteSettings)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_SplittinDetails WHERE IsUse = 'false' AND OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderStatus=5 AND FinishDate <= @FinishDate)");
            this.database.AddInParameter(sqlStringCommand, "FinishDate", System.Data.DbType.DateTime, DateTime.Now.AddDays((double)(-(double)siteSettings.EndOrderDays)));
            IList<SplittinDetailInfo> list = null;
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                list = ReaderConvert.ReaderToList<SplittinDetailInfo>(dataReader);
            }
            if (list != null)
            {
                foreach (SplittinDetailInfo current in list)
                {
                    current.IsUse = true;
                    current.Balance = current.Income.Value + this.GetUserUseSplittin(current.UserId);
                    if (this.AddSplittinDetail(current))
                    {
                        this.RemoveNoUseSplittin(current.OrderId);
                    }
                }
            }
        }
        private bool AddSplittinDetail(SplittinDetailInfo splittinDetail)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SplittinDetails (OrderId, UserId, UserName, IsUse, TradeDate, TradeType, SubUserId, Income, Expenses, Balance, Remark) VALUES (@OrderId, @UserId, @UserName, @IsUse, @TradeDate, @TradeType, @SubUserId, @Income, @Expenses, @Balance, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, splittinDetail.OrderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, splittinDetail.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, splittinDetail.UserName);
            this.database.AddInParameter(sqlStringCommand, "IsUse", System.Data.DbType.Boolean, splittinDetail.IsUse);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, splittinDetail.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, splittinDetail.TradeType);
            this.database.AddInParameter(sqlStringCommand, "SubUserId", System.Data.DbType.Int32, splittinDetail.SubUserId);
            this.database.AddInParameter(sqlStringCommand, "Income", System.Data.DbType.Currency, splittinDetail.Income);
            this.database.AddInParameter(sqlStringCommand, "Expenses", System.Data.DbType.Currency, splittinDetail.Expenses);
            this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, splittinDetail.Balance);
            this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, splittinDetail.Remark);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        private decimal GetUserUseSplittin(int userId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 Balance FROM Ecshop_SplittinDetails WHERE IsUse = 'true' AND UserId =  @UserId ORDER BY JournalNumber DESC");
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            decimal result;
            if (obj != null && obj != DBNull.Value)
            {
                result = (decimal)obj;
            }
            else
            {
                result = 0m;
            }
            return result;
        }
        public bool RemoveNoUseSplittin(string orderId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM  Ecshop_SplittinDetails WHERE IsUse = 'false' AND OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        private void OrderSendVoucher(IList<OrderInfo> orderList)
        {
            IList<VoucherInfo> voucherList = VoucherHelper.GetVoucherBySendType(1);

            IList<VoucherItemInfo> list = new System.Collections.Generic.List<VoucherItemInfo>();

            string claimCode = string.Empty;
            string password = string.Empty;
            if (voucherList.Count > 0)
            {
                foreach (OrderInfo orderInfo in orderList)
                {
                    foreach (VoucherInfo voucher in voucherList)
                    {
                        VoucherItemInfo item = new VoucherItemInfo();
                        ///不存在拆单
                        if (String.IsNullOrEmpty(orderInfo.SourceOrderId))
                        {
                            if (VoucherHelper.GetCountVoucherItem(voucher.VoucherId, orderInfo.OrderId) > 0)
                            {
                                continue;
                            }
                            else
                            {
                                if (orderInfo.Amount >= Convert.ToDecimal(voucher.SendTypeItem))
                                {
                                    claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                    claimCode = Sign(claimCode, "UTF-8").Substring(8, 16);
                                    password = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                    item = new VoucherItemInfo(orderInfo.OrderId, voucher.VoucherId, claimCode, password, new int?(orderInfo.UserId), orderInfo.Username, orderInfo.EmailAddress, System.DateTime.Now, DateTime.Now.AddDays(voucher.Validity));
                                    list.Add(item);
                                }
                            }
                        }
                        //拆单
                        else
                        {
                            if (VoucherHelper.GetCountVoucherItem(voucher.VoucherId, orderInfo.SourceOrderId) > 0)
                            {
                                continue;
                            }
                            else
                            {
                                decimal sourceAmount = OrderHelper.GetOrderAmount(orderInfo.SourceOrderId);
                                if (sourceAmount >= Convert.ToDecimal(voucher.SendTypeItem))
                                {
                                    claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                    claimCode = Sign(claimCode, "UTF-8").Substring(8, 16);
                                    password = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                    item = new VoucherItemInfo(orderInfo.SourceOrderId, voucher.VoucherId, claimCode, password, new int?(orderInfo.UserId), orderInfo.Username, orderInfo.EmailAddress, System.DateTime.Now, DateTime.Now.AddDays(voucher.Validity));
                                    list.Add(item);
                                }
                            }
                        }
                    }


                }

            }

            if (list.Count > 0)
            {
                try
                {
                    VoucherHelper.SendClaimCodes(list);


                }

                catch
                {

                }
            }

        }

        private void OrderSendCoupon(IList<OrderInfo> orderList)
        {
            IList<CouponInfo> couponList = CouponHelper.GetCouponsBySendType(1);

            IList<CouponItemInfo> list = new System.Collections.Generic.List<CouponItemInfo>();

            string claimCode = string.Empty;
            if (couponList.Count > 0)
            {
                foreach (OrderInfo orderInfo in orderList)
                {
                    foreach (CouponInfo coupon in couponList)
                    {
                        CouponItemInfo item = new CouponItemInfo();
                        if (String.IsNullOrEmpty(orderInfo.SourceOrderId))
                        {
                            if (CouponHelper.GetCountCouponItem(coupon.CouponId, orderInfo.OrderId) > 0)
                            {
                                continue;
                            }
                            else
                            {
                                if (orderInfo.Amount >= Convert.ToDecimal(coupon.SendTypeItem))
                                {
                                    claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                    item = new CouponItemInfo(coupon.CouponId, claimCode, new int?(orderInfo.UserId), orderInfo.Username, orderInfo.EmailAddress, System.DateTime.Now, orderInfo.OrderId);
                                    list.Add(item);
                                }
                            }
                        }

                        else
                        {
                            if (CouponHelper.GetCountCouponItem(coupon.CouponId, orderInfo.SourceOrderId) > 0)
                            {
                                continue;
                            }
                            else
                            {
                                decimal sourceAmount = OrderHelper.GetOrderAmount(orderInfo.SourceOrderId);
                                if (sourceAmount >= Convert.ToDecimal(coupon.SendTypeItem))
                                {
                                    claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                    item = new CouponItemInfo(coupon.CouponId, claimCode, new int?(orderInfo.UserId), orderInfo.Username, orderInfo.EmailAddress, System.DateTime.Now, orderInfo.SourceOrderId);
                                    list.Add(item);
                                }
                            }
                        }
                    }


                }

            }

            if (list.Count > 0)
            {
                try
                {
                    CouponHelper.SendClaimCodes(list);


                }

                catch
                {

                }
            }
        }

        protected string GetOrderSendCoupontime()
        {
            string setdate = string.Empty;
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("select Value FROM Ecshop_SiteSetting WHERE [Key]= 'OrderSendCoupontime';");
            object a = database.ExecuteScalar(sqlStringCommand);
            if (a == null)
            {
                setdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sqlStringCommand = database.GetSqlStringCommand("insert into Ecshop_SiteSetting([Key],[Value]) values('OrderSendCoupontime',@Value)");
                database.AddInParameter(sqlStringCommand, "Value", DbType.String, setdate);
                database.ExecuteNonQuery(sqlStringCommand);
            }
            else
            {
                setdate = a.ToString();
            }

            return setdate;
        }

        protected void UpdateOrderSendCoupontime(string value)
        {
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("update  Ecshop_SiteSetting set Value=@Value  WHERE [Key]= 'OrderSendCoupontime';");
            database.AddInParameter(sqlStringCommand, "Value", DbType.String, value);
            database.ExecuteNonQuery(sqlStringCommand);

        }

        /// <summary>
        /// MD5签名
        /// </summary>
        /// <param name="prestr"></param>
        /// <param name="_input_charset"></param>
        /// <returns></returns>
        public static string Sign(string prestr, string _input_charset)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(32);
            System.Security.Cryptography.MD5 mD = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(System.Text.Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
            }

            return stringBuilder.ToString();
        }
    }
}
