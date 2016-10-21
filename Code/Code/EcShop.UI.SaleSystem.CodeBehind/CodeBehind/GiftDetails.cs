using EcShop.Core;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class GiftDetails : HtmlTemplatedWebControl
	{
		private int giftId;
		private System.Web.UI.WebControls.Literal litGiftTite;
		private System.Web.UI.WebControls.Literal litGiftName;
		private FormatedMoneyLabel lblMarkerPrice;
		private System.Web.UI.WebControls.Label litNeedPoint;
		private System.Web.UI.WebControls.Label litCurrentPoint;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private System.Web.UI.WebControls.Literal litDescription;
		private HiImage imgGiftImage;
		private System.Web.UI.WebControls.Button btnChage;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-GiftDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["giftId"], out this.giftId))
			{
				base.GotoResourceNotFound();
			}
			this.litGiftTite = (System.Web.UI.WebControls.Literal)this.FindControl("litGiftTite");
			this.litGiftName = (System.Web.UI.WebControls.Literal)this.FindControl("litGiftName");
			this.lblMarkerPrice = (FormatedMoneyLabel)this.FindControl("lblMarkerPrice");
			this.litNeedPoint = (System.Web.UI.WebControls.Label)this.FindControl("litNeedPoint");
			this.litCurrentPoint = (System.Web.UI.WebControls.Label)this.FindControl("litCurrentPoint");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.imgGiftImage = (HiImage)this.FindControl("imgGiftImage");
			this.btnChage = (System.Web.UI.WebControls.Button)this.FindControl("btnChage");
			this.btnChage.Click += new System.EventHandler(this.btnChage_Click);
			GiftInfo gift = ProductBrowser.GetGift(this.giftId);
			if (gift == null)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件礼品已经不再参与积分兑换；或被管理员删除"));
				return;
			}
			if (!this.Page.IsPostBack)
			{
				this.litGiftName.Text = gift.Name;
				this.lblMarkerPrice.Money = gift.MarketPrice;
				this.litNeedPoint.Text = gift.NeedPoint.ToString();
				this.litShortDescription.Text = gift.ShortDescription;
				this.litDescription.Text = gift.LongDescription;
				this.imgGiftImage.ImageUrl = gift.ThumbnailUrl310;
				this.LoadPageSearch(gift);
			}
			if (HiContext.Current.User.UserRole == UserRole.Member && gift.NeedPoint > 0)
			{
				this.btnChage.Enabled = true;
				this.btnChage.Text = "立即兑换";
				this.litCurrentPoint.Text = ((Member)HiContext.Current.User).Points.ToString();
				return;
			}
			if (gift.NeedPoint <= 0)
			{
				this.btnChage.Enabled = false;
				this.btnChage.Text = "礼品不允许兑换";
				return;
			}
			this.btnChage.Text = "请登录方能兑换";
		}
		private void btnChage_Click(object sender, System.EventArgs e)
		{
			if (this.btnChage.Text == "请登录方能兑换")
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/Login.aspx");
				return;
			}
			if (HiContext.Current.User.UserRole != UserRole.Member)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("请登录后才能购买"));
				return;
			}
			if (int.Parse(this.litNeedPoint.Text) <= int.Parse(this.litCurrentPoint.Text))
			{
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
				if (shoppingCart != null && shoppingCart.LineGifts != null && shoppingCart.LineGifts.Count > 0)
				{
					foreach (ShoppingCartGiftInfo current in shoppingCart.LineGifts)
					{
						if (current.GiftId == this.giftId)
						{
							this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=购物车中已存在该礼品，请删除购物车中已有的礼品或者下次兑换！");
							return;
						}
					}
				}
				if (ShoppingCartProcessor.AddGiftItem(this.giftId, 1, PromoteType.NotSet))
				{
					this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
				}
			}
		}
		private void LoadPageSearch(GiftInfo gift)
		{
			if (!string.IsNullOrEmpty(gift.Meta_Keywords))
			{
				MetaTags.AddMetaKeywords(gift.Meta_Keywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(gift.Meta_Description))
			{
				MetaTags.AddMetaDescription(gift.Meta_Description, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(gift.Title))
			{
				PageTitle.AddSiteNameTitle(gift.Title);
				return;
			}
			PageTitle.AddSiteNameTitle(gift.Name);
		}
	}
}
