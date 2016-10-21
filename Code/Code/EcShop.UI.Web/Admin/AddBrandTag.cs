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
    public class AddBrandTag : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtBrandName;
        protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected System.Web.UI.WebControls.TextBox txtCompanyUrl;
        protected System.Web.UI.WebControls.TextBox txtReUrl;
        protected System.Web.UI.WebControls.TextBox txtkeyword;
        protected System.Web.UI.WebControls.TextBox txtMetaDescription;
        protected KindeditorControl fckDescription;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnAddBrandCategory;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnAddBrandCategory.Click += new System.EventHandler(this.btnAddBrandCategory_Click);
            if (!base.IsPostBack)
            {
                this.chlistBrand.DataBind();
            }
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            BrandTagInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (!this.ValidationBrandCategory(brandCategoryInfo))
            {
                return;
            }
            if (CatalogHelper.AddBrandTag(brandCategoryInfo))
            {
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandType.aspx"), true);
                return;
            }
            this.ShowMsg("添加品牌标签失败", true);
            return;
        }
        protected void btnAddBrandCategory_Click(object sender, System.EventArgs e)
        {
            string value = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtBrandName.Text.Trim())).Replace("\\", ""));
            if (string.IsNullOrEmpty(value))
            {
                this.ShowMsg("请填写品牌标签名称,品牌标签名称中不能包含HTML字符，脚本字符，以及\\", false);
                return;
            }
            BrandTagInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (!this.ValidationBrandCategory(brandCategoryInfo))
            {
                return;
            }
            if (CatalogHelper.AddBrandTag(brandCategoryInfo))
            {
                this.ShowMsg("成功添加品牌标签", true);
                return;
            }
            this.ShowMsg("添加品牌标签失败", true);
            return;
        }
        private BrandTagInfo GetBrandCategoryInfo()
        {
            BrandTagInfo brandCategoryInfo = new BrandTagInfo();
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
