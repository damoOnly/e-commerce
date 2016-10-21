using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.YJF
{
    public class YJFRequest
    {
        /// <summary>
        /// 电商企业代码
        /// </summary>
        public string eshopEntCode
        {
            get;
            set;
        }
        /// <summary>
        /// 电商企业名称
        /// </summary>
        public string eshopEntName
        {
            get;
            set;
        }
        /// <summary>
        /// 电商类型
        /// </summary>
        public string eshopType
        {
            get;
            set;
        }
        /// <summary>
        /// 易极付调用接口
        /// </summary>
        public string yijifuUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 接口服务代码
        /// </summary>
        public string serviceCode
        {
            get;
            set;
        }
        /// <summary>
        /// 易极付账号对应的合作方ID
        /// </summary>
        public string partnerId
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
        /// 页面跳转返回URL
        /// </summary>
        public string returnUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 异步通知URL
        /// </summary>
        public string notifyUrl
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
        /// 签名
        /// </summary>
        public string sign
        {
            get;
            set;
        }

        /// <summary>
        /// 易极付Key
        /// </summary>
        public string YJFPaySignKey
        {
            get;
            set;
        }
        /// <summary>
        /// 订单列表
        /// </summary>
        public string voList
        {
            get;
            set;
        }
    }
    public class vosList
    {
        /// <summary>
        /// 订单金额
        /// </summary>
        public string paymentBillAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 真实人姓名
        /// </summary>
        public string payerName
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证信息
        /// </summary>
        public string payerId
        {
            get;
            set;
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string paymentType
        {
            get;
            set;
        }

        /// <summary>
        /// 海关代码
        /// </summary>
        public string customsCode
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
        /// 交易流水号
        /// </summary>
        public string tradeNo
        {
            get;
            set;
        }
    }

    public class CertNoValidRequest : YJFRequest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string realName
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证
        /// </summary>
        public string certNo
        {
            get;
            set;
        }
    }

}
