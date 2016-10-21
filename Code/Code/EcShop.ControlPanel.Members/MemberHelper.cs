using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Members;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.Members;
using Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
namespace EcShop.ControlPanel.Members
{
	public static class MemberHelper
	{
		public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			return new MemberGradeDao().HasSamePointMemberGrade(memberGrade);
		}
		public static bool CreateMemberGrade(MemberGradeInfo memberGrade)
		{
			bool result;
			if (null == memberGrade)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(memberGrade, true);
				bool flag = new MemberGradeDao().CreateMemberGrade(memberGrade);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.AddMemberGrade, string.Format(CultureInfo.InvariantCulture, "添加了名为 “{0}” 的会员等级", new object[]
					{
						memberGrade.Name
					}));
				}
				result = flag;
			}
			return result;
		}
		public static bool UpdateMemberGrade(MemberGradeInfo memberGrade)
		{
			bool result;
			if (null == memberGrade)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(memberGrade, true);
				bool flag = new MemberGradeDao().UpdateMemberGrade(memberGrade);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditMemberGrade, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员等级", new object[]
					{
						memberGrade.GradeId
					}));
				}
				result = flag;
			}
			return result;
		}
		public static void SetDefalutMemberGrade(int gradeId)
		{
			new MemberGradeDao().SetDefalutMemberGrade(gradeId);
		}
		public static bool DeleteMemberGrade(int gradeId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMemberGrade);
			bool flag = new MemberGradeDao().DeleteMemberGrade(gradeId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员等级", new object[]
				{
					gradeId
				}));
			}
			return flag;
		}
		public static IList<MemberGradeInfo> GetMemberGrades()
		{
			return new MemberGradeDao().GetMemberGrades();
		}
		public static MemberGradeInfo GetMemberGrade(int gradeId)
		{
			return new MemberGradeDao().GetMemberGrade(gradeId);
		}
		public static DbQueryResult GetMembers(MemberQuery query)
		{
			return new MemberDao().GetMembers(query);
		}
		public static System.Data.DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
		{
			return new MemberDao().GetMembersNopage(query, fields);
		}
   
		public static Member GetMember(int userId)
		{
			IUser user = Users.GetUser(userId, false);
			Member result;
			if (user != null && user.UserRole == UserRole.Member)
			{
				result = (user as Member);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public static bool Delete(int userId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			IUser user = Users.GetUser(userId);
			bool flag = new MemberDao().Delete(userId);
			if (flag)
			{
				Users.ClearUserCache(user);
				EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员", new object[]
				{
					userId
				}));
			}
			return flag;
		}
		public static bool Update(Member member)
		{
			bool flag = Users.UpdateUser(member);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditMember, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员", new object[]
				{
					member.UserId
				}));
			}
			return flag;
		}
		public static DbQueryResult GetReferrals(MemberQuery query)
		{
			return new ReferralDao().GetReferrals(query);
		}
		public static DbQueryResult GetSplittinDraws(BalanceDrawRequestQuery query, int? auditStatus)
		{
			return new ReferralDao().GetSplittinDraws(query, auditStatus);
		}
		public static DbQueryResult GetSplittinDetails(BalanceDetailQuery query)
		{
			return new ReferralDao().GetSplittinDetails(query, null);
		}
		public static bool AccepteDraw(long journalNumber, string managerRemark)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					ReferralDao referralDao = new ReferralDao();
					if (!referralDao.AccepteDraw(journalNumber, managerRemark))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						SplittinDrawInfo splittinDraw = referralDao.GetSplittinDraw(journalNumber);
						if (splittinDraw == null)
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!referralDao.AddSplittinDetail(new SplittinDetailInfo
							{
								OrderId = string.Empty,
								UserId = splittinDraw.UserId,
								UserName = splittinDraw.UserName,
								IsUse = true,
								TradeDate = DateTime.Now,
								TradeType = SplittingTypes.DrawRequest,
								Expenses = new decimal?(splittinDraw.Amount),
								Balance = referralDao.GetUserUseSplittin(splittinDraw.UserId) - splittinDraw.Amount,
								Remark = managerRemark
							}))
							{
								dbTransaction.Rollback();
								result = false;
							}
							else
							{
								dbTransaction.Commit();
								result = true;
							}
						}
					}
				}
				catch (Exception var_6_11F)
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
		public static bool AccepteRerralRequest(int userId)
		{
			IUser user = Users.GetUser(userId, false);
			bool result;
			if (user != null && user.UserRole == UserRole.Member)
			{
				Member member = user as Member;
				if (member.ReferralStatus == 1)
				{
					member.ReferralStatus = 2;
					member.ReferralAuditDate = new DateTime?(DateTime.Now);
					result = Users.UpdateUser(member);
					return result;
				}
			}
			result = false;
			return result;
		}
		public static bool RefuseRerralRequest(int userId, string refusalReason)
		{
			IUser user = Users.GetUser(userId, false);
			bool result;
			if (user != null && user.UserRole == UserRole.Member)
			{
				Member member = user as Member;
				if (member.ReferralStatus == 1)
				{
					member.ReferralStatus = 3;
					member.RefusalReason = refusalReason;
					result = Users.UpdateUser(member);
					return result;
				}
			}
			result = false;
			return result;
		}
		public static DbQueryResult GetMemberBlanceList(MemberQuery query)
		{
			return new BalanceDetailDao().GetMemberBlanceList(query);
		}
		public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			return new BalanceDetailDao().GetBalanceDetails(query);
		}

        /// 获取应付总汇
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet GetReport(ReconciliationOrdersQuery query)
        {
            return new ReconciliationOrdersDao().GetReport(query);
        }
        /// <summary>
        /// 获取对账订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetReconciliationOrders(ReconciliationOrdersQuery query)
        {
            return new ReconciliationOrdersDao().GetReconciliationOrders(query);
        }
        /// <summary>
        /// 导出对账订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult ExportReconciliationOrders(ReconciliationOrdersQuery query)
        {
            return new ReconciliationOrdersDao().ExportReconciliationOrders(query);
        }

        /// <summary>
        /// 获取对账订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetReconciliationOrdersDetailed(ReconciliationOrdersQuery query)
        {
            return new ReconciliationOrdersDao().GetReconciliationOrdersDetailed(query);
        }
        /// <summary>
        /// 获取快递费用报表
        /// </summary>
        /// <returns></returns>
        public static DbQueryResult GetReconciliationExpressFeeDetails(ReconciliationExpressFeeQuery query)
        {
            return new ReconciliationOrdersDao().GetReconciliationExpressFeeDetails(query);
        }
        /// <summary>
        /// 导出对账订单商品明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult ExportReconciliationOrdersDetailed(ReconciliationOrdersQuery query)
        {
            return new ReconciliationOrdersDao().ExportReconciliationOrdersDetailed(query);
        }


        public static DbQueryResult GetVendorSalesDetail(VendorSalesDetailQuery query)
        {
            return new BalanceDetailDao().GetVendorSalesDetail(query);
        }
        public static DbQueryResult GetVendorSalesReport(VendorSalesReportQuery query)
        {
            return new BalanceDetailDao().GetVendorSalesReport(query);
        }
		public static DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			return new BalanceDetailDao().GetBalanceDetailsNoPage(query);
		}
		public static DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
		{
			return new BalanceDetailDao().GetBalanceDrawRequests(query);
		}
		public static DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query)
		{
			return new BalanceDetailDao().GetBalanceDrawRequestsNoPage(query);
		}
		public static bool AddBalance(BalanceDetailInfo balanceDetails, decimal money)
		{
			bool result;
			if (null == balanceDetails)
			{
				result = false;
			}
			else
			{
				bool flag = new BalanceDetailDao().InsertBalanceDetail(balanceDetails);
				if (flag)
				{
					Users.ClearUserCache(Users.GetUser(balanceDetails.UserId));
				}
				EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"添加预付款\"{1}\"", new object[]
				{
					balanceDetails.UserName,
					money
				}));
				result = flag;
			}
			return result;
		}
        /// <summary>
        /// 判断预付款充值订单是否存在，防止重复提交。
        /// </summary>
        /// <param name="tradeNo"></param>
        /// <returns></returns>
        public static bool IsTradeNoExists(string tradeNo)
        {
            return new BalanceDetailDao().IsTradeNoExists(tradeNo);
        }
		public static bool DealBalanceDrawRequest(int userId, bool agree)
		{
			bool flag = new BalanceDetailDao().DealBalanceDrawRequest(userId, agree);
			if (flag)
			{
                EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "会员\"{0}\"预付款提现申请操作成功", new object[]
				{
					userId
				}));
				Users.ClearUserCache(Users.GetUser(userId));
			}
			return flag;
		}
		public static bool InsertClientSet(Dictionary<int, MemberClientSet> clientset)
		{
			return new MemberDao().InsertClientSet(clientset);
		}
		public static Dictionary<int, MemberClientSet> GetMemberClientSet()
		{
			return new MemberDao().GetMemberClientSet();
		}
        public static int GetDefaultMemberGrade()
        {
            return new MemberGradeDao().GetDefaultMemberGrade();
        }

        public static DbQueryResult GetMembersAddress(MemberQuery query)
        {
            return new MemberDao().GetMembersAddress(query);
        }
        /// <summary>
        /// 验证失败累计次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool AddIsVerifyMsg(int userId)
        {
            return new MemberDao().AddIsVerifyMsg(userId); 
        }
         /// <summary>
        /// 获取当前用户当前的登录的失败次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int CheckErrorCount(int userId)
        {
            return new MemberDao().CheckErrorCount(userId);
        }
        // /// <summary>
        ///// 验证成功清楚失败纪录
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public static bool ClearErrCount(int userId)
        //{
        //    return new MemberDao().ClearErrCount(userId);
        //}

        /// <summary>
        /// 根据用户ID获取其邀请码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetRecommendCodeByUserId(int userId)
        {
            return new MemberDao().GetRecommendCodeByUserId(userId);
        }

         /// <summary>
        /// 根据邀请码获取其USERID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserIdByRecommendCode(string code)
        {
            return new MemberDao().GetUserIdByRecommendCode(code);
        }
        /// <summary>
        /// 生成邀请码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string AddRecommendCodeByUserId(int userId)
        {
            return new MemberDao().AddRecommendCodeByUserId(userId);
        }

        /// <summary>
        /// 添加邀请码使用记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="useredid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool AddRecommendCodeRecord(int userId,int useredId,string code,string systemcode)
        {
            return new MemberDao().AddRecommendCodeRecord(userId, useredId, code, systemcode);
        }
        public static bool UpdateUserNameCoupon(Member member,string recemmendCode,out bool IsSendCoupon)
        {
            IsSendCoupon = false;
            bool flag = false;
            Database database = DatabaseFactory.CreateDatabase();
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    // 判断是否有邀请码
                    if (!string.IsNullOrWhiteSpace(recemmendCode))
                    {
                        string sourcechars = ConfigurationManager.AppSettings["sourcechars"];
                        string newsourcechars = ConfigurationManager.AppSettings["newsourcechars"];
                        if (!string.IsNullOrWhiteSpace(sourcechars) && !string.IsNullOrWhiteSpace(newsourcechars))
                        {
                            string currcode = BaseConvertHelper.BaseConvert(recemmendCode, newsourcechars, sourcechars);
                            string useredId = MemberHelper.GetUserIdByRecommendCode(currcode);
                            if (!string.IsNullOrWhiteSpace(useredId) && !string.IsNullOrWhiteSpace(currcode))
                            {
                                // 插入到邀请码记录表
                                if (!MemberHelper.AddRecommendCodeRecord(member.UserId, Convert.ToInt32(useredId), currcode, recemmendCode))
                                {
                                    dbTransaction.Rollback();
                                    flag = false;
                                    return flag;
                                }
                            }
                        }
                    }
                    bool CouponFlag = true;
                    // 判断该手机号码是否有openid
                    if (!UserHelper.IsExistOpendByUserName(member.Username))
                    {
                        CouponFlag = false;
                    }
                    //if (Users.UpdateUser(member))
                    if (!UserHelper.UpdateUserUserNameByCellPhone(member.UserId, member.Username, member.CellPhone, member.Password, member.OpenId, 1, UserHelper.CreateSalt()))
                    {
                        dbTransaction.Rollback();
                        flag = false;
                        return flag;
                    }
                    if (CouponFlag)
                    {
                        // 判断是否有优惠卷
                        int count = CouponHelper.GetCountCouponItemed(member.UserId, 5);
                        if (count == 0)
                        {
                            // 获取有效时间内注册赠劵的数量
                            IList<CouponInfo> couponList = CouponHelper.GetCouponsBySendType(5);
                            IList<CouponItemInfo> couponItemList = new List<CouponItemInfo>();
                            string claimCode = string.Empty;
                            if (couponList != null && couponList.Count > 0)
                            {
                                foreach (CouponInfo coupon in couponList)
                                {
                                    CouponItemInfo item = new CouponItemInfo();
                                    claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                    
                                    if (coupon.Validity > 0 && member.CreateDate >= coupon.StartTime)
                                    {
                                        coupon.StartTime = member.CreateDate;
                                        coupon.ClosingTime = member.CreateDate.AddDays(coupon.Validity);
                                    }
                                    else if (coupon.Validity > 0 && member.CreateDate < coupon.StartTime)
                                    {
                                        coupon.StartTime = DateTime.Now;
                                        coupon.ClosingTime = DateTime.Now.AddDays(coupon.Validity);
                                    }
                                    item = new CouponItemInfo(coupon.CouponId, claimCode, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, coupon.StartTime.Date, coupon.ClosingTime.Date.AddDays(1).AddSeconds(-1), coupon.Amount, coupon.DiscountValue);
                                    couponItemList.Add(item);
                                }
                            }
                            if (couponItemList.Count > 0)
                            {
                                // 发劵
                                if (!CouponHelper.SendClaimCodes(couponItemList))
                                {
                                    dbTransaction.Rollback();
                                    flag = false;
                                    return flag;
                                }
                                else {
                                    IsSendCoupon = true;
                                }
                            }

                        }
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
            return flag;
        }

        /// <summary>
        /// 新用户注册赠送优惠券
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool NewUserSendRegisterCoupon(Member member)
        {
            ErrorLog.Write("Begin优惠券");
            bool Flag = true;
            int count = CouponHelper.GetCountCouponItemed(member.UserId, 5);
            if (count == 0)
            {
                // 获取有效时间内注册赠劵的数量
                IList<CouponInfo> couponList = CouponHelper.GetCouponsBySendType(5);
                IList<CouponItemInfo> couponItemList = new List<CouponItemInfo>();
                string claimCode = string.Empty;
                if (couponList != null && couponList.Count > 0)
                {
                    foreach (CouponInfo coupon in couponList)
                    {
                        ErrorLog.Write("开始进入优惠券1：Validity:" + coupon.Validity + ";Createdate:" + member.CreateDate + ";StartTime:" + coupon.StartTime + "");
                        CouponItemInfo item = new CouponItemInfo();
                        claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                        if (coupon.Validity > 0 && member.CreateDate >= coupon.StartTime)
                        {
                            ErrorLog.Write("开始进入优惠券2：Validity:" + coupon.Validity + ";Createdate:" + member.CreateDate + ";StartTime:" + coupon.StartTime + "");
                            coupon.StartTime = member.CreateDate;
                            coupon.ClosingTime = member.CreateDate.AddDays(coupon.Validity);
                        }
                        else if (coupon.Validity > 0 && member.CreateDate < coupon.StartTime)
                        {
                            coupon.StartTime = DateTime.Now;
                            coupon.ClosingTime = DateTime.Now.AddDays(coupon.Validity);
                        }
                        ErrorLog.Write("开始进入优惠券3：Validity:" + coupon.Validity + ";StartTime:" + coupon.StartTime + ";ClosingTime:" + coupon.ClosingTime + "");
                        item = new CouponItemInfo(coupon.CouponId, claimCode, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, coupon.StartTime.Date, coupon.ClosingTime.Date.AddDays(1).AddSeconds(-1), coupon.Amount, coupon.DiscountValue);
                        couponItemList.Add(item);
                    }
                }
                if (couponItemList.Count > 0)
                {
                    Flag = CouponHelper.SendClaimCodes(couponItemList);
                }
                ErrorLog.Write("End优惠券");
            }
            return Flag;
        }

        /// <summary>
        /// 获取邀请码
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetRecemmendCode(int userid)
        {
            // 判断是否有邀请码，没有就给与，有就用原来的
            int code = MemberHelper.GetRecommendCodeByUserId(userid);
            string sourcechars = ConfigurationManager.AppSettings["sourcechars"];
            string newsourcechars = ConfigurationManager.AppSettings["newsourcechars"];
            ErrorLog.Write("code" + code);
            if (code < 50000) // 没有生成新的邀请码
            {
                string currcode = MemberHelper.AddRecommendCodeByUserId(userid);
                if (!string.IsNullOrWhiteSpace(currcode))
                {
                    return BaseConvertHelper.BaseConvert(currcode, sourcechars, newsourcechars);
                }
            }
            else
            {
                return  BaseConvertHelper.BaseConvert(code.ToString(), sourcechars, newsourcechars);
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断邀请码是否存在，如果是新注册用户，userid为0
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsExsitRecommendCode(string code,int userid)
        {
            bool flag=false;

             if (!string.IsNullOrWhiteSpace(code))
             {
                 string sourcechars = ConfigurationManager.AppSettings["sourcechars"];
                 string newsourcechars = ConfigurationManager.AppSettings["newsourcechars"];
                 code = BaseConvertHelper.BaseConvert(code, newsourcechars, sourcechars);
                 flag=new MemberDao().IsExsitRecommendCode(code,userid);
             }

             return flag;
        }

        /// <summary>
        /// 获取所有会员的手机号码
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllUserCellPhones()
        {
            return new MemberDao().GetAllUserCellPhones();
        }


        public static MembersUser GetMembersUser(int userId)
        {
            return new MemberDao().GetMembersUser(userId);
        }

        public static MemberRecommendCodeInfo GetRecommendCodeInfo(int userId)
        {
            return new MemberDao().GetRecommendCodeInfo(userId);
        }
	}
}
