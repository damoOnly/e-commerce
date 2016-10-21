using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Promotions;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class OnlineGifts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptGifts;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-OnlineGifts.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptGifts = (ThemedTemplatedRepeater)this.FindControl("rptGifts");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				this.BindGift();
			}
		}
		private void BindGift()
		{
			DbQueryResult onlineGifts = ProductBrowser.GetOnlineGifts(new GiftQuery
			{
				Page = 
				{
					PageIndex = this.pager.PageIndex,
					PageSize = this.pager.PageSize
				}
			});
			this.rptGifts.DataSource = onlineGifts.Data;
			this.rptGifts.DataBind();
			this.pager.TotalRecords = onlineGifts.TotalRecords;
		}
	}
}
