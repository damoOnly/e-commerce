using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using Entities;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VSupplierList : VMemberTemplatedWebControl
    {

        private VshopTemplatedRepeater rptSupplierList;
        private VshopTemplatedRepeater rpthistorysearch;

        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VSupplierList.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺列表");
            this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
            this.rptSupplierList = (VshopTemplatedRepeater)this.FindControl("rptSupplierList");
            this.rpthistorysearch = (VshopTemplatedRepeater)this.FindControl("rpthistorysearch");

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

            string supplierName = this.Page.Request.QueryString["keyWord"];

            SupplierQuery query = new SupplierQuery();
            query.SupplierName = supplierName;
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            query.UserId = HiContext.Current.User.UserId;

            DbQueryResult dt = SupplierHelper.GetAppSupplier(query);
            this.rptSupplierList.DataSource = dt.Data;
            this.rptSupplierList.DataBind();
            this.txtTotal.SetWhenIsNotNull(dt.TotalRecords.ToString());

            int userId = HiContext.Current.User.UserId;
            if (userId > 0 && this.rpthistorysearch != null)
            {
                this.rpthistorysearch.DataSource = HistorySearchHelp.GetSearchHistory(userId, ClientType.VShop, 6);
                this.rpthistorysearch.DataBind();
            }
        }
    }
}
