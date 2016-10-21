using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamPay : ParamUserBase
    {
        public string OrderId { get; set; }
        public string TradeTime { get; set; }
        public string TradeNo { get; set; }
        public decimal TradeAmount { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public string Signature { get; set; }

    }

    public class ParamPrepay : ParamUserBase
    {
        public string OrderId { get; set; }
        public string TradeNo { get; set; }
        public decimal TradeAmount { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public string Signature { get; set; }

    }
}
