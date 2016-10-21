using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
namespace EcShop.ControlPanel.Commodities
{
	public sealed class ProductTypeHelper
	{
		private ProductTypeHelper()
		{
		}
		public static DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return new ProductTypeDao().GetProductTypes(query);
		}
		public static IList<ProductTypeInfo> GetProductTypes()
		{
			return new ProductTypeDao().GetProductTypes();
		}
		public static ProductTypeInfo GetProductType(int typeId)
		{
			return new ProductTypeDao().GetProductType(typeId);
		}
		public static System.Data.DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			return new ProductTypeDao().GetBrandCategoriesByTypeId(typeId);
		}
		public static int GetTypeId(string typeName)
		{
			ProductTypeDao productTypeDao = new ProductTypeDao();
			int typeId = productTypeDao.GetTypeId(typeName);
			int result;
			if (typeId > 0)
			{
				result = typeId;
			}
			else
			{
				result = productTypeDao.AddProductType(new ProductTypeInfo
				{
					TypeName = typeName
				});
			}
			return result;
		}
		public static int AddProductType(ProductTypeInfo productType)
		{
			int result;
			if (productType == null)
			{
				result = 0;
			}
			else
			{
				ProductTypeDao productTypeDao = new ProductTypeDao();
				Globals.EntityCoding(productType, true);
				int num = productTypeDao.AddProductType(productType);
				if (num > 0)
				{
					if (productType.Brands.Count > 0)
					{
						productTypeDao.AddProductTypeBrands(num, productType.Brands);
					}
					EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "创建了一个新的商品类型:”{0}”", new object[]
					{
						productType.TypeName
					}));
				}
				result = num;
			}
			return result;
		}
		public static bool UpdateProductType(ProductTypeInfo productType)
		{
			bool result;
			if (productType == null)
			{
				result = false;
			}
			else
			{
				ProductTypeDao productTypeDao = new ProductTypeDao();
				Globals.EntityCoding(productType, true);
				bool flag = productTypeDao.UpdateProductType(productType);
				if (flag)
				{
					if (productTypeDao.DeleteProductTypeBrands(productType.TypeId))
					{
						productTypeDao.AddProductTypeBrands(productType.TypeId, productType.Brands);
					}
					EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的商品类型", new object[]
					{
						productType.TypeId
					}));
				}
				result = flag;
			}
			return result;
		}
		public static bool DeleteProductType(int typeId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
			bool flag = new ProductTypeDao().DeleteProducType(typeId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的商品类型", new object[]
				{
					typeId
				}));
			}
			return flag;
		}
		public static AttributeInfo GetAttribute(int attributeId)
		{
			return new AttributeDao().GetAttribute(attributeId);
		}
		public static bool AddAttribute(AttributeInfo attribute)
		{
			return new AttributeDao().AddAttribute(attribute);
		}
		public static int GetSpecificationId(int typeId, string specificationName)
		{
			AttributeDao attributeDao = new AttributeDao();
			int specificationId = attributeDao.GetSpecificationId(typeId, specificationName);
			int result;
			if (specificationId > 0)
			{
				result = specificationId;
			}
			else
			{
				result = attributeDao.AddAttributeName(new AttributeInfo
				{
					TypeId = typeId,
					UsageMode = AttributeUseageMode.Choose,
					UseAttributeImage = false,
					AttributeName = specificationName
				});
			}
			return result;
		}
		public static bool AddAttributeName(AttributeInfo attribute)
		{
			return new AttributeDao().AddAttributeName(attribute) > 0;
		}
		public static bool UpdateAttribute(AttributeInfo attribute)
		{
			return new AttributeDao().UpdateAttribute(attribute);
		}
		public static bool UpdateAttributeName(AttributeInfo attribute)
		{
			return new AttributeDao().UpdateAttributeName(attribute);
		}
		public static bool DeleteAttribute(int attriubteId)
		{
			return new AttributeDao().DeleteAttribute(attriubteId);
		}
		public static void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
		{
			new AttributeDao().SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
		}
		public static IList<AttributeInfo> GetAttributes(int typeId)
		{
			return new AttributeDao().GetAttributes(typeId);
		}
		public static IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
		{
			return new AttributeDao().GetAttributes(attributeUseageMode);
		}
		public static IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
		{
			return new AttributeDao().GetAttributes(typeId, attributeUseageMode);
		}
		public static int AddAttributeValue(AttributeValueInfo attributeValue)
		{
			return new AttributeValueDao().AddAttributeValue(attributeValue);
		}
		public static int GetSpecificationValueId(int attributeId, string valueStr)
		{
			AttributeValueDao attributeValueDao = new AttributeValueDao();
			int specificationValueId = attributeValueDao.GetSpecificationValueId(attributeId, valueStr);
			int result;
			if (specificationValueId > 0)
			{
				result = specificationValueId;
			}
			else
			{
				result = attributeValueDao.AddAttributeValue(new AttributeValueInfo
				{
					AttributeId = attributeId,
					ValueStr = valueStr
				});
			}
			return result;
		}
		public static bool ClearAttributeValue(int attributeId)
		{
			return new AttributeValueDao().ClearAttributeValue(attributeId);
		}
		public static bool DeleteAttributeValue(int attributeValueId)
		{
			return new AttributeValueDao().DeleteAttributeValue(attributeValueId);
		}
		public static bool UpdateAttributeValue(AttributeValueInfo attributeValue)
		{
			return new AttributeValueDao().UpdateAttributeValue(attributeValue);
		}
		public static void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			new AttributeValueDao().SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}
		public static AttributeValueInfo GetAttributeValueInfo(int valueId)
		{
			return new AttributeValueDao().GetAttributeValueInfo(valueId);
		}
		public static string UploadSKUImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/sku/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
	}
}
