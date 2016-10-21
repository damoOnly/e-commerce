using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Jobs
{
    public class ProductListItem
    {
        public ProductListItem()
        {
            this.SkuItems = new List<SkuItem>();
            this.Skus = new List<Sku>();
        }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal SalePrice { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal TaxRate { get; set; }
        public int Quantity { get; set; }
        public bool HasSku { get; set; }
        public string SkuId { get; set; }
        public string ShortDescription { get; set; }
        public int CategoryId { get; set; }
        public string PromotionName { get; set; }
        public int SaleCounts { get; set; }
        public int VistiCounts { get; set; }

        //是否收藏
        public int IsCollect { get; set; }
        public List<SkuItem> SkuItems { get; set; }
        public List<Sku> Skus { get; set; }

        public bool IsFreeShipping { get; set; }
        public bool IsCustomsClearance { get; set; }
        public string ShippingMode { get; set; }
    }

    public class HistoryProductListItem : ProductListItem
    {
        public int Id { get; set; }
        public string BrowseTime { get; set; }
        public string BrowseDate { get; set; }
    }
}
