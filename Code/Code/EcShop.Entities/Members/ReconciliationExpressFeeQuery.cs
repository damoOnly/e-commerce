using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members
{
    public class ReconciliationExpressFeeQuery : Pagination
    {

        public string BeginTime
        {
            get;
            set;
        }
        public string EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商
        /// </summary>
        public int Supplier
        {
            get;
            set;
        }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string DetailsType
        {
            get;
            set;
        }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string PayDate
        {
            get;
            set;
        }
    }
}
