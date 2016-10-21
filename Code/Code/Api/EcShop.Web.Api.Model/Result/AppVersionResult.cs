using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class AppAndroidVersionResult
    {
        public string appname { get; set; }
        public string apkname { get; set; }
        public string verName { get; set; }
        public int verCode { get; set; }
        public string url { get; set; }
        public string infoUrl { get; set; }
        public string info { get; set; }

        public bool IsForcibleUpgrade { get; set; }
    }
}
