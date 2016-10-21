using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class HelpCategoryListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplaySequence { get; set; }
        public string IconUrl { get; set; }
        public string Description { get; set; }
    }
}
