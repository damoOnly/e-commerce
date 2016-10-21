using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EcShop.Web.Api.Model.Result
{
    public class ProductItem
    {
        public ProductItem()
        {
            this.ImageUrls = new List<ImageUrl>();
            this.AttributeItems = new List<AttributeItem>();
            this.SkuItems = new List<SkuItem>();
            this.Skus = new List<Sku>();

            this.OrderPromotionList = new List<OrderPromotionItem>();
            this.ProductPromotionList = new List<ProductPromotionItem>();

            this.IsShowShop = true;
        }

        public int ProductId { get; set; }

        string title;
        public string Title
        {
            get { return HttpUtility.HtmlDecode(title); }

            set { title = value; }
        }
        public string Code { get; set; }
        public List<ImageUrl> ImageUrls { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? MarketPrice { get; set; }
        public decimal TaxRate { get; set; }
        public int Stock { get; set; }
        public bool HasSku { get; set; }
        public string Description { get; set; }
        public List<AttributeItem> AttributeItems { get; set; }
        public List<SkuItem> SkuItems { get; set; }
        public List<Sku> Skus { get; set; }
        public int ShippingModeId { get; set; }
        public string ShippingModeName { get; set; }
        public bool IsFavorite { get; set; }
        public string Url { get; set; }
        public string ThumbnailsUrl { get; set; }

        public bool IsCustomsClearance { get; set; }

        public string PromotionName { get; set; }
        public int SaleCounts { get; set; }
        public int VistiCounts { get; set; }

        public string OriginalPlace { get; set; }
        public string OriginalPlaceIcon { get; set; }

        public int SupplierId { get; set; }

        public int ProductCommentCount { get; set; }

        public string SupplierName { get; set; }

        public string Logo { get; set; }

        public int BuyCardinality { get; set; }

        public List<OrderPromotionItem> OrderPromotionList { get; set; }

        public List<ProductPromotionItem> ProductPromotionList { get; set; }

        public string ProductSubTitle { get; set; }

        public string Discount { get; set; }

        public bool IsPromotion { get; set; }

        public string ExtendTaxRate { get; set; }

        public decimal Tax { get; set; }

        public bool IsShowShop { get; set; }
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

    public class ImageUrl
    {
        public ImageUrl() { }
        public ImageUrl(string url)
        {
            this.Url = url;
        }
        public string Url { get; set; }
    }

    public class AttributeItem
    {
        public AttributeItem() { }
        public AttributeItem(string attributeName, string attributeValue)
        {
            this.AttributeName = attributeName;
            this.AttributeValue = attributeValue;
        }

        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

    public class SkuItem
    {
        //AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl
        public SkuItem()
        {
            this.Items = new List<SkuItemOption>();
        }

        public SkuItem(int itemId, string itemName, bool isUseImage)
        {
            this.ItemId = itemId;
            this.ItemName = itemName;
            this.IsUseImage = isUseImage;

            this.Items = new List<SkuItemOption>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public bool IsUseImage { get; set; }

        public List<SkuItemOption> Items { get; set; }
    }

    public class SkuItemOption
    {
        public SkuItemOption() { }
        public SkuItemOption(int id, string value, string imageUrl) {
            this.Id = id;
            this.Value = value;
            this.ImageUrl = imageUrl;
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Sku
    {
        public Sku() {
            this.SkuItems = new List<SkuValue>();
        }
        public Sku(string skuId, string sku, decimal weight, decimal volume, decimal salePrice, int stock)
        {
            this.SkuId = skuId;
            this.SkuCode = sku;
            this.Weight = weight;
            this.Volume = volume;
            this.SalePrice = salePrice;
            this.Stock = stock;

            this.SkuItems = new List<SkuValue>();
        }

        public Sku(string skuId, string sku, decimal weight, decimal volume, decimal salePrice, int stock,decimal tax)
        {
            this.SkuId = skuId;
            this.SkuCode = sku;
            this.Weight = weight;
            this.Volume = volume;
            this.SalePrice = salePrice;
            this.Stock = stock;
            this.Tax = tax;

            this.SkuItems = new List<SkuValue>();
        }


        public string SkuId { get; set; }
        public string SkuCode { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public decimal Tax { get; set; }
        public List<SkuValue> SkuItems { get; set; }

    }

    public class SkuValue
    {
        public SkuValue() { }

        public SkuValue(int itemId, int valueId)
        {
            this.ItemId = itemId;
            this.ValueId = valueId;
        }
        public int ItemId { get; set; }
        public int ValueId { get; set; }
    }

    public class CountDownProductItem : ProductItem
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal CountDownPrice { get; set; }
        public int ActivityMaxCount { get; set; }
        public int ActivityPlanCount { get; set; }
        public int ActivitySaleCount { get; set; }
        public bool EnableCoupon { get; set; }
        public bool EnableVoucher { get; set; }
        public bool EnableScore { get; set; }
    }
}
