using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.VShop
{
    public class ShareInfo
    {
        public int ShareId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 分享时间
        /// </summary>
        public System.DateTime ShareTime
        {
            get;
            set;
        }


        /// <summary>
        /// 分享次数 默认为0
        /// </summary>
        public int ShareTimes
        {
            get;
            set;
        }

        /// <summary>
        /// 分享者
        /// </summary>
        public string SharePerson
        {
            get;
            set;
        }

        /// <summary>
        /// 分享内容
        /// </summary>
        public string ShareContent
        {
            get;
            set;
        }

        //当前链接
        public string Link
        {
            get;
            set;
        }

        /// <summary>
        /// 分享类型：1：分享到朋友圈 2：分享给朋友 3：分享到QQ  4：分享到微博 5：分享到QQ空间
        /// </summary>
        public int ShareType
        {
            get;
            set;
        }


        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// 订单Id 
        /// </summary>
        public string OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 分享人用户ID
        /// </summary>
        public int ShareUserId
        {
            get;
            set;
        }

    }
}
