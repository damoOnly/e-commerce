using EcShop.Core;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHShippingAddresses : AliOHMemberTemplatedWebControl
	{
		private AliOHTemplatedRepeater rptvShipping;
		private System.Web.UI.HtmlControls.HtmlAnchor aLinkToAdd;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Vshippingaddresses.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptvShipping = (AliOHTemplatedRepeater)this.FindControl("rptvShipping");
			this.aLinkToAdd = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("aLinkToAdd");
			this.aLinkToAdd.HRef = Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx";
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["returnUrl"]))
			{
				System.Web.UI.HtmlControls.HtmlAnchor expr_6D = this.aLinkToAdd;
				expr_6D.HRef = expr_6D.HRef + "?returnUrl=" + Globals.UrlEncode(this.Page.Request.QueryString["returnUrl"]);
			}
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
			if (shippingAddresses != null)
			{
				this.rptvShipping.DataSource = shippingAddresses;
				this.rptvShipping.DataBind();
			}
			PageTitle.AddSiteNameTitle("收货地址");
		}
	}
}
