using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.HS;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin
{
    public class HSDeclareDisplay : AdminPage
    {
        double sHours;
        double eHours;
        protected HourDropDownList ddListStartHour;
        protected HourDropDownList ddListEndHour;
        private System.DateTime? startDate;
        private System.DateTime? endDate;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        //protected System.Web.UI.WebControls.Button btnQueryLogs;
        //protected ImageLinkButton lkbDeleteAll;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected Grid dlstHSDeclare;
        protected System.Web.UI.WebControls.TextBox OrderIDSearchText;

        protected System.Web.UI.WebControls.TextBox txtOperationUserName;
        protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        private string searchkey;
        private string OperationUserName;
        private string ShipOrderNumber;

        protected System.Web.UI.WebControls.DropDownList DeclareStatusList;
        private string DeclareStatus;
        protected System.Web.UI.WebControls.HiddenField hidUserid;
        protected System.Web.UI.WebControls.HiddenField hidUserName;

        private string OutNoSearch;
        private string LogisticsNoSearch;
        protected System.Web.UI.WebControls.TextBox OutNoSearchText;
        protected System.Web.UI.WebControls.TextBox LogisticsNoSearchText;

        protected System.Web.UI.WebControls.DropDownList ddlWMSStatus;
        private string WMSStatus;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            dlstHSDeclare.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.dlstHSDeclare_RowDataBound);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            if (!this.Page.IsPostBack)
            {
                this.ddListStartHour.DataBind();
                this.ddListEndHour.DataBind();
                this.BindHSDeclare();
                var member = HiContext.Current.User;
                if (member != null && !member.IsLockedOut)
                {
                    hidUserid.Value = member.UserId.ToString();
                    hidUserName.Value = member.Username;
                }
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("searchkey", this.OrderIDSearchText.Text);
            nameValueCollection.Add("DeclareStatus", this.DeclareStatusList.SelectedValue);
            nameValueCollection.Add("WMSStatus", this.ddlWMSStatus.SelectedValue);
            
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                sHours = double.Parse(ddListStartHour.SelectedValue.ToString());
                nameValueCollection.Add("sHours", sHours.ToString());
                nameValueCollection.Add("startDate", DateTime.Parse(this.calendarStartDate.SelectedDate.Value.ToString()).AddHours(sHours).ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                eHours = double.Parse(ddListEndHour.SelectedValue.ToString());
                nameValueCollection.Add("eHours", eHours.ToString());
                nameValueCollection.Add("endDate", DateTime.Parse(this.calendarEndDate.SelectedDate.Value.AddHours(-23).AddMinutes(-59).AddSeconds(-59).ToString()).AddHours(eHours).ToString());
            }
            nameValueCollection.Add("SortBy", "OrderDate");
            nameValueCollection.Add("SortOrder", SortAction.Asc.ToString());
            nameValueCollection.Add("OutNoSearchText", this.OutNoSearchText.Text);
            nameValueCollection.Add("LogisticsNoSearchText", this.LogisticsNoSearchText.Text);

            nameValueCollection.Add("OperationUserName", this.txtOperationUserName.Text);
            nameValueCollection.Add("ShipOrderNumber", this.txtShipOrderNumber.Text);

            base.ReloadPage(nameValueCollection);
        }

        public void dlstHSDeclare_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Literal lblDeclareStatus = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litDeclareStatus");

                SetRowValue(lblDeclareStatus);
            }
        }

        private void SetRowValue(System.Web.UI.WebControls.Literal status)
        {
            switch (status.Text)
            {
                case "0":
                    status.Text = "未认领";
                    break;
                case "4":
                    status.Text = "已认领";
                    break;
                case "1":
                    status.Text = "已申报";
                    break;
                case "2":
                    status.Text = "申报成功";
                    break;
                case "3":
                    status.Text = "申报失败";
                    break;
                case "5":
                    status.Text = "异常处理";
                    break;
            }
        }

        public void BindHSDeclare()
        {
            HSDeclareQuery HSDeclareQuery = this.GetHSDeclareQuery();
            DbQueryResult HSDeclare = HSCodeHelper.GetHSDeclare(HSDeclareQuery);
            this.dlstHSDeclare.DataSource = HSDeclare.Data;
            this.dlstHSDeclare.DataBind();
            this.SetSearchControl();
            this.pager.TotalRecords = HSDeclare.TotalRecords;
            this.pager1.TotalRecords = HSDeclare.TotalRecords;
        }

        private void SetSearchControl()
        {
            if (!this.Page.IsCallback)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
                {
                    this.OperationUserName = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipOrderNumber"]))
                {
                    this.ShipOrderNumber = base.Server.UrlDecode(this.Page.Request.QueryString["ShipOrderNumber"]);
                }
                
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
                {
                    this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
                {
                    this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchkey"]))
                {
                    this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchkey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["DeclareStatus"]))
                {
                    this.DeclareStatus = Globals.UrlDecode(this.Page.Request.QueryString["DeclareStatus"]);
                }

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["WMSStatus"]))
                {
                    this.WMSStatus = Globals.UrlDecode(this.Page.Request.QueryString["WMSStatus"]);
                }

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sHours"]))
                {
                    this.sHours = double.Parse(Globals.UrlDecode(this.Page.Request.QueryString["sHours"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["eHours"]))
                {
                    this.eHours = double.Parse(Globals.UrlDecode(this.Page.Request.QueryString["eHours"]));
                }

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OutNoSearchText"]))
                {
                    this.OutNoSearch = Globals.UrlDecode(this.Page.Request.QueryString["OutNoSearchText"]);
                }

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["LogisticsNoSearchText"]))
                {
                    this.LogisticsNoSearch = Globals.UrlDecode(this.Page.Request.QueryString["LogisticsNoSearchText"]);
                }

                this.OrderIDSearchText.Text = this.searchkey;
                this.txtOperationUserName.Text = this.OperationUserName;
                this.txtShipOrderNumber.Text = this.ShipOrderNumber;
                
                this.DeclareStatusList.SelectedValue = this.DeclareStatus;
                this.ddlWMSStatus.SelectedValue = this.WMSStatus;

                this.calendarStartDate.SelectedDate = this.startDate;
                this.calendarEndDate.SelectedDate = this.endDate;
                ddListStartHour.SelectedValue = int.Parse(sHours.ToString());
                ddListEndHour.SelectedValue = int.Parse(eHours.ToString());
                this.OutNoSearchText.Text = OutNoSearch;
                this.LogisticsNoSearchText.Text = LogisticsNoSearch;

            }
            this.searchkey = this.OrderIDSearchText.Text;
            this.OperationUserName = this.txtOperationUserName.Text;
            this.ShipOrderNumber = this.txtShipOrderNumber.Text;
            
            this.DeclareStatus = this.DeclareStatusList.SelectedValue;
            this.WMSStatus = this.ddlWMSStatus.SelectedValue;
            this.OutNoSearch = this.OutNoSearchText.Text;
            this.LogisticsNoSearch = this.LogisticsNoSearchText.Text;
        }

        private HSDeclareQuery GetHSDeclareQuery()
        {
            HSDeclareQuery HSDeclareQuery = new HSDeclareQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
            {
                HSDeclareQuery.OperationUserName = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipOrderNumber"]))
            {
                HSDeclareQuery.ShipOrderNumber = base.Server.UrlDecode(this.Page.Request.QueryString["ShipOrderNumber"]);
            }
            

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                HSDeclareQuery.FromDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                HSDeclareQuery.ToDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["endDate"]));
            }

            HSDeclareQuery.Page.SortBy = "OrderDate";
            HSDeclareQuery.Page.SortOrder = SortAction.Asc;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchkey"]))
            {
                HSDeclareQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["searchkey"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["WMSStatus"]))
            {
                HSDeclareQuery.WMSStatus = Globals.UrlDecode(this.Page.Request.QueryString["WMSStatus"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["DeclareStatus"]))
            {
                HSDeclareQuery.DeclareStatus = Globals.UrlDecode(this.Page.Request.QueryString["DeclareStatus"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OutNoSearchText"]))
            {
                HSDeclareQuery.OutNo = Globals.UrlDecode(this.Page.Request.QueryString["OutNoSearchText"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["LogisticsNoSearchText"]))
            {
                HSDeclareQuery.LogisticsNo = Globals.UrlDecode(this.Page.Request.QueryString["LogisticsNoSearchText"]);
            }
            HSDeclareQuery.Page.PageIndex = this.pager.PageIndex;
            HSDeclareQuery.Page.PageSize = this.pager.PageSize;

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings == null)
            {
                HSDeclareQuery.SuspensionTime = 0;
            }
            HSDeclareQuery.SuspensionTime = masterSettings.SuspensionTime;
            

            return HSDeclareQuery;
        }
    }
}
