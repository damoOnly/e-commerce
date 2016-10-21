using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.Comments;
using EcShop.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.ControlPanel.Promotions
{
	public static class PromoteHelper
	{
        public static System.Data.DataTable GetPromotions(bool isProductPromote, bool isWholesale, int? supplierId = null)
		{
            return new PromotionDao().GetPromotions(isProductPromote, isWholesale, supplierId);
		}
        public static System.Data.DataTable GetPromotions(ProductPromotionsQuery query,out int count)
        {
            return new PromotionDao().GetPromotions(query, out count);
        }
		public static PromotionInfo GetPromotion(int activityId)
		{
			return new PromotionDao().GetPromotion(activityId);
		}
		public static IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
		{
			return new PromotionDao().GetPromoteMemberGrades(activityId);
		}
		public static System.Data.DataTable GetPromotionProducts(int activityId)
		{
			return new PromotionDao().GetPromotionProducts(activityId);
		}

        public static int GetActivityProductAmount(int activityId)
        {
            return new PromotionDao().GetActivityProductAmount(activityId);
        }
		public static PromotionInfo GetPromotionByProduct(int productId)
		{
			PromotionInfo result = null;
			int? activeIdByProduct = new PromotionDao().GetActiveIdByProduct(productId);
			if (activeIdByProduct.HasValue)
			{
				result = PromoteHelper.GetPromotion(activeIdByProduct.Value);
			}
			return result;
		}
		public static bool AddPromotionProducts(int activityId, string productIds)
		{
			return new PromotionDao().AddPromotionProducts(activityId, productIds);
		}
        /// <summary>
        /// 添加赠送的商品
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static bool AddPromotionPresentPro(int activityId, string productIds)
        {
            return new PromotionDao().AddPromotionPresentPro(activityId, productIds);
        }
		public static bool DeletePromotionProducts(int activityId, int? productId)
		{
			return new PromotionDao().DeletePromotionProducts(activityId, productId);
		}
        /// <summary>
        /// 删除促销活动中赠送商品
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool DeletePromotionPresentPro(int activityId, int? productId)
        {
            return new PromotionDao().DeletePromotionPresentPro(activityId, productId);
        }
		public static int AddPromotion(PromotionInfo promotion)
		{
			Database database = DatabaseFactory.CreateDatabase();
			int result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					PromotionDao promotionDao = new PromotionDao();
					int num = promotionDao.AddPromotion(promotion, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = -1;
					}
					else
					{
						if (!promotionDao.AddPromotionMemberGrades(num, promotion.MemberGradeIds, dbTransaction))
						{
							dbTransaction.Rollback();
							result = -2;
						}
						else
						{
							dbTransaction.Commit();
							result = num;
						}
					}
				}
				catch (Exception var_5_76)
				{
					dbTransaction.Rollback();
					result = 0;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static int EditPromotion(PromotionInfo promotion)
		{
			Database database = DatabaseFactory.CreateDatabase();
			int result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					PromotionDao promotionDao = new PromotionDao();
					if (!promotionDao.EditPromotion(promotion, dbTransaction))
					{
						dbTransaction.Rollback();
						result = -1;
					}
					else
					{
						if (!promotionDao.AddPromotionMemberGrades(promotion.ActivityId, promotion.MemberGradeIds, dbTransaction))
						{
							dbTransaction.Rollback();
							result = -2;
						}
						else
						{
							dbTransaction.Commit();
							result = 1;
						}
					}
				}
				catch (Exception var_4_72)
				{
					dbTransaction.Rollback();
					result = 0;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static bool DeletePromotion(int activityId)
		{
			return new PromotionDao().DeletePromotion(activityId);
		}
		public static IList<Member> GetMemdersByNames(IList<string> names)
		{
			IList<Member> list = new List<Member>();
			foreach (string current in names)
			{
				IUser user = Users.GetUser(0, current, false, false);
				if (user != null && user.UserRole == UserRole.Member)
				{
					list.Add(user as Member);
				}
			}
			return list;
		}
		public static IList<Member> GetMembersByRank(int? gradeId)
		{
			return new MessageBoxDao().GetMembersByRank(gradeId);
		}

        public static IList<Member> GetMembersByCondition(string condition)
        {
            return new MessageBoxDao().GetMembersByCondition(condition);
        }

        public static IList<Member> GetMembersByCreateDate(string CreateDate,int top)
        {
            return new MessageBoxDao().GetMembersByCreateDate(CreateDate,top);
        }
		public static bool AddBundlingProduct(BundlingInfo bundlingInfo)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					BundlingDao bundlingDao = new BundlingDao();
					int num = bundlingDao.AddBundlingProduct(bundlingInfo, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!bundlingDao.AddBundlingProductItems(num, bundlingInfo.BundlingItemInfos, dbTransaction))
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
				catch (Exception var_5_74)
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
		public static bool UpdateBundlingProduct(BundlingInfo bundlingInfo)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					BundlingDao bundlingDao = new BundlingDao();
					if (!bundlingDao.UpdateBundlingProduct(bundlingInfo, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!bundlingDao.DeleteBundlingByID(bundlingInfo.BundlingID, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!bundlingDao.AddBundlingProductItems(bundlingInfo.BundlingID, bundlingInfo.BundlingItemInfos, dbTransaction))
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
				catch (Exception var_4_91)
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
		public static BundlingInfo GetBundlingInfo(int bundlingID)
		{
			return new BundlingDao().GetBundlingInfo(bundlingID);
		}
		public static bool DeleteBundlingProduct(int bundlingID)
		{
			return new BundlingDao().DeleteBundlingProduct(bundlingID);
		}
		public static DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
		{
			return new BundlingDao().GetBundlingProducts(query);
		}
		public static string GetPriceByProductId(int productId)
		{
			return new GroupBuyDao().GetPriceByProductId(productId);
		}
		public static bool AddGroupBuy(GroupBuyInfo groupBuy)
		{
			Globals.EntityCoding(groupBuy, true);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					GroupBuyDao groupBuyDao = new GroupBuyDao();
					int num = groupBuyDao.AddGroupBuy(groupBuy, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!groupBuyDao.AddGroupBuyCondition(num, groupBuy.GroupBuyConditions, dbTransaction))
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
				catch (Exception var_5_7C)
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
		public static bool ProductGroupBuyExist(int productId)
		{
			return new GroupBuyDao().ProductGroupBuyExist(productId);
		}
		public static bool DeleteGroupBuy(int groupBuyId)
		{
			return new GroupBuyDao().DeleteGroupBuy(groupBuyId);
		}
		public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
		{
			Globals.EntityCoding(groupBuy, true);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					GroupBuyDao groupBuyDao = new GroupBuyDao();
					if (!groupBuyDao.UpdateGroupBuy(groupBuy, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!groupBuyDao.DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!groupBuyDao.AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTransaction))
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
				catch (Exception var_4_99)
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
		public static GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			return new GroupBuyDao().GetGroupBuy(groupBuyId);
		}
		public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
		{
			return new GroupBuyDao().GetGroupBuyList(query);
		}
		public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
		{
			return new GroupBuyDao().GetCurrentPrice(groupBuyId, prodcutQuantity);
		}
		public static void SwapGroupBuySequence(int groupBuyId, int displaySequence)
		{
			new GroupBuyDao().SwapGroupBuySequence(groupBuyId, displaySequence);
		}
		public static int GetOrderCount(int groupBuyId)
		{
			return new GroupBuyDao().GetOrderCount(groupBuyId);
		}
		public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
		{
			return new GroupBuyDao().SetGroupBuyStatus(groupBuyId, status);
		}
		public static bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			return new GroupBuyDao().SetGroupBuyEndUntreated(groupBuyId);
		}
		public static DbQueryResult GetCountDownList(GroupBuyQuery query)
		{
			return new CountDownDao().GetCountDownList(query);
		}
        /// <summary>
        ///获取广告订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetAdOrderInfoList(GroupBuyQuery query)
        {
            return new CountDownDao().GetAdOrderInfoList(query);
        }
          /// <summary>
        ///导出广告订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult ExprtAdOrderInfoList(GroupBuyQuery query)
        {
            return new CountDownDao().ExprtAdOrderInfoList(query);
        }
        public static DbQueryResult GetCountManagerDownList(GroupBuyQuery query)
        {
            return new CountDownDao().GetCountManagerDownList(query);
        }
		public static void SwapCountDownSequence(int countDownId, int displaySequence)
		{
			new CountDownDao().SwapCountDownSequence(countDownId, displaySequence);
		}
        /// <summary>
        /// 获取限时抢购管理
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectCountDownCategories()
        {
           return  new CountDownDao().SelectCountDownCategories();
        }
        /// <summary>
        /// 根据活动状态获取参加活动的商品列表
        /// </summary>
        /// <param name="ActoveTupe">活动类型:1、抢购中；2、未开始;3、已结束</param>
        /// <returns></returns>
        public static DataTable GetDownCateProducts(int CountDownCateGoryId, int ActiveType)
        {
            return new CountDownDao().GetDownCateProducts(CountDownCateGoryId, ActiveType);
        }
         /// <summary>
        /// 报错序列
        /// </summary>
        /// <param name="CountDownCategoryId"></param>
        /// <param name="displaySequence"></param>
        public static void SwapCountCategoriesDownSequence(int CountDownCategoryId, int displaySequence)
        {
            new CountDownDao().SwapCountCategoriesDownSequence(CountDownCategoryId, displaySequence);
        }
		public static bool DeleteCountDown(int countDownId)
		{
			return new CountDownDao().DeleteCountDown(countDownId);
		}
         /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="CountDownCategoryId"></param>
        /// <returns></returns>
        public static bool DeleteCountDownCategories(int CountDownCategoryId)
        {
            return new CountDownDao().DeleteCountDownCategories(CountDownCategoryId);
        }
		public static bool AddCountDown(CountDownInfo countDownInfo)
		{
			return new CountDownDao().AddCountDown(countDownInfo);
		}
          /// <summary>
        /// 保存限时活动
        /// </summary>
        /// <param name="countDownInfo"></param>
        /// <returns></returns>
        public static bool AddCountDownCategories(CountDownCategoriesInfo countDownInfo)
        {
            return new CountDownDao().AddCountDownCategories(countDownInfo);
        }
        /// <summary>
        /// 获取促销活动中赠送商品
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <returns></returns>
        public static System.Data.DataTable GetPromotionPresentPro(int activityId)
        {
            return new PromotionDao().GetPromotionPresentPro(activityId);
        }
		public static bool UpdateCountDown(CountDownInfo countDownInfo)
		{
			return new CountDownDao().UpdateCountDown(countDownInfo);
		}
         /// <summary>
        /// 修改限时活动管理
        /// </summary>
        /// <param name="countDownInfo"></param>
        /// <returns></returns>
        public static bool UpdateCountDownDownCategories(CountDownCategoriesInfo countDownInfo)
        {
            return new CountDownDao().UpdateCountDownDownCategories(countDownInfo);
        }
		public static bool ProductCountDownExist(int productId)
		{
			return new CountDownDao().ProductCountDownExist(productId);
		}
        public static bool ProductCountDownExist(int productId, DateTime startDate, DateTime endDate, int countDownId)
        {
            return new CountDownDao().ProductCountDownExist(productId, startDate, endDate, countDownId);
        }

		public static CountDownInfo GetCountDownInfo(int countDownId)
		{
			return new CountDownDao().GetCountDownInfo(countDownId);
		}
           /// <summary>
        /// 获取限时活动管理对象
        /// </summary>
        /// <param name="CountDownCategoryId"></param>
        /// <returns></returns>
        public  static CountDownCategoriesInfo GetCountDownnCategoriesInfo(int CountDownCategoryId)
        {
            return new CountDownDao().GetCountDownnCategoriesInfo(CountDownCategoryId);
        }
	}
}
