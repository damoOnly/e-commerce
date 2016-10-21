using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class CouponListItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountValue { get; set; }
        public string StartTime { get; set; }
        public string ClosingTime { get; set; }

        public int CouponStatus { get; set; }
        public bool IsExpired { get; set; }
    }

    public class UseCouponListItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountValue { get; set; }
    }
}
