using EcShop.Membership.Context;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_AliOHPaymentTypeSelect : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
			stringBuilder.AppendLine("<ul id=\"selectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
			stringBuilder.AppendLine("<li><a href=\"#\" name=\"-6\">帐户余额支付</a></li>");
			if (masterSettings.EnableAliOHOffLinePay)
			{
				stringBuilder.AppendLine("<li><a href=\"#\" name=\"-1\">线下付款</a></li>");
			}
			if (masterSettings.EnableAliOHPodPay)
			{
				stringBuilder.AppendLine("<li><a href=\"#\" name=\"0\">货到付款</a></li>");
			}
			if (masterSettings.EnableAliOHAliPay)
			{
				stringBuilder.AppendLine("<li><a href=\"#\" name=\"-4\">支付宝手机网页支付</a></li>");
			}
			if (masterSettings.EnableAliOHShengPay)
			{
				stringBuilder.AppendLine("<li><a href=\"#\" name=\"-5\">盛付通手机网页支付</a></li>");
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
