using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPDownloadApp : System.Web.UI.Page
    {
        protected override void OnLoad(System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string userAgent = HttpContext.Current.Request.QueryString["t"] != null ? HttpContext.Current.Request.QueryString["t"].ToLower() : "";
                   
                if (string.IsNullOrEmpty(userAgent))
                {
                    userAgent = HttpContext.Current.Request.UserAgent.ToLower();
                }
                string url = "";
                if (userAgent.IndexOf("ios") != -1)
                {
                    AppVersionRecordInfo appVersion = APPHelper.GetLatestAppVersionRecord("ios");
                    if (appVersion != null)
                    {
                        url = appVersion.UpgradeUrl;
                    }
                }
                else if (userAgent.IndexOf("android") != -1)
                {
                    AppVersionRecordInfo appVersion = APPHelper.GetLatestAppVersionRecord("android");
                    if (appVersion != null)
                    {
                        url = appVersion.UpgradeUrl;
                    }
                }
                else
                {
                    AppVersionRecordInfo appVersion = APPHelper.GetLatestAppVersionRecord("android");
                    if (appVersion != null)
                    {
                        url = appVersion.UpgradeUrl;
                    }
                }

                if (!string.IsNullOrEmpty(url))
                {
                    HttpContext.Current.Response.Redirect(url);
                }
            }
        }
    }
}
