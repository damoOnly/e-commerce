using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamRefund : ParamUserBase
    {
        public string OrderId { get; set; }
        public int RefundType { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }
}
