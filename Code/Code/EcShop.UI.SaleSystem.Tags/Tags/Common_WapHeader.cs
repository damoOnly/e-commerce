using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_WapHeader : WAPTemplatedWebControl
	{
        public System.Web.UI.WebControls.Literal litSitesList;
        public WapTemplatedRepeater rpthistorysearch;
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
            this.litSitesList = (System.Web.UI.WebControls.Literal)this.FindControl("litSitesList");
            this.rpthistorysearch = (WapTemplatedRepeater)this.FindControl("rpthistorysearch");

            this.litSitesList.Text = RegisterSitesScript();

            int userId=HiContext.Current.User.UserId;
            if(userId>0)
            {
                this.rpthistorysearch.DataSource = HistorySearchHelp.GetSearchHistory(userId, ClientType.WAP, 6);
                this.rpthistorysearch.DataBind();
            }

		}
	}
}
