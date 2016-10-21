using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class OrderReviewListItem
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public string SkuId { get; set; }
        public string ProductName { get; set; }
        public string SkuContent { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public string ThumbnailsUrl { get; set; }
        public string Content { get; set; }
        public string ReviewDate { get; set; }
        public int Score { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsCanReview { get; set; }
    }
}
