using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ReferralSubUserInfo
    {
        public string UserName { get; set; }

        public string RealName { get; set; }

        public string Telphone { get; set; }

        /// <summary>
        /// 推广订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 贡献佣金
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 审核通过日期
        /// </summary>
        public string AuditTime{get;set;}


        /// <summary>
        /// 最后一次推广日期
        /// </summary>
        public string LastOrderTime{get;set;}

        
    }
}
