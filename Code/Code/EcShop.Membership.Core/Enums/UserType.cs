using System;
namespace EcShop.Membership.Core.Enums
{
    /// <summary>
    /// 注册用户类型，枚举值和订单来源OrderSource一致
    /// </summary>
    public enum UserType
    {
        All,
        PC=1,
        WeiXin=3,
        Wap=4,
        Android = 11,
        IOS = 12,
        ThirdPartyBind
    }
}
