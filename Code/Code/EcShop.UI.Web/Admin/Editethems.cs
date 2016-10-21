using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Editethems)]
	public class Editethems : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litThemeName;
		protected System.Web.UI.WebControls.Repeater rp_productype;
		protected System.Web.UI.WebControls.Repeater rp_brand;
		protected System.Web.UI.WebControls.Repeater rp_custom;  
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.litThemeName.Text = HiContext.Current.SiteSettings.Theme;
			this.DataBindBrandCustom();
			this.DataBindCategoryCustom();
			this.DataBindCustomTheme();
		}
		private void DataBindCategoryCustom()
		{
			System.Collections.Generic.IEnumerable<CategoryInfo> enumerable = 
				from info in CatalogHelper.GetMainCategories()
				where info.Theme != null && info.Theme != ""
				select info;
			if (enumerable.Count<CategoryInfo>() > 0)
			{
				this.rp_productype.DataSource = enumerable;
				this.rp_productype.DataBind();
			}
		}
		private void DataBindBrandCustom()
		{
			System.Data.DataTable brandCategories = CatalogHelper.GetBrandCategories();
			brandCategories.DefaultView.RowFilter = "Theme is not null";
			if (brandCategories.Rows.Count > 0)
			{
				this.rp_brand.DataSource = brandCategories;
				this.rp_brand.DataBind();
			}
		}
		private void DataBindCustomTheme()
		{
			this.rp_custom.DataSource = this.GetCustomDocument();
			this.rp_custom.DataBind();
		}
		private System.Xml.XmlNodeList GetCustomDocument()
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/" + HiContext.Current.SiteSettings.Theme + ".xml");
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			return xmlDocument.SelectNodes("//CustomTheme/Theme");
		}
	}
}
