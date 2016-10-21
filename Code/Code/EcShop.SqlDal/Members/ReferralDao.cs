using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Members
{
	public class ReferralDao
	{
		private Database database;
		public ReferralDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetReferrals(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ReferralStatus = 2");
			if (!string.IsNullOrEmpty(query.Username))
			{
				stringBuilder.AppendFormat(" AND m.userid in(select u1.userid from aspnet_users as u1 where username LIKE '%{0}%')", DataHelper.CleanSearchString(query.Username));
			}
			if (!string.IsNullOrEmpty(query.ReferralUsername))
			{
				stringBuilder.AppendFormat(" AND m.ReferralUserId in(select u1.userid from aspnet_users as u1 where username LIKE '%{0}%')", DataHelper.CleanSearchString(query.ReferralUsername));
			}
			if (!string.IsNullOrEmpty(query.Realname))
			{
				stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				stringBuilder.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Username FROM aspnet_users WHERE userid = m.ReferralUserId) AS ReferralUsername, (SELECT count(*) from aspnet_Members as u where u.ReferralUserId=m.userid)  as SubNumber, (SELECT COUNT(*) FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId) AS ReferralSplittin");
		}
		public DbQueryResult GetSplittinDraws(BalanceDrawRequestQuery query, int? auditStatus)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (auditStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND AuditStatus = {0}", auditStatus);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate >= '{0}'", query.FromDate);
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate <= '{0}'", query.ToDate);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_SplittinDraws s", "JournalNumber", stringBuilder.ToString(), "*, (SELECT TOP 1 Balance FROM Ecshop_SplittinDetails WHERE UserId = s.UserId AND IsUse = 'true' ORDER BY JournalNumber DESC) AS Balance");
		}

        /// <summary>
        /// 获取已提现的金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserAllSplittinDraws(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Amount) FROM Ecshop_SplittinDraws WHERE AuditStatus =2 AND UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
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
		public bool SplittinDrawRequest(SplittinDrawInfo splittinDraw)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SplittinDraws (UserId, UserName, RequestDate, Amount, Account, AuditStatus) VALUES (@UserId, @UserName, @RequestDate, @Amount, @Account, @AuditStatus)");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, splittinDraw.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, splittinDraw.UserName);
			this.database.AddInParameter(sqlStringCommand, "RequestDate", DbType.DateTime, splittinDraw.RequestDate);
			this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, splittinDraw.Amount);
			this.database.AddInParameter(sqlStringCommand, "Account", DbType.String, splittinDraw.Account);
			this.database.AddInParameter(sqlStringCommand, "AuditStatus", DbType.Int32, splittinDraw.AuditStatus);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DbQueryResult GetSplittinDetails(BalanceDetailQuery query, bool? isUser)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (isUser.HasValue)
			{
				stringBuilder.AppendFormat(" AND IsUse = '{0}'", isUser);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate >= '{0}'", query.FromDate);
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate <= '{0}'", query.ToDate);
			}
			if (query.SplittingTypes != SplittingTypes.NotSet)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0}", (int)query.SplittingTypes);
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat("AND OrderId='{0}'", query.OrderId);
			}

            string strtable = "(select S.*,O.OrderTotal,U.UserName as SubUserName  from Ecshop_SplittinDetails S join dbo.Ecshop_Orders O  on S.OrderId=O.OrderId join dbo.aspnet_Users U on U.UserId=S.SubUserId) as ExtendTable ";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, strtable, "JournalNumber", stringBuilder.ToString(), "*");
		}
		public decimal GetUserAllSplittin(int userId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE IsUse = 'true' AND UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
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
		public decimal GetUserUseSplittin(int userId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 Balance FROM Ecshop_SplittinDetails WHERE IsUse = 'true' AND UserId =  @UserId ORDER BY JournalNumber DESC");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
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
		public decimal GetUserNoUseSplittin(int userId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE IsUse = 'false' AND UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM   Ecshop_SplittinDetails    WHERE IsUse = 'false' AND OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool AccepteDraw(long journalNumber, string managerRemark)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_SplittinDraws SET AuditStatus = 2, AccountDate = getdate(), ManagerRemark = @ManagerRemark WHERE JournalNumber = @JournalNumber");
			this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, managerRemark);
			this.database.AddInParameter(sqlStringCommand, "JournalNumber", DbType.Int64, journalNumber);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool AddSplittinDetail(SplittinDetailInfo splittinDetail)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SplittinDetails (OrderId, UserId, UserName, IsUse, TradeDate, TradeType, SubUserId, Income, Expenses, Balance, Remark) VALUES (@OrderId, @UserId, @UserName, @IsUse, @TradeDate, @TradeType, @SubUserId, @Income, @Expenses, @Balance, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, splittinDetail.OrderId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, splittinDetail.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, splittinDetail.UserName);
			this.database.AddInParameter(sqlStringCommand, "IsUse", DbType.Boolean, splittinDetail.IsUse);
			this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, splittinDetail.TradeDate);
			this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, splittinDetail.TradeType);
			this.database.AddInParameter(sqlStringCommand, "SubUserId", DbType.Int32, splittinDetail.SubUserId);
			this.database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, splittinDetail.Income);
			this.database.AddInParameter(sqlStringCommand, "Expenses", DbType.Currency, splittinDetail.Expenses);
			this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, splittinDetail.Balance);
			this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, splittinDetail.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public SplittinDrawInfo GetSplittinDraw(long journalNumber)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_SplittinDraws WHERE JournalNumber = @JournalNumber;");
			this.database.AddInParameter(sqlStringCommand, "JournalNumber", DbType.Int64, journalNumber);
			SplittinDrawInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<SplittinDrawInfo>(dataReader);
			}
			return result;
		}
		public SplittinDetailInfo GetSplittinDetail(long journalNumber)
		{
			SplittinDetailInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_SplittinDetails WHERE JournalNumber = @JournalNumber;");
			this.database.AddInParameter(sqlStringCommand, "JournalNumber", DbType.Int64, journalNumber);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<SplittinDetailInfo>(dataReader);
			}
			return result;
		}


        public SplittinDrawInfo GetLatestSplittinDrawInfo(int userid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Ecshop_SplittinDraws WHERE UserId=@UserId order by RequestDate desc;");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            SplittinDrawInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<SplittinDrawInfo>(dataReader);
            }
            return result;
        }
	}
}
