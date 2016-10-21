using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamChangePaymentType : ParamUserBase
    {
        public string OrderId { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }

    }
}
