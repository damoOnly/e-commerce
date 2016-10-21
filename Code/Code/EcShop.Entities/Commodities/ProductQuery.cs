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
        /// 备案状态
        /// </summary>
        public int? RecordStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 校验状态
        /// </summary>
        public int? CheckStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 商检状态
        /// </summary>
        public int? InspectionStaus
        {
            get;
            set;
        }

        //促销活动ID
        public int? ActivityId
        {
            get;
            set;
        }

        /// <summary>
        /// 促销活动ID(赠送活动)
        /// </summary>
        public int? PresentActivityId
        {
            get;
            set;
        }

        /// <summary>
        /// 运费模版Id
        /// </summary>
        public int? TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// 型号
        /// </summary>
        public string ItemNo
        {
            get;
            set;
        }

        /// <summary>
        /// 品牌
        /// </summary>
        public string BrandName
        {
            get;
            set;
        }

        /// <summary>
        /// 报关名称
        /// </summary>
        public string HSProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 商品代码
        /// </summary>
        public string SkuId
        {
            get;
            set;
        }

        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode
        {
            get;
            set;
        }

        /// <summary>
        /// 海关编码
        /// </summary>
        public string HSCODE
        {
            get;
            set;
        }

        /// <summary>
        /// 备案批次号查询
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
        /// 审价状态 0 未审核 1审核通过 2 审核不通过  为空时获取所有的状态
        /// </summary>
        public int? IsApprovedPrice
        {
            get;
            set;
        }


        /// <summary>
        /// 是否全部完成归档操作
        /// </summary>
        public int? IsAllClassify
        {
            get;
            set;
        }

        /// <summary>
        /// 1代表正常 2代表组合
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
