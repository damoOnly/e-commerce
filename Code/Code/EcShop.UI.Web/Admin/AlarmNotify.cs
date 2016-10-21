using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class AlarmNotify : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.DataList dlstPtReviews;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstPtReviews.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstPtReviews_DeleteCommand);
			if (!base.IsPostBack)
			{
				this.BindPtReview();
			}
		}
		private void dlstPtReviews_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			int id = System.Convert.ToInt32(e.CommandArgument, System.Globalization.CultureInfo.InvariantCulture);
			if (VShopHelper.DeleteAlarm(id))
			{
				this.ShowMsg("删除成功", true);
				this.BindPtReview();
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void BindPtReview()
		{
			DbQueryResult alarms = VShopHelper.GetAlarms(this.pager.PageIndex, this.pager.PageSize);
			this.dlstPtReviews.DataSource = alarms.Data;
			this.dlstPtReviews.DataBind();
			this.pager.TotalRecords = alarms.TotalRecords;
			this.pager1.TotalRecords = alarms.TotalRecords;
		}
	}
}
