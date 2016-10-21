using System;
namespace Ecdev.Weixin.MP.Domain.Menu
{
	public class Menu
	{
		public ButtonGroup menu
		{
			get;
			set;
		}
		public Menu()
		{
			this.menu = new ButtonGroup();
		}
	}
}
