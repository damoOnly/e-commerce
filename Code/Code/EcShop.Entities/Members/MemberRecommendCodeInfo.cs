using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members
{
    /// <summary>
    /// 注册邀请码信息
    /// </summary>
    public class MemberRecommendCodeInfo
    {
        public MemberRecommendCodeInfo()
        {
            this.NextRecommendCode = new List<NextRecommendCode>();
        }

        /// <summary>
        /// 总邀请人数
        /// </summary>
        public int TotalNum
        {
            get;
            set;
        }

        /// <summary>
        /// 本周排名位
        /// </summary>
        public int WeekLevel
        {
            get;
            set;
        }

        public List<NextRecommendCode> NextRecommendCode { get; set; }
    }

    public class NextRecommendCode
    {
        public string CellPhone { get; set; }
    }
}
