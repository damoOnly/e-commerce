using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHProductList : AliOHTemplatedWebControl
	{
		private int categoryId;
		private string keyWord;
		private HiImage imgUrl;
		private System.Web.UI.WebControls.Literal litContent;
		private AliOHTemplatedRepeater rptProducts;
		private AliOHTemplatedRepeater rptCategories;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductList.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = this.keyWord.Trim();
			}
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.rptProducts = (AliOHTemplatedRepeater)this.FindControl("rptProducts");
			this.rptCategories = (AliOHTemplatedRepeater)this.FindControl("rptCategories");
			this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			string text = this.Page.Request.QueryString["sort"];
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "DisplaySequence";
			}
			string text2 = this.Page.Request.QueryString["order"];
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = "desc";
			}
			int pageNumber;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageNumber))
			{
				pageNumber = 1;
			}
			int maxNum;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out maxNum))
			{
				maxNum = 20;
			}
			System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 1000);
			this.rptCategories.DataSource = maxSubCategories;
			this.rptCategories.DataBind();
			int num;
			this.rptProducts.DataSource = ProductBrowser.GetProducts(null, new int?(this.categoryId), this.keyWord, pageNumber, maxNum, out num, text, text2);
			this.rptProducts.DataBind();
			this.txtTotalPages.SetWhenIsNotNull(num.ToString());
			PageTitle.AddSiteNameTitle("分类搜索页");
		}
	}
}
