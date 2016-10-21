using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_CategoriesWithWindow : AscxTemplatedWebControl
    {
        private Repeater recordsone;
        private int maxCNum = 13;
        private int maxBNum = 1000;
        public int MaxCNum
        {
            get
            {
                return this.maxCNum;
            }
            set
            {
                this.maxCNum = value;
            }
        }
        public int MaxBNum
        {
            get
            {
                return this.maxBNum;
            }
            set
            {
                this.maxBNum = value;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-CategoriesWithWindow.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.recordsone = (Repeater)this.FindControl("recordsone");
            this.recordsone.ItemDataBound += new RepeaterItemEventHandler(this.recordsone_ItemDataBound);
            this.recordsone.ItemCreated += new RepeaterItemEventHandler(this.recordsone_ItemCreated);
            /***是否禁用，0：启用；1：禁用***/
            int isDisable = 0;
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(0, isDisable, this.maxCNum);
            if (maxSubCategories != null && maxSubCategories.Count > 0)
            {
                this.recordsone.DataSource = maxSubCategories;
                this.recordsone.DataBind();
            }
        }
        private void recordsone_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater)control.FindControl("recordstwo");
            repeater.ItemDataBound += new RepeaterItemEventHandler(this.recordstwo_ItemDataBound);
        }
        private void recordsone_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater)control.FindControl("recordstwo");
            HtmlInputHidden htmlInputHidden = (HtmlInputHidden)control.FindControl("hidMainCategoryId");
            int mainCategoryId = int.Parse(htmlInputHidden.Value);
            Repeater repeater2 = (Repeater)control.FindControl("recordsbrands");
            Repeater repeater3 = (Repeater)control.FindControl("rphotkey");
            Repeater recordTwoCategory = (Repeater)control.FindControl("recordTwoCategory");
            Repeater recordtwoHotBuyProduct = (Repeater)control.FindControl("recordtwoHotBuyProduct");
            repeater2.DataSource = CategoryBrowser.GetBrandCategories(mainCategoryId, 12);
            repeater2.DataBind();
            repeater.DataSource = CategoryBrowser.GetMaxSubCategories(mainCategoryId,0, 1000);
            repeater.DataBind();
            if (repeater3 != null)
            {
                repeater3.DataSource = CommentBrowser.GetHotKeywords(mainCategoryId, 12);
                repeater3.DataBind();
            }
            if (recordTwoCategory != null)
            {
                recordTwoCategory.DataSource = CategoryBrowser.GetMaxSubCategories(mainCategoryId,0, 1000);
                recordTwoCategory.DataBind();
            }
            if (recordtwoHotBuyProduct != null)
            {
                recordtwoHotBuyProduct.DataSource = ProductBrowser.GetHotBuyProduct(mainCategoryId);
                recordtwoHotBuyProduct.DataBind();
            }
        }
        private void recordstwo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater)control.FindControl("recordsthree");
            HtmlInputHidden htmlInputHidden = (HtmlInputHidden)control.FindControl("hidTwoCategoryId");
            repeater.DataSource = CategoryBrowser.GetMaxSubCategories(int.Parse(htmlInputHidden.Value),0, 1000);
            repeater.DataBind();
        }
    }
}
