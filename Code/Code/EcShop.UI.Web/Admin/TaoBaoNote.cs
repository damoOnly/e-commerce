using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class TaoBaoNote : AdminPage
	{
		protected System.Web.UI.WebControls.HyperLink hlinkToTaobao;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.hlinkToTaobao.NavigateUrl = string.Format("http://vip.ecdev.cn/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}", HiContext.Current.SiteUrl, Globals.ApplicationPath);
		}
	}
}
