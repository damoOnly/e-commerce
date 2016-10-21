using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class AllHotKeywords : ThemedTemplatedRepeater
	{
		protected override void OnLoad(EventArgs e)
		{
			base.DataSource = CommentBrowser.GetAllHotKeywords();
			base.DataBind();
		}
	}
}
