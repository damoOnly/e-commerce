using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class AccessDenied : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litMessage;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["errormsg"]))
			{
				this.litMessage.Text = base.Request.QueryString["errormsg"];
				return;
			}
			this.litMessage.Text = string.Format("您登录的管理员帐号 “{0}” 没有权限访问当前页面或进行当前操作", HiContext.Current.User.Username);
		}
	}
}
