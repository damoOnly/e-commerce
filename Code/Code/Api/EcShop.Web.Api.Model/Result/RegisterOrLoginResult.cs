using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.Model.Result
{
    /// <summary>
    /// 注册或登陆后返回的json字段
    /// @added by kezesong 
    /// </summary>
    public class RegisterOrLoginResult
    {
        /// <summary>
        /// Username set by job seeker for display in JobsDB website after logged on.
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Authentication ticket, need to pass in cross country request
        /// </summary>
        public string AuthenTicket
        {
            get;
            set;
        }

        /// <summary>
        /// Authentication user ID, need to pass in cross country request
        /// </summary>
        public string AuthenUserId
        {
            get;
            set;
        }

        public long Timestamp { get; set; }
    }

    public class BindThirdPartyResult : RegisterOrLoginResult
    {
       

        /// <summary>
        /// 是否新注册用户
        /// </summary>
        public bool flag
        {
            get;
            set;
        }

        public string openId
        {
            get;
            set;
        }


    }
}
