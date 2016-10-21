using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.HS
{
    public class HSDeclareQuery
    {
        public Pagination Page
		{
			get;
			set;
		}
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 申报状态
        /// </summary>
        public string DeclareStatus
        {
            get;
            set;
        }
		public System.DateTime? FromDate
		{
			get;
			set;
		}
		public System.DateTime? ToDate
		{
			get;
			set;
		}
        /// <summary>
        /// 操作人
        /// </summary>
		public string OperationUserName
		{
			get;
			set;
		}

        /// <summary>
        /// 出库单号
        /// </summary>
        public string OutNo
        {
            get;
            set;
        }

        /// <summary>
        /// 运单号
        /// </summary>
        public string LogisticsNo
        {
            get;
            set;
        }

        /// <summary>
        /// 物流运输编号
        /// </summary>
        public string ShipOrderNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 申报放行状态
        /// </summary>
        public string WMSStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 挂起时间
        /// </summary>
        public int SuspensionTime
        {
            get;
            set;
        }

        public HSDeclareQuery()
		{
			this.Page = new Pagination();
		}
    }
}
