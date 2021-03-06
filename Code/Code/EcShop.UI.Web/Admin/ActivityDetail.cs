using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManageActivity)]
	public class ActivityDetail : AdminPage
	{
		protected ActivityInfo _act;
		protected System.Web.UI.WebControls.Repeater rpt;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				int urlIntParam = base.GetUrlIntParam("id");
				ActivityInfo activity = VShopHelper.GetActivity(urlIntParam);
				if (activity == null)
				{
					return;
				}
				this._act = activity;
				this.rpt.DataSource = VShopHelper.GetActivitySignUpById(urlIntParam);
				this.rpt.DataBind();
			}
		}
	}
}
