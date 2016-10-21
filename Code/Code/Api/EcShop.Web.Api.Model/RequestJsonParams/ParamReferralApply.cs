using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamReferralApply : ParamUserBase
    {
        public string RealName { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string ReferralReason { get; set; }
    }
}
