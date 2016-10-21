using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class RegionListResult
    {
        
    }

    public class RegionListItem
    {
        public RegionListItem()
        {
            this.Children = new List<RegionListItem>();
        }
        public RegionListItem(string regionType, int level)
        {
            this.RegionType = regionType;
            this.Level = level;
            this.Children = new List<RegionListItem>();
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string RegionType { get; set; }
        public int Level { get; set; }
        public List<RegionListItem> Children { get; set; }
    }
}
