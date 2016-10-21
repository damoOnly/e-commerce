using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class MemberSafeGradeLiteral : System.Web.UI.WebControls.Literal
	{
		private string _cssclass = "panquandu";
		public string CssClass
		{
			get
			{
				return this._cssclass;
			}
			set
			{
				this._cssclass = value;
			}
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
			int num = 0;
			if (member.EmailVerification)
			{
				num++;
			}
			if (member.CellPhoneVerification)
			{
				num++;
			}
			if (!string.IsNullOrEmpty(member.PasswordQuestion))
			{
				num++;
			}
			if (num <= 1)
			{
				base.Text = "<div class=\"" + this._cssclass + "A\"></div>\u3000<span class=\"hongse\">低</span>";
			}
			else
			{
				if (num <= 2)
				{
					base.Text = "<div class=\"" + this._cssclass + "B\"></div>\u3000<span class=\"huangse\">中</span>";
				}
				else
				{
					base.Text = "<div class=\"" + this._cssclass + "C\"></div>\u3000<span class=\"green\">高</span>";
				}
			}
			base.Render(writer);
		}
	}
}
