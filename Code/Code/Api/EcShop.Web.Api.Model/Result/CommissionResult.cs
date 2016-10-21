using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class CommissionResult
    {
        public CommissionResult()
        {
            this.CommissionList = new List<CommissionItem>();
        }
        public decimal HistoryCommission { get; set; }

        public decimal AvailableCommission { get; set; }
        public int TotalNumOfRecords { get; set; }
        public List<CommissionItem> CommissionList { get; set; }
    }


    public class CommissionItem
    {
        public long JournalNumber { get; set; } 
        
        public string SubUserName { get; set; }

        public string Balance { get; set; }

        public string OrderId { get; set; }

        public string TradeDate { get; set; }

        public bool IsUse { get; set; }

        public int TradeType { get; set; }

        public decimal OrderTotal { get; set; }

        public string Remark { get; set; }
    }
}
