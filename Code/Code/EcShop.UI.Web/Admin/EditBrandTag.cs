using Ecdev.Components.Validation;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.BrandType)]
    public class EditBrandTag : AdminPage
    {
        private int brandId;
        protected System.Web.UI.WebControls.TextBox txtBrandName;
        protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected System.Web.UI.WebControls.Button btnUpoad;
        protected HiImage imgLogo;
        protected ImageLinkButton btnDeleteLogo;
        protected System.Web.UI.WebControls.TextBox txtCompanyUrl;
        protected System.Web.UI.WebControls.TextBox txtReUrl;
        protected System.Web.UI.WebControls.TextBox txtkeyword;
        protected System.Web.UI.WebControls.TextBox txtMetaDescription;
        protected KindeditorControl fckDescription;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected System.Web.UI.WebControls.Button btnUpdateBrandCategory;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnUpdateBrandCategory.Click += new System.EventHandler(this.btnUpdateBrandCategory_Click);     
        
            if (!this.Page.IsPostBack)
            {
                this.chlistBrand.DataBind();
                this.loadData();
            }
        }
        private void loadData()
        {
            BrandTagInfo brandCategory = CatalogHelper.GetBrandTag(this.brandId);
            if (brandCategory == null)
            {
                base.GotoResourceNotFound();
                return;
            }
            foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistBrand.Items)
            {
                if (brandCategory.BrandCategoryInfos.Contains(int.Parse(listItem.Value)))
                {
                    listItem.Selected = true;
                }
            }          
            this.txtBrandName.Text = Globals.HtmlDecode(brandCategory.TagName);
        
        }
 
        protected void btnUpdateBrandCategory_Click(object sender, System.EventArgs e)
        {
            string value = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtBrandName.Text.Trim())).Replace("\\", ""));
            if (string.IsNullOrEmpty(value))
            {
                this.ShowMsg("请填写品牌标签,品牌标签名称中不能包含HTML字符，脚本字符，以及\\", false);
                return;
            }
            BrandTagInfo brandCategoryInfo = this.GetBrandCategoryInfo();
           
            if (!this.ValidationBrandCategory(brandCategoryInfo))
            {
                return;
            }
            if (CatalogHelper.UpdateBrandTag(brandCategoryInfo))
            {
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandType.aspx"), true);
                return;
            }
            this.ShowMsg("编辑品牌标签失败", true);
        }
       
        private BrandTagInfo GetBrandCategoryInfo()
        {
            BrandTagInfo brandCategoryInfo = new BrandTagInfo();
            brandCategoryInfo.BrandTagId = this.brandId;
           
            brandCategoryInfo.TagName = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtBrandName.Text.Trim())).Replace("\\", ""));
            
            System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
            foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistBrand.Items)
            {
                if (listItem.Selected)
                {
                    list.Add(int.Parse(listItem.Value));
                }
            }
            brandCategoryInfo.BrandCategoryInfos = list;
            return brandCategoryInfo;
        }
        private bool ValidationBrandCategory(BrandTagInfo brandCategory)
        {
            ValidationResults validationResults = Validation.Validate<BrandTagInfo>(brandCategory, new string[]
			{
				"ValBrandCategory"
			});
            string text = string.Empty;
            if (!validationResults.IsValid)
            {
                foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
                {
                    text += Formatter.FormatErrorMessage(current.Message);
                }
                this.ShowMsg(text, false);
            }
            return validationResults.IsValid;
        }
    }
}
