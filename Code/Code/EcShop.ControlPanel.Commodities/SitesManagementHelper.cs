using Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Commodities
{
    public sealed class SitesManagementHelper
    {
        private SitesManagementHelper()
        {
        }
        public static DataTable GetSites()
        {
           DataTable dtSites= HiCache.Get("DataCache-Sites") as DataTable;
           if (dtSites == null)
           {
               dtSites = new SitesManagementDao().GetSites();
               HiCache.Insert("DataCache-Sites", dtSites);
           }
           return dtSites;
        }

        public static SitesManagementInfo GetSites(int SitesId)
        {
            return new SitesManagementDao().GetSites(SitesId);
        }
        public static int AddSites(SitesManagementInfo SitesInfo)
        {
            int ret= new SitesManagementDao().AddSites(SitesInfo);
            if (ret > 0)
            {
                HiCache.Remove("DataCache-Sites");
            }
            return ret;
        }
        public static bool UpdateStore(SitesManagementInfo SitesInfo)
        {
            bool b= new SitesManagementDao().UpdateSites(SitesInfo);
            if (b)
            {
                HiCache.Remove("DataCache-Sites");
            }
            return b;
        }
        public static bool DeleteSites(int SitesId)
        {
            return new SitesManagementDao().DeleteSites(SitesId);
        }
        public static DbQueryResult GetSite(SiteQuery query)
        {
            return new SitesManagementDao().GetSite(query);
        }

        public static DataTable GetMySubMemberByUserId(int UserId)
        {
            return new MemberDao().GetMySubMemberByUserId(UserId);
        }
    }
}
