using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class PointListItem
    {
        public long JournalNumber { get; set; }
        public string OrderId { get; set; }
        public string TradeDate { get; set; }
        public int TradeType { get; set; }
        public int Increased { get; set; }
        public int Reduced { get; set; }
        public int Points { get; set; }
        public string Remark { get; set; }
    }

    public class PointListResult
    {
        public int Points { get; set; }
        public int AvailablePoints { get; set; }

        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int TotalNumOfRecords { set; get; }

        /// <summary>
        /// 返回的记录集
        /// </summary>
        public List<PointListItem> Results { set; get; }
    }
}
