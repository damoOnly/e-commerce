using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class PrepayResult
    {
        public string prepay_id { get; set; }
        public string order_id { get; set; }
        public string out_trade_no { get; set; }
        public string timestamp { get; set; }
        public string noncestr { get; set; }
        public string sign { get; set; }
    }
}
