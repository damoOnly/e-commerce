using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_FriendLinks : ThemedTemplatedRepeater
	{
		private int? maxNum;
		public int? MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			base.DataSource = CommentBrowser.GetFriendlyLinksIsVisible(this.MaxNum);
			base.DataBind();
		}
	}
}
