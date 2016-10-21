using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_HelpItemCategory : ThemedTemplatedRepeater
	{
		protected override void OnInit(EventArgs e)
		{
            base.DataSource = CommentBrowser.GetAllHelps();
			base.DataBind();
		}
	}
}
