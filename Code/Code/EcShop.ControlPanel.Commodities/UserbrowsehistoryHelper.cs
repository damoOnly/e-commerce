using EcShop;
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
    public sealed class UserbrowsehistoryHelper
    {
        private UserbrowsehistoryHelper()
        {
        }

        public static DataTable GetUserbrowsehistorys()
        {
           DataTable dtBrowsehistorys = HiCache.Get("DataCache-Userbrowsehistorys") as DataTable;
           if (dtBrowsehistorys == null)
           {
               dtBrowsehistorys = new UserbrowsehistoryDao().GetUserBrowseHistorys();
               HiCache.Insert("DataCache-Userbrowsehistorys", dtBrowsehistorys);
           }
            return dtBrowsehistorys;
        }

        public static UserbrowsehistoryInfo GetUserBrowseHistory(int historyId)
        {
            return new UserbrowsehistoryDao().GetUserBrowseHistory(historyId);
        }

        public static bool DeleteUserBrowseHistory(int historyId)
        {
            return new UserbrowsehistoryDao().DeleteUserBrowseHistory(historyId);
        }
        public static DbQueryResult GetBrowseHistory(UserbrowsehistoryQuery query)
        {
            return new UserbrowsehistoryDao().GetBrowseHistory(query);
        }

        public static DataTable GetUserBrowseHistory(UserbrowsehistoryInfo browserhistory)
        {
            DataTable dtScanHistorys = HiCache.Get("DataCache-UserScanHistroys") as DataTable;
            if (dtScanHistorys == null)
            {
                dtScanHistorys = new UserbrowsehistoryDao().GetUserBrowseHistory(browserhistory);
                HiCache.Insert("DataCache-UserScanHistroys", dtScanHistorys);
            }
            return dtScanHistorys;
        }

        public static void SetUserBrowseHistory(UserbrowsehistoryInfo browserhistory)
        {
            DataTable dtScanHistorys = HiCache.Get("DataCache-UserScanHistroys") as DataTable;
            if (dtScanHistorys == null)
            {
                dtScanHistorys = new DataTable();

                dtScanHistorys.Columns.Add("ProductId", typeof(System.Int32));
                dtScanHistorys.Columns.Add("UserId", typeof(System.Int32));
                dtScanHistorys.Columns.Add("UserName", typeof(System.String));
                dtScanHistorys.Columns.Add("BrowseTime", typeof(System.DateTime));
                dtScanHistorys.Columns.Add("UserIP", typeof(System.String));
                dtScanHistorys.Columns.Add("Description", typeof(System.String));
                dtScanHistorys.Columns.Add("Sort", typeof(System.Int32));
                dtScanHistorys.Columns.Add("Url", typeof(System.String));
                dtScanHistorys.Columns.Add("BrowerTimes", typeof(System.Int32));
                dtScanHistorys.Columns.Add("PlatType", typeof(System.Int32));
                dtScanHistorys.Columns.Add("IP", typeof(System.Int64));
                dtScanHistorys.Columns.Add("CategoryId", typeof(System.Int32));
            }

            if (dtScanHistorys.Rows.Count >= 2)
            {
                new UserbrowsehistoryDao().BulkUserBrowserHistory(dtScanHistorys);

                dtScanHistorys.Rows.Clear();
            }

            DataRow row = dtScanHistorys.NewRow();
            row["ProductId"] = browserhistory.ProductId;
            row["UserId"] = browserhistory.UserId;
            row["UserName"] = browserhistory.UserName ?? "";
            row["BrowseTime"] = browserhistory.BrowseTime ?? DateTime.Now;
            row["UserIP"] = browserhistory.UserIP ?? "";
            row["Description"] = browserhistory.Description ?? "";
            row["Sort"] = browserhistory.Sort;
            row["Url"] = browserhistory.Url ?? "";
            row["BrowerTimes"] = 1;
            row["PlatType"] = browserhistory.PlatType;
            row["IP"] = browserhistory.Ip;
            row["CategoryId"] = browserhistory.CategoryId;

            dtScanHistorys.Rows.Add(row);

            new UserbrowsehistoryDao().BulkUserBrowserHistory(dtScanHistorys);

            //HiCache.Insert("DataCache-UserScanHistroys", dtScanHistorys);
        }
    }
}
