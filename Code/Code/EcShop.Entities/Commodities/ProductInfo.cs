using Ecdev.Components.Validation.Validators;
using EcShop.Core;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EcShop.Entities.Commodities
{
    public class ProductInfo
    {
        private Dictionary<string, SKUItem> skus;
        private SKUItem defaultSku;
        public SKUItem DefaultSku
        {
            get
            {
                SKUItem defaultSku;
                if ((defaultSku = this.defaultSku) == null)
                {
                    defaultSku = (this.defaultSku = this.Skus.Values.Count > 0 ? this.Skus.Values.First<SKUItem>() : null);
                }
                return defaultSku;
            }
        }
        public Dictionary<string, SKUItem> Skus
        {
            get
            {
                Dictionary<string, SKUItem> skus;
                if ((skus = this.skus) == null)
                {
                    skus = (this.skus = new Dictionary<string, SKUItem>());
                }
                return skus;
            }
        }
        public string SkuId
        {
            get
            {
                return this.DefaultSku.SkuId;
            }
        }
        public string SKU
        {
            get
            {
                return this.DefaultSku != null ? this.DefaultSku.SKU : null;
            }
        }
        public decimal Weight
        {
            get
            {
                return this.DefaultSku.Weight;
            }
        }
        public int Stock
        {
            get
            {
                return this.Skus.Values.Sum((SKUItem sku) => sku.Stock);
            }
        }

        public int FactStock
        {
            get
            {
                return this.Skus.Values.Sum((SKUItem sku) => sku.FactStock);
            }
        }

        public decimal CostPrice
        {
            get
            {
                return this.DefaultSku.CostPrice;
            }
        }
        /// <summary>
        /// 购买基数
        /// </summary>
        public int BuyCardinality
        {
            get;
            set;
        }
        public decimal MinSalePrice
        {
            get
            {
                decimal[] minSalePrice = new decimal[]
				{
					79228162514264337593543950335m
				};
                foreach (SKUItem current in
                    from sku in this.Skus.Values
                    where sku.SalePrice < minSalePrice[0]
                    select sku)
                {
                    minSalePrice[0] = current.SalePrice;
                }
                return minSalePrice[0];
            }
        }
        public decimal MaxSalePrice//修改1
        {
            get
            {
                decimal[] maxSalePrice = new decimal[1];
                foreach (SKUItem item in this.Skus.Values.Where<SKUItem>(delegate(SKUItem sku)
                {
                    return sku.SalePrice > maxSalePrice[0];
                }))
                {
                    maxSalePrice[0] = item.SalePrice;
                }
                return maxSalePrice[0];
            }
        }
        public int? TypeId
        {
            get;
            set;
        }
        public int CategoryId
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
        /// 商品显示名称
        /// </summary>
        [HtmlCoding]
        public string ProductName
        {
            get;
            set;
        }
        /// <summary>
        /// 商品系统名称
        /// </summary>
        public string SysProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 商品副标题
        /// </summary>
        public string ProductTitle
        { get; set; }

        public string ProductCode
        {
            get;
            set;
        }
        [HtmlCoding]
        public string ShortDescription
        {
            get;
            set;
        }
        public string Unit
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string MobblieDescription
        {
            get
            {
                return this.MobbileDescription;
            }
            set
            {
                this.MobbileDescription = value;
            }
        }
        public string MobbileDescription
        {
            get;
            set;
        }
        [HtmlCoding]
        public string Title
        {
            get;
            set;
        }
        [HtmlCoding]
        public string MetaDescription
        {
            get;
            set;
        }
        [HtmlCoding]
        public string MetaKeywords
        {
            get;
            set;
        }
        public ProductSaleStatus SaleStatus
        {
            get;
            set;
        }
        public System.DateTime AddedDate
        {
            get;
            set;
        }
        public int VistiCounts
        {
            get;
            set;
        }
        public int SaleCounts
        {
            get;
            set;
        }
        public int ShowSaleCounts
        {
            get;
            set;
        }
        public int DisplaySequence
        {
            get;
            set;
        }
        public string ImageUrl1
        {
            get;
            set;
        }
        public string ImageUrl2
        {
            get;
            set;
        }
        public string ImageUrl3
        {
            get;
            set;
        }
        public string ImageUrl4
        {
            get;
            set;
        }
        public string ImageUrl5
        {
            get;
            set;
        }
        public string ThumbnailUrl40
        {
            get;
            set;
        }
        public string ThumbnailUrl60
        {
            get;
            set;
        }
        public string ThumbnailUrl100
        {
            get;
            set;
        }
        public string ThumbnailUrl160
        {
            get;
            set;
        }
        public string ThumbnailUrl180
        {
            get;
            set;
        }
        public string ThumbnailUrl220
        {
            get;
            set;
        }
        public string ThumbnailUrl310
        {
            get;
            set;
        }
        public string ThumbnailUrl410
        {
            get;
            set;
        }
        public decimal? MarketPrice
        {
            get;
            set;
        }
        public int? BrandId
        {
            get;
            set;
        }
        public string MainCategoryPath
        {
            get;
            set;
        }
        public string ExtendCategoryPath
        {
            get;
            set;
        }
        public bool HasSKU
        {
            get;
            set;
        }
        public bool IsfreeShipping
        {
            get;
            set;
        }
        public long TaobaoProductId
        {
            get;
            set;
        }
        public decimal? ReferralDeduct
        {
            get;
            set;
        }
        public decimal? SubMemberDeduct
        {
            get;
            set;
        }
        public decimal? SubReferralDeduct
        {
            get;
            set;
        }
        public bool IsCustomsClearance
        {
            get;
            set;
        }
        public decimal TaxRate
        {
            get;
            set;
        }

        public decimal MinTaxRate
        {
            get;
            set;
        }

        public decimal MaxTaxRate
        {
            get;
            set;
        }

        public int? TaxRateId
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
        /// 运费模版Id
        /// </summary>
        public int? SupplierId
        {
            get;
            set;
        }

        /// <summary>
        /// 运费模版实体
        /// </summary>
        public ShippingModeInfo shippingMode
        {
            get;
            set;
        }

        /// <summary>
        /// 原产地
        /// </summary>
        public int? ImportSourceId
        {
            get;
            set;
        }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否核准
        /// </summary>
        public bool IsApproved
        {
            get;
            set;
        }
        /// <summary>
        /// 商品二维码图片地址
        /// </summary>
        public string QRcode
        {
            get;
            set;
        }
        /// <summary>
        /// 提升权重值
        /// </summary>
        public decimal AdminFraction { get; set; }
        /// <summary>
        /// 权重值
        /// </summary>
        public decimal Fraction { get; set; }



        public string SupplierName
        {
            get;
            set;
        }

        public string SupplierImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位海关代码
        /// </summary>
        public string UnitCode
        {
            get;
            set;
        }
        /// <summary>
        /// 海关商品备案编号
        /// </summary>
        public string ProductRegistrationNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 海关编码表Id
        /// </summary>
        public int HSCodeId
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
        /// 报关品牌
        /// </summary>
        public string HSBrand
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
        /// 报关型号
        /// </summary>
        public string HSItemNo
        {
            get;
            set;
        }
        /// <summary>
        /// 报关单位
        /// </summary>
        public string HSUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 报关单位代码
        /// </summary>
        public string HSUnitCode
        {
            get;
            set;
        }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
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
        /// 商品规格，单个规格时使用
        /// </summary>
        public string ProductStandard
        {
            get;
            set;
        }
        /// <summary>
        /// 成分
        /// </summary>
        public string Ingredient
        {
            get;
            set;
        }

        /// <summary>
        /// 换算关系
        /// </summary>
        public int ConversionRelation
        {
            get;
            set;
        }

        //销售类型：默认值为1 表示正常，2 表示组合商品
        public int SaleType
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ProductsCombination> CombinationItemInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 是否促销商品
        /// </summary>
        public bool IsPromotion
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示折扣
        /// </summary>
        public bool IsDisplayDiscount
        {
            get;
            set;
        }

      
        /// <summary>
        /// 获取税费范围
        /// </summary>
        /// <returns></returns>
        public string GetExtendTaxRate()
        {
            string currTaxRate = (this.MinTaxRate * 100).ToString("0")+"%" + "-" + (this.MaxTaxRate * 100).ToString("0")+"%";
            string resultTaxRate;
            if (this.MinTaxRate > 0)
            {
                resultTaxRate = this.MinTaxRate == this.MaxTaxRate ? (this.MinTaxRate * 100).ToString("0")+"%" : currTaxRate;
            }
            else if (this.MinTaxRate == 0 && this.MaxTaxRate > 0)
            {
                resultTaxRate = currTaxRate;
            }
            else
            {
                resultTaxRate = (this.TaxRate * 100).ToString("0")+"%";
            }

            return resultTaxRate;
        }

        public int IsApprovedPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 商品英文名称
        /// </summary>
        public string EnglishName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启动限购：1：限购，2：非限购
        /// </summary>
        public int Purchase
        {
            get;
            set;
        }
        /// <summary>
        /// 限购天数
        /// </summary>
        public int SectionDay
        {
            get;
            set;

        }
        /// <summary>
        /// 限购数量
        /// </summary>
        public int PurchaseMaxNum
        {
            get;
            set;
        }
        /// <summary>
        /// 海关规格型号
        /// </summary>
        public string coustomSkuType
        {
            get;
            set;
        }
        /// <summary>
        /// 国检规格型号
        /// </summary>
        public string countrySku
        {
            get;
            set;
        }
        /// <summary>
        /// 监管类别
        /// </summary>
        public string beLookType
        {
            get;
            set;
        }
        /// <summary>
        /// 主要成分
        /// </summary>
        public string madeOf
        {
            get;
            set;
        }
    }
}
