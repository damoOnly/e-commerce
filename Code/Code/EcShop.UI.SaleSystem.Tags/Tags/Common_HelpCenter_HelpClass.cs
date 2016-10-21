using EcShop.Entities.Comments;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_HelpCenter_HelpClass : ThemedTemplatedRepeater
	{
		public const string TagID = "list_Common_HelpCenter_HelpClass";
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public int MaxNum
		{
			get;
			set;
		}
		public Common_HelpCenter_HelpClass()
		{
			base.ID = "list_Common_HelpCenter_HelpClass";
		}
		protected override void OnLoad(EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				base.DataSource = this.GetDataSource();
				base.DataBind();
			}
		}
		private IList<HelpCategoryInfo> GetDataSource()
		{
			IList<HelpCategoryInfo> list = new List<HelpCategoryInfo>();
			list = CommentBrowser.GetHelpCategorys();
			if (this.MaxNum > 0 && this.MaxNum < list.Count)
			{
				for (int i = list.Count - 1; i >= this.MaxNum; i--)
				{
					list.RemoveAt(i);
				}
			}
			return list;
		}
	}
}
