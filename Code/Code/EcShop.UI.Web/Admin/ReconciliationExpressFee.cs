using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using Members;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ReconciliationOrdersDetailed)]
    public class ReconciliationExpressFee : AdminPage
    {
        private System.DateTime? dateStart;
        private System.DateTime? dateEnd;
        private string sysupperId = string.Empty;

        protected WebCalendar calendarStart;
        protected WebCalendar calendarEnd;
        protected System.Web.UI.WebControls.Button btnQueryExpressFeeDetails;
        protected System.Web.UI.WebControls.LinkButton btnCreateReport;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Grid grdExpressFeeDetails;
        protected Pager pager1;
        protected System.Web.UI.WebControls.DropDownList dllSupper;

        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryExpressFeeDetails.Click += new System.EventHandler(this.btnQueryBalanceDetails_Click);
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindSySupplier();
                this.GetExpressFeeDetails();
            }
        }
        /// <summary>
        /// 绑定供应商
        /// </summary>
        private void BindSySupplier()
        {
            this.dllSupper.Items.Clear();
            this.dllSupper.Items.Add(new System.Web.UI.WebControls.ListItem("全部", string.Empty));
            DataTable dt = SupplierHelper.GetSupplier();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    this.dllSupper.Items.Add(new ListItem(dr["SupplierName"].ToString(), dr["SupplierId"].ToString()));
                }
            }
        }
        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sysupperId"]))
                {
                    this.sysupperId = this.Page.Request.QueryString["sysupperId"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateEnd"])));
                }
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
                this.dllSupper.SelectedValue = this.sysupperId;
                return;
            }
            this.dateStart = this.calendarStart.SelectedDate;
            this.dateEnd = this.calendarEnd.SelectedDate;
            this.sysupperId = this.dllSupper.SelectedValue;
        }
        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("sysupperId", this.dllSupper.SelectedValue.ToString());
            nameValueCollection.Add("dateStart", this.calendarStart.SelectedDate.ToString());
            nameValueCollection.Add("dateEnd", this.calendarEnd.SelectedDate.ToString());
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            this.Response.Redirect(this.GenericReloadUrl(nameValueCollection));
        }
        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            string url = "ReconciliationExpressFee.aspx";
            if (queryStrings == null || queryStrings.Count == 0)
            {
                return url;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(url).Append("?");
            foreach (string text in queryStrings.Keys)
            {
                string text2 = queryStrings[text].Trim().Replace("'", "");
                if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
                {
                    stringBuilder.Append(text).Append("=").Append(base.Server.UrlEncode(text2)).Append("&");
                }
            }
            queryStrings.Clear();
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }
        private void GetExpressFeeDetails()
		{
            //分页获取数据
            DbQueryResult ExpressFeeDetails = MemberHelper.GetReconciliationExpressFeeDetails(new ReconciliationExpressFeeQuery
			{
                DetailsType="",
                BeginTime = !this.dateStart.HasValue ? "" : this.dateStart.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = !this.dateEnd.HasValue ? "" : (this.dateEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59"),
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
                Supplier = int.Parse( string.IsNullOrWhiteSpace(this.sysupperId)?"0":this.sysupperId)
			});
            this.grdExpressFeeDetails.DataSource = ExpressFeeDetails.Data;
            this.grdExpressFeeDetails.DataBind();
            this.pager1.TotalRecords = (this.pager.TotalRecords = ExpressFeeDetails.TotalRecords);
            this.dllSupper.SelectedValue = this.sysupperId;
		}
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.ReconciOrdersDetailsExcel);
            string startDate = this.calendarStart.SelectedDate.ToString();
            string EndSdate = this.calendarEnd.SelectedDate.ToString();
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(EndSdate))
            {
                this.ShowMsg("起止时间不能为空！", false);
                return;
            }

            System.TimeSpan ND = Convert.ToDateTime(EndSdate) - Convert.ToDateTime(startDate);
            double mday = ND.Days;
            if (mday > 31)
            {
                this.ShowMsg("查询起止时间不能大于31天！", false);
                return;
            }
            //获取搜索所有数据 PageIndex=0 为获取所有数据
            DbQueryResult ExpressFeeDetails = MemberHelper.GetReconciliationExpressFeeDetails(new ReconciliationExpressFeeQuery
            {
                BeginTime = !this.dateStart.HasValue ? "" : this.dateStart.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = !this.dateEnd.HasValue ? "" : (this.dateEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59"),
                PageIndex = 0,
                PageSize = 0,
                Supplier = int.Parse(string.IsNullOrWhiteSpace(this.sysupperId) ? "0" : this.sysupperId)
            });
            DataTable dtResult = (DataTable)ExpressFeeDetails.Data;
            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                this.ShowMsg("没有数据！", false);
                return;
            }
            foreach (DataColumn cl in dtResult.Columns)
            {
                if (cl.ColumnName == "OrderId")
                {
                    cl.ColumnName = "子订单编号";
                }
                else if (cl.ColumnName == "SupplierName")
                {
                    cl.ColumnName = "供应商";
                }
                else if (cl.ColumnName == "SourceOrderId")
                {
                    cl.ColumnName = "订单号";
                }
                else if (cl.ColumnName == "TradingTime")
                {
                    cl.ColumnName = "交易时间";
                }
                else if (cl.ColumnName == "ExpressCompanyName")
                {
                    cl.ColumnName = "快递公司";
                }
                else if (cl.ColumnName == "ShipOrderNumber")
                {
                    cl.ColumnName = "快递单号";
                }
                else if (cl.ColumnName.ToLower().Trim() == "shippingdate")
                {
                    cl.ColumnName = "发货时间";
                }
                else if (cl.ColumnName.ToLower().Trim() == "adjustedfreight")
                {
                    cl.ColumnName = "快递费用";
                }

            }
            MemoryStream ms = NPOIExcelHelper.ExportToExcel(dtResult, "快递费用报表明细");


            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ReconciliationOrdersDetailed.xlsx");

            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=快递费用报表明细.xlsx");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=快递费用报表明细_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".xlsx");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
            ms.Close();
            EventLogs.WriteOperationLog(Privilege.ReconciOrdersDetailsExcel, string.Format(CultureInfo.InvariantCulture, "用户{0}生成对快递费用明细报告成功", new object[]
			{
                this.User.Identity.Name
			}));

            this.Page.Response.End();

        }
        private void btnQueryBalanceDetails_Click(object sender, System.EventArgs e)
        {
            try
            {
                string startDate = this.calendarStart.SelectedDate.ToString();
                string EndSdate = this.calendarEnd.SelectedDate.ToString();
                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(EndSdate))
                {
                    this.ShowMsg("起止时间不能为空！", false);
                }
                int interval = new TimeSpan(Convert.ToDateTime(EndSdate).Ticks - Convert.ToDateTime(startDate).Ticks).Days;

                if (interval > 31)
                {
                    this.ShowMsg("查询起止时间不能大于31天！", false);
                    return;
                }
                this.ReBind(true);
            }
            catch (Exception ee)
            {
                this.ShowMsg("执行错误！", false);
            }
        }
    }
}
