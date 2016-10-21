using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class SplittingTypeNameLabel : Label
	{
		public string SplittingType
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			string splittingType;
			if ((splittingType = this.SplittingType) != null)
			{
				if (splittingType == "1")
				{
					base.Text = "直接推广佣金";
					goto IL_7F;
				}
				if (splittingType == "2")
				{
					base.Text = "下级会员佣金";
					goto IL_7F;
				}
				if (splittingType == "3")
				{
					base.Text = "下级推广员佣金";
					goto IL_7F;
				}
				if (splittingType == "4")
				{
					base.Text = "提现";
					goto IL_7F;
				}
			}
			base.Text = "其他";
			IL_7F:
			base.Render(writer);
		}
	}
}
