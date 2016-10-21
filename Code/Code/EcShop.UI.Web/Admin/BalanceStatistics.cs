using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberBalanceStatistics)]
	public class BalanceStatistics : AdminPage
	{
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		private string userName;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDetails;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDetails;
		protected Pager pager1;
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
				this.GetBalanceDetails();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
				{
					this.userName = this.Page.Request.QueryString["userName"].ToString();
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
				this.txtUserName.Text = this.userName;
				return;
			}
			this.dateStart = this.calendarStart.SelectedDate;
			this.dateEnd = this.calendarEnd.SelectedDate;
			this.userName = this.txtUserName.Text;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userName", this.userName);
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
			DbQueryResult balanceDetails = MemberHelper.GetBalanceDetails(new BalanceDetailQuery
			{
				FromDate = this.dateStart,
				ToDate = this.dateEnd,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "TradeDate",
				SortOrder = SortAction.Desc,
				UserName = this.userName
			});
			this.grdBalanceDetails.DataSource = balanceDetails.Data;
			this.grdBalanceDetails.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = balanceDetails.TotalRecords);
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			DbQueryResult balanceDetailsNoPage = MemberHelper.GetBalanceDetailsNoPage(new BalanceDetailQuery
			{
				FromDate = this.dateStart,
				ToDate = this.dateEnd,
				SortBy = "TradeDate",
				SortOrder = SortAction.Desc,
				UserName = this.userName
			});
			string text = string.Empty;
			text += "用户名";
			text += ",交易时间";
			text += ",业务摘要";
			text += ",转入金额";
			text += ",转出金额";
			text += ",当前余额\r\n";
			foreach (System.Data.DataRow dataRow in ((System.Data.DataTable)balanceDetailsNoPage.Data).Rows)
			{
				string str = string.Empty;
				switch (System.Convert.ToInt32(dataRow["TradeType"]))
				{
				case 1:
					str = "自助充值";
					break;
				case 2:
					str = "后台加款";
					break;
				case 3:
					str = "消费";
					break;
				case 4:
					str = "提现";
					break;
				case 5:
					str = "订单退款";
					break;
				default:
					str = "其他";
					break;
				}
				text += dataRow["UserName"];
				text = text + "," + dataRow["TradeDate"];
				text = text + "," + str;
				text = text + "," + dataRow["Income"];
				text = text + "," + dataRow["Expenses"];
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					",",
					dataRow["Balance"],
					"\r\n"
				});
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetailsStatistics.csv");
            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=会员预付款报表.csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=会员预付款报表_"+this.dateStart.Value.ToString("yyyyMMdd")+"-"+this.dateEnd.Value.ToString("yyyyMMdd")+".csv");
            }
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);

            EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "用户{0}，会员为{1}生成预付款信息报告成功", new object[]
			{
                 this.User.Identity.Name,
                this.userName
			}));

			this.Page.Response.End();

            

		}
		private void btnQueryBalanceDetails_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}
