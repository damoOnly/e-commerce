using EcShop.SqlDal.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Sales
{
    public static class BaseEnumDictHelper
    {
        public static Dictionary<string, string> GetBaseEnumDictItems(string dictType)
        {
            return new BaseEnumDictDao().GetBaseEnumDictItems(dictType);
        }
    }
}
