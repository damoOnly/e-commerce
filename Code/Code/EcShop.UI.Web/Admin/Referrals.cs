using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Referrals)]
	public class Referrals : AdminPage
	{
		private string searchKey;
		private string realName;
		private string cellphone;
		private string referralUsername;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected System.Web.UI.WebControls.TextBox txtCellphnoe;
		protected System.Web.UI.WebControls.TextBox txtReferralUsername;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdReferralRequest;
		protected Pager pager1;
		protected System.Web.UI.WebControls.Label lblRealname;
		protected System.Web.UI.WebControls.Label lblCellphone;
		protected System.Web.UI.WebControls.Label lblWeChat;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.Label lblRequetsDate;
		protected System.Web.UI.WebControls.Label lblReferralOrderNumber;
		protected System.Web.UI.WebControls.Label lblReferralExpenditure;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindReferralRequest();
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
				{
					this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cellphone"]))
				{
					this.cellphone = base.Server.UrlDecode(this.Page.Request.QueryString["cellphone"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["referralUsername"]))
				{
					this.referralUsername = base.Server.UrlDecode(this.Page.Request.QueryString["referralUsername"]);
				}
				this.txtUserName.Text = this.searchKey;
				this.txtRealName.Text = this.realName;
				this.txtCellphnoe.Text = this.cellphone;
				this.txtReferralUsername.Text = this.referralUsername;
				return;
			}
			this.searchKey = this.txtUserName.Text;
			this.realName = this.txtRealName.Text;
			this.cellphone = this.txtCellphnoe.Text;
			this.referralUsername = this.txtReferralUsername.Text;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtUserName.Text);
			nameValueCollection.Add("realName", this.txtRealName.Text);
			nameValueCollection.Add("cellphone", this.txtCellphnoe.Text);
			nameValueCollection.Add("referralUsername", this.txtReferralUsername.Text);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		public void BindReferralRequest()
		{
			DbQueryResult referrals = MemberHelper.GetReferrals(new MemberQuery
			{
				Username = this.txtUserName.Text,
				Realname = this.txtRealName.Text,
				CellPhone = this.txtCellphnoe.Text,
				ReferralUsername = this.txtReferralUsername.Text,
				ReferralStatus = new int?(1),
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			});
			this.grdReferralRequest.DataSource = referrals.Data;
			this.grdReferralRequest.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = referrals.TotalRecords);
			this.pager.TotalRecords = (this.pager.TotalRecords = referrals.TotalRecords);
		}
	}
}
