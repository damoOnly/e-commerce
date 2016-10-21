using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Comments
{
	public class ProductReviewDao
	{
		private Database database;
		public ProductReviewDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.Keywords));
			if (!string.IsNullOrEmpty(reviewQuery.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.ProductCode));
			}
			if (reviewQuery.productId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0}", reviewQuery.productId);
			}
			if (reviewQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (CategoryId = {0}", reviewQuery.CategoryId.Value);
				stringBuilder.AppendFormat(" OR  CategoryId IN (SELECT CategoryId FROM Ecshop_Categories WHERE Path LIKE (SELECT Path FROM Ecshop_Categories WHERE CategoryId = {0}) + '%'))", reviewQuery.CategoryId.Value);
			}

            stringBuilder.Append(" and IsNull(IsType,0)=0  ");
			return DataHelper.PagingByRownumber(reviewQuery.PageIndex, reviewQuery.PageSize, reviewQuery.SortBy, reviewQuery.SortOrder, reviewQuery.IsCount, "vw_Ecshop_ProductReviews", "ProductId", stringBuilder.ToString(), "*");
		}

        public IList<ProductReviewInfo> GetSimpleProductReviews(int productId)
        {
            IList<ProductReviewInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 8 * FROM Ecshop_ProductReviews WHERE ProductId=@ProductId order by ReviewDate desc ");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<ProductReviewInfo>(dataReader);
              
            }

            return result;

        }

        public DataTable GetReviewsCountAndAvg(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT round(avg(cast(Score as decimal(5, 1))),1) as avg,count(1) as Amount FROM Ecshop_ProductReviews WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
		public int GetProductReviewsCount(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT count(1) FROM Ecshop_ProductReviews WHERE ProductId =" + productId);
			return (int)this.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString());
		}
        public void GetProductReviewsSummary(int productId, out int count, out int score)
        {
            count = 0;
            score = 0;

            DbCommand command = this.database.GetSqlStringCommand("SELECT COUNT(*) Quantity, ISNULL(SUM(ISNULL(Score, 5)), 0) Score FROM Ecshop_ProductReviews WHERE ProductId = @ProductId and IsNull(IsType,0)=0 ");
            this.database.AddInParameter(command, "ProductId", DbType.Int32, productId);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                if (dataReader.Read())
                {
                    count = int.Parse(dataReader["Quantity"].ToString());
                    score = int.Parse(dataReader["Score"].ToString());
                }
            }
        }
		public ProductReviewInfo GetProductReview(int reviewId)
		{
			ProductReviewInfo result = new ProductReviewInfo();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ProductReviews WHERE ReviewId=@ReviewId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int32, reviewId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = ReaderConvert.ReaderToModel<ProductReviewInfo>(dataReader);
				}
			}
			return result;
		}


        /// <summary>
        /// 获取用户对该商品的评论
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ProductReviewInfo GetProductReview(int productId, string skuId, string orderId, int userId)
        {
            ProductReviewInfo result = new ProductReviewInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1  * FROM Ecshop_ProductReviews WHERE ProductId=@ProductId AND OrderId=@OrderId AND SkuId=@SkuId AND UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateProductReview(dataReader);
                }
            }
            return result;
        }


		public bool InsertProductReview(ProductReviewInfo review)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ProductReviews (ProductId, UserId, ReviewText, UserName, UserEmail, ReviewDate,OrderId,SkuId,Score,IsAnonymous) VALUES(@ProductId, @UserId, @ReviewText, @UserName, @UserEmail, @ReviewDate,@OrderId,@SkuId,@Score,@IsAnonymous)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, review.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, review.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReviewText", DbType.String, review.ReviewText);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, (string.IsNullOrEmpty(review.UserName) ? "" : review.UserName));
			this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, (string.IsNullOrEmpty(review.UserEmail) ? "" : review.UserEmail));
			this.database.AddInParameter(sqlStringCommand, "ReviewDate", DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, review.OrderID);
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, review.SkuId);
            this.database.AddInParameter(sqlStringCommand, "Score", DbType.Int32, review.Score);
            this.database.AddInParameter(sqlStringCommand, "IsAnonymous", DbType.Boolean, review.IsAnonymous);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DataTable GetProductReviewAll(string orderid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select   Productid,SkuId from Ecshop_OrderItems where (Productid not in (select Productid from Ecshop_ProductReviews  where OrderId=@OrderId) or SkuId not in (select SkuId from Ecshop_ProductReviews  where OrderId=@OrderId)) and OrderId=@OrderId");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
		public DataTable GetProductReviewAll(string orderid, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT r.ReviewId, i.ProductId, o.UserId, r.ReviewText, r.UserName, r.UserEmail, r.ReviewDate, i.OrderId, i.SkuId, r.Score, r.IsAnonymous, i.ItemDescription ProductName,i.SKU,i.SKUContent,i.Quantity,i.ItemAdjustedPrice Price, i.ThumbnailsUrl, i.TaxRate FROM Ecshop_OrderItems i JOIN Ecshop_Orders o ON o.OrderId = i.OrderId LEFT JOIN [dbo].[Ecshop_ProductReviews] r ON r.OrderId = i.OrderId AND r.ProductId = i.ProductId AND r.SkuId = i.SkuId AND r.UserId = o.UserId WHERE o.OrderId = @OrderId AND o.UserId = @UserId");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);

			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
        public DataTable GetOrderReviewAll(string orderIds, int userId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("select OrderId, count(distinct SkuId) SkuQuantity from [dbo].[Ecshop_ProductReviews] where UserId = @UserId and OrderId in ('{0}') group by OrderId", orderIds.Replace(",", "','"));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);

            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

		public int DeleteProductReview(long reviewId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ProductReviews WHERE ReviewId = @ReviewId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int64, reviewId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public void LoadProductReview(int productId, int userId, out int buyNum, out int reviewNum, string OrderId = "")
		{
			buyNum = 0;
			reviewNum = 0;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SELECT COUNT(*) FROM Ecshop_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId" + (string.IsNullOrEmpty(OrderId) ? ";" : " and OrderId=@OrderId;"));
			if (string.IsNullOrEmpty(OrderId))
			{
				stringBuilder.AppendLine(" SELECT ISNULL(SUM(Quantity), 0) FROM Ecshop_OrderItems WHERE ProductId=@ProductId AND OrderId IN " + string.Format(" (SELECT OrderId FROM Ecshop_Orders WHERE UserId = @UserId AND OrderStatus = {0})", 5));
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					reviewNum = (int)dataReader[0];
				}
				if (!string.IsNullOrEmpty(OrderId))
				{
					buyNum = 1;
				}
				else
				{
					dataReader.NextResult();
					if (dataReader.Read())
					{
						buyNum = (int)dataReader[0];
					}
				}
			}
		}
		public int GetUserProductReviewsCount()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ReviewId) AS Count FROM Ecshop_ProductReviews WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
			int result = 0;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					if (DBNull.Value != dataReader["Count"])
					{
						result = (int)dataReader["Count"];
					}
				}
			}
			return result;
		}
		public DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_UserReviewsAndReplys_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildUserReviewsAndReplysQuery(query));
			this.database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
			DataSet dataSet = this.database.ExecuteDataSet(storedProcCommand);
			dataSet.Relations.Add("PtReviews", dataSet.Tables[0].Columns["ProductId"], dataSet.Tables[1].Columns["ProductId"], false);
			total = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return dataSet;
		}
		private string BuildUserReviewsAndReplysQuery(UserProductReviewAndReplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT ProductId FROM Ecshop_ProductReviews ");
			stringBuilder.AppendFormat(" WHERE UserId = {0} ", HiContext.Current.User.UserId);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Ecshop_Products)", new object[0]);
			stringBuilder.Append(" GROUP BY ProductId");
			return stringBuilder.ToString();
		}
	}
}
