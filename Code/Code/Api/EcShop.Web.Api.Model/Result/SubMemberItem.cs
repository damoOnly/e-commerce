using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SubMemberItem
    {
        public int SubMemberUserId { get; set; }

        public string HeadImgUrl { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        public string CellPhone { get; set; }

        public string RealName { get; set; }
    }
}
