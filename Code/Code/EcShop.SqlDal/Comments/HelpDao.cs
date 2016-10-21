using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Comments
{
	public class HelpDao
	{
		private Database database;
		public HelpDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool AddHelp(HelpInfo help)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Helps(CategoryId, Title, Meta_Description, Meta_Keywords, Description, Content, AddedDate, IsShowFooter) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords, @Description, @Content, @AddedDate, @IsShowFooter)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, help.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, help.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, help.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, help.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, help.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, help.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, help.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsShowFooter", DbType.Boolean, help.IsShowFooter);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool UpdateHelp(HelpInfo help)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Helps SET CategoryId = @CategoryId, AddedDate = @AddedDate, Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords,  Description = @Description, Content = @Content, IsShowFooter = @IsShowFooter WHERE HelpId = @HelpId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, help.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, help.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, help.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, help.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, help.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, help.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, help.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsShowFooter", DbType.Boolean, help.IsShowFooter);
			this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, help.HelpId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool DeleteHelp(int helpId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Helps WHERE HelpId = @HelpId");
			this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public HelpInfo GetHelp(int helpId)
		{
			HelpInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Helps WHERE HelpId=@HelpId");
			this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateHelp(dataReader);
				}
			}
			return result;
		}
		public DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(helpQuery.Keywords));
			if (helpQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", helpQuery.CategoryId.Value);
			}
			if (helpQuery.StartArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >= '{0}'", helpQuery.StartArticleTime.Value);
			}
			if (helpQuery.EndArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <= '{0}'", helpQuery.EndArticleTime.Value);
			}
			return DataHelper.PagingByTopnotin(helpQuery.PageIndex, helpQuery.PageSize, helpQuery.SortBy, helpQuery.SortOrder, helpQuery.IsCount, "vw_Ecshop_Helps", "HelpId", stringBuilder.ToString(), "*");
		}
		public DataSet GetHelps()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_HelpCategories WHERE IsShowFooter = 1 ORDER BY DisplaySequence SELECT * FROM Ecshop_Helps WHERE IsShowFooter = 1  AND CategoryId IN (SELECT CategoryId FROM Ecshop_HelpCategories WHERE IsShowFooter = 1)");
			DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			DataColumn parentColumn = dataSet.Tables[0].Columns["CateGoryId"];
			DataColumn childColumn = dataSet.Tables[1].Columns["CateGoryId"];
			DataRelation relation = new DataRelation("CateGory", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}
        public DataSet GetAllHelps()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_HelpCategories ORDER BY DisplaySequence SELECT * FROM Ecshop_Helps");
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = dataSet.Tables[0].Columns["CateGoryId"];
            DataColumn childColumn = dataSet.Tables[1].Columns["CateGoryId"];
            DataRelation relation = new DataRelation("CateGory", parentColumn, childColumn);
            dataSet.Relations.Add(relation);
            return dataSet;
        }

        public DataTable GetHelpCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_HelpCategories");
            DataTable dt = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            return dt;
        }

        public DataTable GetHelpByCateGoryId(int CategoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Helps where CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, CategoryId);
            DataTable dt = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            return dt;
        }
        public DataTable GetFooterHelps()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT A1.CategoryId,A1.HelpId,A1.Title,A2.Name,A2.IconUrl from Ecshop_Helps as A1 LEFT JOIN Ecshop_HelpCategories AS A2 ON A1.CategoryId=A2.CategoryId  WHERE A2.IsShowFooter = 1 ORDER BY DisplaySequence ");
           DataSet dsHelps= this.database.ExecuteDataSet(sqlStringCommand);
           if (dsHelps != null&&dsHelps.Tables.Count>0)
           {
               return dsHelps.Tables[0];
           }
           return null;
        }
		public HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type)
		{
			string query = string.Empty;
			if (type == "Next")
			{
				query = "SELECT TOP 1 * FROM Ecshop_Helps WHERE HelpId <@HelpId AND CategoryId=@CategoryId ORDER BY HelpId DESC";
			}
			else
			{
				query = "SELECT TOP 1 * FROM Ecshop_Helps WHERE HelpId >@HelpId AND CategoryId=@CategoryId ORDER BY HelpId ASC";
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			HelpInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateHelp(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}

        public DataTable SearchHelps(Pagination pagination,string  searchcontent, out int totalHelps)
        {
            DataTable result = null;
            StringBuilder sbSearchcontent = new StringBuilder();
            if(!String.IsNullOrEmpty(searchcontent))
            {
                searchcontent = DataHelper.CleanSearchString(searchcontent);

                sbSearchcontent.AppendFormat(" AND (A.Title like '%{0}%' ", searchcontent);

                sbSearchcontent.AppendFormat(" OR A.Content like '%{0}%' ", searchcontent);

                sbSearchcontent.AppendFormat(" OR B.Name like '%{0}%') ", searchcontent);
            }
            string text = string.Format("SELECT COUNT(*) FROM Ecshop_Helps A LEFT JOIN Ecshop_HelpCategories B on A.CategoryId=B.CategoryId WHERE 1=1 ", new object[0]);
            if (!String.IsNullOrEmpty(searchcontent))
            {
                text += string.Format("{0}", sbSearchcontent.ToString());
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
            totalHelps = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string text2 = string.Empty;
            if (pagination.PageIndex == 1)
            {
                text2 = "SELECT TOP 10 A.*,B.Name as Name FROM Ecshop_Helps A LEFT JOIN Ecshop_HelpCategories B on A.CategoryId=B.CategoryId WHERE 1=1 ";
            }
            else
            {
                text2 = string.Format("SELECT TOP {0} A.*,B.Name as Name FROM Ecshop_Helps A LEFT JOIN Ecshop_HelpCategories B on A.CategoryId=B.CategoryId WHERE 1=1 and A.HelpId NOT IN (SELECT TOP {1} A.HelpId FROM Ecshop_Helps A LEFT JOIN Ecshop_HelpCategories B on A.CategoryId=B.CategoryId) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
            }
            if (!String.IsNullOrEmpty(searchcontent))
            {
                text2 += string.Format("{0}", sbSearchcontent.ToString());
            }
            //text2 += " ORDER BY ActivityId DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(text2);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
	}
}
