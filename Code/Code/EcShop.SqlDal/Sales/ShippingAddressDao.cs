using EcShop.Entities;
using EcShop.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace EcShop.SqlDal.Sales
{
	public class ShippingAddressDao
	{
		private Database database;
		public ShippingAddressDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public IList<ShippingAddressInfo> GetShippingAddresses(int userId)
		{
			IList<ShippingAddressInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_UserShippingAddresses WHERE  UserID = @UserID order by IsDefault desc,ShippingId desc");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ShippingAddressInfo>(dataReader);
			}
			return result;
		}
		public int GetShippingAddressCount(int userId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ShippingId) FROM Ecshop_UserShippingAddresses WHERE  UserID = @UserID");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return (int)((obj == DBNull.Value) ? "0" : obj);
		}
		public int AddShippingAddress(ShippingAddressInfo shippingAddress)
		{
            if (shippingAddress.IsDefault == true)
            {
                ChangeShippingAddressDefault(shippingAddress.UserId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_UserShippingAddresses(RegionId,UserId,ShipTo,Address,Zipcode,TelPhone,CellPhone,IsDefault,IdentityCard) VALUES(@RegionId,@UserId,@ShipTo,@Address,@Zipcode,@TelPhone,@CellPhone,@IsDefault,@IdentityCard); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, shippingAddress.UserId);
			this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
			this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shippingAddress.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, shippingAddress.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "IdentityCard", DbType.String, shippingAddress.IdentityCard);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
		public bool DelShippingAddress(int shippingid, int userid)
		{
			StringBuilder stringBuilder = new StringBuilder("delete from Ecshop_UserShippingAddresses");
			stringBuilder.Append(" where shippingId=@shippingId and UserId=@UserId ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingid);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

        /// <summary>
        /// 修改默认显示地址时修改其他地址信息IsDefault=false;
        /// </summary>
        private void ChangeShippingAddressDefault(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_UserShippingAddresses SET IsDefault=0 WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteScalar(sqlStringCommand);
        }
		public bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update Ecshop_UserShippingAddresses");
			stringBuilder.Append(" set ShipTo=@ShipTo,");
			stringBuilder.Append("Address=@Address,");
			stringBuilder.Append("Zipcode=@Zipcode,");
			stringBuilder.Append("TelPhone=@TelPhone,");
			stringBuilder.Append("CellPhone=@CellPhone,");
			stringBuilder.Append(" RegionId=@RegionId,");
            stringBuilder.Append("  IdentityCard=@IdentityCard");
			stringBuilder.Append(" where shippingId=@shippingId");
			stringBuilder.Append(" and UserId=@UserId");

			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, shippingAddress.UserId);
			this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
			this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shippingAddress.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingAddress.ShippingId);
            			this.database.AddInParameter(sqlStringCommand, "IdentityCard", DbType.String, shippingAddress.IdentityCard);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool SetDefaultShippingAddress(int shippingId, int UserId)
		{
			StringBuilder stringBuilder = new StringBuilder("UPDATE  Ecshop_UserShippingAddresses SET IsDefault = 0 where UserId=@UserId;");
			stringBuilder.Append("UPDATE  Ecshop_UserShippingAddresses SET IsDefault = 1 WHERE ShippingId = @ShippingId");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public ShippingAddressInfo GetShippingAddress(int shippingId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_UserShippingAddresses WHERE ShippingId = @ShippingId");
			this.database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, shippingId);
			ShippingAddressInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShippingAddress(dataReader);
				}
			}
			return result;
		}

        public int AddOrUpdateShippingAddress(ShippingAddressInfo shippingAddress, int newUserId,out int newShippingId)
        {
            int result = 0;
            newShippingId = 0;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_AddOrUpdateShippingAddress");
            this.database.AddInParameter(storedProcCommand, "ShippingId", DbType.Int32, shippingAddress.ShippingId);
            this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, shippingAddress.UserId);
            this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
            this.database.AddInParameter(storedProcCommand, "Address", DbType.String, shippingAddress.Address);
            this.database.AddInParameter(storedProcCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
            this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
            this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
            this.database.AddInParameter(storedProcCommand, "IsDefault", DbType.Int32, shippingAddress.IsDefault);
            this.database.AddInParameter(storedProcCommand, "IdentityCard", DbType.String, shippingAddress.IdentityCard);
            this.database.AddInParameter(storedProcCommand, "NewUserId", DbType.Int32, newUserId);
            this.database.AddOutParameter(storedProcCommand, "NewShippingId", DbType.Int32, 4);

            result = this.database.ExecuteNonQuery(storedProcCommand);

            int.TryParse(this.database.GetParameterValue(storedProcCommand, "NewShippingId").ToString(), out newShippingId);

            return result;

        }

        public bool SetDefaultShippingAddressPC(int shippingId,int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_UserShippingAddresses  set IsDefault=(case when  ShippingId=@ShippingId then 1 else 0 end) where UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, shippingId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


	}
}
