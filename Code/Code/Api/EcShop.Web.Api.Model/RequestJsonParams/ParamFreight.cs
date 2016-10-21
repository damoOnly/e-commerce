using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamFreight : ParamUserBase
    {
        public ParamFreight()
        {
            Skus = new List<ParamFreightSku>();
        }
        public int RegionId { get; set; }
        public List<ParamFreightSku> Skus { get; set; }
    }
 
    public class ParamFreightSku
    {
        public string SkuId { get; set; }
    }
}
