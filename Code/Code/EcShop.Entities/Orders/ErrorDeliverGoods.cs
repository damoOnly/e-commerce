using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Orders
{
    public class ErrorDeliverGoods
    {
        public string orderid { get; set; }

        public string ShipOrderNumber { get; set; }

        public string ExpressCompanyName { get; set; }

        public string ExpressCompanyCode { get; set; }

        public string ExpressDate { get; set; }

        public string errorcode { get; set; }

        public string errordescr { get; set; }
    }
}
