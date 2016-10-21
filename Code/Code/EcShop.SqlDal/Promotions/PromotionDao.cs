using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Promotions
{
    public class PromotionDao
    {
        private Database database;
        public PromotionDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetPromotions(bool isProductPromote, bool isWholesale,int? supplierId = null )
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Promotions");
            if (isProductPromote)
            {
                if (isWholesale)
                {
                    DbCommand expr_28 = sqlStringCommand;
                    if (supplierId.HasValue)
                    {
                        expr_28.CommandText += string.Format(" WHERE PromoteType = {0} and SupplierId = {1}", 4,supplierId);
                    }
                    else
                    {
                        expr_28.CommandText += string.Format(" WHERE PromoteType = {0}", 4);
                    }
                    
                }
                else
                {
                    DbCommand expr_4C = sqlStringCommand;
                    if (supplierId.HasValue)
                    {
                        expr_4C.CommandText += string.Format(" WHERE PromoteType <> {0} AND PromoteType < 10 and SupplierId = {1}", 4, supplierId);
                    }
                    else
                    {
                        expr_4C.CommandText += string.Format(" WHERE PromoteType <> {0} AND PromoteType < 10", 4);
                    }
                }
            }
            else
            {
                if (isWholesale)
                {
                    DbCommand expr_7C = sqlStringCommand;
                    if (supplierId.HasValue)
                    {
                        expr_7C.CommandText += string.Format(" WHERE PromoteType = {0} OR PromoteType = {1} and SupplierId = {2} ", 13, 14, supplierId);
                    }
                    else
                    {
                        expr_7C.CommandText += string.Format(" WHERE PromoteType = {0} OR PromoteType = {1}", 13, 14);
                    }
                    
                }
                else
                {
                    DbCommand expr_A8 = sqlStringCommand;
                    if (supplierId.HasValue)
                    {
                        expr_A8.CommandText += string.Format(" WHERE PromoteType <> {0} AND PromoteType <> {1} AND PromoteType > 10 and SupplierId = {2}", 13, 14, supplierId);
                    }
                    else
                    {
                        expr_A8.CommandText += string.Format(" WHERE PromoteType <> {0} AND PromoteType <> {1} AND PromoteType > 10", 13, 14);
                    }
                    
                }
            }
            DbCommand expr_D3 = sqlStringCommand;
            expr_D3.CommandText += " ORDER BY ActivityId DESC";
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataTable GetPromotions(ProductPromotionsQuery query, out int count)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Promotions");
            string sqlcount = "";
            if (query.IsPromotion)
            {
                string sql = "SELECT ROW_NUMBER() over(order by ActivityId) as row_num,* FROM Ecshop_Promotions";

                if (query.IsWholesale)
                {
                    DbCommand expr_28 = sqlStringCommand;
                    sql += string.Format(" WHERE PromoteType = {0}", 4);
                    if (!string.IsNullOrWhiteSpace(query.BeginTime))
                    {
                        sql += string.Format(" AND StartDate >= convert(datetime,'{0}')", query.BeginTime);
                    }
                    if (!string.IsNullOrWhiteSpace(query.EndTime))
                    {
                        sql += string.Format(" WHERE StartDate <= convert(datetime,'{0}')", query.EndTime);
                    }
                    if (query.SupplierId.HasValue)
                    {
                        sql += string.Format(" AND SupplierId ={0} ", query.SupplierId);
                    }
                    sql = string.Format("SELECT * FROM ({0}) as a where 1=1", sql);
                    sqlcount = sql;
                    sql += string.Format(" and row_num between {0} and {1}", (query.Page.PageIndex - 1) * query.Page.PageSize + 1, (query.Page.PageIndex) * query.Page.PageSize);
                    expr_28.CommandText = sql;
                }
                else
                {
                    DbCommand expr_4C = sqlStringCommand;

                    sql += string.Format("  WHERE PromoteType <> {0} AND PromoteType <= 10", 4);
                    if (!string.IsNullOrWhiteSpace(query.BeginTime))
                    {
                        sql += string.Format(" AND StartDate >= convert(datetime,'{0}')", query.BeginTime);
                    }
                    if (!string.IsNullOrWhiteSpace(query.EndTime))
                    {
                        sql += string.Format(" AND StartDate <= convert(datetime,'{0}')", query.EndTime);
                    }
                    if (query.SupplierId.HasValue)
                    {
                        sql += string.Format(" AND SupplierId ={0} ", query.SupplierId);
                    }
                    sql = string.Format("SELECT * FROM ({0}) as a where 1=1", sql);
                    sqlcount = sql;
                    sql += string.Format(" and row_num between {0} and {1}", (query.Page.PageIndex - 1) * query.Page.PageSize + 1, (query.Page.PageIndex) * query.Page.PageSize);

                    expr_4C.CommandText = sql;
                }
            }
            else
            {
                if (query.IsWholesale)
                {
                    DbCommand expr_7C = sqlStringCommand;
                    expr_7C.CommandText += string.Format(" WHERE PromoteType = {0} OR PromoteType = {1}", 13, 14);
                }
                else
                {
                    DbCommand expr_A8 = sqlStringCommand;
                    expr_A8.CommandText += string.Format(" WHERE PromoteType <> {0} AND PromoteType <> {1} AND PromoteType > 10", 13, 14);
                }
            }
            DbCommand expr_D3 = sqlStringCommand;
            expr_D3.CommandText += " ORDER BY ActivityId DESC";
            if (sqlcount != "")
            {
                expr_D3.CommandText += ";SELECT COUNT(*) FROM (" + sqlcount + ") as c";
            }
            DataTable result;
            count = 0;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
                dataReader.NextResult();
                while(dataReader.Read())
                    count = dataReader.GetInt32(0);
            }
            return result;
        }
        public int? GetActiveIdByProduct(int productId)
        {
            int? result = null;
            string commandText = string.Format("SELECT ActivityId FROM Ecshop_PromotionProducts WHERE ProductId = {0}", productId);
            object obj = this.database.ExecuteScalar(CommandType.Text, commandText);
            if (obj != null)
            {
                result = new int?((int)obj);
            }
            return result;
        }
        public PromotionInfo GetPromotion(int activityId)
        {
            PromotionInfo promotionInfo = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Promotions WHERE ActivityId = @ActivityId; SELECT GradeId FROM Ecshop_PromotionMemberGrades WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    promotionInfo = DataMapper.PopulatePromote(dataReader);
                }
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    promotionInfo.MemberGradeIds.Add((int)dataReader["GradeId"]);
                }
            }
            return promotionInfo;
        }
        public IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId IN (SELECT GradeId FROM Ecshop_PromotionMemberGrades WHERE ActivityId = @ActivityId)");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(DataMapper.PopulateMemberGrade(dataReader));
                }
            }
            return list;
        }
        public DataTable GetPromotionProducts(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.* FROM vw_Ecshop_BrowseProductList AS a left join Ecshop_Supplier AS b on a.SupplierId=b.SupplierId  WHERE a.ProductId IN (SELECT ProductId FROM Ecshop_PromotionProducts WHERE ActivityId = @ActivityId) ORDER BY a.DisplaySequence");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取促销活动中赠送商品
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <returns></returns>
        public DataTable GetPromotionPresentPro(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.* FROM vw_Ecshop_BrowseProductList AS a left join Ecshop_Supplier AS b on a.SupplierId=b.SupplierId  WHERE a.ProductId IN (SELECT ProductId FROM Ecshop_PromotionProductsPresent WHERE ActivityId = @ActivityId) ORDER BY a.DisplaySequence");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public int GetActivityProductAmount(int activityId)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(1) FROM Ecshop_PromotionProducts WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            try
            {
                result = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            }
            catch
            {
                result = 0;
            }

            return result;
        }
        public bool AddPromotionProducts(int activityId, string productIds)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("INSERT INTO Ecshop_PromotionProducts SELECT @ActivityId, ProductId FROM Ecshop_Products WHERE ProductId IN ({0})", productIds) + " AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProducts)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("INSERT INTO Ecshop_PromotionProducts SELECT @ActivityId, ProductId FROM Ecshop_Products WHERE ProductId IN ({0})", productIds) + string.Format(@" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProducts A left join Ecshop_Promotions B on A.ActivityId=B.ActivityId where 


(not (convert(varchar(10),B.enddate,120)<(select convert(varchar(10),startdate,120) from Ecshop_Promotions where activityId={0}) 


or convert(varchar(10),B.startdate,120)>(select convert(varchar(10),enddate,120) from Ecshop_Promotions where activityId={0})))

and B.enddate>=convert(varchar(10),getdate(),120))", activityId));
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 添加赠送的商品
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public bool AddPromotionPresentPro(int activityId, string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("INSERT INTO Ecshop_PromotionProductsPresent SELECT @ActivityId, ProductId FROM Ecshop_Products WHERE ProductId IN ({0})", productIds) + string.Format(@" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProductsPresent A left join Ecshop_Promotions B on A.ActivityId=B.ActivityId where  
 (not (convert(varchar(10),B.enddate,120)<(select convert(varchar(10),startdate,120) from Ecshop_Promotions where activityId={0}) 
 or convert(varchar(10),B.startdate,120)>(select convert(varchar(10),enddate,120) from Ecshop_Promotions where activityId={0}))) 
 and B.enddate>=convert(varchar(10),getdate(),120))", activityId));
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool DeletePromotionProducts(int activityId, int? productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_PromotionProducts WHERE ActivityId = @ActivityId");
            if (productId.HasValue)
            {
                DbCommand expr_21 = sqlStringCommand;
                expr_21.CommandText += string.Format(" AND ProductId = {0}", productId.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 删除促销活动中赠送商品
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool DeletePromotionPresentPro(int activityId, int? productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_PromotionProductsPresent WHERE ActivityId = @ActivityId");
            if (productId.HasValue)
            {
                DbCommand expr_21 = sqlStringCommand;
                expr_21.CommandText += string.Format(" AND ProductId = {0}", productId.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public int AddPromotion(PromotionInfo promotion, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Promotions(Name, PromoteType, Condition, DiscountValue, StartDate, EndDate, Description,SupplierId,IsAscend) VALUES(@Name, @PromoteType, @Condition, @DiscountValue, @StartDate, @EndDate, @Description,@SupplierId,@IsAscend); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, promotion.Name);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int)promotion.PromoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, promotion.Condition);
            this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, promotion.DiscountValue);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, promotion.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, promotion.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, promotion.Description);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, promotion.SupplierId);
            this.database.AddInParameter(sqlStringCommand, "IsAscend", DbType.Int32, promotion.IsAscend);
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
        public bool AddPromotionMemberGrades(int activityId, IList<int> memberGrades, DbTransaction dbTran)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("DELETE FROM Ecshop_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
            foreach (int current in memberGrades)
            {
                stringBuilder.AppendFormat(" INSERT INTO Ecshop_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, current);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
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
        public bool EditPromotion(PromotionInfo promotion, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Promotions SET Name = @Name, PromoteType = @PromoteType, Condition = @Condition, DiscountValue = @DiscountValue, StartDate = @StartDate, EndDate = @EndDate, Description = @Description,IsAscend=@IsAscend WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, promotion.Name);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int)promotion.PromoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, promotion.Condition);
            this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, promotion.DiscountValue);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, promotion.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, promotion.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, promotion.Description);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, promotion.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "IsAscend", DbType.Int32, promotion.IsAscend);
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
        public bool DeletePromotion(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Promotions WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public DataTable GetPromotes(Pagination pagination, int promotionType, out int totalPromotes)
        {
            DataTable result = null;
            string text = string.Format("SELECT COUNT(*) FROM Ecshop_Promotions WHERE 1=1 ", new object[0]);
            if (promotionType != 0)
            {
                text += string.Format(" AND PromoteType={0} ", promotionType);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
            totalPromotes = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string text2 = string.Empty;
            StringBuilder stringBuilder = new StringBuilder("case Ecshop_Promotions.PromoteType");
            stringBuilder.AppendFormat(" when 1 then '商品直接打折'", new object[0]);
            stringBuilder.AppendFormat(" when 2 then '商品固定金额出售'", new object[0]);
            stringBuilder.AppendFormat(" when 3 then '商品减价优惠'", new object[0]);
            stringBuilder.AppendFormat(" when 4 then '批发打折'", new object[0]);
            stringBuilder.AppendFormat(" when 5 then '买商品赠送礼品'", new object[0]);
            stringBuilder.AppendFormat(" when 6 then '商品有买有送'", new object[0]);
            stringBuilder.AppendFormat(" when 11 then '订单满额打折'", new object[0]);
            stringBuilder.AppendFormat(" when 12 then '订单满额优惠金额'", new object[0]);
            stringBuilder.AppendFormat(" when 13 then '混合批发打折'", new object[0]);
            stringBuilder.AppendFormat(" when 14 then '混合批发优惠金额'", new object[0]);
            stringBuilder.AppendFormat(" when 15 then '订单满额送礼品'", new object[0]);
            stringBuilder.AppendFormat(" when 16 then '订单满额送倍数积分'", new object[0]);
            stringBuilder.AppendFormat(" when 17 then '订单满额免运费'", new object[0]);
            stringBuilder.Append(" end as PromoteTypeName");
            if (pagination.PageIndex == 1)
            {
                text2 = "SELECT TOP 10 *," + stringBuilder + " FROM Ecshop_Promotions WHERE 1=1 ";
            }
            else
            {
                text2 = string.Format("SELECT TOP {0} *," + stringBuilder + " FROM Ecshop_Promotions WHERE ActivityId NOT IN (SELECT TOP {1} ActivityId FROM Ecshop_Promotions) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
            }
            if (promotionType != 0)
            {
                text2 += string.Format(" AND PromoteType={0} ", promotionType);
            }
            text2 += " ORDER BY ActivityId DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(text2);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type)
        {
            string query = string.Empty;
            if (type == "Next")
            {
                query = "SELECT TOP 1 * FROM Ecshop_Promotions WHERE activityId<@activityId AND PromoteType=@PromoteType  ORDER BY activityId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM Ecshop_Promotions WHERE activityId>@activityId AND PromoteType=@PromoteType ORDER BY activityId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "activityId", DbType.Int32, promote.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, Convert.ToInt32(promote.PromoteType));
            PromotionInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulatePromote(dataReader);
                }
                dataReader.Close();
            }
            return result;
        }
        public PromotionInfo GetReducedPromotion(Member member, decimal amount, int quantity, out decimal reducedAmount)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType BETWEEN 11 AND 14 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = @GradeId)");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            IList<PromotionInfo> list = new List<PromotionInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(DataMapper.PopulatePromote(dataReader));
                }
            }
            PromotionInfo result = null;
            reducedAmount = 0m;
            foreach (PromotionInfo current in list)
            {
                switch (current.PromoteType)
                {
                    case PromoteType.FullAmountDiscount:
                        if (amount >= current.Condition && amount - amount * current.DiscountValue > reducedAmount)
                        {
                            reducedAmount = amount - amount * current.DiscountValue;
                            result = current;
                        }
                        break;
                    case PromoteType.FullAmountReduced:
                        if (amount >= current.Condition && current.DiscountValue > reducedAmount)
                        {
                            reducedAmount = current.DiscountValue;
                            result = current;
                        }
                        break;
                    case PromoteType.FullQuantityDiscount:
                        if (quantity >= (int)current.Condition && amount - amount * current.DiscountValue > reducedAmount)
                        {
                            reducedAmount = amount - amount * current.DiscountValue;
                            result = current;
                        }
                        break;
                    case PromoteType.FullQuantityReduced:
                        if (quantity >= (int)current.Condition && current.DiscountValue > reducedAmount)
                        {
                            reducedAmount = current.DiscountValue;
                            result = current;
                        }
                        break;
                }
            }
            return result;
        }
        public PromotionInfo GetSendPromotion(Member member, decimal amount, PromoteType promoteType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType = @PromoteType AND Condition <= @Condition AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = @GradeId) ORDER BY DiscountValue DESC");
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int)promoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, amount);
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

        public PromotionInfo GetSecondPromotion(Member member, PromoteType promoteType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType = @PromoteType AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = @GradeId) ORDER BY DiscountValue DESC");
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int)promoteType);
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


        /// <summary>
        /// 获取订单促销信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public DataTable GetOrderPromotionInfo(Member member)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            stringBuilder.Append("and PromoteType <> 13 AND PromoteType <> 14 AND PromoteType > 10 ");
            stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = @GradeId)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取订单促销信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllOrderPromotionInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select  * from Ecshop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            stringBuilder.Append("and PromoteType <> 13 AND PromoteType <> 14 AND PromoteType > 10 ");
            stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
    }
}
