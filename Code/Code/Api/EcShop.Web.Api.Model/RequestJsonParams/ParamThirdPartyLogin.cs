using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamThirdPartyLogin : ParamBase
    {
        public string OpenIdType { get; set; }
        public string OpenId { get; set; }
    }
}
