using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Members
{
    public class MemberDao
    {
        private Database database;
        public MemberDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 检查当前用户是否已验证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Checkisverify(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select isnull(isverify,0) from  aspnet_Members  where userId=" + userId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                return obj.ToString() == "1" ? true : false;
            }
            return false;

        }
        public DbQueryResult GetMembers(MemberQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (query.HasVipCard.HasValue)
            {
                if (query.HasVipCard.Value)
                {
                    stringBuilder.Append("VipCardNumber is not null");
                }
                else
                {
                    stringBuilder.Append("VipCardNumber is null");
                }
            }
            if (query.GradeId.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("GradeId = {0}", query.GradeId.Value);
            }
            if (query.ReferralStatus.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("ReferralStatus = {0}", query.ReferralStatus.Value);
            }
            if (query.IsApproved.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("m.userid in(select u1.userid from aspnet_users as u1 where  IsApproved = '{0}')", query.IsApproved.Value);
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("m.userid in(select u1.userid from aspnet_users as u1 where username LIKE '%{0}%')", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.AppendFormat(" AND ", new object[0]);
                }
                stringBuilder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.AppendFormat(" AND ", new object[0]);
                }
                stringBuilder.AppendFormat("CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
            }

            //添加注册用户类型
            if (query.UserType.HasValue)
            {
                if (query.UserType.Value > 0)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(" AND ");
                    }
                    stringBuilder.AppendFormat("m.userid in(select u1.userid from aspnet_users as u1 where  UserType = '{0}')", query.UserType.Value);
                }
            }
            if(query.RstStartTime.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.AppendFormat(" AND ", new object[0]);
                }
                stringBuilder.AppendFormat("  CreateDate>='{0}' ", query.RstStartTime.Value.ToString());
            }
            if(query.RstEndTime.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.AppendFormat(" AND ", new object[0]);
                }
                stringBuilder.AppendFormat("  CreateDate<'{0}' ", query.RstEndTime.Value.ToString());
            }
            if (!string.IsNullOrEmpty(query.ClientType))
            {
                string clientType = query.ClientType;
                string text;
                object obj;
                if (clientType != null)
                {
                    if (clientType == "new")
                    {
                        text = "SElECT UserId FROM aspnet_Users WHERE 1=1";
                        if (query.StartTime.HasValue)
                        {
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,CreateDate,'",
								query.StartTime.Value.Date,
								"')<=0"
							});
                        }
                        if (query.EndTime.HasValue)
                        {
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,CreateDate,'",
								query.EndTime.Value.Date,
								"')>=0"
							});
                        }
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.AppendFormat(" AND ", new object[0]);
                        }
                        stringBuilder.Append("UserId IN (" + text + ")");
                        goto IL_6C6;
                    }
                    if (clientType == "activy")
                    {
                        text = "SELECT UserId FROM Ecshop_Orders WHERE 1=1";
                        if (query.OrderNumber.HasValue)
                        {
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.StartTime.Value.Date,
								"')<=0"
							});
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.EndTime.Value.Date,
								"')>=0"
							});
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" GROUP BY UserId HAVING COUNT(*)",
								query.CharSymbol,
								query.OrderNumber.Value
							});
                        }
                        if (query.OrderMoney.HasValue)
                        {
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.StartTime.Value.Date,
								"')<=0"
							});
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.EndTime.Value.Date,
								"')>=0"
							});
                            obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								" GROUP BY UserId HAVING SUM(OrderTotal)",
								query.CharSymbol,
								query.OrderMoney.Value
							});
                        }
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.AppendFormat(" AND ", new object[0]);
                        }
                        stringBuilder.AppendFormat("UserId IN (" + text + ")", new object[0]);
                        goto IL_6C6;
                    }
                }
                text = "SELECT UserId FROM Ecshop_Orders WHERE 1=1";
                obj = text;
                text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,OrderDate,'",
					query.StartTime.Value.Date,
					"')<=0"
				});
                obj = text;
                text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,OrderDate,'",
					query.EndTime.Value.Date,
					"')>=0"
				});
                text += " GROUP BY UserId";
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.AppendFormat(" AND ", new object[0]);
                }
                stringBuilder.AppendFormat("UserId NOT IN (" + text + ")", new object[0]);
            IL_6C6: ;
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "(select m.*,c.CreateDate  from  aspnet_Members m inner join [dbo].[aspnet_Users] as c on m.UserId=c.UserId) m", "m.UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName,(select u.username from aspnet_users as u where u.userid=m.userid)  as username,(select u.email from aspnet_users as u where u.userid=m.userid)  as email");
            //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName,(select u.username from aspnet_users as u where u.userid=m.userid)  as username,(select u.email from aspnet_users as u where u.userid=m.userid)  as email");
        }
        public DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            DataTable result;
            if (fields.Count == 0)
            {
                result = null;
            }
            else
            {
                DataTable dataTable = null;
                string text = string.Empty;
                foreach (string current in fields)
                {
                    text = text + DataHelper.CleanSearchString(current) + ",";
                }
                text = text.Substring(0, text.Length - 1);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("SELECT {0} FROM aspnet_Members as m,aspnet_users as u WHERE m.UserID=u.UserID ", text);
                if (!string.IsNullOrEmpty(query.Username))
                {
                    stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
                }
                if (query.GradeId.HasValue)
                {
                    stringBuilder.AppendFormat(" AND GradeId={0}", query.GradeId);
                }
                if (query.HasVipCard.HasValue)
                {
                    if (query.HasVipCard.Value)
                    {
                        stringBuilder.Append(" AND VipCardNumber is not null");
                    }
                    else
                    {
                        stringBuilder.Append(" AND VipCardNumber is null");
                    }
                }
                if (!string.IsNullOrEmpty(query.Realname))
                {
                    stringBuilder.AppendFormat(" AND Realname LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
                }
                if (query.RstStartTime.HasValue)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.AppendFormat(" AND ", new object[0]);
                    }
                    stringBuilder.AppendFormat("  CreateDate>='{0}' ", query.RstStartTime.Value.ToString());
                }
                if (query.RstEndTime.HasValue)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.AppendFormat(" AND ", new object[0]);
                    }
                    stringBuilder.AppendFormat("  CreateDate<'{0}' ", query.RstEndTime.Value.ToString());
                }
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                    dataReader.Close();
                }
                result = dataTable;
            }
            return result;
        }
        public bool InsertClientSet(Dictionary<int, MemberClientSet> clientsets)
        {
            StringBuilder stringBuilder = new StringBuilder("DELETE FROM  [Ecshop_MemberClientSet];");
            foreach (KeyValuePair<int, MemberClientSet> current in clientsets)
            {
                string text = "";
                string text2 = "";
                if (current.Value.StartTime.HasValue)
                {
                    text = current.Value.StartTime.Value.ToString("yyyy-MM-dd");
                }
                if (current.Value.EndTime.HasValue)
                {
                    text2 = current.Value.EndTime.Value.ToString("yyyy-MM-dd");
                }
                stringBuilder.AppendFormat(string.Concat(new object[]
				{
					"INSERT INTO Ecshop_MemberClientSet(ClientTypeId,StartTime,EndTime,LastDay,ClientChar,ClientValue) VALUES (",
					current.Key,
					",'",
					text,
					"','",
					text2,
					"',",
					current.Value.LastDay,
					",'",
					current.Value.ClientChar,
					"',",
					current.Value.ClientValue,
					");"
				}), new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public Dictionary<int, MemberClientSet> GetMemberClientSet()
        {
            Dictionary<int, MemberClientSet> dictionary = new Dictionary<int, MemberClientSet>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_MemberClientSet");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    MemberClientSet memberClientSet = DataMapper.PopulateMemberClientSet(dataReader);
                    dictionary.Add(memberClientSet.ClientTypeId, memberClientSet);
                }
            }
            return dictionary;
        }
        public bool Delete(int userId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Member_Delete");
            Member member = Users.GetUser(userId) as Member;
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, member.Username);
            this.database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            this.database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
            return parameterValue != null && parameterValue != DBNull.Value && Convert.ToInt32(parameterValue) == 0;
        }
        public bool UpdateMemberAccount(decimal orderTotal, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1 WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "OrderPrice", DbType.Decimal, orderTotal);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool ReduceMemberAccount(decimal orderTotal, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = case when ISNULL(Expenditure,0) - @OrderTotal > 0 then ISNULL(Expenditure,0) - @OrderTotal else 0 end, OrderNumber = ISNULL(OrderNumber,0) - 1 WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Decimal, orderTotal);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = case when ISNULL(Expenditure,0) >  @refundAmount then ISNULL(Expenditure,0) -  @refundAmount else 0 end, OrderNumber = ISNULL(OrderNumber,0) - @refundNum WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "refundAmount", DbType.Decimal, refundAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            if (isAllRefund)
            {
                this.database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 0);
            }
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public bool ChangeMemberGrade(int userId, int gradId, int points)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM aspnet_MemberGrades Order by Point Desc ");
            bool result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    if ((int)dataReader["GradeId"] == gradId)
                    {
                        break;
                    }
                    if ((int)dataReader["Point"] <= points)
                    {
                        result = this.UpdateUserRank(userId, (int)dataReader["GradeId"]);
                        return result;
                    }
                }
                result = true;
            }
            return result;
        }
        private bool UpdateUserRank(int userId, int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT COUNT(*) AS NoPayOrderNum FROM Ecshop_Orders WHERE UserId = {0} AND OrderStatus = {1};", HiContext.Current.User.UserId, 1);
            stringBuilder.AppendFormat(" SELECT COUNT(*) AS NoReadMessageNum FROM Ecshop_MemberMessageBox WHERE Accepter = '{0}' AND IsRead=0 ;", HiContext.Current.User.Username);
            stringBuilder.AppendFormat(" SELECT COUNT(*) AS NoReplyLeaveCommentNum FROM Ecshop_ProductConsultations WHERE UserId = {0} AND ViewDate is null AND ReplyUserId is not null;", HiContext.Current.User.UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    if (DBNull.Value != dataReader["NoPayOrderNum"])
                    {
                        num = (int)dataReader["NoPayOrderNum"];
                    }
                }
                if (dataReader.NextResult() && dataReader.Read())
                {
                    if (DBNull.Value != dataReader["NoReadMessageNum"])
                    {
                        num2 = (int)dataReader["NoReadMessageNum"];
                    }
                }
                if (dataReader.NextResult() && dataReader.Read())
                {
                    if (DBNull.Value != dataReader["NoReplyLeaveCommentNum"])
                    {
                        num3 = (int)dataReader["NoReplyLeaveCommentNum"];
                    }
                }
            }
            noPayOrderNum = num;
            noReadMessageNum = num2;
            noReplyLeaveCommentNum = num3;
        }
        public DbQueryResult GetMySubUsers(MemberQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("ReferralUserId = {0}", HiContext.Current.User.UserId);
            if (query.ReferralStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND ReferralStatus = {0}", query.ReferralStatus.Value);
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                stringBuilder.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, IsNull((SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId),0) AS SubMemberSplittin, IsNull((SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 3 AND SubUserId = m.UserId),0) AS SubReferralSplittin, (SELECT COUNT(*) FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT Top 1 TradeDate FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate");
        }


        public DbQueryResult GetMySubUsers(MemberQuery query, int userId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("ReferralUserId = {0}", userId);
            if (query.ReferralStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND ReferralStatus = {0}", query.ReferralStatus.Value);
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                stringBuilder.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, IsNull((SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId),0) AS SubMemberSplittin, IsNull((SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 3 AND SubUserId = m.UserId),0) AS SubReferralSplittin, (SELECT COUNT(*) FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT Top 1 TradeDate FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate");
        }


        public SubReferralUser GetMyReferralSubUser(int UserId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT * , (SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId) AS SubMemberSplittin, (SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 3 AND SubUserId = m.UserId) AS SubReferralSplittin, (SELECT COUNT(*) FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT Top 1 TradeDate FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate from vw_aspnet_Members m where UserId=" + UserId, new object[0]);
            SubReferralUser subReferralUser = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    subReferralUser = new SubReferralUser();
                    subReferralUser.UserID = (int)dataReader["UserId"];
                    subReferralUser.UserName = (string)dataReader["UserName"];
                    subReferralUser.RealName = "";
                    if (dataReader["RealName"] != DBNull.Value)
                    {
                        subReferralUser.RealName = (string)dataReader["RealName"];
                    }
                    if (dataReader["ReferralAuditDate"] != DBNull.Value)
                    {
                        subReferralUser.ReferralAuditDate = new DateTime?((DateTime)dataReader["ReferralAuditDate"]);
                    }
                    if (dataReader["LastReferralDate"] != DBNull.Value)
                    {
                        subReferralUser.LastReferralDate = new DateTime?((DateTime)dataReader["LastReferralDate"]);
                    }
                    if (dataReader["CreateDate"] != DBNull.Value)
                    {
                        subReferralUser.CreateDate = (DateTime)dataReader["CreateDate"];
                    }
                    if (dataReader["CellPhone"] != DBNull.Value)
                    {
                        subReferralUser.CellPhone = (string)dataReader["CellPhone"];
                    }
                    if (dataReader["ReferralOrderNumber"] != DBNull.Value)
                    {
                        subReferralUser.ReferralOrderNumber = (int)dataReader["ReferralOrderNumber"];
                    }
                    if (dataReader["SubReferralSplittin"] != DBNull.Value)
                    {
                        subReferralUser.SubReferralSplittin = (decimal)dataReader["SubReferralSplittin"];
                    }
                }
            }
            return subReferralUser;
        }
        public SubMember GetMySubMember(int UserId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT * , (SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId) AS SubMemberSplittin, (SELECT SUM(Income) FROM Ecshop_SplittinDetails WHERE TradeType = 3 AND SubUserId = m.UserId) AS SubReferralSplittin, (SELECT COUNT(*) FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT Top 1 TradeDate FROM Ecshop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate from vw_aspnet_Members m where UserId=" + UserId, new object[0]);
            SubMember subMember = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    subMember = new SubMember();
                    subMember.UserID = (int)dataReader["UserId"];
                    subMember.UserName = (string)dataReader["UserName"];
                    subMember.RealName = "";
                    if (dataReader["RealName"] != DBNull.Value)
                    {
                        subMember.RealName = (string)dataReader["RealName"];
                    }
                    if (dataReader["CreateDate"] != DBNull.Value)
                    {
                        subMember.CreateDate = (DateTime)dataReader["CreateDate"];
                    }
                    if (dataReader["CellPhone"] != DBNull.Value)
                    {
                        subMember.CellPhone = (string)dataReader["CellPhone"];
                    }
                    if (dataReader["SubMemberSplittin"] != DBNull.Value)
                    {
                        subMember.SubMemberSplittin = (decimal)dataReader["SubMemberSplittin"];
                    }
                    if (dataReader["OrderNumber"] != DBNull.Value)
                    {
                        subMember.OrderNumber = (int)dataReader["OrderNumber"];
                    }
                }
            }
            return subMember;
        }

        public DataTable GetMySubMemberByUserId(int UserId)
        {

            DataTable dt = new DataTable();
            try
            {

                string sql = "SELECT RealName,CellPhone,IdentityCard,IsVerify FROM aspnet_Members  WHERE UserId=@UserId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                this.database.AddInParameter(sqlStringCommand, "@UserId", DbType.Int32, UserId);
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dt = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                return dt;
            }
            catch (Exception ee)
            {
                return dt;
            }


        }

        public int GetMemberDiscount(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            int result;
            if (obj != null && obj != DBNull.Value)
            {
                result = (int)obj;
            }
            else
            {
                result = 0;
            }
            return result;
        }


        public DbQueryResult GetMembersAddress(MemberQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1 ");

            //IsDefault= 1 为默认地址
            if (query.IsDefault == 1)
            {
                stringBuilder.AppendFormat(" AND IsDefault = {0}", query.IsDefault);
            }

            if (!string.IsNullOrEmpty(query.Username))
            {
                stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                stringBuilder.AppendFormat(" AND Realname LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                stringBuilder.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
            }

            string selectFields = "RegionId,ShippingId,UserId,ShipTo,[Address],Zipcode,TelPhone,CellPhone,IdentityCard,Realname,UserName,IsDefault,Email";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_MembersAddress", "UserId", stringBuilder.ToString(), selectFields);
        }

        #region for App Api

        public DataTable GetUserByUsername(string username)
        {
            DataTable dt = null;

            DbCommand command = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Users WHERE Username = @Username");
            this.database.AddInParameter(command, "UserName", DbType.String, username);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                dt = DataHelper.ConverDataReaderToDataTable(dataReader);
                dataReader.Close();
            }

            return dt;
        }

        #endregion

        public bool UpdateAtavar(int userId, string avatar)
        {
            DbCommand command = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET HeadImgUrl = @Avatar WHERE UserId = @UserId");
            this.database.AddInParameter(command, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(command, "Avatar", DbType.String, avatar);

            return (this.database.ExecuteNonQuery(command) > 0);
        }
        /// <summary>
        /// 验证失败累计次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AddIsVerifyMsg(int userId)
        {
            DbCommand command = this.database.GetSqlStringCommand(@"  insert into ecshop_IsVerifyMsg values(@userId,1,getdate(),'')");
            this.database.AddInParameter(command, "@userId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(command) > 0);
        }
        /// <summary>
        /// 获取当前用户当前的登录的失败次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int CheckErrorCount(int userId)
        {
            DbCommand command = this.database.GetSqlStringCommand(@"select isnull(sum(filedCount),0)  from ecshop_IsVerifyMsg where  VerifyTime>CONVERT(varchar(100), GETDATE(), 23)
            and userId=@userId");
            this.database.AddInParameter(command, "@userId", DbType.Int32, userId);
            object obj = this.database.ExecuteScalar(command);
            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        /// <summary>
        /// 修改会员资料信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cellPhone"></param>
        /// <param name="IdentityCard"></param>
        /// <returns></returns>
        public bool UpdateMemberInfo(int userId, string cellPhone, string IdentityCard, string RealName)
        {

            DbCommand command = this.database.GetSqlStringCommand(@" UPDATE  aspnet_Members  
                            SET IsVerify=1,
                            cellPhone=@cellPhone,
                            IdentityCard=@IdentityCard,
                            RealName=@RealName
                            WHERE  UserId = @UserId");
            this.database.AddInParameter(command, "@UserId", DbType.Int32, userId);
            this.database.AddInParameter(command, "@cellPhone", DbType.String, cellPhone);
            this.database.AddInParameter(command, "@IdentityCard", DbType.String, IdentityCard);
            this.database.AddInParameter(command, "@RealName", DbType.String, RealName);
            return (this.database.ExecuteNonQuery(command) > 0);

        }
        //        /// <summary>
        //        /// 验证成功清除失败纪录
        //        /// </summary>
        //        /// <param name="userId"></param>
        //        /// <returns></returns>
        //        public bool ClearErrCount(int userId)
        //        {
        //            DbCommand command = this.database.GetSqlStringCommand(@"update  ecshop_IsVerifyMsg  set filedCount=0 
        //                                where  VerifyTime>CONVERT(varchar(100), GETDATE(), 23)
        //                                and userId=@userId");
        //            return (this.database.ExecuteNonQuery(command) > 0);
        //        }

        /// <summary>
        /// 根据用户ID获取其邀请码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetRecommendCodeByUserId(int userId)
        {
            DbCommand command = this.database.GetSqlStringCommand("select code from Ecshop_RecommendCode where UserId =@userId  ");
            this.database.AddInParameter(command, "@userId", DbType.Int32, userId);
            object obj = this.database.ExecuteScalar(command);
            return obj != null ? int.Parse(obj.ToString()) : 49999;
        }

        /// <summary>
        /// 根据邀请码获取其USERID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetUserIdByRecommendCode(string code)
        {
            DbCommand command = this.database.GetSqlStringCommand("select userid from Ecshop_RecommendCode where code =@code  ");
            this.database.AddInParameter(command, "@code", DbType.String, code);
            object obj = this.database.ExecuteScalar(command);
            return obj != null ? obj.ToString() : string.Empty;
        }

        /// <summary>
        /// 生成邀请码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string AddRecommendCodeByUserId(int userId)
        {
            DbCommand command = this.database.GetSqlStringCommand(@" if not exists (select 1 from  Ecshop_RecommendCode where userid = @userId ) insert into  Ecshop_RecommendCode(Userid) values(@userId);select @@Identity  ");
            this.database.AddInParameter(command, "@userId", DbType.Int32, userId);
            object obj = this.database.ExecuteScalar(command);
            return obj != null ? obj.ToString() : "";
        }

        /// <summary>
        /// 添加邀请码使用记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="useredid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool AddRecommendCodeRecord(int userid, int useredid, string code,string systemCode)
        {
            DbCommand command = this.database.GetSqlStringCommand(@" insert into  Ecshop_RecommendCodeRecord(UserId,Code,UseredId,SystemCode) values(@userid,@code,@useredid,@SystemCode)");
            this.database.AddInParameter(command, "@userid", DbType.Int32, userid);
            this.database.AddInParameter(command, "@useredid", DbType.Int32, useredid);
            this.database.AddInParameter(command, "@code", DbType.String, code);
            this.database.AddInParameter(command, "@SystemCode", DbType.String, systemCode);
            return (this.database.ExecuteNonQuery(command) > 0);
        }

        public bool IsExsitRecommendCode(string code, int userid)
        {
            DbCommand command = this.database.GetSqlStringCommand(@"select count(code) from Ecshop_RecommendCode where code =@code and UserId!=@UserId ");

            this.database.AddInParameter(command, "@code", DbType.String, code);

            this.database.AddInParameter(command, "@UserId", DbType.Int32, userid);

            object objresult;

            int result = 0;

            objresult = this.database.ExecuteScalar(command);

            if (objresult != null)
            {
                int.TryParse(objresult.ToString(), out result);
            }

            return result > 0;

        }


        public string[] GetAllUserCellPhones()
        {


           List<string> strlist=new List<string>();

            string sql = "SELECT  phonenumber FROM vw_Ecshop_UserCellPhone";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);

            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    object num = dataReader["phonenumber"];

                    if (num != null)
                    {
                        strlist.Add(num.ToString());
                    }
                }
            }

            return strlist.ToArray();

        }

        public MembersUser GetMembersUser(int userId)
        {
            MembersUser result = new MembersUser();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select RealName,IdentityCard from aspnet_Members where UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
               result = ReaderConvert.ReaderToModel<MembersUser>(dataReader);
            }

            return result;
        }

        /// <summary>
        /// 获取邀请码相关信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MemberRecommendCodeInfo GetRecommendCodeInfo(int userId)
        {
            MemberRecommendCodeInfo recommendCodeInfo = new MemberRecommendCodeInfo();
            string sql = @"select count(*) as TotalNum from Ecshop_RecommendCodeRecord where UseredId = @userId;
                           select COUNT(*) WeekLevel from Ecshop_RecommendCodeRecord
                           where addtime between dateadd(wk,datediff(wk,0,getdate()), 0) and dateadd(wk,datediff(wk,0,getdate()), 6) 
                           and UseredId = @userId group by UseredId;
                           select top 10 SUBSTRING(b.CellPhone,1,LEN(b.CellPhone) - 8) +'****' + right(b.CellPhone,4) CellPhone
                           from Ecshop_RecommendCodeRecord a
                           left join aspnet_Members b on a.UseredId = b.UserId
                           where b.CellPhone <> '' and b.CellPhone is not null and UseredId = @userId   
                           order by AddTime desc;";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    if (dataReader["TotalNum"] != System.DBNull.Value)
                    {
                        recommendCodeInfo.TotalNum = (int)dataReader["TotalNum"];
                    }
                    else
                    {
                        recommendCodeInfo.TotalNum = 0;
                    }
                }

                dataReader.NextResult();
                if (dataReader.Read())
                {
                    if (dataReader["WeekLevel"] != System.DBNull.Value)
                    {
                        recommendCodeInfo.WeekLevel = (int)dataReader["WeekLevel"];
                    }
                    else
                    {
                        recommendCodeInfo.WeekLevel = 0;
                    }
                }

                dataReader.NextResult();
                NextRecommendCode item;
                List<NextRecommendCode> nextitems = new List<NextRecommendCode>();
                while (dataReader.Read())
                {
                    item = new NextRecommendCode();
                    if (dataReader["CellPhone"] != System.DBNull.Value)
                    {
                        item.CellPhone = dataReader["CellPhone"].ToString();
                    }
                    nextitems.Add(item);
                }
                if (nextitems != null && nextitems.Count > 0)
                {
                    recommendCodeInfo.NextRecommendCode.AddRange(nextitems);
                }
                
            }
            return recommendCodeInfo;
        }
    }
}
