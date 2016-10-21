using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal;
using EcShop.SqlDal.Active;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Members;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
namespace EcShop.ControlPanel.Sales
{
    public static class ActiveHelper
    {
        public static DataTable GetThisTopicList()
        {
            return new PCActiveDao().GetThisTopicList();
        }
        public static DbQueryResult GetThisProductList(ProductBrowseQuery query)
        {
            return new PCActiveDao().GetCurrBrowseActiveProductList(query);
        }

        public static DbQueryResult GetCurrActiveOneProductList(ProductBrowseQuery query)
        {
            return new PCActiveDao().GetCurrActiveOneProductList(query);
        }
        public static DataTable GetThisTopicList_Two()
        {
            return new PCActiveDao().GetThisTopicList_Two();
        }

        public static DbQueryResult GetCurrBrowseActiveProductListByTopicId(ProductBrowseQuery query)
        {
            return new PCActiveDao().GetCurrBrowseActiveProductListByTopicId(query);
        }

     
    }
}