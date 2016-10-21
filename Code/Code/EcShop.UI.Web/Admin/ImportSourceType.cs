using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ImportSourceTypeView)]
    public class ImportSourceType : AdminPage
	{
        private string cnArea;
        private string enArea;
        private string remark;
		private System.DateTime? startDate;
		private System.DateTime? endDate;
        protected System.Web.UI.WebControls.TextBox txtCnArea;
        protected System.Web.UI.WebControls.TextBox txtEnArea;
        protected System.Web.UI.WebControls.TextBox txtRemark;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
        protected Grid grdImportSourceTypes;
		protected Pager pager;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.grdImportSourceTypes.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdImportSourceTypes_RowDataBound);
            this.grdImportSourceTypes.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdImportSourceTypes_RowDeleting);
			if (!this.Page.IsPostBack)
			{
                this.BindImportSourceTypes();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}


		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadImportSourceTypes(true);
		}
        private void grdImportSourceTypes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
                //System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
                //System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
			}
		}
        private void grdImportSourceTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
            string text = this.grdImportSourceTypes.DataKeys[e.RowIndex].Value.ToString();
            if (text != "")
            {
                list.Add(System.Convert.ToInt32(text));
            }
            if (ImportSourceTypeHelper.DeleteImportSourceTypes(text) > 0)
            {
                this.ShowMsg("删除原产地成功", true);
                this.ReloadImportSourceTypes(false);
            }
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要删除的原产地", false);
                return;
            }
            int num = ImportSourceTypeHelper.DeleteImportSourceTypes(text);
            if (num > 0)
            {
                this.ShowMsg("成功删除了选择的原产地", true);
                this.BindImportSourceTypes();
                return;
            }
            this.ShowMsg("删除原产地失败，未知错误", false);
		}

        private void BindImportSourceTypes()
		{
			this.LoadParameters();
            ImportSourceTypeQuery importSourceTypeQuery = new ImportSourceTypeQuery
			{
                Remark = this.remark,
                CnArea = this.cnArea,
                EnArea = this.enArea,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = this.startDate,
				EndDate = this.endDate
			};

            Globals.EntityCoding(importSourceTypeQuery, true);
            DbQueryResult importSourceTypes = ImportSourceTypeHelper.GetImportSourceTypes(importSourceTypeQuery);
            this.grdImportSourceTypes.DataSource = importSourceTypes.Data;
            this.grdImportSourceTypes.DataBind();

            this.txtCnArea.Text = importSourceTypeQuery.CnArea;
            this.txtEnArea.Text = importSourceTypeQuery.EnArea;
            this.txtRemark.Text = importSourceTypeQuery.Remark;
            
            this.pager1.TotalRecords = (this.pager.TotalRecords = importSourceTypes.TotalRecords);
		}
        private void ReloadImportSourceTypes(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("remark", Globals.UrlEncode(this.txtRemark.Text.Trim()));
            nameValueCollection.Add("cnArea", Globals.UrlEncode(this.txtCnArea.Text.Trim()));
            nameValueCollection.Add("enArea", Globals.UrlEncode(this.txtEnArea.Text.Trim()));
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["remark"]))
			{
                this.remark = Globals.UrlDecode(this.Page.Request.QueryString["remark"]);
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cnArea"]))
			{
                this.cnArea = Globals.UrlDecode(this.Page.Request.QueryString["cnArea"]);
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["enArea"]))
            {
                this.enArea = Globals.UrlDecode(this.Page.Request.QueryString["enArea"]);
            }
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}

			this.calendarStartDate.SelectedDate = this.startDate;
			this.calendarEndDate.SelectedDate = this.endDate;
		}
	}
}
