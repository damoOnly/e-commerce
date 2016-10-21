using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ShippingDateModeListItem
    {
        public ShippingDateModeListItem()
        {

        }

        public ShippingDateModeListItem(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
