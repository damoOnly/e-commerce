using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VActivity : VMemberTemplatedWebControl
	{
		private HiImage img;
		private System.Web.UI.WebControls.Literal litDescription;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vActivity.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int num;
			int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("id"), out num);
			if (!(HiContext.Current.User is Member))
			{
				System.Web.HttpContext.Current.Response.Redirect("/Vshop/login.aspx?ReturnUrl=/Vshop/Activity.aspx?id=" + num);
				return;
			}
			ActivityInfo activity = VshopBrowser.GetActivity(num);
			if (activity == null)
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				return;
			}
			if (activity.MaxValue <= VshopBrowser.GetUserPrizeCount(num))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"报名人数已达到限制人数！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				return;
			}
			this.img = (HiImage)this.FindControl("img");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.img.ImageUrl = activity.PicUrl;
			this.litDescription.Text = activity.Description;
			PageTitle.AddSiteNameTitle("微报名");
		}
	}
}
