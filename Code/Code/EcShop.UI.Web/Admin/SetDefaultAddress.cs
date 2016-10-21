using ASPNET.WebControls;
using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Plugins;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using EcShop.SaleSystem.Member;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SetDefaultAddress)]
    public partial class SetDefaultAddress : AdminPage
    {
        private string searchKey;
        private string realName;
        private string cellPhone;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.TextBox txtRealName;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;  
        protected Pager pager;
        protected Grid grdAddressList;
        protected Pager pager1;

        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdAddressList.ReBindData += new Grid.ReBindDataEventHandler(this.grdAddressList_ReBindData);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdAddressList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdAddress_RowCommand);
            this.grdAddressList.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdAddress_RowDataBound);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cellPhone"]))
                {
                    this.cellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["cellPhone"]);
                }
                this.txtSearchText.Text = this.searchKey;
                this.txtRealName.Text = this.realName;
                this.txtCellPhone.Text = this.cellPhone;
                return;
            }
            this.searchKey = this.txtSearchText.Text;
            this.realName = this.txtRealName.Text.Trim();
        }
        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("searchKey", this.txtSearchText.Text);
            nameValueCollection.Add("realName", this.txtRealName.Text);
            nameValueCollection.Add("cellPhone", this.txtCellPhone.Text);
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(nameValueCollection);
        }
        protected void BindData()
        {
            this.LoadParameters();
            MemberQuery memberQuery = new MemberQuery();
            memberQuery.Username = this.searchKey;
            memberQuery.Realname = this.realName;
            memberQuery.CellPhone = this.cellPhone;
            memberQuery.PageIndex = this.pager.PageIndex;
            memberQuery.SortBy = this.grdAddressList.SortOrderBy;
            memberQuery.PageSize = this.pager.PageSize;
            if (this.grdAddressList.SortOrder.ToLower() == "desc")
            {
                memberQuery.SortOrder = SortAction.Desc;
            }
            DbQueryResult members = MemberHelper.GetMembersAddress(memberQuery);
            this.grdAddressList.DataSource = members.Data;
            this.grdAddressList.DataBind();
            this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
        }
        private void grdAddressList_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void grdAddress_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
            int shippingId = (int)this.grdAddressList.DataKeys[rowIndex].Value;
            int userId;
            int.TryParse(e.CommandArgument.ToString(),out userId);
            if (e.CommandName == "SetDefaultAddress")
            {
                bool result = MemberProcessor.SetDefaultShippingAddressPC(shippingId, userId);
                if (result)
                {
                    this.BindData();
                    this.ShowMsg("成功的设置该地址为默认地址", true);
                    return;
                }
                this.ShowMsg("设置该订单为默认地址失败", false);
            }
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }

        private void grdAddress_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {

                ImageLinkButton lkbtnSetDefaultAddress = (ImageLinkButton)e.Row.FindControl("lkbtnSetDefaultAddress");
                bool isDefault = bool.Parse((System.Web.UI.DataBinder.Eval(e.Row.DataItem, "IsDefault").ToString()));
                if (!isDefault)
                {
                    lkbtnSetDefaultAddress.Visible = true;
                }
            }
        }
    }
}
