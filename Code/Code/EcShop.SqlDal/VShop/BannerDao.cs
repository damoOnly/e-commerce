using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.VShop
{
    public class BannerDao
    {
        private Database database;
        public BannerDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public IList<BannerInfo> GetAllBanners(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=1 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));
            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where  type=1 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
            IList<BannerInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<BannerInfo>(dataReader);
            }
            return result;
        }

        public IList<BannerInfo> GetAllBanners(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=1 AND Client= {0} and SupplierId={1} ORDER BY DisplaySequence ASC", (int)client, supplierId));
            IList<BannerInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<BannerInfo>(dataReader);
            }
            return result;
        }

        public IList<TplCfgInfo> GetTplCfgInfoList(ClientType client, int type, int pageSize, int pageIndex, ref int total)//分页
        {
            IList<TplCfgInfo> result = null;
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT COUNT(1) as total from Ecshop_Banner where [type]=@type AND Client=@client;SELECT * from (SELECT *,ROW_NUMBER() OVER(ORDER BY DisplaySequence ASC) as no
//from Ecshop_Banner  where [type]=@type AND Client=@client) as AA WHERE AA.no >@pageIndex*@pageSize AND AA.no<=(@pageIndex+1)*@pageSize;");
            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT COUNT(1) as total from Ecshop_Banner where [type]=@type AND Client=@client and (SupplierId=0 or SupplierId is null);SELECT * from (SELECT *,ROW_NUMBER() OVER(ORDER BY DisplaySequence ASC) as no
from Ecshop_Banner  where [type]=@type AND Client=@client and (SupplierId=0 or SupplierId is null)) as AA WHERE AA.no >@pageIndex*@pageSize AND AA.no<=(@pageIndex+1)*@pageSize;");
            this.database.AddInParameter(sqlStringCommand, "type", DbType.Int32, type);
            this.database.AddInParameter(sqlStringCommand, "client", DbType.Int32, (int)client);
            this.database.AddInParameter(sqlStringCommand, "pageIndex", DbType.Int32, pageIndex);
            this.database.AddInParameter(sqlStringCommand, "pageSize", DbType.Int32, pageSize);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    object obj = dataReader["total"];
                    if (obj != DBNull.Value)
                    {
                        total = Convert.ToInt32(dataReader["total"]);
                    }
                }
                dataReader.NextResult();
                result = ReaderConvert.ReaderToList<TplCfgInfo>(dataReader);
            }
            return result;
        }

        public IList<NavigateInfo> GetAllNavigate(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=2 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));
            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select *,case when LocationType = 4 then (select isnull(Depth,0) from Ecshop_Categories where CategoryId=Url) else 0 end Depth from  Ecshop_Banner where type=2 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
            IList<NavigateInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<NavigateInfo>(dataReader);
            }
            return result;
        }

        public IList<NavigateInfo> GetAllNavigate(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=2 AND Client= {0} and supplierId={1} ORDER BY DisplaySequence ASC", (int)client, supplierId));
            IList<NavigateInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<NavigateInfo>(dataReader);
            }
            return result;
        }


        public IList<HotSaleInfo> GetAllHotSaleNomarl(ClientType client)
        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"SELECT * FROM 
//                                                                                            (
//                                                                                            select 
//                                                                                            ROW_NUMBER() OVER(ORDER BY DisplaySequence) rid,*
//                                                                                            from  Ecshop_Banner where type=11 AND Client=1 
//                                                                                            ) a WHERE rid!=1", (int)client));
            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"SELECT * FROM 
                                                                                            (
                                                                                            select 
                                                                                            ROW_NUMBER() OVER(ORDER BY DisplaySequence) rid,*
                                                                                            from  Ecshop_Banner where type=11 AND Client=1 and (SupplierId=0 or SupplierId is null) 
                                                                                            ) a WHERE rid!=1", (int)client));
            IList<HotSaleInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HotSaleInfo>(dataReader);
            }
            return result;
        }

        public IList<HotSaleInfo> GetAllHotSaleNomarl(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"SELECT * FROM 
                                                                                            (
                                                                                            select 
                                                                                            ROW_NUMBER() OVER(ORDER BY DisplaySequence) rid,*
                                                                                            from  Ecshop_Banner where type=11 AND Client={0} and supplierId={1} 
                                                                                            ) a WHERE rid!=1", (int)client, supplierId));
            IList<HotSaleInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HotSaleInfo>(dataReader);
            }
            return result;
        }

        public DataTable GetAllHotSaleNomarl(int count, int between, int and, int supplierid, ClientType clientType)
        {
            DataTable result;
            DbCommand command = this.database.GetStoredProcCommand("cp_Index_PCHotProductList_Get");
            this.database.AddInParameter(command, "TopNum", DbType.Int32, count);
            this.database.AddInParameter(command, "MinNum", DbType.Int32, between);
            this.database.AddInParameter(command, "MaxNum", DbType.Int32, and);
            this.database.AddInParameter(command, "SupplierId", DbType.Int32, supplierid);
            this.database.AddInParameter(command, "Client", DbType.Int32, (int)clientType);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
        }


        public IList<HotSaleInfo> GetAllHotSaleTop(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top 1 * from  Ecshop_Banner where type=11 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));

            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top 1 * from  Ecshop_Banner where type=11 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
            IList<HotSaleInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HotSaleInfo>(dataReader);
            }
            return result;
        }

        public IList<HotSaleInfo> GetAllHotSaleTop(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top 1 * from  Ecshop_Banner where type=11 AND Client= {0} and supplierId={1} ORDER BY DisplaySequence ASC", (int)client, supplierId));
            IList<HotSaleInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HotSaleInfo>(dataReader);
            }
            return result;
        }

        public IList<HotSaleInfo> GetAllHotSale(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select  * from  Ecshop_Banner where type=11 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));

            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select  * from  Ecshop_Banner where type=11 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
           
            IList<HotSaleInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HotSaleInfo>(dataReader);
            }
            return result;
        }

        public IList<HotSaleInfo> GetAllHotSale(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select  * from  Ecshop_Banner where type=11 AND Client= {0} and SupplierId={1} ORDER BY DisplaySequence ASC", (int)client, supplierId));
            IList<HotSaleInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HotSaleInfo>(dataReader);
            }
            return result;
        }


        public IList<RecommendInfo> GetAllRecommend(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=12 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));

            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=12 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
            IList<RecommendInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<RecommendInfo>(dataReader);
            }
            return result;
        }

        public IList<RecommendInfo> GetAllRecommend(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=12 AND Client= {0} and SupplierId={1} ORDER BY DisplaySequence ASC", (int)client, supplierId));
            IList<RecommendInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<RecommendInfo>(dataReader);
            }
            return result;
        }

        public IList<RecommendInfo> GetAllRecommendTop(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top 1 * from  Ecshop_Banner where type=12 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));
            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top 1 * from  Ecshop_Banner where type=12 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
            IList<RecommendInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<RecommendInfo>(dataReader);
            }
            return result;
        }

        public IList<RecommendInfo> GetAllRecommendTop(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top 1 * from  Ecshop_Banner where type=12 AND Client= {0} and SupplierId={1} ORDER BY DisplaySequence ASC", (int)client, supplierId));
            IList<RecommendInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<RecommendInfo>(dataReader);
            }
            return result;
        }

        public IList<RecommendInfo> GetAllRecommendNormal(ClientType client)
        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"SELECT * FROM 
//                                                                            (
//                                                                            select 
//                                                                            ROW_NUMBER() OVER(ORDER BY DisplaySequence) rid,*
//                                                                            from  Ecshop_Banner where type=12 AND Client=1 
//                                                                            ) a WHERE rid!=1", (int)client));


            //若不是供应商，则supplierid=0 或 null

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"SELECT * FROM 
                                                                            (
                                                                            select 
                                                                            ROW_NUMBER() OVER(ORDER BY DisplaySequence) rid,*
                                                                            from  Ecshop_Banner where type=12 AND Client=1 and (SupplierId=0 or SupplierId is null) 
                                                                            ) a WHERE rid!=1", (int)client));
            IList<RecommendInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<RecommendInfo>(dataReader);
            }
            return result;
        }

        public IList<RecommendInfo> GetAllRecommendNormal(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"SELECT * FROM 
                                                                            (
                                                                            select 
                                                                            ROW_NUMBER() OVER(ORDER BY DisplaySequence) rid,*
                                                                            from  Ecshop_Banner where type=12 AND Client={0} and SupplierId={1}
                                                                            ) a WHERE rid!=1", (int)client, supplierId));
            IList<RecommendInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<RecommendInfo>(dataReader);
            }
            return result;
        }


        public IList<PromotionalInfo> GetAllPromotional(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=13 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));
            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=13 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
            IList<PromotionalInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<PromotionalInfo>(dataReader);
            }
            return result;
        }

        public IList<IconInfo> GetAllIcon(ClientType client)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=15 AND Client= {0} ORDER BY DisplaySequence ASC", (int)client));
            //若不是供应商，则supplierid=0 或 null
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=15 AND Client= {0} and (SupplierId=0 or SupplierId is null) ORDER BY DisplaySequence ASC", (int)client));
            IList<IconInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<IconInfo>(dataReader);
            }
            return result;
        }

        public IList<PromotionalInfo> GetAllPromotional(ClientType client,int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from  Ecshop_Banner where type=13 AND Client= {0} and supplierId={1} ORDER BY DisplaySequence ASC", (int)client, supplierId));
            IList<PromotionalInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<PromotionalInfo>(dataReader);
            }
            return result;
        }

        private int GetMaxBannerSequence()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select max(DisplaySequence) from Ecshop_Banner");
            int result;
            if (this.database.ExecuteScalar(sqlStringCommand) != DBNull.Value)
            {
                result = 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            }
            else
            {
                result = 1;
            }
            return result;
        }
        public bool SaveTplCfg(TplCfgInfo info)
        {
            int maxBannerSequence = this.GetMaxBannerSequence();
            StringBuilder stringBuilder = new StringBuilder("insert into  Ecshop_Banner (ShortDesc,ImageUrl,DisplaySequence,LocationType,Url,Type,Client,IsDisable,SupplierId)");
            stringBuilder.Append("values (@ShortDesc,@ImageUrl,@DisplaySequence,@LocationType,@Url,@Type,@Client,@IsDisable,@SupplierId)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ShortDesc", DbType.String, info.ShortDesc);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.String, maxBannerSequence);
            this.database.AddInParameter(sqlStringCommand, "LocationType", DbType.Int32, (int)info.LocationType);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, info.Type);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, info.Client);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, info.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, info.SupplierId);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateTplCfg(TplCfgInfo info)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("update Ecshop_Banner set ");
            stringBuilder.Append("ShortDesc=@ShortDesc,");
            stringBuilder.Append("ImageUrl=@ImageUrl,");
            stringBuilder.Append("DisplaySequence=@DisplaySequence,");
            stringBuilder.Append("LocationType=@LocationType,");
            stringBuilder.Append("Url=@Url,");
            stringBuilder.Append("Type=@Type,");
            stringBuilder.Append("Client=@Client,");
            stringBuilder.Append("IsDisable=@IsDisable,");
            stringBuilder.Append("SupplierId=@SupplierId");

            stringBuilder.Append(" where BannerId=@BannerId ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BannerId", DbType.Int32, info.Id);
            this.database.AddInParameter(sqlStringCommand, "ShortDesc", DbType.String, info.ShortDesc);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.String, info.DisplaySequence);
            this.database.AddInParameter(sqlStringCommand, "LocationType", DbType.Int32, (int)info.LocationType);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, info.Type);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, info.Client);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, info.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, info.SupplierId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public TplCfgInfo GetTplCfgById(int id)
        {
            StringBuilder stringBuilder = new StringBuilder(" select * from Ecshop_Banner where BannerId=@BannerId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BannerId", DbType.Int32, id);
            TplCfgInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<TplCfgInfo>(dataReader);
            }
            return result;
        }
        public bool DelTplCfg(int id)
        {
            StringBuilder stringBuilder = new StringBuilder(" delete from Ecshop_Banner where BannerId=@BannerId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BannerId", DbType.Int32, id);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public string GetProvinceByIp(long ipAddress)
        {
            if (ipAddress == 0)
            {
                return "";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select Province from Ecshop_IpAddress where {0} between StartIp  and EndIp", ipAddress.ToString()));
            object result = this.database.ExecuteScalar(sqlStringCommand);
            if (result != DBNull.Value)
            {
                return result.ToString();
            }
            return "";
        }
        /// <summary>
        /// 根据IP地址获取站点名称
        /// </summary>
        /// <param name="IpAddress"></param>
        /// <returns></returns>
        public string GetSiteName(long IpAddress)
        {
            string sql = "select top 1 City from Ecshop_IpAddress where " + IpAddress.ToString() + " between StartIp  and EndIp";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                object result = this.database.ExecuteScalar(sqlStringCommand);
                if (result != DBNull.Value)
                {
                    result = this.database.ExecuteScalar(sqlStringCommand);
                }
                else
                {
                    result = "";
                }
                return result == null ? "无" : result.ToString();
            }
            catch (Exception ee)
            {
                return ee.Message.ToString() + "Sql:" + sql;
            }
        }


    }
}
