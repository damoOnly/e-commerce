using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.Core.Entities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VMySupFavorites : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptSupplier;
        private System.Web.UI.WebControls.Literal litProFavCount;
        private System.Web.UI.WebControls.Literal litSuppFavCount;
        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMySupFavorites.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            string url = this.Page.Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
            {
                this.Page.Response.Redirect(url);
            }
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.Page.Response.Redirect("/Vshop/Login.aspx");
            }

            this.rptSupplier = (VshopTemplatedRepeater)this.FindControl("rptSupplier");
            this.litProFavCount = (System.Web.UI.WebControls.Literal)this.FindControl("litProFavCount");
            this.litSuppFavCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSuppFavCount");
            this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");

            int pageIndex;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
            {
                pageIndex = 1;
            }
            int pageSize;
            if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
            {
                pageSize = 10;
            }


            SupplierCollectQuery query = new SupplierCollectQuery();
            query.UserId = member.UserId;
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;

            DbQueryResult dbResult = SupplierHelper.GetSupplierCollect(query);

            this.rptSupplier.DataSource = dbResult.Data;
            this.rptSupplier.DataBind();

            this.litProFavCount.Text = ProductBrowser.GetUserFavoriteCount(member.UserId).ToString();
            this.litSuppFavCount.Text = dbResult.TotalRecords.ToString();
            this.txtTotal.SetWhenIsNotNull(dbResult.TotalRecords.ToString());

            PageTitle.AddSiteNameTitle("我的收藏");
        }
    }
}
