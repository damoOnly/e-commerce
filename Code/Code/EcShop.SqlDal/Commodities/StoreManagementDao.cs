using Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop
{
    public class StoreManagementDao
    {
        private Database database;
        public StoreManagementDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetStore()
        {
            DataTable result = null;
            string sql = "SELECT *  FROM  Ecshop_StoreManagement";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetUserStore(int storeId)
        {
            DataTable result = null;
            string sql = "SELECT *  FROM  Ecshop_StoreManagement where 1= 1";
            if (storeId > 0)
            {
                sql += " and StoreId=" + storeId;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public StoreManagementInfo GetStore(int StoreId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_StoreManagement WHERE StoreId = @StoreId");
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            StoreManagementInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<StoreManagementInfo>(dataReader);
            }
            return result;
        }
        public bool RemoveReleatesProductByStore(int StoreId, int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_RelatedStoreProducts WHERE StoreId = @StoreId AND RelatedProductId = @RelatedProductId");
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, productId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public DataTable GetRelatedStoreProducts(int StoreId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock,t.DisplaySequence,t.QRcode from vw_Ecshop_BrowseProductList p inner join  Vshop_RelatedStoreProducts t on p.productid=t.RelatedProductId where t.StoreId=@StoreId");
            stringBuilder.AppendFormat(" and SaleStatus = {0}", 1);
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        public bool RemoveReleatesProductByStore(int StoreId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_RelatedStoreProducts WHERE StoreId = @StoreId");
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateRelateProductSequenceByStore(int StoreId, int RelatedProductId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_RelatedStoreProducts  set DisplaySequence=@DisplaySequence where StoreId=@StoreId and RelatedProductId=@RelatedProductId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
    
            this.database.AddInParameter(sqlStringCommand, "@StoreId", DbType.Int32, StoreId);
            this.database.AddInParameter(sqlStringCommand, "@RelatedProductId", DbType.Int32, RelatedProductId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool AddReleatesProdcutByStore(int StoreId, int prodcutId, string QRcode)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"if exists(select 1 from  Vshop_RelatedStoreProducts where StoreId=@StoreId and RelatedProductId=@RelatedProductId)
                                                                                    begin 
                                                                                       UPDATE [Vshop_RelatedStoreProducts]
                                                                                       SET  [QRcode] = @QRcode
                                                                                       WHERE [StoreId] =@StoreId and  [RelatedProductId] = @RelatedProductId
                                                                                    end 
                                                                                    else 
                                                                                    begin 
                                                                                      INSERT INTO Vshop_RelatedStoreProducts(StoreId, RelatedProductId,DisplaySequence,QRcode) VALUES (@StoreId, @RelatedProductId,0,@QRcode)
                                                                                    end");
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, prodcutId);
            this.database.AddInParameter(sqlStringCommand, "QRcode", DbType.String, QRcode);
            bool result;
            try
            {
                result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public string GetStoreName(int StoreId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT [StoreName]  FROM  [Ecshop_StoreManagement] WHERE StoreId = {0}", StoreId));
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            string result;
            if (obj != null)
            {
                result = obj.ToString();
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
        public int AddStore(StoreManagementInfo supplierInfo)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_StoreManagement([StoreName],[Phone],[Mobile],[Province],[City],[County],[Address],[Status],[Description],[CreateUser]) VALUES(@StoreName,@Phone,@Mobile,@Province,@City,@County,@Address,@Status,@Description,@CreateUser);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, supplierInfo.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Phone", DbType.String, supplierInfo.Phone);
            this.database.AddInParameter(sqlStringCommand, "Mobile", DbType.String, supplierInfo.Mobile);
            this.database.AddInParameter(sqlStringCommand, "Province", DbType.Int32, supplierInfo.Province);
            this.database.AddInParameter(sqlStringCommand, "City", DbType.Int32, supplierInfo.City);
            this.database.AddInParameter(sqlStringCommand, "County", DbType.Int32, supplierInfo.County);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, supplierInfo.Address);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, supplierInfo.Status);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, supplierInfo.Description);
            this.database.AddInParameter(sqlStringCommand, "CreateUser", DbType.Int32, supplierInfo.CreateUser);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                result = Convert.ToInt32(obj.ToString());
            }
            return result;
        }
        public bool UpdateStore(StoreManagementInfo supplierInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_StoreManagement SET StoreName=@StoreName,[Phone]=@Phone,[Mobile]=@Mobile,[County]=@County,[Address]=@Address,[Description]=@Description WHERE StoreId=@StoreId");
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, supplierInfo.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Phone", DbType.String, supplierInfo.Phone);
            this.database.AddInParameter(sqlStringCommand, "Mobile", DbType.String, supplierInfo.Mobile);
            this.database.AddInParameter(sqlStringCommand, "County", DbType.Int32, supplierInfo.County);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, supplierInfo.Address);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, supplierInfo.Description);
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, supplierInfo.StoreId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool DeleteStore(int StoreId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_StoreManagement WHERE StoreId=@StoreId;;DELETE FROM Vshop_RelatedStoreProducts where StoreId=@StoreId");
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public DbQueryResult GetStore(StoreQuery query)
        {
            string filter = string.IsNullOrEmpty(query.StoreName) ? string.Empty : string.Format("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            
            if(query.StoreId > 0)
            {
                filter += string.Format("StoreId ={0}", query.StoreId);
            }
           
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_StoreManagement", "StoreId", filter, "*");
        }
        /// <summary>
        /// 打印商品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        public DataTable PrintProducts(int storeId,string productId)
        {
            string sql = "";
            if (storeId > 0)
            {
                sql += "select a.QRcode,ProductName,ProductCode from Vshop_RelatedStoreProducts as a left join Ecshop_Products as b on a.RelatedProductId=b.ProductId  where  a.StoreId=" + storeId + " and   a.RelatedProductId in (" + productId + ")";
            }
            else
            {
                sql += "select QRcode,ProductName,ProductCode from Ecshop_Products  where  ProductId in (" + productId + ")";
            }
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

    }
}
