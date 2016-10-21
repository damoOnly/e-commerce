using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.PO
{
    public class PurchaseOrderInfo
    {
        public int id
        {
            set;
            get;
        }
        /// <summary>
        /// PO订单号：PO1602250001
        /// </summary>
        public string PONumber
        {
            set;
            get;
        }
        /// <summary>
        /// 单据类型（1：采购订单）
        /// </summary>
        public int POType
        {
            set;
            get;
        }
        /// <summary>
        /// 订单状态（0：创建；1：招商确认；2：关务认领；3：关务申报；4：申报成功；5：申报失败；6：已入库）
        /// </summary>
        public int POStatus
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public int? SupplierId
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            set;
            get;
        }
        /// <summary>
        /// 预计到货时间
        /// </summary>
        public DateTime? ExpectedTime
        {
            set;
            get;
        }

        /// <summary>
        /// 是否发送WMS（0：未发送；1：已发送；2：发送失败）
        /// </summary>
        public int SendWMSByPO
        {
            set;
            get;
        }
        /// <summary>
        /// 发送WMS次数，尝试10次
        /// </summary>
        public int WMSCount
        {
            set;
            get;
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
        /// 备注
        /// </summary>
        public string Remark
        {
            set;
            get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set;
            get;
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserId
        {
            set;
            get;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime
        {
            set;
            get;
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public int UpdateUserId
        {
            set;
            get;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel
        {
            set;
            get;
        }
        /// <summary>
        /// 发船日期
        /// </summary>
        public DateTime? DepartTime
        {
            set;
            get;
        }
        /// <summary>
        /// 到港日期
        /// </summary>
        public DateTime? ArrivalTime
        {
            set;
            get;
        }
        /// <summary>
        /// 柜号
        /// </summary>
        public string ContainerNumber
        {
            set;
            get;
        }
        /// <summary>
        /// 提单号
        /// </summary>
        public string BillNo
        {
            set;
            get;
        }
        /// <summary>
        /// 启运港
        /// </summary>
        public string StartPort
        {
            set;
            get;
        }
        /// <summary>
        /// 目的港
        /// </summary>
        public string EndPort
        {
            set;
            get;
        }
        /// <summary>
        /// 目的港ID
        /// </summary>
        public string EndPortID
        {
            set;
            get;
        }
        /// <summary>
        /// 启运港ID
        /// </summary>
        public string StartPortID
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int? ExtendInt1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ExtendInt2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ExtendInt3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar4
        {
            set;
            get;
        }
    }
}
