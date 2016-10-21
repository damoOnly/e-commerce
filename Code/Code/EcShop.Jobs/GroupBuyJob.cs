using EcShop.Core.Jobs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Xml;
namespace EcShop.Jobs
{
	public class GroupBuyJob : IJob
	{
		public void Execute(XmlNode node)
		{
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Ecshop_GroupBuy SET Status = 2 WHERE Status = 1 AND EndDate <= @CurrentTime;");
			database.AddInParameter(sqlStringCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now);
			database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
