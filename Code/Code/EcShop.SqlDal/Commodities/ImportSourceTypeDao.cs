using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace EcShop.SqlDal.Commodities
{
	public class ImportSourceTypeDao
	{
		private Database database;
        public ImportSourceTypeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetImportSourceTypes(ImportSourceTypeQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1 ");

            if (!string.IsNullOrEmpty(query.CnArea))
            {
                stringBuilder.AppendFormat(" AND CnArea LIKE '%{0}%'", DataHelper.CleanSearchString(query.CnArea));
            }

            if (!string.IsNullOrEmpty(query.EnArea))
            {
                stringBuilder.AppendFormat(" AND EnArea LIKE '%{0}%'", DataHelper.CleanSearchString(query.EnArea));
            }
            if (!string.IsNullOrEmpty(query.Remark))
            {
                stringBuilder.AppendFormat(" AND Remark LIKE '%{0}%'", DataHelper.CleanSearchString(query.Remark));
            }

            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddTime >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddTime <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            string selectFields = "ImportSourceId,Icon,EnArea,CnArea,Remark,DisplaySequence,AddTime,HSCode,FavourableFlag,StandardCName";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_ImportSourceType", "ImportSourceId", stringBuilder.ToString(), selectFields);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="importSourceIds"></param>
        /// <returns></returns>
        public int DeleteImportSourceTypes(string importSourceIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Ecshop_ImportSourceType WHERE ImportSourceId IN ({0})", importSourceIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="importSourceType"></param>
        /// <returns></returns>
        public int AddImportSourceType(ImportSourceTypeInfo importSourceType)
        {
            int result=0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"INSERT [dbo].[Ecshop_ImportSourceType] ([Icon], [EnArea], [CnArea], [Remark], [DisplaySequence],[HSCode],[FavourableFlag],[StandardCName]) VALUES (@Icon,@EnArea,@CnArea,@Remark,@DisplaySequence,@HSCode,@FavourableFlag,@StandardCName);select @@IDENTITY;");
            this.database.AddInParameter(sqlStringCommand, "Icon", DbType.String, importSourceType.Icon);
            this.database.AddInParameter(sqlStringCommand, "EnArea", DbType.String, importSourceType.EnArea);
            this.database.AddInParameter(sqlStringCommand, "CnArea", DbType.String, importSourceType.CnArea);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, importSourceType.Remark);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, importSourceType.DisplaySequence);

            this.database.AddInParameter(sqlStringCommand, "HSCode", DbType.String, importSourceType.HSCode);
            this.database.AddInParameter(sqlStringCommand, "FavourableFlag", DbType.Boolean, importSourceType.FavourableFlag);
            this.database.AddInParameter(sqlStringCommand, "StandardCName", DbType.String, importSourceType.StandardCName);
            int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(),out result);
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="importSourceType"></param>
        /// <returns></returns>
        public int UpdateImportSourceType(ImportSourceTypeInfo importSourceType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update Ecshop_ImportSourceType set Icon=@Icon,EnArea=@EnArea,CnArea=@CnArea,Remark=@Remark,DisplaySequence=@DisplaySequence,HSCode=@HSCode,FavourableFlag=@FavourableFlag,StandardCName=@StandardCName where ImportSourceId = @ImportSourceId");
            this.database.AddInParameter(sqlStringCommand, "Icon", DbType.String, importSourceType.Icon);
            this.database.AddInParameter(sqlStringCommand, "EnArea", DbType.String, importSourceType.EnArea);
            this.database.AddInParameter(sqlStringCommand, "CnArea", DbType.String, importSourceType.CnArea);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, importSourceType.Remark);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, importSourceType.DisplaySequence);
            this.database.AddInParameter(sqlStringCommand, "ImportSourceId", DbType.Int32, importSourceType.ImportSourceId);
            this.database.AddInParameter(sqlStringCommand, "HSCode", DbType.String, importSourceType.HSCode);
            this.database.AddInParameter(sqlStringCommand, "FavourableFlag", DbType.Boolean, importSourceType.FavourableFlag);
            this.database.AddInParameter(sqlStringCommand, "StandardCName", DbType.String, importSourceType.StandardCName);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 获取原产地信息
        /// </summary>
        /// <param name="importSourceId"></param>
        /// <returns></returns>
        public ImportSourceTypeInfo GetImportSourceTypeInfo(int importSourceId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select ImportSourceId,Icon,EnArea,CnArea,Remark,DisplaySequence,AddTime,HSCode,FavourableFlag,StandardCName from Ecshop_ImportSourceType where ImportSourceId = @ImportSourceId");
            this.database.AddInParameter(sqlStringCommand, "ImportSourceId", DbType.Int32, importSourceId);

            ImportSourceTypeInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateImportSourceTypeInfo(dataReader);
                }
            }
            return result;
        }


        public DataTable GetAllImportSourceTypes()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ImportSourceType ORDER BY DisplaySequence");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetAllImportSourceTypes(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select distinct ei.*
from Ecshop_ImportSourceType ei
inner join Ecshop_Products p on p.ImportSourceId = ei.ImportSourceId 
inner join Ecshop_Categories c on c.CategoryId = p.CategoryId 
where c.CategoryId =@categoryId and p.SaleStatus = 1  and p.IsApproved = 1   order by ei.DisplaySequence desc   ");
            this.database.AddInParameter(sqlStringCommand, "categoryId", DbType.Int32, categoryId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetAllImportSourceBySupplierId(int supplierid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select distinct  ei.* 
from Ecshop_Supplier s  
inner join Ecshop_Products p on s.SupplierId = p.SupplierId 
inner join Ecshop_ImportSourceType ei on ei.ImportSourceId = p.ImportSourceId 
where s.SupplierId = @SupplierId and p.SaleStatus = 1  and p.IsApproved = 1  order by ei.DisplaySequence desc  ");
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, supplierid);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
	}
}
