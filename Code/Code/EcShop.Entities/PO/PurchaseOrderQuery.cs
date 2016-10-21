using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.PO
{
    public class PurchaseOrderQuery : Pagination
    {
        /// <summary>
        /// PO订单号：PO1602250001
        /// </summary>
        public string PONumber
        {
            set;
            get;
        }
        /// <summary>
        /// PO状态（0：创建；1：招商确认；2：关务认领；3：关务申报；4：申报成功；5：申报失败；6：已入库）
        /// </summary>
        public int POStatus
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int? SupplierId
        {
            set;
            get;
        }
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
        public string HSInOut
        {
            set;
            get;
        }
        /// <summary>
        /// QP状态
        /// </summary>
        public int QPStatus
        {
            set;
            get;
        }
        /// <summary>
        /// 商检状态
        /// </summary>
        public int CIStatus
        {
            set;
            get;
        }
    }
}
