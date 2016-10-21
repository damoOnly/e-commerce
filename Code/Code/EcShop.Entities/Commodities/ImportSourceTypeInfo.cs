using EcShop.Core;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EcShop.Entities.Commodities
{
    public class ImportSourceTypeInfo
    {
        public int ImportSourceId
        {
            get;
            set;
        }

        public string EnArea
        {
            get;
            set;
        }

        public string CnArea
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public string Icon
        {
            get;
            set;
        }

        public int? DisplaySequence
        {
            get;
            set;
        }

        public DateTime? AddTime
        {
            get;
            set;
        }

        /// <summary>
        /// 海关代码
        /// </summary>
        public string HSCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否最惠国
        /// </summary>
        public bool FavourableFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 标准国家中文名称
        /// </summary>
        public string StandardCName
        {
            get;
            set;
        }
        
    }
}
