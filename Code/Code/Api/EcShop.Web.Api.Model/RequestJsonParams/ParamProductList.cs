using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamProductList :ParamUserBase
    {
        public int pageIndex{get;set;}

        public int pageSize{get;set;}

        public string  keyword{get;set;}

        public int sortType{get;set;}

        public string  sortDirection{get;set;}

        public int TopicId { get; set; }

        public string CategoryIds{get;set;}

        public string BrandIds { get; set; }

        public string ImportsourceIds { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public int SupplierId { get; set; }
    }
}
