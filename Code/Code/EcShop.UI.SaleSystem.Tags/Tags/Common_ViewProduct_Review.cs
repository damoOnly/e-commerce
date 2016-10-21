using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_ViewProduct_Review : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(base.Text))
			{
				base.Text = "我要评论";
			}
			if (HiContext.Current.User.UserRole != UserRole.Member)
			{
                base.Text = "请先登录";
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("login", new object[]
                {
                    this.Page.Request.RawUrl
                });

                //隐藏按钮
                base.Style.Add("display", "none");
			}
			else
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productId"]) && Convert.ToInt32(this.Page.Request.QueryString["productId"]) > 0)
				{
					if (ProductBrowser.IsBuyProduct(Convert.ToInt32(this.Page.Request.QueryString["productId"])))
					{
						base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productReviews", new object[]
						{
							this.Page.Request.QueryString["productId"]
						});
					}
					else
					{
						base.Text = "请先购买";
						base.Attributes["style"] = "color: #0246A1;cursor: default;text-decoration: none;";

                        //隐藏按钮
                        base.Style.Add("display", "none");

					}
				}
			}
			base.Render(writer);
		}
	}
}
