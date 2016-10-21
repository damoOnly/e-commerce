using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class Kuaidi100ExpressJson
    {
        public Kuaidi100ExpressJson()
        {
            this.data = new List<Kuaidi100ExpressJsonTracking>();
        }
        public string nu { get; set; }
        public string message { get; set; }
        public string companytype { get; set; }
        public string ischeck { get; set; }
        public string com { get; set; }
        public string updatetime { get; set; }
        public string status { get; set; }
        public string condition { get; set; }
        public string codenumber { get; set; }
        public string state { get; set; }
        public List<Kuaidi100ExpressJsonTracking> data { get; set; }
    }

    public class Kuaidi100ExpressJsonTracking
    {
        public string time { get; set; }
        public string ftime { get; set; }
        public string context { get; set; }
    }
    
}
