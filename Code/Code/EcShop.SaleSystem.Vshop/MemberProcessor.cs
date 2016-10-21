using EcShop.Entities.Orders;
using EcShop.SqlDal.Members;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Promotions;
using System;
using System.Data;
namespace EcShop.SaleSystem.Vshop
{
	public static class MemberProcessor
	{
		public static int GetUserHistoryPoint(int userId)
		{
			return new PointDetailDao().GetHistoryPoint(userId);
		}
		public static bool ConfirmOrderFinish(OrderInfo order)
		{
			bool result = false;
			if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
			{
				order.OrderStatus = OrderStatus.Finished;
				//order.PayDate = DateTime.Now;    //完成时不要更新支付时间
				order.FinishDate = DateTime.Now;
				result = new OrderDao().UpdateOrder(order, null);
			}
			return result;
		}
        public static bool ConfirmOrderCancel(OrderInfo order)
        {
            bool result = false;
            if (order.CheckAction(OrderActions.BUYER_CANCEL))
            {
                result = new OrderDao().CloseOrder(order.OrderId, CloseOrderType.Manually);
            }
            return result;
        }
		public static DataTable GetUserCoupons(int userId, int useType = 0)
		{
			return new CouponDao().GetUserCoupons(userId, useType);
		}

        public static DataTable GetUserVoucher(int userId, int useType = 0)
        {
            return new VoucherDao().GetUserVoucher(userId, useType);
        }
        public static DataSet GetUserOrder(int userId, OrderQuery query, int delaytimes = 30)
		{
            return new OrderDao().GetUserOrder(userId, query, delaytimes);
		}

        public static DataSet GetWAPUserOrder(int userId, OrderQuery query)
        {
            return new OrderDao().GetWAPUserOrder(userId, query);
		
        }
        public static int UpdateCouponsReaded(int userId)
        {
            return new CouponDao().UpdateCouponsReaded(userId);
        }

        public static int UpdateVoucherReaded(int userId)
        {
            return new VoucherDao().UpdateVoucherReaded(userId);
        }

	}
}
