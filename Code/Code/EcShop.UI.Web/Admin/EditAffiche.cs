using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Affiches)]
	public class EditAffiche : AdminPage
	{
		private int afficheId;
		protected System.Web.UI.WebControls.TextBox txtAfficheTitle;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnEditAffiche;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["afficheId"], out this.afficheId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditAffiche.Click += new System.EventHandler(this.btnEditAffiche_Click);
			if (!this.Page.IsPostBack)
			{
				AfficheInfo affiche = NoticeHelper.GetAffiche(this.afficheId);
				if (affiche == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(affiche, false);
				this.txtAfficheTitle.Text = affiche.Title;
				this.fcContent.Text = affiche.Content;
			}
		}
		private void btnEditAffiche_Click(object sender, System.EventArgs e)
		{
			AfficheInfo afficheInfo = new AfficheInfo();
			afficheInfo.AfficheId = this.afficheId;
			afficheInfo.Title = this.txtAfficheTitle.Text.Trim();
			afficheInfo.Content = this.fcContent.Text;
			afficheInfo.AddedDate = System.DateTime.Now;
			ValidationResults validationResults = Validation.Validate<AfficheInfo>(afficheInfo, new string[]
			{
				"ValAfficheInfo"
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
			afficheInfo.AfficheId = this.afficheId;
			if (NoticeHelper.UpdateAffiche(afficheInfo))
			{
				this.ShowMsg("成功修改了当前公告信息", true);
				return;
			}
			this.ShowMsg("修改公告信息错误", false);
		}
	}
}
