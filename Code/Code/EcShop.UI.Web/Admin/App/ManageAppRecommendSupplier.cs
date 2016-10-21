using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.App
{
    [PrivilegeCheck(Privilege.ManageAppRecommendSupplier)]
    public class ManageAppRecommendSupplier : AdminPage
    {
        protected Grid grdSupplier;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
        private void BindData()
        {
            this.grdSupplier.DataSource = SupplierConfigHelper.GetSupplierCfgByType(ClientType.App, SupplierCfgType.Recommend);
            this.grdSupplier.DataBind();
        }
        protected void grdSupplier_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
            int num = (int)this.grdSupplier.DataKeys[rowIndex].Value;
            int num2 = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < this.grdSupplier.Rows.Count - 1)
                {
                    num2 = (int)this.grdSupplier.DataKeys[rowIndex + 1].Value;
                }
            }
            else
            {
                if (e.CommandName == "Rise" && rowIndex > 0)
                {
                    num2 = (int)this.grdSupplier.DataKeys[rowIndex - 1].Value;
                }
            }
            if (num2 > 0)
            {
                SupplierConfigHelper.SwapSupplierCfgSequence(num, num2);
                base.ReloadPage(null);
            }
            if (e.CommandName == "Delete")
            {
                if (SupplierConfigHelper.DelSupplierCfg(num))
                {
                    this.ShowMsg("删除成功！", true);
                    base.ReloadPage(null);
                    return;
                }
                this.ShowMsg("删除失败！", false);
            }
        }
    }
}
