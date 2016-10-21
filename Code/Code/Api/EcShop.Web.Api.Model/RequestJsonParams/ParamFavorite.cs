using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamFavorite : ParamUserBase
    {
        public int ProductId { get; set; }
        public string Tag { get; set; }
        public string Remark { get; set; }
    }

    public class ParamRemoveFavorite : ParamUserBase
    {
        public string ProductIds { get; set; }
     }
}
