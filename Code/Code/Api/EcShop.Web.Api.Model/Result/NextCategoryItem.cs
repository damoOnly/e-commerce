using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class NextCategoryItem
    {
        public NextCategoryItem()
        {
            this.NextCategory = new List<NextCategory>();
        }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<NextCategory> NextCategory { get; set; }
    }
    public class NextCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
