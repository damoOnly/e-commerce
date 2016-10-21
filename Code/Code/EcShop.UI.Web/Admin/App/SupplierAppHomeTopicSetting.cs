using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.App
{
    [PrivilegeCheck(Privilege.SupplierAppHomeTopicSetting)]
    public class SupplierAppHomeTopicSetting : AdminPage
    {
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdtopic;
        protected ImageLinkButton btnDeleteAll;
        protected System.Web.UI.WebControls.LinkButton btnFinish;
        protected Grid grdHomeTopics;

        private int supplierId;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //根据登录用户获取供应商id
            supplierId = CheckSupplierRole();
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            this.grdHomeTopics.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdHomeProducts_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindHomeProducts();
            }
        }
        private void grdHomeProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            if (VShopHelper.RemoveHomeTopic((int)this.grdHomeTopics.DataKeys[e.RowIndex].Value, ClientType.App, supplierId))
            {
                base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
            }
        }
        private void BindHomeProducts()
        {
            this.grdHomeTopics.DataSource = VShopHelper.GetHomeTopics(ClientType.App, supplierId);
            this.grdHomeTopics.DataBind();
        }
        private void btnDeleteAll_Click(object sender, System.EventArgs e)
        {
            if (VShopHelper.RemoveAllHomeTopics(ClientType.App, supplierId))
            {
                base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
            }
        }
        protected void btnFinish_Click(object sender, System.EventArgs e)
        {
            foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdHomeTopics.Rows)
            {
                int displaysequence = 0;
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
                if (int.TryParse(textBox.Text.Trim(), out displaysequence))
                {
                    int topicId = System.Convert.ToInt32(this.grdHomeTopics.DataKeys[gridViewRow.DataItemIndex].Value);
                    VShopHelper.UpdateHomeTopicSequence(topicId, displaysequence, ClientType.App);
                }
            }
            this.BindHomeProducts();
        }
    }
}
