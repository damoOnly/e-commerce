using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ExpressListItem
    {
        public ExpressListItem()
        {

        }

        public ExpressListItem(string code, string name, string description, int displaySequence)
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
            this.DisplaySequence = displaySequence;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplaySequence { get; set; }
    }

}
