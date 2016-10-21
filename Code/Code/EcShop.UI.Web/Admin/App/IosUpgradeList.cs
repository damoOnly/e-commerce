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
     [PrivilegeCheck(Privilege.IosUpgradeList)]
    public class IosUpgradeList : AdminPage
    {
        protected System.Web.UI.WebControls.Repeater rptIosUpgrades;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            {
                this.BindData();
            }
        }

        private void BindData()
        {
            this.rptIosUpgrades.DataSource = APPHelper.GetAppVersionRecordList("ios");
            this.rptIosUpgrades.DataBind();
        }

        protected void rptIosUpgrades_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
             if (e.CommandName=="Delete")
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

