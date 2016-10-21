using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.PO
{
    public class PODeclareJSON
    {
        /// <summary>
        /// 申报海关
        /// </summary>
        public string applyCustoms
        {
            get;
            set;
        }
        /// <summary>
        /// 申报海关编码
        /// </summary>
        public string applyCustomsCode
        {
            get;
            set;
        }
        /// <summary>
        /// 申报单位编码
        /// </summary>
        public string applyBusinessCode
        {
            get;
            set;
        }
        /// <summary>
        /// 申报单位名称
        /// </summary>
        public string applyBusinessName
        {
            get;
            set;
        }
        /// <summary>
        /// 进出口岸
        /// </summary>
        public string applyort
        {
            get;
            set;
        }
        /// <summary>
        /// 进出口岸编码
        /// </summary>
        public string applyortCode
        {
            get;
            set;
        }
        /// <summary>
        /// 运输方式
        /// </summary>
        public string transportType
        {
            get;
            set;
        }
        /// <summary>
        /// 运输方式编码
        /// </summary>
        public string transportTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 经营单位编码
        /// </summary>
        public string companyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 经营单位名称
        /// </summary>
        public string companyName
        {
            get;
            set;
        }
        /// <summary>
        /// 发货单位名称
        /// </summary>
        public string getGoodsBusinName
        {
            get;
            set;
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string businessType
        {
            get;
            set;
        }
        /// <summary>
        /// 总件数
        /// </summary>
        public string allQlt
        {
            get;
            set;
        }
        /// <summary>
        /// 包装种类
        /// </summary>
        public string wrapType
        {
            get;
            set;
        }
        /// <summary>
        /// 包装种类编码
        /// </summary>
        public string wrapTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 监管方式
        /// </summary>
        public string superviseType
        {
            get;
            set;
        }
        /// <summary>
        /// 监管方式编码
        /// </summary>
        public string superviseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 成交方式
        /// </summary>
        public string tradeType
        {
            get;
            set;
        }
        /// <summary>
        /// 成交方式代码
        /// </summary>
        public string tradeTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 净重
        /// </summary>
        public string Qty
        {
            get;
            set;
        }
        /// <summary>
        /// 毛重
        /// </summary>
        public string allQty
        {
            get;
            set;
        }
        /// <summary>
        /// 启运国
        /// </summary>
        public string sendCountry
        {
            get;
            set;
        }
        /// <summary>
        /// 启运国编码
        /// </summary>
        public string sendCountryName
        {
            get;
            set;
        }
        /// <summary>
        /// 备案企业名称
        /// </summary>
        public string RecordCompanyName
        {
            get;
            set;
        }
        /// <summary>
        /// 报关单类型
        /// </summary>
        public string declarationType
        {
            get;
            set;
        }
        /// <summary>
        /// 运输工具名称
        /// </summary>
        public string transname
        {
            get;
            set;
        }
        /// <summary>
        /// 装货港
        /// </summary>
        public string loadport_no
        {
            get;
            set;
        }
        /// <summary>
        /// 境内目的地
        /// </summary>
        public string end_country_no
        {
            get;
            set;
        }
        /// <summary>
        /// 提运单号
        /// </summary>
        public string cabin_no
        {
            get;
            set;
        }
        /// <summary>
        /// 申报信息表体
        /// </summary>
        public List<setInfo> setInfo
        {
            get;
            set;
        }
    }

    public class setInfo
    {
        /// <summary>
        /// Sku编码
        /// </summary>
        public string SkuId
        {
            get;
            set;
        }
        /// <summary>
        /// 归并方式
        /// </summary>
        public string mergeType
        {
            get;
            set;
        }
        /// <summary>
        /// 商品料件号
        /// </summary>
        public string proLJNO
        {
            get;
            set;
        }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string HS_CODE
        {
            get;
            set;
        }
        /// <summary>
        /// 商品备案编号
        /// </summary>
        public string ProductRegistrationNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 商品中文名称
        /// </summary>
        public string HSProductName
        {
            get;
            set;
        }
        /// <summary>
        /// 申报数量
        /// </summary>
        public string applyNum
        {
            get;
            set;
        }
        /// <summary>
        /// 总价
        /// </summary>
        public string AllPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 法定数量
        /// </summary>
        public string FriNum
        {
            get;
            set;
        }
        /// <summary>
        /// 原产国
        /// </summary>
        public string Country
        {
            get;
            set;
        }
        /// <summary>
        /// 原产国编码
        /// </summary>
        public string CountryCode
        {
            get;
            set;
        }
        /// <summary>
        /// 征免方式
        /// </summary>
        public string getTax
        {
            get;
            set;
        }
        /// <summary>
        /// 征免方式编码
        /// </summary>
        public string getTaxCode
        {
            get;
            set;
        }
        /// <summary>
        /// 用途
        /// </summary>
        public string useType
        {
            get;
            set;
        }
        /// <summary>
        /// 用途编码
        /// </summary>
        public string useTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 归并原则
        /// </summary>
        public string mergeRule
        {
            get;
            set;
        }
        /// <summary>
        /// 单价（成本价）,备案价
        /// </summary>
        public string Price
        {
            get;
            set;
        }
    }

    public class POJsonInfo
    {
        /// <summary>
        /// PO编号
        /// </summary>
        public string POId
        {
            get;
            set;
        }
        /// <summary>
        /// 合同号
        /// </summary>
        public string contractNo
        {
            get;
            set;
        }
        public header header
        {
            get;
            set;
        }
        public List<goods> goods
        {

            get;
            set;
        }

    }
    public class header
    {
                /// <summary>
        /// 入库单号
        /// </summary>
        public string formId
        {
            get;
            set;
        }
    }
    public class goods
    {
        public string num_no
        {
            get;
            set;
        }
        public string amount
        {
            get;
            set;
        }
        public string amount1
        {
            get;
            set;
        }
        public string country_name
        {
            get;
            set;
        }
        public string currency_name
        {
            get;
            set;
        }
        public string goods_name
        {
            get;
            set;
        }
        public string goods_no
        {
            get;
            set;
        }

        public string goods_spec
        {
            get;
            set;
        }
        public string totalamount
        {
            get;
            set;
        }
        public string unit_name
        {
            get;
            set;
        }
        public string unit_name1
        {
            get;
            set;
        }

        public List<lj> lj
        {
            get;
            set;
        }

    }

    public class lj
    {
        public string ljno
        {
            get;
            set;
        }
        public string SkuId
        {
            get;
            set;
        }
    }

    public class htb
    {
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string orgCode
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string agentName
        {
            get;
            set;
        }
        /// <summary>
        /// 主管海关
        /// </summary>
        public string customsCode
        {
            get;
            set;
        }
        /// <summary>
        /// 主管检验检疫
        /// </summary>
        public string ciqCode
        {
            get;
            set;
        }
        /// <summary>
        /// 监管场所
        /// </summary>
        public string supervisionCode
        {
            get;
            set;
        }
        /// <summary>
        /// 业务模式
        /// </summary>
        public string businessMode
        {
            get;
            set;
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string businessType
        {
            get;
            set;
        }
        /// <summary>
        /// 电商账册编号
        /// </summary>
        public string emsNo
        {
            get;
            set;
        }
        /// <summary>
        /// 经营单位名称
        /// </summary>
        public string storageTradeName
        {
            get;
            set;
        }
        /// <summary>
        /// 收（发）货人名称
        /// </summary>
        public string storageEbpName
        {
            get;
            set;
        }
        /// <summary>
        /// 承运企业名称
        /// </summary>
        public string logisticsName
        {
            get;
            set;
        }
        /// <summary>
        ///进口日期
        /// </summary>
        public string strIeDate
        {
            get;
            set;
        }
        /// <summary>
        /// 入库日期
        /// </summary>
        public string storageEnd
        {
            get;
            set;
        }
        /// <summary>
        /// 境区类型
        /// </summary>
        public string psType
        {
            get;
            set;
        }
        /// <summary>
        /// 进出口岸
        /// </summary>
        public string storagePortCode
        {
            get;
            set;
        }
        /// <summary>
        /// 仓库使用企业
        /// </summary>
        public string storageCusName
        {
            get;
            set;
        }
        /// <summary>
        /// 运输方式
        /// </summary>
        public string storageTrafMode
        {
            get;
            set;
        }
        /// <summary>
        /// 运输工具名称
        /// </summary>
        public string shipName
        {
            get;
            set;
        }
        /// <summary>
        /// 航次
        /// </summary>
        public string voyageNo
        {
            get;
            set;
        }
        /// <summary>
        /// 提运单号
        /// </summary>
        public string billNo
        {
            get;
            set;
        }
        /// <summary>
        /// 指运港（抵运港
        /// </summary>
        public string storageDestinationPort
        {
            get;
            set;
        }
        /// <summary>
        /// 包装种类
        /// </summary>
        public string storageWrapType
        {
            get;
            set;
        }
        /// <summary>
        /// 总件数
        /// </summary>
        public string packNo
        {
            get;
            set;
        }
        /// <summary>
        /// 毛重
        /// </summary>
        public string weight
        {
            get;
            set;
        }
        /// <summary>
        /// 净重
        /// </summary>
        public string netWt
        {
            get;
            set;
        }
        /// <summary>
        /// 监管方式
        /// </summary>
        public string storageTradeMode
        {
            get;
            set;
        }
        /// <summary>
        /// 成交方式
        /// </summary>
        public string storageTransMode
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string note
        {
            get;
            set;
        }
        /// <summary>
        /// 商品信息归并原则
        /// </summary>
        public string select6
        {
            get;
            set;
        }
        public List<goods1> goods
        {
            get;
            set;
        }
    }

    public class goods1
    {
        /// <summary>
        /// 商品货号
        /// </summary>
        public string itemNo
        {
            get;
            set;
        }
        /// <summary>
        /// 申报单价
        /// </summary>
        public string price
        {
            get;
            set;
        }
        /// <summary>
        /// 法定数量
        /// </summary>
        public string qty1
        {
            get;
            set;
        }
        /// <summary>
        /// 生产日期
        /// </summary>
        public string ManufactureDate
        {
            get;
            set;
        }
        /// <summary>
        /// 有效日期
        /// </summary>
        public string EffectiveDate
        {
            get;
            set;
        }
        /// <summary>
        /// 法定单位
        /// </summary>
        public string unit1
        {
            get;
            set;
        }
        ///// <summary>
        ///// 法定名称
        ///// </summary>
        //public string unit1Name
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// 法定编码
        ///// </summary>
        //public string unit1Code
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 批次
        /// </summary>
        public string BatchNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 申报数量
        /// </summary>
        public string DeclareNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 归并序号
        /// </summary>
        public string mergerNo
        {
            get;
            set;
        }
    }
}
