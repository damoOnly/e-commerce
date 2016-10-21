using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Comments
{
	public class LeaveCommentDao
	{
		private Database database;
		public LeaveCommentDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public LeaveCommentInfo GetLeaveComment(long leaveId)
		{
			LeaveCommentInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_LeaveComments WHERE LeaveId=@LeaveId;");
			this.database.AddInParameter(sqlStringCommand, "LeaveId", DbType.Int64, leaveId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateLeaveComment(dataReader);
				}
			}
			return result;
		}
		public DbQueryResult GetLeaveComments(LeaveCommentQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_LeaveComments_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildLeaveCommentQuery(query));
			this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
			DataSet dataSet = this.database.ExecuteDataSet(storedProcCommand);
			dataSet.Relations.Add("LeaveCommentReplays", dataSet.Tables[0].Columns["LeaveId"], dataSet.Tables[1].Columns["LeaveId"], false);
			dbQueryResult.Data = dataSet;
			dbQueryResult.TotalRecords = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return dbQueryResult;
		}
		public bool DeleteLeaveComment(long leaveId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_LeaveCommentReplys WHERE LeaveId=@LeaveId;DELETE FROM Ecshop_LeaveComments WHERE LeaveId=@LeaveId");
			this.database.AddInParameter(sqlStringCommand, "leaveId", DbType.Int64, leaveId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_LeaveCommentReplys(LeaveId,UserId,ReplyContent,ReplyDate) VALUES(@LeaveId,@UserId,@ReplyContent,@ReplyDate);SELECT @@IDENTITY ");
			this.database.AddInParameter(sqlStringCommand, "leaveId", DbType.Int64, leaveReply.LeaveId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, leaveReply.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReplyContent", DbType.String, leaveReply.ReplyContent);
			this.database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.String, DataHelper.GetSafeDateTimeFormat(leaveReply.ReplyDate));
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public bool DeleteLeaveCommentReply(long leaveReplyId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_LeaveCommentReplys WHERE replyId=@replyId;");
			this.database.AddInParameter(sqlStringCommand, "replyId", DbType.Int64, leaveReplyId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DataTable GetReplyLeaveComments(long leaveId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_LeaveCommentReplys WHERE LeaveId=@LeaveId");
			this.database.AddInParameter(sqlStringCommand, "LeaveId", DbType.Int64, leaveId);
			DataTable result = new DataTable();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public bool InsertLeaveComment(LeaveCommentInfo leave)
		{
            bool result = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                //打开链接
                connection.Open();
                //创建事务
                DbTransaction Tran = connection.BeginTransaction();

                try
                {
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Insert into Ecshop_LeaveComments(UserId,UserName,Title,PublishContent,PublishDate,LastDate,FeedbackType,ContactWay)  values(@UserId,@UserName,@Title,@PublishContent,@PublishDate,@LastDate,@FeedbackType,@ContactWay);select @@identity");
                    this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, leave.UserId);
                    this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, leave.UserName);
                    this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, leave.Title);
                    this.database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, leave.PublishContent);
                    this.database.AddInParameter(sqlStringCommand, "PublishDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
                    this.database.AddInParameter(sqlStringCommand, "LastDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
                    this.database.AddInParameter(sqlStringCommand, "FeedbackType", DbType.Int32, leave.FeedbackType);
                    this.database.AddInParameter(sqlStringCommand, "ContactWay", DbType.String, leave.ContactWay);

                    int commentid = 0;
                    if (!int.TryParse(this.database.ExecuteScalar(sqlStringCommand, Tran).ToString(), out commentid))
                    {
                        Tran.Rollback();
                        result = false;
                    }


                    //新增图片
                    if (leave.Images.Count > 0)
                    {
                        foreach (string image in leave.Images)
                        {
                            DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Image(Image,BindType,BindId) VALUES (@Image, @BindType,@BindId)");
                            this.database.AddInParameter(sqlStringCommand2, "Image", DbType.String, image);
                            this.database.AddInParameter(sqlStringCommand2, "BindType", DbType.Int32, ImageBindType.Feedback);
                            this.database.AddInParameter(sqlStringCommand2, "BindId", DbType.Int32, commentid);
                            this.database.ExecuteNonQuery(sqlStringCommand2, Tran);
                        }
                    }

                    //提交事务
                    Tran.Commit();

                    result = true;
                }
                catch (Exception Ex)
                {
                    //出错回滚
                    Tran.Rollback();
                    ErrorLog.Write(Ex.ToString());
                }
                finally
                {
                    //关闭连接
                    connection.Close();
                }

                return result;
            }
		}
		private string BuildLeaveCommentQuery(LeaveCommentQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT l.LeaveId FROM Ecshop_LeaveComments l where 0=0");
			if (query.MessageStatus == MessageStatus.Replied)
			{
				stringBuilder.Append(" and (select Count(ReplyId) from Ecshop_LeaveCommentReplys where LeaveId=l.LeaveId) >0 ");
			}
			if (query.MessageStatus == MessageStatus.NoReply)
			{
				stringBuilder.Append(" and (select Count(ReplyId) from Ecshop_LeaveCommentReplys where LeaveId=l.LeaveId) <=0 ");
			}
            if (query.FeedbackType.HasValue)
            {
                stringBuilder.AppendFormat(" and FeedbackType = {0}", query.FeedbackType.Value);
            }
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			else
			{
				stringBuilder.Append(" ORDER BY LastDate desc");
			}
			return stringBuilder.ToString();
		}
	}
}
