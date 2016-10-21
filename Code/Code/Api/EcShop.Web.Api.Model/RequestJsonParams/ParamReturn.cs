using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamReturn : ParamUserBase
    {
        public string OrderId { get; set; }
        public int ReturnType { get; set; }
        public string ExpressCompany { get; set; }
        public string TrackingNumber { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public List<ParamReturnSku> Skus { get; set; }
    }

    public class ParamReturnSku
    {
        public string SkuId { get; set; }
        public int Quantity { get; set; }
    }
}
