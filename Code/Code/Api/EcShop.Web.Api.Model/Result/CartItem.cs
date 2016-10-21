using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EcShop.Entities;
using EcShop.Entities.Promotions;
using System.Web;

namespace EcShop.Web.Api.Model.Result
{
    public class CartItem
    {
        public string SkuId
        {
            get;
            set;
        }
        public int ProductId
        {
            get;
            set;
        }
        public string SKU
        {
            get;
            set;
        }

        private string name;
        public string Name
        {
            get { return HttpUtility.HtmlDecode(name); }

            set { name = value; }
        }
        public decimal MemberPrice
        {
            get;
            set;
        }
        public string ThumbnailUrl40
        {
            get;
            set;
        }
        public string ThumbnailUrl60
        {
            get;
            set;
        }
        public string ThumbnailUrl100
        {
            get;
            set;
        }
        public decimal Weight
        {
            get;
            set;
        }
        public string SkuContent
        {
            get;
            set;
        }
        public int Quantity
        {
            get;
            set;
        }
        public int PromotionId
        {
            get;
            set;
        }
        public PromoteType PromoteType
        {
            get;
            set;
        }
        public string PromotionName
        {
            get;
            set;
        }
        public decimal AdjustedPrice
        {
            get;
            set;
        }
        public int ShippQuantity
        {
            get;
            set;
        }
        public bool IsSendGift
        {
            get;
            set;
        }
        public decimal SubTotal
        {
            get;
            set;
        }
        public bool IsFreeShipping
        {
            get;
            set;
        }
        public decimal SubWeight
        {
            get;
            set;
        }
        public decimal TaxRate
        {
            get;
            set;
        }
        public int TaxRateId
        {
            get;
            set;
        }
        public int TemplateId
        {
            get;
            set;
        }
        public int StoreId
        {
            get;
            set;
        }
        public int SupplierId
        {
            get;
            set;
        }
        public string SupplierName
        {
            get;
            set;
        }

        /// <summary>
        /// 税费小计
        /// </summary>
        public decimal SubTax
        {
            get;
            set;
        }

        /// <summary>
        /// 扩展税率
        /// </summary>
        public string ExtendTaxRate
        {
            get;
            set;
        }


        public decimal Tax
        {
            get;
            set;
        }

        public bool IsCustomsClearance { get; set; }

        public int Stock { get; set; }
        public string StockDesc { get; set; }

        public string Logo { get; set; }

        public int BuyCardinality { get; set; }
    }
}
