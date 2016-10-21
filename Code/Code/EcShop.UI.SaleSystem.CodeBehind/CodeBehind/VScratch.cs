using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VScratch : VActivityidTemplatedWebControl
	{
		private int activityid;
		private System.Web.UI.HtmlControls.HtmlImage bgimg;
		private System.Web.UI.WebControls.Literal litActivityDesc;
		private Common_PrizeNames litPrizeNames;
		private Common_PrizeUsers litPrizeUsers;
		private System.Web.UI.WebControls.Literal litStartDate;
		private System.Web.UI.WebControls.Literal litEndDate;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VScratch.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityid"], out this.activityid))
			{
				base.GotoResourceNotFound("");
			}
			if (!(HiContext.Current.User is Member))
			{
				System.Web.HttpContext.Current.Response.Redirect("/Vshop/login.aspx?ReturnUrl=/Vshop/Scratch.aspx?activityid=" + this.activityid);
				return;
			}
			this.bgimg = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("bgimg");
			this.litActivityDesc = (System.Web.UI.WebControls.Literal)this.FindControl("litActivityDesc");
			this.litPrizeNames = (Common_PrizeNames)this.FindControl("litPrizeNames");
			this.litPrizeUsers = (Common_PrizeUsers)this.FindControl("litPrizeUsers");
			this.litStartDate = (System.Web.UI.WebControls.Literal)this.FindControl("litStartDate");
			this.litEndDate = (System.Web.UI.WebControls.Literal)this.FindControl("litEndDate");
			PageTitle.AddSiteNameTitle("刮刮卡");
			LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(this.activityid);
			if (lotteryActivity == null)
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				return;
			}
			this.litStartDate.Text = lotteryActivity.StartTime.ToString("yyyy年MM月dd日 HH:mm:ss");
			this.litEndDate.Text = lotteryActivity.EndTime.ToString("yyyy年MM月dd日 HH:mm:ss");
			if (VshopBrowser.GetUserPrizeCount(this.activityid) >= lotteryActivity.MaxNum)
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"亲，不好意思您的抽奖机会已经用完了哦，敬请期待下次活动吧！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
			}
			if (lotteryActivity.StartTime < System.DateTime.Now && System.DateTime.Now < lotteryActivity.EndTime)
			{
				this.litActivityDesc.Text = lotteryActivity.ActivityDesc;
				this.litPrizeNames.Activity = lotteryActivity;
				this.litPrizeUsers.Activity = lotteryActivity;
				int userPrizeCount = VshopBrowser.GetUserPrizeCount(this.activityid);
				System.Web.UI.WebControls.Literal expr_1FA = this.litActivityDesc;
				expr_1FA.Text += string.Format("您一共有{0}次参与机会，目前还剩{1}次。", lotteryActivity.MaxNum, lotteryActivity.MaxNum - userPrizeCount);
				return;
			}
			this.Page.ClientScript.RegisterStartupScript(base.GetType(), "scriptErro", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
		}
	}
}
