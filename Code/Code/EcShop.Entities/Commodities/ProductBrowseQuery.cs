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
        public int? ImportsourceId//ԭ����
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

        //�Ƿ��п��
        public bool HasStock
        {
            get;
            set;
        }

        //��Ӧ�̣����ŵ�
        public int? supplierid
        {
            get;
            set;
        }

        /// <summary>
        /// ��Ʒ�޸�ʱ�䣬�Ա�����0�����С�1��Сʱ��2���졢3����
        /// </summary>
        public int? DateContrastType { get; set; }

        /// <summary>
        /// ��Ʒ�޸�ʱ��Ա�ֵ
        /// �Ա�����Ϊ0ʱ���˲���ֵ��Ч��
        /// �Ա�����Ϊ1ʱ���˲���ֵΪ1~23֮��
        /// �Ա�����Ϊ2ʱ���β���ֵΪ1~28֮��
        /// �Ա�����Ϊ3ʱ���˲���ֵΪ1~11֮��
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
