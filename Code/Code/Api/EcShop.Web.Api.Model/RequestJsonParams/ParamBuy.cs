using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamBuy : ParamBase
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string SkuId { get; set; }
        public int Quantity { get; set; }
    }

    public class ParamBuySingle
    {
        public int ProductId { get; set; }
        public string SkuId { get; set; }
        public int Quantity { get; set; }
    }

    public class ParamBuySku
    {
        public string SkuId { get; set; }
    }
}
