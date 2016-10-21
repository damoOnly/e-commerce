using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.PO
{
    public class PurchaseOrderItemInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int POId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string SkuId
        {
            set;
            get;
        }
        /// <summary>
        /// 外箱条码
        /// </summary>
        public string BoxBarCode
        {
            set;
            get;
        }
        /// <summary>
        /// 期望数量
        /// </summary>
        public int? ExpectQuantity
        {
            set;
            get;
        }
        /// <summary>
        /// 实际到货数量
        /// </summary>
        public int? PracticalQuantity
        {
            set;
            get;
        }
        /// <summary>
        /// 是否样品
        /// </summary>
        public bool IsSample
        {
            set;
            get;
        }
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime? ManufactureDate
        {
            set;
            get;
        }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? EffectiveDate
        {
            set;
            get;
        }
        /// <summary>
        /// 生产批号
        /// </summary>
        public string BatchNumber
        {
            set;
            get;
        }
        /// <summary>
        /// 商品总毛重（kg）
        /// </summary>
        public decimal? RoughWeight
        {
            set;
            get;
        }

        /// <summary>
        /// 商品总净重（kg）
        /// </summary>
        public decimal? NetWeight
        {
            set;
            get;
        }
        
        /// <summary>
        /// 币别Id
        /// </summary>
        public int CurrencyId
        {
            set;
            get;
        }
        /// <summary>
        /// 汇率
        /// </summary>
        public decimal? Rate
        {
            set;
            get;
        }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal? CostPrice
        {
            set;
            get;
        }
        /// <summary>
        /// 总成本价
        /// </summary>
        public decimal? TotalCostPrice
        {
            set;
            get;
        }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal? SalePrice
        {
            set;
            get;
        }
        /// <summary>
        /// 总销售价
        /// </summary>
        public decimal? TotalSalePrice
        {
            set;
            get;
        }
        /// <summary>
        /// 装箱规格
        /// </summary>
        public string CartonSize
        {
            set;
            get;
        }

        /// <summary>
        /// 箱子尺寸
        /// </summary>
        public string CartonMeasure
        {
            set;
            get;
        }

        /// <summary>
        /// 箱数
        /// </summary>
        public int Cases
        {
            set;
            get;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            set;
            get;
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserId
        {
            set;
            get;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime
        {
            set;
            get;
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public int UpdateUserId
        {
            set;
            get;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel
        {
            set;
            get;
        }

        /// <summary>
        /// 原币单价
        /// </summary>
        public decimal? OriginalCurrencyPrice
        {
            set;
            get;
        }

        /// <summary>
        /// 原币总价
        /// </summary>
        public decimal? OriginalCurrencyTotalPrice
        {
            set;
            get;
        }



        /// <summary>
        /// 
        /// </summary>
        public int? ExtendInt1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ExtendInt2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ExtendInt3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendChar4
        {
            set;
            get;
        }
    }
}
