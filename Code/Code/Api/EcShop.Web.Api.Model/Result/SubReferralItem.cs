using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SubReferralItem
    {
        public int SubReferralUserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 贡献佣金
        /// </summary>
        public decimal SubReferralSplittin { get; set; }

        /// <summary>
        /// 推广订单量
        /// </summary>
        public int ReferralOrderNumber { get; set; }


        public string HeadImgUrl { get; set; }

        public string RealName { get; set; }

        public string CellPhone { get; set; }

        public string ReferralAuditDate { get; set; }

        public string LastReferralDate { get; set; }
    }
}
