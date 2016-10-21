using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecdev.Weixin.Pay.Domain
{
    [Serializable]
    public class VCodePayEntity
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid = "wx45cd46b5f561deee";
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string Appid { get { return appid; } set { appid = value; } }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id = "1235278902";
        /// <summary>
        /// 商户号
        /// </summary>
        public string Mch_id { get { return mch_id; } set { mch_id = value; } }
        /// <summary>
        /// 设备号
        /// </summary>
        private string device_info = "WEB";
        /// <summary>
        /// 设备号
        /// </summary>
        public string Device_info
        {
            get { return device_info; }
            set { device_info = value; }
        }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 商品描述
        public string body { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 货币类型
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 交易起始时间
        /// </summary>
        public string time_start { get; set; }
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public string time_expire { get; set; }
        /// <summary>
        /// 商品标记
        /// </summary>
        public string goods_tag { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        private string trade_type="NATIVE";
        /// <summary>
        /// 交易类型
        /// </summary>
        public string Trade_type { get { return trade_type; } set { trade_type = value; } }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string product_id { get; set; }
        /// <summary>
        /// 指定支付方式
        /// </summary>
        public string limit_pay { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }

    }
}
