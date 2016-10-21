using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
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
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AggregatePayable)]
    public class AggregatePayable : AdminPage
	{
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;

		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDetails;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
       
        public string DateTitle = ""; //日期
        public string OrderTotal ="0";//银行收款
        public string RefundAmount = "0";//退款金额
        public string Amount = "0";//商品总金额
        public string AdjustedFreight = "0";//代收快递运费
        public string OrderCounterFee = "0";//微信转账手续费
        public string supplyTopName = "无"; //第一个供应商
        public string supplyToAmout = "0"; //第一个供应商的金额
        public string AllTotleAmout = "0";//总额

        public DataTable Dtsuppley = new DataTable();

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
                #region 组装数据
                DataSet ds= MemberHelper.GetReport(new ReconciliationOrdersQuery
                {
                    StartDate = this.dateStart,
                    EndDate = this.dateEnd,
                    SortBy = "TradeDate",
                    SortOrder = SortAction.Desc,
                });
                if(ds.Tables.Count>0)
                {
                   if(ds.Tables[0]!=null&&ds.Tables[0].Rows.Count>0)
                   {
                       supplyTopName = ds.Tables[0].Rows[0]["供应商"].ToString();
                       supplyToAmout = decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["商品金额"].ToString()), 2).ToString();
                       Dtsuppley = ds.Tables[0];
                   }
                   if (ds.Tables[1] != null && ds.Tables[1].Rows.Count>0)
                   {
                       OrderTotal = decimal.Round(Convert.ToDecimal(ds.Tables[1].Rows[0]["银行收款"].ToString()),2).ToString();
                       RefundAmount ="-"+decimal.Round(Convert.ToDecimal(ds.Tables[1].Rows[0]["退款金额"].ToString()),2).ToString();
                       Amount =decimal.Round(Convert.ToDecimal(ds.Tables[1].Rows[0]["商品总金额"].ToString()),2).ToString();
                       AdjustedFreight =decimal.Round(Convert.ToDecimal( ds.Tables[1].Rows[0]["代收快递运费"].ToString()),2).ToString();
                       OrderCounterFee = "-"+decimal.Round(Convert.ToDecimal(ds.Tables[1].Rows[0]["微信转账手续费"].ToString()), 2).ToString();
                   }
                   AllTotleAmout = decimal.Round((Convert.ToDecimal(this.Amount) + Convert.ToDecimal(this.AdjustedFreight) + Convert.ToDecimal(this.OrderCounterFee)), 2).ToString();
                }
                #endregion 

                #region 日期
                if (!string.IsNullOrEmpty(this.dateStart.ToString()) && string.IsNullOrEmpty(this.dateEnd.ToString()))
                {
                    DateTitle =Convert.ToDateTime(this.dateStart.ToString()).ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrEmpty(this.dateStart.ToString()) && !string.IsNullOrEmpty(this.dateEnd.ToString()))
                {
                    DateTitle =Convert.ToDateTime(this.dateEnd.ToString()).ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(this.dateStart.ToString()) && !string.IsNullOrEmpty(this.dateEnd.ToString()))
                {
                    DateTitle = Convert.ToDateTime(this.dateStart.ToString()).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(this.dateEnd.ToString()).ToString("yyyy-MM-dd"); ;
                }
                #endregion 
            }
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
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
				return;
			}
			this.dateStart = this.calendarStart.SelectedDate;
			this.dateEnd = this.calendarEnd.SelectedDate;

		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();

			nameValueCollection.Add("dateStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEnd.SelectedDate.ToString());

			base.ReloadPage(nameValueCollection);
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.AggregatePayable);
            DbQueryResult balanceDetailsNoPage = MemberHelper.ExportReconciliationOrders(new ReconciliationOrdersQuery
			{
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
                SortBy = "TradingTime",
				SortOrder = SortAction.Desc,

			});
            StringBuilder sbstr = new StringBuilder();

			string text = string.Empty;
            sbstr.Append("付款日期");
            sbstr.Append(",订单号");
            sbstr.Append( ",实际金额");
            sbstr.Append( ",收款金额");
            sbstr.Append( ",退款金额");
            sbstr.Append( ",买家昵称");
            sbstr.Append( ",拍单时间");
            sbstr.Append( ",付款时间");
            sbstr.Append( ",商品总金额");
            sbstr.Append( ",订单金额");
            sbstr.Append( ",优惠金额");
            sbstr.Append( ",运费");
            sbstr.Append( ",税额");
            sbstr.Append( ",货到付款服务费");
            sbstr.Append( ",订单备注");
            sbstr.Append( ",买家留言");
            sbstr.Append( ",收货地址");
            sbstr.Append( ",收货人名称");
            sbstr.Append( ",收货国家");
            sbstr.Append( ",州/省");
            sbstr.Append( ",城市");
            sbstr.Append( ",区");
            sbstr.Append( ",邮编");
            sbstr.Append( ",联系电话");
            sbstr.Append( ",手机");
            sbstr.Append( ",买家选择物流");
            sbstr.Append( ",最晚发货时间");
            sbstr.Append( ",海外订单");
            sbstr.Append( ",是否货到付款");
            sbstr.Append( ",订单状态");
            sbstr.Append( ",发货快递单号");
            sbstr.Append( ",快递公司");
            sbstr.Append( ",货主Id");
            sbstr.Append( ",货主名称\r\n");
			foreach (System.Data.DataRow dataRow in ((System.Data.DataTable)balanceDetailsNoPage.Data).Rows)
			{

                sbstr.Append(dataRow["TradingTime"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["OrderId"]);
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
                sbstr.AppendFormat(",{0}", dataRow["Tax"]);
                sbstr.AppendFormat(",{0}", dataRow["PayCharge"]);

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
                sbstr.AppendFormat(",\"{0}\"", dataRow["ExpressCompanyName"]);
                sbstr.AppendFormat(",{0}", dataRow["ownerId"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["ownername"]);
                object obj = sbstr.ToString();
				text = string.Concat(new object[]
				{
					obj,
					",",
					"\r\n"
				});
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetailsStatistics.csv");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);

            EventLogs.WriteOperationLog(Privilege.AggregatePayable, string.Format(CultureInfo.InvariantCulture, "用户{0}生成平台销售额及应付总汇报告成功", new object[]
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

                System.TimeSpan ND = Convert.ToDateTime(EndSdate) - Convert.ToDateTime(startDate);
                double mday = ND.Days;
                if (mday > 31)
                {
                    this.ShowMsg("查询起止时间不能大于31天！", false);
                    return;
                }
                this.ReBind(true);
            }
            catch (Exception ee)
            {
                this.ShowMsg("执行异常！！", false);
            }
		}
	}
}
