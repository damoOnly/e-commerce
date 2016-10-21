using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commodities
{
    public class SupplierCollectQuery : Pagination
    {
        public int UserId { get; set; }
    }
}
