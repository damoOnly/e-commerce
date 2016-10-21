using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class QRCodeResult
    {
        public string title { get; set; }

        public string summary { get; set; }

        public string targetUrl { get; set; }

        public string imageUrl { get; set; }
    }
}
