using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.vshop
{
    [PrivilegeCheck(Privilege.SupplierManageNavigate)]
    public class SupplierManageNavigate : AdminPage
    {
        protected Grid grdNavigate;

        private int supplierId;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //根据登录用户获取供应商id
            supplierId = CheckSupplierRole();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
        private void BindData()
        {
            

            this.grdNavigate.DataSource = VShopHelper.GetAllNavigate(ClientType.VShop, supplierId);
            this.grdNavigate.DataBind();
        }
        protected string GetImageUrl(string url)
        {
            string text = url;
            if (string.IsNullOrWhiteSpace(text))
            {
                text = "/utility/pics/none.gif";
            }
            else
            {
                if (!url.ToLower().Contains("storage/master/navigate") && !url.ToLower().Contains("templates"))
                {
                    text = HiContext.Current.GetVshopSkinPath(null) + "/images/deskicon/" + url;
                }
            }
            return text;
        }
        protected void grdNavigate_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
            int num = (int)this.grdNavigate.DataKeys[rowIndex].Value;
            int num2 = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < this.grdNavigate.Rows.Count - 1)
                {
                    num2 = (int)this.grdNavigate.DataKeys[rowIndex + 1].Value;
                }
            }
            else
            {
                if (e.CommandName == "Rise" && rowIndex > 0)
                {
                    num2 = (int)this.grdNavigate.DataKeys[rowIndex - 1].Value;
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
