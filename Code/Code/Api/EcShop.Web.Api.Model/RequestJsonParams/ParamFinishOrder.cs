using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamFinishOrder : ParamUserBase
    {
        public string OrderId { get; set; }

        public string SourceOrderId { get; set; }

        public int Type { get; set; }
    }
}
