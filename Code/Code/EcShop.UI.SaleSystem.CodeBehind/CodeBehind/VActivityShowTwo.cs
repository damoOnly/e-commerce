using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VActivityShowTwo : VshopTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "active/skin-vactivity-two.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            //PageTitle.AddSiteNameTitle("家居清洁分会场");
            if (!string.IsNullOrWhiteSpace(TitilName))
            {
                PageTitle.AddSiteNameTitle(TitilName);
            }
        }
    }
}


