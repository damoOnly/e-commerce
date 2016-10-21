using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddProductType : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtTypeName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTypeNameTip;
		protected BrandCategoriesCheckBoxList chlistBrand;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
		protected System.Web.UI.WebControls.Button btnAddProductType;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddProductType.Click += new System.EventHandler(this.btnAddProductType_Click);
			if (!base.IsPostBack)
			{
				this.chlistBrand.DataBind();
			}
		}
		private void btnAddProductType_Click(object sender, System.EventArgs e)
		{
			ProductTypeInfo productTypeInfo = new ProductTypeInfo();
			productTypeInfo.TypeName = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtTypeName.Text).Replace("，", ",").Replace("\\", ""));
			if (string.IsNullOrEmpty(productTypeInfo.TypeName))
			{
				this.ShowMsg("类型名称不能为空,不允许包含脚本标签、HTML标签和\\,系统会自动过滤", false);
				return;
			}
			productTypeInfo.Remark = this.txtRemark.Text;
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistBrand.Items)
			{
				if (listItem.Selected)
				{
					list.Add(int.Parse(listItem.Value));
				}
			}
			productTypeInfo.Brands = list;
			if (!this.ValidationProductType(productTypeInfo))
			{
				return;
			}
			int num = ProductTypeHelper.AddProductType(productTypeInfo);
			if (num > 0)
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/AddAttribute.aspx?typeId=" + num), true);
				return;
			}
			this.ShowMsg("添加商品类型失败", false);
		}
		private bool ValidationProductType(ProductTypeInfo productType)
		{
			ValidationResults validationResults = Validation.Validate<ProductTypeInfo>(productType, new string[]
			{
				"ValProductType"
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
