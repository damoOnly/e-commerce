using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ProductCommentListItem
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string ReviewDate { get; set; }
        public string Content { get; set; }
        public int Score { get; set; }
        public bool IsAnonymous { get; set; }

        public string HeadImgUrl { get; set; }

        public string OrderDate { get; set; }
    }

    public class ProductCommentListResult
    {
        public int TotalScore { get; set; }

        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int TotalNumOfRecords { set; get; }

        /// <summary>
        /// 返回的记录集
        /// </summary>
        public List<ProductCommentListItem> Results { set; get; }
    }
}
