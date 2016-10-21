using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VTicket : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litActivityDesc;
		private Common_PrizeNames litPrizeNames;
		private System.Web.UI.WebControls.Literal litStartDate;
		private System.Web.UI.WebControls.Literal litEndDate;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vTicket.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int num;
			int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("id"), out num);
			LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
			if (lotteryTicket == null)
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				return;
			}
			if (!(HiContext.Current.User is Member))
			{
				System.Web.HttpContext.Current.Response.Redirect("/Vshop/login.aspx?ReturnUrl=/Vshop/Tocket.aspx?id=" + num);
				return;
			}
			this.litActivityDesc = (System.Web.UI.WebControls.Literal)this.FindControl("litActivityDesc");
			this.litPrizeNames = (Common_PrizeNames)this.FindControl("litPrizeNames");
			this.litStartDate = (System.Web.UI.WebControls.Literal)this.FindControl("litStartDate");
			this.litEndDate = (System.Web.UI.WebControls.Literal)this.FindControl("litEndDate");
			this.litActivityDesc.Text = lotteryTicket.ActivityDesc;
			this.litPrizeNames.Activity = lotteryTicket;
			this.litStartDate.Text = lotteryTicket.OpenTime.ToString("yyyy年MM月dd日 HH:mm:ss");
			this.litEndDate.Text = lotteryTicket.EndTime.ToString("yyyy年MM月dd日 HH:mm:ss");
			PageTitle.AddSiteNameTitle("微抽奖");
		}
	}
}
