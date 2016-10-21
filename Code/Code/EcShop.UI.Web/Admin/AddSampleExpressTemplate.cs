using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ExpressTemplates)]
	public class AddSampleExpressTemplate : AdminPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string value = this.Page.Request.QueryString["ExpressName"];
			string text = this.Page.Request.QueryString["XmlFile"];
			if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text) || !text.EndsWith(".xml"))
			{
				base.GotoResourceNotFound();
			}
		}
	}
}
