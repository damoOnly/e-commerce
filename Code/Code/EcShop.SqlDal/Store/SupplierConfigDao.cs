using EcShop.Entities;
using EcShop.Entities.Supplier;
using Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.Store
{
    public class SupplierConfigDao
    {

         private Database database;
         public SupplierConfigDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 添加商家配置
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
         public int SaveSupplierCfg(SupplierConfigInfo info)
         {
             int result;
             int displaySequence = this.GetMaxSupplierCfgSequence();
             StringBuilder stringBuilder = new StringBuilder("insert into  Ecshop_SupplierConfig (ShortDesc,ImageUrl,DisplaySequence,Client,IsDisable,SupplierId,Type)");
             stringBuilder.Append("values (@ShortDesc,@ImageUrl,@DisplaySequence,@Client,@IsDisable,@SupplierId,@Type);select @@IDENTITY");
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
             this.database.AddInParameter(sqlStringCommand, "ShortDesc", DbType.String, info.ShortDesc);
             this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
             this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.String, displaySequence);
             this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32,info.Client);
             this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, info.IsDisable);
             this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, info.SupplierId);
             this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, info.Type);

             if (!int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(),out result))
             {
                 result = 0;
             }

             return result;

            
         }

        /// <summary>
        /// 获取商家配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         public SupplierConfigInfo GetSupplierCfgById(int id)
         {
             StringBuilder stringBuilder = new StringBuilder(" select * from Ecshop_SupplierConfig where Id=@Id");
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
             this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
             SupplierConfigInfo result = null;
             using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
             {
                 result = ReaderConvert.ReaderToModel<SupplierConfigInfo>(dataReader);
             }
             return result;
         }

        /// <summary>
        /// 修改商家配置
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
         public bool UpdateSupplierCfg(SupplierConfigInfo info)
         {
             StringBuilder stringBuilder = new StringBuilder();
             stringBuilder.Append("update Ecshop_SupplierConfig set ");
             stringBuilder.Append("ShortDesc=@ShortDesc,");
             stringBuilder.Append("ImageUrl=@ImageUrl,");
             stringBuilder.Append("DisplaySequence=@DisplaySequence,");
             stringBuilder.Append("Type=@Type,");
             stringBuilder.Append("Client=@Client,");
             stringBuilder.Append("IsDisable=@IsDisable,");
             stringBuilder.Append("SupplierId=@SupplierId");

             stringBuilder.Append(" where Id=@Id ");
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
             this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, info.Id);
             this.database.AddInParameter(sqlStringCommand, "ShortDesc", DbType.String, info.ShortDesc);
             this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
             this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.String, info.DisplaySequence);
             this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, info.Type);
             this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, info.Client);
             this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, info.IsDisable);
             this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, info.SupplierId);
             return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
         }


        
         private int GetMaxSupplierCfgSequence()
         {
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select max(DisplaySequence) from Ecshop_SupplierConfig");
             int result;

             if (int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(),out result))
             {
                 result = result + 1;
             }
             else
             {
                 result = 1;
             }
             return result;
         }

         public IList<SupplierConfigInfo> GetSupplierCfgByType(ClientType client, SupplierCfgType cfgType)
         {
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select  * from  Ecshop_SupplierConfig where type=@type AND Client=@Client ORDER BY DisplaySequence ASC");
             this.database.AddInParameter(sqlStringCommand, "type", DbType.Int32, (int)cfgType);
             this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
             IList<SupplierConfigInfo> result;
             using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
             {
                 result = ReaderConvert.ReaderToList<SupplierConfigInfo>(dataReader);
             }
             return result;
         }


         public bool DelSupplierCfg(int id)
         {
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Ecshop_SupplierConfig where Id=@Id");
             this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
             return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
         }

         public DataTable GetConfigSupplier(ClientType client, SupplierCfgType cfgType, int userid)
         {
             StringBuilder stringBuilder = new StringBuilder();
             stringBuilder.Append("select A.ImageUrl,B.*,");
             stringBuilder.Append("(select count(C1.Id) from  Ecshop_SupplierCollect C1 where C1.SupplierId=A.SupplierId) as CollectCount,");
             stringBuilder.AppendFormat("case when (select count(C2.SupplierId) from Ecshop_SupplierCollect C2 where C2.SupplierId=A.SupplierId and C2.Userid={0})>0 then 1 else 0 end as  IsCollect", userid);
             stringBuilder.Append(" from Ecshop_SupplierConfig A ");
             stringBuilder.Append(" inner join  Ecshop_Supplier B on A.SupplierId=B.SupplierId ");
             stringBuilder.AppendFormat(" where A.Client={0} ", (int)client);
             if ((int)cfgType > 0)
             {
                stringBuilder.AppendFormat(" and A.Type={0} ", (int)cfgType);
             }
             stringBuilder.Append(" order by A.DisplaySequence ");
            
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
             return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
         }

    }
}
