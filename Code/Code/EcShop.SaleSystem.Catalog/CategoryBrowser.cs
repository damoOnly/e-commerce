using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.Caching;
namespace EcShop.SaleSystem.Catalog
{
	public static class CategoryBrowser
	{
		private const string MainCategoriesCachekey = "DataCache-Categories";
		public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId,int isDisable, int maxNum = 1000)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
            DataTable categories = CategoryBrowser.GetCategories(isDisable);
			DataRow[] array = categories.Select("ParentCategoryId = " + parentCategoryId);
			int num = 0;
			while (num < maxNum && num < array.Length)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[num]));
				num++;
			}
			return list;
		}
        public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId, int maxNum = 1000)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataTable categories = CategoryBrowser.GetCategories();
            DataRow[] array = categories.Select("ParentCategoryId = " + parentCategoryId);
            int num = 0;
            while (num < maxNum && num < array.Length)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(array[num]));
                num++;
            }
            return list;
        }
		public static IList<CategoryInfo> GetMaxMainCategories(int maxNum = 1000)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			DataTable categories = CategoryBrowser.GetCategories();
			DataRow[] array = categories.Select("Depth = 1");
			int num = 0;
			while (num < maxNum && num < array.Length)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[num]));
				num++;
			}
			return list;
		}
		public static IList<CategoryInfo> GetSequenceCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			IList<CategoryInfo> mainCategories = CategoryBrowser.GetMainCategories();
			foreach (CategoryInfo current in mainCategories)
			{
				list.Add(current);
				CategoryBrowser.LoadSubCategorys(current.CategoryId, list);
			}
			return list;
		}
		public static IList<CategoryInfo> GetMainCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			DataTable categories = CategoryBrowser.GetCategories(0);
			DataRow[] array = categories.Select("Depth = 1");
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}

        public static IList<CategoryInfo> GetCategoryiesBySupplierId(int supplierId)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataTable dtcategories = CategoryBrowser.GetDataCacheCategoriesBySupplierId(supplierId);
            DataRow[] array = dtcategories.Select(" 1 = 1");
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
            }
            return list;
        }
        
        /// <summary>
        /// 根据一级类别ID获取类目的所有信息
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public static List<CategoryInfo> GetTwoCategoryByCategoryId(int CategoryId)
        {
            List<CategoryInfo> items = new List<CategoryInfo>();
            DataTable subCategories = GetSecondCategoryByCategoryId(CategoryId);
            if (subCategories != null && subCategories.Rows.Count > 0)
            {
                CategoryInfo item = new CategoryInfo();
                item.CategoryId = subCategories.Rows[0]["ParentCategoryId"] == null ? 0 : Convert.ToInt32(subCategories.Rows[0]["ParentCategoryId"].ToString());
                item.Name = subCategories.Rows[0]["ParentCategoryName"].ToString();
                List<CategoryPartInfo> nextitems = new List<CategoryPartInfo>();
                foreach (DataRow row in subCategories.Rows)
                {
                    CategoryPartInfo nextitem = new CategoryPartInfo();
                    nextitem.CategoryId = row["CategoryId"] == null ? 0 : Convert.ToInt32(row["CategoryId"].ToString());
                    nextitem.Name = row["Name"].ToString();
                    nextitems.Add(nextitem);
                }
                item.NextCategoryPartInfo = nextitems;
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 根据类别ID获取类别的信息
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public static List<CategoryInfo> GetThreeCategoryByCategoryId(int CategoryId)
        {
            List<CategoryInfo> items = new List<CategoryInfo>();
            DataTable subCategories = GetThirdCategoryByCategoryId(CategoryId);
            if (subCategories != null && subCategories.Rows.Count > 0)
            {
                CategoryInfo item = new CategoryInfo();
                item.CategoryId = subCategories.Rows[0]["ParentCategoryId"] == null ? 0 : Convert.ToInt32(subCategories.Rows[0]["ParentCategoryId"].ToString());
                item.Name = subCategories.Rows[0]["ParentCategoryName"].ToString();
                List<CategoryPartInfo> nextitems = new List<CategoryPartInfo>();
                foreach (DataRow row in subCategories.Rows)
                {
                    CategoryPartInfo nextitem = new CategoryPartInfo();
                    nextitem.CategoryId = row["CategoryId"] == null ? 0 : Convert.ToInt32(row["CategoryId"].ToString());
                    nextitem.Name = row["Name"].ToString();
                    nextitems.Add(nextitem);
                }
                item.NextCategoryPartInfo = nextitems;
                items.Add(item);
            }

            return items;
        }

		private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
		{
			IList<CategoryInfo> subCategories = CategoryBrowser.GetSubCategories(parentCategoryId);
			if (subCategories != null && subCategories.Count > 0)
			{
				foreach (CategoryInfo current in subCategories)
				{
					categories.Add(current);
					CategoryBrowser.LoadSubCategorys(current.CategoryId, categories);
				}
			}
		}
		public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
			DataTable categories = CategoryBrowser.GetCategories(0);
			DataRow[] array = categories.Select(filterExpression);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}
		public static DataTable GetCategories()
		{
			DataTable dataTable = new DataTable();
			dataTable = (HiCache.Get("DataCache-Categories") as DataTable);
			if (dataTable == null)
			{
				dataTable = new CategoryDao().GetCategories();
				HiCache.Insert("DataCache-Categories", dataTable, 360, CacheItemPriority.Normal);
			}
			return dataTable;
		}

        public static DataTable GetDataCacheCategoriesBySupplierId(int supplerId)
        {
            DataTable dataTable = new DataTable();
            //dataTable = (HiCache.Get("DataCache-SupplierCategories") as DataTable);
            //if (dataTable == null)
            //{
            //    dataTable = new CategoryDao().GetCategoryiesBySupplierId(supplerId);
            //    HiCache.Insert("DataCache-SupplierCategories", dataTable, 360, CacheItemPriority.Normal);
            //}
            dataTable = new CategoryDao().GetCategoryiesBySupplierId(supplerId);
            return dataTable;
        }

        public static DataTable GetCategories(int isDisable)
		{
			DataTable dataTable = new DataTable();
            dataTable = (HiCache.Get("DataCache-DisableCategories") as DataTable);
			if (dataTable == null)
			{
                dataTable = new CategoryDao().GetCategories(isDisable);
                HiCache.Insert("DataCache-DisableCategories", dataTable, 360, CacheItemPriority.Normal);
			}
			return dataTable;
		}
        
        public static DataTable GetCategoriesByThisId(int categoryId,int c2,int c3,int c4,int c5,int c6,int c7)
        {
            DataTable dataTable = new DataTable();
            dataTable = (HiCache.Get("DataCache-Categories-thisC") as DataTable);
            if (dataTable == null||dataTable.Rows.Count==0)
            {
                dataTable = new CategoryDao().GetCategoriesByTopId(categoryId,c2,c3,c4,c5,c6,c7);                
                HiCache.Insert("DataCache-Categories-thisC", dataTable, 86400, CacheItemPriority.Normal);
            }
            return dataTable;
        }
		public static CategoryInfo GetCategory(int categoryId)
		{
			return new CategoryDao().GetCategory(categoryId);
		}

        public static Dictionary<string, string> GetSecondCategoriesById(int userid)
		{
            return new CategoryDao().GetSecondCategoriesById(userid);
		}
		public static DataSet GetThreeLayerCategories()
		{
			return new CategoryDao().GetThreeLayerCategories();
		}
          /// <summary>
        /// 检查商品是否是限制商品
        /// </summary>
        /// <param name="CategoryIdpt">二级分类ＩＤ</param>
        /// <param name="CategoryId">商品类别ID</param>
        /// <returns></returns>
        public static bool CheckPructCag(string CategoryIdpt, string CategoryId)
        {
            return new CategoryDao().CheckPructCag(CategoryIdpt, CategoryId);
        }
		public static DataTable GetBrandCategories(int categoryId, int maxNum = 1000)
		{
			return new BrandCategoryDao().GetBrandCategories(categoryId, maxNum);
		}

        public static DataTable GetBindList(int categoryId)
        {
            return new BrandCategoryDao().GetBindList(categoryId);
        }

		public static BrandCategoryInfo GetBrandCategory(int brandId)
		{
			return new BrandCategoryDao().GetBrandCategory(brandId);
		}
		public static IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum = 1000)
		{
			return new AttributeDao().GetAttributeInfoByCategoryId(categoryId, maxNum);
		}
        public static IList<CategoryInfo> GetSiblingsCategories(int categoryId, int maxNum = 1000)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataTable categories = CategoryBrowser.GetCategories();
            DataRow[] array = categories.Select("CategoryId = " + categoryId);
            int parentId = 0;
            if (array.Length > 0)
            {
                int.TryParse(array[0]["ParentCategoryId"].ToString(),out parentId);
                if (parentId != 0)
                {
                    DataRow[] array1 = categories.Select("ParentCategoryId = " + parentId);
                    int num = 0;
                    while (num < maxNum && num < array1.Length)
                    {
                        if (array1[num]["CategoryId"].ToString() == categoryId.ToString())
                        {
                            num++;
                            continue;
                        }
                        list.Add(DataMapper.ConvertDataRowToProductCategory(array1[num]));
                        num++;
                    }
                   
                }
            }
            return list;
        }
        /// <summary>
        /// 根据首层分类ID获取最底层即第三次分类数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="maxNum"></param>
        /// <returns></returns>
        public static IList<CategoryInfo> GetThirdCategoriesById(int categoryId, int maxNum = 1000)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataTable categories = CategoryBrowser.GetCategories();
            DataRow[] array = categories.Select("CategoryId = " + categoryId);
            int parentId = 0;
            if (array.Length > 0)
            {
                int.TryParse(array[0]["ParentCategoryId"].ToString(), out parentId);
                if (parentId != 0)
                {
                    DataRow[] array1 = categories.Select("ParentCategoryId = " + parentId);
                    int num = 0;
                    while (num < maxNum && num < array1.Length)
                    {
                        if (array1[num]["CategoryId"].ToString() == categoryId.ToString())
                        {
                            num++;
                            continue;
                        }
                        list.Add(DataMapper.ConvertDataRowToProductCategory(array1[num]));
                        num++;
                    }

                }
            }
            return list;
        }
        /// <summary>
        /// 根据主类型ID获取第三层类型数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static DataTable GetThirdCategoryiesById(int categoryId)
        {
            DataTable dataTable = new CategoryDao().GetThirdCategoryiesById(categoryId);
            return dataTable;
        }


        /// <summary>
        /// 获取下二层的数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static DataTable GetSecondCategoryByCategoryId(int categoryId)
        {
            DataTable dataTable = new CategoryDao().GetSecondCategoryByCategoryId(categoryId);
            return dataTable;
        }

        /// <summary>
        /// 获取下三层的数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static DataTable GetThirdCategoryByCategoryId(int categoryId)
        {
            DataTable dataTable = new CategoryDao().GetThirdCategoryByCategoryId(categoryId);
            return dataTable;
        }

        /// <summary>
        /// 获取分类的类型
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string GetCategoryTypeByCategoryId(int categoryId)
        {
            return new CategoryDao().GetCategoryTypeByCategoryId(categoryId); 
        }

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static DataTable GetBrandsByCategoryId(int categoryId)
        {
            return new CategoryDao().GetBrandsByCategoryId(categoryId);
        }

        public static List<BrandCategoryInfo> GetSecondBrandByCategoryId(int CategoryId)
        {
            List<BrandCategoryInfo> items = new List<BrandCategoryInfo>();
            DataTable brands = GetBrandsByCategoryId(CategoryId);
            if (brands != null && brands.Rows.Count > 0)
            {
                foreach (DataRow row in brands.Rows)
                {
                    BrandCategoryInfo brand = new BrandCategoryInfo();
                    if (row["BrandId"] != null && !string.IsNullOrWhiteSpace(row["BrandId"].ToString()))
                    {
                        brand.BrandId = int.Parse(row["BrandId"].ToString());
                    }
                    if (row["BrandName"] != null && !string.IsNullOrWhiteSpace(row["BrandName"].ToString()))
                    {
                        brand.BrandName = row["BrandName"].ToString();
                    }
                    items.Add(brand);
                }
            }
            return items;
        }
	}
}
