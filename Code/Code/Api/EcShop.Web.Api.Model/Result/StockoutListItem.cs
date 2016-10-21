using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class StockoutListItem
    {
        public string SkuId { get; set; }
        public int Stock { get; set; }
        public string ProductName { get; set; }
    }
}
