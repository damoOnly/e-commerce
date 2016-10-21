using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Commodities
{
    public class HomeProductDao
    {
        private Database database;
        public HomeProductDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DbQueryResult GetProducts(ProductQuery query)
        {
            query.IsIncludeHomeProduct = new bool?(false);
            return new ProductDao().GetProducts(query, false);
        }
        public bool AddHomeProdcut(int productId, ClientType client)
        {
            bool flag = true;
            string commandText = string.Format("SELECT COUNT(1) FROM Vshop_HomeProducts WHERE ProductId={0} AND Client = {1}", productId, (int)client);
            int num = (int)this.database.ExecuteScalar(CommandType.Text, commandText);
            bool result;
            if (num == 0)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_HomeProducts(ProductId, Client,DisplaySequence) VALUES (@ProductId, @Client,0)");
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, client);
                try
                {
                    result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
                    return result;
                }
                catch
                {
                    flag = false;
                }
            }
            result = flag;
            return result;
        }

        public bool AddHomeProdcut(int productId, ClientType client,int supplierId)
        {
            bool flag = true;
            string commandText = string.Format("SELECT COUNT(1) FROM Vshop_HomeProducts WHERE ProductId={0} AND Client = {1} and supplierId={2}", productId, (int)client, supplierId);
            int num = (int)this.database.ExecuteScalar(CommandType.Text, commandText);
            bool result;
            if (num == 0)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_HomeProducts(ProductId, Client,DisplaySequence,supplierId) VALUES (@ProductId, @Client,0,@supplierId)");
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, client);
                this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
                
                try
                {
                    result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
                    return result;
                }
                catch
                {
                    flag = false;
                }
            }
            result = flag;
            return result;
        }

        public bool RemoveHomeProduct(int productId, ClientType client)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts WHERE ProductId = @ProductId AND Client = @Client");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, client);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool RemoveAllHomeProduct(ClientType client)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Vshop_HomeProducts WHERE Client = {0}", (int)client));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public DataTable GetHomeProducts(ClientType client)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,ThumbnailUrl220,ThumbnailUrl180,ThumbnailUrl310,ThumbnailUrl410,p.TaxRate,MarketPrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence,fastbuy_skuid,TaxRate,");
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            stringBuilder.Append(" from vw_Ecshop_BrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
            stringBuilder.AppendFormat(" and SaleStatus = {0}  and IsApproved=1 WHERE Client = {1}", 1, (int)client);
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetHomeProducts(ClientType client, bool issupplier, int supplierId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,ThumbnailUrl220,ThumbnailUrl180,ThumbnailUrl310,ThumbnailUrl410,p.TaxRate,MarketPrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence,t.supplierId as supplierId,fastbuy_skuid,TaxRate,");
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            stringBuilder.Append(" from vw_Ecshop_BrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
            stringBuilder.AppendFormat(" and SaleStatus = {0}  and IsApproved=1 WHERE Client = {1} and t.supplierId={2}", 1, (int)client, supplierId);
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        public DataTable GetProductSelect(ClientType client)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,ThumbnailUrl220,ThumbnailUrl180,ThumbnailUrl310,ThumbnailUrl410,p.TaxRate,MarketPrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence,fastbuy_skuid,TaxRate,");
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            stringBuilder.Append(" from vw_Ecshop_CDisableBrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
            stringBuilder.AppendFormat(" and SaleStatus = {0}  and IsApproved=1 WHERE Client = {1}", 1, (int)client);
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        public DataTable GetHomeProducts(ClientType client, bool isAnonymous)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,ThumbnailUrl220,ThumbnailUrl180,ThumbnailUrl310,ThumbnailUrl410,p.TaxRate,MarketPrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence,fastbuy_skuid,");
            Member member = HiContext.Current.User as Member;

            if (isAnonymous)
            {
                stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		         and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=p.productid)
	             AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
            }
            else
            {
                if (member != null)
                {
                    stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		            and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=p.productid)
		            AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                }
            }

            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            stringBuilder.Append(" from vw_Ecshop_CDisableBrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
            stringBuilder.AppendFormat(" and SaleStatus = {0} and IsApproved=1 WHERE Client = {1}", 1, (int)client);//¼ÓÈëÉóºË×´Ì¬
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public bool UpdateHomeProductSequence(ClientType client, int ProductId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_HomeProducts  set DisplaySequence=@DisplaySequence where ProductId=@ProductId AND Client = @Client");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, ProductId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, client);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}
