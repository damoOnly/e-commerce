using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Store
{
	public class FriendlyLinkDao
	{
		private Database database;
		public FriendlyLinkDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public IList<FriendlyLinksInfo> GetFriendlyLinks()
		{
			IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_FriendlyLinks ORDER BY DisplaySequence DESC");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateFriendlyLink(dataReader));
				}
			}
			return list;
		}
		public IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? number)
		{
			IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
			string query = string.Empty;
			if (number.HasValue)
			{
				query = string.Format("SELECT Top {0} * FROM Ecshop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence DESC", number.Value);
			}
			else
			{
				query = string.Format("SELECT * FROM Ecshop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence DESC", new object[0]);
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateFriendlyLink(dataReader));
				}
			}
			return list;
		}
		public FriendlyLinksInfo GetFriendlyLink(int linkId)
		{
			FriendlyLinksInfo result = new FriendlyLinksInfo();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_FriendlyLinks WHERE LinkId=@LinkId");
			this.database.AddInParameter(sqlStringCommand, "LinkId", DbType.Int32, linkId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateFriendlyLink(dataReader);
				}
			}
			return result;
		}
		public bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action)
		{
			bool result;
			if (null == friendlyLink)
			{
				result = false;
			}
			else
			{
				DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_FriendlyLink_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "LinkId", DbType.Int32, friendlyLink.LinkId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "ImageUrl", DbType.String, friendlyLink.ImageUrl);
					this.database.AddInParameter(storedProcCommand, "LinkUrl", DbType.String, friendlyLink.LinkUrl);
					this.database.AddInParameter(storedProcCommand, "Title", DbType.String, friendlyLink.Title);
					this.database.AddInParameter(storedProcCommand, "Visible", DbType.Boolean, friendlyLink.Visible);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Ecshop_FriendlyLinks", "LinkId", "DisplaySequence", linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
		}
		public int FriendlyLinkDelete(int linkId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_FriendlyLinks WHERE linkid = @linkid");
			this.database.AddInParameter(sqlStringCommand, "Linkid", DbType.Int32, linkId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
