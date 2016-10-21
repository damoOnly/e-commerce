using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Promotions;
using EcShop.SqlDal.Promotions;
using System;
using System.Collections.Generic;
namespace EcShop.ControlPanel.Promotions
{
    public static class VoucherHelper
    {

        /// <summary>
        /// 添加现金券
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="count"></param>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        public static VoucherActionStatus CreateVoucher(VoucherInfo voucher, int count, out string lotNumber, int pwdType)
        {
            Globals.EntityCoding(voucher, true);
            return new VoucherDao().CreateVoucher(voucher, count, out lotNumber,pwdType);
        }
        public static DbQueryResult GetVouchersList(VoucherItemInfoQuery voucherquery)
        {
            return new VoucherDao().GetVouchersList(voucherquery);
        }
        public static IList<VoucherItemInfo> GetVoucherItemInfos(string lotNumber)
        {
            return new VoucherDao().GetVoucherItemInfos(lotNumber);
        }
        public static VoucherActionStatus UpdateVoucher(VoucherInfo voucher)
        {
            Globals.EntityCoding(voucher, true);
            return new VoucherDao().UpdateVoucher(voucher);
        }
        public static bool DeleteVoucher(int voucherId)
        {
            return new VoucherDao().DeleteVoucher(voucherId);
        }
        public static VoucherInfo GetVoucher(int voucherId)
        {
            return new VoucherDao().GetVoucherDetails(voucherId);
        }
        public static DbQueryResult GetNewVouchers(Pagination page)
        {
            return new VoucherDao().GetNewVouchers(page);
        }

        /// <summary>
        /// 发送现金券
        /// </summary>
        /// <param name="listCouponItem"></param>
        public static bool SendClaimCodes(IList<VoucherItemInfo> listvoucherItem)
        {
            return new VoucherDao().SendClaimCodes(listvoucherItem);
        }

        /// <summary>
        /// 根据现金券ID获取现金券数量
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public static int GetVoucherItemAmount(int voucherId)
        {
            return new VoucherDao().GetVoucherItemAmount(voucherId);
        }

         //通过发送方式获取现金券列表
        public static IList<VoucherInfo> GetVoucherBySendType(int sendType)
        {
            return new VoucherDao().GetVoucherBySendType(sendType);
        }

        public static int GetVoucherItemsCount(int userId, string keyCode)
        {
            return new VoucherDao().GetVoucherItemsCount(userId, keyCode);
        }

        public static bool BulkAddVoucherItems(IList<VoucherItemInfo> voucherItems)
        {
            return new VoucherDao().BulkAddVoucherItems(voucherItems);
        }

        public static int GetCountVoucherItem(int voucherid, int userId)
        {
            return new VoucherDao().GetCountVoucherItem(voucherid, userId);
        }

        public static int GetCountVoucherItem(int voucherid, string sendOrderId)
        {
            return new VoucherDao().GetCountVoucherItem(voucherid, sendOrderId);
        }

    }
}
