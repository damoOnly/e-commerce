using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class UserProductReferLabel : Literal
	{
		private string _txtClass = "";
		private string _priceClass = "";
		private string _style = "";
		public ProductInfo product
		{
			get;
			set;
		}
		public string txtClass
		{
			get
			{
				return this._txtClass;
			}
			set
			{
				this._txtClass = value;
			}
		}
		public string PriceClass
		{
			get
			{
				return this._priceClass;
			}
			set
			{
				this._priceClass = value;
			}
		}
		public string Style
		{
			get
			{
				return this._style;
			}
			set
			{
				this._style = value;
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			Member member = HiContext.Current.User as Member;
			if (member == null || member.ReferralStatus != 2 || this.product == null)
			{
				base.Text = "";
				return;
			}
			decimal minSalePrice = this.product.MinSalePrice;
			decimal d = this.product.ReferralDeduct.HasValue ? this.product.ReferralDeduct.Value : 0m;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (d <= 0m)
			{
				d = masterSettings.ReferralDeduct;
			}
			if (d > 0m)
			{
				base.Text = string.Concat(new string[]
				{
					"<div ",
					string.IsNullOrEmpty(this.txtClass) ? "" : ("class=\"" + this.txtClass + "\""),
					(this.Style == "") ? "" : (" style=\"" + this.Style + "\""),
					">推广分佣：<span ",
					string.IsNullOrEmpty(this.PriceClass) ? "" : ("class=\"" + this.PriceClass + "\""),
					">￥",
					(minSalePrice * (d / 100m)).ToString("f2"),
					"</span></div>"
				});
			}
			else
			{
				base.Text = "";
			}
			base.Render(writer);
		}
	}
}
