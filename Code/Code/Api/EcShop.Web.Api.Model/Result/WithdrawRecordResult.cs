using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
   
    public class WithdrawRecordResult
    {
        public WithdrawRecordResult()
        {
            this.WithdrawRecordList = new List<WithdrawRecordItem>();
        }
        public decimal TotalWithdraw { get; set; }

        public decimal AvailableWithdraw { get; set; }
        public int TotalNumOfRecords { get; set; }
        public List<WithdrawRecordItem> WithdrawRecordList { get; set; }
    }


    public class WithdrawRecordItem
    {
        public decimal Amount { get; set; }
        public string AuditStatus { get; set; }

        public string RequestDate { get; set; }
    }
}
