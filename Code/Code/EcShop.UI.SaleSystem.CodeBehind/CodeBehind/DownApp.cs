﻿using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class DownApp : HtmlTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-twoDimension.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
            {
                string text = System.Web.HttpContext.Current.Request.Url.ToString();
                if (text.IndexOf("?") > -1)
                {
                    text = text + "&ReferralUserId=" + HiContext.Current.User.UserId;
                }
                else
                {
                    text = text + "?ReferralUserId=" + HiContext.Current.User.UserId;
                }
                this.Page.Response.Redirect(text);
                return;
            }
            HiContext current = HiContext.Current;
            PageTitle.AddTitle(current.SiteSettings.SiteName + " - " + current.SiteSettings.SiteDescription, HiContext.Current.Context);
        }
    }
}
