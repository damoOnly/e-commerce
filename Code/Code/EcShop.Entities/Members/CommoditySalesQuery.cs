using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members
{
    public class CommoditySalesQuery : Pagination
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public System.DateTime? StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public System.DateTime? EndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName
        {
            get;
            set;
        }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode
        {
            get;
            set;
        }
    }
}
