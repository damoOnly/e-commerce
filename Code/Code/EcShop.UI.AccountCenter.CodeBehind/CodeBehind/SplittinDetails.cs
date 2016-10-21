using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class SplittinDetails : MemberTemplatedWebControl
	{
		private Common_Referral_Splitin rptReferralSplitin;
		private Pager pager;
		private WebCalendar calendarStart;
		private WebCalendar calendarEnd;
		private System.Web.UI.WebControls.DropDownList dropSplittingType;
		private System.Web.UI.WebControls.DropDownList dropUseStatus;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private System.Web.UI.WebControls.TextBox txtOrderId;
		private FormatedMoneyLabel litAllSplittin;
		private FormatedMoneyLabel litUseSplittin;
		private FormatedMoneyLabel litNoUseSplittin;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SplittinDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptReferralSplitin = (Common_Referral_Splitin)this.FindControl("Common_Referral_Splitin");
			this.pager = (Pager)this.FindControl("pager");
			this.calendarStart = (WebCalendar)this.FindControl("calendarStart");
			this.calendarEnd = (WebCalendar)this.FindControl("calendarEnd");
			this.dropSplittingType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropSplittingType");
			this.dropUseStatus = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropUseStatus");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.litAllSplittin = (FormatedMoneyLabel)this.FindControl("litAllSplittin");
			this.litUseSplittin = (FormatedMoneyLabel)this.FindControl("litUseSplittin");
			this.litNoUseSplittin = (FormatedMoneyLabel)this.FindControl("litNoUseSplittin");
			this.txtOrderId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOrderId");
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			PageTitle.AddSiteNameTitle("我的佣金");
			if (!this.Page.IsPostBack)
			{
				this.BindSplittins();
				int userId = HiContext.Current.User.UserId;
				this.litAllSplittin.Money = MemberProcessor.GetUserAllSplittin(userId);
				this.litUseSplittin.Money = MemberProcessor.GetUserUseSplittin(userId);
				this.litNoUseSplittin.Money = MemberProcessor.GetUserNoUseSplittin(userId);
			}
		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadMyBalanceDetails(true);
		}
		private void BindSplittins()
		{
			BalanceDetailQuery query = this.GetQuery();
			bool? flag = null;
			bool value = true;
			if (this.Page.Request.QueryString["usestatus"] != null && !string.IsNullOrEmpty(this.Page.Request.QueryString["usestatus"]) && bool.TryParse(this.Page.Request.QueryString["usestatus"], out value))
			{
				flag = new bool?(value);
			}
			DbQueryResult mySplittinDetails = MemberProcessor.GetMySplittinDetails(query, flag);
			this.rptReferralSplitin.DataSource = mySplittinDetails.Data;
			this.rptReferralSplitin.DataBind();
			this.dropSplittingType.Items.Add(new System.Web.UI.WebControls.ListItem("全部", "0"));
			this.dropSplittingType.Items.Add(new System.Web.UI.WebControls.ListItem("直接推广", "1"));
			this.dropSplittingType.Items.Add(new System.Web.UI.WebControls.ListItem("下级会员", "2"));
			this.dropSplittingType.Items.Add(new System.Web.UI.WebControls.ListItem("下级推广员", "3"));
			this.dropSplittingType.Items.Add(new System.Web.UI.WebControls.ListItem("提现", "4"));
			this.dropSplittingType.SelectedValue = ((int)query.SplittingTypes).ToString();
			this.dropUseStatus.Items.Add(new System.Web.UI.WebControls.ListItem("全部", ""));
			this.dropUseStatus.Items.Add(new System.Web.UI.WebControls.ListItem("可用", "true"));
			this.dropUseStatus.Items.Add(new System.Web.UI.WebControls.ListItem("不可用", "false"));
			this.dropUseStatus.SelectedValue = (flag.HasValue ? ((flag == true) ? "true" : "false") : "");
			this.calendarStart.SelectedDate = query.FromDate;
			this.calendarEnd.SelectedDate = query.ToDate;
			this.pager.TotalRecords = mySplittinDetails.TotalRecords;
		}
		private BalanceDetailQuery GetQuery()
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
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["splittingtype"]))
			{
				balanceDetailQuery.SplittingTypes = (SplittingTypes)System.Convert.ToInt32(this.Page.Server.UrlDecode(this.Page.Request.QueryString["splittingtype"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderid"]))
			{
				balanceDetailQuery.OrderId = this.Page.Request.QueryString["orderid"];
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
			nameValueCollection.Add("splittingtype", this.dropSplittingType.SelectedValue);
			nameValueCollection.Add("usestatus", this.dropUseStatus.SelectedValue);
			nameValueCollection.Add("orderid", this.txtOrderId.Text);
			base.ReloadPage(nameValueCollection);
		}
	}
}
