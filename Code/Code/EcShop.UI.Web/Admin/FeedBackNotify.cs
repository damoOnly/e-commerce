using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Weixin.Pay;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class FeedBackNotify : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected MemberGradeDropDownList rankList;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.DataList dlstPtReviews;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstPtReviews.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstPtReviews_DeleteCommand);
			this.dlstPtReviews.UpdateCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstPtReviews_DeleteCommand);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			if (!base.IsPostBack)
			{
				this.BindPtReview();
			}
		}
		private void dlstPtReviews_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			int id = System.Convert.ToInt32(e.CommandArgument, System.Globalization.CultureInfo.InvariantCulture);
			if (e.CommandName == "Delete")
			{
				if (VShopHelper.DeleteFeedBack(id))
				{
					this.ShowMsg("删除成功", true);
					this.BindPtReview();
					return;
				}
				this.ShowMsg("删除失败", false);
				return;
			}
			else
			{
				FeedBackInfo feedBack = VShopHelper.GetFeedBack(id);
				if (feedBack == null)
				{
					return;
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				PayAccount account = new PayAccount(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
				NotifyClient notifyClient = new NotifyClient(account);
				if (notifyClient.UpdateFeedback(feedBack.FeedBackId, feedBack.OpenId))
				{
					VShopHelper.UpdateFeedBackMsgType(feedBack.FeedBackId, "已处理");
					this.ShowMsg("处理成功", true);
					this.BindPtReview();
					return;
				}
				this.ShowMsg("处理失败", false);
				return;
			}
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.BindPtReview();
		}
		private void BindPtReview()
		{
			string msgType = "";
			switch (this.rankList.SelectedIndex)
			{
			case 1:
				msgType = "未处理";
				break;
			case 2:
				msgType = "已处理";
				break;
			}
			DbQueryResult feedBacks = VShopHelper.GetFeedBacks(this.pager.PageIndex, this.pager.PageSize, msgType);
			this.dlstPtReviews.DataSource = feedBacks.Data;
			this.dlstPtReviews.DataBind();
			this.pager.TotalRecords = feedBacks.TotalRecords;
			this.pager1.TotalRecords = feedBacks.TotalRecords;
		}
	}
}
