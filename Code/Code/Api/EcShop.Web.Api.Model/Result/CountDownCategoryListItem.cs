using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class CountDownCategoryListItem
    {
        public CountDownCategoryListItem()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string AdImageUrl { get; set; }
        public string AdImageLinkUrl { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ProductCount { get; set; }
        public int DisplaySequence { get; set; }
    }
}
