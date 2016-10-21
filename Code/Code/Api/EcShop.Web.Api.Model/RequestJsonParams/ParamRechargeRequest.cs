using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamRechargeRequest : ParamUserBase
    {
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }

    }
}
