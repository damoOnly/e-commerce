using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Messages
{
    public enum MessageTypeEnum
    {
        ChangedDealPassword = 1,
        ChangedPassword=2,
        ForgottenPassword=3,
        NewUserAccountCreated=4,
        OrderClosed=5,
        OrderCreated=6,
        OrderPayment=7,
        OrderRefund=8,
        OrderShipping=9,
        SiteLetter=10,
        /// <summary>
        /// 未知类型
        /// </summary>
        NoType=10000
    }
    public enum MessageT_TypeEnum
    {
        /// <summary>
        /// 会员
        /// </summary>
        UserType=1,
        /// <summary>
        /// 订单
        /// </summary>
        OrderType=2,
        /// <summary>
        ///站内信
        /// </summary>
        SiteTyoe=3,
        NoType=1000,
    }
}
