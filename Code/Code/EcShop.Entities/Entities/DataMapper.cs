using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using System;
using System.Data;
namespace EcShop.Entities
{
    public static class DataMapper
    {
        public static ArticleInfo PopulateArticle(IDataRecord reader)
        {
            ArticleInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ArticleInfo articleInfo = new ArticleInfo();
                articleInfo.ArticleId = (int)reader["ArticleId"];
                articleInfo.CategoryId = (int)reader["CategoryId"];
                articleInfo.Title = (string)reader["Title"];
                if (reader["Meta_Description"] != System.DBNull.Value)
                {
                    articleInfo.MetaDescription = (string)reader["Meta_Description"];
                }
                if (reader["Meta_Keywords"] != System.DBNull.Value)
                {
                    articleInfo.MetaKeywords = (string)reader["Meta_Keywords"];
                }
                if (reader["Description"] != System.DBNull.Value)
                {
                    articleInfo.Description = (string)reader["Description"];
                }
                if (reader["IconUrl"] != System.DBNull.Value)
                {
                    articleInfo.IconUrl = (string)reader["IconUrl"];
                }
                articleInfo.Content = (string)reader["Content"];
                articleInfo.AddedDate = (System.DateTime)reader["AddedDate"];
                articleInfo.IsRelease = (bool)reader["IsRelease"];
                result = articleInfo;
            }
            return result;
        }
        public static ArticleCategoryInfo PopulateArticleCategory(IDataRecord reader)
        {
            ArticleCategoryInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ArticleCategoryInfo articleCategoryInfo = new ArticleCategoryInfo();
                articleCategoryInfo.CategoryId = (int)reader["CategoryId"];
                articleCategoryInfo.Name = (string)reader["Name"];
                articleCategoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
                if (reader["IconUrl"] != System.DBNull.Value)
                {
                    articleCategoryInfo.IconUrl = (string)reader["IconUrl"];
                }
                if (reader["Description"] != System.DBNull.Value)
                {
                    articleCategoryInfo.Description = (string)reader["Description"];
                }
                result = articleCategoryInfo;
            }
            return result;
        }


        public static TaxRateInfo PopulateTaxRate(IDataRecord reader)
        {
            TaxRateInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                TaxRateInfo taxRateInfo = new TaxRateInfo();
                taxRateInfo.TaxRateId = (int)reader["TaxId"];
                taxRateInfo.TaxRate = (decimal)reader["TaxRate"];
                result = taxRateInfo;
            }
            return result;
        }


        public static HelpInfo PopulateHelp(IDataReader reader)
        {
            HelpInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                HelpInfo helpInfo = new HelpInfo();
                helpInfo.CategoryId = (int)reader["CategoryId"];
                helpInfo.HelpId = (int)reader["HelpId"];
                helpInfo.Title = (string)reader["Title"];
                if (reader["Meta_Description"] != System.DBNull.Value)
                {
                    helpInfo.MetaDescription = (string)reader["Meta_Description"];
                }
                if (reader["Meta_Keywords"] != System.DBNull.Value)
                {
                    helpInfo.MetaKeywords = (string)reader["Meta_Keywords"];
                }
                if (reader["Description"] != System.DBNull.Value)
                {
                    helpInfo.Description = (string)reader["Description"];
                }
                helpInfo.Content = (string)reader["Content"];
                helpInfo.AddedDate = (System.DateTime)reader["AddedDate"];
                helpInfo.IsShowFooter = (bool)reader["IsShowFooter"];
                result = helpInfo;
            }
            return result;
        }
        public static HelpCategoryInfo PopulateHelpCategory(IDataReader reader)
        {
            HelpCategoryInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
                helpCategoryInfo.CategoryId = new int?((int)reader["CategoryId"]);
                helpCategoryInfo.Name = (string)reader["Name"];
                helpCategoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
                if (reader["IconUrl"] != System.DBNull.Value)
                {
                    helpCategoryInfo.IconUrl = (string)reader["IconUrl"];
                }
                if (reader["IndexChar"] != System.DBNull.Value)
                {
                    helpCategoryInfo.IndexChar = (string)reader["IndexChar"];
                }
                if (reader["Description"] != System.DBNull.Value)
                {
                    helpCategoryInfo.Description = (string)reader["Description"];
                }
                helpCategoryInfo.IsShowFooter = (bool)reader["IsShowFooter"];
                result = helpCategoryInfo;
            }
            return result;
        }
        public static AfficheInfo PopulateAffiche(IDataReader reader)
        {
            AfficheInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                AfficheInfo afficheInfo = new AfficheInfo();
                afficheInfo.AfficheId = (int)reader["AfficheId"];
                if (reader["Title"] != System.DBNull.Value)
                {
                    afficheInfo.Title = (string)reader["Title"];
                }
                afficheInfo.Content = (string)reader["Content"];
                afficheInfo.AddedDate = (System.DateTime)reader["AddedDate"];
                result = afficheInfo;
            }
            return result;
        }
        public static LeaveCommentInfo PopulateLeaveComment(IDataReader reader)
        {
            LeaveCommentInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                LeaveCommentInfo leaveCommentInfo = new LeaveCommentInfo();
                leaveCommentInfo.LeaveId = (long)reader["LeaveId"];
                if (reader["UserId"] != System.DBNull.Value)
                {
                    leaveCommentInfo.UserId = new int?((int)reader["UserId"]);
                }
                if (reader["UserName"] != System.DBNull.Value)
                {
                    leaveCommentInfo.UserName = (string)reader["UserName"];
                }
                leaveCommentInfo.Title = (string)reader["Title"];
                leaveCommentInfo.PublishContent = (string)reader["PublishContent"];
                leaveCommentInfo.PublishDate = (System.DateTime)reader["PublishDate"];
                leaveCommentInfo.LastDate = (System.DateTime)reader["LastDate"];
                leaveCommentInfo.FeedbackType = new Nullable<int>();
                if (reader["FeedbackType"] != System.DBNull.Value)
                {
                    leaveCommentInfo.FeedbackType = (int)reader["FeedbackType"];
                }
                if (reader["ContactWay"] != System.DBNull.Value)
                {
                    leaveCommentInfo.ContactWay = (string)reader["ContactWay"];
                }
                result = leaveCommentInfo;
            }
            return result;
        }
        public static LeaveCommentReplyInfo PopulateLeaveCommentReply(IDataReader reader)
        {
            LeaveCommentReplyInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                result = new LeaveCommentReplyInfo
                {
                    LeaveId = (long)reader["LeaveId"],
                    ReplyId = (long)reader["ReplyId"],
                    UserId = (int)reader["UserId"],
                    ReplyContent = (string)reader["ReplyContent"],
                    ReplyDate = (System.DateTime)reader["ReplyDate"]
                };
            }
            return result;
        }
        public static ProductConsultationInfo PopulateProductConsultation(IDataRecord reader)
        {
            ProductConsultationInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ProductConsultationInfo productConsultationInfo = new ProductConsultationInfo();
                productConsultationInfo.ConsultationId = (int)reader["ConsultationId"];
                productConsultationInfo.ProductId = (int)reader["ProductId"];
                productConsultationInfo.UserId = (int)reader["UserId"];
                productConsultationInfo.UserName = (string)reader["UserName"];
                productConsultationInfo.ConsultationText = (string)reader["ConsultationText"];
                productConsultationInfo.ConsultationDate = (System.DateTime)reader["ConsultationDate"];
                productConsultationInfo.UserEmail = (string)reader["UserEmail"];
                if (System.DBNull.Value != reader["ReplyText"])
                {
                    productConsultationInfo.ReplyText = (string)reader["ReplyText"];
                }
                if (System.DBNull.Value != reader["ReplyDate"])
                {
                    productConsultationInfo.ReplyDate = new System.DateTime?((System.DateTime)reader["ReplyDate"]);
                }
                if (System.DBNull.Value != reader["ReplyUserId"])
                {
                    productConsultationInfo.ReplyUserId = new int?((int)reader["ReplyUserId"]);
                }
                result = productConsultationInfo;
            }
            return result;
        }
        public static ProductReviewInfo PopulateProductReview(IDataRecord reader)
        {
            ProductReviewInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                result = new ProductReviewInfo
                {
                    ReviewId = (long)reader["ReviewId"],
                    ProductId = (int)reader["ProductId"],
                    UserId = (int)reader["UserId"],
                    ReviewText = (string)reader["ReviewText"],
                    UserName = (string)reader["UserName"],
                    UserEmail = (string)reader["UserEmail"],
                    ReviewDate = (System.DateTime)reader["ReviewDate"],
                    OrderID = (string)reader["OrderId"],
                    SkuId = (string)reader["SkuId"],
                    Score = (int)reader["Score"],
                    IsAnonymous = (bool)reader["IsAnonymous"]
                };
            }
            return result;
        }
        public static ProductInfo PopulateProduct(IDataReader reader)
        {
            ProductInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                ProductInfo productInfo = new ProductInfo();
                productInfo.CategoryId = (int)reader["CategoryId"];
                productInfo.ProductId = (int)reader["ProductId"];
                if (System.DBNull.Value != reader["TypeId"])
                {
                    productInfo.TypeId = new int?((int)reader["TypeId"]);
                }
                productInfo.ProductName = (string)reader["ProductName"];
                if (System.DBNull.Value != reader["ProductCode"])
                {
                    productInfo.ProductCode = (string)reader["ProductCode"];
                }
                if (System.DBNull.Value != reader["ShortDescription"])
                {
                    productInfo.ShortDescription = (string)reader["ShortDescription"];
                }
                if (System.DBNull.Value != reader["Unit"])
                {
                    productInfo.Unit = (string)reader["Unit"];
                }
                if (System.DBNull.Value != reader["Description"])
                {
                    productInfo.Description = (string)reader["Description"];
                }
                if (System.DBNull.Value != reader["MobbileDescription"])
                {
                    productInfo.MobbileDescription = (string)reader["MobbileDescription"];
                }
                productInfo.MobblieDescription = productInfo.MobbileDescription;
                if (System.DBNull.Value != reader["Title"])
                {
                    productInfo.Title = (string)reader["Title"];
                }
                if (System.DBNull.Value != reader["Meta_Description"])
                {
                    productInfo.MetaDescription = (string)reader["Meta_Description"];
                }
                if (System.DBNull.Value != reader["Meta_Keywords"])
                {
                    productInfo.MetaKeywords = (string)reader["Meta_Keywords"];
                }
                productInfo.SaleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
                productInfo.AddedDate = (System.DateTime)reader["AddedDate"];
                productInfo.VistiCounts = (int)reader["VistiCounts"];
                productInfo.SaleCounts = (int)reader["SaleCounts"];
                productInfo.ShowSaleCounts = (int)reader["ShowSaleCounts"];
                productInfo.DisplaySequence = (int)reader["DisplaySequence"];
                if (System.DBNull.Value != reader["ImageUrl1"])
                {
                    productInfo.ImageUrl1 = (string)reader["ImageUrl1"];
                }
                if (System.DBNull.Value != reader["ImageUrl2"])
                {
                    productInfo.ImageUrl2 = (string)reader["ImageUrl2"];
                }
                if (System.DBNull.Value != reader["ImageUrl3"])
                {
                    productInfo.ImageUrl3 = (string)reader["ImageUrl3"];
                }
                if (System.DBNull.Value != reader["ImageUrl4"])
                {
                    productInfo.ImageUrl4 = (string)reader["ImageUrl4"];
                }
                if (System.DBNull.Value != reader["ImageUrl5"])
                {
                    productInfo.ImageUrl5 = (string)reader["ImageUrl5"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl40"])
                {
                    productInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl60"])
                {
                    productInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl100"])
                {
                    productInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl160"])
                {
                    productInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl180"])
                {
                    productInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl220"])
                {
                    productInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl310"])
                {
                    productInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl410"])
                {
                    productInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
                }
                if (System.DBNull.Value != reader["MarketPrice"])
                {
                    productInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
                }
                if (System.DBNull.Value != reader["BrandId"])
                {
                    productInfo.BrandId = new int?((int)reader["BrandId"]);
                }
                if (reader["MainCategoryPath"] != System.DBNull.Value)
                {
                    productInfo.MainCategoryPath = (string)reader["MainCategoryPath"];
                }
                if (reader["ExtendCategoryPath"] != System.DBNull.Value)
                {
                    productInfo.ExtendCategoryPath = (string)reader["ExtendCategoryPath"];
                }
                productInfo.HasSKU = (bool)reader["HasSKU"];
                if (reader["IsApproved"] != System.DBNull.Value)
                {
                    productInfo.IsApproved = (bool)reader["IsApproved"];
                }
                else
                {
                    productInfo.IsApproved = false;
                }
                if (reader["TaobaoProductId"] != System.DBNull.Value)
                {
                    productInfo.TaobaoProductId = (long)reader["TaobaoProductId"];
                }
                if (reader["IsfreeShipping"] != System.DBNull.Value)
                {
                    productInfo.IsfreeShipping = (bool)reader["IsfreeShipping"];
                }
                if (reader["ReferralDeduct"] != System.DBNull.Value)
                {
                    productInfo.ReferralDeduct = new decimal?((decimal)reader["ReferralDeduct"]);
                }
                if (reader["SubMemberDeduct"] != System.DBNull.Value)
                {
                    productInfo.SubMemberDeduct = new decimal?((decimal)reader["SubMemberDeduct"]);
                }
                if (reader["SubReferralDeduct"] != System.DBNull.Value)
                {
                    productInfo.SubReferralDeduct = new decimal?((decimal)reader["SubReferralDeduct"]);
                }
                if (reader["TaxRate"] != System.DBNull.Value)
                {
                    if (reader["TaxRate"].ToString() == "0")
                    {
                        productInfo.TaxRate = 0M;
                    }
                    else
                    {
                        productInfo.TaxRate = (decimal)reader["TaxRate"];
                    }
                }
                if (reader["MinTaxRate"] != System.DBNull.Value)
                {
                    if (reader["MinTaxRate"].ToString() == "0")
                    {
                        productInfo.MinTaxRate = 0M;
                    }
                    else
                    {
                        productInfo.MinTaxRate = (decimal)reader["MinTaxRate"];
                    }
                }
                if (reader["MaxTaxRate"] != System.DBNull.Value)
                {
                    if (reader["MaxTaxRate"].ToString() == "0")
                    {
                        productInfo.MaxTaxRate = 0M;
                    }
                    else
                    {
                        productInfo.MaxTaxRate = (decimal)reader["MaxTaxRate"];
                    }
                }

                if (reader["TemplateName"] != System.DBNull.Value)
                {
                    productInfo.TemplateName = (string)reader["TemplateName"];
                }
                if (reader["QRcode"] != System.DBNull.Value)
                {
                    productInfo.QRcode = reader["QRcode"].ToString();
                }
                if (reader["BuyCardinality"] != System.DBNull.Value)
                {
                    productInfo.BuyCardinality = (int)reader["BuyCardinality"];
                }
                productInfo.TemplateId = reader["TemplateId"] != System.DBNull.Value ? int.Parse(reader["TemplateId"].ToString()) : 0;
                if (reader["SupplierId"] != System.DBNull.Value)
                {
                    productInfo.SupplierId = new int?((int)reader["SupplierId"]);
                }

                if (reader["ProductTitle"] != System.DBNull.Value)
                {
                    productInfo.ProductTitle = (string)reader["ProductTitle"];
                }

                //是否显示优惠
                if (reader["IsPromotion"] != System.DBNull.Value)
                {
                    productInfo.IsPromotion = (bool)reader["IsPromotion"];
                }
                else
                {
                    productInfo.IsPromotion = false;
                }

                //是否显示折扣
                if (reader["IsDisplayDiscount"] != System.DBNull.Value)
                {
                    productInfo.IsDisplayDiscount = (bool)reader["IsDisplayDiscount"];
                }
                else
                {
                    productInfo.IsDisplayDiscount = false;
                }

                //是否是组合商品 1.代表不是 2.代表是
                if (reader["SaleType"] != System.DBNull.Value)
                {
                    productInfo.SaleType = (int)reader["SaleType"];
                }
                else
                {
                    productInfo.SaleType = 1;
                }

                
                result = productInfo;
            }
            return result;
        }
        public static ProductInfo PopulateSubProduct(IDataReader reader)
        {
            ProductInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                ProductInfo productInfo = new ProductInfo();
                productInfo.CategoryId = (int)reader["CategoryId"];
                productInfo.ProductId = (int)reader["ProductId"];
                if (System.DBNull.Value != reader["TypeId"])
                {
                    productInfo.TypeId = new int?((int)reader["TypeId"]);
                }
                productInfo.ProductName = (string)reader["ProductName"];
                if (System.DBNull.Value != reader["ProductCode"])
                {
                    productInfo.ProductCode = (string)reader["ProductCode"];
                }
                if (System.DBNull.Value != reader["ShortDescription"])
                {
                    productInfo.ShortDescription = (string)reader["ShortDescription"];
                }
                if (System.DBNull.Value != reader["Description"])
                {
                    productInfo.Description = (string)reader["Description"];
                }
                if (System.DBNull.Value != reader["MobbileDescription"])
                {
                    productInfo.MobblieDescription = (string)reader["MobbileDescription"];
                }
                if (System.DBNull.Value != reader["Title"])
                {
                    productInfo.Title = (string)reader["Title"];
                }
                if (System.DBNull.Value != reader["Meta_Description"])
                {
                    productInfo.MetaDescription = (string)reader["Meta_Description"];
                }
                if (System.DBNull.Value != reader["Meta_Keywords"])
                {
                    productInfo.MetaKeywords = (string)reader["Meta_Keywords"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl40"])
                {
                    productInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl60"])
                {
                    productInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl100"])
                {
                    productInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl160"])
                {
                    productInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl180"])
                {
                    productInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl220"])
                {
                    productInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl310"])
                {
                    productInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl410"])
                {
                    productInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
                }
                if (System.DBNull.Value != reader["MarketPrice"])
                {
                    productInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
                }
                if (System.DBNull.Value != reader["BrandId"])
                {
                    productInfo.BrandId = new int?((int)reader["BrandId"]);
                }
                productInfo.SaleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
                productInfo.VistiCounts = (int)reader["VistiCounts"];
                productInfo.DisplaySequence = (int)reader["DisplaySequence"];
                result = productInfo;
            }
            return result;
        }
        public static CategoryInfo ConvertDataRowToProductCategory(DataRow row)
        {
            CategoryInfo categoryInfo = new CategoryInfo();
            categoryInfo.CategoryId = (int)row["CategoryId"];
            categoryInfo.Name = (string)row["Name"];
            categoryInfo.DisplaySequence = (int)row["DisplaySequence"];
            if (row["ParentCategoryId"] != System.DBNull.Value)
            {
                categoryInfo.ParentCategoryId = new int?((int)row["ParentCategoryId"]);
            }
            if (row["Icon"] != System.DBNull.Value)
            {
                categoryInfo.Icon = (string)row["Icon"];
            }
            categoryInfo.Depth = (int)row["Depth"];
            categoryInfo.Path = (string)row["Path"];
            if (row["RewriteName"] != System.DBNull.Value)
            {
                categoryInfo.RewriteName = (string)row["RewriteName"];
            }
            if (row["Theme"] != System.DBNull.Value)
            {
                categoryInfo.Theme = (string)row["Theme"];
            }
            categoryInfo.HasChildren = (bool)row["HasChildren"];
            return categoryInfo;
        }
        public static CategoryPartInfo ConvertDataRowToPartCategory(DataRow row)
        {
            CategoryPartInfo categoryInfo = new CategoryPartInfo();
            categoryInfo.CategoryId = (int)row["CategoryId"];
            categoryInfo.Name = (string)row["Name"];
            categoryInfo.DisplaySequence = (int)row["DisplaySequence"];
            categoryInfo.FoolTag = (int)row["FoolTag"];
            return categoryInfo;
        }
        public static ProductTypeInfo PopulateType(IDataReader reader)
        {
            ProductTypeInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                ProductTypeInfo productTypeInfo = new ProductTypeInfo();
                productTypeInfo.TypeId = (int)reader["TypeId"];
                productTypeInfo.TypeName = (string)reader["TypeName"];
                if (reader["Remark"] != System.DBNull.Value)
                {
                    productTypeInfo.Remark = (string)reader["Remark"];
                }
                result = productTypeInfo;
            }
            return result;
        }
        public static SKUItem PopulateSKU(IDataReader reader)
        {
            SKUItem result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                SKUItem sKUItem = new SKUItem();
                sKUItem.SkuId = (string)reader["SkuId"];
                sKUItem.ProductId = (int)reader["ProductId"];
                if (reader["SKU"] != System.DBNull.Value)
                {
                    sKUItem.SKU = (string)reader["SKU"];
                }
                if (reader["Weight"] != System.DBNull.Value)
                {
                    sKUItem.Weight = (decimal)reader["Weight"];
                }
                sKUItem.Stock = (int)reader["Stock"];
                if (reader["FactStock"] != System.DBNull.Value)
                {
                    sKUItem.FactStock = (int)reader["FactStock"];
                }
                if (reader["CostPrice"] != System.DBNull.Value)
                {
                    sKUItem.CostPrice = (decimal)reader["CostPrice"];
                }
                sKUItem.SalePrice = (decimal)reader["SalePrice"];
                if (reader["DeductFee"] != System.DBNull.Value)
                {
                    sKUItem.DeductFee = (decimal)reader["DeductFee"];
                }
                //reader.GetSchemaTable().Columns.Contains(attr.FieldName);
                if (reader.GetSchemaTable().Select("ColumnName='ProductRegistrationNumber'").Length > 0 && reader["ProductRegistrationNumber"] != null && reader["ProductRegistrationNumber"] != System.DBNull.Value)
                {
                    sKUItem.ProductRegistrationNumber = (string)reader["ProductRegistrationNumber"];
                }
                if (reader.GetSchemaTable().Select("ColumnName='LJNo'").Length > 0 && reader["LJNo"] != null && reader["LJNo"] != System.DBNull.Value)
                {
                    sKUItem.LJNo = (string)reader["LJNo"];
                }
                
                result = sKUItem;
            }
            return result;
        }


        public static SKUItem ExtendPopulateSKU(IDataReader reader)
        {
            SKUItem result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                SKUItem sKUItem = new SKUItem();
                sKUItem.SkuId = (string)reader["SkuId"];
                sKUItem.ProductId = (int)reader["ProductId"];
                if (reader["SKU"] != System.DBNull.Value)
                {
                    sKUItem.SKU = (string)reader["SKU"];
                }
                if (reader["Weight"] != System.DBNull.Value)
                {
                    sKUItem.Weight = (decimal)reader["Weight"];
                }
                sKUItem.Stock = (int)reader["Stock"];
                if (reader["FactStock"] != System.DBNull.Value)
                {
                    sKUItem.FactStock = (int)reader["FactStock"];
                }
                if (reader["CostPrice"] != System.DBNull.Value)
                {
                    sKUItem.CostPrice = (decimal)reader["CostPrice"];
                }
                sKUItem.SalePrice = (decimal)reader["SalePrice"];
                if (reader["DeductFee"] != System.DBNull.Value)
                {
                    sKUItem.DeductFee = (decimal)reader["DeductFee"];
                }
                //reader.GetSchemaTable().Columns.Contains(attr.FieldName);
                if (reader.GetSchemaTable().Select("ColumnName='ProductRegistrationNumber'").Length > 0 && reader["ProductRegistrationNumber"] != null && reader["ProductRegistrationNumber"] != System.DBNull.Value)
                {
                    sKUItem.ProductRegistrationNumber = (string)reader["ProductRegistrationNumber"];
                }
                if (reader.GetSchemaTable().Select("ColumnName='LJNo'").Length > 0 && reader["LJNo"] != null && reader["LJNo"] != System.DBNull.Value)
                {
                    sKUItem.LJNo = (string)reader["LJNo"];
                }

                if (reader["WMSStock"] != System.DBNull.Value)
                {
                    sKUItem.WMSStock = (int)reader["WMSStock"];
                }

                if (reader["GrossWeight"] != System.DBNull.Value)
                {
                    sKUItem.GrossWeight = (decimal)reader["GrossWeight"];
                }

                result = sKUItem;
            }
            return result;
        }


        public static AttributeValueInfo PopulateAttributeValue(IDataReader reader)
        {
            AttributeValueInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                result = new AttributeValueInfo
                {
                    ValueId = (int)reader["ValueId"],
                    AttributeId = (int)reader["AttributeId"],
                    ValueStr = (string)reader["ValueStr"]
                };
            }
            return result;
        }
        public static CategoryInfo PopulateProductCategory(IDataRecord reader)
        {
            CategoryInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                CategoryInfo categoryInfo = new CategoryInfo();
                categoryInfo.CategoryId = (int)reader["CategoryId"];
                categoryInfo.Name = (string)reader["Name"];
                categoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
                if (reader["AssociatedProductType"] != System.DBNull.Value)
                {
                    categoryInfo.AssociatedProductType = new int?((int)reader["AssociatedProductType"]);
                }
                if (reader["Meta_Description"] != System.DBNull.Value)
                {
                    categoryInfo.MetaDescription = (string)reader["Meta_Description"];
                }
                if (reader["Meta_Keywords"] != System.DBNull.Value)
                {
                    categoryInfo.MetaKeywords = (string)reader["Meta_Keywords"];
                }
                if (reader["Notes1"] != System.DBNull.Value)
                {
                    categoryInfo.Notes1 = (string)reader["Notes1"];
                }
                if (reader["Notes2"] != System.DBNull.Value)
                {
                    categoryInfo.Notes2 = (string)reader["Notes2"];
                }
                if (reader["Notes3"] != System.DBNull.Value)
                {
                    categoryInfo.Notes3 = (string)reader["Notes3"];
                }
                if (reader["Notes4"] != System.DBNull.Value)
                {
                    categoryInfo.Notes4 = (string)reader["Notes4"];
                }
                if (reader["Notes5"] != System.DBNull.Value)
                {
                    categoryInfo.Notes5 = (string)reader["Notes5"];
                }
                if (reader["Icon"] != System.DBNull.Value)
                {
                    categoryInfo.Icon = (string)reader["Icon"];
                }
                if (reader["ParentCategoryId"] != System.DBNull.Value)
                {
                    categoryInfo.ParentCategoryId = new int?((int)reader["ParentCategoryId"]);
                }
                categoryInfo.Depth = (int)reader["Depth"];
                categoryInfo.Path = (string)reader["Path"];
                if (reader["Meta_Title"] != System.DBNull.Value)
                {
                    categoryInfo.MetaTitle = (string)reader["Meta_Title"];
                }
                if (reader["RewriteName"] != System.DBNull.Value)
                {
                    categoryInfo.RewriteName = (string)reader["RewriteName"];
                }
                if (reader["SKUPrefix"] != System.DBNull.Value)
                {
                    categoryInfo.SKUPrefix = (string)reader["SKUPrefix"];
                }
                if (reader["Theme"] != System.DBNull.Value)
                {
                    categoryInfo.Theme = (string)reader["Theme"];
                }
                categoryInfo.HasChildren = (bool)reader["HasChildren"];
                result = categoryInfo;
            }
            return result;
        }
        public static BrandCategoryInfo PopulateBrandCategory(IDataRecord reader)
        {
            BrandCategoryInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
                brandCategoryInfo.BrandId = (int)reader["BrandId"];
                brandCategoryInfo.BrandName = (string)reader["BrandName"];
                if (reader["DisplaySequence"] != System.DBNull.Value)
                {
                    brandCategoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
                }
                if (reader["Logo"] != System.DBNull.Value)
                {
                    brandCategoryInfo.Logo = (string)reader["Logo"];
                }
                if (reader["CompanyUrl"] != System.DBNull.Value)
                {
                    brandCategoryInfo.CompanyUrl = (string)reader["CompanyUrl"];
                }
                if (reader["RewriteName"] != System.DBNull.Value)
                {
                    brandCategoryInfo.RewriteName = (string)reader["RewriteName"];
                }
                if (reader["MetaKeywords"] != System.DBNull.Value)
                {
                    brandCategoryInfo.MetaKeywords = (string)reader["MetaKeywords"];
                }
                if (reader["MetaDescription"] != System.DBNull.Value)
                {
                    brandCategoryInfo.MetaDescription = (string)reader["MetaDescription"];
                }
                if (reader["Description"] != System.DBNull.Value)
                {
                    brandCategoryInfo.Description = (string)reader["Description"];
                }
                if (reader["Theme"] != System.DBNull.Value)
                {
                    brandCategoryInfo.Theme = (string)reader["Theme"];
                }
                result = brandCategoryInfo;
            }
            return result;
        }
        public static InpourRequestInfo PopulateInpourRequest(IDataReader reader)
        {
            InpourRequestInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                result = new InpourRequestInfo
                {
                    InpourId = (string)reader["InpourId"],
                    TradeDate = (System.DateTime)reader["TradeDate"],
                    UserId = (int)reader["UserId"],
                    PaymentId = (int)reader["PaymentId"],
                    InpourBlance = (decimal)reader["InpourBlance"]
                };
            }
            return result;
        }
        public static AccountSummaryInfo PopulateAccountSummary(IDataRecord reader)
        {
            AccountSummaryInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                AccountSummaryInfo accountSummaryInfo = new AccountSummaryInfo();
                if (reader["AccountAmount"] != System.DBNull.Value)
                {
                    accountSummaryInfo.AccountAmount = (decimal)reader["AccountAmount"];
                }
                if (reader["FreezeBalance"] != System.DBNull.Value)
                {
                    accountSummaryInfo.FreezeBalance = (decimal)reader["FreezeBalance"];
                }
                accountSummaryInfo.UseableBalance = accountSummaryInfo.AccountAmount - accountSummaryInfo.FreezeBalance;
                result = accountSummaryInfo;
            }
            return result;
        }
        public static CouponInfo PopulateCoupon(IDataReader reader)
        {
            CouponInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                CouponInfo couponInfo = new CouponInfo();
                couponInfo.CouponId = (int)reader["CouponId"];
                couponInfo.Name = (string)reader["Name"];
                couponInfo.StartTime = (System.DateTime)reader["StartTime"];
                couponInfo.ClosingTime = (System.DateTime)reader["ClosingTime"];
                if (reader["Description"] != System.DBNull.Value)
                {
                    couponInfo.Description = (string)reader["Description"];
                }
                if (reader["Amount"] != System.DBNull.Value)
                {
                    couponInfo.Amount = new decimal?((decimal)reader["Amount"]);
                }
                couponInfo.DiscountValue = (decimal)reader["DiscountValue"];
                couponInfo.SentCount = (int)reader["SentCount"];
                couponInfo.UsedCount = (int)reader["UsedCount"];
                couponInfo.NeedPoint = (int)reader["NeedPoint"];
                result = couponInfo;
            }
            return result;
        }
        public static GroupBuyInfo PopulateGroupBuy(IDataReader reader)
        {
            GroupBuyInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                GroupBuyInfo groupBuyInfo = new GroupBuyInfo();
                groupBuyInfo.GroupBuyId = (int)reader["GroupBuyId"];
                groupBuyInfo.ProductId = (int)reader["ProductId"];
                if (System.DBNull.Value != reader["NeedPrice"])
                {
                    groupBuyInfo.NeedPrice = (decimal)reader["NeedPrice"];
                }
                groupBuyInfo.MaxCount = (int)reader["MaxCount"];
                groupBuyInfo.StartDate = (System.DateTime)reader["StartDate"];
                groupBuyInfo.EndDate = (System.DateTime)reader["EndDate"];
                if (System.DBNull.Value != reader["Content"])
                {
                    groupBuyInfo.Content = (string)reader["Content"];
                }
                groupBuyInfo.Status = (GroupBuyStatus)((int)reader["Status"]);
                try
                {
                    if (reader["ProdcutQuantity"] != System.DBNull.Value)
                    {
                        groupBuyInfo.ProdcutQuantity = (int)reader["ProdcutQuantity"];
                    }
                }
                catch
                {
                }
                try
                {
                    groupBuyInfo.Price = ((reader["Price"] == System.DBNull.Value) ? 0m : ((decimal)reader["Price"]));
                }
                catch
                {
                }
                result = groupBuyInfo;
            }
            return result;
        }
        public static BundlingInfo PopulateBindInfo(IDataReader reader)
        {
            BundlingInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                BundlingInfo bundlingInfo = new BundlingInfo();
                bundlingInfo.BundlingID = (int)reader["BundlingID"];
                bundlingInfo.Name = (string)reader["Name"];
                if (System.DBNull.Value != reader["price"])
                {
                    bundlingInfo.Price = (decimal)reader["price"];
                }
                bundlingInfo.Num = (int)reader["Num"];
                bundlingInfo.AddTime = (System.DateTime)reader["AddTime"];
                if (System.DBNull.Value != reader["ShortDescription"])
                {
                    bundlingInfo.ShortDescription = (string)reader["ShortDescription"];
                }
                bundlingInfo.SaleStatus = (int)reader["SaleStatus"];
                if (System.DBNull.Value != reader["DisplaySequence"])
                {
                    bundlingInfo.DisplaySequence = (int)reader["DisplaySequence"];
                }
                result = bundlingInfo;
            }
            return result;
        }
        public static CountDownInfo PopulateCountDown(IDataReader reader)
        {
            CountDownInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                CountDownInfo countDownInfo = new CountDownInfo();
                countDownInfo.CountDownId = (int)reader["CountDownId"];
                countDownInfo.ProductId = (int)reader["ProductId"];
                countDownInfo.StartDate = (System.DateTime)reader["StartDate"];
                countDownInfo.EndDate = (System.DateTime)reader["EndDate"];
                if (System.DBNull.Value != reader["Content"])
                {
                    countDownInfo.Content = (string)reader["Content"];
                }
                if (System.DBNull.Value != reader["CountDownPrice"])
                {
                    countDownInfo.CountDownPrice = (decimal)reader["CountDownPrice"];
                }
                if (System.DBNull.Value != reader["MaxCount"])
                {
                    countDownInfo.MaxCount = (int)reader["MaxCount"];
                }
                if (System.DBNull.Value != reader["PlanCount"])
                {
                    countDownInfo.PlanCount = (int)reader["PlanCount"];
                }
                result = countDownInfo;
            }
            return result;
        }

        public static CountDownCategoriesInfo PopulateCountDownCategories(IDataReader reader)
        {
            CountDownCategoriesInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                CountDownCategoriesInfo countDownInfo = new CountDownCategoriesInfo();
                countDownInfo.CountDownCategoryId = (int)reader["CountDownCategoryId"];
                countDownInfo.Title = (string)reader["Title"];
                countDownInfo.StartTime = DateTime.ParseExact(reader["StartTime"].ToString(), "HH:mm:ss", null);// (System.DateTime)reader["StartTime"];
                countDownInfo.EndTime = DateTime.ParseExact(reader["EndTime"].ToString(), "HH:mm:ss", null);// (System.DateTime)reader["EndTime"];

                if (System.DBNull.Value != reader["AdImageUrl"])
                {
                    countDownInfo.AdImageUrl = (string)reader["AdImageUrl"];
                }
                if (System.DBNull.Value != reader["AdImageLinkUrl"])
                {
                    countDownInfo.AdImageLinkUrl = (string)reader["AdImageLinkUrl"];
                }
                if (System.DBNull.Value != reader["DisplaySequence"])
                {
                    countDownInfo.DisplaySequence = (int)reader["DisplaySequence"];
                }
                if (System.DBNull.Value != reader["CreatedBy"])
                {
                    countDownInfo.CreatedBy = (int)reader["CreatedBy"];
                }
                if (System.DBNull.Value != reader["CreatedOn"])
                {
                    countDownInfo.CreatedOn = (System.DateTime)reader["CreatedOn"];
                }

                if (System.DBNull.Value != reader["UpdatedBy"])
                {
                    countDownInfo.UpdatedBy = (int)reader["UpdatedBy"];
                }
                if (System.DBNull.Value != reader["UpdatedOn"])
                {
                    countDownInfo.UpdatedOn = (System.DateTime)reader["UpdatedOn"];
                }

                result = countDownInfo;
            }
            return result;
        }

        public static CouponItemInfo PopulateCouponItem(IDataReader reader)
        {
            CouponItemInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                CouponItemInfo couponItemInfo = new CouponItemInfo();
                couponItemInfo.CouponId = (int)reader["CouponId"];
                couponItemInfo.ClaimCode = (string)reader["ClaimCode"];
                couponItemInfo.GenerateTime = (System.DateTime)reader["GenerateTime"];
                if (reader["UserId"] != System.DBNull.Value)
                {
                    couponItemInfo.UserId = new int?((int)reader["UserId"]);
                }
                if (reader["EmailAddress"] != System.DBNull.Value)
                {
                    couponItemInfo.EmailAddress = (string)reader["EmailAddress"];
                }
                result = couponItemInfo;
            }
            return result;
        }
        public static GiftInfo PopulateGift(IDataReader reader)
        {
            GiftInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                GiftInfo giftInfo = new GiftInfo();
                giftInfo.GiftId = (int)reader["GiftId"];
                giftInfo.Name = ((System.DBNull.Value == reader["Name"]) ? null : ((string)reader["Name"]));
                giftInfo.ShortDescription = ((System.DBNull.Value == reader["ShortDescription"]) ? null : ((string)reader["ShortDescription"]));
                giftInfo.Unit = ((System.DBNull.Value == reader["Unit"]) ? null : ((string)reader["Unit"]));
                giftInfo.LongDescription = ((System.DBNull.Value == reader["LongDescription"]) ? null : ((string)reader["LongDescription"]));
                giftInfo.Title = ((System.DBNull.Value == reader["Title"]) ? null : ((string)reader["Title"]));
                giftInfo.Meta_Description = ((System.DBNull.Value == reader["Meta_Description"]) ? null : ((string)reader["Meta_Description"]));
                giftInfo.Meta_Keywords = ((System.DBNull.Value == reader["Meta_Keywords"]) ? null : ((string)reader["Meta_Keywords"]));
                if (System.DBNull.Value != reader["CostPrice"])
                {
                    giftInfo.CostPrice = new decimal?((decimal)reader["CostPrice"]);
                }
                if (System.DBNull.Value != reader["ImageUrl"])
                {
                    giftInfo.ImageUrl = (string)reader["ImageUrl"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl40"])
                {
                    giftInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl60"])
                {
                    giftInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl100"])
                {
                    giftInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl160"])
                {
                    giftInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl180"])
                {
                    giftInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl220"])
                {
                    giftInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl310"])
                {
                    giftInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
                }
                if (System.DBNull.Value != reader["ThumbnailUrl410"])
                {
                    giftInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
                }
                if (System.DBNull.Value != reader["MarketPrice"])
                {
                    giftInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
                }
                giftInfo.NeedPoint = (int)reader["NeedPoint"];
                giftInfo.IsPromotion = (bool)reader["IsPromotion"];
                result = giftInfo;
            }
            return result;
        }
        public static PromotionInfo PopulatePromote(IDataRecord reader)
        {
            PromotionInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                PromotionInfo promotionInfo = new PromotionInfo();
                promotionInfo.ActivityId = (int)reader["ActivityId"];
                promotionInfo.Name = (string)reader["Name"];
                promotionInfo.PromoteType = (PromoteType)reader["PromoteType"];
                promotionInfo.Condition = (decimal)reader["Condition"];
                promotionInfo.DiscountValue = (decimal)reader["DiscountValue"];
                promotionInfo.StartDate = (System.DateTime)reader["StartDate"];
                promotionInfo.EndDate = (System.DateTime)reader["EndDate"];
                if (System.DBNull.Value != reader["Description"])
                {
                    promotionInfo.Description = (string)reader["Description"];
                }
                if (System.DBNull.Value != reader["IsAscend"])
                {
                    promotionInfo.IsAscend = (int)reader["IsAscend"];
                }
                result = promotionInfo;
            }
            return result;
        }
        public static PCMenuInfo PcMenuInfoMapper(IDataRecord reader)
        {
            PCMenuInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                PCMenuInfo menuInfo = new PCMenuInfo();
                int ParentId = -1;
                int Status = -1;
                int IsMenu = -1;
                int levelId = -1;
                int SortValue = -1;
                int.TryParse(reader["ParentId"].ToString(), out ParentId);
                int.TryParse(reader["Status"].ToString(), out Status);
                int.TryParse(reader["IsMenu"].ToString(), out IsMenu);
                int.TryParse(reader["levelId"].ToString(), out levelId);
                int.TryParse(reader["SortValue"].ToString(), out SortValue);


                menuInfo.PrivilegeId = (int)reader["PrivilegeId"];
                menuInfo.PrivilegeName = reader["PrivilegeName"] != null ? (string)reader["PrivilegeName"] : "";
                menuInfo.PrivilegeENName = reader["PrivilegeName"] != null ? (string)reader["PrivilegeENName"] : "";
                menuInfo.ParentId = ParentId;
                menuInfo.SortValue = SortValue;
                menuInfo.Status = Status;
                menuInfo.LinkUrl = reader["LinkUrl"] != null ? reader["LinkUrl"].ToString() : "";
                menuInfo.IsMenu = IsMenu;
                menuInfo.levelId = levelId;

                result = menuInfo;
            }
            return result;
        }
        public static PCMenuInfo PcMenuInfoCheckedMapper(IDataRecord reader)
        {
            PCMenuInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                PCMenuInfo menuInfo = new PCMenuInfo();
                int ParentId = -1;
                int Status = -1;
                int IsMenu = -1;
                int levelId = -1;
                int SortValue = -1;
                int isChecked = 0;
                int.TryParse(reader["ParentId"].ToString(), out ParentId);
                int.TryParse(reader["Status"].ToString(), out Status);
                int.TryParse(reader["IsMenu"].ToString(), out IsMenu);
                int.TryParse(reader["levelId"].ToString(), out levelId);
                int.TryParse(reader["SortValue"].ToString(), out SortValue);
                int.TryParse(reader["checked"].ToString(), out isChecked);

                menuInfo.PrivilegeId = (int)reader["PrivilegeId"];
                menuInfo.PrivilegeName = reader["PrivilegeName"] != null ? (string)reader["PrivilegeName"] : "";
                menuInfo.PrivilegeENName = reader["PrivilegeName"] != null ? (string)reader["PrivilegeENName"] : "";
                menuInfo.ParentId = ParentId;
                menuInfo.SortValue = SortValue;
                menuInfo.Status = Status;
                menuInfo.LinkUrl = reader["LinkUrl"] != null ? reader["LinkUrl"].ToString() : "";
                menuInfo.IsMenu = IsMenu;
                menuInfo.levelId = levelId;
                menuInfo.IsChecked = isChecked;
                menuInfo.newPrivilegeId = reader["newPrivilegeId"] != null ? reader["newPrivilegeId"].ToString() : "";
                result = menuInfo;
            }
            return result;
        }
        public static CountDownInfo PopulateCountDown(IDataRecord reader)
        {
            CountDownInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                CountDownInfo countDownInfo = new CountDownInfo();
                countDownInfo.CountDownId = (int)reader["CountDownId"];
                countDownInfo.ProductId = (int)reader["ProductId"];
                if (System.DBNull.Value != reader["CountDownPrice"])
                {
                    countDownInfo.CountDownPrice = (decimal)reader["CountDownPrice"];
                }
                countDownInfo.StartDate = (System.DateTime)reader["StartDate"];
                countDownInfo.EndDate = (System.DateTime)reader["EndDate"];
                if (System.DBNull.Value != reader["Content"])
                {
                    countDownInfo.Content = (string)reader["Content"];
                }
                countDownInfo.DisplaySequence = (int)reader["DisplaySequence"];
                if (System.DBNull.Value != reader["MaxCount"])
                {
                    countDownInfo.MaxCount = (int)reader["MaxCount"];
                }
                result = countDownInfo;
            }
            return result;
        }
        public static ShippersInfo PopulateShipper(IDataRecord reader)
        {
            ShippersInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ShippersInfo shippersInfo = new ShippersInfo();
                shippersInfo.ShipperId = (int)reader["ShipperId"];
                shippersInfo.DistributorUserId = (int)reader["DistributorUserId"];
                shippersInfo.IsDefault = (bool)reader["IsDefault"];
                shippersInfo.ShipperTag = (string)reader["ShipperTag"];
                shippersInfo.ShipperName = (string)reader["ShipperName"];
                shippersInfo.RegionId = (int)reader["RegionId"];
                shippersInfo.Address = (string)reader["Address"];
                if (reader["CellPhone"] != System.DBNull.Value)
                {
                    shippersInfo.CellPhone = (string)reader["CellPhone"];
                }
                if (reader["TelPhone"] != System.DBNull.Value)
                {
                    shippersInfo.TelPhone = (string)reader["TelPhone"];
                }
                if (reader["Zipcode"] != System.DBNull.Value)
                {
                    shippersInfo.Zipcode = (string)reader["Zipcode"];
                }
                if (reader["Remark"] != System.DBNull.Value)
                {
                    shippersInfo.Remark = (string)reader["Remark"];
                }
                result = shippersInfo;
            }
            return result;
        }
        public static PaymentModeInfo PopulatePayment(IDataRecord reader)
        {
            PaymentModeInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                PaymentModeInfo paymentModeInfo = new PaymentModeInfo
                {
                    ModeId = (int)reader["ModeId"],
                    Name = (string)reader["Name"],
                    DisplaySequence = (int)reader["DisplaySequence"],
                    IsUseInpour = (bool)reader["IsUseInpour"],
                    Charge = (decimal)reader["Charge"],
                    IsPercent = (bool)reader["IsPercent"],
                    ApplicationType = (PayApplicationType)reader["ApplicationType"]
                };
                if (reader["Description"] != System.DBNull.Value)
                {
                    paymentModeInfo.Description = (string)reader["Description"];
                }
                if (reader["Gateway"] != System.DBNull.Value)
                {
                    paymentModeInfo.Gateway = (string)reader["Gateway"];
                }
                if (reader["Settings"] != System.DBNull.Value)
                {
                    paymentModeInfo.Settings = (string)reader["Settings"];
                }
                result = paymentModeInfo;
            }
            return result;
        }
        public static ShippingModeInfo PopulateShippingMode(IDataRecord reader)
        {
            ShippingModeInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
                if (reader["ModeId"] != System.DBNull.Value)
                {
                    shippingModeInfo.ModeId = (int)reader["ModeId"];
                }
                if (reader["TemplateId"] != System.DBNull.Value)
                {
                    shippingModeInfo.TemplateId = (int)reader["TemplateId"];
                }
                shippingModeInfo.Name = (string)reader["Name"];
                shippingModeInfo.TemplateName = (string)reader["TemplateName"];
                if (reader["Weight"] != System.DBNull.Value)
                {
                    shippingModeInfo.Weight = (decimal)reader["Weight"];
                }
                if (System.DBNull.Value != reader["AddWeight"])
                {
                    shippingModeInfo.AddWeight = new decimal?((decimal)reader["AddWeight"]);
                }
                if (reader["Price"] != System.DBNull.Value)
                {
                    shippingModeInfo.Price = (decimal)reader["Price"];
                }
                if (System.DBNull.Value != reader["AddPrice"])
                {
                    shippingModeInfo.AddPrice = new decimal?((decimal)reader["AddPrice"]);
                }
                if (reader["Description"] != System.DBNull.Value)
                {
                    shippingModeInfo.Description = (string)reader["Description"];
                }
                shippingModeInfo.DisplaySequence = (int)reader["DisplaySequence"];
                result = shippingModeInfo;
            }
            return result;
        }
        public static ShippingModeInfo PopulateShippingTemplate(IDataRecord reader)
        {
            ShippingModeInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
                if (reader["TemplateId"] != System.DBNull.Value)
                {
                    shippingModeInfo.TemplateId = (int)reader["TemplateId"];
                }
                shippingModeInfo.Name = (string)reader["TemplateName"];
                shippingModeInfo.Weight = (decimal)reader["Weight"];
                if (System.DBNull.Value != reader["AddWeight"])
                {
                    shippingModeInfo.AddWeight = new decimal?((decimal)reader["AddWeight"]);
                }
                shippingModeInfo.Price = (decimal)reader["Price"];
                if (System.DBNull.Value != reader["AddPrice"])
                {
                    shippingModeInfo.AddPrice = new decimal?((decimal)reader["AddPrice"]);
                }
                result = shippingModeInfo;
            }
            return result;
        }
        public static ShippingModeGroupInfo PopulateShippingModeGroup(IDataRecord reader)
        {
            ShippingModeGroupInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ShippingModeGroupInfo shippingModeGroupInfo = new ShippingModeGroupInfo();
                shippingModeGroupInfo.TemplateId = (int)reader["TemplateId"];
                shippingModeGroupInfo.GroupId = (int)reader["GroupId"];
                shippingModeGroupInfo.Price = (decimal)reader["Price"];
                if (System.DBNull.Value != reader["AddPrice"])
                {
                    shippingModeGroupInfo.AddPrice = (decimal)reader["AddPrice"];
                }
                result = shippingModeGroupInfo;
            }
            return result;
        }
        public static ShippingRegionInfo PopulateShippingRegion(IDataRecord reader)
        {
            ShippingRegionInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                result = new ShippingRegionInfo
                {
                    TemplateId = (int)reader["TemplateId"],
                    GroupId = (int)reader["GroupId"],
                    RegionId = (int)reader["RegionId"]
                };
            }
            return result;
        }
        public static OrderInfo PopulateOrder(IDataRecord reader)
        {
            OrderInfo result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                OrderInfo orderInfo = new OrderInfo();
                orderInfo.OrderId = (string)reader["OrderId"];
                if (System.DBNull.Value != reader["GatewayOrderId"])
                {
                    orderInfo.GatewayOrderId = (string)reader["GatewayOrderId"];
                }
                if (System.DBNull.Value != reader["Remark"])
                {
                    orderInfo.Remark = (string)reader["Remark"];
                }
                if (System.DBNull.Value != reader["ManagerMark"])
                {
                    orderInfo.ManagerMark = new OrderMark?((OrderMark)reader["ManagerMark"]);
                }
                if (System.DBNull.Value != reader["RemarkPeople"])
                {
                    orderInfo.RemarkPeople = reader["RemarkPeople"].ToString();
                }
                if (System.DBNull.Value != reader["RemarkTime"])
                {
                    orderInfo.RemarkTime = (DateTime)reader["RemarkTime"];
                }
                else
                {
                    orderInfo.RemarkTime = null;
                }
                if (System.DBNull.Value != reader["ManagerRemark"])
                {
                    orderInfo.ManagerRemark = (string)reader["ManagerRemark"];
                }
                if (System.DBNull.Value != reader["AdjustedDiscount"])
                {
                    orderInfo.AdjustedDiscount = (decimal)reader["AdjustedDiscount"];
                }
                if (System.DBNull.Value != reader["OrderStatus"])
                {
                    orderInfo.OrderStatus = (OrderStatus)reader["OrderStatus"];
                }
                if (System.DBNull.Value != reader["CloseReason"])
                {
                    orderInfo.CloseReason = (string)reader["CloseReason"];
                }
                if (System.DBNull.Value != reader["OrderPoint"])
                {
                    orderInfo.Points = (int)reader["OrderPoint"];
                }
                orderInfo.OrderDate = (System.DateTime)reader["OrderDate"];
                if (System.DBNull.Value != reader["PayDate"])
                {
                    orderInfo.PayDate = (System.DateTime)reader["PayDate"];
                }
                if (System.DBNull.Value != reader["ShippingDate"])
                {
                    orderInfo.ShippingDate = (System.DateTime)reader["ShippingDate"];
                }
                if (System.DBNull.Value != reader["FinishDate"])
                {
                    orderInfo.FinishDate = (System.DateTime)reader["FinishDate"];
                }
                if (reader["ReferralUserId"] != System.DBNull.Value)
                {
                    orderInfo.ReferralUserId = (int)reader["ReferralUserId"];
                }
                orderInfo.UserId = (int)reader["UserId"];
                orderInfo.Username = (string)reader["Username"];
                if (System.DBNull.Value != reader["EmailAddress"])
                {
                    orderInfo.EmailAddress = (string)reader["EmailAddress"];
                }
                if (System.DBNull.Value != reader["RealName"])
                {
                    orderInfo.RealName = (string)reader["RealName"];
                }
                if (System.DBNull.Value != reader["QQ"])
                {
                    orderInfo.QQ = (string)reader["QQ"];
                }
                if (System.DBNull.Value != reader["Wangwang"])
                {
                    orderInfo.Wangwang = (string)reader["Wangwang"];
                }
                if (System.DBNull.Value != reader["MSN"])
                {
                    orderInfo.MSN = (string)reader["MSN"];
                }
                if (System.DBNull.Value != reader["ShippingRegion"])
                {
                    orderInfo.ShippingRegion = (string)reader["ShippingRegion"];
                }
                if (System.DBNull.Value != reader["Address"])
                {
                    orderInfo.Address = (string)reader["Address"];
                }
                if (System.DBNull.Value != reader["ZipCode"])
                {
                    orderInfo.ZipCode = (string)reader["ZipCode"];
                }
                if (System.DBNull.Value != reader["ShipTo"])
                {
                    orderInfo.ShipTo = (string)reader["ShipTo"];
                }
                if (System.DBNull.Value != reader["TelPhone"])
                {
                    orderInfo.TelPhone = (string)reader["TelPhone"];
                }
                if (System.DBNull.Value != reader["CellPhone"])
                {
                    orderInfo.CellPhone = (string)reader["CellPhone"];
                }
                if (System.DBNull.Value != reader["ShipToDate"])
                {
                    orderInfo.ShipToDate = (string)reader["ShipToDate"];
                }
                if (System.DBNull.Value != reader["ShippingModeId"])
                {
                    orderInfo.ShippingModeId = (int)reader["ShippingModeId"];
                }
                if (System.DBNull.Value != reader["ModeName"])
                {
                    orderInfo.ModeName = (string)reader["ModeName"];
                }
                if (System.DBNull.Value != reader["RealShippingModeId"])
                {
                    orderInfo.RealShippingModeId = (int)reader["RealShippingModeId"];
                }
                if (System.DBNull.Value != reader["RealModeName"])
                {
                    orderInfo.RealModeName = (string)reader["RealModeName"];
                }
                if (System.DBNull.Value != reader["RegionId"])
                {
                    orderInfo.RegionId = (int)reader["RegionId"];
                }
                if (System.DBNull.Value != reader["Freight"])
                {
                    orderInfo.Freight = (decimal)reader["Freight"];
                }
                if (System.DBNull.Value != reader["AdjustedFreight"])
                {
                    orderInfo.AdjustedFreight = (decimal)reader["AdjustedFreight"];
                }
                if (System.DBNull.Value != reader["ShipOrderNumber"])
                {
                    orderInfo.ShipOrderNumber = (string)reader["ShipOrderNumber"];
                }
                if (System.DBNull.Value != reader["ExpressCompanyName"])
                {
                    orderInfo.ExpressCompanyName = (string)reader["ExpressCompanyName"];
                }
                if (System.DBNull.Value != reader["ExpressCompanyAbb"])
                {
                    orderInfo.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
                }
                if (System.DBNull.Value != reader["PaymentTypeId"])
                {
                    orderInfo.PaymentTypeId = (int)reader["PaymentTypeId"];
                }
                if (System.DBNull.Value != reader["PaymentType"])
                {
                    orderInfo.PaymentType = (string)reader["PaymentType"];
                }
                if (System.DBNull.Value != reader["PayCharge"])
                {
                    orderInfo.PayCharge = (decimal)reader["PayCharge"];
                }
                if (System.DBNull.Value != reader["RefundStatus"])
                {
                    orderInfo.RefundStatus = (RefundStatus)reader["RefundStatus"];
                }
                if (System.DBNull.Value != reader["RefundAmount"])
                {
                    orderInfo.RefundAmount = (decimal)reader["RefundAmount"];
                }
                if (System.DBNull.Value != reader["RefundRemark"])
                {
                    orderInfo.RefundRemark = (string)reader["RefundRemark"];
                }
                if (System.DBNull.Value != reader["Gateway"])
                {
                    orderInfo.Gateway = (string)reader["Gateway"];
                }
                if (System.DBNull.Value != reader["ReducedPromotionId"])
                {
                    orderInfo.ReducedPromotionId = (int)reader["ReducedPromotionId"];
                }
                if (System.DBNull.Value != reader["ReducedPromotionName"])
                {
                    orderInfo.ReducedPromotionName = (string)reader["ReducedPromotionName"];
                }
                if (System.DBNull.Value != reader["ReducedPromotionAmount"])
                {
                    orderInfo.ReducedPromotionAmount = (decimal)reader["ReducedPromotionAmount"];
                }
                if (System.DBNull.Value != reader["IsReduced"])
                {
                    orderInfo.IsReduced = (bool)reader["IsReduced"];
                }
                if (System.DBNull.Value != reader["SentTimesPointPromotionId"])
                {
                    orderInfo.SentTimesPointPromotionId = (int)reader["SentTimesPointPromotionId"];
                }
                if (System.DBNull.Value != reader["SentTimesPointPromotionName"])
                {
                    orderInfo.SentTimesPointPromotionName = (string)reader["SentTimesPointPromotionName"];
                }
                if (System.DBNull.Value != reader["IsSendTimesPoint"])
                {
                    orderInfo.IsSendTimesPoint = (bool)reader["IsSendTimesPoint"];
                }
                if (System.DBNull.Value != reader["TimesPoint"])
                {
                    orderInfo.TimesPoint = (decimal)reader["TimesPoint"];
                }
                if (System.DBNull.Value != reader["FreightFreePromotionId"])
                {
                    orderInfo.FreightFreePromotionId = (int)reader["FreightFreePromotionId"];
                }
                if (System.DBNull.Value != reader["FreightFreePromotionName"])
                {
                    orderInfo.FreightFreePromotionName = (string)reader["FreightFreePromotionName"];
                }
                if (System.DBNull.Value != reader["IsFreightFree"])
                {
                    orderInfo.IsFreightFree = (bool)reader["IsFreightFree"];
                }
                if (System.DBNull.Value != reader["CouponName"])
                {
                    orderInfo.CouponName = (string)reader["CouponName"];
                }
                if (System.DBNull.Value != reader["CouponCode"])
                {
                    orderInfo.CouponCode = (string)reader["CouponCode"];
                }
                if (System.DBNull.Value != reader["CouponAmount"])
                {
                    orderInfo.CouponAmount = (decimal)reader["CouponAmount"];
                }
                if (System.DBNull.Value != reader["CouponValue"])
                {
                    orderInfo.CouponValue = (decimal)reader["CouponValue"];
                }
                if (System.DBNull.Value != reader["GroupBuyId"])
                {
                    orderInfo.GroupBuyId = (int)reader["GroupBuyId"];
                }
                if (System.DBNull.Value != reader["CountDownBuyId"])
                {
                    orderInfo.CountDownBuyId = (int)reader["CountDownBuyId"];
                }
                if (System.DBNull.Value != reader["Bundlingid"])
                {
                    orderInfo.BundlingID = (int)reader["Bundlingid"];
                }
                if (System.DBNull.Value != reader["BundlingPrice"])
                {
                    orderInfo.BundlingPrice = (decimal)reader["BundlingPrice"];
                }
                if (System.DBNull.Value != reader["NeedPrice"])
                {
                    orderInfo.NeedPrice = (decimal)reader["NeedPrice"];
                }
                if (System.DBNull.Value != reader["GroupBuyStatus"])
                {
                    orderInfo.GroupBuyStatus = (GroupBuyStatus)reader["GroupBuyStatus"];
                }
                if (System.DBNull.Value != reader["TaobaoOrderId"])
                {
                    orderInfo.TaobaoOrderId = (string)reader["TaobaoOrderId"];
                }
                if (System.DBNull.Value != reader["OrderType"])
                {
                    orderInfo.OrderType = (int)reader["OrderType"];
                }
                if (System.DBNull.Value != reader["Tax"])
                {
                    orderInfo.Tax = (decimal)reader["Tax"];
                }
                else
                {
                    orderInfo.Tax = 0m;
                }
                if (System.DBNull.Value != reader["IdentityCard"])
                {
                    orderInfo.IdentityCard = reader["IdentityCard"].ToString();
                }
                if (System.DBNull.Value != reader["PCustomsClearanceDate"])
                {
                    orderInfo.PCustomsClearanceDate = (DateTime)reader["PCustomsClearanceDate"];
                }
                else
                {
                    orderInfo.PCustomsClearanceDate = DateTime.Now;
                }

                if (System.DBNull.Value != reader["InvoiceTitle"])
                {
                    orderInfo.InvoiceTitle = (string)reader["InvoiceTitle"];
                }
                else
                {
                    orderInfo.InvoiceTitle = "";
                }
                if (System.DBNull.Value != reader["SourceOrder"])
                {
                    OrderSource orderSource = (OrderSource)int.Parse(reader["SourceOrder"].ToString());
                    orderInfo.OrderSource = orderSource;
                }
                if (System.DBNull.Value != reader["SourceOrderId"])
                {
                    orderInfo.SourceOrderId = reader["SourceOrderId"].ToString();
                }
                else
                {
                    orderInfo.SourceOrderId = "";
                }

                if (System.DBNull.Value != reader["VoucherName"])
                {
                    orderInfo.VoucherName = (string)reader["VoucherName"];
                }
                if (System.DBNull.Value != reader["VoucherCode"])
                {
                    orderInfo.VoucherCode = (string)reader["VoucherCode"];
                }
                if (System.DBNull.Value != reader["VoucherAmount"])
                {
                    orderInfo.VoucherAmount = (decimal)reader["VoucherAmount"];
                }
                if (System.DBNull.Value != reader["VoucherValue"])
                {
                    orderInfo.VoucherValue = (decimal)reader["VoucherValue"];
                }
                if (System.DBNull.Value != reader["OriginalTax"])
                {
                    orderInfo.OriginalTax = (decimal)reader["OriginalTax"];
                }
                if (System.DBNull.Value != reader["ActivityType"])
                {
                    orderInfo.ActivityType = (int)reader["ActivityType"];
                }
                result = orderInfo;
            }
            return result;
        }

        public static OrderExpress PopulateOrderExpress(IDataRecord reader)
        {
            OrderExpress result;
            if (reader == null)
            {
                result = null;
            }
            else
            {
                OrderExpress orderExpress = new OrderExpress();
                if (System.DBNull.Value != reader["ShipOrderNumber"])
                {
                    orderExpress.ShipOrderNumber = (string)reader["ShipOrderNumber"];
                }
                if (System.DBNull.Value != reader["ExpressCompanyName"])
                {
                    orderExpress.ExpressCompanyName = (string)reader["ExpressCompanyName"];
                }
                if (System.DBNull.Value != reader["OrderStatus"])
                {
                    orderExpress.OrderStatus = (OrderStatus)reader["OrderStatus"];
                }
                if (System.DBNull.Value != reader["ExpressCompanyAbb"])
                {
                    orderExpress.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
                }

                result = orderExpress;
            }
            return result;
        }

        public static OrderGiftInfo PopulateOrderGift(IDataRecord reader)
        {
            OrderGiftInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                OrderGiftInfo orderGiftInfo = new OrderGiftInfo
                {
                    OrderId = (string)reader["OrderId"],
                    GiftId = (int)reader["GiftId"],
                    GiftName = (string)reader["GiftName"],
                    CostPrice = (reader["CostPrice"] == System.DBNull.Value) ? 0m : ((decimal)reader["CostPrice"]),
                    ThumbnailsUrl = (reader["ThumbnailsUrl"] == System.DBNull.Value) ? string.Empty : ((string)reader["ThumbnailsUrl"]),
                    Quantity = (reader["Quantity"] == System.DBNull.Value) ? 0 : ((int)reader["Quantity"]),
                    PromoteType = (int)reader["PromoType"]
                };
                result = orderGiftInfo;
            }
            return result;
        }

        public static ShoppingCartPresentInfo PopulateOrderPresents(IDataRecord reader)
        {
            ShoppingCartPresentInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ShoppingCartPresentInfo orderPersentInfo = new ShoppingCartPresentInfo
                {
                    OrderId = (string)reader["OrderId"],
                    SkuId = (string)reader["SkuId"],
                    ProductId = (int)reader["ProductId"],
                    SKU = (reader["SKU"] == System.DBNull.Value) ?string.Empty: ((string)reader["SKU"]),
                    ShipmentQuantity = (int)reader["ShipmentQuantity"],
                    CostPrice = (reader["CostPrice"] == System.DBNull.Value) ? 0m : ((decimal)reader["CostPrice"]),
                    ItemListPrice = (reader["ItemListPrice"] == System.DBNull.Value) ? 0m : ((decimal)reader["ItemListPrice"]),
                    ItemAdjustedPrice = (reader["ItemAdjustedPrice"] == System.DBNull.Value) ? 0m : ((decimal)reader["ItemAdjustedPrice"]),
                    ItemDescription = (reader["ItemDescription"] == System.DBNull.Value) ? string.Empty : ((string)reader["ItemDescription"]),
                    SkuContent = (reader["SKUContent"] == System.DBNull.Value) ? string.Empty : ((string)reader["SKUContent"]),
                    PromotionId = (reader["PromotionId"] == System.DBNull.Value) ? 0 : ((int)reader["PromotionId"]),
                    PromotionName = (reader["PromotionName"] == System.DBNull.Value) ? string.Empty : ((string)reader["PromotionName"]),
                    ThumbnailUrl40 = (reader["ThumbnailsUrl"] == System.DBNull.Value) ? string.Empty : ((string)reader["ThumbnailsUrl"]),
                    StoreId = (reader["storeId"] == System.DBNull.Value) ? 0 : ((int)reader["storeId"]),
                    SupplierId = (reader["SupplierId"] == System.DBNull.Value) ? 0 : ((int)reader["SupplierId"])
                };
                result = orderPersentInfo;
            }
            return result;
        }
        public static LineItemInfo PopulateLineItem(IDataRecord reader)
        {
            LineItemInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                LineItemInfo lineItemInfo = new LineItemInfo();
                lineItemInfo.SkuId = (string)reader["SkuId"];
                lineItemInfo.ProductId = (int)reader["ProductId"];
                if (reader["SKU"] != System.DBNull.Value)
                {
                    lineItemInfo.SKU = (string)reader["SKU"];
                }
                lineItemInfo.Quantity = (int)reader["Quantity"];
                lineItemInfo.ShipmentQuantity = (int)reader["ShipmentQuantity"];

                if (reader["TaxRate"] != System.DBNull.Value)
                {
                    if (reader["TaxRate"].ToString() == "0")
                    {
                        lineItemInfo.TaxRate = 0M;
                    }
                    else
                    {
                        lineItemInfo.TaxRate = (decimal)reader["TaxRate"];
                    }
                }

                if (reader["PromotionPrice"] != System.DBNull.Value)
                {
                    if (reader["PromotionPrice"].ToString() == "0")
                    {
                        lineItemInfo.PromotionPrice = 0M;
                    }
                    else
                    {
                        lineItemInfo.PromotionPrice = (decimal)reader["PromotionPrice"];
                    }
                }

                lineItemInfo.ItemCostPrice = (decimal)reader["CostPrice"];
                lineItemInfo.ItemListPrice = (decimal)reader["ItemListPrice"];
                lineItemInfo.ItemAdjustedPrice = (decimal)reader["ItemAdjustedPrice"];
                lineItemInfo.ItemDescription = (string)reader["ItemDescription"];
                //if (reader["ThumbnailsUrl"] != System.DBNull.Value)
                //{
                //    lineItemInfo.ThumbnailsUrl = (string)reader["ThumbnailsUrl"];
                //}

                if (reader["ThumbnailUrl220"] != System.DBNull.Value)
                {
                    lineItemInfo.ThumbnailsUrl = (string)reader["ThumbnailUrl220"];
                }


                lineItemInfo.ItemWeight = (decimal)reader["Weight"];
                if (System.DBNull.Value != reader["SKUContent"])
                {
                    lineItemInfo.SKUContent = (string)reader["SKUContent"];
                }
                if (System.DBNull.Value != reader["PromotionId"])
                {
                    lineItemInfo.PromotionId = (int)reader["PromotionId"];
                }
                if (System.DBNull.Value != reader["PromotionName"])
                {
                    lineItemInfo.PromotionName = (string)reader["PromotionName"];
                }
                if (System.DBNull.Value != reader["TemplateId"])
                {
                    lineItemInfo.TemplateId = (int)reader["TemplateId"];
                }
                if (System.DBNull.Value != reader["SupplierId"])
                {
                    lineItemInfo.SupplierId = (int)reader["SupplierId"];
                }
                if (System.DBNull.Value != reader["SupplierName"])
                {
                    lineItemInfo.SupplierName = reader["SupplierName"].ToString();
                }
                else
                {
                    lineItemInfo.SupplierName = "";
                }
                if (System.DBNull.Value != reader["OrderId"])
                {
                    lineItemInfo.OrderId = (string)reader["OrderId"];
                }
                if (System.DBNull.Value != reader["OrderStatus"])
                {
                    lineItemInfo.OrderStatus = (OrderStatus)reader["OrderStatus"];
                }
                result = lineItemInfo;
            }
            return result;
        }
        public static UserStatisticsInfo PopulateUserStatistics(IDataRecord reader)
        {
            UserStatisticsInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                UserStatisticsInfo userStatisticsInfo = new UserStatisticsInfo();
                if (reader["RegionId"] != System.DBNull.Value)
                {
                    userStatisticsInfo.RegionId = (long)((int)reader["RegionId"]);
                }
                if (reader["Usercounts"] != System.DBNull.Value)
                {
                    userStatisticsInfo.Usercounts = (int)reader["Usercounts"];
                }
                if (reader["AllUserCounts"] != System.DBNull.Value)
                {
                    userStatisticsInfo.AllUserCounts = (int)reader["AllUserCounts"];
                }
                result = userStatisticsInfo;
            }
            return result;
        }
        public static StatisticsInfo PopulateStatistics(IDataRecord reader)
        {
            StatisticsInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                StatisticsInfo statisticsInfo = new StatisticsInfo();
                statisticsInfo.OrderNumbWaitConsignment = (int)reader["orderNumbWaitConsignment"];
                statisticsInfo.ApplyRequestWaitDispose = (int)reader["ApplyRequestWaitDispose"];
                statisticsInfo.ProductNumStokWarning = (int)reader["ProductNumStokWarning"];
                statisticsInfo.LeaveComments = (int)reader["LeaveComments"];
                statisticsInfo.ProductConsultations = (int)reader["ProductConsultations"];
                statisticsInfo.Messages = (int)reader["Messages"];
                statisticsInfo.OrderNumbToday = (int)reader["OrderNumbToday"];
                statisticsInfo.OrderPriceToday = (decimal)reader["OrderPriceToday"];
                statisticsInfo.OrderProfitToday = (decimal)reader["OrderProfitToday"];
                statisticsInfo.UserNewAddToday = (int)reader["UserNewAddToday"];
                statisticsInfo.UserNumbBirthdayToday = (int)reader["userNumbBirthdayToday"];
                statisticsInfo.OrderNumbYesterday = (int)reader["OrderNumbYesterday"];
                statisticsInfo.OrderPriceYesterday = (decimal)reader["OrderPriceYesterday"];
                statisticsInfo.OrderProfitYesterday = (decimal)reader["OrderProfitYesterday"];
                statisticsInfo.UserNumb = (int)reader["UserNumb"];
                statisticsInfo.Balance = (decimal)reader["memberBalance"];
                statisticsInfo.BalanceDrawRequested = (decimal)reader["BalanceDrawRequested"];
                statisticsInfo.ProductNumbOnSale = (int)reader["ProductNumbOnSale"];
                statisticsInfo.ProductNumbInStock = (int)reader["ProductNumbInStock"];
                if (reader["arealdyPaidNum"] != System.DBNull.Value)
                {
                    statisticsInfo.AlreadyPaidOrdersNum = (int)reader["arealdyPaidNum"];
                }
                if (reader["arealdyPaidTotal"] != System.DBNull.Value)
                {
                    statisticsInfo.AreadyPaidOrdersAmount = (decimal)reader["arealdyPaidTotal"];
                }
                result = statisticsInfo;
            }
            return result;
        }
        public static FriendlyLinksInfo PopulateFriendlyLink(IDataRecord reader)
        {
            FriendlyLinksInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                FriendlyLinksInfo friendlyLinksInfo = new FriendlyLinksInfo();
                friendlyLinksInfo.LinkId = new int?((int)reader["LinkId"]);
                friendlyLinksInfo.Visible = (bool)reader["Visible"];
                friendlyLinksInfo.DisplaySequence = (int)reader["DisplaySequence"];
                if (reader["ImageUrl"] != System.DBNull.Value)
                {
                    friendlyLinksInfo.ImageUrl = (string)reader["ImageUrl"];
                }
                if (reader["Title"] != System.DBNull.Value)
                {
                    friendlyLinksInfo.Title = (string)reader["Title"];
                }
                if (reader["LinkUrl"] != System.DBNull.Value)
                {
                    friendlyLinksInfo.LinkUrl = (string)reader["LinkUrl"];
                }
                result = friendlyLinksInfo;
            }
            return result;
        }

        public static StoreInfo PopulateStoreInfo(IDataRecord reader)
        {
            StoreInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                StoreInfo storeInfo = new StoreInfo();
                if (reader["StoreName"] != System.DBNull.Value)
                {
                    storeInfo.StoreName = (string)reader["StoreName"];
                }
                result = storeInfo;
            }
            return result;
        }


        public static VoteInfo PopulateVote(IDataRecord reader)
        {
            return new VoteInfo
            {
                VoteId = (long)reader["VoteId"],
                VoteName = (string)reader["VoteName"],
                IsBackup = (bool)reader["IsBackup"],
                MaxCheck = (int)reader["MaxCheck"],
                VoteCounts = (int)reader["VoteCounts"]
            };
        }
        public static VoteItemInfo PopulateVoteItem(IDataRecord reader)
        {
            return new VoteItemInfo
            {
                VoteId = (long)reader["VoteId"],
                VoteItemId = (long)reader["VoteItemId"],
                VoteItemName = (string)reader["VoteItemName"],
                ItemCount = (int)reader["ItemCount"]
            };
        }
        public static MessageBoxInfo PopulateMessageBox(IDataReader reader)
        {
            MessageBoxInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                result = new MessageBoxInfo
                {
                    MessageId = (long)reader["MessageId"],
                    Accepter = (string)reader["Accepter"],
                    Sernder = (string)reader["Sernder"],
                    IsRead = (bool)reader["IsRead"],
                    ContentId = (long)reader["ContentId"],
                    Title = (string)reader["Title"],
                    Content = (string)reader["Content"],
                    Date = (System.DateTime)reader["Date"]
                };
            }
            return result;
        }
        public static MemberGradeInfo PopulateMemberGrade(IDataReader reader)
        {
            MemberGradeInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                MemberGradeInfo memberGradeInfo = new MemberGradeInfo();
                memberGradeInfo.GradeId = (int)reader["GradeId"];
                memberGradeInfo.Name = (string)reader["Name"];
                if (reader["Description"] != System.DBNull.Value)
                {
                    memberGradeInfo.Description = (string)reader["Description"];
                }
                memberGradeInfo.Points = (int)reader["Points"];
                memberGradeInfo.IsDefault = (bool)reader["IsDefault"];
                memberGradeInfo.Discount = (int)reader["Discount"];
                result = memberGradeInfo;
            }
            return result;
        }
        public static ShippingAddressInfo PopulateShippingAddress(IDataRecord reader)
        {
            ShippingAddressInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
                shippingAddressInfo.ShippingId = (int)reader["ShippingId"];
                shippingAddressInfo.ShipTo = (string)reader["ShipTo"];
                shippingAddressInfo.RegionId = (int)reader["RegionId"];
                shippingAddressInfo.UserId = (int)reader["UserId"];
                shippingAddressInfo.Address = (string)reader["Address"];
                shippingAddressInfo.Zipcode = (string)reader["Zipcode"];
                shippingAddressInfo.IsDefault = (bool)reader["IsDefault"];
                if (reader["TelPhone"] != System.DBNull.Value)
                {
                    shippingAddressInfo.TelPhone = (string)reader["TelPhone"];
                }
                if (reader["IdentityCard"] != System.DBNull.Value)
                {
                    shippingAddressInfo.IdentityCard = (string)reader["IdentityCard"];
                }
                if (reader["CellPhone"] != System.DBNull.Value)
                {
                    shippingAddressInfo.CellPhone = (string)reader["CellPhone"];
                }
                result = shippingAddressInfo;
            }
            return result;
        }
        public static MemberClientSet PopulateMemberClientSet(IDataReader reader)
        {
            MemberClientSet result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                MemberClientSet memberClientSet = new MemberClientSet();
                memberClientSet.ClientTypeId = (int)reader["ClientTypeId"];
                if (System.DateTime.Compare((System.DateTime)reader["StartTime"], System.Convert.ToDateTime("1900-01-01")) != 0)
                {
                    memberClientSet.StartTime = new System.DateTime?((System.DateTime)reader["StartTime"]);
                }
                if (System.DateTime.Compare((System.DateTime)reader["EndTime"], System.Convert.ToDateTime("1900-01-01")) != 0)
                {
                    memberClientSet.EndTime = new System.DateTime?((System.DateTime)reader["EndTime"]);
                }
                memberClientSet.LastDay = (int)reader["LastDay"];
                if (reader["ClientChar"] != System.DBNull.Value)
                {
                    memberClientSet.ClientChar = (string)reader["ClientChar"];
                }
                memberClientSet.ClientValue = (decimal)reader["ClientValue"];
                result = memberClientSet;
            }
            return result;
        }
        public static ShoppingCartItemInfo PopulateCartItem(IDataReader reader)
        {
            ShoppingCartItemInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
                shoppingCartItemInfo.SkuId = (string)reader["SkuId"];
                shoppingCartItemInfo.ProductId = (int)reader["ProductId"];
                shoppingCartItemInfo.SKU = (string)reader["SKU"];
                shoppingCartItemInfo.Name = (string)reader["Name"];
                shoppingCartItemInfo.MemberPrice = (decimal)reader["MemberPrice"];
                shoppingCartItemInfo.Quantity = (int)reader["Quantity"];
                shoppingCartItemInfo.Weight = (decimal)reader["Weight"];
                if (reader["SKUContent"] != System.DBNull.Value)
                {
                    shoppingCartItemInfo.SkuContent = (string)reader["SKUContent"];
                }
                if (reader["ThumbnailUrl40"] != System.DBNull.Value)
                {
                    shoppingCartItemInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
                }
                if (reader["ThumbnailUrl60"] != System.DBNull.Value)
                {
                    shoppingCartItemInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
                }
                if (reader["ThumbnailUrl100"] != System.DBNull.Value)
                {
                    shoppingCartItemInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
                }
                shoppingCartItemInfo.IsCustomsClearance = false;

                try
                {
                    if (reader["IsCustomsClearance"] != System.DBNull.Value)
                    {
                        shoppingCartItemInfo.IsCustomsClearance = (bool)reader["IsCustomsClearance"];
                    }
                }
                catch { }

                result = shoppingCartItemInfo;
            }
            return result;
        }
        public static ShoppingCartGiftInfo PopulateGiftCartItem(IDataReader reader)
        {
            ShoppingCartGiftInfo shoppingCartGiftInfo = new ShoppingCartGiftInfo();
            shoppingCartGiftInfo.UserId = (int)reader["UserId"];
            shoppingCartGiftInfo.GiftId = (int)reader["GiftId"];
            shoppingCartGiftInfo.Name = (string)reader["Name"];
            if (reader["CostPrice"] != System.DBNull.Value)
            {
                shoppingCartGiftInfo.CostPrice = (decimal)reader["CostPrice"];
            }
            shoppingCartGiftInfo.NeedPoint = (int)reader["NeedPoint"];
            if (reader["ThumbnailUrl40"] != System.DBNull.Value)
            {
                shoppingCartGiftInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
            }
            if (reader["ThumbnailUrl60"] != System.DBNull.Value)
            {
                shoppingCartGiftInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
            }
            if (reader["ThumbnailUrl100"] != System.DBNull.Value)
            {
                shoppingCartGiftInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
            }
            if (reader["PromoType"] != System.DBNull.Value)
            {
                shoppingCartGiftInfo.PromoType = (int)reader["PromoType"];
            }
            return shoppingCartGiftInfo;
        }

        public static ShoppingCartPresentInfo PopulatePresentCartItem(IDataReader reader)
        {
            ShoppingCartPresentInfo presendInfo = new ShoppingCartPresentInfo();
            if (reader["SkuId"] != System.DBNull.Value)
            {
                presendInfo.SkuId = (string)reader["SkuId"];
            }
            if (reader["ProductId"] != System.DBNull.Value)
            {
                presendInfo.ProductId = (int)reader["ProductId"];
            }
            if (reader["ProductName"] != System.DBNull.Value)
            {
                presendInfo.ProductName = (string)reader["ProductName"];
            }
            if (reader["SKU"] != System.DBNull.Value)
            {
                presendInfo.SKU = (string)reader["SKU"];
            }
            if (reader["ShipmentQuantity"] != System.DBNull.Value)
            {
                presendInfo.ShipmentQuantity = (int)reader["ShipmentQuantity"];
            }
            if (reader["CostPrice"] != System.DBNull.Value)
            {
                presendInfo.CostPrice = (decimal)reader["CostPrice"];
            }
            if (reader["ItemListPrice"] != System.DBNull.Value)
            {
                presendInfo.ItemListPrice = (decimal)reader["ItemListPrice"];
            }
            if (reader["ItemAdjustedPrice"] != System.DBNull.Value)
            {
                presendInfo.ItemAdjustedPrice = Decimal.Parse(reader["ItemAdjustedPrice"].ToString());
            }
            if (reader["ThumbnailUrl40"] != System.DBNull.Value)
            {
                presendInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
            }
            if (reader["SkuContent"] != System.DBNull.Value)
            {
                presendInfo.SkuContent = (string)reader["SkuContent"];
            }
            if (reader["PromotionId"] != System.DBNull.Value)
            {
                presendInfo.PromotionId = (int)reader["PromotionId"];
            }
            if (reader["PromotionName"] != System.DBNull.Value)
            {
                presendInfo.PromotionName = (string)reader["PromotionName"];
            }
            if (reader["StoreId"] != System.DBNull.Value)
            {
                presendInfo.StoreId = (int)reader["StoreId"];
            }
            if (reader["SupplierId"] != System.DBNull.Value)
            {
                presendInfo.SupplierId = (int)reader["SupplierId"];
            }
            if (reader["SupplierName"] != System.DBNull.Value)
            {
                presendInfo.SupplierName = (string)reader["SupplierName"];
            }
            if (reader["ShopName"] != System.DBNull.Value)
            {
                presendInfo.ShopName = (string)reader["ShopName"];
            }
            if (reader["ShopOwner"] != System.DBNull.Value)
            {
                presendInfo.ShopOwner = (string)reader["ShopOwner"];
            }
            if (reader["FactStock"] != System.DBNull.Value)
            {
                presendInfo.FactStock = (int)reader["FactStock"];
            }
            if (reader["DiscountValue"] != System.DBNull.Value)
            {
                presendInfo.DiscountValue =Decimal.ToInt32((decimal)reader["DiscountValue"]);
                presendInfo.ShipmentQuantity = presendInfo.DiscountValue;
            }
            if (reader["IsAscend"] != System.DBNull.Value)
            {
                presendInfo.IsAscend = (int)reader["IsAscend"];
            }
            if (reader["PromotionProductId"] != System.DBNull.Value)
            {
                presendInfo.PromotionProductId = (int)reader["PromotionProductId"];
            }
            return presendInfo;
        }

        public static ProductsCombination PopulateCombinationCartItem(IDataReader reader)
        {
            ProductsCombination combination = new ProductsCombination();
            if (reader["ProductId"] != System.DBNull.Value)
            {
                combination.ProductId = (int)reader["ProductId"];
            }
            if (reader["SkuId"] != System.DBNull.Value)
            {
                combination.SkuId = (string)reader["SkuId"];
            }
            if (reader["CombinationSkuId"] != System.DBNull.Value)
            {
                combination.CombinationSkuId = (string)reader["CombinationSkuId"];
            }
            if (reader["SKU"] != System.DBNull.Value)
            {
                combination.SKU = (string)reader["SKU"];
            }
            if (reader["Quantity"] != System.DBNull.Value)
            {
                combination.Quantity = (int)reader["Quantity"];
            }
            if (reader["Price"] != System.DBNull.Value)
            {
                combination.Price = (decimal)reader["Price"];
            }
            if (reader["ProductName"] != System.DBNull.Value)
            {
                combination.ProductName = (string)reader["ProductName"];
            }
            if (reader["ThumbnailsUrl"] != System.DBNull.Value)
            {
                combination.ThumbnailsUrl = (string)reader["ThumbnailsUrl"];
            }
            if (reader["SKUContent"] != System.DBNull.Value)
            {
                combination.SKUContent = (string)reader["SKUContent"];
            }
            if (reader["Weight"] != System.DBNull.Value)
            {
                combination.Weight = Decimal.ToInt32((decimal)reader["Weight"]);
            }
            if (reader["AddTime"] != System.DBNull.Value)
            {
                combination.AddTime = (DateTime)reader["AddTime"];
            }
            if (reader["TaxRate"] != System.DBNull.Value)
            {
                combination.TaxRate = (decimal)reader["TaxRate"];
            }
            return combination;
        }

        public static TopicInfo PopulateTopic(IDataReader reader)
        {
            TopicInfo topicInfo = new TopicInfo();
            topicInfo.TopicId = (int)reader["TopicId"];
            topicInfo.Title = (string)reader["Title"];
            if (reader["Icon"] != System.DBNull.Value)
            {
                topicInfo.IconUrl = (string)reader["Icon"];
            }
            topicInfo.Content = (string)reader["Content"];
            topicInfo.AddedDate = (System.DateTime)reader["AddedDate"];
            topicInfo.IsRelease = (bool)reader["IsRelease"];
            return topicInfo;
        }
        public static SplittinDetailInfo PopulateSplittinDetail(IDataReader reader)
        {
            SplittinDetailInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                SplittinDetailInfo splittinDetailInfo = new SplittinDetailInfo();
                splittinDetailInfo.JournalNumber = (long)reader["JournalNumber"];
                if (reader["Balance"] != System.DBNull.Value)
                {
                    splittinDetailInfo.Balance = (decimal)reader["Balance"];
                }
                if (System.DBNull.Value != reader["Income"])
                {
                    splittinDetailInfo.Income = new decimal?((decimal)reader["Income"]);
                }
                if (System.DBNull.Value != reader["IsUse"])
                {
                    splittinDetailInfo.IsUse = (bool)reader["IsUse"];
                }
                if (System.DBNull.Value != reader["OrderId"])
                {
                    splittinDetailInfo.OrderId = (string)reader["OrderId"];
                }
                if (System.DBNull.Value != reader["Remark"])
                {
                    splittinDetailInfo.Remark = (string)reader["Remark"];
                }
                if (System.DBNull.Value != reader["SubUserId"])
                {
                    splittinDetailInfo.SubUserId = (int)reader["SubUserId"];
                }
                if (System.DBNull.Value != reader["TradeDate"])
                {
                    splittinDetailInfo.TradeDate = (System.DateTime)reader["TradeDate"];
                }
                if (System.DBNull.Value != reader["TradeType"])
                {
                    splittinDetailInfo.TradeType = (SplittingTypes)((int)reader["TradeType"]);
                }
                splittinDetailInfo.UserId = (int)reader["UserId"];
                splittinDetailInfo.UserName = (string)reader["UserName"];
                result = splittinDetailInfo;
            }
            return result;
        }

        public static ImportSourceTypeInfo ConvertDataRowToImportSourceType(DataRow row)
        {
            ImportSourceTypeInfo importSourceTypeInfo = new ImportSourceTypeInfo();
            if (row["ImportSourceId"] != System.DBNull.Value)
            {
                importSourceTypeInfo.ImportSourceId = (int)row["ImportSourceId"];
            }
            if (row["Icon"] != System.DBNull.Value)
            {
                importSourceTypeInfo.Icon = (string)row["Icon"];
            }
            if (row["EnArea"] != System.DBNull.Value)
            {
                importSourceTypeInfo.EnArea = (string)row["EnArea"];
            }
            if (row["CnArea"] != System.DBNull.Value)
            {
                importSourceTypeInfo.CnArea = (string)row["CnArea"];
            }
            if (row["Remark"] != System.DBNull.Value)
            {
                importSourceTypeInfo.Remark = (string)row["Remark"];
            }
            if (row["DisplaySequence"] != System.DBNull.Value)
            {
                importSourceTypeInfo.DisplaySequence = (int)row["DisplaySequence"];
            }
            if (row["AddTime"] != System.DBNull.Value)
            {
                importSourceTypeInfo.AddTime = (System.DateTime)row["AddTime"];
            }
            return importSourceTypeInfo;
        }


        public static ImportSourceTypeInfo PopulateImportSourceTypeInfo(IDataReader reader)
        {
            ImportSourceTypeInfo result;
            if (null == reader)
            {
                result = null;
            }
            else
            {
                ImportSourceTypeInfo importSourceTypeInfo = new ImportSourceTypeInfo();

                if (reader["ImportSourceId"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.ImportSourceId = (int)reader["ImportSourceId"];
                }
                if (reader["Icon"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.Icon = (string)reader["Icon"];
                }
                if (reader["EnArea"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.EnArea = (string)reader["EnArea"];
                }
                if (reader["CnArea"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.CnArea = (string)reader["CnArea"];
                }
                if (reader["Remark"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.Remark = (string)reader["Remark"];
                }
                if (reader["DisplaySequence"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.DisplaySequence = (int)reader["DisplaySequence"];
                }
                if (reader["AddTime"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.AddTime = (System.DateTime)reader["AddTime"];
                }

                if (reader["HSCode"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.HSCode = (string)reader["HSCode"];
                }
                if (reader["FavourableFlag"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.FavourableFlag = (System.Boolean)reader["FavourableFlag"];
                }

                if (reader["StandardCName"] != System.DBNull.Value)
                {
                    importSourceTypeInfo.StandardCName = (string)reader["StandardCName"];
                }

                result = importSourceTypeInfo;
            }
            return result;
        }
    }
}
