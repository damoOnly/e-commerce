using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamChangePassword : ParamUserBase
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

    public class ParamResetPassword : ParamUserBase
    {
        public string Password { get; set; }
        public string Code { get; set; }
        public string Mobile { get; set; }
    }
}
