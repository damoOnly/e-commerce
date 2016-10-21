using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamReplacement : ParamUserBase
    {
        public string OrderId { get; set; }
        public string Remark { get; set; }
        public string ExpressCompany { get; set; }
        public string TrackingNumber { get; set; }
        public List<ParamReplacementSku> Skus { get; set; }
    }

    public class ParamReplacementSku
    {
        public string SkuId { get; set; }
        public int Quantity { get; set; }
    }
}
