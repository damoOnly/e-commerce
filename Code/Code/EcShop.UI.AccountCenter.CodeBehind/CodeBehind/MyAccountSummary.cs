using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class MyAccountSummary : MemberTemplatedWebControl
	{
		private Common_Advance_AccountList accountList;
		private Pager pager;
		private WebCalendar calendarStart;
		private WebCalendar calendarEnd;
		private TradeTypeDropDownList dropTradeType;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private FormatedMoneyLabel litAccountAmount;
		private FormatedMoneyLabel litRequestBalance;
		private FormatedMoneyLabel litUseableBalance;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyAccountSummary.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.accountList = (Common_Advance_AccountList)this.FindControl("Common_Advance_AccountList");
			this.pager = (Pager)this.FindControl("pager");
			this.calendarStart = (WebCalendar)this.FindControl("calendarStart");
			this.calendarEnd = (WebCalendar)this.FindControl("calendarEnd");
			this.dropTradeType = (TradeTypeDropDownList)this.FindControl("dropTradeType");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
			this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			PageTitle.AddSiteNameTitle("预付款账户");
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", System.Web.HttpContext.Current.Request.Url));
				}
				this.BindBalanceDetails();
				this.litAccountAmount.Money = member.Balance;
				this.litRequestBalance.Money = member.RequestBalance;
				this.litUseableBalance.Money = member.Balance - member.RequestBalance;
			}
		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadMyBalanceDetails(true);
		}
		private void BindBalanceDetails()
		{
			BalanceDetailQuery balanceDetailQuery = this.GetBalanceDetailQuery();
			DbQueryResult balanceDetails = MemberProcessor.GetBalanceDetails(balanceDetailQuery);
			this.accountList.DataSource = balanceDetails.Data;
			this.accountList.DataBind();
			this.dropTradeType.DataBind();
			this.dropTradeType.SelectedValue = balanceDetailQuery.TradeType;
			this.calendarStart.SelectedDate = balanceDetailQuery.FromDate;
			this.calendarEnd.SelectedDate = balanceDetailQuery.ToDate;
			this.pager.TotalRecords = balanceDetails.TotalRecords;
		}
		private BalanceDetailQuery GetBalanceDetailQuery()
		{
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
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tradeType"]))
			{
				balanceDetailQuery.TradeType = (TradeTypes)System.Convert.ToInt32(this.Page.Server.UrlDecode(this.Page.Request.QueryString["tradeType"]));
			}
			balanceDetailQuery.PageIndex = this.pager.PageIndex;
			balanceDetailQuery.PageSize = this.pager.PageSize;
			return balanceDetailQuery;
		}
		private void ReloadMyBalanceDetails(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			nameValueCollection.Add("tradeType", ((int)this.dropTradeType.SelectedValue).ToString());
			base.ReloadPage(nameValueCollection);
		}
	}
}
