using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Messages
{
    public enum XinGeMsgType
    {
        /// <summary>
        /// 修改交易密码
        /// </summary>
        ChangeTradePassword,
        /// <summary>
        /// 修改登录密码
        /// </summary>
        ChangeLoginPassword,
        /// <summary>
        /// 找回会员登录密码
        /// </summary>
        FindLoginPassword,
        /// <summary>
        /// 会员注册
        /// </summary>
        RegistUser,
        /// <summary>
        /// 订单关闭
        /// </summary>
        OrderClose,
        /// <summary>
        /// 订单创建
        /// </summary>
        OrderCreate,
        /// <summary>
        /// 订单支付
        /// </summary>
        OrderPay,
        /// <summary>
        /// 订单退款
        /// </summary>
        OrderRefund,
        /// <summary>
        /// 订单发货
        /// </summary>
        OrderShipments
    }
}
