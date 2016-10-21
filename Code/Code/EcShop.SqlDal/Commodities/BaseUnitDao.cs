using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.Commodities
{
    public class BaseUnitDao
    {
        private Database database;
        public BaseUnitDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 新增计量单位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUnit(BaseUnitsInfo model)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(Sort) IS NULL THEN 1 ELSE MAX(Sort) + 1 END) FROM Ecshop_BaseUnits;INSERT INTO dbo.Ecshop_BaseUnits( HSJoinID, Name_CN,Sort,CreateUser,CreateTime )VALUES(@HSJoinID,@Name_CN,@DisplaySequence,@CreateUser,getdate()); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "HSJoinID", DbType.String, model.HSJoinID);
            this.database.AddInParameter(sqlStringCommand, "Name_CN", DbType.String, model.Name_CN);
            this.database.AddInParameter(sqlStringCommand, "CreateUser", DbType.String, model.CreateUser);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                if (Convert.ToInt32(obj) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 编辑计量单位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditUnit(BaseUnitsInfo model)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update dbo.Ecshop_BaseUnits set HSJoinID=@HSJoinID, Name_CN=@Name_CN,UpdateUser=@UpdateUser,UpdateTime=getdate() where id=@id");
            this.database.AddInParameter(sqlStringCommand, "HSJoinID", DbType.String, model.HSJoinID);
            this.database.AddInParameter(sqlStringCommand, "Name_CN", DbType.String, model.Name_CN);
            this.database.AddInParameter(sqlStringCommand, "UpdateUser", DbType.String, model.UpdateUser);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, model.ID);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 是否存在计量单位
        /// </summary>
        /// <param name="HSJoinID">海关代码</param>
        /// <param name="Name_CN">名称</param>
        /// <returns></returns>
        public bool IsExistUnit(string HSJoinID, string Name_CN,int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(1) from Ecshop_BaseUnits where (HSJoinID=@HSJoinID or Name_CN=@Name_CN)  AND DeleteFlag=0 and id<>@id ");
            this.database.AddInParameter(sqlStringCommand, "HSJoinID", DbType.String, HSJoinID);
            this.database.AddInParameter(sqlStringCommand, "Name_CN", DbType.String, Name_CN);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, id);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                if (Convert.ToInt32(obj) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据计量单位名称或海关代码获取数据
        /// </summary>
        /// <param name="UnitName">计量单位名称或海关代码</param>
        /// <returns></returns>
        public DataTable GetUnits(string Key)
        {
            string text = " DeleteFlag=0";
            if (!string.IsNullOrEmpty(Key))
            {
                text = text + " AND (Name_CN LIKE '%" + DataHelper.CleanSearchString(Key) + "%' OR HSJoinID LIKE '%" + DataHelper.CleanSearchString(Key) + "%')";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ID,HSJoinID ,Name_CN , Name_En ,ForbitFlag ,Sort ,DeleteFlag ,CreateUser ,CreateTime ,UpdateUser ,UpdateTime ,DeleteUser ,DeleteTime FROM Ecshop_BaseUnits  WHERE " + text + " ORDER BY Sort");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据计量单位Id获取数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataTable GetUnitsById(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ID,HSJoinID ,Name_CN , Name_En ,ForbitFlag ,Sort ,DeleteFlag ,CreateUser ,CreateTime ,UpdateUser ,UpdateTime ,DeleteUser ,DeleteTime FROM Ecshop_BaseUnits WHERE DeleteFlag=0 and Id=@Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取计量单位
        /// </summary>
        /// <returns></returns>
        public DataTable GetUnits()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ID,HSJoinID ,Name_CN , Name_En ,ForbitFlag ,Sort ,DeleteFlag ,CreateUser ,CreateTime ,UpdateUser ,UpdateTime ,DeleteUser ,DeleteTime FROM Ecshop_BaseUnits where DeleteFlag=0 ORDER BY Sort");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据Id删除计量单位
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteUnit(int Id, string DeleteUser)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_BaseUnits SET DeleteFlag=1,DeleteUser=@DeleteUser,DeleteTime=getdate() WHERE ID = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            this.database.AddInParameter(sqlStringCommand, "DeleteUser", DbType.String, DeleteUser);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        /// <param name="UnitId"></param>
        /// <param name="displaysequence"></param>
        /// <returns></returns>
        public bool UpdateUnitDisplaySequence(int UnitId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_BaseUnits set Sort=@DisplaySequence where Id=@Id");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@Id", DbType.Int32, UnitId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}
