using EcShop.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.SaleSystem.Catalog
{
    public static class BrandBrowser
    {
        public static DataTable GetVistiedBrandList(IList<int> brandIds)
        {
            return new BrandTagDao().GetDefaultBrand(brandIds);
        }
        public static DataTable GetVistiedBrandList(int count, int between, int and,int brandTagId)
        {
            return new BrandTagDao().GetVistiedBrandList(count, between, and, brandTagId);
        }
        public static DataTable GetVistiedBrandTagList()
        {
            return new BrandTagDao().GetVistiedBrandTagList();
        }
    }
}
