using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SaleReport)]
	public class SaleReport : AdminPage
	{
		private int monthYear = System.DateTime.Now.Year;
		private SaleStatisticsType monthType = SaleStatisticsType.SaleCounts;
		private int dayYear = System.DateTime.Now.Year;
		private int dayMonth = System.DateTime.Now.Month;
		private SaleStatisticsType dayType = SaleStatisticsType.SaleCounts;
		protected YearDropDownList dropMonthForYaer;
		protected SaleStatisticsTypeRadioButtonList radioMonthForSaleType;
		protected System.Web.UI.WebControls.Button btnQueryMonthSaleTotal;
		protected System.Web.UI.WebControls.LinkButton btnCreateReportOfMonth;
		protected System.Web.UI.WebControls.Label lblMonthAllTotal;
		protected System.Web.UI.WebControls.Literal litMonthAllTotal;
		protected System.Web.UI.WebControls.Label lblMonthMaxTotal;
		protected System.Web.UI.WebControls.Literal litMonthMaxTotal;
		protected Grid grdMonthSaleTotalStatistics;
		protected YearDropDownList dropDayForYear;
		protected MonthDropDownList dropMoth;
		protected SaleStatisticsTypeRadioButtonList radioDayForSaleType;
		protected System.Web.UI.WebControls.Button btnQueryDaySaleTotal;
		protected System.Web.UI.WebControls.LinkButton btnCreateReportOfDay;
		protected System.Web.UI.WebControls.Label lblDayAllTotal;
		protected System.Web.UI.WebControls.Literal litDayAllTotal;
		protected System.Web.UI.WebControls.Label lblDayMaxTotal;
		protected System.Web.UI.WebControls.Literal litDayMaxTotal;
		protected System.Web.UI.WebControls.GridView grdDaySaleTotalStatistics;
        protected SitesDropDownList sitesDropDownList;//站点
        private int? siteId;
        private int orderSource;
        protected System.Web.UI.WebControls.DropDownList ddlOrderSource;

        protected SitesDropDownList sitesDropDownLists;//站点
        private int? siteIds;
        private int orderSources;
        protected System.Web.UI.WebControls.DropDownList ddlOrderSources;


		public System.Data.DataTable TableOfDay
		{
			get
			{
				if (this.ViewState["TableOfDay"] != null)
				{
					return (System.Data.DataTable)this.ViewState["TableOfDay"];
				}
				return null;
			}
			set
			{
				this.ViewState["TableOfDay"] = value;
			}
		}
		public System.Data.DataTable TableOfMonth
		{
			get
			{
				if (this.ViewState["TableOfMonth"] != null)
				{
					return (System.Data.DataTable)this.ViewState["TableOfMonth"];
				}
				return null;
			}
			set
			{
				this.ViewState["TableOfMonth"] = value;
			}
		}
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQueryMonthSaleTotal.Click += new System.EventHandler(this.btnQueryMonthSaleTotal_Click);
			this.btnCreateReportOfMonth.Click += new System.EventHandler(this.btnCreateReportOfMonth_Click);
			this.btnQueryDaySaleTotal.Click += new System.EventHandler(this.btnQueryDaySaleTotal_Click);
			this.btnCreateReportOfDay.Click += new System.EventHandler(this.btnCreateReportOfDay_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindMonthSaleTotalStatistics();
				this.BindDaySaleTotalStatistics();

                this.sitesDropDownList.DataBind();
                this.ddlOrderSource.BindEnum<EcShop.Entities.Orders.OrderSource>();//默认显示全部

                this.sitesDropDownLists.DataBind();
                this.ddlOrderSources.BindEnum<EcShop.Entities.Orders.OrderSource>();//默认显示全部

                this.dropMonthForYaer.SelectedValue = this.monthYear;
                this.radioMonthForSaleType.SelectedValue = this.monthType;
                this.dropDayForYear.SelectedValue = this.dayYear;
                this.dropMoth.SelectedValue = this.dayMonth;
                this.radioDayForSaleType.SelectedValue = this.dayType;
                this.sitesDropDownList.SelectedValue = this.siteId;
                this.ddlOrderSource.SelectedValue = ((OrderSource)this.orderSource).ToShowText();

                this.sitesDropDownLists.SelectedValue = this.siteIds;
                this.ddlOrderSources.SelectedValue = ((OrderSource)this.orderSources).ToShowText();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["monthYear"]))
				{
					int.TryParse(this.Page.Request.QueryString["monthYear"], out this.monthYear);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["monthType"]))
				{
					this.monthType = (SaleStatisticsType)System.Convert.ToInt32(this.Page.Request.QueryString["monthType"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dayYear"]))
				{
					int.TryParse(this.Page.Request.QueryString["dayYear"], out this.dayYear);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dayMonth"]))
				{
					int.TryParse(this.Page.Request.QueryString["dayMonth"], out this.dayMonth);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dayType"]))
				{
					this.dayType = (SaleStatisticsType)System.Convert.ToInt32(this.Page.Request.QueryString["dayType"]);
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["siteId"]))//站点Id
                {
                    this.siteId = new int?(int.Parse(this.Page.Request.QueryString["siteId"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderSource"]))//
                {
                    int.TryParse(this.Page.Request.QueryString["orderSource"], out this.orderSource);
                }

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["siteIds"]))//站点Id
                {
                    this.siteIds = new int?(int.Parse(this.Page.Request.QueryString["siteIds"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderSources"]))//
                {
                    int.TryParse(this.Page.Request.QueryString["orderSources"], out this.orderSources);
                }

                //this.dropMonthForYaer.SelectedValue = this.monthYear;
                //this.radioMonthForSaleType.SelectedValue = this.monthType;
                //this.dropDayForYear.SelectedValue = this.dayYear;
                //this.dropMoth.SelectedValue = this.dayMonth;
                //this.radioDayForSaleType.SelectedValue = this.dayType;
                //this.sitesDropDownList.SelectedValue = this.siteId;
                //this.ddlOrderSource.SelectedValue = this.ddlOrderSource.ToString();

                //this.sitesDropDownLists.SelectedValue = this.siteIds;
                //this.ddlOrderSources.SelectedValue = this.ddlOrderSources.ToString();
				return;
			}
			this.monthYear = this.dropMonthForYaer.SelectedValue;
			this.monthType = this.radioMonthForSaleType.SelectedValue;
			this.dayYear = this.dropDayForYear.SelectedValue;
			this.dayMonth = this.dropMoth.SelectedValue;
			this.dayType = this.radioDayForSaleType.SelectedValue;
            this.siteId = this.sitesDropDownList.SelectedValue;
            this.orderSource = (int)(OrderSource)System.Enum.Parse(typeof(OrderSource), this.ddlOrderSource.SelectedValue);
            this.siteIds = this.sitesDropDownLists.SelectedValue;
            this.orderSources = (int)(OrderSource)System.Enum.Parse(typeof(OrderSource), this.ddlOrderSources.SelectedValue);
		}
		private void ReBind()
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"monthYear",
					this.dropMonthForYaer.SelectedValue.ToString()
				},

				{
					"monthType",
					((int)this.radioMonthForSaleType.SelectedValue).ToString()
				},

				{
					"dayYear",
					this.dropDayForYear.SelectedValue.ToString()
				},

				{
					"dayMonth",
					this.dropMoth.SelectedValue.ToString()
				},

				{
					"dayType",
					((int)this.radioDayForSaleType.SelectedValue).ToString()
				},

				{
					"siteId",
					((int)this.sitesDropDownList.SelectedValue).ToString()
				},

				{
					"orderSource",
					((int)(OrderSource)System.Enum.Parse(typeof(OrderSource), this.ddlOrderSource.SelectedValue)).ToString()
				},

				{
					"siteIds",
					((int)this.sitesDropDownLists.SelectedValue).ToString()
				},

				{
					"orderSources",
					((int)(OrderSource)System.Enum.Parse(typeof(OrderSource), this.ddlOrderSources.SelectedValue)).ToString()
				}
			});
		}
		private void BindMonthSaleTotalStatistics()
		{
			System.Data.DataTable monthSaleTotal = SalesHelper.GetMonthSaleTotal(this.monthYear, this.monthType,this.siteId,this.orderSource);
			if (this.radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
			{
				this.grdMonthSaleTotalStatistics.Columns[1].Visible = true;
				this.grdMonthSaleTotalStatistics.Columns[1].HeaderText = this.radioMonthForSaleType.SelectedItem.Text;
				this.grdMonthSaleTotalStatistics.Columns[2].Visible = false;
			}
			else
			{
				this.grdMonthSaleTotalStatistics.Columns[1].Visible = false;
				this.grdMonthSaleTotalStatistics.Columns[2].Visible = true;
				this.grdMonthSaleTotalStatistics.Columns[2].HeaderText = this.radioMonthForSaleType.SelectedItem.Text;
			}
			this.grdMonthSaleTotalStatistics.DataSource = monthSaleTotal;
			this.grdMonthSaleTotalStatistics.DataBind();
			this.TableOfMonth = monthSaleTotal;
			this.lblMonthAllTotal.Text = string.Format("总{0}：", this.radioMonthForSaleType.SelectedItem.Text);
			decimal yearSaleTotal = SalesHelper.GetYearSaleTotal(this.monthYear, this.monthType);
			if (this.radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
			{
				this.litMonthAllTotal.Text = yearSaleTotal.ToString();
			}
			else
			{
				this.litMonthAllTotal.Text = Globals.FormatMoney(yearSaleTotal);
			}
			this.lblMonthMaxTotal.Text = string.Format("最高峰{0}：", this.radioMonthForSaleType.SelectedItem.Text);
			decimal num = 0m;
			foreach (System.Data.DataRow dataRow in monthSaleTotal.Rows)
			{
				if ((decimal)dataRow["SaleTotal"] > num)
				{
					num = (decimal)dataRow["SaleTotal"];
				}
			}
			if (this.radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
			{
				this.litMonthMaxTotal.Text = num.ToString();
				return;
			}
			this.litMonthMaxTotal.Text = Globals.FormatMoney(num);
		}
		private void btnCreateReportOfMonth_Click(object sender, System.EventArgs e)
		{
			string text = string.Empty;
			text += string.Format("总{0}：", this.radioMonthForSaleType.SelectedItem.Text);
			text = text + "," + this.litMonthAllTotal.Text;
			text += ",\"\"";
			text = text + "," + string.Format("最高峰{0}：", this.radioMonthForSaleType.SelectedItem.Text);
			text = text + "," + this.litMonthMaxTotal.Text + "\r\n\r\n";
			text += "月份";
			text = text + "," + this.radioMonthForSaleType.SelectedItem.Text;
			text += ",比例\r\n";
			foreach (System.Data.DataRow dataRow in this.TableOfMonth.Rows)
			{
				text += dataRow["Date"].ToString();
				text = text + "," + dataRow["SaleTotal"].ToString();
				text = text + "," + dataRow["Percentage"].ToString() + "%\r\n";
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=SaleTotalStatistics.csv");
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + this.dropMonthForYaer.SelectedValue.ToString() + "年零售生意"+ this.radioMonthForSaleType.SelectedItem.Text+"报告.csv");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
		private void btnQueryMonthSaleTotal_Click(object sender, System.EventArgs e)
		{
			this.ReBind();
		}
		private void BindDaySaleTotalStatistics()
		{
			System.Data.DataTable daySaleTotal = SalesHelper.GetDaySaleTotal(this.dayYear, this.dayMonth, this.dayType,this.siteIds,this.orderSources);
			if (this.radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
			{
				this.grdDaySaleTotalStatistics.Columns[1].Visible = true;
				this.grdDaySaleTotalStatistics.Columns[1].HeaderText = this.radioDayForSaleType.SelectedItem.Text;
				this.grdDaySaleTotalStatistics.Columns[2].Visible = false;
			}
			else
			{
				this.grdDaySaleTotalStatistics.Columns[1].Visible = false;
				this.grdDaySaleTotalStatistics.Columns[2].Visible = true;
				this.grdDaySaleTotalStatistics.Columns[2].HeaderText = this.radioDayForSaleType.SelectedItem.Text;
			}
			this.grdDaySaleTotalStatistics.DataSource = daySaleTotal;
			this.grdDaySaleTotalStatistics.DataBind();
			this.TableOfDay = daySaleTotal;
			this.lblDayAllTotal.Text = string.Format("总{0}：", this.radioDayForSaleType.SelectedItem.Text);
			decimal monthSaleTotal = SalesHelper.GetMonthSaleTotal(this.dayYear, this.dayMonth, this.dayType);
			if (this.radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
			{
				this.litDayAllTotal.Text = monthSaleTotal.ToString();
			}
			else
			{
				this.litDayAllTotal.Text = Globals.FormatMoney(monthSaleTotal);
			}
			this.lblDayMaxTotal.Text = string.Format("最高峰{0}：", this.radioDayForSaleType.SelectedItem.Text);
			decimal num = 0m;
			foreach (System.Data.DataRow dataRow in daySaleTotal.Rows)
			{
				if ((decimal)dataRow["SaleTotal"] > num)
				{
					num = (decimal)dataRow["SaleTotal"];
				}
			}
			if (this.radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
			{
				this.litDayMaxTotal.Text = num.ToString();
				return;
			}
			this.litDayMaxTotal.Text = Globals.FormatMoney(num);
		}
		private void btnQueryDaySaleTotal_Click(object sender, System.EventArgs e)
		{
			this.ReBind();
		}
		private void btnCreateReportOfDay_Click(object sender, System.EventArgs e)
		{
			string text = string.Empty;
			text += string.Format("总{0}：", this.radioDayForSaleType.SelectedItem.Text);
			text = text + "," + this.litDayAllTotal.Text;
			text += ",\"\"";
			text = text + "," + string.Format("最高峰{0}：", this.radioDayForSaleType.SelectedItem.Text);
			text = text + "," + this.litDayMaxTotal.Text + "\r\n\r\n";
			text += "日期";
			text = text + "," + this.radioDayForSaleType.SelectedItem.Text;
			text += ",比例\r\n";
			foreach (System.Data.DataRow dataRow in this.TableOfDay.Rows)
			{
				text += dataRow["Date"].ToString();
				text = text + "," + dataRow["SaleTotal"].ToString();
				text = text + "," + dataRow["Percentage"].ToString() + "%\r\n";
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=SaleTotalStatistics.csv");
            string strdate=this.dropDayForYear.SelectedValue.ToString()+"年"+this.dropMoth.SelectedValue.ToString()+"月";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + strdate + "零售生意" + this.radioDayForSaleType.SelectedItem.Text + "报告.csv");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
	}
}
