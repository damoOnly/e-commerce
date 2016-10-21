using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BrandCategories)]
	public class EditBrandCategory : AdminPage
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
		protected ProductTypesCheckBoxList chlistProductTypes;
		protected System.Web.UI.WebControls.Button btnUpdateBrandCategory;
        protected System.Web.UI.WebControls.FileUpload fileUpload1;
        protected System.Web.UI.WebControls.Button btnUpoad1;
        protected HiImage imgBigLogo;
        protected ImageLinkButton btnDeleteBigLogo;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnUpdateBrandCategory.Click += new System.EventHandler(this.btnUpdateBrandCategory_Click);
			this.btnUpoad.Click += new System.EventHandler(this.btnUpoad_Click);
			this.btnDeleteLogo.Click += new System.EventHandler(this.btnDeleteLogo_Click);
            this.btnUpoad1.Click += new System.EventHandler(this.btnUpoad1_Click);
            this.btnDeleteBigLogo.Click += new System.EventHandler(this.btnDeleteBigLogo_Click);
			if (!this.Page.IsPostBack)
			{
				this.chlistProductTypes.DataBind();
				this.loadData();
			}
		}
		private void loadData()
		{
			BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
			if (brandCategory == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.ViewState["Logo"] = brandCategory.Logo;
            this.ViewState["BigLogo"] = brandCategory.BigLogo;
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistProductTypes.Items)
			{
				if (brandCategory.ProductTypes.Contains(int.Parse(listItem.Value)))
				{
					listItem.Selected = true;
				}
			}
			this.txtBrandName.Text = Globals.HtmlDecode(brandCategory.BrandName);
			this.txtCompanyUrl.Text = brandCategory.CompanyUrl;
			this.txtReUrl.Text = Globals.HtmlDecode(brandCategory.RewriteName);
			this.txtkeyword.Text = Globals.HtmlDecode(brandCategory.MetaKeywords);
			this.txtMetaDescription.Text = Globals.HtmlDecode(brandCategory.MetaDescription);
			this.fckDescription.Text = brandCategory.Description;
			if (string.IsNullOrEmpty(brandCategory.Logo))
			{
				this.btnDeleteLogo.Visible = false;
			}
			else
			{
				this.btnDeleteLogo.Visible = true;
			}
            if (string.IsNullOrEmpty(brandCategory.BigLogo))
            {
                this.btnDeleteBigLogo.Visible = false;
            }
            else
            {
                this.btnDeleteBigLogo.Visible = true;
            }
			this.imgLogo.ImageUrl = brandCategory.Logo;
            this.imgBigLogo.ImageUrl = brandCategory.BigLogo;
		}
		private void btnUpoad_Click(object sender, System.EventArgs e)
		{
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			
				try
				{
					ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
					brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(this.fileUpload.PostedFile);
					this.ViewState["Logo"] = brandCategoryInfo.Logo;
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
				CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
			
			this.loadData();
		}
        private void btnUpoad1_Click(object sender, System.EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
           
                try
                {
                    ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
                    brandCategoryInfo.BigLogo = CatalogHelper.UploadBrandCategorieImage(this.fileUpload1.PostedFile);
                    this.ViewState["BigLogo"] = brandCategoryInfo.BigLogo;
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
                CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
          
            this.loadData();
        }
		protected void btnUpdateBrandCategory_Click(object sender, System.EventArgs e)
		{
			string value = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtBrandName.Text.Trim())).Replace("\\", ""));
			if (string.IsNullOrEmpty(value))
			{
				this.ShowMsg("请填写品牌名称,品牌名称中不能包含HTML字符，脚本字符，以及\\", false);
				return;
			}
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			
			if (!this.ValidationBrandCategory(brandCategoryInfo))
			{
				return;
			}
			if (CatalogHelper.UpdateBrandCategory(brandCategoryInfo))
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandCategories.aspx"), true);
				return;
			}
			this.ShowMsg("编辑品牌分类失败", true);
		}
		private void btnDeleteLogo_Click(object sender, System.EventArgs e)
		{
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			try
			{
				ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
				brandCategoryInfo.Logo = null;
				this.ViewState["Logo"] = null;
				CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
			}
			catch
			{
				this.ShowMsg("删除失败", false);
				return;
			}
			this.loadData();
		}

        private void btnDeleteBigLogo_Click(object sender, System.EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            try
            {
                ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
                brandCategoryInfo.BigLogo = null;
                this.ViewState["BigLogo"] = null;
                CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
            }
            catch
            {
                this.ShowMsg("删除失败", false);
                return;
            }
            this.loadData();
        }
		private BrandCategoryInfo GetBrandCategoryInfo()
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
			brandCategoryInfo.BrandId = this.brandId;
			if (this.ViewState["Logo"] != null)
			{
				brandCategoryInfo.Logo = (string)this.ViewState["Logo"];
			}
            if (this.ViewState["BigLogo"] != null)
            {
                brandCategoryInfo.BigLogo = (string)this.ViewState["BigLogo"];
            }
			brandCategoryInfo.BrandName = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtBrandName.Text.Trim())).Replace("\\", ""));
			if (!string.IsNullOrEmpty(this.txtCompanyUrl.Text))
			{
				brandCategoryInfo.CompanyUrl = this.txtCompanyUrl.Text.Trim();
			}
			else
			{
				brandCategoryInfo.CompanyUrl = null;
			}
			brandCategoryInfo.RewriteName = Globals.HtmlEncode(this.txtReUrl.Text.Trim());
			brandCategoryInfo.MetaKeywords = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtkeyword.Text.Trim())).Replace("\\", ""));
			brandCategoryInfo.MetaDescription = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtMetaDescription.Text.Trim())).Replace("\\", ""));
			brandCategoryInfo.Description = ((!string.IsNullOrEmpty(this.fckDescription.Text) && this.fckDescription.Text.Length > 0) ? this.fckDescription.Text : null);
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistProductTypes.Items)
			{
				if (listItem.Selected)
				{
					list.Add(int.Parse(listItem.Value));
				}
			}
			brandCategoryInfo.ProductTypes = list;
			return brandCategoryInfo;
		}
		private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
		{
			ValidationResults validationResults = Validation.Validate<BrandCategoryInfo>(brandCategory, new string[]
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
