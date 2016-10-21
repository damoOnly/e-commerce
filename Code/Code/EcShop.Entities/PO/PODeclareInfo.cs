using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.PO
{
    public class PODeclareInfo
    {
        public int id
        {
            get;
            set;
        }
        /// <summary>
        /// PO编号
        /// </summary>
        public string PONumber
        {
            get;
            set;
        }
        /// <summary>
        /// 运输方式
        /// </summary>
        public string TransportType
        {
            set;
            get;
        }
        /// <summary>
        /// 运输方式编码
        /// </summary>
        public int TransportTypeCode
        {
            set;
            get;
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType
        {
            set;
            get;
        }
        /// <summary>
        /// 业务类型编码
        /// </summary>
        public string BusinessTypeCode
        {
            set;
            get;
        }
        /// <summary>
        /// 包装种类
        /// </summary>
        public string WrapType
        {
            set;
            get;
        }
        /// <summary>
        /// 包装种类
        /// </summary>
        public int WrapTypeCode
        {
            set;
            get;
        }
        /// <summary>
        /// 成交方式
        /// </summary>
        public string TradeType
        {
            set;
            get;
        }
        /// <summary>
        /// 成交方式编码
        /// </summary>
        public int TradeTypeCode
        {
            set;
            get;
        }
        ///// <summary>
        ///// 申报单类型
        ///// </summary>
        //public string declarationType
        //{
        //    set;
        //    get;
        //}
        ///// <summary>
        ///// 净重
        ///// </summary>
        //public double Qty
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// 毛重
        ///// </summary>
        //public double Gwt
        //{
        //    get;
        //    set;
        //}
        //运输工具名称
        public string transname
        {
            get;
            set;
        }
        /// <summary>
        /// 提运单号
        /// </summary>
        public string cabinno
        {
            get;
            set;
        }
        ///// <summary>
        ///// 申报数量
        ///// </summary>
        //public int applyNum
        //{
        //    set;
        //    get;
        //}
        /// <summary>
        /// 法定数量
        /// </summary>
        public string FriNum
        {
            set;
            get;
        }
        /// <summary>
        /// 征免方式
        /// </summary>
        public string getTax
        {
            set;
            get;
        }
        /// <summary>
        /// 征免方式编码
        /// </summary>
        public int getTaxCode
        {
            set;
            get;
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
        public int useTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 进口口岸
        /// </summary>
        public string applyort
        {
            get;
            set;
        }
        /// <summary>
        /// 进口口岸编码
        /// </summary>
        public int applyortCode
        {
            get;
            set;
        }
        /// <summary>
        /// 集装箱规格号
        /// </summary>
        public string ContainerNumberType
        {
            get;
            set;
        }
        /// <summary>
        /// 航班号
        /// </summary>
        public string voyage
        { 
            get; 
            set; 
        }
    }
}
