using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EcShop.Web.Api.Model.Result
{
    public class OrderItemResult
    {
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public int OrderSource { get; set; }
        public string Address { get; set; }
        public string Reciver { get; set; }
        public string Telephone { get; set; }
        public string Cellphone { get; set; }
        public string ShippingDate { get; set; }
        public string ShippingModeName { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public string ShipToDate { get; set; }
        public bool IsNeedInvoice { get; set; }
        public string InvoiceTitle { get; set; }
        public int BuyQuantity { get; set; }
        public decimal Total { get; set; }
        public decimal Amount { get; set; }
        public decimal PayCharge { get; set; }
        public decimal Freight { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public int Point { get; set; }
        public bool IsCanReview { get; set; }
        public string Gateway { get; set; }
        public string GatewayOrderId { get; set; }
        public string IdentityCard { get; set; }
        public string CouponName { get; set; }
        public string VoucherName { get; set; }
        public string FinishDate { get; set; }
        public string PayDate { get; set; }

        public List<OrderSkuItem> Items { get; set; }
        public List<OrderGiftItem> GiftItems { get; set; }

        public int ApplyId { get; set; }
        public int ApplyType { get; set; }
        public string ApplyForTime { get; set; }
        public string Comments { get; set; }
        public string AdminRemark { get; set; }
        public int HandleStatus { get; set; }
        public string HandleTime { get; set; }
        public decimal RefundMoney { get; set; }
        public int RefundType { get; set; }
        public string ExpressCompany { get; set; }
        public string TrackingNumber { get; set; }

        public string Remark { get; set; }

        public string SourceOrderId { get; set; }
        public int IsCancelOrder
        { get; set; }

        public int IsRefund { get; set; }

    }

    public class OrderSkuItem
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
        public string SkuCode
        {
            get;
            set;
        }
        public int Quantity
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }
        public decimal AdjustedPrice
        {
            get;
            set;
        }

        string description;
        public string Description
        {
            get { return HttpUtility.HtmlDecode(description);; }

            set { description = value; }
        }
        public string ThumbnailsUrl
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

        public string PromotionName
        {
            get;
            set;
        }
        public decimal TaxRate
        {
            get;
            set;
        }

        public decimal SubTotal
        {
            get;
            set;
        }

        public string ShareUrl { get; set; }
        

        // 评论
        public string ReviewContent { get; set; }
        public int ReviewScore { get; set; }
        public bool IsCanReview { get; set; }
    }

    public class OrderGiftItem
    {
        public int GiftId
        {
            get;
            set;
        }
        public string GiftName
        {
            get;
            set;
        }
        public int Quantity
        {
            get;
            set;
        }
        public string ThumbnailsUrl
        {
            get;
            set;
        }
    }
}
