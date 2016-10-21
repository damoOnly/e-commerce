using System;
namespace EcShop.Entities.Orders
{
    public enum OrderType
    {
        Normal=1,
        WillSplit,
        AlreadySplit,
        WillMerge,
        AlreadyMerge,
    }
}
