using EcShop.Entities;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPEditShippingAddress : WAPMemberTemplatedWebControl
	{
		private RegionSelector dropRegions;
		private System.Web.UI.HtmlControls.HtmlInputText shipTo;
		private System.Web.UI.HtmlControls.HtmlTextArea address;
		private System.Web.UI.HtmlControls.HtmlInputText cellphone;
		private System.Web.UI.HtmlControls.HtmlInputHidden Hiddenshipid;
		private System.Web.UI.HtmlControls.HtmlInputHidden regionText;
		private System.Web.UI.HtmlControls.HtmlInputHidden region;
        private System.Web.UI.HtmlControls.HtmlInputText identityCard;
		private int shippingid;
		protected override void OnInit(System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ShippingId"], out this.shippingid))
			{
				this.Page.Response.Redirect("./ShippingAddresses.aspx", true);
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Veditshippingaddress.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.shipTo = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("shipTo");
			this.address = (System.Web.UI.HtmlControls.HtmlTextArea)this.FindControl("address");
			this.cellphone = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("cellphone");
			this.Hiddenshipid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("shipId");
			this.regionText = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("regionText");
			this.region = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("region");
            this.identityCard = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("identityCard");
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(this.shippingid);
			string fullRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, " ");
			this.shipTo.Value = shippingAddress.ShipTo;
			this.address.Value = shippingAddress.Address;
			this.cellphone.Value = shippingAddress.CellPhone;
			this.Hiddenshipid.Value = this.shippingid.ToString();
			this.regionText.SetWhenIsNotNull(fullRegion);
			this.region.SetWhenIsNotNull(shippingAddress.RegionId.ToString());
            this.identityCard.Value = shippingAddress.IdentityCard;
			PageTitle.AddSiteNameTitle("编辑收货地址");
            WAPHeadName.AddHeadName("编辑收货地址");
		}
	}
}
