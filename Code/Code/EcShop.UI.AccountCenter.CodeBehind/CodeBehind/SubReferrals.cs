using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class SubReferrals : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtSearchText;
		private System.Web.UI.WebControls.TextBox txtRealName;
		private System.Web.UI.WebControls.TextBox txtCellPhone;
		private System.Web.UI.WebControls.ImageButton btnSearchButton;
		private Common_Referral_MemberList grdReferralmembers;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SubReferrals.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtSearchText = (System.Web.UI.WebControls.TextBox)this.FindControl("txtSearchText");
			this.txtRealName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRealName");
			this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
			this.btnSearchButton = (System.Web.UI.WebControls.ImageButton)this.FindControl("btnSearchButton");
			this.grdReferralmembers = (Common_Referral_MemberList)this.FindControl("Common_Referral_MemberList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnSearchButton.Click += new System.Web.UI.ImageClickEventHandler(this.btnSearchButton_Click);
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("下级推广员");
				MemberQuery memberQuery = new MemberQuery();
				memberQuery.ReferralStatus = new int?(2);
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["username"]))
				{
					memberQuery.Username = this.Page.Server.UrlDecode(this.Page.Request.QueryString["username"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realname"]))
				{
					memberQuery.Realname = this.Page.Server.UrlDecode(this.Page.Request.QueryString["realname"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cellPhone"]))
				{
					memberQuery.CellPhone = this.Page.Server.UrlDecode(this.Page.Request.QueryString["cellPhone"]);
				}
				memberQuery.PageIndex = this.pager.PageIndex;
				memberQuery.PageSize = this.pager.PageSize;
				DbQueryResult mySubUsers = MemberProcessor.GetMySubUsers(memberQuery);
				this.grdReferralmembers.DataSource = mySubUsers.Data;
				this.grdReferralmembers.DataBind();
				this.txtSearchText.Text = memberQuery.Username;
				this.txtRealName.Text = memberQuery.Realname;
				this.txtCellPhone.Text = memberQuery.CellPhone;
				this.pager.TotalRecords = mySubUsers.TotalRecords;
			}
		}
		private void btnSearchButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			this.ReloadReferralMembers(true);
		}
		private void ReloadReferralMembers(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("username", this.txtSearchText.Text.Trim());
			nameValueCollection.Add("realname", this.txtRealName.Text.Trim());
			nameValueCollection.Add("cellPhone", this.txtCellPhone.Text.Trim());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
