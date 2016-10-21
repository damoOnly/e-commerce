using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.App
{
    [PrivilegeCheck(Privilege.ManageAppIcon)]
    public class ManageAppIcon : AdminPage
    {
        protected Grid grdIcon;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
        private void BindData()
        {
            this.grdIcon.DataSource = VShopHelper.GetAllIcon(ClientType.App);
            this.grdIcon.DataBind();
        }
        protected void grdIcon_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
            int num = (int)this.grdIcon.DataKeys[rowIndex].Value;
            int num2 = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < this.grdIcon.Rows.Count - 1)
                {
                    num2 = (int)this.grdIcon.DataKeys[rowIndex + 1].Value;
                }
            }
            else
            {
                if (e.CommandName == "Rise" && rowIndex > 0)
                {
                    num2 = (int)this.grdIcon.DataKeys[rowIndex - 1].Value;
                }
            }
            if (num2 > 0)
            {
                VShopHelper.SwapTplCfgSequence(num, num2);
                base.ReloadPage(null);
            }
            if (e.CommandName == "Delete")
            {
                if (VShopHelper.DelTplCfg(num))
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
