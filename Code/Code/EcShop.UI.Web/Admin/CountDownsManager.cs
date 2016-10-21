using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.CountDownsManager)]
    public class CountDownsManager : AdminPage
	{
		private string Title = string.Empty;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.WebControls.LinkButton btnOrder;
		protected Grid grdCountDownsList;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdCountDownsList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdGroupBuyList_RowDeleting);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindCountDown();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void BindCountDown()
		{
            DbQueryResult countDownList = PromoteHelper.GetCountManagerDownList(new GroupBuyQuery
			{
                Title = this.Title,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc
			});
			this.grdCountDownsList.DataSource = countDownList.Data;
			this.grdCountDownsList.DataBind();
			this.pager.TotalRecords = countDownList.TotalRecords;
			this.pager1.TotalRecords = countDownList.TotalRecords;
		}
		private void btnOrder_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCountDownsList.Rows)
			{
				int displaySequence = 0;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out displaySequence))
				{
					int countDownId = (int)this.grdCountDownsList.DataKeys[gridViewRow.RowIndex].Value;
                    PromoteHelper.SwapCountCategoriesDownSequence(countDownId, displaySequence);
				}
			}
			this.BindCountDown();
		}
		private void grdGroupBuyList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
            if (PromoteHelper.DeleteCountDownCategories((int)this.grdCountDownsList.DataKeys[e.RowIndex].Value))
			{
				this.BindCountDown();
				this.ShowMsg("成功删除了选择的限时记录", true);
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("title", Globals.UrlEncode(this.txtTitle.Text.Trim()));
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
			nameValueCollection.Add("SortBy", this.grdCountDownsList.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadHelpList(true);
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCountDownsList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int countDownId = System.Convert.ToInt32(this.grdCountDownsList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
                    PromoteHelper.DeleteCountDownCategories(countDownId);
				}
			}
			if (num != 0)
			{
				this.BindCountDown();
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除\"{0}\"条限时记录", new object[]
				{
					num
				}), true);
				return;
			}
			this.ShowMsg("请先选择需要删除信息！", false);
		}
		private void LoadParameters()
		{
			if (!base.IsPostBack)
			{
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["title"]))
				{
                    this.Title = Globals.UrlDecode(this.Page.Request.QueryString["title"]);
				}
                this.txtTitle.Text = this.Title;
				return;
			}
            this.Title = this.txtTitle.Text;
		}
	}
}
