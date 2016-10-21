using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.UI.Common.Controls
{
	public abstract class ControlProvider
	{
		private static readonly ControlProvider _defaultInstance;
		static ControlProvider()
		{
			ControlProvider._defaultInstance = (DataProviders.CreateInstance("EcShop.UI.Common.Data.SqlCommonDataProvider, EcShop.UI.Common.Data") as ControlProvider);
		}
		public static ControlProvider Instance()
		{
			return ControlProvider._defaultInstance;
		}
		public abstract IList<ProductTypeInfo> GetProductTypes();
		public abstract DataTable GetBrandCategories();
		public abstract DataTable GetBrandCategoriesByTypeId(int typeId);
		public abstract IList<ShippingModeInfo> GetShippingModes();
		public abstract void GetMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum);
		public abstract DataTable GetTags();
		public abstract DataTable GetSkuContentBySku(string skuId);
		public abstract BFDOrder GetBFDOrder(string orderid);
	}
}
