using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Comments;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductConsultationsManage)]
	public class ReplyProductConsultations : AdminPage
	{
		private int consultationId;
		protected System.Web.UI.WebControls.Literal litUserName;
		protected FormatedTimeLabel lblTime;
		protected System.Web.UI.WebControls.Literal litConsultationText;
		protected KindeditorControl fckReplyText;
		protected System.Web.UI.WebControls.Button btnReplyProductConsultation;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ConsultationId"], out this.consultationId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReplyProductConsultation.Click += new System.EventHandler(this.btnReplyProductConsultation_Click);
			if (!this.Page.IsPostBack)
			{
				ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
				if (productConsultation == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litUserName.Text = productConsultation.UserName;
				this.litConsultationText.Text = productConsultation.ConsultationText;
				this.lblTime.Time = productConsultation.ConsultationDate;
			}
		}
		protected void btnReplyProductConsultation_Click(object sender, System.EventArgs e)
		{
			ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
			if (string.IsNullOrEmpty(this.fckReplyText.Text))
			{
				productConsultation.ReplyText = null;
			}
			else
			{
				productConsultation.ReplyText = this.fckReplyText.Text;
			}
			productConsultation.ReplyUserId = new int?(HiContext.Current.User.UserId);
			productConsultation.ReplyDate = new System.DateTime?(System.DateTime.Now);
			ValidationResults validationResults = Validation.Validate<ProductConsultationInfo>(productConsultation, new string[]
			{
				"Reply"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
			{
				this.fckReplyText.Text = string.Empty;
				this.CloseWindow();
				return;
			}
			this.ShowMsg("回复商品咨询失败", false);
		}
	}
}
