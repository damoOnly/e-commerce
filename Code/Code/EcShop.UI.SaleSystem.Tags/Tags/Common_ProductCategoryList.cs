using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_ProductCategoryList : ThemedTemplatedRepeater
	{
		private int categoryId;
		private int maxNum = 1000;
		public int MaxNum
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
		public bool IsShowSubCategory
		{
			get;
			set;
		}
		protected override void OnLoad(EventArgs e)
		{
			base.ItemDataBound += new RepeaterItemEventHandler(this.Common_ProductCategoryList_ItemDataBound);
			if (this.IsShowSubCategory)
			{
				int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			}
			this.BindList();
		}
		private void Common_ProductCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			int parentCategoryId = ((CategoryInfo)e.Item.DataItem).CategoryId;
			Repeater repeater = (Repeater)e.Item.Controls[0].FindControl("rptSubCategries");
			if (repeater != null)
			{
				IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(parentCategoryId, 1000);
				repeater.DataSource = maxSubCategories;
				repeater.DataBind();
			}
		}
		private void BindList()
		{
			if (this.categoryId != 0)
			{
				IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, this.MaxNum);
				if (maxSubCategories.Count <= 0)
				{
					CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
					if (category == null)
					{
						return;
					}
					int? parentCategoryId = category.ParentCategoryId;
					if (parentCategoryId.HasValue)
					{
						this.categoryId = parentCategoryId.Value;
					}
					maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, this.MaxNum);
				}
				base.DataSource = maxSubCategories;
				base.DataBind();
				return;
			}
			base.DataSource = CategoryBrowser.GetMaxMainCategories(this.MaxNum);
			base.DataBind();
		}
	}
}
