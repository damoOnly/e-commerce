using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserBatchBuy : MemberTemplatedWebControl
	{
		private Common_BatchBuy_ProductList batchbuys;
		private System.Web.UI.WebControls.Repeater rpSku;
		private IButton btnBatchBuy;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private BrandCategoriesDropDownList dropBrandCategories;
		private Common_CategoriesDropDownList ddlCategories;
		private Pager pager;
		private System.Web.UI.WebControls.TextBox txtProductName;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserBatchBuy.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.batchbuys = (Common_BatchBuy_ProductList)this.FindControl("Common_BatchBuy_ProductList");
			this.btnBatchBuy = ButtonManager.Create(this.FindControl("btnBatchBuy"));
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.dropBrandCategories = (BrandCategoriesDropDownList)this.FindControl("dropBrandCategories");
			this.ddlCategories = (Common_CategoriesDropDownList)this.FindControl("ddlCategories");
			this.pager = (Pager)this.FindControl("pager");
			this.txtProductName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtProductName");
			this.btnBatchBuy.Click += new System.EventHandler(this.btnBatchBuy_Click);
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			this.batchbuys.ItemDataBound += new Common_BatchBuy_ProductList.ItemEventHandler(this.batchbuys_ItemDataBound);
			if (!this.Page.IsPostBack)
			{
				this.dropBrandCategories.DataBind();
				this.ddlCategories.DataBind();
				this.BindProducts();
			}
		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProducts(true);
		}
		private void batchbuys_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				this.rpSku = (e.Item.FindControl("rptSkus") as System.Web.UI.WebControls.Repeater);
				object value = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "productid");
				if (this.rpSku != null)
				{
					this.rpSku.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rpSku_ItemDataBound);
					this.rpSku.DataSource = ProductBrowser.GetSkusByProductId(System.Convert.ToInt32(value));
					this.rpSku.DataBind();
				}
			}
		}
		private void rpSku_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("skuContent");
			if (literal != null)
			{
				object obj = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "skuid");
				literal.Text = this.GetSkuContent((string)obj);
			}
		}
		public string GetSkuContent(string skuId)
		{
			string text = "";
			if (!string.IsNullOrEmpty(skuId.Trim()))
			{
				DataTable skuContentBySku = ControlProvider.Instance().GetSkuContentBySku(skuId);
				foreach (DataRow dataRow in skuContentBySku.Rows)
				{
					if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							dataRow["AttributeName"],
							":",
							dataRow["ValueStr"],
							"; "
						});
					}
				}
			}
			if (!(text == ""))
			{
				return text;
			}
			return "无规格";
		}
		private void btnBatchBuy_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			string text = this.Page.Request.Form["chkskus"];
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					string text3 = this.Page.Request.Form[text2];
					if (!string.IsNullOrWhiteSpace(text3) && int.Parse(text3) > 0)
					{
						ShoppingCartProcessor.AddLineItem(text2, int.Parse(text3.Trim()),0);
						num++;
					}
				}
			}
			if (num > 0)
			{
				this.ShowMessage("选择的商品已经放入购物车", true);
			}
			else
			{
				this.ShowMessage("请选择要购买的商品！", false);
			}
			this.BindProducts();
		}
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyname"]))
			{
				this.txtProductName.Text = Globals.UrlDecode(this.Page.Request.QueryString["keyname"]);
			}
			int value;
			if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["brandId"]), out value))
			{
				this.dropBrandCategories.SelectedValue = new int?(value);
			}
			int value2;
			if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["categoryId"]), out value2))
			{
				this.ddlCategories.SelectedValue = new int?(value2);
			}
		}
		private void BindProducts()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = this.pager.PageSize;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.Keywords = this.txtProductName.Text;
			productQuery.BrandId = this.dropBrandCategories.SelectedValue;
			productQuery.CategoryId = this.ddlCategories.SelectedValue;
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CategoryBrowser.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			Globals.EntityCoding(productQuery, true);
			DbQueryResult batchBuyProducts = ProductBrowser.GetBatchBuyProducts(productQuery);
			this.batchbuys.DataSource = batchBuyProducts.Data;
			this.batchbuys.DataBind();
			this.pager.TotalRecords = batchBuyProducts.TotalRecords;
		}
		private void ReloadProducts(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			new ProductQuery();
			if (!string.IsNullOrEmpty(this.txtProductName.Text.Trim()))
			{
				nameValueCollection.Add("keyname", Globals.UrlEncode(this.txtProductName.Text.Trim()));
			}
			if (this.dropBrandCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", Globals.UrlEncode(this.dropBrandCategories.SelectedValue.Value.ToString()));
			}
			if (this.ddlCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", Globals.UrlEncode(this.ddlCategories.SelectedValue.Value.ToString()));
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
