using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ServiceOrderItemResult
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

		public int ApplyId  { get; set; }
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
        public List<ServiceOrderSkuItem> Items { get; set; }

    }

    public class ServiceOrderSkuItem
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

        public string Description
        {
            get;
            set;
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
        
    }

}
