using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin
{
    public class MainAccessDenied : AdminPage
    {
        protected System.Web.UI.WebControls.Literal litMessage;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["errormsg"]))
            {
                this.litMessage.Text = base.Request.QueryString["errormsg"];
                return;
            }
            this.litMessage.Text = string.Format("欢迎进行商城后台管理中心,请点击上方菜单项进入工作项。", HiContext.Current.User.Username);
        }
    }
}
