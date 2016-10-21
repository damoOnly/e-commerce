using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace EcShop.SqlDal.Active
{
    public class PCActiveDao
    {
         private Database database;
         public PCActiveDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}	

        public  DataTable GetThisTopicList()
        {
            DataTable result;
            DbCommand command = this.database.GetStoredProcCommand("cp_Index_PCActiveProduct_ONE_Get");         

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
        }
        public DataTable GetThisTopicList_Two()
        {
            DataTable result;
            DbCommand command = this.database.GetStoredProcCommand("cp_Index_PCActiveProduct_Two_Get");

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
        }

        public DbQueryResult GetCurrActiveOneProductList(ProductBrowseQuery query)
        {
            string filter = this.BuildCurrProductBrowseQuerySearch(query);
            string text = "ShopName,ShopOwner,SupplierId,SupplierName,fastbuy_skuid,TaxRate,ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId,ShippingMode,CollectCount";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                text += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                text += ",SalePrice as RankPrice";
            }

            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                text += string.Format(",(select Name from Ecshop_Promotions pm  left join Ecshop_PromotionMemberGrades pg on pm.ActivityId=pg.ActivityId where pm.ActivityId=p.ActivityId  and pg.GradeId={0}) as PromotionName", member.GradeId);
            }
            else
            {
                text += ",'' as PromotionName";
            }



            if (query.TopId.HasValue && query.TopId.Value > 0)
            {
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_ActiveOneProductList p", "ProductId", filter, text);
            }
            else
            {
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_ActiveOneProductList p", "ProductId", filter, text);
            }


            //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", filter, text);
        }

        public DbQueryResult GetCurrBrowseActiveProductList(ProductBrowseQuery query)
        {
            string filter = this.BuildCurrProductBrowseQuerySearch(query);
            string text = "ShopName,ShopOwner,SupplierId,SupplierName,fastbuy_skuid,TaxRate,ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId,ShippingMode,CollectCount";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                text += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                text += ",SalePrice as RankPrice";
            }

            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                text += string.Format(",(select Name from Ecshop_Promotions pm  left join Ecshop_PromotionMemberGrades pg on pm.ActivityId=pg.ActivityId where pm.ActivityId=p.ActivityId  and pg.GradeId={0}) as PromotionName", member.GradeId);
            }
            else
            {
                text += ",'' as PromotionName";
            }



            if (query.TopId.HasValue && query.TopId.Value > 0)
            {
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseActiveProductList p", "ProductId", filter, text);
            }
            else
            {
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseActiveProductList p", "ProductId", filter, text);
            }


            //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", filter, text);
        }

        public DbQueryResult GetCurrBrowseActiveProductListByTopicId(ProductBrowseQuery query)
        {
            string filter = this.BuildActiveTopicProductBrowseQuerySearch(query);
            string text = "ShopName,ShopOwner,SupplierId,SupplierName,fastbuy_skuid,TaxRate,ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId,ShippingMode,CollectCount";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                text += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                text += ",SalePrice as RankPrice";
            }

            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                text += string.Format(",(select Name from Ecshop_Promotions pm  left join Ecshop_PromotionMemberGrades pg on pm.ActivityId=pg.ActivityId where pm.ActivityId=p.ActivityId  and pg.GradeId={0}) as PromotionName", member.GradeId);
            }
            else
            {
                text += ",'' as PromotionName";
            }



            if (query.TopId.HasValue && query.TopId.Value > 0)
            {
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseActiveProductList p", "ProductId", filter, text);
            }
            else
            {
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseActiveProductList p", "ProductId", filter, text);
            }


            //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", filter, text);
        }


        public DataSet GetActiveProductListByTopicIds(string topicids,int maxnum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM Vshop_Topics WHERE TopicId in({0});",topicids);

            sb.Append("select topicid,ShopName,ShopOwner,SupplierId,SupplierName,fastbuy_skuid,TaxRate,ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId,ShippingMode,CollectCount,IsCustomsClearance,IsfreeShipping,VistiCounts,BuyCardinality,Icon,IsPromotion,SaleType,MinTaxRate,MaxTaxRate,Convert(decimal(10,1), (SalePrice/MarketPrice)*10) as 'DiscountRate', isnull(IsDisplayDisCount,0) as 'IsDisplayDisCount'");
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                sb.AppendFormat(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                sb.AppendFormat("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                sb.Append(",SalePrice as RankPrice");
            }

            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                sb.AppendFormat(",(select Name from Ecshop_Promotions pm  left join Ecshop_PromotionMemberGrades pg on pm.ActivityId=pg.ActivityId where pm.ActivityId=p.ActivityId  and pg.GradeId={0}) as PromotionName", member.GradeId);
            }

            else
            {
               sb.Append(",'' as PromotionName");
            }

            //这里获取每个专题前maxnum件商品，排序按商品修改时间
            sb.AppendFormat(" from (select *, ROW_NUMBER() over(partition by TopicId order by sort2 asc) as rowNum from vw_Ecshop_BrowseTopicProductList) p  where p.rowNum <={0}  and p.topicid in({1})", maxnum, topicids);
            
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = dataSet.Tables[0].Columns["topicid"];
            DataColumn childColumn = dataSet.Tables[1].Columns["topicid"];
            DataRelation relation = new DataRelation("ProductItems", parentColumn, childColumn);
            dataSet.Relations.Add(relation);
            return dataSet;
        }
        protected string BuildCurrProductBrowseQuerySearch(ProductBrowseQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("IsApproved = 1 and SaleStatus = {0}", (int)query.ProductSaleStatus);
            stringBuilder.AppendFormat(" AND SaleStatus = {0}", 1);
            if (!query.IsPrecise)
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode) Like  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode)='{0}'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            if (query.AttributeValues.Count > 0)
            {
                foreach (AttributeValueInfo current in query.AttributeValues)
                {
                    stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Ecshop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", current.AttributeId, current.ValueId);
                }
            }
            if (!string.IsNullOrWhiteSpace(query.StrBrandId))
            {
                stringBuilder.AppendFormat(" AND BrandId in ({0})", query.StrBrandId);
            }

            if (query.MinSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
            }
            if (query.MaxSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
            }
            if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                    string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND (ProductName = '{0}'  or ShopName = '{0}') ", DataHelper.CleanSearchString(query.Keywords));
                }
            }

            if (!string.IsNullOrEmpty(query.SubKeywords) && query.SubKeywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.SubKeywords = DataHelper.CleanSearchString(query.SubKeywords);
                    string[] array = Regex.Split(query.SubKeywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND （ProductName = '{0}'or ShopName = '{0}')", DataHelper.CleanSearchString(query.SubKeywords));
                }
            }
            if (!string.IsNullOrWhiteSpace(query.StrCategoryId))
            {
                IList<CategoryInfo> categorys = new CategoryDao().GetListCategoryByIds(query.StrCategoryId);
                if (categorys != null && categorys.Count > 0)
                {
                    stringBuilder.Append(" and ( ");
                    foreach (CategoryInfo category in categorys)
                    {
                        stringBuilder.AppendFormat(" MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' or", category.Path);
                    }
                    stringBuilder.Remove(stringBuilder.ToString().LastIndexOf("or"), 2);
                    stringBuilder.Append(" ) ");
                }
                //stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE 'SELECT Path FROM Ecshop_Categories WHERE CategoryId in ({0})|%' OR ExtendCategoryPath LIKE 'SELECT Path FROM Ecshop_Categories WHERE CategoryId in ({0})|%') ", query.StrCategoryId);
            }
            if (!string.IsNullOrWhiteSpace(query.StrImportsourceId))//原产地
            {
                stringBuilder.AppendFormat(" AND ImportsourceId in ({0}) ", query.StrImportsourceId);
            }
            if (!string.IsNullOrEmpty(query.TagIds))
            {
                string[] array2 = query.TagIds.Split(new char[]
				{
					'_'
				});
                string[] array3 = array2;
                for (int i = 0; i < array3.Length; i++)
                {
                    string text = DataHelper.CleanSearchString(array3[i]);
                    if (!string.IsNullOrEmpty(text))
                    {
                        stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM Ecshop_ProductTag WHERE TagId = {0})", text);
                    }
                }
            }
            if (query.HasStock)
            {
                stringBuilder.Append(" AND stock>0 ");
            }
      

            if (query.supplierid.HasValue)
            {
                stringBuilder.AppendFormat(" AND supplierid={0} ", query.supplierid.Value);
            }

            if (query.TopId.HasValue)
            {
                if (query.TopId.Value > 0)
                {
                    stringBuilder.AppendFormat(" AND topicid={0} ", query.TopId.Value);
                }
            }



            return stringBuilder.ToString();
        }
        protected string BuildActiveTopicProductBrowseQuerySearch(ProductBrowseQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("IsApproved = 1 and SaleStatus = {0}", (int)query.ProductSaleStatus);
            stringBuilder.AppendFormat(" AND SaleStatus = {0}", 1);
            if (!query.IsPrecise)
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode) Like  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode)='{0}'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            if (query.AttributeValues.Count > 0)
            {
                foreach (AttributeValueInfo current in query.AttributeValues)
                {
                    stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Ecshop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", current.AttributeId, current.ValueId);
                }
            }
            if (!string.IsNullOrWhiteSpace(query.StrBrandId))
            {
                stringBuilder.AppendFormat(" AND BrandId in ({0})", query.StrBrandId);
            }

            if (query.MinSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
            }
            if (query.MaxSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
            }
            if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                    string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND (ProductName = '{0}'  or ShopName = '{0}') ", DataHelper.CleanSearchString(query.Keywords));
                }
            }

            if (!string.IsNullOrEmpty(query.SubKeywords) && query.SubKeywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.SubKeywords = DataHelper.CleanSearchString(query.SubKeywords);
                    string[] array = Regex.Split(query.SubKeywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND （ProductName = '{0}'or ShopName = '{0}')", DataHelper.CleanSearchString(query.SubKeywords));
                }
            }
            if (!string.IsNullOrWhiteSpace(query.StrCategoryId))
            {
                IList<CategoryInfo> categorys = new CategoryDao().GetListCategoryByIds(query.StrCategoryId);
                if (categorys != null && categorys.Count > 0)
                {
                    stringBuilder.Append(" and ( ");
                    foreach (CategoryInfo category in categorys)
                    {
                        stringBuilder.AppendFormat(" MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' or", category.Path);
                    }
                    stringBuilder.Remove(stringBuilder.ToString().LastIndexOf("or"), 2);
                    stringBuilder.Append(" ) ");
                }
                //stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE 'SELECT Path FROM Ecshop_Categories WHERE CategoryId in ({0})|%' OR ExtendCategoryPath LIKE 'SELECT Path FROM Ecshop_Categories WHERE CategoryId in ({0})|%') ", query.StrCategoryId);
            }
            if (!string.IsNullOrWhiteSpace(query.StrImportsourceId))//原产地
            {
                stringBuilder.AppendFormat(" AND ImportsourceId in ({0}) ", query.StrImportsourceId);
            }
            if (!string.IsNullOrEmpty(query.TagIds))
            {
                string[] array2 = query.TagIds.Split(new char[]
				{
					'_'
				});
                string[] array3 = array2;
                for (int i = 0; i < array3.Length; i++)
                {
                    string text = DataHelper.CleanSearchString(array3[i]);
                    if (!string.IsNullOrEmpty(text))
                    {
                        stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM Ecshop_ProductTag WHERE TagId = {0})", text);
                    }
                }
            }
            if (query.HasStock)
            {
                stringBuilder.Append(" AND stock>0 ");
            }

            if (query.supplierid.HasValue)
            {
                stringBuilder.AppendFormat(" AND supplierid={0} ", query.supplierid.Value);
            }

            if (query.TopId.HasValue)
            {
                if (query.TopId.Value > 0)
                {
                    stringBuilder.AppendFormat(" AND topicid={0} ", query.TopId.Value);
                }
            }



            return stringBuilder.ToString();
        }



    }
}
