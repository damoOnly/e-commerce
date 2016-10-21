using EcShop.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Sales
{
	public class ExpressTemplateDao
	{
		private Database database;
		public ExpressTemplateDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool AddExpressTemplate(string expressName, string xmlFile)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ExpressTemplates(ExpressName, XmlFile, IsUse) VALUES(@ExpressName, @XmlFile, 1)");
			this.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
			this.database.AddInParameter(sqlStringCommand, "XmlFile", DbType.String, xmlFile);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateExpressTemplate(int expressId, string expressName)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_ExpressTemplates SET ExpressName = @ExpressName WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
			this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool SetExpressIsUse(int expressId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_ExpressTemplates SET IsUse = ~IsUse WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteExpressTemplate(int expressId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ExpressTemplates WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DataTable GetExpressTemplates(bool? isUser)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ExpressTemplates");
			if (isUser.HasValue)
			{
				DbCommand expr_24 = sqlStringCommand;
				expr_24.CommandText += string.Format(" WHERE IsUse = '{0}'", isUser);
			}
			DataTable result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
	}
}
