using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPApp : WAPTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WAPApp.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {          

            PageTitle.AddSiteNameTitle("下载APP");
            WAPHeadName.AddHeadName("下载APP");
            if (!this.Page.IsPostBack)
            {



            }
        }
    }
}
