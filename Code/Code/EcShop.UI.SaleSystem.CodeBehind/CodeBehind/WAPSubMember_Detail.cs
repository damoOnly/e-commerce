using EcShop.Entities.Members;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPSubMember_Detail : WAPMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUsername;
		private System.Web.UI.WebControls.Literal litTrueName;
		private System.Web.UI.WebControls.Literal litTelphone;
		private System.Web.UI.WebControls.Literal litOrderCount;
		private System.Web.UI.WebControls.Literal litCreateTime;
		private System.Web.UI.WebControls.Literal litLastOrderTime;
		private FormatedMoneyLabel litAmount;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubMember_Detail.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litAmount = (FormatedMoneyLabel)this.FindControl("litAmount");
			this.litUsername = (System.Web.UI.WebControls.Literal)this.FindControl("litUsername");
			this.litTrueName = (System.Web.UI.WebControls.Literal)this.FindControl("litTrueName");
			this.litOrderCount = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderCount");
			this.litTelphone = (System.Web.UI.WebControls.Literal)this.FindControl("litTelphone");
			this.litCreateTime = (System.Web.UI.WebControls.Literal)this.FindControl("litCreateTime");
			this.litLastOrderTime = (System.Web.UI.WebControls.Literal)this.FindControl("litLastOrderTime");
			PageTitle.AddSiteNameTitle("下级会员详情");
			int userId = 0;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserID"]))
			{
				int.TryParse(this.Page.Request.QueryString["UserID"], out userId);
			}
			SubMember mySubUser = MemberProcessor.GetMySubUser(userId);
			if (mySubUser == null)
			{
				this.ShowMessage("错误的会员ID", false);
			}
			if (this.litAmount != null)
			{
				this.litAmount.Money = mySubUser.SubMemberSplittin;
			}
			this.litUsername.Text = mySubUser.UserName;
			this.litTrueName.Text = mySubUser.RealName;
			this.litOrderCount.Text = mySubUser.OrderNumber.ToString();
			this.litTelphone.Text = mySubUser.CellPhone;
			this.litCreateTime.Text = mySubUser.CreateDate.ToString("yyyy-MM-dd hh:mm:ss");
		}
	}
}
