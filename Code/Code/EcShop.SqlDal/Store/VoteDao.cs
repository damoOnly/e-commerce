using EcShop.Entities;
using EcShop.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Store
{
	public class VoteDao
	{
		private Database database;
		public VoteDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DataSet GetVotes(bool? isBackup = null)
		{
			string query = string.Empty;
			if (isBackup.HasValue)
			{
				query = "SELECT * FROM Ecshop_Votes WHERE IsBackup = 1; SELECT * FROM Ecshop_VoteItems WHERE  voteId IN (SELECT voteId FROM Ecshop_Votes WHERE IsBackup = 1)";
			}
			else
			{
				query = "SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Ecshop_VoteItems WHERE VoteId = Ecshop_Votes.VoteId) AS VoteCounts FROM Ecshop_Votes";
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public int SetVoteIsBackup(long voteId)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Votes_IsBackup");
			this.database.AddInParameter(storedProcCommand, "VoteId", DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(storedProcCommand);
		}
		public long CreateVote(VoteInfo vote)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Votes_Create");
			this.database.AddInParameter(storedProcCommand, "VoteName", DbType.String, vote.VoteName);
			this.database.AddInParameter(storedProcCommand, "IsBackup", DbType.Boolean, vote.IsBackup);
			this.database.AddInParameter(storedProcCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
			this.database.AddOutParameter(storedProcCommand, "VoteId", DbType.Int64, 8);
			long result = 0L;
			if (this.database.ExecuteNonQuery(storedProcCommand) > 0)
			{
				result = (long)this.database.GetParameterValue(storedProcCommand, "VoteId");
			}
			return result;
		}
		public bool UpdateVote(VoteInfo vote, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Votes SET VoteName = @VoteName, MaxCheck = @MaxCheck WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
			this.database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
			this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, vote.VoteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1;
		}
		public int DeleteVote(long voteId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Votes WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public int CreateVoteItem(VoteItemInfo voteItem, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
			this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteItem.VoteId);
			this.database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, voteItem.VoteItemName);
			this.database.AddInParameter(sqlStringCommand, "ItemCount", DbType.Int32, voteItem.ItemCount);
			int result;
			if (dbTran == null)
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			else
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
			}
			return result;
		}
		public bool DeleteVoteItem(long voteId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
		}
		public VoteInfo GetVoteById(long voteId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Ecshop_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM Ecshop_Votes WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			VoteInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateVote(dataReader);
				}
			}
			return result;
		}
		public IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			IList<VoteItemInfo> list = new List<VoteItemInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					VoteItemInfo item = DataMapper.PopulateVoteItem(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public VoteItemInfo GetVoteItem(long voteItemId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_VoteItems WHERE VoteItemId = @VoteItemId");
			this.database.AddInParameter(sqlStringCommand, "VoteItemId", DbType.Int64, voteItemId);
			VoteItemInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateVoteItem(dataReader);
				}
			}
			return result;
		}
		public int GetVoteCounts(long voteId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM Ecshop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public int Vote(long voteItemId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_VoteItems SET ItemCount = ItemCount + 1 WHERE VoteItemId = @VoteItemId");
			this.database.AddInParameter(sqlStringCommand, "VoteItemId", DbType.Int32, voteItemId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
