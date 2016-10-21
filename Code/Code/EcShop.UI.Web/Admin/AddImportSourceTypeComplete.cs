using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    public class AddImportSourceTypeComplete : AdminPage
	{
        private int importSourceId;
		private int isEdit;
		protected System.Web.UI.WebControls.Literal txtAction1;
		protected System.Web.UI.WebControls.Literal txtAction2;
		protected System.Web.UI.WebControls.Literal txtAction;
        protected System.Web.UI.WebControls.HyperLink hlinkImportSourceTypeEdit;
        protected System.Web.UI.WebControls.HyperLink hlinkAddImportSourceType;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!int.TryParse(base.Request.QueryString["importSourceId"], out this.importSourceId))
			{
				base.GotoResourceNotFound();
				return;
			}
			int.TryParse(base.Request.QueryString["isEdit"], out this.isEdit);
			if (!this.Page.IsPostBack)
			{
				if (this.isEdit == 1)
				{
					this.txtAction.Text = (this.txtAction1.Text = (this.txtAction2.Text = "编辑"));
				}
				else
				{
					this.txtAction.Text = (this.txtAction1.Text = (this.txtAction2.Text = "添加"));
				}

                this.hlinkAddImportSourceType.NavigateUrl = Globals.GetAdminAbsolutePath("/product/AddImportSourceType.aspx");
                this.hlinkImportSourceTypeEdit.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/AddImportSourceType.aspx?importSourceId={0}", this.importSourceId));
			}
		}
	}
}
