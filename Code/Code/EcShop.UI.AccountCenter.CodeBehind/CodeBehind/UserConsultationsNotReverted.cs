using ASPNET.WebControls;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserConsultationsNotReverted : MemberTemplatedWebControl
	{
		private ThemedTemplatedList dlstPtConsultationReply;
		private Pager pagerConsultationReply;
		private System.Web.UI.WebControls.Literal litreply;
		private System.Web.UI.WebControls.Literal litNotReverted;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserConsultationsNotReverted.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.dlstPtConsultationReply = (ThemedTemplatedList)this.FindControl("dlstPtConsultationReply");
			this.pagerConsultationReply = (Pager)this.FindControl("pagerConsultationReply");
			this.litreply = (System.Web.UI.WebControls.Literal)this.FindControl("litreply");
			this.litNotReverted = (System.Web.UI.WebControls.Literal)this.FindControl("litNotReverted");
			PageTitle.AddSiteNameTitle("咨询/未回复");
			if (!this.Page.IsPostBack)
			{
				this.BindPtConsultationReply();
			}
		}
		private void BindPtConsultationReply()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.PageIndex = this.pagerConsultationReply.PageIndex;
			productConsultationAndReplyQuery.UserId = HiContext.Current.User.UserId;
			productConsultationAndReplyQuery.Type = ConsultationReplyType.NoReply;
			int num = 0;
			DataSet productConsultationsAndReplys = ProductBrowser.GetProductConsultationsAndReplys(productConsultationAndReplyQuery, out num);
			this.dlstPtConsultationReply.DataSource = productConsultationsAndReplys.Tables[0].DefaultView;
			this.dlstPtConsultationReply.DataBind();
			this.litNotReverted.Text = num.ToString();
			this.litreply.Text = (productConsultationsAndReplys.Tables[1].Select("UserId=" + productConsultationAndReplyQuery.UserId).Length - num).ToString();
			this.pagerConsultationReply.TotalRecords = num;
		}
	}
}
