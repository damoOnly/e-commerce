using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_CategoriesDropDownList : System.Web.UI.WebControls.DropDownList
	{
		private string m_NullToDisplay = "";
		private bool m_AutoDataBind;
		private string strDepth = "\u3000\u3000";
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
		public new int? SelectedValue
		{
			get
			{
				if (!string.IsNullOrEmpty(base.SelectedValue))
				{
					return new int?(int.Parse(base.SelectedValue, System.Globalization.CultureInfo.InvariantCulture));
				}
				return null;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
					return;
				}
				base.SelectedIndex = -1;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			if (this.AutoDataBind && !this.Page.IsPostBack)
			{
				this.DataBind();
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new System.Web.UI.WebControls.ListItem(this.NullToDisplay, string.Empty));
			System.Collections.Generic.IList<CategoryInfo> sequenceCategories = CategoryBrowser.GetSequenceCategories();
			for (int i = 0; i < sequenceCategories.Count; i++)
			{
				this.Items.Add(new System.Web.UI.WebControls.ListItem(this.FormatDepth(sequenceCategories[i].Depth, Globals.HtmlDecode(sequenceCategories[i].Name)), sequenceCategories[i].CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
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
