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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserRefundApply : MemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputText txtOrderId;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private Common_OrderManage_RefundApply RefundList;
		private System.Web.UI.HtmlControls.HtmlSelect handleStatus;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserRefundApply.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtOrderId = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtOrderId");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.RefundList = (Common_OrderManage_RefundApply)this.FindControl("Common_OrderManage_RefundApply");
			this.handleStatus = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl("handleStatus");
			this.pager = (Pager)this.FindControl("pager");
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindRefund();
			}
		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadRefunds(true);
		}
		private void BindRefund()
		{
			RefundApplyQuery refundQuery = this.GetRefundQuery();
			refundQuery.UserId = new int?(HiContext.Current.User.UserId);
			DbQueryResult refundApplys = TradeHelper.GetRefundApplys(refundQuery);
			this.RefundList.DataSource = refundApplys.Data;
			this.RefundList.DataBind();
			this.pager.TotalRecords = refundApplys.TotalRecords;
			this.txtOrderId.Value = refundQuery.OrderId;
			this.handleStatus.SelectedIndex = 0;
			if (refundQuery.HandleStatus.HasValue && refundQuery.HandleStatus.Value > -1)
			{
				this.handleStatus.Value = refundQuery.HandleStatus.Value.ToString();
			}
		}
		private RefundApplyQuery GetRefundQuery()
		{
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				refundApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					refundApplyQuery.HandleStatus = new int?(num);
				}
			}
			refundApplyQuery.PageIndex = this.pager.PageIndex;
			refundApplyQuery.PageSize = this.pager.PageSize;
			refundApplyQuery.SortBy = "ApplyForTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			return refundApplyQuery;
		}
		private void ReloadRefunds(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Value);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			if (!string.IsNullOrEmpty(this.handleStatus.Value))
			{
				nameValueCollection.Add("HandleStatus", this.handleStatus.Value);
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
