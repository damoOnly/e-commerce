using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class WapCountDownProductList : WAPTemplatedWebControl
	{
		private int categoryId;
		private string keyWord;
		private HiImage imgUrl;
		private System.Web.UI.WebControls.Literal litContent;
		private WapTemplatedRepeater rptProducts;
		private WapTemplatedRepeater rptCategories;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VCountDownProductList.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptCountDownProducts");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.rptCategories = (WapTemplatedRepeater)this.FindControl("rptCategories");
			if (this.rptCategories != null)
			{
				System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 1000);
				this.rptCategories.DataSource = maxSubCategories;
				this.rptCategories.DataBind();
			}
			int page;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out page))
			{
				page = 1;
			}
			int size;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out size))
			{
				size = 10;
			}
			int num;
			DataTable countDownProductList = ProductBrowser.GetCountDownProductList(new int?(this.categoryId), this.keyWord, page, size, out num, true);
			this.rptProducts.DataSource = countDownProductList;
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(num.ToString());
			PageTitle.AddSiteNameTitle("限时抢购");
		}
	}
}
