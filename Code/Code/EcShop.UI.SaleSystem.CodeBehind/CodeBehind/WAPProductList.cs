using EcShop.ControlPanel.Store;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
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
	public class WAPProductList : WAPTemplatedWebControl
	{
		private int categoryId;
		private string keyWord;
		private HiImage imgUrl;
		private System.Web.UI.WebControls.Literal litContent;
		//private WapTemplatedRepeater rptCategories;
        private System.Web.UI.HtmlControls.HtmlAnchor txtSearch;

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
			this.keyWord = System.Web.HttpUtility.UrlDecode(this.Page.Request.QueryString["keyWord"]);

			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = this.keyWord.Trim();
                if(HiContext.Current.User.UserId>0)
                {
                    HistorySearchHelp.NewSearchHistory(this.keyWord.Trim(), HiContext.Current.User.UserId, Entities.ClientType.WAP);
                }
			}

			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			//this.rptCategories = (WapTemplatedRepeater)this.FindControl("rptCategories");
            this.txtSearch = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("txtSearch");
            if (this.categoryId > 0)
            {
               this.txtSearch.InnerText = CategoryBrowser.GetCategory(this.categoryId).Name;
            }
            else if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.txtSearch.InnerText = this.keyWord;
            }

            //System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 1000);
            //this.rptCategories.DataSource = maxSubCategories;
            //this.rptCategories.DataBind();
          
			//this.txtTotalPages.SetWhenIsNotNull(num.ToString());
			PageTitle.AddSiteNameTitle("分类搜索页");
		}
	}
}
