using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ProductCollectResult
    {
        public List<FavoriteListItem> Results { get; set; }

        public int SupplierCollectCount { get; set; }

        public int ProductCollectCount { get; set; }
    }
}
