using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Orders
{
    public class SimpleSKUUpdateInfo
    {
        public string SkuId { get; set; }

        public int Amount { get; set; }

        public decimal GrossWeight { get; set; }
    }
}
