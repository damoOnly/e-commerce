using EcShop.Core;
using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Commodities
{
    public class ProductQuery : Pagination
    {
        [HtmlCoding]
        public string Keywords
        {
            get;
            set;
        }
        [HtmlCoding]
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
        public string MaiCategoryPath
        {
            get;
            set;
        }
        public int? BrandId
        {
            get;
            set;
        }
        public int? TagId
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
        public ProductSaleStatus SaleStatus
        {
            get;
            set;
        }
        public int? IsMakeTaobao
        {
            get;
            set;
        }
        public bool? IsIncludePromotionProduct
        {
            get;
            set;
        }
        public bool? IsIncludeBundlingProduct
        {
            get;
            set;
        }
        public PublishStatus PublishStatus
        {
            get;
            set;
        }
        public PenetrationStatus PenetrationStatus
        {
            get;
            set;
        }
        public System.DateTime? StartDate
        {
            get;
            set;
        }
        public System.DateTime? EndDate
        {
            get;
            set;
        }
        public int? TypeId
        {
            get;
            set;
        }
        public int? TopicId
        {
            get;
            set;
        }
        public bool? IsIncludeHomeProduct
        {
            get;
            set;
        }
        public int? Client
        {
            get;
            set;
        }
        public int? UserId
        {
            get;
            set;
        }
        public int? ProductLineId
        {
            get;
            set;
        }
        public bool IsAlert
        {
            get;
            set;
        }

        public int? ImportSourceId
        {
            get;
            set;
        }

        public int? SupplierId
        {
            get;
            set;
        }
        public int? IsApproved
        {
            get;
            set;
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public int? RecordStatus
        {
            get;
            set;
        }

        /// <summary>
        /// У��״̬
        /// </summary>
        public int? CheckStatus
        {
            get;
            set;
        }

        /// <summary>
        /// �̼�״̬
        /// </summary>
        public int? InspectionStaus
        {
            get;
            set;
        }

        //�����ID
        public int? ActivityId
        {
            get;
            set;
        }

        /// <summary>
        /// �����ID(���ͻ)
        /// </summary>
        public int? PresentActivityId
        {
            get;
            set;
        }

        /// <summary>
        /// �˷�ģ��Id
        /// </summary>
        public int? TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// �ͺ�
        /// </summary>
        public string ItemNo
        {
            get;
            set;
        }

        /// <summary>
        /// Ʒ��
        /// </summary>
        public string BrandName
        {
            get;
            set;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string HSProductName
        {
            get;
            set;
        }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string SkuId
        {
            get;
            set;
        }

        /// <summary>
        /// ������
        /// </summary>
        public string BarCode
        {
            get;
            set;
        }

        /// <summary>
        /// ���ر���
        /// </summary>
        public string HSCODE
        {
            get;
            set;
        }

        /// <summary>
        /// �������κŲ�ѯ
        /// </summary>
        public string BatchNo
        {
            get;
            set;
        }

        public int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// ���״̬ 0 δ��� 1���ͨ�� 2 ��˲�ͨ��  Ϊ��ʱ��ȡ���е�״̬
        /// </summary>
        public int? IsApprovedPrice
        {
            get;
            set;
        }


        /// <summary>
        /// �Ƿ�ȫ����ɹ鵵����
        /// </summary>
        public int? IsAllClassify
        {
            get;
            set;
        }

        /// <summary>
        /// 1�������� 2�������
        /// </summary>
        public int? SaleType
        {
            get;
            set;
        }


        public string MulSaleStatus
        {
            get;
            set;
        }

        public bool? HasStock
        {
            get;
            set;
        }

        public string SupplierCode
        {
            get;
            set;
        }

        public string ProductRegistrationNumber
        {
            get;
            set;
        }

        public string LJNo
        {
            get;
            set;
        }
    }
}
