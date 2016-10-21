using EcShop.Entities.Comments;
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
    public class VHelpItem : VMemberTemplatedWebControl
    {
        private System.Web.UI.WebControls.Literal helpContent;

        private int helpid;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VHelpItem.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.helpContent = (System.Web.UI.WebControls.Literal)this.FindControl("helpContent");

            if (!int.TryParse(this.Page.Request.QueryString["helpid"], out this.helpid))
            {
                base.GotoResourceNotFound("");
            }
            else
            {
                HelpInfo helpinfo = CommentBrowser.GetHelp(this.helpid);
                this.helpContent.Text = helpinfo.Content;
                string name = helpinfo.Title;

                PageTitle.AddSiteNameTitle(name);

            }
        }
    }
}
