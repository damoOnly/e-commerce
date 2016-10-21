using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.YJF
{
    public class YJFResponse
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string sign
        {
            get;
            set;
        }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string signType
        {
            get;
            set;
        }
        /// <summary>
        /// HTTP协议类型
        /// </summary>
        public string protocol
        {
            get;
            set;
        }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string tradeNo
        {
            get;
            set;
        }
        /// <summary>
        /// 系统生成ID(对应易极付订单ID)
        /// </summary>
        public string orderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 返回码
        /// </summary>
        public string resultCode
        {
            get;
            set;
        }
        /// <summary>
        /// 接口服务代码
        /// </summary>
        public string service
        {
            get;
            set;
        }
        /// <summary>
        /// 成功标识
        /// </summary>
        public string success
        {
            get;
            set;
        }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string resultMessage
        {
            get;
            set;
        }
        /// <summary>
        /// 支付人证件号码
        /// </summary>
        public string partnerId
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string version
        {
            get;
            set;
        }
        /// <summary>
        /// 处理结果列表
        /// </summary>
        public List<resultInfos> resultInfos
        {
            get;
            set;
        }
        /// <summary>
        /// 异步处理结果列表
        /// </summary>
        public List<resultInfosAsy> resultInfosAsy
        {
            get;
            set;
        }
        /// <summary>
        /// 原始订单(平台订单ID)
        /// </summary>
        public string outOrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public string serviceStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 异步回传时间
        /// </summary>
        public string notifyTime
        {
            get;
            set;
        }

    }

    public class resultInfos
    {
        /// <summary>
        /// 回传消息描述
        /// </summary>
        public string message
        {
            get;
            set;
        }
        /// <summary>
        /// 原始订单
        /// </summary>
        public string outOrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string paymentBillMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 同步回传数据状态
        /// </summary>
        public string serviceStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 异步回传数据状态
        /// </summary>
        public string status
        {
            get;
            set;
        }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string tradeNo
        {
            get;
            set;
        }
        /// <summary>
        /// 流水余额
        /// </summary>
        public string availableBalance
        {
            get;
            set;
        }
    }

    public class resultInfosAsy
    {
        /// <summary>
        /// 回传消息描述
        /// </summary>
        public string message
        {
            get;
            set;
        }
        /// <summary>
        /// 异步回传数据状态
        /// </summary>
        public string status
        {
            get;
            set;
        }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string tradeNo
        {
            get;
            set;
        }
        /// <summary>
        /// 原始订单
        /// </summary>
        public string outOrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// 流水余额
        /// </summary>
        public decimal availableBalance
        {
            get;
            set;
        }
    }

    public class CertNoValidResponse : YJFResponse
    {


        /// <summary>
        /// 易极付身份证验证结果
        /// </summary>
        public string realNameQueryResult
        {
            get;
            set;
        }
    }
    //public class YJFsynResponse : YJFResponse
    //{
    //}

    //public class YJFasyResponse : YJFResponse
    //{
    //}
}
