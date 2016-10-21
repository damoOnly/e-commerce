using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Supplier
{
    public class SupplierConfigInfo
    {
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string ShortDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int DisplaySequence
        {
            get;
            set;
        }

        /// <summary>
        /// 供货商id
        /// </summary>
        public int SupplierId
        {
            get;
            set;
        }
      
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisable
        {
            get;
            set;
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public int Client
        {
            get;
            set;
        }

        /// <summary>
        /// SupplierCfgType
        /// </summary>
        public int Type
        {
            get;
            set;
        }
      
    }
}
