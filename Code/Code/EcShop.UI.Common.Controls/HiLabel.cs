using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class HiLabel : Label, IText
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
