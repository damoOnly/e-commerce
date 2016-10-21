using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class MyOrderResult<T> : ListResult<T>
    {
        public int NonDeliveryCount { get; set; }

        public int DeliveryCount { get; set; }
    }
}
