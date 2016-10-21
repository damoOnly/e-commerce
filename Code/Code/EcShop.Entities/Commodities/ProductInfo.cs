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
        /// �������
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
        public decimal MaxSalePrice//�޸�1
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
        /// ��Ʒ��ʾ����
        /// </summary>
        [HtmlCoding]
        public string ProductName
        {
            get;
            set;
        }
        /// <summary>
        /// ��Ʒϵͳ����
        /// </summary>
        public string SysProductName
        {
            get;
            set;
        }

        /// <summary>
        /// ��Ʒ������
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
        /// �˷�ģ��Id
        /// </summary>
        public int? TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// �˷�ģ��Id
        /// </summary>
        public int? SupplierId
        {
            get;
            set;
        }

        /// <summary>
        /// �˷�ģ��ʵ��
        /// </summary>
        public ShippingModeInfo shippingMode
        {
            get;
            set;
        }

        /// <summary>
        /// ԭ����
        /// </summary>
        public int? ImportSourceId
        {
            get;
            set;
        }

        /// <summary>
        /// ģ������
        /// </summary>
        public string TemplateName
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ��׼
        /// </summary>
        public bool IsApproved
        {
            get;
            set;
        }
        /// <summary>
        /// ��Ʒ��ά��ͼƬ��ַ
        /// </summary>
        public string QRcode
        {
            get;
            set;
        }
        /// <summary>
        /// ����Ȩ��ֵ
        /// </summary>
        public decimal AdminFraction { get; set; }
        /// <summary>
        /// Ȩ��ֵ
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
        /// ������λ���ش���
        /// </summary>
        public string UnitCode
        {
            get;
            set;
        }
        /// <summary>
        /// ������Ʒ�������
        /// </summary>
        public string ProductRegistrationNumber
        {
            get;
            set;
        }
        /// <summary>
        /// ���ر����Id
        /// </summary>
        public int HSCodeId
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
        /// ����Ʒ��
        /// </summary>
        public string HSBrand
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
        /// �����ͺ�
        /// </summary>
        public string HSItemNo
        {
            get;
            set;
        }
        /// <summary>
        /// ���ص�λ
        /// </summary>
        public string HSUnit
        {
            get;
            set;
        }

        /// <summary>
        /// ���ص�λ����
        /// </summary>
        public string HSUnitCode
        {
            get;
            set;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string Manufacturer
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
        /// ��Ʒ��񣬵������ʱʹ��
        /// </summary>
        public string ProductStandard
        {
            get;
            set;
        }
        /// <summary>
        /// �ɷ�
        /// </summary>
        public string Ingredient
        {
            get;
            set;
        }

        /// <summary>
        /// �����ϵ
        /// </summary>
        public int ConversionRelation
        {
            get;
            set;
        }

        //�������ͣ�Ĭ��ֵΪ1 ��ʾ������2 ��ʾ�����Ʒ
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
        /// �Ƿ������Ʒ
        /// </summary>
        public bool IsPromotion
        {
            get;
            set;
        }

        /// <summary>
        /// �Ƿ���ʾ�ۿ�
        /// </summary>
        public bool IsDisplayDiscount
        {
            get;
            set;
        }

      
        /// <summary>
        /// ��ȡ˰�ѷ�Χ
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
        /// ��ƷӢ������
        /// </summary>
        public string EnglishName
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ������޹���1���޹���2�����޹�
        /// </summary>
        public int Purchase
        {
            get;
            set;
        }
        /// <summary>
        /// �޹�����
        /// </summary>
        public int SectionDay
        {
            get;
            set;

        }
        /// <summary>
        /// �޹�����
        /// </summary>
        public int PurchaseMaxNum
        {
            get;
            set;
        }
        /// <summary>
        /// ���ع���ͺ�
        /// </summary>
        public string coustomSkuType
        {
            get;
            set;
        }
        /// <summary>
        /// �������ͺ�
        /// </summary>
        public string countrySku
        {
            get;
            set;
        }
        /// <summary>
        /// ������
        /// </summary>
        public string beLookType
        {
            get;
            set;
        }
        /// <summary>
        /// ��Ҫ�ɷ�
        /// </summary>
        public string madeOf
        {
            get;
            set;
        }
    }
}
