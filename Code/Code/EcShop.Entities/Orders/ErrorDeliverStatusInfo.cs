using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Orders
{
    public class ErrorDeliverStatusInfo : DeliverStatusInfo
    {
        public string errorcode { get; set; }

        public string errordescr { get; set; }
    }
}
