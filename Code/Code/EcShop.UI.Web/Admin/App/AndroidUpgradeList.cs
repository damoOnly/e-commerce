using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.App
{
    [PrivilegeCheck(Privilege.AndroidUpgradeList)]
    public class AndroidUpgradeList : AdminPage
    {
        protected System.Web.UI.WebControls.Repeater rptAndroidUpgrades;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            {
                this.BindData();
            }
        }

        private void BindData()
        {
            this.rptAndroidUpgrades.DataSource = APPHelper.GetAppVersionRecordList("android");
            this.rptAndroidUpgrades.DataBind();
        }

        protected void rptAndroidUpgrades_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                if (APPHelper.DelAppVersionById(id))
                {
                    this.ShowMsg("删除成功", true);
                    this.BindData();
                    return;
                }

                else
                {
                    this.ShowMsg("删除失败", false);
                    return;
                }

            }
        }





    }
}

