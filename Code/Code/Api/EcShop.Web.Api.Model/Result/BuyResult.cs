using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class BuyResult
    {
        public BuyResult()
        {

        }

        public BuyResult(bool isSuccess, decimal amount, int quantity)
        {
            this.IsSuccess = isSuccess;
            this.Amount = amount;
            this.Quantity = quantity;
        }
        public bool IsSuccess { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
    }
}
