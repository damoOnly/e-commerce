using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamThirdPartyBind : ParamBase
    {
        public string OpenIdType { get; set; }
        public string OpenId { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }

    public class ThirdAccountBind : ParamBase
    {
        public string OpenId { get; set; }
        public string OpenIdType { get; set; }
        public string CellPhone { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }


        /// <summary>
        /// 邀请码
        /// </summary>
        public string RecemmendCode { get; set; }
    }
}
