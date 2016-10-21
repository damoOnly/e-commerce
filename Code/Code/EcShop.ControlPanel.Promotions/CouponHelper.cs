using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Promotions;
using EcShop.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.ControlPanel.Promotions
{
    public static class CouponHelper
    {
        public static CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().CreateCoupon(coupon, count, out lotNumber);
        }

        public static CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber, out int couponId)
        {

            Globals.EntityCoding(coupon, true);
            return new CouponDao().CreateCoupon(coupon, count, out lotNumber, out couponId);
        }
        public static DbQueryResult GetCouponsList(CouponItemInfoQuery couponquery)
        {
            return new CouponDao().GetCouponsList(couponquery);
        }
        public static DbQueryResult GetCouponsListToExport(CouponItemInfoQuery couponquery)
        {
            return new CouponDao().GetCouponsListToExport(couponquery);
        }
        public static IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            return new CouponDao().GetCouponItemInfos(lotNumber);
        }

        /// <summary>
        /// 通过发送方式获取优惠券列表
        /// </summary>
        /// <param name="sendType"></param>
        /// <returns></returns>
        public static IList<CouponInfo> GetCouponsBySendType(int sendType)
        {
            return new CouponDao().GetCouponsBySendType(sendType);
        }

        public static CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().UpdateCoupon(coupon);
        }

        public static CouponActionStatus NewUpdateCoupon(CouponInfo coupon)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().NewUpdateCoupon(coupon);
        }
        public static bool DeleteCoupon(int couponId)
        {
            return new CouponDao().DeleteCoupon(couponId);
        }
        public static CouponInfo GetCoupon(int couponId)
        {
            return new CouponDao().GetCouponDetails(couponId);
        }
        public static DbQueryResult GetNewCoupons(Pagination page)
        {
            return new CouponDao().GetNewCoupons(page);
        }

        /// <summary>
        /// 供应商满劵活动
        /// </summary>
        /// <param name="UseType">使用类型4</param>
        /// <param name="SendType">发送类型1</param>
        /// <param name="supplierid">供应商ID</param>
        /// <returns></returns>
        public static DataTable GetSupplierCoupon(int UseType, int SendType, int SupplierId)
        {
            return new CouponDao().GetSupplierCoupon(UseType, SendType, SupplierId);
        }

        public static bool SendClaimCodes(IList<CouponItemInfo> listCouponItem)
        {
            bool Flag = false;
            Database database = DatabaseFactory.CreateDatabase();
            using (System.Data.Common.DbConnection dbCon = database.CreateConnection())
            {
                dbCon.Open();
                System.Data.Common.DbTransaction dbTran = dbCon.BeginTransaction();
                try
                {
                    new CouponDao().SendClaimCodesByTran(listCouponItem, dbTran);
                }
                catch
                {
                    dbTran.Rollback();
                    Flag = false;
                }
                finally
                {
                    dbCon.Close();
                }
                Flag = true;
            }
            return Flag;
        }

        public static bool SendClaimCodes(CouponItemInfo couponItemInfo)
        {
            return new CouponDao().SendClaimCodes(couponItemInfo);
        }
        public static bool SendClaimCodesList(CouponItemInfo couponItemInfo, int count)
        {
            bool result = false;
            Database database = DatabaseFactory.CreateDatabase();
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        new CouponDao().SendClaimCodes(couponItemInfo, dbTransaction);
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
                result = true;
            }
            return result;
        }

        public static bool SendClaimCodes(int couponId, string memberNames, int? gradeId, string remark, bool isSendSms, int createdBy, string createdByName, string StartTime, string ClosingTime, decimal Amount, decimal DiscountValue, out int count)
        {
            return new CouponDao().SendClaimCodes(couponId, memberNames, gradeId, remark, isSendSms, createdBy, createdByName,  StartTime,  ClosingTime,  Amount,  DiscountValue, out count);
        }

        public static int CreateCouponsSendTypeItem(CouponsSendTypeItem item)
        {
            return new CouponDao().CreateCouponsSendTypeItem(item);
        }

        public static IList<CouponsSendTypeItem> GetCouponsSendTypeItems(int couponid)
        {
            return new CouponDao().GetCouponsSendTypeItems(couponid);
        }

        public static bool DeleteCouponSendTypeItem(int couponId)
        {
            return new CouponDao().DeleteCouponSendTypeItem(couponId);
        }

        public static int GetCountCouponItem(int couponid, int userId)
        {
            return new CouponDao().GetCountCouponItem(couponid, userId);
        }

        /// <summary>
        /// 已经领取的优惠卷
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="sendtype">发送方式</param>
        /// <returns></returns>
        public static int GetCountCouponItemed(int userId, int sendtype)
        {
            return new CouponDao().GetCountCouponItemed(userId, sendtype);
        }

        public static int GetCountCouponItem(int couponid, string sendorderid)
        {
            return new CouponDao().GetCountCouponItem(couponid, sendorderid);
        }

        public static CouponInfo GetCouponByName(string couponName)
        {
            return new CouponDao().GetCouponByName(couponName);
        }

        /// <summary>
        /// 根据特定的时间获取需要提醒的优惠券
        /// </summary>
        /// <param name="curDate"></param>
        /// <returns></returns>
        public static DataTable GetAllCountDownCoupons(DateTime curDate)
        {
            return new CouponDao().GetAllCountDownCoupons(curDate);
        }
    }
}
