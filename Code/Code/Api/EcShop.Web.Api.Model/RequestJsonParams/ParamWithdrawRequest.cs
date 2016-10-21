using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamWithdrawRequest : ParamUserBase
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }
}
