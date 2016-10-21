using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Orders
{
    public class DeliverStatusInfo
    {
        public long Id { get; set; }

        public string OrderId { get; set; }

        public int OrderStatus { get; set; }

        public string Describe { get; set; }

        public string Warehouse { get; set; }

        public DateTime UpdateDate { get; set; }

        public string ShipOrderNumber { get; set; }

        public string ExpressCompanyName { get; set; }
    }
}
