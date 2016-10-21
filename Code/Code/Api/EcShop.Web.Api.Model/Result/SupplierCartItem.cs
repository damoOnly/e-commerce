using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SupplierCartItem
    {
        public SupplierCartItem()
        {
            this.CartItems = new List<CartItem>();
        }
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string Logo { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
