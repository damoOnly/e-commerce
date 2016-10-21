using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamTicketRefresh : ParamUserBase
    {
        public string Signature { get; set; }
        public string Timestamp { get; set; }
        public string Ticket { get; set; }

    }
}
