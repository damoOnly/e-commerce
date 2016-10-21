using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_CustomerService : AscxTemplatedWebControl
    {
        private Repeater rptCustomerService;
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/skin-Common_CustomerService.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.rptCustomerService = (Repeater)this.FindControl("rptCustomerService");
            this.BindData();
        }
        private void BindData()
        {
           
        }
    }
}
