using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SupplierListItem
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string ShopOwner { get; set; }

        public string Location { get; set; }
        public string Image { get; set; }

        public string CreateDate { get; set; }

        public int IsCollect { get; set; }

        public int CollectCount { get; set; }

        public string Background { get; set; }
    }

   
}
