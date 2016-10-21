using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VHelps : VMemberTemplatedWebControl
    {

        VTemplatedRepeater helpCategories;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VHelps.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("帮助与反馈");
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.Page.Response.Redirect("/Vshop/Login.aspx");
            }
            this.helpCategories = (VTemplatedRepeater)this.FindControl("helpCategories");

            this.helpCategories.DataSource = CommentBrowser.GetHelpCategories();
            this.helpCategories.DataBind();
        }
    }
}
