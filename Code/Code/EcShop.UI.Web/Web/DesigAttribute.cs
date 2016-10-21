using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml;
namespace EcShop.UI.Web
{
	public static class DesigAttribute
	{
		private static string _pagename;
		public static string DesigPageName;
		public static string SourcePageName = "";
        public static string PageName//ÐÞ¸Ä1
        {
            get
            {
                return _pagename;
            }
            set
            {
                _pagename = value;
                if (_pagename != "")
                {
                    switch (_pagename)
                    {
                        case "default":
                            DesigPageName = "Skin-Desig_Templete.html";
                            SourcePageName = "Default.aspx";
                            return;

                        case "login":
                            DesigPageName = "Skin-Desig_login.html";
                            SourcePageName = "Login.aspx";
                            return;

                        case "brand":
                            DesigPageName = "Skin-Desig_Brand.html";
                            SourcePageName = "Brand.aspx";
                            return;

                        case "branddetail":
                            {
                                DesigPageName = "Skin-Desig_BrandDetails.html";
                                SourcePageName = "BrandDetails.aspx";
                                DataTable brandCategories = CategoryBrowser.GetBrandCategories(0, 1);
                                if (brandCategories.Rows.Count > 0)
                                {
                                    SourcePageName = "BrandDetails.aspx?brandId=" + brandCategories.Rows[0]["BrandId"].ToString();
                                    brandCategories.Dispose();
                                }
                                return;
                            }
                        case "product":
                            DesigPageName = "Skin-Desig_SubCategory.html";
                            SourcePageName = "SubCategory.aspx";
                            return;

                        case "productdetail":
                            {
                                DesigPageName = "Skin-Desig_ProductDetails.html";
                                SourcePageName = "ProductDetails.aspx";
                                SubjectListQuery query = new SubjectListQuery();
                                query.MaxNum = 1;
                                DataTable subjectList = ProductBrowser.GetSubjectList(query);
                                if (subjectList.Rows.Count > 0)
                                {
                                    SourcePageName = "ProductDetails.aspx?productId=" + subjectList.Rows[0]["ProductId"].ToString();
                                    subjectList.Dispose();
                                }
                                return;
                            }
                        case "article":
                            DesigPageName = "Skin-Desig_Articles.html";
                            SourcePageName = "Articles.aspx";
                            return;

                        case "articledetail":
                            {
                                DesigPageName = "Skin-Desig_ArticleDetails.html";
                                SourcePageName = "ArticleDetails.aspx";
                                IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(0, 1);
                                if (articleList.Count > 0)
                                {
                                    SourcePageName = "ArticleDetails.aspx?articleId=" + articleList[0].ArticleId.ToString();
                                }
                                return;
                            }
                        case "cuountdown":
                            DesigPageName = "Skin-Desig_CountDownProducts.html";
                            SourcePageName = "CountDownProducts.aspx";
                            return;

                        case "cuountdowndetail":
                            {
                                DesigPageName = "Skin-Desig_CountDownProductsDetails.html";
                                SourcePageName = "CountDownProductsDetails.aspx";
                                DataTable counDownProducList = ProductBrowser.GetCounDownProducList(1);
                                if (counDownProducList.Rows.Count > 0)
                                {
                                    SourcePageName = "CountDownProductsDetails.aspx?productId=" + counDownProducList.Rows[0]["ProductId"].ToString();
                                    counDownProducList.Dispose();
                                }
                                return;
                            }
                        case "groupbuy":
                            DesigPageName = "Skin-Desig_GroupBuyProducts.html";
                            SourcePageName = "GroupBuyProducts.aspx";
                            return;

                        case "groupbuydetail":
                            {
                                DesigPageName = "Skin-Desig_CountDownProductsDetails.html";
                                SourcePageName = "GroupBuyProductDetails.aspx";
                                DataTable groupByProductList = ProductBrowser.GetGroupByProductList(1);
                                if (groupByProductList.Rows.Count > 0)
                                {
                                    SourcePageName = "GroupBuyProductDetails.aspx?productId=" + groupByProductList.Rows[0]["ProductId"].ToString();
                                    groupByProductList.Dispose();
                                }
                                return;
                            }
                        case "help":
                            DesigPageName = "Skin-Desig_Helps.html";
                            SourcePageName = "Helps.aspx";
                            return;

                        case "helpdetail":
                            {
                                DesigPageName = "Skin-Desig_HelpDetails.html";
                                SourcePageName = "HelpDetails.aspx";
                                DataSet helps = CommentBrowser.GetHelps();
                                if ((helps.Tables.Count > 0) && (helps.Tables[1].Rows.Count > 0))
                                {
                                    SourcePageName = "HelpDetails.aspx?helpId=" + helps.Tables[1].Rows[0]["HelpId"].ToString();
                                    helps.Dispose();
                                }
                                return;
                            }
                        case "gift":
                            DesigPageName = "Skin-Desig_OnlineGifts.html";
                            SourcePageName = "OnlineGifts.aspx";
                            return;

                        case "giftdetail":
                            {
                                DesigPageName = "Skin-Desig_GiftDetails.html";
                                SourcePageName = "GiftDetails.aspx";
                                IList<GiftInfo> gifts = ProductBrowser.GetGifts(1);
                                if (gifts.Count > 0)
                                {
                                    SourcePageName = "GiftDetails.aspx?giftId=" + gifts[0].GiftId.ToString();
                                }
                                return;
                            }
                        case "shopcart":
                            DesigPageName = "Skin-Desig_ShoppingCart.html";
                            SourcePageName = "ShoppingCart.aspx";
                            return;

                        case "categorycustom":
                            {
                                int result = 0;
                                int.TryParse(HttpContext.Current.Request.Form["Params"], out result);
                                DesigPageName = "Skin-Desig_Custom.html";
                                if (result > 0)
                                {
                                    SourcePageName = "SubCategory.aspx?categoryId=" + result.ToString();
                                }
                                return;
                            }
                        case "brandcustom":
                            {
                                int num2 = 0;
                                int.TryParse(HttpContext.Current.Request.Form["Params"], out num2);
                                DesigPageName = "Skin-Desig_Custom.html";
                                if (num2 > 0)
                                {
                                    SourcePageName = "BrandDetails.aspx?brandId=" + num2.ToString();
                                }
                                return;
                            }
                        case "customthemes":
                            {
                                int num3 = 0;
                                int.TryParse(HttpContext.Current.Request.Form["Params"], out num3);
                                DesigPageName = "Skin-Desig_Custom.html";
                                if (num3 > 0)
                                {
                                    SourcePageName = GetCustomSourcePage(num3);
                                }
                                return;
                            }
                        default:
                            return;
                    }
                }
            }
        }
		public static string DesigPagePath
		{
			get
			{
				return Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/" + DesigAttribute.DesigPageName);
			}
		}
		public static string SourcePagePath
		{
			get
			{
				return HiContext.Current.HostPath + "/" + DesigAttribute.SourcePageName;
			}
		}
		private static string GetCustomSourcePage(int tid)
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/default.xml");
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			return xmlDocument.SelectSingleNode("//CustomTheme/Theme[@Tid=" + tid + "]").Attributes["SourcePage"].Value.ToString();
		}
	}
}
