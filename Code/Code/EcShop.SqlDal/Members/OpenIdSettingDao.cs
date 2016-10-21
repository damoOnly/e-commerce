using EcShop.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Members
{
	public class OpenIdSettingDao
	{
		private Database database;
		public OpenIdSettingDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public void SaveSettings(OpenIdSettingsInfo settings)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_OpenIdSettings_Save");
			this.database.AddInParameter(storedProcCommand, "OpenIdType", DbType.String, settings.OpenIdType.ToLower());
			this.database.AddInParameter(storedProcCommand, "Name", DbType.String, settings.Name);
			this.database.AddInParameter(storedProcCommand, "Description", DbType.String, settings.Description);
			this.database.AddInParameter(storedProcCommand, "Settings", DbType.String, settings.Settings);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public void DeleteSettings(string openIdType)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType.ToLower());
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
		{
			OpenIdSettingsInfo openIdSettingsInfo = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType.ToLower());
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					openIdSettingsInfo = new OpenIdSettingsInfo
					{
						OpenIdType = openIdType,
						Name = (string)dataReader["Name"],
						Settings = (string)dataReader["Settings"]
					};
					if (dataReader["Description"] != DBNull.Value)
					{
						openIdSettingsInfo.Description = (string)dataReader["Description"];
					}
				}
			}
			return openIdSettingsInfo;
		}
		public IList<OpenIdSettingsInfo> GetConfigedItems()
		{
			IList<OpenIdSettingsInfo> list = new List<OpenIdSettingsInfo>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					OpenIdSettingsInfo item = this.PopulateOpenIdSettings(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		private OpenIdSettingsInfo PopulateOpenIdSettings(IDataReader reader)
		{
			OpenIdSettingsInfo openIdSettingsInfo = new OpenIdSettingsInfo
			{
				OpenIdType = (string)reader["OpenIdType"],
				Name = (string)reader["Name"],
				Settings = (string)reader["Settings"]
			};
			if (reader["Description"] != DBNull.Value)
			{
				openIdSettingsInfo.Description = (string)reader["Description"];
			}
			return openIdSettingsInfo;
		}
		public IList<string> GetConfigedTypes()
		{
			IList<string> list = new List<string>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OpenIdType FROM aspnet_OpenIdSettings");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader.GetString(0));
				}
			}
			return list;
		}
	}
}
