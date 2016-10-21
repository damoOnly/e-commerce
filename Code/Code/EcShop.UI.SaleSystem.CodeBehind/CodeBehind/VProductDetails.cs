using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VProductDetails : VshopTemplatedWebControl
    {
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private System.Web.UI.WebControls.Literal litProdcutName;
        private System.Web.UI.WebControls.Literal litSalePrice;
        private System.Web.UI.WebControls.Literal litSoldCount;
        private System.Web.UI.WebControls.Literal litMarketPrice;
        private System.Web.UI.WebControls.Literal litShortDescription;
        private System.Web.UI.WebControls.Literal litDescription;
        private System.Web.UI.WebControls.Literal litStock;
        private System.Web.UI.WebControls.Literal litBuyCardinality;
        private System.Web.UI.WebControls.Literal litConsultationsCount;
        private System.Web.UI.WebControls.Literal litReviewsCount;
        private System.Web.UI.WebControls.Literal litSupplier;


        private Common_SKUSelector skuSelector;
        private Common_ExpandAttributes expandAttr;
        private System.Web.UI.WebControls.HyperLink linkDescription;
        private System.Web.UI.HtmlControls.HtmlInputHidden litHasCollected;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidden_skus;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidden_BuyCardinality;
        private System.Web.UI.WebControls.Literal litTaxRate;
        private System.Web.UI.WebControls.Literal litShipping;
        private UserProductReferLabel lbUserProductRefer;
        private ProductPromote promote;
        private System.Web.UI.WebControls.Literal litCnArea;
        private HiImage imgIcon;
        //private HiImage imgSupplierIcon;

        //副标题
        private System.Web.UI.WebControls.Literal lblsmalltitle;

        //折扣
        private System.Web.UI.WebControls.Literal litDiscrunt;

        //促销价图标显示
        private System.Web.UI.WebControls.Literal litpropricemsg;

        //商品促销
        private System.Web.UI.WebControls.Literal litProPromration;

        //订单促销
        private System.Web.UI.WebControls.Literal litOrderPromration;

        //包邮
        private System.Web.UI.WebControls.Literal litFreeShip;


        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductDetails.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
            this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
            this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
            this.litMarketPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litMarketPrice");
            this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
            this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
            this.litTaxRate = (System.Web.UI.WebControls.Literal)this.FindControl("litTaxRate");
            this.litShipping = (System.Web.UI.WebControls.Literal)this.FindControl("litShipping");
            this.litStock = (System.Web.UI.WebControls.Literal)this.FindControl("litStock");
            this.litBuyCardinality = (System.Web.UI.WebControls.Literal)this.FindControl("litBuyCardinality");
            this.litSupplier = (System.Web.UI.WebControls.Literal)this.FindControl("litSupplier");
            this.lblsmalltitle = (System.Web.UI.WebControls.Literal)this.FindControl("lblsmalltitle");
            this.litDiscrunt = (System.Web.UI.WebControls.Literal)this.FindControl("litDiscrunt");
            this.litpropricemsg = (System.Web.UI.WebControls.Literal)this.FindControl("litpropricemsg");
            this.litProPromration = (System.Web.UI.WebControls.Literal)this.FindControl("litProPromration");
            this.litOrderPromration = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderPromration");
            this.litFreeShip = (System.Web.UI.WebControls.Literal)this.FindControl("litFreeShip");

            this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
            this.linkDescription = (System.Web.UI.WebControls.HyperLink)this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
            this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
            this.litConsultationsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litConsultationsCount");
            this.litReviewsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewsCount");
            this.litHasCollected = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litHasCollected");
            this.hidden_skus = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidden_skus");
            this.hidden_BuyCardinality = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidden_BuyCardinality");
            this.lbUserProductRefer = (UserProductReferLabel)this.FindControl("lbUserProductRefer");
            this.promote = (ProductPromote)this.FindControl("ProductPromote");
            this.litCnArea = (System.Web.UI.WebControls.Literal)this.FindControl("litCnArea");
            this.imgIcon = (HiImage)this.FindControl("imgIcon");
            //this.imgSupplierIcon = (HiImage)this.FindControl("imgSupplierIcon");

            ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, null, null);

            if (productBrowseInfo == null)
            {
                base.GotoResourceNotFound("此商品已不存在");
            }

            if (productBrowseInfo.Product == null)
            {
                base.GotoResourceNotFound("此商品已不存在");
            }
            if (productBrowseInfo.Product.SaleStatus != ProductSaleStatus.OnSale)
            {
                base.GotoResourceNotFound("此商品已下架");
            }
            if (!productBrowseInfo.Product.IsApproved)
            {
                base.GotoResourceNotFound("此商品未审核");
            }


            System.Collections.IEnumerable value =
              from item in productBrowseInfo.Product.Skus
              select item.Value;
            if (this.hidden_skus != null)
            {
                this.hidden_skus.Value = JsonConvert.SerializeObject(value);
            }


            if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
            {
                string text = System.Web.HttpContext.Current.Request.Url.ToString();
                if (text.IndexOf("?") > -1)
                {
                    text = text + "&ReferralUserId=" + HiContext.Current.User.UserId;
                }
                else
                {
                    text = text + "?ReferralUserId=" + HiContext.Current.User.UserId;
                }
                base.RegisterShareScript(productBrowseInfo.Product.ImageUrl4, text, productBrowseInfo.Product.ShortDescription, productBrowseInfo.Product.ProductName);
            }
            if (this.lbUserProductRefer != null)
            {
                this.lbUserProductRefer.product = productBrowseInfo.Product;
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

            if (this.rptProductImages != null)
            {
                string locationUrl = "javascript:;";
                SlideImage[] source = new SlideImage[]
				{
					new SlideImage(productBrowseInfo.Product.ImageUrl1, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl2, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl3, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl4, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl5, locationUrl),

				};
                this.rptProductImages.DataSource =
                    from item in source
                    where !string.IsNullOrWhiteSpace(item.ImageUrl)
                    select item;
                this.rptProductImages.DataBind();
            }
            this.litProdcutName.Text = productBrowseInfo.Product.ProductName;
            this.litSalePrice.Text = productBrowseInfo.Product.MinSalePrice.ToString("F2");
            if (productBrowseInfo.Product.MarketPrice.HasValue)
            {
                this.litMarketPrice.SetWhenIsNotNull(productBrowseInfo.Product.MarketPrice.GetValueOrDefault(0m).ToString("F2"));
            }
            this.litShortDescription.Text = productBrowseInfo.Product.ShortDescription;
            if (this.litDescription != null)
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<script[^>]*?>.*?</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (!string.IsNullOrWhiteSpace(productBrowseInfo.Product.MobblieDescription))
                {
                    this.litDescription.Text = regex.Replace(productBrowseInfo.Product.MobblieDescription, "");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(productBrowseInfo.Product.Description))
                    {
                        this.litDescription.Text = regex.Replace(productBrowseInfo.Product.Description, "");
                    }
                }
            }
            this.litSoldCount.SetWhenIsNotNull(productBrowseInfo.Product.ShowSaleCounts.ToString());
            this.litStock.Text = productBrowseInfo.Product.Stock.ToString();
            //this.litTaxRate.Text = (productBrowseInfo.Product.TaxRate * 100).ToString("0");

            if (this.litTaxRate != null)
            {
                this.litTaxRate.Text = productBrowseInfo.Product.GetExtendTaxRate();
            }

            if (productBrowseInfo.Product.BuyCardinality > 1)
            {
                this.litBuyCardinality.Text = "起购数：" + productBrowseInfo.Product.BuyCardinality;
            }
            else
            {
                this.litBuyCardinality.Visible = false;
            }
            this.hidden_BuyCardinality.Value = productBrowseInfo.Product.BuyCardinality.ToString();

            //运费模版
            ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(Int32.Parse(productBrowseInfo.Product.TemplateId != null ? productBrowseInfo.Product.TemplateId.ToString() : "0"));
            this.litShipping.Text = shippingMode != null ? shippingMode.TemplateName : "未设置";

            this.skuSelector.ProductId = this.productId;
            if (this.expandAttr != null)
            {
                this.expandAttr.ProductId = this.productId;
            }
            if (this.linkDescription != null)
            {
                this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
            }
            this.litConsultationsCount.SetWhenIsNotNull(productBrowseInfo.ConsultationCount.ToString());
            this.litReviewsCount.SetWhenIsNotNull(productBrowseInfo.ReviewCount.ToString());
            Member member = HiContext.Current.User as Member;
            int favoriteId = 0;
            if (member != null)
            {
                favoriteId = ProductBrowser.GetFavoriteId(member.UserId, this.productId);
            }
            this.litHasCollected.SetWhenIsNotNull(favoriteId.ToString());

            if (this.promote != null)
            {
                this.promote.ProductId = this.productId;
            }
            PageTitle.AddSiteNameTitle(productBrowseInfo.Product.ProductName);

            //this.litSupplierName.Text = productBrowseInfo.SupplierName;

            //if (this.imgSupplierIcon != null && productBrowseInfo.SupplierImageUrl != null)
            //{
            //    this.imgSupplierIcon.ImageUrl = productBrowseInfo.SupplierImageUrl;
            //}

            if (productBrowseInfo.Product.SupplierId == null || productBrowseInfo.Product.SupplierId == 0)
            {
                this.litSupplier.Text = "";
            }
            else
            {
                this.litSupplier.Text = "<a class=\"shop-link fix\" href=\"/vshop/SupProductList.aspx?SupplierId=" + productBrowseInfo.Product.SupplierId + "\"><span>进入店铺</span><img src=\"" + productBrowseInfo.SupplierLogo + "\"/>" + productBrowseInfo.SupplierName + "</a>";
            }

            this.lblsmalltitle.Text = productBrowseInfo.Product.ProductTitle;

            //this.litDiscrunt = (System.Web.UI.WebControls.Literal)this.FindControl("litDiscrunt");
            //this.litpropricemsg = (System.Web.UI.WebControls.Literal)this.FindControl("litpropricemsg");
            //显示促销信息
            if (productBrowseInfo.Product.IsPromotion)
            {
                this.litpropricemsg.Text = "<span class=\"pro-pricemsg\">促销价</span>";
            }

            decimal tempMinSalePrice = productBrowseInfo.Product.MinSalePrice;//折扣价
            decimal tempMarketPrice = productBrowseInfo.Product.MarketPrice.GetValueOrDefault(0m);//会员价
            decimal tempDiscrunt = 1M;
            if (tempMarketPrice > 0)
            {
                tempDiscrunt = (tempMinSalePrice / tempMarketPrice) * 10;
            }
            //显示折扣信息
            if (productBrowseInfo.Product.IsDisplayDiscount)
            {
                this.litDiscrunt.Text = "<span class=\"pro-discrunt\">" + tempDiscrunt.ToString("F2") + "折</span>";
            }


            //PromotionInfo promotionInfo;
            List<OrderPromotionItem> orderpromotionlist = new List<OrderPromotionItem>();
            List<ProductPromotionItem> productpromotionlist = new List<ProductPromotionItem>();

            if (member != null)
            {
                //promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productId);
                DataTable dtorderpromotion = ProductBrowser.GetOrderPromotionInfo(member);
                if (dtorderpromotion != null)
                {
                    OrderPromotionItem item = null;
                    foreach (DataRow row in dtorderpromotion.Rows)
                    {
                        item = new OrderPromotionItem();
                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }
                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }
                        orderpromotionlist.Add(item);
                    }
                }

                DataTable dtproductpromotion = ProductBrowser.GetProductPromotionList(member, productId);
                if (dtproductpromotion != null)
                {
                    ProductPromotionItem item = null;
                    foreach (DataRow row in dtproductpromotion.Rows)
                    {
                        item = new ProductPromotionItem();
                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }
                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }
                        productpromotionlist.Add(item);
                    }
                }
            }
            else
            {

                //promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productId);
                DataTable dtorderpromotion = ProductBrowser.GetAllOrderPromotionInfo();
                if (dtorderpromotion != null)
                {
                    OrderPromotionItem item = null;
                    foreach (DataRow row in dtorderpromotion.Rows)
                    {
                        item = new OrderPromotionItem();
                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }
                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }
                        orderpromotionlist.Add(item);
                    }
                }

                DataTable dtproductpromotion = ProductBrowser.GetAllProductPromotionList(productId);

                if (dtproductpromotion != null)
                {
                    ProductPromotionItem item = null;
                    foreach (DataRow row in dtproductpromotion.Rows)
                    {
                        item = new ProductPromotionItem();
                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }
                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }
                        productpromotionlist.Add(item);
                    }
                }
            }


            //this.litProPromration = (System.Web.UI.WebControls.Literal)this.FindControl("litProPromration");
            //this.litOrderPromration = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderPromration");
            //包邮
            if (orderpromotionlist != null)
            {
                for (int i = 0; i < orderpromotionlist.Count; i++)
                {
                    if (orderpromotionlist[i].PromoteType == 17)
                    {
                        this.litFreeShip.Text = "<div><span style=\"color:#999\">" + orderpromotionlist[i].Name + "</span></div>";
                    }
                    else
                    {
                        this.litOrderPromration.Text = "<span>" + orderpromotionlist[i].Name + "</span>";
                    }

                }
            }
            if (productpromotionlist != null)
            {
                for (int i = 0; i < productpromotionlist.Count; i++)
                {
                    this.litProPromration.Text = "<span>" + productpromotionlist[i].Name + "</span>";
                    break;
                }
            }

        }
    }

    public class OrderPromotionItem
    {
        public int PromoteType { get; set; }

        public string Name { get; set; }
    }

    public class ProductPromotionItem
    {
        public int PromoteType { get; set; }

        public string Name { get; set; }
    }
}
