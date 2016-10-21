using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPDefault : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptSlide;
		private WapTemplatedRepeater rptNavigate;
		private WapTemplatedRepeater rptTopic;
		private WapTemplatedRepeater rptProducts;

        private WapTemplatedRepeater rptHotSale;
        private WapTemplatedRepeater rptNewPro;
        private WapTemplatedRepeater rptPromotional;
		private System.Web.UI.HtmlControls.HtmlImage img;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VDefault.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text = System.Web.HttpContext.Current.Request.Url.ToString();
				if (text.IndexOf("?") > -1)
				{
					text = text + "&ReferralUserId=" + HiContext.Current.User.UserId;
				}
				else
				{
					text = text + "?ReferralUserId=" + HiContext.Current.User.UserId;
				}
				this.Page.Response.Redirect(text);
				return;
			}
			PageTitle.AddSiteNameTitle("首页");
			this.rptSlide = (WapTemplatedRepeater)this.FindControl("rptSlide");
			this.rptNavigate = (WapTemplatedRepeater)this.FindControl("rptNavigate");
			this.rptTopic = (WapTemplatedRepeater)this.FindControl("rptTopics");
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptProducts");
			this.img = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgDefaultBg");

            this.rptHotSale = (WapTemplatedRepeater)this.FindControl("rptHotSale");
            this.rptNewPro = (WapTemplatedRepeater)this.FindControl("rptNewPro");
            this.rptPromotional = (WapTemplatedRepeater)this.FindControl("rptPromotional");
			if (this.rptSlide != null)
			{
				this.rptSlide.DataSource = VshopBrowser.GetAllBanners(ClientType.WAP);
				this.rptSlide.DataBind();
			}
			if (this.rptProducts != null)
			{
				DataTable homeProduct = ProductBrowser.GetHomeProduct(ClientType.WAP);
				this.rptProducts.DataSource = homeProduct;
				this.rptProducts.DataBind();
			}
			if (this.rptTopic != null)
			{
				DataTable topics = VshopBrowser.GetTopics(ClientType.WAP);
				this.rptTopic.DataSource = topics;
				this.rptTopic.DataBind();
			}
			if (this.rptNavigate != null)
			{
				System.Collections.Generic.IList<NavigateInfo> allNavigate = VshopBrowser.GetAllNavigate(ClientType.WAP);
				foreach (NavigateInfo current in allNavigate)
				{
					if (!current.ImageUrl.ToLower().Contains("storage/master/navigate") && !current.ImageUrl.ToLower().Contains("templates"))
					{
						current.ImageUrl = HiContext.Current.GetWapshopSkinPath(null) + "/images/deskicon/" + current.ImageUrl;
					}
				}
				this.rptNavigate.DataSource = allNavigate;
				this.rptNavigate.DataBind();
			}

            //添加
            if (this.rptHotSale != null)
            {

                this.rptHotSale.DataSource = VshopBrowser.GetAllHotSale(ClientType.WAP);
                this.rptHotSale.DataBind();
            }

            if (this.rptNewPro != null)
            {
                this.rptNewPro.DataSource = VshopBrowser.GetAllRecommend(ClientType.WAP);
                this.rptNewPro.DataBind();
            }

            if (this.rptPromotional != null)
            {
                this.rptPromotional.DataSource = VshopBrowser.GetAllPromotional(ClientType.WAP);
                this.rptPromotional.DataBind();
            }
			if (this.img != null)
			{
				this.img.Src = new WapTemplateHelper().GetDefaultBg();
			}
		}
	}
}
