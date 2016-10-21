using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.PO
{
    public class PODeclareListQuery : Pagination
    {
        /// <summary>
        /// PO编码
        /// </summary>
        public string PONumber
        {
            get;
            set;
        }
        ///// <summary>
        ///// PO报关单状态
        ///// </summary>
        //public int POCDStatus
        //{
        //    set;
        //    get;
        //}
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
        /// 海关入库单号
        /// </summary>
        public string fromID
        {
            set;
            get;
        }
    }
}
