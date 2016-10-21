using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamMember : ParamUserBase
    {
        public string RealName { get; set; }
        public string QQ { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public int Gender { get; set; }

        public string IdNo { get; set; }
    }

    public class ParamReferral : ParamMember
    {
        public string ReferralReason;
    }
}
