using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class MemberSummaryResult
    {
        public MemberSummaryResult ()
        {
            this.Username = "";
            this.Grade = "";
            this.Points = 0;
            this.Balance = 0;
            this.Expenditure = 0;
            this.OrderCount = 0;
            this.WaitPayOrderCount = 0;
            this.AlreadySentOrderCount = 0;
            this.RefundCount = 0;
            this.ReturnCount = 0;
            this.ReplacementCount = 0;
            this.UnreadCouponCount = 0;
            this.UnreadVoucherCount = 0;
            this.CartItemCount = 0;
            this.Avatar = "";

            this.FinishOrderCount = 0;
            this.WaitSentOrderCount = 0;

            this.ReferralStatus = 0;
        }

        public string Username { get; set; }
        public string Grade { get; set; }
        public int Points { get; set; }
        public decimal Balance { get; set; }
        public decimal Expenditure { get; set; }
        public int OrderCount { get; set; }
        public int WaitPayOrderCount { get; set; }
        public int AlreadySentOrderCount { get; set; }
        public int RefundCount { get; set; }
        public int ReturnCount { get; set; }
        public int ReplacementCount { get; set; }
        public int UnreadCouponCount { get; set; }
        public int UnreadVoucherCount { get; set; }
        public int CartItemCount { get; set; }

        public string Avatar { get; set; }


        public int WaitSentOrderCount { get; set; }
        public int FinishOrderCount { get; set; }

        public int ReferralStatus { get; set; }
    }
}
