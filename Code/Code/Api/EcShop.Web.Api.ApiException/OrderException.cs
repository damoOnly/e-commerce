using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.ApiException
{
    public class OrderException : Exception
    {
        public OrderException()
        {
        }

        public OrderException(string message)
            : base(message)
        {
        }
    }
}
