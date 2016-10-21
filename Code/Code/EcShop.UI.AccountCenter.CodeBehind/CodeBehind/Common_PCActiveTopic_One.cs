using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EcShop.Entities;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Sales;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class Common_PCActiveTopic_One : AscxTemplatedWebControl
    {
        public const string TagID = "Common_PCActiveTopic_One";
        private System.Web.UI.WebControls.Repeater rptsuppliercouponlist;
        private int supplierId = 0;
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
        public Common_PCActiveTopic_One()
        {
            base.ID = "Common_PCActiveTopic_One";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Common_PCActiveTopic_One.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["supplierId"], out this.supplierId);
            this.rptsuppliercouponlist = (System.Web.UI.WebControls.Repeater)this.FindControl("rptsuppliercouponlist");
            BindList();
        }

        private void BindList()
        {
            if (this.rptsuppliercouponlist != null)
            {
                rptsuppliercouponlist.DataSource = ActiveHelper.GetThisTopicList();
                rptsuppliercouponlist.DataBind();
            }
        }

    }
}
