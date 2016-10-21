using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.Entities.Commodities;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class ShareLine : AdminPage
	{
		private int pagesize = 10;
		private int pageindex = 1;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
		protected System.Web.UI.WebControls.Repeater rp_shareproduct;
		protected Pager pager;
		protected System.Web.UI.HtmlControls.HtmlInputHidden shareId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden shareUrl;
		protected System.Web.UI.WebControls.TextBox txtshareTitle;
		protected System.Web.UI.WebControls.Button btnUpdateValue;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnUpdateValue.Click += new System.EventHandler(this.btnUpdateValue_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.rp_shareproduct.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rp_shareproduct_ItemCommand);
			if (!base.IsPostBack)
			{
				this.BindShareLine();
			}
		}
		private void rp_shareproduct_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "delete")
			{
				int num = 0;
				int.TryParse(e.CommandArgument.ToString(), out num);
				if (num > 0)
				{
					ProductHelper.DeleteShareLine(num);
					this.ShowMsg("删除分享页成功", true);
					this.BindShareLine();
					return;
				}
				this.ShowMsg("删除分享页失败", false);
			}
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择要删除的分享页", false);
				return;
			}
			if (ProductHelper.DeleteShareLine(text))
			{
				this.ShowMsg("批量删除成功", true);
				this.BindShareLine();
				return;
			}
			this.ShowMsg("批量删除失败", false);
		}
		private void btnUpdateValue_Click(object sender, System.EventArgs e)
		{
			ShareProductInfo shareProductInfo = new ShareProductInfo();
			if (!string.IsNullOrEmpty(this.txtshareTitle.Text))
			{
				shareProductInfo.ShareTitle = this.txtshareTitle.Text.Trim();
			}
			if (!string.IsNullOrEmpty(this.shareId.Value))
			{
				shareProductInfo.ShareId = System.Convert.ToInt32(this.shareId.Value);
			}
			if (!string.IsNullOrEmpty(this.shareUrl.Value))
			{
				shareProductInfo.ShareUrl = this.shareUrl.Value;
			}
			ProductHelper.UpdateShareLine(shareProductInfo);
			this.BindShareLine();
		}
		private void BindShareLine()
		{
			this.LoadParams();
			System.Data.DataSet shareProcuts = ProductHelper.GetShareProcuts(this.pagesize, this.pageindex);
			this.rp_shareproduct.DataSource = shareProcuts;
			this.rp_shareproduct.DataBind();
		}
		private void LoadParams()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pagesize"]))
			{
				int.TryParse(this.Page.Request.QueryString["pagesize"], out this.pagesize);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageindex"]))
			{
				int.TryParse(this.Page.Request.QueryString["pageindex"], out this.pageindex);
			}
		}
	}
}
