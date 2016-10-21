using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ExtendOrderResult
    {
        public ExtendOrderResult()
        {
            this.ProductNames = new List<string>();
        }
        public string OrderId { get; set; }
        public int WaitBuyerPayOrderCount { get; set; }
        public bool IsNeedPayment { get; set; }
        public decimal ProductAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Freight { get; set; }
        public decimal Tax { get; set; }

        public int PaymentTypeId { get; set; }

        public List<string> ProductNames { get; set; }


    }
}
