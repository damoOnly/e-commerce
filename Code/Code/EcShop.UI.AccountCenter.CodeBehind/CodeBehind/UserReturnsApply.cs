using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserReturnsApply : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtOrderId;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private Common_OrderManage_ReturnsApply listReturns;
		private System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReturnsApply.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtOrderId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOrderId");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.listReturns = (Common_OrderManage_ReturnsApply)this.FindControl("Common_OrderManage_ReturnsApply");
			this.ddlHandleStatus = (System.Web.UI.WebControls.DropDownList)this.FindControl("ddlHandleStatus");
			this.pager = (Pager)this.FindControl("pager");
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindReturns();
			}
		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadReturns(true);
		}
		private void BindReturns()
		{
			ReturnsApplyQuery returnsQuery = this.GetReturnsQuery();
			returnsQuery.UserId = new int?(HiContext.Current.User.UserId);
			DbQueryResult returnsApplys = TradeHelper.GetReturnsApplys(returnsQuery);
			this.listReturns.DataSource = returnsApplys.Data;
			this.listReturns.DataBind();
			this.pager.TotalRecords = returnsApplys.TotalRecords;
			this.txtOrderId.Text = returnsQuery.OrderId;
			this.ddlHandleStatus.SelectedIndex = 0;
			if (returnsQuery.HandleStatus.HasValue && returnsQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = returnsQuery.HandleStatus.Value.ToString();
			}
		}
		private ReturnsApplyQuery GetReturnsQuery()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					returnsApplyQuery.HandleStatus = new int?(num);
				}
			}
			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			return returnsApplyQuery;
		}
		private void ReloadReturns(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
			{
				nameValueCollection.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
