using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
namespace EcShop.Entities.Commodities
{
    public class ProductBrowseQuery : Pagination
    {
        private System.Collections.Generic.IList<AttributeValueInfo> attributeValues;
        private ProductSaleStatus productSaleStatus = ProductSaleStatus.OnSale;
        public ProductSaleStatus ProductSaleStatus
        {
            get
            {
                return this.productSaleStatus;
            }
            set
            {
                this.productSaleStatus = value;
            }
        }
        public bool IsPrecise
        {
            get;
            set;
        }
        public string TagIds
        {
            get;
            set;
        }
        public string Keywords
        {
            get;
            set;
        }

        public string SubKeywords
        {
            get;
            set;
        }

        public string ProductCode
        {
            get;
            set;
        }
        public int? CategoryId
        {
            get;
            set;
        }
        public string StrCategoryId
        {
            get;
            set;
        }
        public int? BrandId
        {
            get;
            set;
        }
        public string StrBrandId
        {
            get;
            set;
        }
        public decimal? MinSalePrice
        {
            get;
            set;
        }
        public decimal? MaxSalePrice
        {
            get;
            set;
        }
        public int? ImportsourceId//原产地
        {
            get;
            set;
        }
        public string StrImportsourceId
        {
            get;
            set;
        }

        public int? TopId;
        public System.Collections.Generic.IList<AttributeValueInfo> AttributeValues
        {
            get
            {
                if (this.attributeValues == null)
                {
                    this.attributeValues = new System.Collections.Generic.List<AttributeValueInfo>();
                }
                return this.attributeValues;
            }
            set
            {
                this.attributeValues = value;
            }
        }

        //是否有库存
        public bool HasStock
        {
            get;
            set;
        }

        //供应商，即门店
        public int? supplierid
        {
            get;
            set;
        }

        /// <summary>
        /// 商品修改时间，对比类型0：所有、1：小时、2：天、3：月
        /// </summary>
        public int? DateContrastType { get; set; }

        /// <summary>
        /// 商品修改时间对比值
        /// 对比类型为0时，此参数值无效；
        /// 对比类型为1时，此参数值为1~23之间
        /// 对比类型为2时，次参数值为1~28之间
        /// 对比类型为3时，此参数值为1~11之间
        /// </summary>
        public int? DateContrastValue { get; set; }

        public string DataVersion { get; set; }

        public int? IsApproved
        {
            get;
            set;
        }

        public int? SendWMSCount { get; set; }
    }
}
