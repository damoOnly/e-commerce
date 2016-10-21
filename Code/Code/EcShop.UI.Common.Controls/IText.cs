using System;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	public interface IText
	{
		bool Visible
		{
			get;
			set;
		}
		string Text
		{
			get;
			set;
		}
		Control Control
		{
			get;
		}
	}
}
