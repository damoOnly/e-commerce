using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class BrandListItem
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int DisplaySequence { get; set; }

    }

    public class BrandListItems
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
