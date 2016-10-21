using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.Entities.HS
{
    public class HSCodeInfo
    {

        /// <summary>
        /// 海关编码编号
        /// </summary>
        public int HS_CODE_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 海关编码
        /// </summary>
        public string HS_CODE
        {
            get;
            set;
        }
        /// <summary>
        /// 海关名称
        /// </summary>
        public string HS_NAME
        {
            get;
            set;
        }
        /// <summary>
        /// 海关描述
        /// </summary>
        public string HS_DESC
        {
            get;
            set;
        }
        /// <summary>
        /// 最惠国税率
        /// </summary>
        public decimal LOW_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 普通国税率
        /// </summary>
        public decimal HIGH_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 出口税率
        /// </summary>
        public decimal OUT_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 增值税率
        /// </summary>
        public decimal TAX_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 退税率
        /// </summary>
        public decimal TSL_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 消费税
        /// </summary>
        public decimal CONSUMPTION_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 临时进口税率
        /// </summary>
        public decimal TEMP_IN_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 临时出口税率
        /// </summary>
        public decimal TEMP_OUT_RATE
        {
            get;
            set;
        }
        /// <summary>
        /// 产品类型
        /// </summary>
        public string PRODUCTTYPE
        {
            get;
            set;
        }
        /// <summary>
        /// 申报要素
        /// </summary>
        public string ELEMENTS
        {
            get;
            set;
        }
        /// <summary>
        /// 海关监管条件
        /// </summary>
        public string CONTROL_MA
        {
            get;
            set;
        }
        /// <summary>
        /// 单位1
        /// </summary>
        public string UNIT_1
        {
            get;
            set;
        }
        /// <summary>
        /// 单位2
        /// </summary>
        public string UNIT_2
        {
            get;
            set;
        }
        /// <summary>
        /// 海关说明
        /// </summary>
        public string NOTE_S
        {
            get;
            set;
        }
        /// <summary>
        /// 海关商检条件
        /// </summary>
        public string CONTROL_INSPECTION
        {
            get;
            set;
        }
        /// <summary>
        /// 申报要素表
        /// </summary>
        public DataSet FND_HS_ELMENTS
        {
            get;
            set;
        }
    }
}
