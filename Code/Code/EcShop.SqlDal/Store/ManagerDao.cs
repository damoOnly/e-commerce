using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Store;
using EcShop.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Store
{
	public class ManagerDao
	{
		private Database database;
		public ManagerDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetManagers(ManagerQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			if (query.RoleId != Guid.Empty)
			{
				stringBuilder.AppendFormat(" AND UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", query.RoleId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Managers", "UserId", stringBuilder.ToString(), "*");
		}
		public void ClearRolePrivilege(Guid roleId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT UserName FROM vw_aspnet_Managers WHERE UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", roleId));
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					RoleHelper.SignOut((string)dataReader["UserName"]);
				}
			}
		}
		public bool DeleteManager(int userId)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Manager_Delete");
			this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
			this.database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
			return parameterValue != null && parameterValue != DBNull.Value && Convert.ToInt32(parameterValue) == 0;
		}
        public bool AddSupplierUser(int supplierId, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Ecshop_SupplierUser values(@supplierId,@userId)");
            this.database.AddInParameter(sqlStringCommand,"supplierId", DbType.Int32, supplierId);
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
            try
            {
                return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            catch 
            {
                return false;
            }
        }

        public bool AddStoreIdUser(int storeId, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Ecshop_StoreIdUser values(@storeId,@userId)");
            this.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
            try
            {
                return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            catch
            {
                return false;
            }
        }

        //根据用户Id获取门店Id
        public int GetStoreIdByUserId(int userId)
        {
            int storeId = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select StoreId from dbo.Ecshop_StoreIdUser where UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            try
            {
                storeId = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                storeId = 0;
            }
            return storeId;
        }

	}
}
