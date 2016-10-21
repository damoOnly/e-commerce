using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.Model.Result
{
    public class AccessTokenResult
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string SessionKey { get; set; }
        public string SessionSecret { get; set; }
    }
}
