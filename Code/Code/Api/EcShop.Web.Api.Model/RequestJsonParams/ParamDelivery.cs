using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamDelivery
    {
        public string order_id { get; set; }
        public string express_company_name { get; set; }
        public string tracking_number { get; set; }
        public string delivery_time { get; set; }
        public string access_token { get; set; }
    }
}
