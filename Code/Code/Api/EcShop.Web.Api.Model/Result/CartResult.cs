using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class CartResult
    {
        public CartResult()
        {
            //this.CartItems = new List<CartItem>();
            //按照供应商对商品进行打包
            this.SupplierCartItems = new List<SupplierCartItem>();
            this.GiftItems = new List<GiftItem>();
        }

        public int ReducedPromotionId
        {
            get;
            set;
        }
        public string ReducedPromotionName
        {
            get;
            set;
        }
        public decimal ReducedPromotionAmount
        {
            get;
            set;
        }
        public bool IsReduced
        {
            get;
            set;
        }
        public int SendGiftPromotionId
        {
            get;
            set;
        }
        public string SendGiftPromotionName
        {
            get;
            set;
        }
        public bool IsSendGift
        {
            get;
            set;
        }
        public int SentTimesPointPromotionId
        {
            get;
            set;
        }
        public string SentTimesPointPromotionName
        {
            get;
            set;
        }
        public bool IsSendTimesPoint
        {
            get;
            set;
        }
        public decimal TimesPoint
        {
            get;
            set;
        }
        public int FreightFreePromotionId
        {
            get;
            set;
        }
        public string FreightFreePromotionName
        {
            get;
            set;
        }
        public bool IsFreightFree
        {
            get;
            set;
        }

        //public List<CartItem> CartItems
        //{
        //    get;
        //    set;
        //}

        public List<SupplierCartItem> SupplierCartItems { get; set; }

        public List<GiftItem> GiftItems
        {
            get;
            set;
        }

        
        public decimal Weight
        {
            get;
            set;
        }
        public decimal TotalWeight
        {
            get;
            set;
        }
        public decimal Total
        {
            get;
            set;
        }
        public int TotalNeedPoint
        {
            get;
            set;
        }
        public int Point
        {
            get;
            set;
        }
        public int GetPointMoney
        {
            get;
            set;
        }
        public decimal Amount
        {
            get;
            set;
        }
        public decimal GetOriginalAmount
        {
            get;
            set;
        }
        public decimal TotalTax
        {
            get;
            set;
        }
        public decimal TotalIncludeTax   //计算总价，包含税费，不包含运费
        {
            get;
            set;
        }
        public int Quantity
        {
            get;
            set;
        }
        public int SkuQuantity
        {
            get;
            set;
        }
    }
}
