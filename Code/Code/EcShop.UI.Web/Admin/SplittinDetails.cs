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
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Members)]
	public class SplittinDetails : AdminPage
	{
		private System.DateTime? dataStart;
		private System.DateTime? dataEnd;
		private int userId;
		protected System.Web.UI.WebControls.Literal litUserName;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdSplittinDetail;
		protected Pager pager1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				if (int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
				{
					Member member = MemberHelper.GetMember(this.userId);
					if (member != null)
					{
						this.litUserName.Text = member.Username;
					}
				}
				this.BindSplittinrawRequest();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					this.dataStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					this.dataEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
				}
				this.calendarStart.SelectedDate = this.dataStart;
				this.calendarEnd.SelectedDate = this.dataEnd;
				return;
			}
			this.dataStart = this.calendarStart.SelectedDate;
			this.dataEnd = this.calendarEnd.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
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
			DbQueryResult splittinDetails = MemberHelper.GetSplittinDetails(new BalanceDetailQuery
			{
				UserId = new int?(this.userId),
				FromDate = this.dataStart,
				ToDate = this.dataEnd,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			});
			this.grdSplittinDetail.DataSource = splittinDetails.Data;
			this.grdSplittinDetail.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = splittinDetails.TotalRecords);
			this.pager.TotalRecords = (this.pager.TotalRecords = splittinDetails.TotalRecords);
		}
	}
}
