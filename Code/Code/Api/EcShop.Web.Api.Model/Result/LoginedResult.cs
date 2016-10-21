using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.Model.Result
{
    /// <summary>
    /// 登录返回类
    /// By Ocean.deng @ 20131017 add
    /// </summary>
    [Serializable]
    public class LoginedResult
    {
        /// <summary>
        /// Username set by job seeker for display in JobsDB website after logged on.
        /// </summary>
        public string DisplayName { set; get; }

        /// <summary>
        /// Authentication ticket,need to pass in cross country request.
        /// </summary>
        public string AuthenTicket { set; get; }

        /// <summary>
        /// Authentication user ID,need to pass in cross country request.
        /// </summary>
        public string AuthenUserId { set; get; }

        public long Timestamp { get; set; }
    }
}
