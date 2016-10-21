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
	public class WapSubReferrals : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptSubReferrals;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubReferrals.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.rptSubReferrals = (WapTemplatedRepeater)this.FindControl("rptSubReferrals");
			PageTitle.AddSiteNameTitle("下级推广员");
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
			memberQuery.ReferralStatus = new int?(2);
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
			this.rptSubReferrals.DataSource = mySubUsers.Data;
			this.rptSubReferrals.DataBind();
			int totalRecords = mySubUsers.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
	}
}
