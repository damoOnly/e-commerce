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

namespace Commodities
{
    public class SitesManagementDao
    {
        private Database database;
        public SitesManagementDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetSites()
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SitesId,SitesName,City,IsDefault,Province,Sort FROM Ecshop_Sites ORDER BY Sort  DESC ");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public SitesManagementInfo GetSites(int SitesId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Sites WHERE SitesId = @SitesId");
            this.database.AddInParameter(sqlStringCommand, "SitesId", DbType.Int32, SitesId);
            SitesManagementInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<SitesManagementInfo>(dataReader);
            }
            return result;
        }
        public int AddSites(SitesManagementInfo SitesInfo)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"INSERT INTO [Ecshop_Sites]
                                                                            ([SitesName]
                                                                            ,[Code]
                                                                            ,[Province]
                                                                            ,[City]
                                                                            ,[IsDefault]
                                                                            ,[Sort]
                                                                            ,[Description])
                                                                        VALUES
                                                                            (@SitesName
                                                                            ,@Code
                                                                            ,@Province
                                                                            ,@City
                                                                            ,@IsDefault
                                                                            ,@Sort
                                                                            ,@Description);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "SitesName", DbType.String, SitesInfo.SitesName);
            this.database.AddInParameter(sqlStringCommand, "Code", DbType.String, SitesInfo.Code);
            this.database.AddInParameter(sqlStringCommand, "Province", DbType.String, SitesInfo.Province);
            this.database.AddInParameter(sqlStringCommand, "City", DbType.Int32, SitesInfo.City);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Int32, SitesInfo.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Sort", DbType.Int32, SitesInfo.Sort);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, SitesInfo.Description);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                result = Convert.ToInt32(obj.ToString());
            }
            return result;
        }
        public bool UpdateSites(SitesManagementInfo SitesInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"UPDATE [Ecshop_Sites]
                                                                               SET [SitesName] = @SitesName
                                                                                  ,[Code] = @Code
                                                                                  ,[Province] = @Province
                                                                                  ,[City] = @City
                                                                                  ,[IsDefault] =@IsDefault
                                                                                  ,[Sort] = @Sort
                                                                                  ,[Description] = @Description
                                                                             WHERE SitesId=@SitesId");
            this.database.AddInParameter(sqlStringCommand, "SitesName", DbType.String, SitesInfo.SitesName);
            this.database.AddInParameter(sqlStringCommand, "Code", DbType.String, SitesInfo.Code);
            this.database.AddInParameter(sqlStringCommand, "Province", DbType.Int32, SitesInfo.Province);
            this.database.AddInParameter(sqlStringCommand, "City", DbType.Int32, SitesInfo.City);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.String, SitesInfo.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Sort", DbType.String, SitesInfo.Sort);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, SitesInfo.Description);
            this.database.AddInParameter(sqlStringCommand, "SitesId", DbType.Int32, SitesInfo.SitesId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool DeleteSites(int SitesId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE  FROM Ecshop_Sites WHERE SitesId=@SitesId;");
            this.database.AddInParameter(sqlStringCommand, "SitesId", DbType.Int32, SitesId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public DbQueryResult GetSite(SiteQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_Sites", "Sort", string.IsNullOrEmpty(query.SiteName) ? string.Empty : string.Format("SitesName LIKE '%{0}%'", DataHelper.CleanSearchString(query.SiteName)), "*");
        }

    }
}
