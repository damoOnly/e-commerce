using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class IosVersionResult
    {
        public string VerName { get; set; }

        public string Descript { get; set; }

        public bool IsForceUpgrade { get; set; }
    }
}
