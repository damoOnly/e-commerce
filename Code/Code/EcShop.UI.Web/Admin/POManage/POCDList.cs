using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.PO;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Reflection;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using System.Web;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

namespace EcShop.UI.Web.Admin.PO
{
    [PrivilegeCheck(Privilege.POCDList)]
    public class POCDList : AdminPage
    {
        private string poNumber;
        //private int POCDStatus = -1;
        private string fromID;

        protected ASPNET.WebControls.PageSize hrefPageSize;
        protected Pager pager1;
        protected Pager pager;
        protected System.Web.UI.WebControls.Repeater rpPO;
        protected System.Web.UI.WebControls.HiddenField hidUserid;
        protected System.Web.UI.WebControls.HiddenField hidUserName;
        protected System.Web.UI.WebControls.TextBox txtPONumber;
        protected System.Web.UI.WebControls.TextBox txtfromID;
        protected WebCalendar CreateStartDate;
        protected WebCalendar CreateEndDate;
        private System.DateTime? startDate;
        private System.DateTime? endDate;

        protected System.Web.UI.WebControls.Button btnSearch;
        

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            var member = HiContext.Current.User;
            if (member != null && !member.IsLockedOut)
            {
                hidUserid.Value = member.UserId.ToString();
                hidUserName.Value = member.Username;
            }
            else
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }
            if (!this.Page.IsPostBack)
            {
                this.BindData("CreateTime", SortAction.Desc);

            }
        }

        private void BindData(string creTime, SortAction sortAction)
        {
            DbQueryResult purchaseOrder = this.GetData(creTime, sortAction);
            this.rpPO.DataSource = purchaseOrder.Data;
            this.rpPO.DataBind();

            this.pager1.TotalRecords = (this.pager.TotalRecords = purchaseOrder.TotalRecords);

        }

        private DbQueryResult GetData(string sortBy, SortAction sortOrder)
        {
            this.LoadParameters();
            PODeclareListQuery PODeclareListQuery = new PODeclareListQuery
            {
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = sortOrder,
                SortBy = sortBy,
                StartDate = this.startDate,
                EndDate = this.endDate,
                //POCDStatus = this.POCDStatus,
                fromID = this.fromID,
                PONumber = this.poNumber
            };
            Globals.EntityCoding(PODeclareListQuery, true);
            return PurchaseOrderHelper.GetPODeclareList(PODeclareListQuery);
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }
        private void ReloadProductOnSales(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("poNumber", Globals.UrlEncode(this.txtPONumber.Text.Trim()));
            nameValueCollection.Add("fromID", Globals.UrlEncode(this.txtfromID.Text.Trim()));
            //nameValueCollection.Add("fromID", ddlSupplier.SelectedValue.ToString());

            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (this.CreateStartDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("startDate", this.CreateStartDate.SelectedDate.Value.ToString());
            }
            if (this.CreateEndDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("endDate", this.CreateEndDate.SelectedDate.Value.ToString());
            }

            //nameValueCollection.Add("POCDStatus", this.ddlStatus.SelectedValue.ToString());
            base.ReloadPage(nameValueCollection, false);
        }
        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["poNumber"]))
            {
                this.poNumber = Globals.UrlDecode(this.Page.Request.QueryString["poNumber"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["fromID"]))
            {
                this.fromID = Globals.UrlDecode(this.Page.Request.QueryString["fromID"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }

            this.txtPONumber.Text = this.poNumber;
            this.txtfromID.Text = this.fromID;

            this.CreateStartDate.SelectedDate = this.startDate;
            this.CreateEndDate.SelectedDate = this.endDate;
        }
        /// <summary>
        /// 把双引号替换为&#34;，单引号替换为&#39;。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ReplaceStr(string str)
        {
            return str.Replace("\"", "&#34;").Replace("'", "&#39;");
        }

        protected void rpPO_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }
            if (e.CommandName == "del")
            {
                int num = System.Convert.ToInt32(e.CommandArgument);
                if (!PurchaseOrderHelper.Deletet(num, member.UserId))
                {
                    this.ShowMsg("该单状态不允许操作或您无权操作该单", false);
                    return;
                }
                this.ReloadProductOnSales(true);
            }
        }
    }
}
