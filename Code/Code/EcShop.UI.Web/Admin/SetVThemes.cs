using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true), PrivilegeCheck(Privilege.SetVThemes)]
	public class SetVThemes : AdminPage
	{
		private string path;
		private string vTheme;
		protected System.Web.UI.WebControls.Literal litThemeName;
		protected System.Web.UI.WebControls.TextBox txtTopicProductMaxNum;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTopicProductMaxNumTip;
		protected System.Web.UI.WebControls.TextBox txtCategoryProductMaxNum;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtCategoryProductMaxNumTip;
		protected System.Web.UI.WebControls.RadioButton RadDefault;
		protected System.Web.UI.WebControls.RadioButton RadCustom;
		protected System.Web.UI.HtmlControls.HtmlGenericControl customerbg;
		protected System.Web.UI.HtmlControls.HtmlAnchor delpic;
		protected HiImage imgPic;
		protected System.Web.UI.HtmlControls.HtmlImage littlepic;
		protected System.Web.UI.WebControls.TextBox txtMarketPrice;
		protected System.Web.UI.WebControls.TextBox txtSalePrice;
		protected System.Web.UI.WebControls.TextBox txtPhone;
		protected System.Web.UI.WebControls.TextBox txtNavigate;
		protected System.Web.UI.WebControls.Button btnOK;
		protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.vTheme = base.Request.QueryString["themeName"];
			if (string.IsNullOrWhiteSpace(this.vTheme))
			{
				this.vTheme = SettingsManager.GetMasterSettings(true).VTheme;
			}
			this.path = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Templates/vshop/" + this.vTheme + "/template.xml");
			this.litThemeName.Text = this.vTheme;
			if (!this.Page.IsPostBack)
			{
				System.Xml.XmlDocument xmlNode = this.GetXmlNode();
				this.txtTopicProductMaxNum.Text = xmlNode.SelectSingleNode("root/TopicProductMaxNum").InnerText;
				this.txtCategoryProductMaxNum.Text = xmlNode.SelectSingleNode("root/CategoryProductMaxNum").InnerText;
				this.txtMarketPrice.Text = xmlNode.SelectSingleNode("root/MarketPrice").InnerText;
				this.txtSalePrice.Text = xmlNode.SelectSingleNode("root/SalePrice").InnerText;
				this.txtPhone.Text = xmlNode.SelectSingleNode("root/Phone").InnerText;
				this.txtNavigate.Text = xmlNode.SelectSingleNode("root/Navigate").InnerText;
				this.imgPic.ImageUrl = HiContext.Current.GetVshopSkinPath(this.vTheme) + "/images/ad/imgDefaultBg.jpg";
				if (xmlNode.SelectSingleNode("root/DefaultBg").InnerText.EndsWith("imgDefaultBg.jpg"))
				{
					this.RadDefault.Checked = true;
					this.delpic.Attributes.CssStyle.Add("display", "none");
					this.customerbg.Attributes.CssStyle.Add("display", "none");
					this.littlepic.Attributes.CssStyle.Add("display", "none");
				}
				else
				{
					this.littlepic.Src = xmlNode.SelectSingleNode("root/DefaultBg").InnerText;
					this.fmSrc.Value = xmlNode.SelectSingleNode("root/DefaultBg").InnerText;
					this.RadCustom.Checked = true;
					this.customerbg.Attributes.CssStyle.Remove("display");
					this.delpic.Attributes.CssStyle.Remove("display");
					this.littlepic.Attributes.CssStyle.Remove("display");
				}
			}
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			System.Xml.XmlDocument xmlNode = this.GetXmlNode();
			if (this.RadDefault.Checked || string.IsNullOrWhiteSpace(this.fmSrc.Value))
			{
				xmlNode.SelectSingleNode("root/DefaultBg").InnerText = HiContext.Current.GetVshopSkinPath(this.vTheme) + "/images/ad/imgDefaultBg.jpg";
			}
			else
			{
				xmlNode.SelectSingleNode("root/DefaultBg").InnerText = this.fmSrc.Value;
			}
			xmlNode.SelectSingleNode("root/TopicProductMaxNum").InnerText = this.txtTopicProductMaxNum.Text;
			xmlNode.SelectSingleNode("root/CategoryProductMaxNum").InnerText = this.txtCategoryProductMaxNum.Text;
			xmlNode.SelectSingleNode("root/MarketPrice").InnerText = this.txtMarketPrice.Text;
			xmlNode.SelectSingleNode("root/SalePrice").InnerText = this.txtSalePrice.Text;
			xmlNode.SelectSingleNode("root/Phone").InnerText = this.txtPhone.Text;
			xmlNode.SelectSingleNode("root/Navigate").InnerText = this.txtNavigate.Text;
			xmlNode.Save(this.path);
			HiCache.Remove("TemplateFileCache");
			base.Response.Redirect("ManageVthemes.aspx");
		}
		private System.Xml.XmlDocument GetXmlNode()
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(this.path);
			return xmlDocument;
		}
	}
}
