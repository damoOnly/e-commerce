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
    public class VHelpList : VMemberTemplatedWebControl
    {
        VTemplatedRepeater helpItems;

        private int CategoryId;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VHelpList.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.helpItems = (VTemplatedRepeater)this.FindControl("helpItems");

            if (!int.TryParse(this.Page.Request.QueryString["CategoryId"], out this.CategoryId))
            {
                base.GotoResourceNotFound("");
            }
            else
            {
                this.helpItems.DataSource = CommentBrowser.GetHelpByCateGoryId(this.CategoryId);
                this.helpItems.DataBind();

                string name = CommentBrowser.GetHelpCategory(this.CategoryId).Name;

                PageTitle.AddSiteNameTitle(name);
                //WAPHeadName.AddHeadName(name);


            }
        }
    }
}
