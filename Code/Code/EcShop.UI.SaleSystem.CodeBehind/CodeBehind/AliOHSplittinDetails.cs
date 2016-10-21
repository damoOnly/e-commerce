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
	public class AliOHSplittinDetails : AliOHMemberTemplatedWebControl
	{
		private FormatedMoneyLabel litSplittinDraws;
		private FormatedMoneyLabel litAllSplittin;
		private AliOHTemplatedRepeater rptSplittin;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.litSplittinDraws = (FormatedMoneyLabel)this.FindControl("litSplittinDraws");
			this.litAllSplittin = (FormatedMoneyLabel)this.FindControl("litAllSplittin");
			this.rptSplittin = (AliOHTemplatedRepeater)this.FindControl("rptSplittin");
			PageTitle.AddSiteNameTitle("佣金明细");
			Users.GetUser(HiContext.Current.User.UserId, false);
			int userId = HiContext.Current.User.UserId;
			this.litAllSplittin.Money = MemberProcessor.GetUserAllSplittin(userId);
			this.litSplittinDraws.Money = MemberProcessor.GetUserUseSplittin(userId);
			this.BindSplittins();
		}
		private void BindSplittins()
		{
			BalanceDetailQuery query = this.GetQuery();
			bool value = false;
			bool? isUse;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["usestatus"]))
			{
				bool.TryParse(this.Page.Request.QueryString["usestatus"], out value);
				isUse = new bool?(value);
			}
			else
			{
				isUse = null;
			}
			DbQueryResult mySplittinDetails = MemberProcessor.GetMySplittinDetails(query, isUse);
			this.rptSplittin.DataSource = mySplittinDetails.Data;
			this.rptSplittin.DataBind();
			int totalRecords = mySplittinDetails.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
		private BalanceDetailQuery GetQuery()
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
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			balanceDetailQuery.UserId = new int?(HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
			{
				balanceDetailQuery.FromDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
			{
				balanceDetailQuery.ToDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["splittingtype"]))
			{
				balanceDetailQuery.SplittingTypes = (SplittingTypes)System.Convert.ToInt32(this.Page.Server.UrlDecode(this.Page.Request.QueryString["splittingtype"]));
			}
			balanceDetailQuery.PageIndex = pageIndex;
			balanceDetailQuery.PageSize = pageSize;
			return balanceDetailQuery;
		}
	}
}
