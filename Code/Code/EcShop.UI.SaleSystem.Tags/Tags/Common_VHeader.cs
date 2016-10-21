using EcShop.ControlPanel.Store;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_VHeader : VshopTemplatedWebControl
	{
        private System.Web.UI.WebControls.Literal litJSApi;
        public System.Web.UI.WebControls.Literal litSitesList;
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/skin-Common_Header.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
            this.litJSApi = (System.Web.UI.WebControls.Literal)this.FindControl("litJSApi");
            this.litSitesList = (System.Web.UI.WebControls.Literal)this.FindControl("litSitesList");
            this.litSitesList.Text = RegisterSitesScript();
            litJSApi.Text = GetJSApiScript();
		}


	}
}
