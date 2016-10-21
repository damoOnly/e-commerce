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
	public class AliOHSplittinDrawRecords : AliOHMemberTemplatedWebControl
	{
		private AliOHTemplatedRepeater rptDrawRecodes;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDrawRecords.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.rptDrawRecodes = (AliOHTemplatedRepeater)this.FindControl("rptDrawRecodes");
			PageTitle.AddSiteNameTitle("提现记录");
			Users.GetUser(HiContext.Current.User.UserId, false);
			int arg_5B_0 = HiContext.Current.User.UserId;
			this.BindDrawRecords();
		}
		private void BindDrawRecords()
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
			DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
			{
				PageIndex = pageIndex,
				PageSize = pageSize,
				UserId = new int?(HiContext.Current.User.UserId)
			}, null);
			this.rptDrawRecodes.DataSource = mySplittinDraws.Data;
			this.rptDrawRecodes.DataBind();
			int totalRecords = mySplittinDraws.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
	}
}
