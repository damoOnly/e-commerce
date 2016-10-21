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
    public class VMyFavorites : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptProducts;
        private System.Web.UI.WebControls.Literal litProFavCount;
        private System.Web.UI.WebControls.Literal litSuppFavCount;
        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyFavorites.html";
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

            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
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
                pageSize = 8;
            }

            //
            ProductFavoriteQuery query = new ProductFavoriteQuery();
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            query.UserId = member.UserId;
            query.GradeId = member.GradeId;


            DbQueryResult dr = ProductBrowser.GetFavorites(query);

            this.rptProducts.DataSource = dr.Data;
            this.rptProducts.DataBind();

            this.litProFavCount.Text = dr.TotalRecords.ToString();
            this.litSuppFavCount.Text = SupplierHelper.GetUserSupplierCollectCount(member.UserId).ToString();
            this.txtTotal.SetWhenIsNotNull(dr.TotalRecords.ToString());

            PageTitle.AddSiteNameTitle("我的收藏");
        }
    }
}
