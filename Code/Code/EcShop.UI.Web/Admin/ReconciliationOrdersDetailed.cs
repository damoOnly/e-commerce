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
    public class ReconciliationOrdersDetailed: AdminPage
	{
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
        private string sysupperId=string.Empty;

		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDetails;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDetails;
		protected Pager pager1;
        protected System.Web.UI.WebControls.DropDownList dllSupper;

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQueryBalanceDetails.Click += new System.EventHandler(this.btnQueryBalanceDetails_Click);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
       
			this.LoadParameters();
			if (!this.Page.IsPostBack)
            {
                this.BindSySupplier();
				this.GetBalanceDetails();
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
			base.ReloadPage(nameValueCollection);
		}
		private void GetBalanceDetails()
		{
            DbQueryResult balanceDetails = MemberHelper.GetReconciliationOrdersDetailed(new ReconciliationOrdersQuery
			{
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
                SortBy = "TradingTime",
				SortOrder = SortAction.Desc,
                Supplier = this.sysupperId
			});
			this.grdBalanceDetails.DataSource = balanceDetails.Data;
			this.grdBalanceDetails.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = balanceDetails.TotalRecords);
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
            DbQueryResult balanceDetailsNoPage = MemberHelper.ExportReconciliationOrdersDetailed(new ReconciliationOrdersQuery
			{
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
                SortBy = "TradingTime",
				SortOrder = SortAction.Desc,
                Supplier = this.sysupperId
			});
            DataTable dtResult = (DataTable)balanceDetailsNoPage.Data;
            if (dtResult==null||dtResult.Rows.Count == 0)
            {
                this.ShowMsg("没有数据！", false);
                return;
            }
            MemoryStream ms = NPOIExcelHelper.ExportToExcel(dtResult, "对账单");


            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ReconciliationOrdersDetailed.xlsx");

            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单明细报表.xlsx");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单明细报表_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".xlsx");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
            ms.Close();
            EventLogs.WriteOperationLog(Privilege.ReconciOrdersDetailsExcel, string.Format(CultureInfo.InvariantCulture, "用户{0}生成对账订单明细报告成功", new object[]
			{
                this.User.Identity.Name
			}));

            this.Page.Response.End();

			string text = string.Empty;
            StringBuilder sbstr = new StringBuilder();
            sbstr.Append("付款日期");
            sbstr.Append(",订单号");
            sbstr.Append(",子订单号");
            sbstr.Append(",实际金额");
            sbstr.Append(",收款金额");
            sbstr.Append(",退款金额");
            sbstr.Append(",买家昵称");
            sbstr.Append(",拍单时间");
            sbstr.Append(",付款时间");
            sbstr.Append(",商品总金额");
            sbstr.Append(",订单金额");
            sbstr.Append(",优惠金额");
            sbstr.Append(",运费");
            sbstr.Append(",优惠券");
            sbstr.Append(",现金劵");
            sbstr.Append(",货到付款服务费");
            sbstr.Append(",扣点");
            sbstr.Append(",成本价");
            sbstr.Append(",订单备注");
            sbstr.Append(",买家留言");
            sbstr.Append(",收货地址");
            sbstr.Append(",收货人名称");
            sbstr.Append(",收货国家");
            sbstr.Append(",州/省");
            sbstr.Append(",城市");
            sbstr.Append(",区");
            sbstr.Append(",邮编");
            sbstr.Append(",联系电话");
            sbstr.Append(",手机");
            sbstr.Append(",买家选择物流");
            sbstr.Append(",最晚发货时间");
            sbstr.Append(",海外订单");
            sbstr.Append(",是否货到付款");
            sbstr.Append(",订单状态");
            sbstr.Append(",发货快递单号");
            sbstr.Append(",退货快递单号");
            sbstr.Append(",商品名称");
            sbstr.Append(",商品条码");
            sbstr.Append(",购买数量");
            sbstr.Append(",退货数量");
            sbstr.Append(",售价");
            sbstr.Append(",销售价格");
            sbstr.Append(",货主Id");
            sbstr.Append(",货主名称");
            sbstr.Append(",发货仓库");
            sbstr.Append(",是否匹配促销模板\r\n");
			foreach (System.Data.DataRow dataRow in ((System.Data.DataTable)balanceDetailsNoPage.Data).Rows)
			{
                sbstr.Append(dataRow["TradingTime"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["OrderId"]);
                sbstr.AppendFormat(",{0}", dataRow["ChildOrderId"]);
                sbstr.AppendFormat(",{0}", dataRow["ActualAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["TotalAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["RefundAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["RealName"]);
                sbstr.AppendFormat(",{0}", dataRow["OrderDate"]);
                sbstr.AppendFormat(",{0}", dataRow["PayDate"]);
                sbstr.AppendFormat(",{0}", dataRow["Amount"]);
                sbstr.AppendFormat(",{0}", dataRow["OrderTotal"]);
                sbstr.AppendFormat(",{0}", dataRow["DiscountAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["AdjustedFreight"]);
                sbstr.AppendFormat(",{0}", dataRow["CouponValue"]);
                sbstr.AppendFormat(",{0}", dataRow["VoucherValue"]);
                sbstr.AppendFormat(",{0}", dataRow["PayCharge"]);
                sbstr.AppendFormat(",{0}", dataRow["DeductFee"]);
                sbstr.AppendFormat(",{0}", dataRow["CostPrice"]);

                sbstr.AppendFormat(",\"{0}\"", dataRow["ManagerRemark"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Remark"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Address"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["ShipTo"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Country"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Province"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["City"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Area"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["ZipCode"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["TelPhone"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["CellPhone"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["ModeName"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["LatestDelivery"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["OverseasOrders"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["CashDelivery"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["OrderStatus"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["ShipOrderNumber"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["ReturnsExpressNumber"]);

                sbstr.AppendFormat(",\"{0}\"", dataRow["ItemDescription"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["SKU"]);
                sbstr.AppendFormat(",{0}", dataRow["Quantity"]);
                sbstr.AppendFormat(",{0}", dataRow["ReturnQuantity"]);
                sbstr.AppendFormat(",{0}", dataRow["ItemListPrice"]);
                sbstr.AppendFormat(",{0}", dataRow["ItemAdjustedPrice"]);
                sbstr.AppendFormat(",{0}", dataRow["SupplierId"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["SupplierName"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["ShipWarehouseName"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Template"]);
                sbstr.Append("\r\n");
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ReconciliationOrdersDetailed.csv");
            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单明细报表.csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单明细报表_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".csv");
            }

			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
            this.Page.Response.Write(sbstr.ToString());
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
