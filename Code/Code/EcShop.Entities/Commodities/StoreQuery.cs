using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commodities
{
    public class StoreQuery : Pagination
    {

        public string StoreName
        {
            get;
            set;
        }

        public int StoreId
        {
            get;
            set;
        }
    }
}
