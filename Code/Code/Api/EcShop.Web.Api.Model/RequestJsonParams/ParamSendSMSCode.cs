using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamSendSMSCode : ParamBase
    {
        public string Mobile { get; set; }

        //类型：1为注册 2：重置密码，发送短信验证码
        public int CType { get; set; }
    }
}
