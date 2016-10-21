using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Commodities
{
	public class AttributeDao
	{
		private Database database;
		public AttributeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public AttributeInfo GetAttribute(int attributeId)
		{
			AttributeInfo attributeInfo = new AttributeInfo();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_AttributeValues WHERE AttributeId = @AttributeId ORDER BY DisplaySequence DESC; SELECT * FROM Ecshop_Attributes WHERE AttributeId = @AttributeId;");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				IList<AttributeValueInfo> attributeValues = ReaderConvert.ReaderToList<AttributeValueInfo>(dataReader);
				dataReader.NextResult();
				attributeInfo = ReaderConvert.ReaderToModel<AttributeInfo>(dataReader);
				attributeInfo.AttributeValues = attributeValues;
			}
			return attributeInfo;
		}
		public bool AddAttribute(AttributeInfo attribute)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_Attributes; INSERT INTO Ecshop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
			this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int)attribute.UsageMode);
			this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (attribute.AttributeValues.Count != 0 && obj != null)
			{
				int num = Convert.ToInt32(obj);
				foreach (AttributeValueInfo current in attribute.AttributeValues)
				{
					DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_AttributeValues; INSERT INTO Ecshop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
					this.database.AddInParameter(sqlStringCommand2, "AttributeId", DbType.Int32, num);
					this.database.AddInParameter(sqlStringCommand2, "ValueStr", DbType.String, current.ValueStr);
					this.database.AddInParameter(sqlStringCommand2, "ImageUrl", DbType.String, current.ImageUrl);
					this.database.ExecuteNonQuery(sqlStringCommand2);
				}
			}
			return obj != null;
		}
		public int GetSpecificationId(int typeId, string specificationName)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT AttributeId FROM Ecshop_Attributes WHERE UsageMode = 2 AND TypeId = @TypeId AND AttributeName = @AttributeName");
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, specificationName);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = (int)obj;
			}
			return result;
		}
		public int AddAttributeName(AttributeInfo attribute)
		{
			int result = 0;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_Attributes; INSERT INTO Ecshop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
			this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int)attribute.UsageMode);
			this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			return result;
		}
		public bool UpdateAttribute(AttributeInfo attribute)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Attributes SET AttributeName = @AttributeName, TypeId = @TypeId, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId; DELETE FROM Ecshop_AttributeValues WHERE AttributeId = @AttributeId;");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
			this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
			bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
			if (flag && attribute.AttributeValues.Count != 0)
			{
				foreach (AttributeValueInfo current in attribute.AttributeValues)
				{
					DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_AttributeValues; INSERT INTO Ecshop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
					this.database.AddInParameter(sqlStringCommand2, "AttributeId", DbType.Int32, attribute.AttributeId);
					this.database.AddInParameter(sqlStringCommand2, "ValueStr", DbType.String, current.ValueStr);
					this.database.AddInParameter(sqlStringCommand2, "ImageUrl", DbType.String, current.ImageUrl);
					this.database.ExecuteNonQuery(sqlStringCommand2);
				}
			}
			return flag;
		}
		public bool UpdateAttributeName(AttributeInfo attribute)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Attributes SET AttributeName = @AttributeName, UsageMode = @UsageMode WHERE AttributeId = @AttributeId;");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int)attribute.UsageMode);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteAttribute(int attributeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Attributes WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Ecshop_SKUItems WHERE AttributeId = @AttributeId)");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Ecshop_Attributes", "AttributeId", "DisplaySequence", attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
		}
		public IList<AttributeInfo> GetAttributes(int typeId)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Attributes WHERE TypeId = @TypeId ORDER BY DisplaySequence DESC SELECT * FROM Ecshop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Ecshop_Attributes WHERE TypeId = @TypeId) ORDER BY DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			using (DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (DataRow dataRow in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)dataRow["AttributeId"];
					attributeInfo.AttributeName = (string)dataRow["AttributeName"];
					attributeInfo.DisplaySequence = (int)dataRow["DisplaySequence"];
					attributeInfo.TypeId = (int)dataRow["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)((int)dataRow["UsageMode"]);
					attributeInfo.UseAttributeImage = (bool)dataRow["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString());
						DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow2 = array2[i];
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}
		public IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			string text;
			if (attributeUseageMode == AttributeUseageMode.Choose)
			{
				text = "UsageMode = 2";
			}
			else
			{
				text = "UsageMode <> 2";
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT * FROM Ecshop_Attributes WHERE TypeId = @TypeId AND ",
				text,
				" ORDER BY DisplaySequence Desc SELECT * FROM Ecshop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Ecshop_Attributes WHERE TypeId = @TypeId AND  ",
				text,
				" ) ORDER BY DisplaySequence Desc"
			}));
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			using (DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (DataRow dataRow in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)dataRow["AttributeId"];
					attributeInfo.AttributeName = (string)dataRow["AttributeName"];
					attributeInfo.DisplaySequence = (int)dataRow["DisplaySequence"];
					attributeInfo.TypeId = (int)dataRow["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)((int)dataRow["UsageMode"]);
					attributeInfo.UseAttributeImage = (bool)dataRow["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString());
						DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow2 = array2[i];
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							if (dataRow2["ImageUrl"] != DBNull.Value)
							{
								attributeValueInfo.ImageUrl = (string)dataRow2["ImageUrl"];
							}
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}
		public IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			string text;
			if (attributeUseageMode == AttributeUseageMode.Choose)
			{
				text = "UsageMode = 2";
			}
			else
			{
				text = "UsageMode <> 2";
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT * FROM Ecshop_Attributes WHERE ",
				text,
				" ORDER BY DisplaySequence Desc SELECT * FROM Ecshop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Ecshop_Attributes Where  ",
				text,
				" ) ORDER BY DisplaySequence Desc"
			}));
			using (DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (DataRow dataRow in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)dataRow["AttributeId"];
					attributeInfo.AttributeName = (string)dataRow["AttributeName"];
					attributeInfo.DisplaySequence = (int)dataRow["DisplaySequence"];
					attributeInfo.TypeId = (int)dataRow["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)((int)dataRow["UsageMode"]);
					attributeInfo.UseAttributeImage = (bool)dataRow["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString());
						DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow2 = array2[i];
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							if (dataRow2["ImageUrl"] != DBNull.Value)
							{
								attributeValueInfo.ImageUrl = (string)dataRow2["ImageUrl"];
							}
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}
		public IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Ecshop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM Ecshop_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2) AND ValueId IN (SELECT ValueId FROM Ecshop_ProductAttributes) ORDER BY DisplaySequence DESC;" + string.Format(" SELECT TOP {0} * FROM Ecshop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM Ecshop_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2", maxNum) + " AND AttributeId IN (SELECT AttributeId FROM Ecshop_ProductAttributes) ORDER BY DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				IList<AttributeValueInfo> list2 = new List<AttributeValueInfo>();
				while (dataReader.Read())
				{
					AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
					attributeValueInfo.ValueId = (int)dataReader["ValueId"];
					attributeValueInfo.AttributeId = (int)dataReader["AttributeId"];
					attributeValueInfo.DisplaySequence = (int)dataReader["DisplaySequence"];
					attributeValueInfo.ValueStr = (string)dataReader["ValueStr"];
					if (dataReader["ImageUrl"] != DBNull.Value)
					{
						attributeValueInfo.ImageUrl = (string)dataReader["ImageUrl"];
					}
					list2.Add(attributeValueInfo);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						AttributeInfo attributeInfo = new AttributeInfo();
						attributeInfo.AttributeId = (int)dataReader["AttributeId"];
						attributeInfo.AttributeName = (string)dataReader["AttributeName"];
						attributeInfo.DisplaySequence = (int)dataReader["DisplaySequence"];
						attributeInfo.TypeId = (int)dataReader["TypeId"];
						attributeInfo.UsageMode = (AttributeUseageMode)((int)dataReader["UsageMode"]);
						attributeInfo.UseAttributeImage = (bool)dataReader["UseAttributeImage"];
						foreach (AttributeValueInfo current in list2)
						{
							if (attributeInfo.AttributeId == current.AttributeId)
							{
								attributeInfo.AttributeValues.Add(current);
							}
						}
						list.Add(attributeInfo);
					}
				}
			}
			return list;
		}
	}
}
