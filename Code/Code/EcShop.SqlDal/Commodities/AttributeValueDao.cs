using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Commodities
{
	public class AttributeValueDao
	{
		private Database database;
		public AttributeValueDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public AttributeValueInfo GetAttributeValueInfo(int valueId)
		{
			AttributeValueInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_AttributeValues WHERE ValueId=@ValueId");
			this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<AttributeValueInfo>(dataReader);
			}
			return result;
		}
		public int GetSpecificationValueId(int attributeId, string ValueStr)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ValueId FROM Ecshop_AttributeValues WHERE AttributeId = @AttributeId AND ValueStr = @ValueStr");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, ValueStr);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			return result;
		}
		public int AddAttributeValue(AttributeValueInfo attributeValue)
		{
			int result = 0;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_AttributeValues; INSERT INTO Ecshop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeValue.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj.ToString());
			}
			return result;
		}
		public bool UpdateAttributeValue(AttributeValueInfo attributeValue)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_AttributeValues SET  ValueStr=@ValueStr, ImageUrl=@ImageUrl WHERE ValueId=@valueId");
			this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
			this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValue.ValueId);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteAttributeValue(int attributeValueId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_AttributeValues WHERE ValueId = @ValueId AND not exists (SELECT * FROM Ecshop_SKUItems WHERE ValueId = @ValueId) DELETE FROM Ecshop_ProductAttributes WHERE ValueId = @ValueId");
			this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValueId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool ClearAttributeValue(int attributeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Ecshop_SKUItems WHERE AttributeId = @AttributeId)");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Ecshop_AttributeValues", "ValueId", "DisplaySequence", attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}
	}
}
