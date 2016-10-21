using EcShop.ControlPanel.Comments;
using EcShop.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ArticleCategoriesListBox : ListBox
	{
		public new IList<int> SelectedValue
		{
			get
			{
				IList<int> list = new List<int>();
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						list.Add(int.Parse(this.Items[i].Value));
					}
				}
				return list;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].Selected = false;
				}
				foreach (int current in value)
				{
					for (int j = 0; j < this.Items.Count; j++)
					{
						if (this.Items[j].Value == current.ToString())
						{
							this.Items[j].Selected = true;
						}
					}
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			IList<ArticleCategoryInfo> mainArticleCategories = ArticleHelper.GetMainArticleCategories();
			foreach (ArticleCategoryInfo current in mainArticleCategories)
			{
				this.Items.Add(new ListItem(current.Name, current.CategoryId.ToString()));
			}
		}
	}
}
