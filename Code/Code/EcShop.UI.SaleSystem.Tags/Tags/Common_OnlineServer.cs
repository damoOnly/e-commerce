using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_OnlineServer : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_Comment/Skin-Common_OnlineServer.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			Literal literal = (Literal)this.FindControl("litOnlineServer");
			literal.Text = HiContext.Current.SiteSettings.HtmlOnlineServiceCode;
		}
	}
}
