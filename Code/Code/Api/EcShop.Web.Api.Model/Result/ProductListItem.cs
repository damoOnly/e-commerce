using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EcShop.Web.Api.Model.Result
{
    public class ProductListItem
    {
        public ProductListItem()
        {
            this.SkuItems = new List<SkuItem>();
            this.Skus = new List<Sku>();
        }
        public int ProductId { get; set; }

        string title;
        public string Title
        {
            get { return HttpUtility.HtmlDecode(title); }

            set { title = value; }
        }
        
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

        public int BuyCardinality { get; set; }

        public string Icon { get; set; }

        public string ShopName { get; set; }

        public string Discount { get; set; }

        public string ExtendTaxRate { get; set; }

        public string BrandId { get; set; }

        /// <summary>
        /// 获取税费范围
        /// </summary>
        /// <returns></returns>
        public string GetExtendTaxRate(decimal taxrate,decimal mintaxrate,decimal maxtaxrate)
        {
            string currTaxRate = (mintaxrate * 100).ToString("0") + "%" + "-" + (maxtaxrate * 100).ToString("0") + "%";
            string resultTaxRate;
            if (mintaxrate > 0)
            {
                resultTaxRate = mintaxrate == maxtaxrate ? (mintaxrate * 100).ToString("0") + "%" : currTaxRate;
            }
            else if (mintaxrate == 0 && maxtaxrate > 0)
            {
                resultTaxRate = currTaxRate;
            }
            else
            {
                resultTaxRate = (taxrate * 100).ToString("0") + "%";
            }

            return resultTaxRate;
        }

        
    }

    public class HistoryProductListItem : ProductListItem
    {
        public int Id { get; set; }
        public string BrowseTime { get; set; }
        public string BrowseDate { get; set; }
    }

    public class CountDownProductListItem : ProductListItem
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
