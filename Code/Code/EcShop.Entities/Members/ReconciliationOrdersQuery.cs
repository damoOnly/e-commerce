using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members
{
    public class ReconciliationOrdersQuery : Pagination
    {

        public System.DateTime? StartDate
        {
            get;
            set;
        }
        public System.DateTime? EndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier
        {
            get;
            set;
        }
    }
}
