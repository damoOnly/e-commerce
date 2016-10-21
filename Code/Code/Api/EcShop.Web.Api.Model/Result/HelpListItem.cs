using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class HelpListItem
    {
        public int CategoryId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string AddedDate { get; set; }

    }
}
