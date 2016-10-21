using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class Helpitem: HtmlTemplatedWebControl
    {
        private ThemedTemplatedRepeater rptHelps;
        private Pager pager;

        private System.Web.UI.WebControls.Label lblCategory;
        private System.Web.UI.WebControls.Label lblhelpName;
        private System.Web.UI.WebControls.Literal lblhelpcontent;

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Helpitem.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.rptHelps = (ThemedTemplatedRepeater)this.FindControl("rptHelps");
            this.pager = (Pager)this.FindControl("pager");
            this.lblCategory = (System.Web.UI.WebControls.Label)this.FindControl("lblCategory");
            this.lblhelpName = (System.Web.UI.WebControls.Label)this.FindControl("lblhelpName");
            this.lblhelpcontent = (System.Web.UI.WebControls.Literal)this.FindControl("lblhelpcontent");
            if (!this.Page.IsPostBack)
            {
               
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["helpid"]))
                {
                    int helpid = 0;
                    int.TryParse(this.Page.Request.QueryString["helpid"], out helpid);
                    HelpInfo helpInfo = CommentBrowser.GetHelp(helpid);
                    if (helpInfo != null)
                    {
                        HelpCategoryInfo helpCategory = CommentBrowser.GetHelpCategory(helpInfo.CategoryId);
                        PageTitle.AddSiteNameTitle(helpInfo.Title);
                        this.lblCategory.Text = helpCategory.Name;
                        this.lblhelpName.Text = helpInfo.Title;
                        this.lblhelpcontent.Text = helpInfo.Content;
                    }
                }
                this.BindList();
            }
        }
        private void BindList()
        {
            HelpQuery helpQuery = this.GetHelpQuery();
            DbQueryResult dbQueryResult = new DbQueryResult();
            dbQueryResult = CommentBrowser.GetHelpList(helpQuery);
            this.rptHelps.DataSource = dbQueryResult.Data;
            this.rptHelps.DataBind();
            this.pager.TotalRecords = dbQueryResult.TotalRecords;
        }
        private HelpQuery GetHelpQuery()
        {
            HelpQuery helpQuery = new HelpQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
            {
                int value = 0;
                if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
                {
                    helpQuery.CategoryId = new int?(value);
                }
            }
            helpQuery.PageIndex = this.pager.PageIndex;
            helpQuery.PageSize = this.pager.PageSize;
            helpQuery.SortBy = "AddedDate";
            helpQuery.SortOrder = SortAction.Desc;
            return helpQuery;
        }
    }
}

