using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SupplierConfigItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int Type { get; set; }
        public string Values { get; set; }

        public int SupplierId { get; set; }

        public int DisplaySequence { get; set; }
    }
}
