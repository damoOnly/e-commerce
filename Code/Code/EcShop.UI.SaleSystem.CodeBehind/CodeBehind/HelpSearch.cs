using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class HelpSearch : HtmlTemplatedWebControl
    {
        private ThemedTemplatedRepeater rptHelpSearch;
        private Pager pager;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-HelpSearch.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.rptHelpSearch = (ThemedTemplatedRepeater)this.FindControl("rptHelpSearch");
            this.pager = (Pager)this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                this.BindPromoteSales();
            }
        }
        private void BindPromoteSales()
        {
            
            string searchcontent = this.Page.Request.QueryString["searchcontent"];
            searchcontent=HttpUtility.UrlDecode(searchcontent);
            Pagination pagination = new Pagination();
            pagination.PageIndex = this.pager.PageIndex;
            pagination.PageSize = this.pager.PageSize;
            int totalRecords = 0;
            DataTable helps = CommentBrowser.SearchHelps(pagination, searchcontent, out totalRecords);
            if (helps != null && helps.Rows.Count > 0)
            {
                this.rptHelpSearch.DataSource = helps;
                this.rptHelpSearch.DataBind();
            }
            this.pager.TotalRecords = totalRecords;
        }
    }
}