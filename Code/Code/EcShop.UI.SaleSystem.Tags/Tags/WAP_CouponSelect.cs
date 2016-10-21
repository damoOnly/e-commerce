using EcShop.SaleSystem.Shopping;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class WAP_CouponSelect : WebControl
    {

        public decimal CartTotal
        {
            get;
            set;
        }

        public string BindId
        {
            get;
            set;
        }

        public string CategoryId
        {
            get;
            set;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable coupon = ShoppingProcessor.GetCoupon(this.CartTotal,this.BindId,this.CategoryId);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" <select  id=\"{0}\">", this.ID);
            stringBuilder.AppendLine("<option name=\" \" value=\"0\" >请选择一张优惠券");
            foreach (DataRow dataRow in coupon.Rows)
            {
                stringBuilder.AppendFormat("<option name=\"{0}\" value=\"{1}\">{2}</option>", new object[]
				{
					
					dataRow["ClaimCode"],
					((decimal)dataRow["discountValue"]).ToString("F2"),
					dataRow["Name"],
				}).AppendLine();
            }
            stringBuilder.Append("</select>");
            writer.Write(stringBuilder.ToString());
        }
    }
}