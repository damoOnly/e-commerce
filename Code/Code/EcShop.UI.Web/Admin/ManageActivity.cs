using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManageActivity)]
	public class ManageActivity : AdminPage
	{
		protected Grid grdActivity;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdActivity.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdActivity_RowDeleting);
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void grdActivity_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int activityId = (int)this.grdActivity.DataKeys[e.RowIndex].Value;
			if (VShopHelper.DeleteActivity(activityId))
			{
				this.BindData();
				this.ShowMsg("成功删除了选定的活动", true);
				return;
			}
			this.ShowMsg("删除活动失败", false);
		}
		private void BindData()
		{
			System.Collections.Generic.IList<ActivityInfo> allActivity = VShopHelper.GetAllActivity();
			this.grdActivity.DataSource = allActivity;
			this.grdActivity.DataBind();
			this.pager.TotalRecords = allActivity.Count;
		}
		public string GetUrl(object activityId)
		{
			return string.Concat(new object[]
			{
				"http://",
				Globals.DomainName,
				"/Vshop/Activity.aspx?id=",
				activityId
			});
		}
	}
}
