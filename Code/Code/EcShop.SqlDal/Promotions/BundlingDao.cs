using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Promotions
{
	public class BundlingDao
	{
		private Database database;
		public BundlingDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND Name Like '%{0}%' ", DataHelper.CleanSearchString(query.ProductName));
			}
            if (query.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" AND SupplierId = {0} ", query.SupplierId);
            }
            string selectFields = "Bundlingid,Name,Num,price,SaleStatus,OrderCount,AddTime,DisplaySequence,ShortDescription,SupplierId";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BundlingProducts", "Bundlingid", stringBuilder.ToString(), selectFields);
		}
		public PromotionInfo GetProductPromotionInfo(int productid, Member member)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select * from Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            stringBuilder.Append("and ActivityId IN (select ActivityId from dbo.Ecshop_PromotionProducts where productid=@productid)");
			stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = @GradeId)");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
			this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
			PromotionInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePromote(dataReader);
				}
			}
			return result;
		}
		public PromotionInfo GetAllProductPromotionInfo(int productid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select top 1 * from Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            stringBuilder.Append("and ActivityId IN (select ActivityId from dbo.Ecshop_PromotionProducts where productid=@productid)");
			stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
			PromotionInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePromote(dataReader);
				}
			}
			return result;
		}

        public DataTable GetAllProductPromotionInfo(List<int> productIds)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select t.ProductId, t.ActivityId, t.PromotionName ");
            sbSql.Append("from ( ");
            sbSql.Append("		select b.ProductId, b.ActivityId, a.Name as PromotionName, row_number() over(partition by b.ProductId order by StartDate desc) PartNo ");
            sbSql.Append("		from Ecshop_Promotions a  ");
            sbSql.Append("			join Ecshop_PromotionProducts b on b.ActivityId = a.ActivityId  ");
            sbSql.Append("		where StartDate < getdate() and EndDate > getdate() ");
            sbSql.AppendFormat("			and b.ProductId in ({0})", string.Join(",", productIds));
            sbSql.Append("			and a.ActivityId in (select ActivityId from Ecshop_PromotionMemberGrades) ");
            sbSql.Append("	) t ");
            sbSql.Append("where t.PartNo = 1 ");

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sbSql.ToString());
            
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
        }

        public DataTable GetProductPromotionInfo(Member member, List<int> productIds)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select t.ProductId, t.ActivityId, t.PromotionName ");
            sbSql.Append("from ( ");
            sbSql.Append("		select b.ProductId, b.ActivityId, a.Name as PromotionName, row_number() over(partition by b.ProductId order by StartDate desc) PartNo ");
            sbSql.Append("		from Ecshop_Promotions a  ");
            sbSql.Append("			join Ecshop_PromotionProducts b on b.ActivityId = a.ActivityId  ");
            sbSql.Append("		where StartDate < getdate() and EndDate > getdate() ");
            sbSql.AppendFormat("			and b.ProductId in ({0})", string.Join(",", productIds));
            sbSql.Append("			and a.ActivityId in (select ActivityId from Ecshop_PromotionMemberGrades where GradeId = @GradeId) ");
            sbSql.Append("	) t ");
            sbSql.Append("where t.PartNo = 1 ");

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sbSql.ToString());
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);

            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
        }

		public BundlingInfo GetBundlingInfo(int bundlingID)
		{
			BundlingInfo bundlingInfo = null;
			StringBuilder stringBuilder = new StringBuilder("SELECT * FROM Ecshop_BundlingProducts WHERE BundlingID=@BundlingID;");
			stringBuilder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName,");
			stringBuilder.Append(" (select saleprice FROM  Ecshop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
			stringBuilder.Append(" FROM  Ecshop_BundlingProductItems a JOIN Ecshop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
			List<BundlingItemInfo> list = new List<BundlingItemInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					bundlingInfo = DataMapper.PopulateBindInfo(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					BundlingItemInfo bundlingItemInfo = new BundlingItemInfo();
					bundlingItemInfo.ProductID = (int)dataReader["ProductID"];
					bundlingItemInfo.ProductNum = (int)dataReader["ProductNum"];
					if (dataReader["SkuId"] != DBNull.Value)
					{
						bundlingItemInfo.SkuId = (string)dataReader["SkuId"];
					}
					if (dataReader["ProductName"] != DBNull.Value)
					{
						bundlingItemInfo.ProductName = (string)dataReader["ProductName"];
					}
					if (dataReader["ProductPrice"] != DBNull.Value)
					{
						bundlingItemInfo.ProductPrice = (decimal)dataReader["ProductPrice"];
					}
					bundlingItemInfo.BundlingID = bundlingID;
					list.Add(bundlingItemInfo);
				}
			}
			bundlingInfo.BundlingItemInfos = list;
			return bundlingInfo;
		}
		public int AddBundlingProduct(BundlingInfo bind, DbTransaction dbTran)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_BundlingProducts;INSERT INTO Ecshop_BundlingProducts(Name,ShortDescription,Num,Price,SaleStatus,AddTime,DisplaySequence,SupplierId) VALUES(@Name,@ShortDescription,@Num,@Price,@SaleStatus,@AddTime,@DisplaySequence,@SupplierId); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, bind.Name);
			this.database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, bind.ShortDescription);
			this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int32, bind.Num);
			this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, bind.Price);
			this.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, bind.SaleStatus);
			this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, bind.AddTime);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, bind.SupplierId);
			object obj;
			if (dbTran != null)
			{
				obj = this.database.ExecuteScalar(sqlStringCommand, dbTran);
			}
			else
			{
				obj = this.database.ExecuteScalar(sqlStringCommand);
			}
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
		public bool AddBundlingProductItems(int bundlingID, List<BundlingItemInfo> BundlingItemInfos, DbTransaction dbTran)
		{
			bool result;
			if (BundlingItemInfos.Count <= 0)
			{
				result = false;
			}
			else
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_BundlingProductItems(BundlingID,ProductID,SkuId,ProductNum) VALUES(@BundlingID,@ProductID,@Skuid,@ProductNum)");
				this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
				this.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32);
				this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String);
				this.database.AddInParameter(sqlStringCommand, "ProductNum", DbType.Int32);
				try
				{
					foreach (BundlingItemInfo current in BundlingItemInfos)
					{
						this.database.SetParameterValue(sqlStringCommand, "ProductID", current.ProductID);
						this.database.SetParameterValue(sqlStringCommand, "SkuId", current.SkuId);
						this.database.SetParameterValue(sqlStringCommand, "ProductNum", current.ProductNum);
						if (dbTran != null)
						{
							this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							this.database.ExecuteNonQuery(sqlStringCommand);
						}
					}
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}
		public bool DeleteBundlingProduct(int BundlingID)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_BundlingProducts WHERE BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, BundlingID);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateBundlingProduct(BundlingInfo bind, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_BundlingProducts  SET Name=@Name,ShortDescription=@ShortDescription,Num=@Num,Price=@Price,SaleStatus=@SaleStatus,AddTime=@AddTime WHERE BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, bind.Name);
			this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bind.BundlingID);
			this.database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, bind.ShortDescription);
			this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int32, bind.Num);
			this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, bind.Price);
			this.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, bind.SaleStatus);
			this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, bind.AddTime);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public bool DeleteBundlingByID(int BundlingID, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_BundlingProductItems WHERE  BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, BundlingID);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public List<BundlingItemInfo> GetBundlingItemsByID(int bundlingID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName, ");
			stringBuilder.Append(" (select saleprice FROM  Ecshop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
			stringBuilder.Append(" FROM  Ecshop_BundlingProductItems a JOIN Ecshop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
			List<BundlingItemInfo> list = new List<BundlingItemInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					BundlingItemInfo bundlingItemInfo = new BundlingItemInfo();
					bundlingItemInfo.ProductID = (int)dataReader["ProductID"];
					bundlingItemInfo.ProductNum = (int)dataReader["ProductNum"];
					bundlingItemInfo.SkuId = (string)dataReader["SkuId"];
					bundlingItemInfo.ProductName = (string)dataReader["ProductName"];
					if (dataReader["ProductPrice"] != DBNull.Value)
					{
						bundlingItemInfo.ProductPrice = (decimal)dataReader["ProductPrice"];
					}
					bundlingItemInfo.BundlingID = bundlingID;
					list.Add(bundlingItemInfo);
				}
			}
			return list;
		}


        public DataTable GetProductPromotionList(int productid, Member member)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            stringBuilder.Append("and ActivityId IN (select ActivityId from dbo.Ecshop_PromotionProducts where productid=@productid)");
            stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = @GradeId)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataTable GetAllProductPromotionList(int productid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            stringBuilder.Append("and ActivityId IN (select ActivityId from dbo.Ecshop_PromotionProducts where productid=@productid)");
            stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
    }
}
