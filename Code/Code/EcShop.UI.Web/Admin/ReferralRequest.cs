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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ReferralRequest)]
	public class ReferralRequest : AdminPage
	{
		private string searchKey;
		private string realName;
		private string cellphone;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected System.Web.UI.WebControls.TextBox txtCellphnoe;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdReferralRequest;
		protected Pager pager1;
		protected System.Web.UI.WebControls.Label lblRealname;
		protected System.Web.UI.WebControls.Label lblCellphone;
		protected System.Web.UI.WebControls.Label lblWeChat;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.Label lblReferralReason;
		protected System.Web.UI.WebControls.TextBox txtRefusalReason;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidUserId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefusalReason;
		protected System.Web.UI.WebControls.Button btnAccept;
		protected System.Web.UI.WebControls.Button btnRefuse;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			this.btnRefuse.Click += new System.EventHandler(this.btnRefuse_Click);
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
				this.txtUserName.Text = this.searchKey;
				this.txtRealName.Text = this.realName;
				this.txtCellphnoe.Text = this.cellphone;
				return;
			}
			this.searchKey = this.txtUserName.Text;
			this.realName = this.txtRealName.Text;
			this.cellphone = this.txtCellphnoe.Text;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtUserName.Text);
			nameValueCollection.Add("realName", this.txtRealName.Text);
			nameValueCollection.Add("cellphone", this.txtCellphnoe.Text);
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
		protected void btnAccept_Click(object sender, System.EventArgs e)
		{
			int userId = 0;
			int.TryParse(this.hidUserId.Value, out userId);
			if (MemberHelper.AccepteRerralRequest(userId))
			{
				this.BindReferralRequest();
				this.ShowMsg("推广员的申请已经审核通过", true);
				return;
			}
			this.ShowMsg("审核通过失败", true);
		}
		private void btnRefuse_Click(object sender, System.EventArgs e)
		{
			int userId = 0;
			int.TryParse(this.hidUserId.Value, out userId);
			if (MemberHelper.RefuseRerralRequest(userId, this.hidRefusalReason.Value))
			{
				this.BindReferralRequest();
				this.ShowMsg("拒绝了推广员的申请", true);
				return;
			}
			this.ShowMsg("拒绝通过失败", true);
		}
		public void BindReferralRequest()
		{
			DbQueryResult members = MemberHelper.GetMembers(new MemberQuery
			{
				Username = this.txtUserName.Text,
				Realname = this.txtRealName.Text,
				CellPhone = this.txtCellphnoe.Text,
				ReferralStatus = new int?(1),
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			});
			this.grdReferralRequest.DataSource = members.Data;
			this.grdReferralRequest.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
			this.pager.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
		}
	}
}
