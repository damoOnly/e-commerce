using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.Members;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Sales;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.SaleSystem.Member
{
    using EcShop.Membership.Context;
    using EcShop.SqlDal;
    using EcShop.SqlDal.Promotions;//修改1
	public static class MemberProcessor
	{
         /// <summary>
        /// 检查当前用户是否已认证账号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool CheckUserIsVerify(int userId)
        {
            return new PointDetailDao().CheckUserIsVerify(userId);
        }
		public static int GetDefaultMemberGrade()
		{
			return new MemberGradeDao().GetDefaultMemberGrade();
		}
		public static MemberGradeInfo GetMemberGrade(int gradeId)
		{
			return new MemberGradeDao().GetMemberGrade(gradeId);
		}
		public static CreateUserStatus CreateMember(Member member)
		{
			return Users.CreateUser(member, HiContext.Current.Config.RolesConfiguration.Member);
		}
		public static DbQueryResult GetMySubUsers(MemberQuery query)
		{
			return new MemberDao().GetMySubUsers(query);
		}

        public static DbQueryResult GetMySubUsers(MemberQuery query,int userId)
        {
            return new MemberDao().GetMySubUsers(query, userId);
        }

		public static SubReferralUser GetMyReferralSubUser(int UserId)
		{
			return new MemberDao().GetMyReferralSubUser(UserId);
		}
		public static SubMember GetMySubUser(int UserId)
		{
			return new MemberDao().GetMySubMember(UserId);
		}
		public static DbQueryResult GetMySplittinDetails(BalanceDetailQuery query, bool? isUse)
		{
			return new ReferralDao().GetSplittinDetails(query, isUse);
		}
		public static SplittinDrawInfo GetSplittinDraw(long journalNumber)
		{
			return new ReferralDao().GetSplittinDraw(journalNumber);
		}
		public static SplittinDetailInfo GetSplittinDetail(long JournalNumber)
		{
			return new ReferralDao().GetSplittinDetail(JournalNumber);
		}
        public static bool ReferralRequest(int userId, string realName, string cellPhone, string referralReason, string Email)
		{
			IUser user = Users.GetUser(userId, false);
			bool result;
			if (user != null && user.UserRole == UserRole.Member)
			{
				Member member = user as Member;
				if (member.ReferralStatus != 2)
				{
					if (HiContext.Current.SiteSettings.IsAuditReferral)
					{
						member.ReferralStatus = 1;
					}
					else
					{
						member.ReferralStatus = 2;
						member.ReferralAuditDate = new DateTime?(DateTime.Now);
					}
					member.ReferralRequetsDate = new DateTime?(DateTime.Now);
					member.RealName = realName;
					member.CellPhone = cellPhone;
					member.ReferralReason = referralReason;
                    member.Email = Email;
					result = Users.UpdateUser(member);
					return result;
				}
			}
			result = false;
			return result;
		}
		public static DbQueryResult GetMySplittinDraws(BalanceDrawRequestQuery query, int? auditStatus)
		{
			return new ReferralDao().GetSplittinDraws(query, auditStatus);
		}


        /// <summary>
        /// 获取最近一次提现记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
         public static SplittinDrawInfo GetLatestSplittinDrawInfo(int userid)
        {
            return new ReferralDao().GetLatestSplittinDrawInfo(userid);
        }

        /// <summary>
        /// 获取已提现的金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static decimal GetUserAllSplittinDraws(int userId)
        {
            return new ReferralDao().GetUserAllSplittinDraws(userId);
        }


		public static bool SplittinDrawRequest(SplittinDrawInfo splittinDraw)
		{
			return new ReferralDao().SplittinDrawRequest(splittinDraw);
		}
		public static decimal GetUserAllSplittin(int userId)
		{
			return new ReferralDao().GetUserAllSplittin(userId);
		}
		public static decimal GetUserUseSplittin(int userId)
		{
			return new ReferralDao().GetUserUseSplittin(userId);
		}
		public static decimal GetUserNoUseSplittin(int userId)
		{
			return new ReferralDao().GetUserNoUseSplittin(userId);
		}
		public static DataSet GetUserOrder(int userId, OrderQuery query)
		{
			return new OrderDao().GetUserOrder(userId, query);
		}
        public static DataTable GetUserInvoices(int userId)
        {
            return new OrderDao().GetUserInvoices(userId);
        }
		public static int GetUserOrderCount(int userId, OrderQuery query)
		{
			return new OrderDao().GetUserOrderCount(userId, query);
		}

        public static int AppGetUserOrderCount(int userId, OrderQuery query)
        {
            return new OrderDao().WAPGetUserOrderCount(userId, query);
        }

        public static  int GetWaitCommentOrderCount(int userId)
        {
            return new OrderDao().GetWaitCommentOrderCount(userId);
        }

        public static int WAPGetUserOrderCount(int userId, OrderQuery query)
        {
            return new OrderDao().WAPGetUserOrderCount(userId, query);
        }
		public static LoginUserStatus ValidLogin(Member member)
		{
			LoginUserStatus result;
			if (member == null)
			{
				result = LoginUserStatus.InvalidCredentials;
			}
			else
			{
				result = Users.ValidateUser(member);
			}
			return result;
		}
		public static int AddShippingAddress(ShippingAddressInfo shippingAddress)
		{
			ShippingAddressDao shippingAddressDao = new ShippingAddressDao();
			return shippingAddressDao.AddShippingAddress(shippingAddress);
		}
		public static int GetShippingAddressCount(int UserID)
		{
			return new ShippingAddressDao().GetShippingAddressCount(UserID);
		}
		public static int GetShippingAddressCount()
		{
			return new ShippingAddressDao().GetShippingAddresses(HiContext.Current.User.UserId).Count;
		}
		public static IList<ShippingAddressInfo> GetShippingAddresses()
		{
			return new ShippingAddressDao().GetShippingAddresses(HiContext.Current.User.UserId);
		}
        public static IList<ShippingAddressInfo> GetShippingAddresses(int userId)
        {
            return new ShippingAddressDao().GetShippingAddresses(userId);
        }
		public static ShippingAddressInfo GetDefaultShippingAddress()
		{
			IList<ShippingAddressInfo> shippingAddresses = new ShippingAddressDao().GetShippingAddresses(HiContext.Current.User.UserId);
			ShippingAddressInfo result;
			foreach (ShippingAddressInfo current in shippingAddresses)
			{
				if (current.IsDefault)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}
		public static ShippingAddressInfo GetShippingAddress(int shippingId)
		{
			return new ShippingAddressDao().GetShippingAddress(shippingId);
		}   
		public static bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
		{
			return new ShippingAddressDao().UpdateShippingAddress(shippingAddress);
		}

        public static bool SetDefaultShippingAddressPC(int shippingId,int userId)
        {
            return new ShippingAddressDao().SetDefaultShippingAddressPC(shippingId, userId);
        }
		public static bool SetDefaultShippingAddress(int shippingId, int UserId)
		{
			return new ShippingAddressDao().SetDefaultShippingAddress(shippingId, UserId);
		}
		public static bool DelShippingAddress(int shippingid, int userid)
		{
			return new ShippingAddressDao().DelShippingAddress(shippingid, userid);
		}

		public static OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
		{
			return new OpenIdSettingDao().GetOpenIdSettings(openIdType);
		}
		public static IList<OpenIdSettingsInfo> GetConfigedItems()
		{
			return new OpenIdSettingDao().GetConfigedItems();
		}
        public static decimal GetBalanceAmountByUserId(int userId, string cloumn)
        {
            return new BalanceDetailDao().GetBalanceAmountByUserId(userId, cloumn);
        }
		public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			return new BalanceDetailDao().GetBalanceDetails(query);
		}
		public static bool Recharge(BalanceDetailInfo balanceDetails)
		{
			InpourRequestDao inpourRequestDao = new InpourRequestDao();
			bool flag = inpourRequestDao.IsRecharge(balanceDetails.InpourId);
			if (!flag)
			{
				flag = new BalanceDetailDao().InsertBalanceDetail(balanceDetails);
				inpourRequestDao.RemoveInpourRequest(balanceDetails.InpourId);
			}
			return flag;
		}
		public static bool AddInpourBlance(InpourRequestInfo inpourRequest)
		{
			return new InpourRequestDao().AddInpourBlance(inpourRequest);
		}
		public static InpourRequestInfo GetInpourBlance(string inpourId)
		{
			return new InpourRequestDao().GetInpourBlance(inpourId);
		}
		public static void RemoveInpourRequest(string inpourId)
		{
			new InpourRequestDao().RemoveInpourRequest(inpourId);
		}
		public static void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum)
		{
			new MemberDao().GetStatisticsNum(out noPayOrderNum, out noReadMessageNum, out noReplyLeaveCommentNum);
		}
		public static bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			Globals.EntityCoding(balanceDrawRequest, true);
			bool flag = new BalanceDetailDao().BalanceDrawRequest(balanceDrawRequest);
			if (flag)
			{
				Users.ClearUserCache(HiContext.Current.User);
			}
			return flag;
		}
        public static int GetReturnCount(int userId)
        {
            return new ReturnDao().GetRefundCount(userId);
        }
        public static int GetRefundCount(int userId)
        {
            return new RefundDao().GetRefundCount(userId);
        }
        public static int GetReplaceCount(int userId)
        { 
            return new ReplaceDao().GetReplaceCount(userId);
        }
        public static int GetUserNotReadCoupons(int userId)
        {
            return new CouponDao().GetUserNotReadCoupons(userId);
        }

        public static int GetUserNotReadVouchers(int userId)
        {
            return new CouponDao().GetUserNotReadVouchers(userId);
        }

        public static int GetUserNotReadVoucher(int userId)
        {
            return new VoucherDao().GetUserNotReadVoucher(userId);
        }

        public static int AddOrUpdateShippingAddress(ShippingAddressInfo shippingAddress, int newUserId,out int newShippingId)
        {
            return new ShippingAddressDao().AddOrUpdateShippingAddress(shippingAddress, newUserId,out newShippingId);
        }
        /// <summary>
        /// 将DataTable批量添加到Ecshop_TradeDetails表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool DataTableAddToTradeDetails(DataTable dt, out Exception ex)
        {
            return new Promotions.TradeDetailsDao().ExecuteTransactionScopeInsertToEcshop_TradeDetails(dt, out ex);
        }
        /// <summary>
        /// 删除指定时间段的数据信息AliPay or WxPay
        /// </summary>
        /// <param name="beginTime">开起始时间</param>
        /// <param name="endTiem">结束时间</param>
        /// <param name="detailsType">订单类型</param>
        /// <returns></returns>
        public static bool DeleteEcshop_TradeDetails(DateTime? beginTime, DateTime? endTiem, string detailsType)
        {
            return new Promotions.TradeDetailsDao().DeleteEcshop_TradeDetails(beginTime, endTiem, detailsType);
        }
    }
}
