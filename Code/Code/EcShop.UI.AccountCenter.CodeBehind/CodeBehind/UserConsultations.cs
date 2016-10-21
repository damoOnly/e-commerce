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
	public class UserConsultations : MemberTemplatedWebControl
	{
		private ThemedTemplatedList dlstPtConsultationReplyed;
		private Pager pagerConsultationReplyed;
		private System.Web.UI.WebControls.Literal litreply;
		private System.Web.UI.WebControls.Literal litNotReverted;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserConsultations.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.dlstPtConsultationReplyed = (ThemedTemplatedList)this.FindControl("dlstPtConsultationReplyed");
			this.pagerConsultationReplyed = (Pager)this.FindControl("pagerConsultationReplyed");
			this.litreply = (System.Web.UI.WebControls.Literal)this.FindControl("litreply");
			this.litNotReverted = (System.Web.UI.WebControls.Literal)this.FindControl("litNotReverted");
			PageTitle.AddSiteNameTitle("咨询/已回复");
			if (!this.Page.IsPostBack)
			{
				this.BindPtConsultationReplyed();
			}
		}
		private void BindPtConsultationReplyed()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.PageIndex = this.pagerConsultationReplyed.PageIndex;
			productConsultationAndReplyQuery.UserId = HiContext.Current.User.UserId;
			productConsultationAndReplyQuery.Type = ConsultationReplyType.Replyed;
			int num = 0;
			DataSet productConsultationsAndReplys = ProductBrowser.GetProductConsultationsAndReplys(productConsultationAndReplyQuery, out num);
			this.dlstPtConsultationReplyed.DataSource = productConsultationsAndReplys.Tables[0].DefaultView;
			this.dlstPtConsultationReplyed.DataBind();
			this.pagerConsultationReplyed.TotalRecords = num;
			this.litreply.Text = num.ToString();
			this.litNotReverted.Text = (productConsultationsAndReplys.Tables[1].Select("UserId=" + productConsultationAndReplyQuery.UserId).Length - num).ToString();
		}
	}
}
