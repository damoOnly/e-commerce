using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Sales
{
	public class PaymentModeDao
	{
		private Database database;
		public PaymentModeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PaymentType_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
				if (action == DataProviderAction.Create)
				{
					this.database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
				}
				else
				{
					this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, paymentMode.ModeId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", DbType.String, paymentMode.Name);
					this.database.AddInParameter(storedProcCommand, "Description", DbType.String, paymentMode.Description);
					this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, paymentMode.Gateway);
					this.database.AddInParameter(storedProcCommand, "IsUseInpour", DbType.Boolean, paymentMode.IsUseInpour);
					this.database.AddInParameter(storedProcCommand, "Charge", DbType.Currency, paymentMode.Charge);
					this.database.AddInParameter(storedProcCommand, "IsPercent", DbType.Boolean, paymentMode.IsPercent);
					this.database.AddInParameter(storedProcCommand, "Settings", DbType.String, paymentMode.Settings);
					this.database.AddInParameter(storedProcCommand, "ApplicationType", DbType.Int32, paymentMode.ApplicationType);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				PaymentModeActionStatus paymentModeActionStatus = (PaymentModeActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
				result = paymentModeActionStatus;
			}
			return result;
		}
		public void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Ecshop_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public PaymentModeInfo GetPaymentMode(int modeId)
		{
			PaymentModeInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_PaymentTypes WHERE ModeId = @ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public PaymentModeInfo GetPaymentMode(string gateway)
		{
			PaymentModeInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Ecshop_PaymentTypes WHERE Gateway = @Gateway");
			this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
		{
			IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
			string text = "SELECT * FROM Ecshop_PaymentTypes";
			if (payApplicationType != PayApplicationType.payOnAll)
			{
				text += " where ApplicationType=@ApplicationType";
			}
			text += " Order by DisplaySequence desc";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ApplicationType", DbType.Int32, (int)payApplicationType);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
	}
}
