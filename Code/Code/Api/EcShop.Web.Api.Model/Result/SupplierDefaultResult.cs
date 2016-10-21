using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SupplierDefaultResult
    {
        public int code { get; set; }
        public string msg { get; set; }

        public SupplierData data { get; set; }
    }

    public class SupplierData
    {
        public SupplierData() { }
        public SupplierListItem SupplierDetail { get; set; }
        public ListResult<TopicListItem> Topic { get; set; }
        public ListResult<SupplierConfigItem> HotSale { get; set; }
        public ListResult<SupplierConfigItem> Recommend { get; set; }
       
    }
}
