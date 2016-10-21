using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SupplierDefaultListResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public SupplierDefaultListData data { get; set; }
    }

    public class SupplierDefaultListData
    {
        public SupplierDefaultListData() { }
        public ListResult<SupplierListItem> HotSupplier { get; set; }
        public ListResult<SupplierListItem> RecommendSupplier { get; set; }
    }
}
