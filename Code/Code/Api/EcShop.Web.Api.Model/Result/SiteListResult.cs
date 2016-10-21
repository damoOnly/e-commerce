using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SiteListResult
    {
        public SiteListResult()
        {
            
        }

        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int TotalNumOfRecords { set; get; }

        public int DefaultSiteId { get; set; }
        public string DefaultSiteName { get; set; }

        /// <summary>
        /// 返回的记录集
        /// </summary>
        public List<SiteListItem> Results { set; get; }
    }

    public class SiteListItem
    {
        public SiteListItem() { }
        public SiteListItem(int siteId, string siteName, int regionId, bool isDefault, int displaySequence)
        {
            this.SiteId = siteId;
            this.SiteName = siteName;
            this.RegionId = regionId;
            this.IsDefault = IsDefault;
            this.DisplaySequence = displaySequence;
        }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int RegionId { get; set; }
        public bool IsDefault { get; set; }
        public int DisplaySequence { get; set; }

    }
}
