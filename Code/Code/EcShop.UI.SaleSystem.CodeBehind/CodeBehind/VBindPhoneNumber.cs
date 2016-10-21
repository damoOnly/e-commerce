using Ecdev.Weixin.MP.Api;
using Ecdev.Weixin.MP.Domain;
using EcShop.ControlPanel.Promotions;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VBindPhoneNumber : VMemberTemplatedWebControl
    {
        private System.Web.UI.WebControls.Literal litAD;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VBindPhoneNumber.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.litAD = (System.Web.UI.WebControls.Literal)this.FindControl("litAD");
            IList<CouponInfo> couponList = CouponHelper.GetCouponsBySendType(5);
            if (couponList != null && couponList.Count > 0)
            {
                litAD.Text = "True";
            }
            PageTitle.AddSiteNameTitle("绑定手机号码");
        }
    }
}

