using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamTradePassword: ParamUserBase
    {
        public string TradePassword { get; set; }
    }

    public class ParamEditTradePassword : ParamUserBase
    {
        public string OldTradePassword { get; set; }
        public string NewTradePassword { get; set; }
    }
}
