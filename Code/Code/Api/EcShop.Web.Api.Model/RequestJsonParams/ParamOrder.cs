using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EcShop.Web.Api.Model.Result;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamOrder : ParamBase
    {
        public string UserId { get; set; }
        public int SiteId { get; set; }
        public int StoreId { get; set; }
        public AddressItem ShippingAddress { get; set; }
        public string ShipToDate { get; set; }
        public string ShippingDate { get; set; }
        public int ShippingModeId { get; set; }
        public int PaymentTypeId { get; set; }
        public List<OrderSkuId> SkuIds { get; set; }
        public string UsedCouponCode { get; set; }
        public bool IsNeedInvoice { get; set; }
        public string InvoiceTitle { get; set; }
        public string Remark { get; set; }
        public string RealName { get; set; }
        public string IdNo { get; set; }

        public int IsUnpackOrder { get; set; }
    }

    public class OrderSkuId
    {
        public string SkuId { get; set; }
    }

    public class ParamOrderSignBuy : ParamBase
    {
        public string UserId { get; set; }
        public int SiteId { get; set; }
        public int StoreId { get; set; }
        public AddressItem ShippingAddress { get; set; }
        public string ShipToDate { get; set; }
        public string ShippingDate { get; set; }
        public int ShippingModeId { get; set; }
        public int PaymentTypeId { get; set; }
        public int ProductId { get; set; }
        public string SkuId { get; set; }
        public int Quantity { get; set; }
        public string UsedCouponCode { get; set; }
        public bool IsNeedInvoice { get; set; }
        public string InvoiceTitle { get; set; }
        public string Remark { get; set; }
        public string RealName { get; set; }
        public string IdNo { get; set; }
        public string BuyType { get; set; }
        public int BuyActivityId { get; set; }

        public int IsUnpackOrder { get; set; }

    }
}
