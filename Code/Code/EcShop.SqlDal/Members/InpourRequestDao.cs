using EcShop.Entities;
using EcShop.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Members
{
	public class InpourRequestDao
	{
		private Database database;
		public InpourRequestDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool IsRecharge(string inpourId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_BalanceDetails WHERE InpourId = @InpourId");
			this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public bool AddInpourBlance(InpourRequestInfo inpourRequest)
		{
			bool result;
			if (null == inpourRequest)
			{
				result = false;
			}
			else
			{
				DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_InpourRequest_Create");
				this.database.AddInParameter(storedProcCommand, "InpourId", DbType.String, inpourRequest.InpourId);
				this.database.AddInParameter(storedProcCommand, "TradeDate", DbType.DateTime, inpourRequest.TradeDate);
				this.database.AddInParameter(storedProcCommand, "InpourBlance", DbType.Currency, inpourRequest.InpourBlance);
				this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, inpourRequest.UserId);
				this.database.AddInParameter(storedProcCommand, "PaymentId", DbType.String, inpourRequest.PaymentId);
				this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public InpourRequestInfo GetInpourBlance(string inpourId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_InpourRequest WHERE InpourId = @InpourId;");
			this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
			InpourRequestInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateInpourRequest(dataReader);
				}
			}
			return result;
		}
		public void RemoveInpourRequest(string inpourId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_InpourRequest WHERE InpourId = @InpourId");
			this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
