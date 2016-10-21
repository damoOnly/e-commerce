using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamComment : ParamUserBase
    {
        public ParamComment()
        {
            IsAnonymous = true;
        }
        public string OrderId { get; set; }
        public string SkuId { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
        public int Score { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
