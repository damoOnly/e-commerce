using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Core;
using EcShop.Core.Entities;
using System.Text;

namespace EcShop.SqlDal.Comments
{
	public class AfficheDao
	{
		private Database database;
		public AfficheDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool AddAffiche(AfficheInfo affiche)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Affiche(Title, Content, AddedDate) VALUES (@Title, @Content, @AddedDate)");
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, affiche.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, affiche.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, affiche.AddedDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool UpdateAffiche(AfficheInfo affiche)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Affiche SET Title = @Title, AddedDate = @AddedDate, Content = @Content WHERE AfficheId = @AfficheId");
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, affiche.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, affiche.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, affiche.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, affiche.AfficheId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool DeleteAffiche(int afficheId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Affiche WHERE AfficheId = @AfficheId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public AfficheInfo GetAffiche(int afficheId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Affiche WHERE AfficheId = @AfficheId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
			AfficheInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateAffiche(dataReader);
				}
			}
			return result;
		}
		public List<AfficheInfo> GetAfficheList()
		{
			List<AfficheInfo> list = new List<AfficheInfo>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Affiche ORDER BY AddedDate DESC");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					AfficheInfo item = DataMapper.PopulateAffiche(dataReader);
					list.Add(item);
				}
			}
			return list;
		}

        public DataTable GetAffiches(int top)
        {
            string sql = string.Format("SELECT top {0} * FROM Ecshop_Affiche ORDER BY AddedDate DESC", top);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

		public AfficheInfo GetFrontOrNextAffiche(int afficheId, string type)
		{
			string query = string.Empty;
			if (type == "Next")
			{
				query = "SELECT TOP 1 * FROM Ecshop_Affiche WHERE AfficheId < @AfficheId  ORDER BY AfficheId DESC";
			}
			else
			{
				query = "SELECT TOP 1 * FROM Ecshop_Affiche WHERE AfficheId > @AfficheId ORDER BY AfficheId ASC";
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
			AfficheInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateAffiche(dataReader);
				}
			}
			return result;
		}

        public DbQueryResult GetAfficheList(AfficheQuery afficheQuery)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(afficheQuery.Keywords));
            if (afficheQuery.StartArticleTime.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate >= '{0}'", afficheQuery.StartArticleTime.Value);
            }
            if (afficheQuery.EndArticleTime.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate <= '{0}'", afficheQuery.EndArticleTime.Value);
            }
            return DataHelper.PagingByRownumber(afficheQuery.PageIndex, afficheQuery.PageSize, afficheQuery.SortBy, afficheQuery.SortOrder, afficheQuery.IsCount, "Ecshop_Affiche", "AfficheId", stringBuilder.ToString(), "*");
        }
	}
}
