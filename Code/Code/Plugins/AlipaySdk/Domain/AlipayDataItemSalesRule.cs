using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayDataItemSalesRule Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayDataItemSalesRule : AopObject
    {
        /// <summary>
        /// 购买人群限制集合，多个类型以逗号分隔
        /// </summary>
        [XmlElement("buyer_crowd_limit")]
        public string BuyerCrowdLimit { get; set; }

        /// <summary>
        /// 商品单日销售上限
        /// </summary>
        [XmlElement("daily_sales_limit")]
        public long DailySalesLimit { get; set; }

        /// <summary>
        /// 用户购买策略
        /// </summary>
        [XmlArray("user_sales_limit")]
        [XmlArrayItem("string")]
        public List<string> UserSalesLimit { get; set; }
    }
}
