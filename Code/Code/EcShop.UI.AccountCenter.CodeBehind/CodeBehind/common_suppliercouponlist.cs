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
namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class Common_SupplierCouponList : AscxTemplatedWebControl
    {
        public const string TagID = "Common_SupplierCouponList";
        private System.Web.UI.WebControls.Repeater rptsuppliercouponlist;
        private int supplierId = 0;
        private System.Web.UI.HtmlControls.HtmlControl div;
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
        public Common_SupplierCouponList()
        {
            base.ID = "Common_SupplierCouponList";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_SupplierCouponList.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["supplierId"], out this.supplierId);
            this.rptsuppliercouponlist = (System.Web.UI.WebControls.Repeater)this.FindControl("rptsuppliercouponlist");
            this.div = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("showcoupon");
            BindList();
        }

        private void BindList()
        {
            if (this.rptsuppliercouponlist != null)
            {
                DataTable dt = CouponHelper.GetSupplierCoupon(4, 1, supplierId);
                rptsuppliercouponlist.DataSource = dt;
                rptsuppliercouponlist.DataBind();
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.div.Style.Add("display", "block");
                }
                else
                {
                    this.div.Style.Add("display", "none");
                }
            }
        }

    }
}
