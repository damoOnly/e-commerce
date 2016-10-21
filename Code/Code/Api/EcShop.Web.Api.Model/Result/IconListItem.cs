using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class IconListItem
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public int DisplaySequence { get; set; }
    }
}
