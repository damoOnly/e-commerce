using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class AboutHaiMeiWang : WAPTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WAboutHaiMeiWang.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("关于海美生活");

            WAPHeadName.AddHeadName("关于海美生活");
        }
    }
}
