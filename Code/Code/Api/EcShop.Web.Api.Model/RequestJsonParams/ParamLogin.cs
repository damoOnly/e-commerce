using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamLogin : ParamBase
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
