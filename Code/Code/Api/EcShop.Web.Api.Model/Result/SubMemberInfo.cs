using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SubMemberInfo
    {
        public string UserName { get; set; }

        public string RealName { get; set; }

        public string Telphone { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

       
        //public decimal Amount { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string CreateTime { get; set; }


      
    }
}
