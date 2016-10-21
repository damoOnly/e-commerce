using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public static class ControlHelper
	{
		public static void SetWhenIsNotNull(this System.Web.UI.Control control, string value)
		{
			if (control == null)
			{
				return;
			}
			if (control is System.Web.UI.ITextControl)
			{
				System.Web.UI.ITextControl textControl = (System.Web.UI.ITextControl)control;
				textControl.Text = value;
				return;
			}
			if (control is System.Web.UI.HtmlControls.HtmlInputControl)
			{
				System.Web.UI.HtmlControls.HtmlInputControl htmlInputControl = (System.Web.UI.HtmlControls.HtmlInputControl)control;
				htmlInputControl.Value = value;
				return;
			}
			if (control is System.Web.UI.WebControls.HyperLink)
			{
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)control;
				hyperLink.NavigateUrl = value;
				return;
			}
			throw new System.ApplicationException("未实现" + control.GetType().ToString() + "的SetWhenIsNotNull方法");
		}
	}
}
