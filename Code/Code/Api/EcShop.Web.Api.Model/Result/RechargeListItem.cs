using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class RechargeListItem
    {
        public long JournalNumber { get; set; }
        public string TradeDate { get; set; }
        public int TradeType { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
        public string RechargeWay { get; set; }
        public string InpourId { get; set; }
    }

    public class RechargeListResult
    {
        public decimal Balance { get; set; }
        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int TotalNumOfRecords { set; get; }

        /// <summary>
        /// 返回的记录集
        /// </summary>
        public List<RechargeListItem> Results { set; get; }
    }
}
