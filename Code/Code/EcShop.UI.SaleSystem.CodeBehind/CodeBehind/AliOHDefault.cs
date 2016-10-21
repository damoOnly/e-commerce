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
	public class AliOHDefault : AliOHTemplatedWebControl
	{
		private AliOHTemplatedRepeater rptSlide;
		private AliOHTemplatedRepeater rptNavigate;
		private AliOHTemplatedRepeater rptTopic;
		private AliOHTemplatedRepeater rptProducts;
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
			this.rptSlide = (AliOHTemplatedRepeater)this.FindControl("rptSlide");
			this.rptNavigate = (AliOHTemplatedRepeater)this.FindControl("rptNavigate");
			this.rptTopic = (AliOHTemplatedRepeater)this.FindControl("rptTopics");
			this.rptProducts = (AliOHTemplatedRepeater)this.FindControl("rptProducts");
			this.img = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgDefaultBg");
			if (this.rptSlide != null)
			{
				this.rptSlide.DataSource = VshopBrowser.GetAllBanners(ClientType.AliOH);
				this.rptSlide.DataBind();
			}
			if (this.rptProducts != null)
			{
				DataTable homeProduct = ProductBrowser.GetHomeProduct(ClientType.AliOH);
				this.rptProducts.DataSource = homeProduct;
				this.rptProducts.DataBind();
			}
			if (this.rptTopic != null)
			{
				DataTable topics = VshopBrowser.GetTopics(ClientType.AliOH);
				this.rptTopic.DataSource = topics;
				this.rptTopic.DataBind();
			}
			if (this.rptNavigate != null)
			{
				System.Collections.Generic.IList<NavigateInfo> allNavigate = VshopBrowser.GetAllNavigate(ClientType.AliOH);
				foreach (NavigateInfo current in allNavigate)
				{
					if (!current.ImageUrl.ToLower().Contains("storage/master/navigate") && !current.ImageUrl.ToLower().Contains("templates"))
					{
						current.ImageUrl = HiContext.Current.GetAliOHshopSkinPath(null) + "/images/deskicon/" + current.ImageUrl;
					}
				}
				this.rptNavigate.DataSource = allNavigate;
				this.rptNavigate.DataBind();
			}
			if (this.img != null)
			{
				this.img.Src = new AliohTemplateHelper().GetDefaultBg();
			}
		}
	}
}
