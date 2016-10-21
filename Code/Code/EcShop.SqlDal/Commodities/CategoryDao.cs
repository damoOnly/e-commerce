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
    public class CategoryDao
    {
        private Database database;
        public CategoryDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Categories ORDER BY DisplaySequence");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetCategories(int isDisable)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Categories where ISNULL(IsDisable,0)=" + isDisable + " ORDER BY DisplaySequence");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetCategoryiesBySupplierId(int supplierId)
        {
            DbCommand strsql = this.database.GetSqlStringCommand(@"select distinct c.CategoryId,c.Name,c.DisplaySequence,c.ParentCategoryId,c.Depth,c.Path,c.RewriteName,
c.Theme,c.HasChildren,c.Icon from Ecshop_Products p
inner join Ecshop_Supplier s on p.SupplierId = s.SupplierId 
inner join Ecshop_Categories c on c.CategoryId = p.CategoryId 
where s.SupplierId =@SupplierId and p.SaleStatus = 1 and ISNULL(c.IsDisable,0)=0  order by c.DisplaySequence desc  ");
            this.database.AddInParameter(strsql, "SupplierId", DbType.Int32, supplierId);
            DataTable result = new DataTable();
            using (IDataReader dataReader = this.database.ExecuteReader(strsql))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取下二层的数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public DataTable GetSecondCategoryByCategoryId(int categoryId)
        {
            DbCommand strsql = this.database.GetSqlStringCommand(@"SELECT c.Name as ParentCategoryName,cc.* 
FROM  Ecshop_Categories c
inner join Ecshop_Categories cc on cc.ParentCategoryId =  c.CategoryId
where c.CategoryId = @categoryId 
and  cc.CategoryId in (
select ParentCategoryId from Ecshop_Categories ec
inner join Ecshop_Products ep on ec.CategoryId = ep.CategoryId 
--and  ep.SaleStatus = 1 
and ISNULL(ec.IsDisable,0)=0 
 )
and  ISNULL(c.IsDisable,0)=0 order by c.DisplaySequence desc   ");
            this.database.AddInParameter(strsql, "categoryId", DbType.Int32, categoryId);
            DataTable result = new DataTable();
            using (IDataReader dataReader = this.database.ExecuteReader(strsql))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

                /// <summary>
        /// 获取下三层的数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public DataTable GetThirdCategoryByCategoryId(int categoryId)
        {
            DbCommand strsql = this.database.GetSqlStringCommand(@"SELECT c.Name as ParentCategoryName,cc.* 
FROM  Ecshop_Categories c
inner join Ecshop_Categories cc on cc.ParentCategoryId =  c.CategoryId
where c.CategoryId = @categoryId 
and  cc.CategoryId in (
select ec.CategoryId from Ecshop_Categories ec
inner join Ecshop_Products ep on ec.CategoryId = ep.CategoryId 
--and  ep.SaleStatus = 1 
and ISNULL(ec.IsDisable,0)=0 
 )
and  ISNULL(c.IsDisable,0)=0 order by c.DisplaySequence desc   ");
            this.database.AddInParameter(strsql, "categoryId", DbType.Int32, categoryId);
            DataTable result = new DataTable();
            using (IDataReader dataReader = this.database.ExecuteReader(strsql))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取分类的类型
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public string GetCategoryTypeByCategoryId(int categoryId)
        {
            DbCommand strsql = this.database.GetSqlStringCommand(@"	select CategoryType from Ecshop_Categories where CategoryId = @categoryId ");
            this.database.AddInParameter(strsql, "categoryId", DbType.Int32, categoryId);
            object obj =this.database.ExecuteScalar(strsql);
            int result = 0;
            if (obj != null)
            {
                int.TryParse(obj.ToString(), out result);
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public DataTable GetBrandsByCategoryId(int categoryId)
        {
            DbCommand strsql = this.database.GetSqlStringCommand(@"select distinct b.BrandId,b.BrandName
from Ecshop_Products p
inner join Ecshop_BrandCategories b on p.BrandId = b.BrandId 
inner join Ecshop_Categories c on c.CategoryId = p.CategoryId 
where c.ParentCategoryId = @CategoryId and p.SaleStatus = 1 and ISNULL(c.IsDisable,0)=0  order by BrandId desc ");
            this.database.AddInParameter(strsql, "CategoryId", DbType.Int32, categoryId);
            DataTable result = new DataTable();
            using (IDataReader dataReader = this.database.ExecuteReader(strsql))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据主类型ID获取第三层类型数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public DataTable GetThirdCategoryiesById(int categoryId)
        {
            DbCommand strsql = this.database.GetSqlStringCommand("select * from Ecshop_Categories where ParentCategoryId in (select CategoryId from Ecshop_Categories where ParentCategoryId = @tempId ) AND ISNULL(IsDisable,0)=0 ");
            this.database.AddInParameter(strsql, "tempId", DbType.Int32, categoryId);
            DataTable result = new DataTable();
            using (IDataReader dataReader = this.database.ExecuteReader(strsql))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetCategoriesByTopId(int cId, int c2, int c3, int c4, int c5, int c6, int c7)
        {
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("GetThisCategory");
            this.database.AddInParameter(sqlStringCommand, "cateGoryId", DbType.Int32, cId);
            this.database.AddInParameter(sqlStringCommand, "cateGoryId2", DbType.Int32, c2);
            this.database.AddInParameter(sqlStringCommand, "cateGoryId3", DbType.Int32, c3);
            this.database.AddInParameter(sqlStringCommand, "cateGoryId4", DbType.Int32, c4);
            this.database.AddInParameter(sqlStringCommand, "cateGoryId5", DbType.Int32, c5);
            this.database.AddInParameter(sqlStringCommand, "cateGoryId6", DbType.Int32, c6);
            this.database.AddInParameter(sqlStringCommand, "cateGoryId7", DbType.Int32, c7);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public CategoryInfo GetCategory(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Categories WHERE CategoryId =@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            CategoryInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<CategoryInfo>(dataReader);
            }
            return result;
        }
        public Dictionary<string, string> GetSecondCategoriesById(int userid)
        {
            string sql = string.Format(@"select p.ProductId,ec.ParentCategoryId
from Ecshop_Products p
inner join ecshop_skus s on p.ProductId =  s.ProductId 
inner join Ecshop_Categories ec on ec.CategoryId = p.CategoryId
where s.SkuId in (select skuid from Ecshop_ShoppingCarts  where UserId = {0})", userid);
            DbCommand strsql = this.database.GetSqlStringCommand(sql);
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (IDataReader dataReader = this.database.ExecuteReader(strsql))
            {
                while (dataReader.Read())
                {
                    if (dataReader["ProductId"] != null && dataReader["ParentCategoryId"] != null)
                    {
                        result.Add(dataReader["ProductId"].ToString(), dataReader["ParentCategoryId"].ToString());
                    }
                }
            }
            return result;
        }
        public int CreateCategory(CategoryInfo category)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Create");
            this.database.AddOutParameter(storedProcCommand, "CategoryId", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(storedProcCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Int32, category.DisplaySequence);
            if ((int)category.CategoryType >=0)
            {
                this.database.AddInParameter(storedProcCommand, "CategoryType", DbType.Int32, (int)category.CategoryType);
            }
            if (!string.IsNullOrEmpty(category.IconUrl))
            {
                this.database.AddInParameter(storedProcCommand, "Icon", DbType.String, category.IconUrl);
            }
            if (!string.IsNullOrEmpty(category.MetaTitle))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Title", DbType.String, category.MetaTitle);
            }
            if (!string.IsNullOrEmpty(category.MetaDescription))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, category.MetaDescription);
            }
            if (!string.IsNullOrEmpty(category.MetaKeywords))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            }
            if (!string.IsNullOrEmpty(category.Notes1))
            {
                this.database.AddInParameter(storedProcCommand, "Notes1", DbType.String, category.Notes1);
            }
            if (!string.IsNullOrEmpty(category.Notes2))
            {
                this.database.AddInParameter(storedProcCommand, "Notes2", DbType.String, category.Notes2);
            }
            if (!string.IsNullOrEmpty(category.Notes3))
            {
                this.database.AddInParameter(storedProcCommand, "Notes3", DbType.String, category.Notes3);
            }
            if (!string.IsNullOrEmpty(category.Notes4))
            {
                this.database.AddInParameter(storedProcCommand, "Notes4", DbType.String, category.Notes4);
            }
            if (!string.IsNullOrEmpty(category.Notes5))
            {
                this.database.AddInParameter(storedProcCommand, "Notes5", DbType.String, category.Notes5);
            }
            if (category.ParentCategoryId.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, category.ParentCategoryId.Value);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, 0);
            }
            if (category.AssociatedProductType.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType.Value);
            }
            if (!string.IsNullOrEmpty(category.RewriteName))
            {
                this.database.AddInParameter(storedProcCommand, "RewriteName", DbType.String, category.RewriteName);
            }

            if (category.TaxRateId.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "TaxRateId", DbType.Int32, category.TaxRateId.Value);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "TaxRateId", DbType.Int32, 0);
            }
            this.database.AddInParameter(storedProcCommand, "IsDisable", DbType.Int32, category.IsDisable);


            this.database.ExecuteNonQuery(storedProcCommand);
            return (int)this.database.GetParameterValue(storedProcCommand, "CategoryId");
        }
        public CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Categories SET [Name] = @Name, SKUPrefix = @SKUPrefix,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Description = @Meta_Description, Icon = @IconUrl,Meta_Keywords = @Meta_Keywords, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName,TaxRateId=@TaxRateId,IsDisable=@IsDisable,CategoryType=@CategoryType WHERE CategoryId = @CategoryId;UPDATE Ecshop_Categories SET RewriteName = @RewriteName WHERE ParentCategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, category.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(sqlStringCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(sqlStringCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType);
            this.database.AddInParameter(sqlStringCommand, "Meta_Title", DbType.String, category.MetaTitle);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, category.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, category.IconUrl);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "Notes1", DbType.String, category.Notes1);
            this.database.AddInParameter(sqlStringCommand, "Notes2", DbType.String, category.Notes2);
            this.database.AddInParameter(sqlStringCommand, "Notes3", DbType.String, category.Notes3);
            this.database.AddInParameter(sqlStringCommand, "Notes4", DbType.String, category.Notes4);
            this.database.AddInParameter(sqlStringCommand, "Notes5", DbType.String, category.Notes5);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, category.RewriteName);
            this.database.AddInParameter(sqlStringCommand, "TaxRateId", DbType.Int32, category.TaxRateId);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Int32, category.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "CategoryType", DbType.Int32, (int)category.CategoryType);

            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1) ? CategoryActionStatus.Success : CategoryActionStatus.UnknowError;
        }
        public bool DeleteCategory(int categoryId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Delete");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            return this.database.ExecuteNonQuery(storedProcCommand) > 0;
        }
        public int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Ecshop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
            this.database.AddInParameter(sqlStringCommand, "oldCategoryId", DbType.Int32, oldCategoryId);
            this.database.AddInParameter(sqlStringCommand, "newCategory", DbType.Int32, newCategory);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@CategoryId", DbType.Int32, categoryId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", DbType.String, extendCategoryPath);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
        }
        public bool SetCategoryThemes(int categoryId, string themeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
        }
        public DataTable GetCategoryes(string categroynaem)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId,DePth FROM Ecshop_Categories WHERE [Name]=@Name");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, DataHelper.CleanSearchString(categroynaem));
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataSet GetThreeLayerCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name,CategoryId,Icon,RewriteName FROM Ecshop_Categories WHERE ParentCategoryId=0 AND Depth = 1 ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,Icon,CategoryId,RewriteName FROM Ecshop_Categories WHERE Depth = 2 ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,Icon,CategoryId,RewriteName FROM Ecshop_Categories WHERE Depth = 3 ORDER BY DisplaySequence;");
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            dataSet.Relations.Add("One", dataSet.Tables[0].Columns["CategoryId"], dataSet.Tables[1].Columns["ParentCategoryId"], false);
            dataSet.Relations.Add("Two", dataSet.Tables[1].Columns["CategoryId"], dataSet.Tables[2].Columns["ParentCategoryId"], false);
            return dataSet;
        }

        /// <summary>
        /// 检查商品是否是限制商品
        /// </summary>
        /// <param name="CategoryIdpt">二级分类ＩＤ</param>
        /// <param name="CategoryId">商品类别ID</param>
        /// <returns></returns>
        public bool CheckPructCag(string CategoryIdpt, string CategoryId)
        {
//            string sql = @"select count(1) from Ecshop_Categories where ParentCategoryId in 
//                        (select CategoryId from Ecshop_Categories where CategoryId  in ("+CategoryIdpt+@") ) AND ISNULL(IsDisable,0)=0 
//                        and  CategoryId=@CategoryId";
            string sql = @"	select count(1) from Ecshop_Categories  where  CategoryId=@CategoryId  and ParentCategoryId in
						(" + CategoryIdpt + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "@CategoryId", DbType.Int32, CategoryId);
            object obj=this.database.ExecuteScalar(sqlStringCommand);
            int count=0;
            if (obj != null)
            {
                int.TryParse(obj.ToString(), out count);
            }
            return count > 0 ? true : false;
          
        }
        public IList<CategoryInfo> GetCategoryByIds(IList<int> Categoryids)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            string text = "(";
            foreach (int current in Categoryids)
            {
                text = text + current + ",";
            }
            IList<CategoryInfo> result;
            if (text.Length <= 1)
            {
                result = list;
            }
            else
            {
                text = text.Substring(0, text.Length - 1) + ")";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Ecshop_Categories WHERE CategoryId IN " + text);
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(DataMapper.PopulateProductCategory(dataReader));
                    }
                }
                result = list;
            }
            return result;
        }

        public IList<CategoryInfo> GetListCategoryByIds(string ids)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Ecshop_Categories WHERE CategoryId IN (" + ids + ")");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(DataMapper.PopulateProductCategory(dataReader));
                }
            }

            return list;
        }


    }
}
