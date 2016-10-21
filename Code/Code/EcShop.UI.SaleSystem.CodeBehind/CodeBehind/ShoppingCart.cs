using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class ShoppingCart : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtSKU;
		private System.Web.UI.WebControls.Button btnSKU;
		private ImageLinkButton btnClearCart;
		private Common_ShoppingCart_ProductList shoppingCartProductList;
		private Common_ShoppingCart_GiftList shoppingCartGiftList;
		private Common_ShoppingCart_PromoGiftList shoppingCartPromoGiftList;
		private FormatedMoneyLabel lblTotalPrice;
		private System.Web.UI.WebControls.Literal lblAmoutPrice;
        private System.Web.UI.WebControls.Literal lblTax;
		private System.Web.UI.WebControls.HyperLink hlkReducedPromotion;
		private System.Web.UI.WebControls.Button btnCheckout;
		private System.Web.UI.WebControls.Panel pnlShopCart;
		private System.Web.UI.WebControls.Panel pnlPromoGift;
		private System.Web.UI.WebControls.Panel pnlNoProduct;
		private System.Web.UI.WebControls.HiddenField hfdIsLogin;
        private System.Web.UI.WebControls.Literal lblReducedActivity;
        ShoppingCartInfo shoppingCart = null;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShoppingCart.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtSKU = (System.Web.UI.WebControls.TextBox)this.FindControl("txtSKU");
			this.btnSKU = (System.Web.UI.WebControls.Button)this.FindControl("btnSKU");
			this.btnClearCart = (ImageLinkButton)this.FindControl("btnClearCart");
			this.shoppingCartProductList = (Common_ShoppingCart_ProductList)this.FindControl("Common_ShoppingCart_ProductList");
			this.shoppingCartGiftList = (Common_ShoppingCart_GiftList)this.FindControl("Common_ShoppingCart_GiftList");
			this.shoppingCartPromoGiftList = (Common_ShoppingCart_PromoGiftList)this.FindControl("Common_ShoppingCart_PromoGiftList");
			this.lblTotalPrice = (FormatedMoneyLabel)this.FindControl("lblTotalPrice");
            this.lblAmoutPrice = (System.Web.UI.WebControls.Literal)this.FindControl("lblAmoutPrice");
            this.lblTax = (System.Web.UI.WebControls.Literal)this.FindControl("lblTax");
            this.lblReducedActivity = (System.Web.UI.WebControls.Literal)this.FindControl("lblReducedActivity");
			this.hlkReducedPromotion = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlkReducedPromotion");
			this.btnCheckout = (System.Web.UI.WebControls.Button)this.FindControl("btnCheckout");
			this.pnlShopCart = (System.Web.UI.WebControls.Panel)this.FindControl("pnlShopCart");
			this.pnlPromoGift = (System.Web.UI.WebControls.Panel)this.FindControl("pnlPromoGift");
			this.pnlNoProduct = (System.Web.UI.WebControls.Panel)this.FindControl("pnlNoProduct");
			this.hfdIsLogin = (System.Web.UI.WebControls.HiddenField)this.FindControl("hfdIsLogin");
			this.btnSKU.Click += new System.EventHandler(this.btnSKU_Click);
			this.btnClearCart.Click += new System.EventHandler(this.btnClearCart_Click);
			this.shoppingCartProductList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.shoppingCartProductList_ItemCommand);
            this.shoppingCartProductList.ItemDataBound += shoppingCartProductList_ItemDataBound;
			this.shoppingCartGiftList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.shoppingCartGiftList_ItemCommand);
			this.shoppingCartGiftList.FreeItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.shoppingCartGiftList_FreeItemCommand);
			this.shoppingCartPromoGiftList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.shoppingCartPromoGiftList_ItemCommand);
			this.btnCheckout.Click += new System.EventHandler(this.btnCheckout_Click);
            shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			if (!HiContext.Current.SiteSettings.IsOpenSiteSale)
			{
				this.btnSKU.Visible = false;
				this.btnCheckout.Visible = false;
			}
			if (!HiContext.Current.User.IsAnonymous)
			{
				this.hfdIsLogin.Value = "logined";
			}
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            this.BindShoppingCart();
            sw.Stop();
            ErrorLog.Write("PC端购物车加载所用的时间（毫秒）:" + sw.ElapsedMilliseconds.ToString());
		}



        void shoppingCartProductList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden hidsupplierId = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("hidsupplierId");
                System.Web.UI.WebControls.DataList productlist = (System.Web.UI.WebControls.DataList)e.Item.FindControl("rpProduct");
                productlist.ItemCommand += productlist_ItemCommand;
                productlist.ItemDataBound += productlist_ItemDataBound;
                if (productlist != null)
                {
                    if (shoppingCart != null)
                    {
                        List<ShoppingCartItemInfo> list = (List<ShoppingCartItemInfo>)shoppingCart.LineItems;
                        var llist = list.Where(p => p.SupplierId == Convert.ToInt32(hidsupplierId.Value)).Select(c => c);
                        productlist.DataSource = llist;
                        productlist.DataBind();
                        
                    }
                    
                }

            }
        }

        void productlist_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlInputHidden promotionProductId = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("promotionProductId");
            System.Web.UI.WebControls.DataList dtPresendPro = (System.Web.UI.WebControls.DataList)e.Item.FindControl("dtPresendPro");
            if (shoppingCart != null)
            {
                // 赠送活动
                List<ShoppingCartPresentInfo> presentList = (List<ShoppingCartPresentInfo>)shoppingCart.LinePresentPro;
                if (presentList != null && presentList.Count > 0)
                {
                    var p = presentList.Where(m => m.PromotionProductId == (string.IsNullOrWhiteSpace(promotionProductId.Value) ? 0 : Convert.ToInt32(promotionProductId.Value))).Select(n => n);
                    dtPresendPro.DataSource = p;
                    dtPresendPro.DataBind();
                }

            }
        }

        void productlist_ItemCommand(object source, DataListCommandEventArgs e)
        {
            System.Web.UI.Control control = e.Item.Controls[0];
            System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)control.FindControl("txtBuyNum");
            System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)control.FindControl("litSkuId");
            int num;
            if (!int.TryParse(textBox.Text, out num) || textBox.Text.IndexOf(".") != -1)
            {
                this.ShowMessage("购买数量必须为整数", false);
                return;
            }
            if (num <= 0)
            {
                this.ShowMessage("购买数量必须为大于0的整数", false);
                return;
            }
            if (e.CommandName == "updateBuyNum")
            {
                if (ShoppingCartProcessor.GetSkuStock(literal.Text.Trim()) < num)
                {
                    this.ShowMessage("该商品库存不够", false);
                    this.btnCheckout.Visible = false;
                    return;
                }
                ShoppingCartProcessor.UpdateLineItemQuantity(literal.Text, num);
            }
            if (e.CommandName == "delete")
            {
                ShoppingCartProcessor.RemoveLineItem(literal.Text);
            }
            this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart") + "?productSkuId=" + literal.Text, true);
        }
		protected void btnSKU_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtSKU.Text.Trim()))
			{
				this.ShowMessage("请输入货号", false);
				return;
			}
			System.Collections.Generic.IList<string> skuIdsBysku = ShoppingProcessor.GetSkuIdsBysku(this.txtSKU.Text.Trim());
			if (skuIdsBysku == null || skuIdsBysku.Count == 0)
			{
				this.ShowMessage("货号无效，请确认后重试", false);
				return;
			}
			foreach (string current in skuIdsBysku)
			{
				ShoppingCartProcessor.AddLineItem(current, 1,0);
			}
			this.BindShoppingCart();
		}
		protected void btnClearCart_Click(object sender, System.EventArgs e)
		{
			string text = this.Page.Request.Form["ck_productId"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMessage("请选择要清除的商品", false);
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string skuId = array[i];
				ShoppingCartProcessor.RemoveLineItem(skuId);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
		}
		protected void shoppingCartProductList_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			System.Web.UI.Control control = e.Item.Controls[0];
			System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)control.FindControl("txtBuyNum");
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)control.FindControl("litSkuId");
			int num;
			if (!int.TryParse(textBox.Text, out num) || textBox.Text.IndexOf(".") != -1)
			{
				this.ShowMessage("购买数量必须为整数", false);
				return;
			}
			if (num <= 0)
			{
				this.ShowMessage("购买数量必须为大于0的整数", false);
				return;
			}
			if (e.CommandName == "updateBuyNum")
			{
				if (ShoppingCartProcessor.GetSkuStock(literal.Text.Trim()) < num)
				{
					this.ShowMessage("该商品库存不够", false);
                    this.btnCheckout.Visible = false;
					return;
				}
				ShoppingCartProcessor.UpdateLineItemQuantity(literal.Text, num);
			}
			if (e.CommandName == "delete")
			{
				ShoppingCartProcessor.RemoveLineItem(literal.Text);
			}
            this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart") + "?productSkuId=" + literal.Text, true);
		}
		protected void shoppingCartPromoGiftList_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName.Equals("change"))
			{
				int num = System.Convert.ToInt32(e.CommandArgument.ToString());
				if (num > 0)
				{
					ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
					if (shoppingCart != null && shoppingCart.LineGifts != null && shoppingCart.LineGifts.Count > 0)
					{
						foreach (ShoppingCartGiftInfo current in shoppingCart.LineGifts)
						{
							if (current.GiftId == num)
							{
								this.ShowMessage("购物车中已存在该礼品，请删除购物车中已有的礼品或者下次兑换！", false);
								return;
							}
						}
					}
					int giftItemQuantity = ShoppingCartProcessor.GetGiftItemQuantity(PromoteType.SentGift);
					if (this.shoppingCartPromoGiftList.SumNum > giftItemQuantity)
					{
						ShoppingCartProcessor.AddGiftItem(num, 1, PromoteType.SentGift);
						this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
						return;
					}
					this.ShowMessage("礼品兑换失败，您不能超过最多兑换数", false);
				}
			}
		}
		protected void shoppingCartGiftList_FreeItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (e.CommandName == "delete")
			{
				string text = e.CommandArgument.ToString();
				int giftId = 0;
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out giftId))
				{
					ShoppingCartProcessor.RemoveGiftItem(giftId, PromoteType.SentGift);
				}
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart") + "?productSkuId=" + text, true);
			}
		}
		protected void shoppingCartGiftList_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			System.Web.UI.Control control = e.Item.Controls[0];
			System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)control.FindControl("txtBuyNum");
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)control.FindControl("litGiftId");
			int num;
			if (!int.TryParse(textBox.Text, out num) || textBox.Text.IndexOf(".") != -1)
			{
				this.ShowMessage("兑换数量必须为整数", false);
				return;
			}
			if (num <= 0)
			{
				this.ShowMessage("兑换数量必须为大于0的整数", false);
				return;
			}
			if (e.CommandName == "updateBuyNum")
			{
				ShoppingCartProcessor.UpdateGiftItemQuantity(System.Convert.ToInt32(literal.Text), num, PromoteType.NotSet);
			}
			if (e.CommandName == "delete")
			{
				ShoppingCartProcessor.RemoveGiftItem(System.Convert.ToInt32(literal.Text), PromoteType.NotSet);
			}
            this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart")  +"?productSkuId=" + literal.Text, true);
		}
		protected void btnCheckout_Click(object sender, System.EventArgs e)
		{
            if (!(HiContext.Current.User is Member))
            {
                this.ShowMessage("请登录", false);
                return;
            }
			HiContext.Current.Context.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
		}
		private void BindShoppingCart()
		{
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			if (shoppingCart == null)
			{
				this.pnlShopCart.Visible = false;
				this.pnlNoProduct.Visible = true;
				ShoppingCartProcessor.ClearShoppingCart();
				return;
			}
			this.pnlShopCart.Visible = true;
			this.pnlNoProduct.Visible = false;
			if (shoppingCart.LineItems.Count > 0)
			{
                decimal tax = shoppingCart.CalTotalTax();
                if (tax <= 50)
                {
                    this.lblTax.Text = string.Format("商品关税：<span style='text-decoration: line-through;'>{0}</span>", tax.ToString("F2"));
                }
                else
                {
                    this.lblTax.Text = string.Format("商品关税：{0}", tax.ToString("F2"));
                }
                //this.lblTax.Text = string.Format("商品关税：{0}", tax <50 ? "0.00" : tax.ToString("F2"));
                //foreach (ShoppingCartItemInfo item in shoppingCart.LineItems)
                //{
                //    item.TaxRate = Math.Round(item.TaxRate * 100, 0);                 
                //}

                decimal activityReduct = shoppingCart.GetActivityPrice();
                this.lblReducedActivity.Text = string.Format("活动优惠：{0}", activityReduct == 0 ? "0.00" : activityReduct.ToString("F2"));

                this.lblTotalPrice.Money = shoppingCart.GetNewTotalIncludeTax();

                var query = from q in shoppingCart.LineItems
                            group q by new { id = q.SupplierId }
                                into g
                                select new
                                {
                                    SupplierId = g.FirstOrDefault().SupplierId,
                                    SupplierName = g.FirstOrDefault().SupplierName,
                                    Logo = g.FirstOrDefault().Logo,
                                    ProductId = g.FirstOrDefault().ProductId,
                                    SkuId = g.FirstOrDefault().SkuId,
                                    Name = g.FirstOrDefault().Name,
                                    ThumbnailUrl60 = g.FirstOrDefault().ThumbnailUrl60,
                                    TaxRate = g.FirstOrDefault().TaxRate,
                                    MemberPrice = g.FirstOrDefault().MemberPrice,
                                    Quantity = g.FirstOrDefault().Quantity,
                                    SKU = g.FirstOrDefault().SKU,
                                    ShippQuantity = g.FirstOrDefault().ShippQuantity,
                                    PromotionId = g.FirstOrDefault().PromotionId,
                                    PromotionName = g.FirstOrDefault().PromotionName,
                                    SubNewTotal = g.FirstOrDefault().SubNewTotal,
                                    NewTaxRate = GetNewTaxRate(g.FirstOrDefault().TaxRate, g.FirstOrDefault().MinTaxRate, g.FirstOrDefault().MaxTaxRate)
                                    //itemInfo  = g.FirstOrDefault()
                                };
                this.shoppingCartProductList.DataSource = query;
				this.shoppingCartProductList.DataBind();
				this.shoppingCartProductList.ShowProductCart();
			}
			if (shoppingCart.LineGifts.Count > 0)
			{
				System.Collections.Generic.IEnumerable<ShoppingCartGiftInfo> enumerable = 
					from s in shoppingCart.LineGifts
					where s.PromoType == 0
					select s;
				System.Collections.Generic.IEnumerable<ShoppingCartGiftInfo> enumerable2 = 
					from s in shoppingCart.LineGifts
					where s.PromoType == 5
					select s;
				this.shoppingCartGiftList.DataSource = enumerable;
				this.shoppingCartGiftList.FreeDataSource = enumerable2;
				this.shoppingCartGiftList.DataBind();
				this.shoppingCartGiftList.ShowGiftCart(enumerable.Count<ShoppingCartGiftInfo>() > 0, enumerable2.Count<ShoppingCartGiftInfo>() > 0);
			}
			if (shoppingCart.IsSendGift)
			{
				int num = 1;
				int giftItemQuantity = ShoppingCartProcessor.GetGiftItemQuantity(PromoteType.SentGift);
				System.Collections.Generic.IList<GiftInfo> onlinePromotionGifts = ProductBrowser.GetOnlinePromotionGifts();
				if (onlinePromotionGifts != null && onlinePromotionGifts.Count > 0)
				{
					this.shoppingCartPromoGiftList.DataSource = onlinePromotionGifts;
					this.shoppingCartPromoGiftList.DataBind();
					this.shoppingCartPromoGiftList.ShowPromoGift(num - giftItemQuantity, num);
				}
			}

            //if (shoppingCart.IsReduced)
            //{
                this.lblAmoutPrice.Text = string.Format("商品金额：{0}", shoppingCart.GetNewAmount().ToString("F2"));
				this.hlkReducedPromotion.Text = shoppingCart.ReducedPromotionName + string.Format(" 优惠：{0}", shoppingCart.ReducedPromotionAmount.ToString("F2"));
                if (shoppingCart.ReducedPromotionId != 0)
                {
                    this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					shoppingCart.ReducedPromotionId
				});
                }
            //}

            HttpCookie cookieSkuIds = this.Page.Request.Cookies["UserSession-SkuIds"];
            if (cookieSkuIds != null)
            {
                //cookieSkuIds.Expires = DateTime.Now.AddDays(-1);
                this.Page.Response.AppendCookie(cookieSkuIds);
            }
		}

        private string GetNewTaxRate(decimal taxRate,decimal minTaxRate,decimal maxTaxRate)
        {
            // 组合商品
            var currTaxRate = (minTaxRate * 100).ToString("0") + "-" + (maxTaxRate * 100).ToString("0");
            if (minTaxRate > 0)
            {
                return minTaxRate == maxTaxRate ? (minTaxRate * 100).ToString("0") : currTaxRate;
            }
            else if (minTaxRate == 0 && maxTaxRate > 0)
            {
                return currTaxRate;
            }
            else
            {
                return (taxRate * 100).ToString("0");
            }
        }
	}
}
