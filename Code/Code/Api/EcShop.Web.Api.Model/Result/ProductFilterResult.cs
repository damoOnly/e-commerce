using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ProductFilterResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public ProductFilterData data { get; set; }
    }

    public class ProductFilterData
    {
        public ProductFilterData() { }
        public ListResult<BrandListItem> Brand { get; set; }

        public ListResult<CategoryListItem> Category { get; set; }

        public ListResult<OriginPlaceListItem> OriginalPlace { get; set; }
       
       
    }
}
