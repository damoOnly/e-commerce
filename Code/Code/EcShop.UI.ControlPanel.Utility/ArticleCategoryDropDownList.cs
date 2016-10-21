using EcShop.ControlPanel.Comments;
using EcShop.Core;
using EcShop.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ArticleCategoryDropDownList : DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "";
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}
		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}
		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
					return;
				}
				base.SelectedIndex = -1;
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			IList<ArticleCategoryInfo> mainArticleCategories = ArticleHelper.GetMainArticleCategories();
			foreach (ArticleCategoryInfo current in mainArticleCategories)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.CategoryId.ToString()));
			}
		}
	}
}
