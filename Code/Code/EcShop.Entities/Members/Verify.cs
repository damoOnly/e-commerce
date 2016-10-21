using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Members
{
    /// <summary>
    /// 手机短信验证码
    /// </summary>
    public class Verify
    {
        public int VerifyId { get; set; }
        public string VerifyCode { get; set; }

        public string CellPhone { get; set; }

        public DateTime ExpiredTime { get; set; }

        public DateTime CreateTeime { get; set; }

        public int CType { get; set; }
    }
}
