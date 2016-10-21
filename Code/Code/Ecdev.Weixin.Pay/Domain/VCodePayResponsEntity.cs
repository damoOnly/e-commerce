using Ecdev.Weixin.Pay.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecdev.Weixin.Pay.Domain
{
    /// <summary>
    /// 创建微信支付响应信息
    /// </summary>
    [Serializable]
    public class VCodePayResponsEntity
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// mch_id
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// device_info
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        private string nonce_str = VCodePayHelper.CreateRandom(20);
        /// <summary>
        /// 随机字符串 默认自动创建 20位字符串
        /// </summary>
        public string Nonce_str
        {
            get { return nonce_str; }
            set { nonce_str = value; }
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        ///// <summary>
        ///// 业务结果
        ///// </summary>
        //public string result_code { get; set; }
        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 二维码链接
        /// </summary>
        public string code_url { get; set; }
    }
}
