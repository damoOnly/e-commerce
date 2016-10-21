using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Favorites : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtKeyWord;
		private ThemedTemplatedRepeater favorites;
		private ThemedTemplatedRepeater favoritesTags;
		private Pager pager;
		private System.Web.UI.WebControls.ImageButton btnSearch;
		private System.Web.UI.WebControls.LinkButton btnDeleteSelect;
		private string tagname = string.Empty;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-Favorites.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.favorites = (ThemedTemplatedRepeater)this.FindControl("rptFavorites");
			this.favoritesTags = (ThemedTemplatedRepeater)this.FindControl("rptFavoritesTags");
			this.btnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.txtKeyWord = (System.Web.UI.WebControls.TextBox)this.FindControl("txtKeyWord");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = (System.Web.UI.WebControls.LinkButton)this.FindControl("btnDeleteSelect");
			this.btnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.btnSearch_Click);
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			PageTitle.AddSiteNameTitle("商品收藏夹");
			if (!this.Page.IsPostBack)
			{
				this.BindList();
				this.BindFavoritesTags();
			}
		}
		private void BindFavoritesTags()
		{
			this.favoritesTags.DataSource = ProductBrowser.GetFavoritesTypeTags();
			this.favoritesTags.DataBind();
		}
		protected void btnDeleteSelect_Click(object sender, System.EventArgs e)
		{
			string ids = this.Page.Request["CheckboxGroup"];
			if (!ProductBrowser.DeleteFavorites(ids))
			{
				this.ShowMessage("删除失败", false);
			}
			this.ReloadFavorites();
		}
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadFavorites();
		}
		private void BindList()
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = this.pager.PageSize;
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
			{
				text = Globals.HtmlDecode(this.Page.Request.QueryString["keyword"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tags"]))
			{
				this.tagname = Globals.HtmlDecode(this.Page.Request.QueryString["tags"]);
			}
			DbQueryResult dbQueryResult = ProductBrowser.GetFavorites(text, this.tagname, pagination);
			this.favorites.DataSource = dbQueryResult.Data;
			this.favorites.DataBind();
			this.txtKeyWord.Text = text;
			this.pager.TotalRecords = dbQueryResult.TotalRecords;
		}
		private void ReloadFavorites()
		{
			base.ReloadPage(new NameValueCollection
			{

				{
					"keyword",
					Globals.HtmlEncode(this.txtKeyWord.Text.Trim())
				},

				{
					"tags",
					Globals.HtmlEncode(this.tagname)
				}
			});
		}
	}
}
