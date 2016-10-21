using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class HiLiteral : Literal, IText
	{
		public Control Control
		{
			get
			{
				return this;
			}
		}
	}
}
