using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VActivity3 : VshopTemplatedWebControl
    {
        private string topicId;
        private VshopTemplatedRepeater rptActivity3;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VActivity3.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            if (string.IsNullOrWhiteSpace(this.Page.Request.QueryString["TopicId"]))
            {
                base.GotoResourceNotFound("");
            }
            else
            {
                this.topicId = this.Page.Request.QueryString["TopicId"]; 
            }
            this.rptActivity3 = (VshopTemplatedRepeater)this.FindControl("rptActivity3");
            DataTable topics = VshopBrowser.GetHomeActivityTopics(ClientType.VShop, topicId);
            this.rptActivity3.DataSource = topics;
            this.rptActivity3.DataBind();
            PageTitle.AddSiteNameTitle("×¨ÌâÀ¸");
        }
    }
}
