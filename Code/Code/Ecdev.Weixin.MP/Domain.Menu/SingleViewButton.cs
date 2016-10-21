using System;
namespace Ecdev.Weixin.MP.Domain.Menu
{
	public class SingleViewButton : SingleButton
	{
		public string url
		{
			get;
			set;
		}
		public SingleViewButton() : base(ButtonType.view.ToString())
		{
		}
	}
}
