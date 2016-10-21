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
    public class Common_ActiveTopeTwo : AscxTemplatedWebControl
    {
        public const string TagID = "Common_ActiveTopeTwo";
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
        public Common_ActiveTopeTwo()
        {
            base.ID = "Common_ActiveTopeTwo";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Common_PCActiveTopic_Two.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
           
            this.rptsuppliercouponlist = (System.Web.UI.WebControls.Repeater)this.FindControl("rptsuppliercouponlist");
            BindList();
        }

        private void BindList()
        {
            if (this.rptsuppliercouponlist != null)
            {
                rptsuppliercouponlist.DataSource = ActiveHelper.GetThisTopicList_Two();
                rptsuppliercouponlist.DataBind();
            }
        }

    }
}
