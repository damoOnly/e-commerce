using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.UI.Common.Data
{
	public class SqlCommonDataProvider : ControlProvider
	{
		private Database database;
		public SqlCommonDataProvider()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override IList<ProductTypeInfo> GetProductTypes()
		{
			IList<ProductTypeInfo> list = new List<ProductTypeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ProductTypes ORDER BY TypeId DESC");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateType(dataReader));
				}
			}
			return list;
		}
		public override System.Data.DataTable GetBrandCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_BrandCategories ORDER BY DisplaySequence DESC");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Ecshop_BrandCategories B INNER JOIN Ecshop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE ProductTypeId=@ProductTypeId ORDER BY DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override IList<ShippingModeInfo> GetShippingModes()
		{
			IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ShippingTypes st INNER JOIN Ecshop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShippingMode(dataReader));
				}
			}
			return list;
		}
		public override void GetMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name FROM aspnet_MemberGrades WHERE GradeId = @GradeId;SELECT COUNT(*) AS NoReadMessageNum FROM Ecshop_MemberMessageBox WHERE Accepter = @Accepter AND IsRead=0");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "Accepter", System.Data.DbType.String, userName);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					gradeName = (string)dataReader["Name"];
				}
				else
				{
					gradeName = string.Empty;
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					messageNum = (int)dataReader["NoReadMessageNum"];
				}
				else
				{
					messageNum = 0;
				}
			}
		}
		public override System.Data.DataTable GetSkuContentBySku(string skuId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT AttributeName, ValueStr");
			stringBuilder.Append(" FROM Ecshop_SKUs s join Ecshop_SKUItems si on s.SkuId = si.SkuId");
			stringBuilder.Append(" join Ecshop_Attributes a on si.AttributeId = a.AttributeId join Ecshop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetTags()
		{
			System.Data.DataTable result = new System.Data.DataTable();
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Ecshop_Tags");
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		public override BFDOrder GetBFDOrder(string orderid)
		{
			string text = "[";
			BFDOrder bFDOrder = new BFDOrder();
			StringBuilder stringBuilder = new StringBuilder("select ModeName,paymenttype,ordertotal from [Ecshop_Orders] where orderid=@orderid;");
			stringBuilder.Append("select productid,quantity,itemlistprice from Ecshop_OrderItems where orderid=@orderid");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "orderid", System.Data.DbType.String, orderid);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					bFDOrder.ModeName = (string)dataReader["ModeName"];
					bFDOrder.paymenttype = (string)dataReader["paymenttype"];
					bFDOrder.ordertotal = string.Format("{0:f2}", dataReader["ordertotal"]);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"[\"",
							Convert.ToString(dataReader["productid"]),
							"\",",
							Convert.ToString(dataReader["quantity"]),
							",",
							string.Format("{0:f2}", dataReader["itemlistprice"]),
							"],"
						});
					}
				}
				bFDOrder.orderlist = text.Substring(0, text.Length - 1) + "]";
			}
			return bFDOrder;
		}
	}
}
