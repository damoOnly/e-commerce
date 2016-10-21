using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamBase
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string accessToken { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public int channel { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public int platform { get; set; }

        /// <summary>
        /// Api的版本
        /// </summary>
        public string ver { get; set; }
    }
}
