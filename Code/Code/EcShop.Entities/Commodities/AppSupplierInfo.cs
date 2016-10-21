using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commodities
{
    public class AppSupplierInfo : SupplierInfo
    {
        public int IsCollect { get; set; }

        public int CollectCount { get; set; }
    }
}
