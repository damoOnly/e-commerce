using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SplittinDrawRequest)]
	public class SplittinDrawRequest : AdminPage
	{
		private string searchKey;
		private System.DateTime? dataStart;
		private System.DateTime? dataEnd;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDrawRequest;
		protected Pager pager1;
		protected System.Web.UI.WebControls.Label lblAccount;
		protected System.Web.UI.WebControls.TextBox txtManagerRemark;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidJournalNumber;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidManagerRemark;
		protected System.Web.UI.WebControls.Button btnAccept;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
		}
		private void btnAccept_Click(object sender, System.EventArgs e)
		{
			long journalNumber = 0L;
			long.TryParse(this.hidJournalNumber.Value, out journalNumber);
			if (MemberHelper.AccepteDraw(journalNumber, this.hidManagerRemark.Value))
			{
				this.BindSplittinrawRequest();
				this.ShowMsg("结算申请已经审核通过", true);
				return;
			}
			this.ShowMsg("审核通过失败", false);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				int userId;
				if (int.TryParse(this.Page.Request.QueryString["userId"], out userId))
				{
					Member member = MemberHelper.GetMember(userId);
					if (member != null)
					{
						this.txtUserName.Text = member.Username;
					}
				}
				this.BindSplittinrawRequest();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					this.dataStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					this.dataEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
				}
				this.txtUserName.Text = this.searchKey;
				this.calendarStart.SelectedDate = this.dataStart;
				this.calendarEnd.SelectedDate = this.dataEnd;
				return;
			}
			this.searchKey = this.txtUserName.Text;
			this.dataStart = this.calendarStart.SelectedDate;
			this.dataEnd = this.calendarEnd.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtUserName.Text);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		public void BindSplittinrawRequest()
		{
			DbQueryResult splittinDraws = MemberHelper.GetSplittinDraws(new BalanceDrawRequestQuery
			{
				FromDate = this.dataStart,
				ToDate = this.dataEnd,
				UserName = this.txtUserName.Text,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			}, new int?(1));
			this.grdBalanceDrawRequest.DataSource = splittinDraws.Data;
			this.grdBalanceDrawRequest.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = splittinDraws.TotalRecords);
			this.pager.TotalRecords = (this.pager.TotalRecords = splittinDraws.TotalRecords);
		}
	}
}
