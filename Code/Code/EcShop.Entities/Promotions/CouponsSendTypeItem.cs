using EcShop.Core;
using Ecdev.Components.Validation;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.Promotions
{
    public class CouponsSendTypeItem
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public CouponsSendTypeItem()
        {

        }

        public int CouponId { get; set; }

        public int BindId { get; set; }

        public int UserId { get; set; }


    }
}
