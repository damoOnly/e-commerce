using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class SiteCopyright : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(string.Format("<div class=\"copyright\">Powered by <a target=\"_blank\" href=\"http://www.ecdev.cn\"><font color=\"#0033cc\">B2C商城</font></a><font color=\"#ff6633\">&nbsp" + HiContext.Current.Config.Version + "</font> &copy; 2012 - 2015&nbsp;<a target=\"_blank\" href=\"http://www.ecdev.cn\" style=\"\"><img src=\"{0}\" align=\"absmiddle\" /></a>&nbsp;Inc.</div>", Globals.ApplicationPath + "/Admin/images/images_4.jpg"));
		}
	}
}
