using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Desig_Templete : HtmlTemplatedWebControl
	{
		private const string templetestr = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:EcShop_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:EcShop_designer.moveUp()\" class=\"up updisable\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:EcShop_designer.moveDown()\" class=\"down downdisable\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"EcShop_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"EcShop_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><script>EcShop_designer.Design_Page_Init();</script>";
		protected string skintemp = "";
		protected string tempurl = "";
		protected string viewname = "";
		protected string skinparams = "0";
		protected override void OnInit(System.EventArgs e)
		{
			IUser contexUser = Users.GetContexUser();
			if (contexUser.UserRole != UserRole.SiteManager)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("login.aspx"), true);
			}
			this.SetDesignSkinName();
			if (this.SkinName == null || this.tempurl == "")
			{
				base.GotoResourceNotFound();
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)this.FindControl("litPageName");
			System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)this.FindControl("litTempete");
			System.Web.UI.WebControls.Literal literal3 = (System.Web.UI.WebControls.Literal)this.FindControl("litaccount");
			System.Web.UI.WebControls.Literal literal4 = (System.Web.UI.WebControls.Literal)this.FindControl("litview");
			System.Web.UI.WebControls.Literal literal5 = (System.Web.UI.WebControls.Literal)this.FindControl("litDefault");
			if (!this.Page.IsPostBack)
			{
				if (literal != null)
				{
					literal.Text = string.Concat(new string[]
					{
						"<script>EcShop_designer.CurrentPageName='",
						this.skintemp,
						"';EcShop_designer.CurrentParams=",
						this.skinparams,
						"</script>"
					});
				}
				if (literal2 != null)
				{
					literal2.Text = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:EcShop_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:EcShop_designer.moveUp()\" class=\"up updisable\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:EcShop_designer.moveDown()\" class=\"down downdisable\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"EcShop_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"EcShop_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><script>EcShop_designer.Design_Page_Init();</script>";
				}
				if (literal3 != null)
				{
					IUser contexUser = Users.GetContexUser();
					if (contexUser != null)
					{
						literal3.Text = "<a>我的账号：" + contexUser.Username + "</a>";
					}
				}
				if (literal5 != null)
				{
					literal5.Text = "<a href=\"" + Globals.ApplicationPath + "/\">查看店铺</a>";
				}
				if (literal4 != null)
				{
					string str = Globals.ApplicationPath + "/";
					if (this.viewname != "")
					{
						str = Globals.GetSiteUrls().UrlData.FormatUrl(this.viewname);
					}
					literal4.Text = "<a href=\"" + str + "\" target=\"_blank\" class=\"button\">预览</a>";
				}
			}
		}
		protected void SetDesignSkinName()
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["skintemp"]))
			{
				base.GotoResourceNotFound();
			}
			this.skintemp = this.Page.Request.QueryString["skintemp"];
			string key;
			switch (key = this.skintemp)
			{
			case "default":
				this.SkinName = "Skin-Desig_Templete.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Default.html");
				return;
			case "login":
				this.SkinName = "Skin-Desig_login.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Login.html");
				return;
			case "brand":
				this.SkinName = "Skin-Desig_Brand.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Brand.html");
				return;
			case "branddetail":
				this.SkinName = "Skin-Desig_BrandDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-BrandDetails.html");
				return;
			case "product":
				this.SkinName = "Skin-Desig_SubCategory.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-SubCategory.html");
				return;
			case "productdetail":
				this.SkinName = "Skin-Desig_ProductDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-ProductDetails.html");
				return;
			case "article":
				this.SkinName = "Skin-Desig_Articles.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Articles.html");
				return;
			case "articledetail":
				this.SkinName = "Skin-Desig_ArticleDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-ArticleDetails.html");
				return;
			case "cuountdown":
				this.SkinName = "Skin-Desig_CountDownProducts.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-CountDownProducts.html");
				return;
			case "cuountdowndetail":
				this.SkinName = "Skin-Desig_CountDownProductsDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-CountDownProductsDetails.html");
				return;
			case "groupbuy":
				this.SkinName = "Skin-Desig_GroupBuyProducts.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-GroupBuyProducts.html");
				return;
			case "groupbuydetail":
				this.SkinName = "Skin-Desig_GroupBuyProductDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-GroupBuyProductDetails.html");
				return;
			case "help":
				this.SkinName = "Skin-Desig_Helps.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Helps.html");
				return;
			case "helpdetail":
				this.SkinName = "Skin-Desig_HelpDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-HelpDetails.html");
				return;
			case "gift":
				this.SkinName = "Skin-Desig_OnlineGifts.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-OnlineGifts.html");
				return;
			case "giftdetail":
				this.SkinName = "Skin-Desig_GiftDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-GiftDetails.html");
				return;
			case "shopcart":
				this.SkinName = "Skin-Desig_ShoppingCart.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-ShoppingCart.html");
				return;
			case "categorycustom":
			{
				int categoryId = 0;
				int.TryParse(this.Page.Request.QueryString["cid"], out categoryId);
				CategoryInfo category = CategoryBrowser.GetCategory(categoryId);
				this.skinparams = categoryId.ToString();
				this.SkinName = "Skin-Desig_Custom.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/categorythemes/" + category.Theme);
				return;
			}
			case "brandcustom":
			{
				this.SkinName = "Skin-Desig_Custom.html";
				int brandId = 0;
				int.TryParse(this.Page.Request.QueryString["brandId"], out brandId);
				this.skinparams = brandId.ToString();
				BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(brandId);
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/brandcategorythemes/" + brandCategory.Theme);
				return;
			}
			case "customthemes":
			{
				this.SkinName = "Skin-Desig_Custom.html";
				int tid = 0;
				int.TryParse(this.Page.Request.QueryString["tid"], out tid);
				this.skinparams = tid.ToString();
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/customthemes/" + this.GetCustomSkinName(tid));
				return;
			}
			}
			this.SkinName = null;
		}
		private string GetCustomSkinName(int tid)
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/default.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			return xmlDocument.SelectSingleNode("//CustomTheme/Theme[@Tid=" + tid + "]").Attributes["SkinName"].Value;
		}
	}
}
