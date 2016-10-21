using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SqlDal.Comments;
using EcShop.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
namespace EcShop.ControlPanel.Comments
{
    public static class TaxRateHelper
	{
        public static IList<TaxRateInfo> GetMainTaxRate()
		{
            return new TaxRateDao().GetMainTaxRate();
		}

        public static IList<TaxRateInfo> GetMainTaxRate(int categoryId)
        {
            return new TaxRateDao().GetMainTaxRate(categoryId);
        }	

        /// <summary>
        /// 根据行邮编码获取数据
        /// </summary>
        /// <param name="Code">行邮编码</param>
        /// <returns></returns>
        public static DataTable GetTaxRateBuyCode(string Code)
        {
            return new TaxRateDao().GetTaxRateBuyCode(Code);
        }
	}
}
