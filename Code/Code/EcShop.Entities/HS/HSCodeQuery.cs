using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.HS
{
    public class HSCodeQuery : Pagination
    {
        /// <summary>
        /// 海关编码编号
        /// </summary>
        public int HS_CODE_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 海关编码
        /// </summary>
        public string HS_CODE
        {
            get;
            set;
        }
        /// <summary>
        /// 海关名称
        /// </summary>
        public string HS_NAME
        {
            get;
            set;
        }
        /// <summary>
        /// 海关描述
        /// </summary>
        public string HS_DESC
        {
            get;
            set;
        }
        /// <summary>
        /// 最惠国税率
        /// </summary>
        public decimal LOW_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 普通国税率
        /// </summary>
        public decimal HIGH_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 出口税率
        /// </summary>
        public decimal OUT_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 增值税率
        /// </summary>
        public decimal TAX_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 退税率
        /// </summary>
        public decimal TSL_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 消费税
        /// </summary>
        public decimal CONSUMPTION_RATE
        {
            get;
            set;
        }
    }
}
