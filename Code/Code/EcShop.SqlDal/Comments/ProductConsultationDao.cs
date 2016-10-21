using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Comments
{
	public class ProductConsultationDao
	{
		private Database database;
		public ProductConsultationDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
			if (consultationQuery.Type == ConsultationReplyType.NoReply)
			{
				stringBuilder.Append(" AND ReplyUserId IS NULL ");
			}
			else
			{
				if (consultationQuery.Type == ConsultationReplyType.Replyed)
				{
					stringBuilder.Append(" AND ReplyUserId IS NOT NULL");
				}
			}
			if (consultationQuery.ProductId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0}", consultationQuery.ProductId);
			}
			if (consultationQuery.UserId > 0)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", consultationQuery.UserId);
			}
			if (!string.IsNullOrEmpty(consultationQuery.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
			}
			if (consultationQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (CategoryId = {0}", consultationQuery.CategoryId.Value);
				stringBuilder.AppendFormat(" OR CategoryId IN (SELECT CategoryId FROM Ecshop_Categories WHERE Path LIKE (SELECT Path FROM Ecshop_Categories WHERE CategoryId = {0}) + '%'))", consultationQuery.CategoryId.Value);
			}
			if (consultationQuery.HasReplied.HasValue)
			{
				if (consultationQuery.HasReplied.Value)
				{
					stringBuilder.AppendFormat(" AND ReplyText is not null", new object[0]);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ReplyText is null", new object[0]);
				}
			}
			return DataHelper.PagingByRownumber(consultationQuery.PageIndex, consultationQuery.PageSize, consultationQuery.SortBy, consultationQuery.SortOrder, consultationQuery.IsCount, "vw_Ecshop_ProductConsultations", "ProductId", stringBuilder.ToString(), "*");
		}
		public int GetProductConsultationsCount(int productId, bool includeUnReplied)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT count(1) FROM Ecshop_ProductConsultations WHERE ProductId =" + productId);
			if (!includeUnReplied)
			{
				stringBuilder.Append(" AND ReplyText is not null");
			}
			return (int)this.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString());
		}
		public ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			ProductConsultationInfo result = null;
			string query = "SELECT * FROM Ecshop_ProductConsultations WHERE ConsultationId=@ConsultationId";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, consultationId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ProductConsultationInfo>(dataReader);
			}
			return result;
		}
		public bool InsertProductConsultation(ProductConsultationInfo productConsultation)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ProductConsultations(ProductId, UserId, UserName, UserEmail, ConsultationText, ConsultationDate)VALUES(@ProductId, @UserId, @UserName, @UserEmail, @ConsultationText, @ConsultationDate)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productConsultation.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, productConsultation.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, productConsultation.UserName);
			this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, productConsultation.UserEmail);
			this.database.AddInParameter(sqlStringCommand, "ConsultationText", DbType.String, productConsultation.ConsultationText);
			this.database.AddInParameter(sqlStringCommand, "ConsultationDate", DbType.DateTime, productConsultation.ConsultationDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_ProductConsultations SET ReplyText = @ReplyText, ReplyDate = @ReplyDate, ReplyUserId = @ReplyUserId WHERE ConsultationId = @ConsultationId");
			this.database.AddInParameter(sqlStringCommand, "ReplyText", DbType.String, productConsultation.ReplyText);
			this.database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.DateTime, productConsultation.ReplyDate);
			this.database.AddInParameter(sqlStringCommand, "ReplyUserId", DbType.Int32, productConsultation.ReplyUserId);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, productConsultation.ConsultationId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public int DeleteProductConsultation(int consultationId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ProductConsultations WHERE consultationId = @consultationId");
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int64, consultationId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_ConsultationsAndReplys_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildConsultationAndReplyQuery(query));
			this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
			DataSet dataSet = this.database.ExecuteDataSet(storedProcCommand);
			dataSet.Relations.Add("ConsultationReplys", dataSet.Tables[0].Columns["ConsultationId"], dataSet.Tables[1].Columns["ConsultationId"], false);
			total = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return dataSet;
		}
		private string BuildConsultationAndReplyQuery(ProductConsultationAndReplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT ConsultationId FROM Ecshop_ProductConsultations ");
			stringBuilder.Append(" WHERE 1 = 1");
			if (query.ProductId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0} ", query.ProductId);
			}
			if (query.UserId > 0)
			{
				stringBuilder.AppendFormat(" AND UserId = {0} ", query.UserId);
			}
			if (query.Type == ConsultationReplyType.NoReply)
			{
				stringBuilder.Append(" AND ReplyText IS NULL");
			}
			else
			{
				if (query.Type == ConsultationReplyType.Replyed)
				{
					stringBuilder.Append(" AND ReplyText IS NOT NULL");
				}
			}
			stringBuilder.Append(" ORDER BY replydate DESC");
			return stringBuilder.ToString();
		}
	}
}
