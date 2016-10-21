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
	public class UserReplaceApply : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtOrderId;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private Common_OrderManage_ReplaceApply listReplace;
		private System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReplaceApply.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtOrderId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOrderId");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.listReplace = (Common_OrderManage_ReplaceApply)this.FindControl("Common_OrderManage_ReplaceApply");
			this.ddlHandleStatus = (System.Web.UI.WebControls.DropDownList)this.FindControl("ddlHandleStatus");
			this.pager = (Pager)this.FindControl("pager");
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindRefund();
			}
		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadReplace(true);
		}
		private void BindRefund()
		{
			ReplaceApplyQuery replaceQuery = this.GetReplaceQuery();
			replaceQuery.UserId = new int?(HiContext.Current.User.UserId);
			DbQueryResult replaceApplys = TradeHelper.GetReplaceApplys(replaceQuery);
			this.listReplace.DataSource = replaceApplys.Data;
			this.listReplace.DataBind();
			this.pager.TotalRecords = replaceApplys.TotalRecords;
			this.txtOrderId.Text = replaceQuery.OrderId;
			this.ddlHandleStatus.SelectedIndex = 0;
			if (replaceQuery.HandleStatus.HasValue && replaceQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = replaceQuery.HandleStatus.Value.ToString();
			}
		}
		private ReplaceApplyQuery GetReplaceQuery()
		{
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				replaceApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					replaceApplyQuery.HandleStatus = new int?(num);
				}
			}
			replaceApplyQuery.PageIndex = this.pager.PageIndex;
			replaceApplyQuery.PageSize = this.pager.PageSize;
			replaceApplyQuery.SortBy = "ApplyForTime";
			replaceApplyQuery.SortOrder = SortAction.Desc;
			return replaceApplyQuery;
		}
		private void ReloadReplace(bool isSearch)
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
