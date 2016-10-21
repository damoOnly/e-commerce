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
    public class VHotSupplierList : VMemberTemplatedWebControl
    {

        private VshopTemplatedRepeater rptHotSupplierList;
        private VshopTemplatedRepeater rptRecSupplierList;
        private VshopTemplatedRepeater rpthistorysearch;
        private VshopTemplatedRepeater rptAllSupplierList;

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VHotSupplierList.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺列表");

            this.rptHotSupplierList = (VshopTemplatedRepeater)this.FindControl("rptHotSupplierList");
            this.rptRecSupplierList = (VshopTemplatedRepeater)this.FindControl("rptRecSupplierList");
            this.rpthistorysearch = (VshopTemplatedRepeater)this.FindControl("rpthistorysearch");
            this.rptAllSupplierList = (VshopTemplatedRepeater)this.FindControl("rptAllSupplierList");

            int referUserId;
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                referUserId = member.UserId;
            }
            else
            {
                referUserId = 0;
            }

            //热卖
            DataTable hotdt = SupplierConfigHelper.GetConfigSupplier(ClientType.App, SupplierCfgType.Hot, referUserId);
            this.rptHotSupplierList.DataSource = hotdt;
            this.rptHotSupplierList.DataBind();
            //推荐
            DataTable recdt = SupplierConfigHelper.GetConfigSupplier(ClientType.App, SupplierCfgType.Recommend, referUserId);
            this.rptRecSupplierList.DataSource = recdt;
            this.rptRecSupplierList.DataBind();
            //所有
            DataTable recdtAll = SupplierConfigHelper.GetConfigSupplier(ClientType.App,0, referUserId);
            if (recdtAll != null && recdtAll.Rows.Count > 0)
            {
                rptAllSupplierList.DataSource = recdtAll;
                rptAllSupplierList.DataBind();
            }

            int userId = HiContext.Current.User.UserId;
            if (userId > 0 && this.rpthistorysearch != null)
            {
                this.rpthistorysearch.DataSource = HistorySearchHelp.GetSearchHistory(userId, ClientType.VShop, 6);
                this.rpthistorysearch.DataBind();
            }
        }
    }
}
