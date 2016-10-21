using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPHotSearch : WAPTemplatedWebControl
    {
        public System.Web.UI.WebControls.Literal litSitesList;
        public WapTemplatedRepeater rpthistorysearch;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WHotSearch.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.litSitesList = (System.Web.UI.WebControls.Literal)this.FindControl("litSitesList");
            this.rpthistorysearch = (WapTemplatedRepeater)this.FindControl("rpthistorysearch");
            this.litSitesList.Text = RegisterSitesScript();
             PageTitle.AddSiteNameTitle("热门搜索");
             int userId = HiContext.Current.User.UserId;
             if (userId > 0)
             {
                 this.rpthistorysearch.DataSource = HistorySearchHelp.GetSearchHistory(userId, ClientType.WAP, 6);
                 this.rpthistorysearch.DataBind();
             }

        }
    }
}
