using EcShop.Web.Api.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.Model
{
    public class ListResult<T>
    {
        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int TotalNumOfRecords { set; get; }

        /// <summary>
        /// 返回的记录集
        /// </summary>
        public List<T> Results { set; get; }

    }

    public class ListResultWithActivity<T>
    {
        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int TotalNumOfRecords { set; get; }

        /// <summary>
        /// 返回的记录集
        /// </summary>
        public List<T> Results { set; get; }

        public string ActivityBannerImg { get; set; }
        public string ActivityDesc { get; set; }

        public List<NextCategoryItem> NextCategoryItem { get; set; }
        public List<BrandListItems> BrandListItem { get; set; }
    }
}
