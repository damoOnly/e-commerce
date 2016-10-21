using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class ProductDetails : HtmlTemplatedWebControl
    {
        private int productId;
        private Common_Location common_Location;
        private System.Web.UI.WebControls.Literal litProductName;
        private System.Web.UI.WebControls.Literal lblProductCode;
        private SkuLabel lblSku;
        private StockLabel lblStock;
        private System.Web.UI.WebControls.Label litWeight;
        private System.Web.UI.WebControls.Literal litUnit;
        private System.Web.UI.WebControls.Literal litBuyCardinality;
        private System.Web.UI.WebControls.Literal litBrosedNum;
        private System.Web.UI.WebControls.Literal litBrand;
        private System.Web.UI.WebControls.Literal litSaleCounts;
        private FormatedMoneyLabel lblMarkerPrice;
        private System.Web.UI.WebControls.Label lblBuyPrice;
        private TotalLabel lblTotalPrice;
        private System.Web.UI.WebControls.Literal litDescription;
        private System.Web.UI.WebControls.Literal litShortDescription;
        private BuyButton btnBuy;
        private AddCartButton btnaddgouwu;
        private System.Web.UI.WebControls.HyperLink hpkProductConsultations;
        private System.Web.UI.WebControls.HyperLink hpkProductReviews;
        private System.Web.UI.WebControls.HyperLink hpkProductSales;
        private Common_ProductImages images;
        //private ThemedTemplatedRepeater rptExpandAttributes;
        private Common_ExpandAttributes1 expandAttributes;//新样式的商品扩展属性
        private SKUSelector skuSelector;
        private Common_ProductReview reviews;
        private Common_ProductConsultations consultations;
        private Common_GoodsList_Correlative correlative;
        private Common_GoodsList_HotSale hotSale;//销售排行
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddenpid;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidden_skus;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidden_skuItem;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidCartQuantity;
        private System.Web.UI.HtmlControls.HtmlInputHidden buyCardinality;
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddeSkuId;
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddeCategoryId;
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddeCategoryName;
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddeProductName;
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddeProductCode;
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddeUserId;


        private ProductPromote promote;
        private UserProductReferLabel lbUserProductRefer;
        private System.Web.UI.WebControls.Literal litTaxRate;
        private System.Web.UI.WebControls.Literal litCnArea;
        private HiImage imgIcon;
        private System.Web.UI.WebControls.Literal litShipping;
        private System.Web.UI.WebControls.Literal litCategoryNotes3;//广告图
        /// <summary>
        /// 商品二维码图片
        /// </summary>
        private System.Web.UI.HtmlControls.HtmlImage productImg;
        private System.Web.UI.HtmlControls.HtmlAnchor nowBuyBtn;
        private System.Web.UI.WebControls.Literal litReviewCount;
        private System.Web.UI.WebControls.Literal litItemNumber;
        private System.Web.UI.WebControls.Literal litviewcount;
        private System.Web.UI.HtmlControls.HtmlInputHidden hiddensupplierid;
        private System.Web.UI.WebControls.Literal litMarkPrice;
        private BuyAmountBox txtBuyAmount;
        private System.Web.UI.WebControls.Label lblsmalltitle;
        

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ProductDetails.html";
            }
            base.OnInit(e);
        }
        public string GetQuantity_Product(int productId)
        {
            string text = "";
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart != null)
            {
                foreach (ShoppingCartItemInfo current in shoppingCart.LineItems)
                {
                    if (current.ProductId == productId)
                    {
                        string str = current.SkuId + "|" + current.Quantity;
                        text = text + ((text == "") ? "" : ",") + str;
                    }
                }
                return text;
            }
            return "";
        }
        protected override void AttachChildControls()
        {
            if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
            {
                this.Page.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString() + "&ReferralUserId=" + HiContext.Current.User.UserId);
                return;
            }
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.hiddenpid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddenpid");
            this.hiddeSkuId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddeSkuId");
            this.hiddeCategoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddeCategoryId");
            this.hiddeCategoryName = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddeCategoryName");
            this.hidCartQuantity = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txCartQuantity");
            this.hiddeProductCode = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddeProductCode");
            this.hiddeUserId =      (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddeUserId");
            this.hiddeProductName = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddeProductName");
            this.buyCardinality = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("buyCardinality");
            this.hiddenpid.Value = this.productId.ToString();
            this.promote = (ProductPromote)this.FindControl("ProductPromote");
            this.common_Location = (Common_Location)this.FindControl("common_Location");
            this.litProductName = (System.Web.UI.WebControls.Literal)this.FindControl("litProductName");
            this.lblProductCode = (System.Web.UI.WebControls.Literal)this.FindControl("lblProductCode");
            this.lblSku = (SkuLabel)this.FindControl("lblSku");
            this.lblStock = (StockLabel)this.FindControl("lblStock");
            this.litUnit = (System.Web.UI.WebControls.Literal)this.FindControl("litUnit");
            this.litBuyCardinality = (System.Web.UI.WebControls.Literal)this.FindControl("litBuyCardinality");
            this.litWeight = (System.Web.UI.WebControls.Label)this.FindControl("litWeight");
            this.litBrosedNum = (System.Web.UI.WebControls.Literal)this.FindControl("litBrosedNum");
            this.litBrand = (System.Web.UI.WebControls.Literal)this.FindControl("litBrand");
            this.litSaleCounts = (System.Web.UI.WebControls.Literal)this.FindControl("litSaleCounts");
            this.lblMarkerPrice = (FormatedMoneyLabel)this.FindControl("lblMarkerPrice");
            this.lblBuyPrice = (System.Web.UI.WebControls.Label)this.FindControl("lblBuyPrice");
            this.lblsmalltitle = (System.Web.UI.WebControls.Label)this.FindControl("lblsmalltitle");
            
            this.lblTotalPrice = (TotalLabel)this.FindControl("lblTotalPrice");
            this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
            this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
            this.btnBuy = (BuyButton)this.FindControl("btnBuy");
            this.btnaddgouwu = (AddCartButton)this.FindControl("btnaddgouwu");
            this.hpkProductConsultations = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpkProductConsultations");
            this.hpkProductReviews = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpkProductReviews");
            this.hpkProductSales = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpkProductSales");
            this.litReviewCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewCount");
            this.litItemNumber = (System.Web.UI.WebControls.Literal)this.FindControl("litItemNumber");
            this.images = (Common_ProductImages)this.FindControl("common_ProductImages");
            //this.rptExpandAttributes = (ThemedTemplatedRepeater)this.FindControl("rptExpandAttributes");
            this.expandAttributes = (Common_ExpandAttributes1)this.FindControl("expandAttributes");
            this.skuSelector = (SKUSelector)this.FindControl("SKUSelector");
            this.reviews = (Common_ProductReview)this.FindControl("list_Common_ProductReview");
            this.consultations = (Common_ProductConsultations)this.FindControl("list_Common_ProductConsultations");
            this.correlative = (Common_GoodsList_Correlative)this.FindControl("list_Common_GoodsList_Correlative");
            this.lbUserProductRefer = (UserProductReferLabel)this.FindControl("lbUserProductRefer");
            this.hidden_skus = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidden_skus");
            this.hidden_skuItem = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidden_skuItem");
            this.litTaxRate = (System.Web.UI.WebControls.Literal)this.FindControl("litTaxRate");
            this.imgIcon = (HiImage)this.FindControl("imgIcon");
            this.litCnArea = (System.Web.UI.WebControls.Literal)this.FindControl("litCnArea");
            this.litShipping = (System.Web.UI.WebControls.Literal)this.FindControl("litShipping");
            this.hotSale = (Common_GoodsList_HotSale)this.FindControl("list_Common_GoodsList_HotSale");//销售排行
            this.litCategoryNotes3 = (System.Web.UI.WebControls.Literal)this.FindControl("litCategoryNotes3");
            this.productImg = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("productImg");//二维码图片
            this.nowBuyBtn = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("nowBuyBtn");
            this.litviewcount = (System.Web.UI.WebControls.Literal)this.FindControl("litviewcount");
            this.hiddensupplierid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddensupplierid");
            this.litMarkPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litMarkPrice");
            this.txtBuyAmount = (BuyAmountBox)this.FindControl("txtBuyAmount");
            if (!this.Page.IsPostBack)
            {
                ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, new int?(this.reviews.MaxNum), new int?(this.consultations.MaxNum));
                if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除"));
                    return;
                }
                if (this.hidCartQuantity != null)
                {
                    this.hidCartQuantity.Value = this.GetQuantity_Product(productBrowseInfo.Product.ProductId);
                }
                System.Collections.IEnumerable value =
                    from item in productBrowseInfo.Product.Skus
                    select item.Value;
                if (JsonConvert.SerializeObject(productBrowseInfo.DbSKUs) != null)
                {
                    this.hidden_skuItem.Value = JsonConvert.SerializeObject(productBrowseInfo.DbSKUs);
                }
                if (this.hidden_skus != null)
                {
                    this.hidden_skus.Value = JsonConvert.SerializeObject(value);
                }
                if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale)
                {
                    this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[]
					{
						this.Page.Request.QueryString["productId"]
					}));
                }
                if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnStock)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该商品已入库"));
                    return;
                }

                if (productBrowseInfo.cIsDisable == 1)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该商品已入库"));
                    return;
                }


                this.LoadPageSearch(productBrowseInfo.Product);
                if (this.lbUserProductRefer != null)
                {
                    this.lbUserProductRefer.product = productBrowseInfo.Product;
                }
                this.hpkProductReviews.Text = "查看全部" + productBrowseInfo.ReviewCount.ToString() + "条评论";
                this.litviewcount.Text = productBrowseInfo.ReviewCount.ToString();
                this.hpkProductConsultations.Text = "查看全部" + productBrowseInfo.ConsultationCount.ToString() + "条咨询";
                string count = ProductBrowser.GetLineItemNumber(this.productId).ToString();
                //this.hpkProductSales.Text = "查看全部" + count + "条成交记录";

                int countt = ProductBrowser.GetLineItemCount(this.productId);
                if (countt > 0)
                {
                    this.hpkProductSales.Text = "查看全部" + countt + "条成交记录";
                    this.hpkProductSales.NavigateUrl = string.Format("LookLineItems.aspx?productId={0}", this.productId);
                }
                else
                {
                    this.hpkProductSales.Text = "暂无成交记录";
                }

                this.litReviewCount.Text = productBrowseInfo.ReviewCount.ToString();
                this.litItemNumber.Text = countt.ToString();
                this.hpkProductConsultations.NavigateUrl = string.Format("ProductConsultationsAndReplay.aspx?productId={0}", this.productId);
                this.hpkProductReviews.NavigateUrl = string.Format("LookProductReviews.aspx?productId={0}", this.productId);
                //this.hpkProductSales.NavigateUrl = string.Format("LookLineItems.aspx?productId={0}", this.productId);
                this.LoadProductInfo(productBrowseInfo.Product, productBrowseInfo.BrandName);

                 this.hiddeCategoryId.Value = productBrowseInfo.CategoryId;
                 this.hiddeCategoryName.Value = productBrowseInfo.CategoryName;
                 this.hiddeProductName.Value = productBrowseInfo.Product.ProductName;
                 this.hiddeProductCode.Value = productBrowseInfo.Product.ProductCode;
                 this.hiddeUserId.Value = HiContext.Current.User.UserId.ToString();
                this.btnBuy.Stock = productBrowseInfo.Product.Stock;
                this.txtBuyAmount.Quantity = productBrowseInfo.Product.BuyCardinality;
                this.btnaddgouwu.Stock = productBrowseInfo.Product.Stock;
                BrowsedProductQueue.EnQueue(this.productId);
                this.images.ImageInfo = productBrowseInfo.Product;
                if (this.promote != null)
                {
                    this.promote.ProductId = this.productId;
                }
                if (productBrowseInfo.DbAttribute != null)
                {
                    this.expandAttributes.DbAttribute = productBrowseInfo.DbAttribute;//商品扩展属性
                    //this.rptExpandAttributes.DataSource = productBrowseInfo.DbAttribute;
                    //this.rptExpandAttributes.DataBind();
                }
                if (productBrowseInfo.DbSKUs != null)
                {
                    this.skuSelector.ProductId = this.productId;
                    this.skuSelector.DataSource = productBrowseInfo.DbSKUs;
                }
                if (productBrowseInfo.DBReviews != null)
                {
                    this.reviews.DataSource = productBrowseInfo.DBReviews;
                    this.reviews.DataBind();
                }
                if (productBrowseInfo.DBConsultations != null)
                {
                    this.consultations.DataSource = productBrowseInfo.DBConsultations;
                    this.consultations.DataBind();
                }
                if (productBrowseInfo.DbCorrelatives != null)
                {
                    this.correlative.DataSource = productBrowseInfo.DbCorrelatives;//推荐 ，捆绑或同类型下的商品
                    this.correlative.DataBind();
                }
                if (productBrowseInfo.DBHotSale != null)
                {
                    this.hotSale.DataSource = productBrowseInfo.DBHotSale;
                    this.hotSale.DataBind();
                }
                if (this.productImg != null)
                {
                    if (!string.IsNullOrWhiteSpace(productBrowseInfo.Product.QRcode))
                    {
                        this.productImg.Src = productBrowseInfo.Product.QRcode;
                    }
                    //else
                    //{
                    //    //this.productImg.Attributes.Add("display", "none");
                    //}
                }
                this.litCategoryNotes3.Text = productBrowseInfo.CategoryNote3;
            }
        }
        private void LoadPageSearch(ProductInfo productDetails)
        {
            if (!string.IsNullOrEmpty(productDetails.MetaKeywords))
            {
                MetaTags.AddMetaKeywords(productDetails.MetaKeywords, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(productDetails.MetaDescription))
            {
                MetaTags.AddMetaDescription(productDetails.MetaDescription, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(productDetails.Title))
            {
                PageTitle.AddSiteNameTitle(productDetails.Title);
                return;
            }
            PageTitle.AddSiteNameTitle(productDetails.ProductName);
        }
        private void LoadProductInfo(ProductInfo productDetails, string brandName)
        {
            if (this.common_Location != null && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
            {
                this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
                this.common_Location.ProductName = productDetails.ProductName;
            }
            this.litProductName.Text = productDetails.ProductName;
            this.lblProductCode.Text = productDetails.ProductCode;
            this.lblSku.Text = productDetails.SKU;
            this.lblSku.Value = productDetails.SkuId;
            this.hiddeSkuId.Value = productDetails.SkuId;
           
            this.lblStock.Stock = productDetails.Stock;

            string unit = productDetails.Unit;
            if (!string.IsNullOrEmpty(unit))
            {
                this.litUnit.Text = productDetails.Unit;
            }
            else
            {
                this.litUnit.Text = "件";
            }


            if (productDetails.BuyCardinality > 1)
            {
                this.litBuyCardinality.Text = "起购数：" + productDetails.BuyCardinality;
            }
            else
            {
                this.litBuyCardinality.Visible = false;
            }
            buyCardinality.Value = productDetails.BuyCardinality.ToString();

            if (productDetails.Weight > 0m)
            {
                this.litWeight.Text = string.Format("{0:F2} g", productDetails.Weight);
            }
            else
            {
                this.litWeight.Text = "无";
            }
            this.litBrosedNum.Text = productDetails.VistiCounts.ToString();
            this.litBrand.Text = brandName;
            if (this.litSaleCounts != null)
            {
                this.litSaleCounts.Text = productDetails.ShowSaleCounts.ToString();
            }
            if (productDetails.MinSalePrice == productDetails.MaxSalePrice)
            {
                this.lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2");
                this.lblTotalPrice.Value = new decimal?(productDetails.MinSalePrice);
            }
            else
            {
                this.lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2") + " - " + productDetails.MaxSalePrice.ToString("F2");
            }
            this.hiddensupplierid.Value = productDetails.SupplierId.ToString();
            this.lblMarkerPrice.Money = productDetails.MarketPrice;
            if (productDetails.MarketPrice.HasValue && productDetails.MarketPrice > 0)
            {
                this.litMarkPrice.Text = string.Format("市场价：<span id='sp_MarketPrice'>￥{0}</span>", Globals.FormatMoney((decimal)productDetails.MarketPrice));
            }
            this.litDescription.Text = productDetails.Description;
            if (this.litShortDescription != null)
            {
                this.litShortDescription.Text = productDetails.ShortDescription;
            }

            if (this.litTaxRate != null)
            {
                // 组合商品

                this.litTaxRate.Text = productDetails.GetExtendTaxRate();
                
               
            }

            ImportSourceTypeInfo imSourceType = ProductBrowser.GetProductImportSourceType(this.productId);

            if (this.litCnArea != null && imSourceType != null)
            {
                this.litCnArea.Text = imSourceType.CnArea;
            }

            if (this.imgIcon != null && imSourceType != null)
            {
                this.imgIcon.ImageUrl = imSourceType.Icon;
            }

            if (this.litShipping != null)
            {
                this.litShipping.Text = productDetails.TemplateName;
            }


            this.lblsmalltitle.Text = productDetails.ProductTitle;
            //if (this.lblStock.Stock ==0)
            //{
            //    //if (productImg != null)
            //    //{
            //    //    productImg.Visible = false;
            //    //}

            //    //if (btnaddgouwu != null)
            //    //{
            //    //    btnaddgouwu.Visible = false;
            //    //}

            //    //if (nowBuyBtn != null)
            //    //{
            //    //    nowBuyBtn.Visible = false;
            //    //}
            //}
        }
    }
}
