using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_Help1 : ThemedTemplatedRepeater//ԭ�еĿؼ�
	{
		protected override void OnLoad(EventArgs e)
		{
			base.DataSource = CommentBrowser.GetHelps();
			base.DataBind();
		}
	}
}
