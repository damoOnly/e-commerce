using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Members
{
	public class PointDetailDao
	{
		private Database database;
		public PointDetailDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        /// <summary>
        /// 检查当前用户是否已认证账号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckUserIsVerify(int userId)
        {
            try
            {
                string sql = "SELECT ISNULL(IsVerify,0) FROM    aspnet_Members  WHERE userId=" + userId;
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                int rest = 0;
                int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out rest);
                return rest > 0 ? true : false;
            }
            catch (Exception ee)
            {
                return false;
            }
			
        }
		public bool AddPointDetail(PointDetailInfo point)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
			this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, point.TradeDate);
			this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int)point.TradeType);
			this.database.AddInParameter(sqlStringCommand, "Increased", DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Reduced", DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
			this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

        public bool UpdatePointDetail(PointDetailInfo point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_PointDetails set Points= case when Points - @Points > 0 then Points - @Points else 0 end where UserId=@UserId and OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

		public int GetHistoryPoint(int userId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM Ecshop_PointDetails WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public DbQueryResult GetUserPoints(int pageIndex)
		{
			//return DataHelper.PagingByRownumber(pageIndex, 10, "JournalNumber", SortAction.Desc, true, "Ecshop_PointDetails", "JournalNumber", string.Format("UserId={0}", HiContext.Current.User.UserId), "*");
            return GetUserPoints(HiContext.Current.User.UserId, pageIndex, 10);
		}

        public DbQueryResult GetUserPoints(int userId, int pageIndex, int pageSize)
        {
            //去除积分没有变化的
            return DataHelper.PagingByRownumber(pageIndex, pageSize, "JournalNumber", SortAction.Desc, true, "Ecshop_PointDetails", "JournalNumber", string.Format("UserId={0} and not (Increased=0 and Reduced=0)", userId), "*");
        }
    }
}
