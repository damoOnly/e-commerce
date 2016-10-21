using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commodities
{
    public class SupplierQuery : Pagination
    {

        public string SupplierName
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public int? DateContrastType { get; set; }

        public int? DateContrastValue { get; set; }

        public string DataVersion { get; set; }
    }
}
