using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SupplierProductPromotions)]
    public class SupplierSetPromotionProducts : AdminPage
	{
		private int activityId;
		public bool isWholesale;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdactivy;
		protected System.Web.UI.WebControls.Literal litPromotionName;
		protected ImageLinkButton btnDeleteAll;
		protected Grid grdPromotionProducts;
		protected System.Web.UI.WebControls.LinkButton btnFinesh;
        protected System.Web.UI.WebControls.LinkButton btnCreateReport;
        private int supplierId;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdsupplierid;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
        }
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
				return;
			}
            int.TryParse(this.Page.Request.QueryString["supplierId"], out this.supplierId);
			this.hdactivy.Value = this.activityId.ToString();
            this.hdsupplierid.Value = this.supplierId.ToString();
			this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
			this.grdPromotionProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdPromotionProducts_RowDeleting);
			if (!this.Page.IsPostBack)
			{
                this.btnFinesh.PostBackUrl = "SupplierProductPromotions.aspx";
				bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
				if (this.isWholesale)
				{
                    this.btnFinesh.PostBackUrl = "SupplierProductPromotions.aspx?isWholesale=true";
				}
				PromotionInfo promotion = PromoteHelper.GetPromotion(this.activityId);
				if (promotion == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litPromotionName.Text = promotion.Name;
				this.BindPromotionProducts();
			}
		}
		private void grdPromotionProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (PromoteHelper.DeletePromotionProducts(this.activityId, new int?((int)this.grdPromotionProducts.DataKeys[e.RowIndex].Value)))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		private void BindPromotionProducts()
		{
			this.grdPromotionProducts.DataSource = PromoteHelper.GetPromotionProducts(this.activityId);
			this.grdPromotionProducts.DataBind();
		}
		private void btnDeleteAll_Click(object sender, System.EventArgs e)
		{
			if (PromoteHelper.DeletePromotionProducts(this.activityId, null))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
        /// <summary>
        /// 导出活动商品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {
            DataTable dt = PromoteHelper.GetPromotionProducts(this.activityId);
            if (dt == null || dt.Rows.Count == 0)
            {
                this.ShowMsg("该活动下无商品！", false);
                return;
            }
            List<DataColumn> removeCl = new List<DataColumn>();
            foreach (DataColumn cl in dt.Columns)
            {
                if (cl.ColumnName == "ProductId")
                {
                    cl.ColumnName = "商品编号";
                }
                else if (cl.ColumnName == "ProductName")
                {
                    cl.ColumnName = "商品名称";
                }
                else if (cl.ColumnName == "ProductCode")
                {
                    cl.ColumnName = "商家编码";
                }
                else if (cl.ColumnName == "Stock")
                {
                    cl.ColumnName = "库存";
                }
                else if (cl.ColumnName == "MarketPrice")
                {
                    cl.ColumnName = "市场价";
                }
                else if (cl.ColumnName == "SalePrice")
                {
                    cl.ColumnName = "一口价";
                }
                else if (cl.ColumnName == "SupplierId")
                {
                    cl.ColumnName = "供应商编号";
                }
                else if (cl.ColumnName == "SupplierName")
                {
                    cl.ColumnName = "供应商名称";
                }
                else
                {
                    removeCl.Add(cl);
                }
            }
            if (removeCl.Count > 0)
            {
                foreach (DataColumn cl in removeCl)
                {
                    dt.Columns.Remove(cl);
                }
            }
            MemoryStream ms = NPOIExcelHelper.ExportToExcel(dt, "对账单");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + this.activityId + "_" + this.litPromotionName.Text + "促销活动商品.xlsx");
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
            ms.Close();
            this.Page.Response.End();
        }
	}
}
