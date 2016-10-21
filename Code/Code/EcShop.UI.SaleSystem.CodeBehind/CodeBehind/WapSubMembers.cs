using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WapSubMembers : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptSubMembers;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubMembers.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.rptSubMembers = (WapTemplatedRepeater)this.FindControl("rptSubMembers");
			PageTitle.AddSiteNameTitle("下级会员");
			Users.GetUser(HiContext.Current.User.UserId, false);
			int arg_5B_0 = HiContext.Current.User.UserId;
			this.BindSubReferrals();
		}
		private void BindSubReferrals()
		{
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 10;
			}
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.PageIndex = pageIndex;
			memberQuery.PageSize = pageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
			{
				memberQuery.Username = this.Page.Server.UrlDecode(this.Page.Request.QueryString["keyword"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realname"]))
			{
				memberQuery.Realname = this.Page.Server.UrlDecode(this.Page.Request.QueryString["realname"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cellPhone"]))
			{
				memberQuery.CellPhone = this.Page.Server.UrlDecode(this.Page.Request.QueryString["cellPhone"]);
			}
			DbQueryResult mySubUsers = MemberProcessor.GetMySubUsers(memberQuery);
			this.rptSubMembers.DataSource = mySubUsers.Data;
			this.rptSubMembers.DataBind();
			int totalRecords = mySubUsers.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
	}
}
