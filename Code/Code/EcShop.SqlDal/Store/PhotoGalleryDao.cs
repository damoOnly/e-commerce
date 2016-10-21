using EcShop.Core;
using EcShop.Core.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Store
{
	public class PhotoGalleryDao
	{
		private Database database;
		public PhotoGalleryDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public int MovePhotoType(List<int> pList, int pTypeId)
		{
			int result;
			if (pList.Count <= 0)
			{
				result = 0;
			}
			else
			{
				string text = string.Empty;
				foreach (int current in pList)
				{
					text = text + current + ",";
				}
				text = text.Remove(text.Length - 1);
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_PhotoGallery SET CategoryId = @CategoryId WHERE PhotoId IN ({0})", text));
				this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, pTypeId);
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public bool AddPhotoCategory(string name)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence INT; SELECT @DisplaySequence = ISNULL(MAX(DisplaySequence), 0) + 1 FROM Ecshop_PhotoCategories; INSERT Ecshop_PhotoCategories (CategoryName, DisplaySequence) VALUES (@CategoryName, @DisplaySequence)");
			this.database.AddInParameter(sqlStringCommand, "CategoryName", DbType.String, name);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
		{
			int result;
			if (photoCategorys.Count <= 0)
			{
				result = 0;
			}
			else
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				StringBuilder stringBuilder = new StringBuilder();
				foreach (int current in photoCategorys.Keys)
				{
					string text = current.ToString();
					stringBuilder.AppendFormat("UPDATE Ecshop_PhotoCategories SET CategoryName = @CategoryName{0} WHERE CategoryId = {0}", text);
					this.database.AddInParameter(sqlStringCommand, "CategoryName" + text, DbType.String, photoCategorys[current]);
				}
				sqlStringCommand.CommandText = stringBuilder.ToString();
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public bool DeletePhotoCategory(int categoryId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_PhotoCategories WHERE CategoryId = @CategoryId; UPDATE Ecshop_PhotoGallery SET CategoryId = 0 WHERE CategoryId = @CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DataTable GetPhotoCategories()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT COUNT(PhotoId) FROM Ecshop_PhotoGallery WHERE CategoryId = pc.CategoryId) AS PhotoCounts FROM Ecshop_PhotoCategories pc ORDER BY DisplaySequence DESC");
			DataTable result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public void SwapSequence(int categoryId1, int categoryId2)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence1 INT , @DisplaySequence2 INT;  SELECT @DisplaySequence1 = DisplaySequence FROM Ecshop_PhotoCategories WHERE CategoryId = @CategoryId1; SELECT @DisplaySequence2 = DisplaySequence FROM Ecshop_PhotoCategories WHERE CategoryId = @CategoryId2; UPDATE Ecshop_PhotoCategories SET DisplaySequence = @DisplaySequence1 WHERE CategoryId = @CategoryId2; UPDATE Ecshop_PhotoCategories SET DisplaySequence = @DisplaySequence2 WHERE CategoryId = @CategoryId1");
			this.database.AddInParameter(sqlStringCommand, "CategoryId1", DbType.Int32, categoryId1);
			this.database.AddInParameter(sqlStringCommand, "CategoryId2", DbType.Int32, categoryId2);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public DbQueryResult GetPhotoList(string keyword, int? categoryId, Pagination page)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(keyword))
			{
				text += string.Format("PhotoName LIKE '%{0}%'", DataHelper.CleanSearchString(keyword));
			}
			if (categoryId.HasValue)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " AND";
				}
				text += string.Format(" CategoryId = {0}", categoryId.Value);
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Ecshop_PhotoGallery", "ProductId", text, "*");
		}
		public bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_PhotoGallery(CategoryId, PhotoName, PhotoPath, FileSize, UploadTime, LastUpdateTime) VALUES (@CategoryId, @PhotoName, @PhotoPath, @FileSize, @UploadTime, @LastUpdateTime)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "PhotoName", DbType.String, photoName);
			this.database.AddInParameter(sqlStringCommand, "PhotoPath", DbType.String, photoPath);
			this.database.AddInParameter(sqlStringCommand, "FileSize", DbType.Int32, fileSize);
			this.database.AddInParameter(sqlStringCommand, "UploadTime", DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "LastUpdateTime", DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeletePhoto(int photoId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_PhotoGallery WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public void RenamePhoto(int photoId, string newName)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_PhotoGallery SET PhotoName = @PhotoName WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
			this.database.AddInParameter(sqlStringCommand, "PhotoName", DbType.String, newName);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public void ReplacePhoto(int photoId, int fileSize)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_PhotoGallery SET FileSize = @FileSize, LastUpdateTime = @LastUpdateTime WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
			this.database.AddInParameter(sqlStringCommand, "FileSize", DbType.Int32, fileSize);
			this.database.AddInParameter(sqlStringCommand, "LastUpdateTime", DbType.DateTime, DateTime.Now);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public string GetPhotoPath(int photoId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT PhotoPath FROM Ecshop_PhotoGallery WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
			return this.database.ExecuteScalar(sqlStringCommand).ToString();
		}
		public int GetPhotoCount()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Ecshop_PhotoGallery");
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
		public int GetDefaultPhotoCount()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Ecshop_PhotoGallery where CategoryId=0");
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
	}
}
