using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamSendMessage : ParamUserBase
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
