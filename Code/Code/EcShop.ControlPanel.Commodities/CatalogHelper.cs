using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
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
using System.Web.Caching;
namespace EcShop.ControlPanel.Commodities
{
    public sealed class CatalogHelper
    {
        private const string CategoriesCachekey = "DataCache-Categories";
        private CatalogHelper()
        {
        }
        public static IList<CategoryInfo> GetMainCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            System.Data.DataTable categories = CatalogHelper.GetCategories();
            System.Data.DataRow[] array = categories.Select("Depth = 1");
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
            }
            return list;
        }

        //不包含1级分类的其他所有分类
        public static IList<CategoryInfo> GetAllCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            System.Data.DataTable categories = CatalogHelper.GetCategories();
            System.Data.DataRow[] array = categories.Select("Depth <> 1 and ISNULL(IsDisable,0)=0 ");
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
            }
            return list;
        }


        public static IList<CategoryInfo> GetAllCategorieList()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            System.Data.DataTable categories = CatalogHelper.GetCategories();
            System.Data.DataRow[] array = categories.Select("");
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
            }
            return list;
        }


        public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
            System.Data.DataTable categories = CatalogHelper.GetCategories();
            System.Data.DataRow[] array = categories.Select(filterExpression);
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
            }
            return list;
        }
        public static CategoryInfo GetCategory(int categoryId)
        {
            return new CategoryDao().GetCategory(categoryId);
        }
        public static string GetFullCategory(int categoryId)
        {
            CategoryInfo category = CatalogHelper.GetCategory(categoryId);
            string result;
            if (category == null)
            {
                result = null;
            }
            else
            {
                string text = category.Name;
                while (category != null && category.ParentCategoryId.HasValue)
                {
                    category = CatalogHelper.GetCategory(category.ParentCategoryId.Value);
                    if (category != null)
                    {
                        text = category.Name + " &raquo; " + text;
                    }
                }
                result = text;
            }
            return result;
        }
        public static System.Data.DataTable GetCategories()
        {
            System.Data.DataTable dataTable = HiCache.Get("DataCache-Categories") as System.Data.DataTable;
            if (null == dataTable)
            {
                dataTable = new CategoryDao().GetCategories();
                HiCache.Insert("DataCache-Categories", dataTable, 360, CacheItemPriority.Normal);
            }
            return dataTable;
        }
        public static System.Data.DataTable GetCategoryisChirldren(int categoryId)
        {
            System.Data.DataTable categories = CatalogHelper.GetCategories();
            System.Data.DataTable dataTable = categories.Clone();
            if (categoryId > 0)
            {
                CategoryInfo category = CatalogHelper.GetCategory(categoryId);
                System.Data.DataRow[] array = categories.Select(string.Concat(new object[]
				{
					"CategoryId=",
					categoryId,
					" OR [Path] like '",
					category.Path,
					"|%'"
				}));
                dataTable.Rows.Clear();
                for (int i = 0; i < array.Length; i++)
                {
                    dataTable.Rows.Add(array[i].ItemArray);
                }
            }
            return dataTable;
        }
        public static IList<CategoryInfo> GetSequenceCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            IList<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
            foreach (CategoryInfo current in mainCategories)
            {
                list.Add(current);
                CatalogHelper.LoadSubCategorys(current.CategoryId, list);
            }
            return list;
        }
        private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
        {
            IList<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(parentCategoryId);
            if (subCategories != null && subCategories.Count > 0)
            {
                foreach (CategoryInfo current in subCategories)
                {
                    categories.Add(current);
                    CatalogHelper.LoadSubCategorys(current.CategoryId, categories);
                }
            }
        }
        public static CategoryActionStatus AddCategory(CategoryInfo category)
        {
            CategoryActionStatus result;
            if (null == category)
            {
                result = CategoryActionStatus.UnknowError;
            }
            else
            {
                Globals.EntityCoding(category, true);
                int num = new CategoryDao().CreateCategory(category);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.AddProductCategory, string.Format(CultureInfo.InvariantCulture, "创建了一个新的店铺分类:”{0}”", new object[]
					{
						category.Name
					}));
                    HiCache.Remove("DataCache-Categories");
                }
                result = CategoryActionStatus.Success;
            }
            return result;
        }
        public static CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            CategoryActionStatus result;
            if (null == category)
            {
                result = CategoryActionStatus.UnknowError;
            }
            else
            {
                Globals.EntityCoding(category, true);
                CategoryActionStatus categoryActionStatus = new CategoryDao().UpdateCategory(category);
                if (categoryActionStatus == CategoryActionStatus.Success)
                {
                    EventLogs.WriteOperationLog(Privilege.EditProductCategory, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品分类", new object[]
					{
						category.CategoryId
					}));
                    HiCache.Remove("DataCache-Categories");
                }
                result = categoryActionStatus;
            }
            return result;
        }
        public static bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            return new CategoryDao().SwapCategorySequence(categoryId, displaysequence);
        }
        public static bool DeleteCategory(int categoryId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductCategory);
            bool flag = new CategoryDao().DeleteCategory(categoryId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductCategory, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的店铺分类", new object[]
				{
					categoryId
				}));
                HiCache.Remove("DataCache-Categories");
            }
            return flag;
        }
        public static int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            return new CategoryDao().DisplaceCategory(oldCategoryId, newCategory);
        }
        public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            return new CategoryDao().SetProductExtendCategory(productId, extendCategoryPath);
        }
        public static bool SetCategoryThemes(int categoryId, string themeName)
        {
            bool flag = new CategoryDao().SetCategoryThemes(categoryId, themeName);
            if (flag)
            {
                HiCache.Remove("DataCache-Categories");
            }
            return false;
        }
        public static string UploadCategoryIcon(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                if (!Directory.Exists(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/category/")))
                {
                    Directory.CreateDirectory(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/category/"));
                }
                string text = HiContext.Current.GetStoragePath() + "/category/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static System.Data.DataTable GetCategoryes(string categroynaem)
        {
            return new CategoryDao().GetCategoryes(categroynaem);
        }
        public static bool AddBrandCategory(BrandCategoryInfo brandCategory)
        {
            BrandCategoryDao brandCategoryDao = new BrandCategoryDao();
            int num = brandCategoryDao.AddBrandCategory(brandCategory);
            if (num > 0 && brandCategory.ProductTypes.Count > 0)
            {
                brandCategoryDao.AddBrandProductTypes(num, brandCategory.ProductTypes);
            }
            return true;
        }
        public static System.Data.DataTable GetBrandCategories()
        {
            return new BrandCategoryDao().GetBrandCategories();
        }
        public static System.Data.DataTable GetBrandCategories(int categoryId)
        {
            return new BrandCategoryDao().GetBrandCategories(categoryId);
        }
        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return new BrandCategoryDao().GetBrandCategory(brandId);
        }
        public static bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
        {
            BrandCategoryDao brandCategoryDao = new BrandCategoryDao();
            bool flag = brandCategoryDao.UpdateBrandCategory(brandCategory);
            if (flag && brandCategoryDao.DeleteBrandProductTypes(brandCategory.BrandId))
            {
                brandCategoryDao.AddBrandProductTypes(brandCategory.BrandId, brandCategory.ProductTypes);
            }
            return flag;
        }
        public static bool BrandHvaeProducts(int brandId)
        {
            return new BrandCategoryDao().BrandHvaeProducts(brandId);
        }
        public static bool DeleteBrandCategory(int brandId)
        {
            return new BrandCategoryDao().DeleteBrandCategory(brandId);
        }
        public static void UpdateBrandCategorieDisplaySequence(int brandId, SortAction action)
        {
            new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(brandId, action);
        }
        public static bool UpdateBrandCategoryDisplaySequence(int barndId, int displaysequence)
        {
            return new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(barndId, displaysequence);
        }
        public static string UploadBrandCategorieImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + "/brand/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        /// <summary>
        ///限时抢购获取上传地址
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public static string UploadActiveCategorieImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                if (Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(HiContext.Current.GetStoragePath() + "/ActiveCategorie")) == false)
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(HiContext.Current.GetStoragePath() + "/ActiveCategorie"));
                }
                string text = HiContext.Current.GetStoragePath() + "/ActiveCategorie/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static bool SetBrandCategoryThemes(int brandid, string themeName)
        {
            bool flag = new BrandCategoryDao().SetBrandCategoryThemes(brandid, themeName);
            if (flag)
            {
                HiCache.Remove("DataCache-Categories");
            }
            return flag;
        }
        public static System.Data.DataTable GetBrandCategories(string brandName)
        {
            return new BrandCategoryDao().GetBrandCategories(brandName);
        }
        public static System.Data.DataTable GetTags()
        {
            return new TagDao().GetTags();
        }
        public static string GetTagName(int tagId)
        {
            return new TagDao().GetTagName(tagId);
        }

        public static int AddTags(string tagName)
        {
            TagDao tagDao = new TagDao();
            int result = 0;
            if (tagDao.GetTags(tagName) <= 0)
            {
                result = tagDao.AddTags(tagName);
            }
            return result;
        }
        public static bool UpdateTags(int tagId, string tagName)
        {
            return new TagDao().UpdateTags(tagId, tagName);
        }
        public static bool DeleteTags(int tagId)
        {
            return new TagDao().DeleteTags(tagId);
        }

        public static System.Data.DataTable GetBrandTags(string tagName)
        {
            return new BrandTagDao().GetBrandTags(tagName);
        }

        public static System.Data.DataTable GetBrandTags()
        {
            return new BrandTagDao().GetBrandTags();
        }
        public static bool DeleteBrandTags(int tagId)
        {
            return new BrandTagDao().DeleteBrandTag(tagId);
        }
        public static bool BrandTagHaveBrand(int brandTagId)
        {
            return new BrandTagDao().BrandTagHaveBrand(brandTagId);
        }
        public static bool UpdateBrandTagDisplaySequence(int brandTagId, int displaysequence)
        {
            return new BrandTagDao().UpdateBrandCategoryDisplaySequence(brandTagId, displaysequence);
        }
        public static bool AddBrandTag(BrandTagInfo brandTag)
        {
            BrandTagDao brandCategoryDao = new BrandTagDao();
            int num = brandCategoryDao.AddBrandTag(brandTag);
            if (num > 0 && brandTag.BrandCategoryInfos.Count > 0)
            {
                brandCategoryDao.AddBrandBrandTags(num, brandTag.BrandCategoryInfos);
            }
            return true;
        }
        public static BrandTagInfo GetBrandTag(int brandId)
        {
            return new BrandTagDao().GetBrandTag(brandId);
        }
        public static bool UpdateBrandTag(BrandTagInfo brandCategory)
        {
            BrandTagDao brandCategoryDao = new BrandTagDao();
            bool flag = brandCategoryDao.UpdateBrandTag(brandCategory);
            if (flag && brandCategoryDao.DeleteBrandTagBrand(brandCategory.BrandTagId))
            {
                brandCategoryDao.AddBrandAndBrandTags(brandCategory.BrandTagId, brandCategory.BrandCategoryInfos);
            }
            return flag;
        }

        public static System.Data.DataTable GetTaxRate()
        {
            return new TaxRateDao().GetTaxRate();
        }

        public static System.Data.DataTable GetTaxRate(string taxRate)
        {
            return new TaxRateDao().GetTaxRate(taxRate);
        }

        public static string GetTaxRate(int taxId)
        {
            return new TaxRateDao().GetTaxRate(taxId);
        }
        public static int AddTaxRate(decimal taxRate, string code, string CodeDescription)
        {
            //修改添加行邮编码支持
            TaxRateDao taxRateDao = new TaxRateDao();
            int result = 0;
            if (taxRateDao.GetTaxRate(0, code) <= 0)
            {
                result = taxRateDao.AddTaxRate(taxRate, code, CodeDescription);
            }
            return result;
        }
        public static bool UpdateTaxRate(int taxId, decimal taxRate, string code, string CodeDescription)
        {
            //修改添加行邮编码支持
            TaxRateDao taxRateDao = new TaxRateDao();
            if (taxRateDao.GetTaxRate(taxId, code) <= 0)
            {
                return new TaxRateDao().UpdateTaxRate(taxId, taxRate, code, CodeDescription);
            }
            return false;
        }
        public static bool DeleteTaxRate(int taxId)
        {
            return new TaxRateDao().DeleteTaxRate(taxId);
        }

        public static IList<CategoryInfo> GetCategoryByIds(IList<int> Categoryids)
        {
            return new CategoryDao().GetCategoryByIds(Categoryids);
        }

        public static IList<CategoryInfo> GetListCategoryByIds(string ids)
        {
            return new CategoryDao().GetListCategoryByIds(ids);
        }

        /////////////////////////////计量单位/////////////////////////////////////////////////
        /// <summary>
        /// 新增计量单位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddUnit(BaseUnitsInfo model)
        {
            return new BaseUnitDao().AddUnit(model);
        }
        /// <summary>
        /// 编辑计量单位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool EditUnit(BaseUnitsInfo model)
        {
            return new BaseUnitDao().EditUnit(model);
        }
        /// <summary>
        /// 是否存在计量单位
        /// </summary>
        /// <param name="HSJoinID">海关代码</param>
        /// <param name="Name_CN">名称</param>
        /// <returns></returns>
        public static bool IsExistUnit(string HSJoinID, string Name_CN, int id)
        {
            return new BaseUnitDao().IsExistUnit(HSJoinID, Name_CN, id);
        }

        /// <summary>
        /// 根据计量单位名称获取数据
        /// </summary>
        /// <param name="UnitName">计量单位名称</param>
        /// <returns></returns>
        public static System.Data.DataTable GetUnits()
        {
            return new BaseUnitDao().GetUnits();
        }

        /// <summary>
        /// 根据计量单位名称或海关代码获取数据
        /// </summary>
        /// <returns></returns>
        public static System.Data.DataTable GetUnits(string Key)
        {
            return new BaseUnitDao().GetUnits(Key);
        }
        /// <summary>
        /// 根据计量单位Id获取数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static DataTable GetUnitsById(int Id)
        {
            return new BaseUnitDao().GetUnitsById(Id);
        }
        /// <summary>
        /// 根据Id删除计量单位
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DeleteUnit(int Id, string DeleteUser)
        {
            return new BaseUnitDao().DeleteUnit(Id, DeleteUser);
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        /// <param name="UnitId"></param>
        /// <param name="displaysequence"></param>
        /// <returns></returns>
        public static bool UpdateUnitDisplaySequence(int UnitId, int displaysequence)
        {
            return new BaseUnitDao().UpdateUnitDisplaySequence(UnitId, displaysequence);
        }
        /////////////////////////////计量单位/////////////////////////////////////////////////
    }
}
