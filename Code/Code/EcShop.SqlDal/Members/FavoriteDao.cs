using Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Members
{
    public class FavoriteDao
    {
        private Database database;
        public FavoriteDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public bool AddProductToFavorite(int productId, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Favorite(ProductId, UserId, Tags, Remark)VALUES(@ProductId, @UserId, '', '')");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool AddProductToFavorite(int productId, int userId, string tags, string remark)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Favorite(ProductId, UserId, Tags, Remark)VALUES(@ProductId, @UserId, @Tags, @Remark);select @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "Tags", DbType.String, DataHelper.CleanSearchString(tags));
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, DataHelper.CleanSearchString(remark));

            int result;
            try
            {
                object obj = this.database.ExecuteScalar(sqlStringCommand);
                int num;
                if (int.TryParse(obj.ToString(), out num))
                {
                    result = num;

                    if (!tags.Equals(""))
                    {
                        try
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append("DELETE FROM Ecshop_FavoriteTags WHERE TagName=@TagName AND UserId=@UserId;");
                            stringBuilder.Append("INSERT INTO Ecshop_FavoriteTags(TagName,UserId,UpdateTime) VALUES(@TagName,@UserId,getdate());");
                            DbCommand command = this.database.GetSqlStringCommand(stringBuilder.ToString());
                            this.database.AddInParameter(command, "TagName", DbType.String, DataHelper.CleanSearchString(tags));
                            this.database.AddInParameter(command, "UserId", DbType.Int32, userId);

                            this.database.ExecuteNonQuery(command);
                        }
                        catch
                        { }
                    }
                }
                else
                {
                    result = -1;
                }
            }
            catch
            {
                result = -1;
            }

            return (result > 0);
        }
        public DataTable GetTypeTags()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT TagId,TagName,UserId,");
            stringBuilder.Append("(SELECT COUNT(*) FROM Ecshop_Favorite WHERE charindex(TagName,Tags)>0 AND UserId=@UserId and (select COUNT(*) from vw_Ecshop_CDisableBrowseProductList where ProductId=Ecshop_Favorite.ProductId and SaleStatus=1)>0) AS ProductNum");
            stringBuilder.Append(" FROM Ecshop_FavoriteTags WHERE UserId=@UserId ORDER BY UpdateTime DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        public int AddProduct(int productId, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Favorite(ProductId, UserId, Tags)VALUES(@ProductId, @UserId, @Tags);select @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "Tags", DbType.String, string.Empty);
            int result;
            try
            {
                object obj = this.database.ExecuteScalar(sqlStringCommand);
                int num;
                if (int.TryParse(obj.ToString(), out num))
                {
                    result = num;
                }
                else
                {
                    result = -1;
                }
            }
            catch
            {
                result = -1;
            }
            return result;
        }
        public int UpdateOrAddFavoriteTags(string tagname)
        {
            int num = -1;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DELETE FROM Ecshop_FavoriteTags WHERE TagName=@TagName AND UserId=@UserId;");
            stringBuilder.Append("INSERT INTO Ecshop_FavoriteTags(TagName,UserId,UpdateTime) VALUES(@TagName,@UserId,getdate());select @@IDENTITY");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, DataHelper.CleanSearchString(tagname));
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            int result;
            if (int.TryParse(obj.ToString(), out num))
            {
                result = num;
            }
            else
            {
                result = num;
            }
            return result;
        }
        public bool ExistsProduct(int productId, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_Favorite WHERE UserId=@UserId AND ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
        }
        public int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Favorite SET Tags = @Tags, Remark = @Remark WHERE FavoriteId = @FavoriteId");
            this.database.AddInParameter(sqlStringCommand, "Tags", DbType.String, tags);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public int DeleteFavorite(int favoriteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Favorite WHERE FavoriteId = @FavoriteId");
            this.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public DataTable GetFavorites()
        {
            Member member = HiContext.Current.User as Member;
            return GetFavorites(member);
        }

        public DataTable GetFavorites(Member member)
        {
            //Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT a.*, b.ProductName, b.ThumbnailUrl60, b.MarketPrice,b.ShortDescription,b.TaxRate,");
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            stringBuilder.AppendFormat(" FROM Ecshop_Favorite a left join vw_Ecshop_CDisableBrowseProductList b on a.ProductId = b.ProductId WHERE a.UserId={0} ORDER BY a.FavoriteId DESC", member.UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DbQueryResult GetFavorites( ProductFavoriteQuery query)
        {
            //Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            stringBuilder.Append("SELECT a.*, b.ProductName, b.ThumbnailUrl60,b.ThumbnailUrl220,b.MarketPrice,b.ShortDescription,b.TaxRate,");

            int discount = new MemberGradeDao().GetMemberGrade(query.GradeId).Discount;
            stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) = 1", query.GradeId);
            stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", query.GradeId, discount);

            stringBuilder.AppendFormat(" FROM Ecshop_Favorite a left join vw_Ecshop_CDisableBrowseProductList b on a.ProductId = b.ProductId WHERE a.UserId={0}", query.UserId);
            stringBuilder.Append(") as FavoriteTable ");

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, stringBuilder.ToString(), "FavoriteId", "", "*");
        }



        public DbQueryResult GetProductFavorites(Pagination page)
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" UserId={0}", member.UserId);

            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Ecshop_ProductFavs ", "FavoriteId", stringBuilder.ToString(), "*");
        }
        public DbQueryResult GetFavorites(int userId, string keyword, string tags, Pagination page)
        {
            DbQueryResult dbQueryResult = new DbQueryResult();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_Favorites_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, page.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, page.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, page.IsCount);
            Member member = HiContext.Current.User as Member;
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, member.GradeId);
            this.database.AddInParameter(storedProcCommand, "SqlPopulate", DbType.String, this.BuildFavoriteQuery(userId, keyword, tags));
            this.database.AddOutParameter(storedProcCommand, "TotalFavorites", DbType.Int32, 4);
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
                if (page.IsCount && dataReader.NextResult())
                {
                    dataReader.Read();
                    dbQueryResult.TotalRecords = dataReader.GetInt32(0);
                }
            }
            return dbQueryResult;
        }
        private string BuildFavoriteQuery(int userId, string keywords, string tags)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" SELECT FavoriteId FROM Ecshop_Favorite WHERE UserId = {0} ", userId);
            stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Ecshop_Products WHERE SaleStatus=1) ", new object[0]);
            if (!string.IsNullOrEmpty(keywords))
            {
                stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Ecshop_Products WHERE SaleStatus=1 AND ProductName LIKE '%{0}%') ", DataHelper.CleanSearchString(keywords));
            }
            if (!string.IsNullOrEmpty(tags))
            {
                stringBuilder.AppendFormat(" AND Tags LIKE '%{0}%'", DataHelper.CleanSearchString(tags));
            }
            stringBuilder.AppendFormat(" ORDER BY FavoriteId DESC", new object[0]);
            return stringBuilder.ToString();
        }
        public bool CheckHasCollect(int memberId, int productId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT COUNT(1)");
            stringBuilder.AppendFormat(" FROM Ecshop_Favorite WHERE UserId={0} AND ProductId ={1} ", memberId, productId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            int num = (int)this.database.ExecuteScalar(sqlStringCommand);
            return num > 0;
        }
        public int DeleteFavoriteTags(string tagname)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DELETE FROM Ecshop_FavoriteTags WHERE TagName=@TagName AND UserId=@UserId AND ");
            stringBuilder.AppendFormat("NOT EXISTS (SELECT FavoriteId FROM Ecshop_Favorite WHERE CHARINDEX('{0}',Tags)>0 AND UserId=@UserId)", DataHelper.CleanSearchString(tagname));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, DataHelper.CleanSearchString(tagname));
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public DataSet GetFavoriteTags()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT  TOP 5 TagId,TagName FROM Ecshop_FavoriteTags WHERE UserId=@UserId ORDER BY UpdateTime desc");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public bool DeleteFavorites(string ids)
        {
            string query = "DELETE from Ecshop_Favorite WHERE FavoriteId IN (" + ids + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool DeleteFavorites(int userId, string ids)
        {
            string query = "DELETE from Ecshop_Favorite WHERE UserId = " + userId.ToString() + " AND ProductId IN (" + ids + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public int GetUserFavoriteCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_Favorite WHERE UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public int GetFavoriteCountByProductId(int productid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_Favorite WHERE ProductId=@productid ");
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public int GetUserFavoriteCount(int UserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_Favorite WHERE UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }


        public int GetFavoriteId(int userId, int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT FavoriteId FROM Ecshop_Favorite WHERE UserId={0} AND ProductId ={1}", userId, productId));
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj == null)
            {
                return 0;
            }
            return (int)obj;
        }
    }
}
