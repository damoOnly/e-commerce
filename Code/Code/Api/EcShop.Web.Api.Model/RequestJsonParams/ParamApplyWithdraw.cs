using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamApplyWithdraw : ParamUserBase
    {
        public decimal Amount { get; set; }

        public string Account { get; set; }

        public string TradePassword { get; set; }
    }
}
