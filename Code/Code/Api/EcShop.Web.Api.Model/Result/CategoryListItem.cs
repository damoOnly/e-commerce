using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class CategoryListItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public int DisplaySequence { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Path { get; set; }
        public List<CategoryListItem> Children { get; set; }
    }
}
