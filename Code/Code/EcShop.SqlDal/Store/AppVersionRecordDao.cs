using EcShop.Core;
using EcShop.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Store
{
    public class AppVersionRecordDao
    {
        private Database database;
        public AppVersionRecordDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public AppVersionRecordInfo GetLatestAppVersionRecord(string device)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 * FROM Ecshop_AppVersionRecords WHERE Device = @Device ORDER BY Version DESC");
            this.database.AddInParameter(sqlStringCommand, "Device", DbType.String, device);
            AppVersionRecordInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<AppVersionRecordInfo>(dataReader);
            }
            return result;
        }

        public AppVersionRecordInfo GetLatestAppVersionRecordOrderById(string device)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 * FROM Ecshop_AppVersionRecords WHERE Device = @Device ORDER BY Id DESC");
            this.database.AddInParameter(sqlStringCommand, "Device", DbType.String, device);
            AppVersionRecordInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<AppVersionRecordInfo>(dataReader);
            }
            return result;
        }
        public bool ChangeAppRunImg(List<AppRunImgInfo> appRunImgInfo)
        {
            List<AppRunImgInfo> list = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_AppRunImg");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                list = ReaderConvert.ReaderToList<AppRunImgInfo>(dataReader) as List<AppRunImgInfo>;
            }
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            int i = 0;
            foreach (AppRunImgInfo r in appRunImgInfo)
            {
                if (list.Exists(a => a.phoneType == r.phoneType))
                {
                    sql.AppendFormat("UPDATE Ecshop_AppRunImg SET imgSrc=@imgSrc{0} WHERE phoneType=@phoneType{0};",i);
                }
                else
                {
                    sql.AppendFormat("INSERT INTO Ecshop_AppRunImg (imgSrc,phoneType) VALUES(@imgSrc{0},@phoneType{0});", i);
                }
                i++;
            }
            i=0;
            sqlStringCommand = this.database.GetSqlStringCommand(sql.ToString());
            foreach (AppRunImgInfo r in appRunImgInfo)
            {
                this.database.AddInParameter(sqlStringCommand, "imgSrc"+i, DbType.String, r.imgSrc);
                this.database.AddInParameter(sqlStringCommand, "phoneType" + i, DbType.String, r.phoneType);
                i++;
            }
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public List<AppRunImgInfo> GetAppRunImg()
        {
            List<AppRunImgInfo> list = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_AppRunImg");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                list = ReaderConvert.ReaderToList<AppRunImgInfo>(dataReader) as List<AppRunImgInfo>;
            }
            return list;
        }
        public bool AddAppVersionRecord(AppVersionRecordInfo appVersionRecord)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_AppVersionRecords(Device,Version, Description, IsForcibleUpgrade, UpgradeUrl, VersionName, UpgradeInfoUrl, AppName, AppPackageName) VALUES(@Device,@Version, @Description, @IsForcibleUpgrade, @UpgradeUrl, @VersionName, @UpgradeInfoUrl, @AppName, @AppPackageName)");
            this.database.AddInParameter(sqlStringCommand, "Device", DbType.String, appVersionRecord.Device);
            this.database.AddInParameter(sqlStringCommand, "Version", DbType.Decimal, appVersionRecord.Version);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, appVersionRecord.Description);
            this.database.AddInParameter(sqlStringCommand, "IsForcibleUpgrade", DbType.Boolean, appVersionRecord.IsForcibleUpgrade);
            this.database.AddInParameter(sqlStringCommand, "UpgradeUrl", DbType.String, appVersionRecord.UpgradeUrl);
            this.database.AddInParameter(sqlStringCommand, "VersionName", DbType.String, appVersionRecord.VersionName);
            this.database.AddInParameter(sqlStringCommand, "UpgradeInfoUrl", DbType.String, appVersionRecord.UpgradeInfoUrl);
            this.database.AddInParameter(sqlStringCommand, "AppName", DbType.String, appVersionRecord.AppName);
            this.database.AddInParameter(sqlStringCommand, "AppPackageName", DbType.String, appVersionRecord.AppPackageName);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        public bool UpdateAppVersionRecord(AppVersionRecordInfo appVersionRecord)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_AppVersionRecords set Device=@Device,Version=@Version,Description=@Description, IsForcibleUpgrade=@IsForcibleUpgrade, UpgradeUrl=@UpgradeUrl, VersionName=@VersionName, UpgradeInfoUrl=@UpgradeInfoUrl, AppName=@AppName, AppPackageName=@AppPackageName where Id=@Id ");
            this.database.AddInParameter(sqlStringCommand, "Device", DbType.String, appVersionRecord.Device);
            this.database.AddInParameter(sqlStringCommand, "Version", DbType.Decimal, appVersionRecord.Version);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, appVersionRecord.Description);
            this.database.AddInParameter(sqlStringCommand, "IsForcibleUpgrade", DbType.Boolean, appVersionRecord.IsForcibleUpgrade);
            this.database.AddInParameter(sqlStringCommand, "UpgradeUrl", DbType.String, appVersionRecord.UpgradeUrl);
            this.database.AddInParameter(sqlStringCommand, "VersionName", DbType.String, appVersionRecord.VersionName);
            this.database.AddInParameter(sqlStringCommand, "UpgradeInfoUrl", DbType.String, appVersionRecord.UpgradeInfoUrl);
            this.database.AddInParameter(sqlStringCommand, "AppName", DbType.String, appVersionRecord.AppName);
            this.database.AddInParameter(sqlStringCommand, "AppPackageName", DbType.String, appVersionRecord.AppPackageName);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, appVersionRecord.Id);


            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        public bool IsForcibleUpgrade(string device, decimal version)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT IsForcibleUpgrade FROM Ecshop_AppVersionRecords WHERE Version > @Version AND Device = @Device");
            this.database.AddInParameter(sqlStringCommand, "Device", DbType.String, device);
            this.database.AddInParameter(sqlStringCommand, "Version", DbType.Decimal, version);
            bool result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    if ((bool)dataReader[0])
                    {
                        result = true;
                        return result;
                    }
                }
            }
            result = false;
            return result;
        }
        public bool AddAppInstallRecord(AppInstallRecordInfo appInstallRecord)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_AppInstallRecords WHERE VID=@VID; INSERT INTO Ecshop_AppInstallRecords(VID,Device) VALUES(@VID,@Device)");
            this.database.AddInParameter(sqlStringCommand, "VID", DbType.String, appInstallRecord.VID);
            this.database.AddInParameter(sqlStringCommand, "Device", DbType.String, appInstallRecord.Device);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public DataTable GetAppVersionRecordList(string device)
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT  * FROM Ecshop_AppVersionRecords WHERE Device = @Device ORDER BY Version DESC");
            this.database.AddInParameter(sqlStringCommand, "Device", DbType.String, device);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public AppVersionRecordInfo GetAppVersionById(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 * FROM Ecshop_AppVersionRecords WHERE Id = @Id ");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
            AppVersionRecordInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<AppVersionRecordInfo>(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据先前的版本号判断是否强制升级
        /// </summary>
        /// <param name="version"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public bool IsForcibleUpgradeByPreviousVersion(decimal version, string device)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(1) from dbo.Ecshop_AppVersionRecords where device=@device and IsForcibleUpgrade=1 and [Version]>@Version");
            this.database.AddInParameter(sqlStringCommand, "device", DbType.String, device);
            this.database.AddInParameter(sqlStringCommand, "Version", DbType.Decimal, version);

            object resultobj = this.database.ExecuteScalar(sqlStringCommand);

            int result = 0;
            if(resultobj!=null)
            {
                int.TryParse(resultobj.ToString(), out result);
            }

            return result > 0;
        }


        /// <summary>
        /// 根据id删除版本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelAppVersionById(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Delete from dbo.Ecshop_AppVersionRecords where Id=@Id ");

            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}
