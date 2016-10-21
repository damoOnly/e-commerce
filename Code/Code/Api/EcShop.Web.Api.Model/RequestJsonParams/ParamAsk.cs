using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamQuestion : ParamUserBase
    {
        public int ProductId { get; set; }
        public string Content { get; set; }
    }
}
