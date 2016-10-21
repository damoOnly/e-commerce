using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ProductCategoriesDropDownList : DropDownList
	{
		private string m_NullToDisplay = "";
		private bool m_AutoDataBind;
		private string strDepth = "\u3000\u3000";
		private bool isTopCategory;
		public string NullToDisplay
		{
			get
			{
				return this.m_NullToDisplay;
			}
			set
			{
				this.m_NullToDisplay = value;
			}
		}
		public bool AutoDataBind
		{
			get
			{
				return this.m_AutoDataBind;
			}
			set
			{
				this.m_AutoDataBind = value;
			}
		}
		public bool IsTopCategory
		{
			get
			{
				return this.isTopCategory;
			}
			set
			{
				this.isTopCategory = value;
			}
		}
		public new int? SelectedValue
		{
			get
			{
				if (!string.IsNullOrEmpty(base.SelectedValue))
				{
					return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
				}
				return null;
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
		public bool IsUnclassified
		{
			get;
			set;
		}
		protected override void OnLoad(EventArgs e)
		{
			if (this.AutoDataBind && !this.Page.IsPostBack)
			{
				this.DataBind();
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			if (this.IsUnclassified)
			{
				this.Items.Add(new ListItem("未分类商品", "0"));
			}
			if (this.IsTopCategory)
			{
				IList<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
				using (IEnumerator<CategoryInfo> enumerator = mainCategories.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CategoryInfo current = enumerator.Current;
						this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.CategoryId.ToString()));
					}
					return;
				}
			}
			IList<CategoryInfo> sequenceCategories = CatalogHelper.GetSequenceCategories();
			for (int i = 0; i < sequenceCategories.Count; i++)
			{
				this.Items.Add(new ListItem(this.FormatDepth(sequenceCategories[i].Depth, Globals.HtmlDecode(sequenceCategories[i].Name)), sequenceCategories[i].CategoryId.ToString(CultureInfo.InvariantCulture)));
			}
		}
		private string FormatDepth(int depth, string categoryName)
		{
			for (int i = 1; i < depth; i++)
			{
				categoryName = this.strDepth + categoryName;
			}
			return categoryName;
		}
	}
}
