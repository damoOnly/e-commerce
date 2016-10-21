using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class FavoriteListItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Tag { get; set; }
        public string Remark { get; set; }
        public string ProductName { get; set; } 
        public string ThumbnailUrl { get; set; } 
        public decimal MarketPrice { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
    }
}
