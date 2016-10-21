using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Promotions
{
	public class CountDownDao
	{
		private Database database;
		public CountDownDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetCountDownList(GroupBuyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1  ");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat("and ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
            if (query.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat("and SupplierId  = {0}", query.SupplierId);
            }
            string selectFields = "CountDownId,productId,ProductName,CountDownPrice,StartDate,EndDate,DisplaySequence,MaxCount,PlanCount,SupplierId";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown", "CountDownId", stringBuilder.ToString(), selectFields);
		}
        /// <summary>
        ///导出广告订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult ExprtAdOrderInfoList(GroupBuyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1  ");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat("and OrderNo Like '%{0}%'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (!string.IsNullOrEmpty(query.starttime) && query.starttime!="0001/1/1 0:00:00")
            {
                stringBuilder.AppendFormat("and OrderTIme>='{0}'", DataHelper.CleanSearchString(query.starttime));
            }
            if (!string.IsNullOrEmpty(query.endtime) && query.endtime != "0001/1/1 0:00:00")
            {
                stringBuilder.AppendFormat("and OrderTIme<'{0}'", DataHelper.CleanSearchString(query.endtime));
            }
            if (!string.IsNullOrEmpty(query.keyword))
            {
                stringBuilder.AppendFormat("and (OrderNo like '%{0}%'  or ProductName like '%{0}%'  or OrderStatus like '%{0}%'  or PaymentType like '%{0}%' or paymentStatus  like '%{0}%')", DataHelper.CleanSearchString(query.keyword));
            }
            string sql = @"select 
                            OrderNo,
                            OrderTotal,
                            ProductName,
                            BarCode,
                            sku,
                            ItemAdjustedPrice,
                            OrderStatus,
                            PaymentType,
                            paymentStatus,
                            OrderTIme,
                            UpdateDate
                            from vw_GetAdOrderList  as a where  " + stringBuilder.ToString();

            DbCommand storedProcCommand = this.database.GetSqlStringCommand(sql);
            DbQueryResult rs = new DbQueryResult();
            rs.Data = this.database.ExecuteDataSet(storedProcCommand).Tables[0];
            return rs;
        }
        /// <summary>
        ///获取广告订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetAdOrderInfoList(GroupBuyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1  ");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat("and OrderNo Like '%{0}%'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (!string.IsNullOrEmpty(query.starttime) && query.starttime != "0001/1/1 0:00:00")
            {
                stringBuilder.AppendFormat("and OrderTIme>='{0}'", DataHelper.CleanSearchString(query.starttime));
            }
            if (!string.IsNullOrEmpty(query.endtime) && query.endtime != "0001/1/1 0:00:00")
            {
                stringBuilder.AppendFormat("and OrderTIme<'{0}'", DataHelper.CleanSearchString(query.endtime));
            }
            if(!string.IsNullOrEmpty(query.keyword))
            {
                stringBuilder.AppendFormat("and (OrderNo like '%{0}%'  or ProductName like '%{0}%' or OrderStatus like '%{0}%'  or PaymentType like '%{0}%' or paymentStatus  like '%{0}%')", DataHelper.CleanSearchString(query.keyword));
            }
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_GetAdOrderList", "OrderTIme", stringBuilder.ToString(), "*");
        }
        /// <summary>
        /// 限时抢购活动管理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetCountManagerDownList(GroupBuyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1  ");
            if (!string.IsNullOrEmpty(query.Title))
            {
                stringBuilder.AppendFormat("and Title Like '%{0}%'", DataHelper.CleanSearchString(query.Title));
            }

            string selectFields = "CountDownCategoryId,Title,StartTime,EndTime,AdImageUrl,AdImageLinkUrl,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_CountDownCategories", "CountDownCategoryId", stringBuilder.ToString(), selectFields);
        }
		public void SwapCountDownSequence(int countDownId, int displaySequence)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_CountDown SET DisplaySequence = @DisplaySequence WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, displaySequence);
			this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
        /// <summary>
        /// 报错序列
        /// </summary>
        /// <param name="CountDownCategoryId"></param>
        /// <param name="displaySequence"></param>
        public void SwapCountCategoriesDownSequence(int CountDownCategoryId, int displaySequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_CountDownCategories SET DisplaySequence = @DisplaySequence WHERE CountDownCategoryId=@CountDownCategoryId");
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, displaySequence);
            this.database.AddInParameter(sqlStringCommand, "CountDownCategoryId", DbType.Int32, CountDownCategoryId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
		public bool DeleteCountDown(int countDownId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_CountDown WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="CountDownCategoryId"></param>
        /// <returns></returns>
        public bool DeleteCountDownCategories(int CountDownCategoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_CountDownCategories WHERE CountDownCategoryId=@CountDownCategoryId");
            this.database.AddInParameter(sqlStringCommand, "CountDownCategoryId", DbType.Int32, CountDownCategoryId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
		public bool AddCountDown(CountDownInfo countDownInfo)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_CountDown;INSERT INTO Ecshop_CountDown(ProductId,CountDownPrice,StartDate,EndDate,Content,DisplaySequence,MaxCount,PlanCount,SupplierId ) VALUES(@ProductId,@CountDownPrice,@StartDate,@EndDate,@Content,@DisplaySequence,@MaxCount,@PlanCount,@SupplierId );");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
			this.database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, countDownInfo.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, countDownInfo.MaxCount);
            this.database.AddInParameter(sqlStringCommand, "PlanCount", DbType.Int32, countDownInfo.PlanCount);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, countDownInfo.SupplierId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
        /// <summary>
        /// 保存限时活动
        /// </summary>
        /// <param name="countDownInfo"></param>
        /// <returns></returns>
        public bool AddCountDownCategories(CountDownCategoriesInfo countDownInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_CountDownCategories;
            INSERT INTO Ecshop_CountDownCategories(Title,StartTime,EndTime,AdImageUrl,AdImageLinkUrl,DisplaySequence,CreatedBy,CreatedOn ) 
                                        VALUES(@Title,@StartTime,@EndTime,@AdImageUrl,@AdImageLinkUrl,@DisplaySequence,@CreatedBy,@CreatedOn);");

            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, countDownInfo.Title);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.Time, countDownInfo.StartTime.ToLongTimeString());
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.Time, countDownInfo.EndTime.ToLongTimeString());
            this.database.AddInParameter(sqlStringCommand, "AdImageUrl", DbType.String, countDownInfo.AdImageUrl);
            this.database.AddInParameter(sqlStringCommand, "AdImageLinkUrl", DbType.String, countDownInfo.AdImageLinkUrl);
            this.database.AddInParameter(sqlStringCommand, "CreatedBy", DbType.Int32, countDownInfo.CreatedBy);
            this.database.AddInParameter(sqlStringCommand, "CreatedOn", DbType.DateTime, DateTime.Now);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
		public bool UpdateCountDown(CountDownInfo countDownInfo)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_CountDown SET ProductId=@ProductId,CountDownPrice=@CountDownPrice,StartDate=@StartDate,EndDate=@EndDate,Content=@Content,MaxCount=@MaxCount,PlanCount=@PlanCount  WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownInfo.CountDownId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
			this.database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, countDownInfo.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, countDownInfo.MaxCount);
            this.database.AddInParameter(sqlStringCommand, "PlanCount", DbType.Int32, countDownInfo.PlanCount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
        /// <summary>
        /// 修改限时活动管理
        /// </summary>
        /// <param name="countDownInfo"></param>
        /// <returns></returns>
        public bool UpdateCountDownDownCategories(CountDownCategoriesInfo countDownInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update  Ecshop_CountDownCategories
                                          set 
	                                          Title=@Title,
	                                          StartTime=@StartTime,
	                                          EndTime=@EndTime,
	                                          AdImageUrl=@AdImageUrl,
	                                          AdImageLinkUrl=@AdImageLinkUrl,
	                   
	                                          UpdatedBy=@UpdatedBy,
	                                          UpdatedOn=@UpdatedOn
                                          where CountDownCategoryId=@CountDownCategoryId");

            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, countDownInfo.Title);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.Time, countDownInfo.StartTime.ToLongTimeString());
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.Time, countDownInfo.EndTime.ToLongTimeString());
            this.database.AddInParameter(sqlStringCommand, "AdImageUrl", DbType.String, countDownInfo.AdImageUrl);
            this.database.AddInParameter(sqlStringCommand, "AdImageLinkUrl", DbType.String, countDownInfo.AdImageLinkUrl);
     
            this.database.AddInParameter(sqlStringCommand, "UpdatedBy", DbType.Int32, countDownInfo.UpdatedBy);
            this.database.AddInParameter(sqlStringCommand, "UpdatedOn", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "CountDownCategoryId", DbType.Int32, countDownInfo.CountDownCategoryId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
		public bool ProductCountDownExist(int productId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_CountDown WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
        public bool ProductCountDownExist(int productId, DateTime startDate, DateTime endDate, int countDownId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_CountDown WHERE ProductId=@ProductId AND ((StartDate <= @StartDate AND EndDate > @StartDate) OR (StartDate < @EndDate AND EndDate >= @EndDate)) AND CountDownId <> @CountDownId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, startDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, endDate);
            this.database.AddInParameter(sqlStringCommand, "countDownId", DbType.Int32, countDownId);
            return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
        }

        public CountDownInfo GetCountDownInfo(int countDownId)
		{
			CountDownInfo result = null;
            StringBuilder strbu = new StringBuilder();
            strbu.Append("SELECT * FROM Ecshop_CountDown WHERE CountDownId=@CountDownId");
           
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strbu.ToString());
			this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}

        /// <summary>
        /// 获取限时活动管理对象
        /// </summary>
        /// <param name="CountDownCategoryId"></param>
        /// <returns></returns>
        public CountDownCategoriesInfo GetCountDownnCategoriesInfo(int CountDownCategoryId)
        {
            CountDownCategoriesInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_CountDownCategories WHERE CountDownCategoryId=@CountDownCategoryId");
            this.database.AddInParameter(sqlStringCommand, "CountDownCategoryId", DbType.Int32, CountDownCategoryId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateCountDownCategories(dataReader);
                }
            }
            return result;
        }

        public int GetCountDownSaleCountByCountDownId(int countDownId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SaleCount, 0) FROM vw_Ecshop_CountDown WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);

            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public CountDownInfo GetCountDownByProductId(int productId, int countDownId = 0)
		{
			CountDownInfo result = null;
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT TOP 1 * FROM Ecshop_CountDown WHERE /*datediff(hh,EndDate,getdate())<=0*/ EndDate >= getdate() AND ProductId=@ProductId ");
            if (countDownId > 0)
            {
                sql.Append(" and CountDownId = @CountDownId  ");
            }
            sql.Append("  ORDER BY EndDate ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql.ToString());
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
		{
			string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown", "CountDownId", filter, "*");
		}

        public DbQueryResult GetCountDownProductList_Active_temp(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown_active_temp", "CountDownId", filter, "*");
        }
        public DbQueryResult GetCountDownProductList_Active_temp2(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown_active_temp2", "CountDownId", filter, "*");
        }
        public DbQueryResult GetCountDownProductList_Active_temp3(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown_active_temp3", "CountDownId", filter, "*");
        }
        public DbQueryResult GetCountDownProductList_Active_temp4(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown_active_temp4", "CountDownId", filter, "*");
        }
        public DbQueryResult GetCountDownProductList_Active_temp5(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown_active_temp5", "CountDownId", filter, "*");
        }

        public DbQueryResult GetActivityProductList(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(d,EndDate,getdate())<1 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CountDown", "CountDownId", filter, "*");
        }

		public DataTable GetCountDownProductList(int? categoryId, string keyWord, int page, int size, out int total, bool onlyUnFinished = true)
		{
			StringBuilder sbSql = new StringBuilder();
			//sbSql.Append("a.CountDownId,a.ProductId,a.ProductName,b.ProductCode,b.ShortDescription,");
			//sbSql.Append(" a.ThumbnailUrl60,a.ThumbnailUrl100,a.ThumbnailUrl160,a.ThumbnailUrl180,a.ThumbnailUrl220,a.ThumbnailUrl310,a.MarketPrice,b.SalePrice,a.MaxCount");
            sbSql.Append("a.SupplierId,a.CountDownId,a.ProductId,a.ProductName,a.ProductCode,a.ShortDescription,a.MarketPrice,a.SalePrice,a.CountDownPrice,a.StartDate,a.EndDate,a.MaxCount,a.PlanCount,a.DisplaySequence,a.ThumbnailUrl40,a.ThumbnailUrl60,a.ThumbnailUrl100,a.ThumbnailUrl160,a.ThumbnailUrl180,a.ThumbnailUrl220,a.ThumbnailUrl310,a.MainCategoryPath,a.ExtendCategoryPath ");
			StringBuilder sbTable = new StringBuilder();
			//sbTable.Append(" vw_Ecshop_CountDown a left join vw_Ecshop_BrowseProductList b on a.ProductId = b.ProductId  ");
            sbTable.Append(" vw_Ecshop_CountDown a ");
			StringBuilder sbWhere = new StringBuilder(" SaleStatus=1");
			if (onlyUnFinished)
			{
				string arg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				sbWhere.AppendFormat(" AND ( a.StartDate <= '{0}' ) AND ( a.EndDate >= '{0}') ", arg);
			}
			if (categoryId.HasValue)
			{
				CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
				if (category != null)
				{
					sbWhere.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
				}
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
			    keyWord = DataHelper.CleanSearchString(keyWord);
				sbWhere.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
			}
			string sortBy = "a.DisplaySequence";
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(page, size, sortBy, SortAction.Desc, true, sbTable.ToString(), "CountDownId", sbWhere.ToString(), sbSql.ToString());
			DataTable result = (DataTable)dbQueryResult.Data;
			total = dbQueryResult.TotalRecords;
			return result;
		}

        /// <summary>
        /// 获取限时抢购管理
        /// </summary>
        /// <returns></returns>
        public DataTable SelectCountDownCategories()
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append(" vw_Ecshop_CountDownCategories a ");

            string sortBy = "a.Startdate";
            DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(1, 100, sortBy, SortAction.Asc, true, sbTable.ToString(), "CountDownCategoryId", "", "*");

            return (DataTable)dbQueryResult.Data;
        }

        /// <summary>
        /// 根据活动状态获取参加活动的商品列表
        /// </summary>
        /// <param name="ActoveTupe">活动类型:1、抢购中；2、未开始;3、已结束</param>
        /// <returns></returns>
        public DataTable GetDownCateProducts(int CountDownCateGoryId, int ActiveType)
        {
             DataTable dt = new DataTable();
             DbCommand storedProcCommand = this.database.GetStoredProcCommand("proc_DownCategoriesProducts");
             this.database.AddInParameter(storedProcCommand, "@CountDownCateGoryId", DbType.Int32, CountDownCateGoryId);
             this.database.AddInParameter(storedProcCommand, "@ActiveType", DbType.Int32, ActiveType);
             using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
             {
                 dt = DataHelper.ConverDataReaderToDataTable(dataReader);
             }
             return dt;
        }


        public DataTable GetCountDownProductList(DateTime start, DateTime end, int page, int size, out int total)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("a.CategoryId,a.SupplierId,a.CountDownId,a.ProductId,a.ProductName,a.ProductCode,a.ShortDescription,a.MarketPrice,a.SalePrice,a.CountDownPrice,");
            sbSql.Append("a.StartDate,a.EndDate,a.MaxCount,a.PlanCount,a.DisplaySequence,a.ThumbnailUrl40,a.ThumbnailUrl60,a.ThumbnailUrl100,a.ThumbnailUrl160,a.ThumbnailUrl180,");
            sbSql.Append("a.ThumbnailUrl220,a.ThumbnailUrl310,a.MainCategoryPath,a.ExtendCategoryPath,a.TaxRate, a.Stock, a.SaleCounts,a.SaleCount ");

            StringBuilder sbTable = new StringBuilder();
            sbTable.Append(" vw_Ecshop_CountDown a ");

            StringBuilder sbWhere = new StringBuilder(" a.SaleStatus=1");
            sbWhere.AppendFormat(" and a.StartDate = '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and a.EndDate = '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "'");

            string sortBy = "a.StartDate";
            DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(page, size, sortBy, SortAction.Asc, true, sbTable.ToString(), "CountDownId", sbWhere.ToString(), sbSql.ToString());
            DataTable result = (DataTable)dbQueryResult.Data;
            total = dbQueryResult.TotalRecords;
            return result;
        }

        public DataTable GetCountDownProductList(int count)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("a.CategoryId,a.SupplierId,a.CountDownId,a.ProductId,a.ProductName,a.ProductCode,a.ShortDescription,a.MarketPrice,a.SalePrice,a.CountDownPrice,");
            sbSql.Append("a.StartDate,a.EndDate,a.MaxCount,a.PlanCount,a.DisplaySequence,a.ThumbnailUrl40,a.ThumbnailUrl60,a.ThumbnailUrl100,a.ThumbnailUrl160,a.ThumbnailUrl180,");
            sbSql.Append("a.ThumbnailUrl220,a.ThumbnailUrl310,a.MainCategoryPath,a.ExtendCategoryPath,a.TaxRate, a.Stock, a.SaleCounts,a.SaleCount ");

            StringBuilder sbTable = new StringBuilder();
            sbTable.Append(" vw_Ecshop_CountDown a join vw_Ecshop_CountDownCategories b on b.StartDate = a.StartDate and b.EndDate = a.EndDate ");

            StringBuilder sbWhere = new StringBuilder(" a.SaleStatus=1");
            sbWhere.AppendFormat(" and a.EndDate >= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"); // and a.StartDate <= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");

            string sortBy = "a.StartDate";
            DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(1, count, sortBy, SortAction.Asc, true, sbTable.ToString(), "CountDownId", sbWhere.ToString(), sbSql.ToString());
            DataTable result = (DataTable)dbQueryResult.Data;
            
            return result;
        }

        public DbQueryResult GetCountDownCategories()
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append(" vw_Ecshop_CountDownCategories a ");

            string sortBy = "a.StartDate";
            DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(1, 100, sortBy, SortAction.Asc, true, sbTable.ToString(), "CountDownCategoryId", "", "*");
 
            return dbQueryResult;
        }

        public DbQueryResult GetCountDownCategories(int pageindex,int pagesize)
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append(" vw_Ecshop_CountDownCategories a ");

            string sortBy = "a.StartDate";
            DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(pageindex, pagesize, sortBy, SortAction.Asc, true, sbTable.ToString(), "CountDownCategoryId", "", "*");

            return dbQueryResult;
        }

		public DataTable GetCounDownProducList(int maxnum)
		{
			DataTable result = new DataTable();
			string query = string.Format("select top " + maxnum + " CountDownId,ProductId,ProductName,SalePrice,CountDownPrice,StartDate,EndDate, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310 from vw_Ecshop_CountDown where datediff(hh,EndDate,getdate())<=0 AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0}) order by DisplaySequence desc", 1);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
	}
}
