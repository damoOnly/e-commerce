using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class CommissionInfo
    {
        public string OrderId { get; set; }

        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal SplittinAmount { get; set; }

        /// <summary>
        /// 佣金类型
        /// </summary>
        public string SplittinType { get; set; }

        /// <summary>
        /// 佣金分配日期
        /// </summary>
        public string SplittinDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
