using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VWinningResults : VMemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputText txtName;
		private System.Web.UI.HtmlControls.HtmlInputText txtPhone;
		private System.Web.UI.WebControls.Literal litPrizeLevel;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vWinningResults.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int activityid;
			int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("activityid"), out activityid);
			PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
			this.litPrizeLevel = (System.Web.UI.WebControls.Literal)this.FindControl("litPrizeLevel");
			if (userPrizeRecord != null)
			{
				this.litPrizeLevel.Text = userPrizeRecord.Prizelevel;
			}
			Member member = HiContext.Current.User as Member;
			if (member != null)
			{
				this.txtName = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtName");
				this.txtPhone = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtPhone");
				this.txtName.Value = member.RealName;
				this.txtPhone.Value = member.CellPhone;
			}
			PageTitle.AddSiteNameTitle("中奖记录");
		}
	}
}
