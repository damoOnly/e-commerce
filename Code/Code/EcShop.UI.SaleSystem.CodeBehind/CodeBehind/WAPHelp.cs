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
    public class WAPHelp:WAPTemplatedWebControl
    {
        WapTemplatedRepeater helpCategories;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WAPHelp.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.helpCategories = (WapTemplatedRepeater)this.FindControl("helpCategories");

            this.helpCategories.DataSource = CommentBrowser.GetHelpCategories();
            this.helpCategories.DataBind();

            PageTitle.AddSiteNameTitle("帮助中心");
            WAPHeadName.AddHeadName("帮助中心");
        }
    }
}
