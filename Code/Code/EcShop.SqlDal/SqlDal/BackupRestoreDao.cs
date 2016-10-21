using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace EcShop.SqlDal
{
	public class BackupRestoreDao
	{
		private Database database;
		public BackupRestoreDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		private string StringCut(string str, string bg, string ed)
		{
			string text = str.Substring(str.IndexOf(bg) + bg.Length);
			return text.Substring(0, text.IndexOf(ed));
		}
		public string BackupData(string path)
		{
			string text;
			using (DbConnection dbConnection = this.database.CreateConnection())
			{
				text = dbConnection.Database;
			}
			string text2 = text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("backup database [{0}] to disk='{1}'", text, path + text2));
			string result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = text2;
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}
		public bool RestoreData(string bakFullName)
		{
			string arg;
			string dataSource;
			using (DbConnection dbConnection = this.database.CreateConnection())
			{
				arg = dbConnection.Database;
				dataSource = dbConnection.DataSource;
			}
			SqlConnection sqlConnection = new SqlConnection(string.Format("Data Source={0};Initial Catalog=master;Integrated Security=SSPI", dataSource));
			bool result;
			try
			{
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand(string.Format("SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", arg), sqlConnection);
				ArrayList arrayList = new ArrayList();
				using (IDataReader dataReader = sqlCommand.ExecuteReader())
				{
					while (dataReader.Read())
					{
						arrayList.Add(dataReader.GetInt16(0));
					}
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					sqlCommand = new SqlCommand(string.Format("KILL {0}", arrayList[i].ToString()), sqlConnection);
					sqlCommand.ExecuteNonQuery();
				}
				sqlCommand = new SqlCommand(string.Format("RESTORE DATABASE [{0}]  FROM DISK = '{1}' WITH REPLACE", arg, bakFullName), sqlConnection);
				sqlCommand.ExecuteNonQuery();
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				sqlConnection.Close();
			}
			return result;
		}
		public void Restor()
		{
			try
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch
			{
			}
		}
	}
}
