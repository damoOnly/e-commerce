using EcShop.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Commodities
{
    public class BaseCountryHelper
    {
        /// <summary>
        /// 获取所有国家
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBaseCountry()
        {
            return new BaseCountryDao().GetBaseCountry();
        }

        /// <summary>
        /// 获取国家
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBaseCountryByName(string Name)
        {
            return new BaseCountryDao().GetBaseCountryByName(Name);
        }
        
    }
}
