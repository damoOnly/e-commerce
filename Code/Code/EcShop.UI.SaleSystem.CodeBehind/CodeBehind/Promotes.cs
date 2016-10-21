using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Promotes : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptPromoteSales;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Promotes.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptPromoteSales = (ThemedTemplatedRepeater)this.FindControl("rptPromoteSales");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("优惠活动中心");
				this.BindPromoteSales();
			}
		}
		private void BindPromoteSales()
		{
			int promotiontype = 0;
			int num;
			if (int.TryParse(this.Page.Request.QueryString["promoteType"], out num))
			{
				promotiontype = num;
			}
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = this.pager.PageSize;
			int totalRecords = 0;
			DataTable promotes = CommentBrowser.GetPromotes(pagination, promotiontype, out totalRecords);
			if (promotes != null && promotes.Rows.Count > 0)
			{
				this.rptPromoteSales.DataSource = promotes;
				this.rptPromoteSales.DataBind();
			}
			this.pager.TotalRecords = totalRecords;
		}
	}
}
