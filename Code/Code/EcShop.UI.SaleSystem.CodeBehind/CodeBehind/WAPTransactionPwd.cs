using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPTransactionPwd : WAPTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VTransactionPwd.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("交易密码");
			System.Web.UI.HtmlControls.HtmlInputHidden arg_1A_0 = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidkey");
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)this.FindControl("OrderId");
			System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
			literal.Text = this.Page.Request.QueryString["orderId"];
			decimal num;
			if (decimal.TryParse(this.Page.Request.QueryString["totalAmount"], out num))
			{
				literal2.Text = num.ToString("F2");
				return;
			}
			base.GotoResourceNotFound("传入值有误");
		}
	}
}
